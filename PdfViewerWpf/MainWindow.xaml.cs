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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.OpenFileDialog())
            {
                dialog.Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*";
                dialog.Title = "Open PDF File";

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //_pdfViewer.Document = PdfDocument.Load(dialog.FileName);

                    ViewModel.PdfPages.Clear();
                    using (var pdfDocument = PdfDocument.Load(dialog.FileName))
                    {
                        for (int i = 0; i < pdfDocument.PageCount; i++)
                        {
                            var gdi = pdfDocument.Render(i, 96, 96, false);
                            var wpfImage = gdi.GetImageWpf();
                            ViewModel.PdfPages.Add(new PdfPageViewModel(wpfImage));
                            GC.Collect();
                        }
                    }
                }
            }
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
