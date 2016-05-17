using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace GraphikosTests
{
    public static class Ext
    {
        private static readonly string FilePath = "../../BitmapsForTesting/";

        public static byte[] GetBytes(this BitmapSource bitmap)
        {
            var encoder = new JpegBitmapEncoder { QualityLevel = 100 };
            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                encoder.Save(stream);
                bytes = stream.ToArray();
                stream.Close();
            }
            return bytes;
        }

        public static void StoreBitMapSource(this BitmapSource bitmap, string fileName)
        {
            var data = bitmap.GetBytes();
            using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                fileStream.Write(data, 0, data.Length);
                fileStream.Close();
            }
        }

        public static byte[] LoadBytes(string fileName)
        {
            return File.ReadAllBytes(FilePath + fileName);
        }

        public static BitmapSource LoadBitMapSource(string fileName)
        {
            return new BitmapImage(new Uri(FilePath + fileName, UriKind.Relative));
        }
    }
}
