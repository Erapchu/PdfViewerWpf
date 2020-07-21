using PdfiumViewer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PdfViewerWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private PdfDocument _pdfDocument;

        private MainViewModel ViewModel => DataContext as MainViewModel;

        private PdfViewer _pdfViewer;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            /*// Create the interop host control.
            System.Windows.Forms.Integration.WindowsFormsHost host = new System.Windows.Forms.Integration.WindowsFormsHost();

            _pdfViewer = new PdfViewer();
            _pdfViewer.ShowToolbar = false;
            _pdfViewer.ShowBookmarks = false;
            host.Child = _pdfViewer;
            // Add the interop host control to the Grid
            // control's collection of child controls.
            this.grid1.Children.Add(host);*/
        }
    }
}
