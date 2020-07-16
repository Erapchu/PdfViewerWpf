using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PdfViewerWpf
{
    public static class ImageExtensions
    {
        public static ImageSource GetImageWpf(this Image image)
        {
            if (image is null)
                return null;

            var bitmap = new Bitmap(image);
            var wpfImage = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
            return wpfImage;
        }
    }
}
