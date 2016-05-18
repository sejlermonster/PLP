using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Graphikos.Utility
{
    public interface IBitmapDrawing
    {
        BitmapSource DrawPixels(IReadOnlyCollection<object> listOfCoordinates, Color drawColor, Bitmap bitmap);
        BitmapSource DrawText(IReadOnlyCollection<object> listOfCoordinates, Color color, Bitmap bitmap);
    }
}
