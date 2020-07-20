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
    public class MainViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Design Time Instance
        private static Lazy<MainViewModel> _lazyDesignTimeInstance = new Lazy<MainViewModel>(() => new MainViewModel());
        public static MainViewModel DesignTimeInstance => _lazyDesignTimeInstance.Value;
        #endregion

        //public ObservableCollection<PdfPageViewModel> PdfPages { get; set; } = new ObservableCollection<PdfPageViewModel>();

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
    }
}
