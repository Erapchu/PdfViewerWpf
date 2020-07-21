using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PdfiumViewer;
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

        //public ObservableCollection<PdfPageViewModel> PdfPages { get; set; } = new ObservableCollection<PdfPageViewModel>();

        private RelayCommand _loadPdfCommand;
        public RelayCommand LoadPdfCommand => _loadPdfCommand ?? (_loadPdfCommand = new RelayCommand(LoadPdf));

        private RelayCommand _nextPageCommand;
        public RelayCommand NextPageCommand => _nextPageCommand ?? (_nextPageCommand = new RelayCommand(GoNextPage));

        private RelayCommand _prevPageCommand;
        public RelayCommand PrevPageCommand => _prevPageCommand ?? (_prevPageCommand = new RelayCommand(GoPrevPage));

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

        public MainViewModel()
        {

        }

        private void GoNextPage()
        {
            CurrentPage++;
            VisiblePdfPage = LoadedDocument.Render(CurrentPage, 96, 96, false).GetImageWpf();
            GC.Collect();
        }

        private void GoPrevPage()
        {
            CurrentPage--;
            VisiblePdfPage = LoadedDocument.Render(CurrentPage, 96, 96, false).GetImageWpf();
            GC.Collect();
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

                    /*ViewModel.PdfPages.Clear();
                    using (var pdfDocument = PdfDocument.Load(dialog.FileName))
                    {
                        for (int i = 0; i < pdfDocument.PageCount; i++)
                        {
                            using (var gdi = pdfDocument.Render(i, 96, 96, false))
                            {
                                var wpfImage = gdi.GetImageWpf();
                                ViewModel.PdfPages.Add(new PdfPageViewModel(wpfImage));
                            }
                            GC.Collect();
                        }
                    }*/
                }
            }
        }
    }
}
