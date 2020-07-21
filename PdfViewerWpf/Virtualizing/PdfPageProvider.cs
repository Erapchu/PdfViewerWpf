using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PdfViewerWpf.Virtualizing
{
    public class PdfPageProvider : IItemsProvider<ImageSource>
    {
        private readonly int _count;
        public int FetchCount()
        {
            return _count;
        }

        public IList<ImageSource> FetchRange(int startIndex, int count)
        {
            throw new NotImplementedException();
        }
    }
}
