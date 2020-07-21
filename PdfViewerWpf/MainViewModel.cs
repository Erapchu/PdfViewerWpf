using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PdfiumViewer;
using PdfViewerWpf.Virtualizing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PdfViewerWpf
{
    public class MainViewModel : ViewModelBase
    {
        #region Design Time Instance
        private static Lazy<MainViewModel> _lazyDesignTimeInstance = new Lazy<MainViewModel>(() => new MainViewModel());
        public static MainViewModel DesignTimeInstance => _lazyDesignTimeInstance.Value;
        #endregion

        public ObservableCollection<PdfPageViewModel> PdfPages { get; set; } = new ObservableCollection<PdfPageViewModel>();

        private RelayCommand _loadPdfCommand;
        public RelayCommand LoadPdfCommand => _loadPdfCommand ?? (_loadPdfCommand = new RelayCommand(LoadPdf));

        private RelayCommand _searchCommand;
        public RelayCommand SearchCommand => _searchCommand ?? (_searchCommand = new RelayCommand(StartSearch));

        private ImageSource _visiblePdfPage;
        public ImageSource VisiblePdfPage
        {
            get => _visiblePdfPage;
            set
            {
                _visiblePdfPage = value;
                RaisePropertyChanged();
            }
        }

        public PdfDocument LoadedDocument { get; set; }

        public int CurrentPage { get; set; }

        private string _searchableText;
        public string SearchableText
        {
            get => _searchableText;
            set
            {
                _searchableText = value;
                RaisePropertyChanged();
            }
        }

        public event Action<List<HighlightingRequest>> DoHighlight;

        public MainViewModel()
        {

        }

        private void StartSearch()
        {
            var words = SearchableText.Split();
            List<PdfMatches> allMatches = new List<PdfMatches>();
            foreach(var word in words)
            {
                var wordMatches = LoadedDocument?.Search(word, false, false);
                allMatches.Add(wordMatches);
            }
            var allBounds = GetAllBounds(allMatches).SelectMany(m => m);

            List<HighlightingRequest> highlightings = new List<HighlightingRequest>();

            foreach (var bound in allBounds)
            {
                var convertedRecatngle = LoadedDocument.RectangleFromPdf(0, new System.Drawing.RectangleF(bound.Bounds.Location, bound.Bounds.Size));
                highlightings.Add(new HighlightingRequest() { Page = bound.Page, Rectangle = convertedRecatngle });
            }

            DoHighlight?.Invoke(highlightings);
        }

        private List<IList<PdfRectangle>> GetAllBounds(IEnumerable<PdfMatches> pdfMatches)
        {
            var result = new List<IList<PdfRectangle>>();

            foreach (var match in pdfMatches.SelectMany(p => p.Items))
            {
                result.Add(LoadedDocument.GetTextBounds(match.TextSpan));
            }

            return result;
        }

        public void GoNextPage()
        {
            if (CurrentPage < LoadedDocument.PageCount - 1)
            {
                CurrentPage++;
                VisiblePdfPage = LoadedDocument.Render(CurrentPage, 96, 96, false).GetImageWpf();
                GC.Collect();
            }
        }

        public void GoPrevPage()
        {
            if (CurrentPage > 0)
            {
                CurrentPage--;
                VisiblePdfPage = LoadedDocument.Render(CurrentPage, 96, 96, false).GetImageWpf();
                GC.Collect();
            }
        }

        private void LoadPdf()
        {
            using (var dialog = new System.Windows.Forms.OpenFileDialog())
            {
                dialog.Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*";
                dialog.Title = "Open PDF File";

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    LoadedDocument?.Dispose();
                    LoadedDocument = PdfDocument.Load(dialog.FileName);
                    CurrentPage = 0;
                    VisiblePdfPage = LoadedDocument.Render(CurrentPage, 96, 96, false).GetImageWpf();

                    /*PdfPages.Clear();
                    using (var pdfDocument = PdfDocument.Load(dialog.FileName))
                    {
                        for (int i = 0; i < pdfDocument.PageCount; i++)
                        {
                            using (var gdi = pdfDocument.Render(i, 96, 96, false))
                            {
                                var wpfImage = gdi.GetImageWpf();
                                PdfPages.Add(new PdfPageViewModel(wpfImage));
                            }
                            GC.Collect();
                        }
                    }*/
                }
            }
        }
    }
}
