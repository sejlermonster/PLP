using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Graphikos.Utility
{
    public class BitmapDrawing : IBitmapDrawing
    {
        public BitmapSource DrawPixels(IReadOnlyCollection<object> listOfCoordinates, Color drawColor, Bitmap bitmap)
        {
            for (var i = 0; i + 3 < listOfCoordinates.Count; i++)
            {
                bitmap.SetPixel((int)listOfCoordinates.ElementAt(i),
                                bitmap.Height - (int)listOfCoordinates.ElementAt(i + 1),
                                drawColor);
                i++;
            }
            return BitmapToBitmapSource(bitmap);
        }

        public BitmapSource DrawText(IReadOnlyCollection<object> listOfCoordinates, Color color, Bitmap bitmap)
        {
            if (listOfCoordinates.Count < 3)
                return BitmapToBitmapSource(bitmap);

            using (var g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                var format = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                var drawPoint = new PointF(Convert.ToInt32(listOfCoordinates.ElementAt(0)), bitmap.Height - Convert.ToInt32(listOfCoordinates.ElementAt(1)));
                g.DrawString(listOfCoordinates.Last().ToString(), new Font("Tahoma", 12), new SolidBrush(color), drawPoint, format);
            }
            return BitmapToBitmapSource(bitmap);
        }

        private static BitmapSource BitmapToBitmapSource(Bitmap source)
        {
            return Imaging.CreateBitmapSourceFromHBitmap(
                                                         source.GetHbitmap(),
                                                         IntPtr.Zero,
                                                         Int32Rect.Empty,
                                                         BitmapSizeOptions.FromEmptyOptions());
        }
    }
}
