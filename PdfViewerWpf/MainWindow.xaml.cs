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

        private List<Rectangle> _highlightedRects = new List<Rectangle>();
        private Color[] _colors = new Color[] 
        { 
            Color.FromRgb(0xFF, 0x00, 0x00),
            Color.FromRgb(0x00, 0xFF, 0x00),
            Color.FromRgb(0x00, 0x00, 0xFF)
        };

        public MainWindow()
        {
            InitializeComponent();
            var vm = new MainViewModel();
            vm.DoHighlight += ViewModel_DoHighlight;
            DataContext = vm;
        }

        private void ViewModel_DoHighlight(List<HighlightingRequest> highlightingRequest)
        {
            foreach (var highlightedRect in _highlightedRects)
                rootCanvas.Children.Remove(highlightedRect);

            int colorNumber = 0;

            foreach (var highlight in highlightingRequest)
            {
                Rectangle rectangle = new Rectangle
                {
                    Stroke = new SolidColorBrush(_colors[colorNumber]),
                    StrokeThickness = 2,
                    Fill = new SolidColorBrush(_colors[colorNumber]),
                    Opacity = 0.5,
                    Width = highlight.Rectangle.Width,
                    Height = highlight.Rectangle.Height
                };
                rootCanvas.Children.Add(rectangle);
                Canvas.SetLeft(rectangle, highlight.Rectangle.X);
                Canvas.SetTop(rectangle, highlight.Rectangle.Y);
                _highlightedRects.Add(rectangle);
                if (colorNumber < _colors.Length - 1)
                    colorNumber++;
                else
                    colorNumber = 0;
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

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
                ViewModel.GoPrevPage();
            else
                ViewModel.GoNextPage();
        }
    }
}
