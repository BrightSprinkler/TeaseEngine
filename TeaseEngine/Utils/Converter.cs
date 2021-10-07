using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace TeaseEngine.Utils
{
    public class Converter
    {
        public BitmapSource ConvertToBitmapSource(Bitmap bitmap)
        {
            BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap
             (
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions()
              );

            return bitmapSource;
        }

        public Bitmap CounvertToBitmap(BitmapSource source)
        {
            Bitmap bmp = new Bitmap
            (
              source.PixelWidth,
              source.PixelHeight,
              PixelFormat.Format32bppPArgb
            );

            BitmapData data = bmp.LockBits
            (
                new Rectangle(System.Drawing.Point.Empty, bmp.Size),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppPArgb
            );

            source.CopyPixels
            (
              Int32Rect.Empty,
              data.Scan0,
              data.Height * data.Stride,
              data.Stride
            );

            bmp.UnlockBits(data);

            return bmp;
        }

        public BitmapSource RenderTargetBitmapToBitmapSource(RenderTargetBitmap bitmap)
        {
            BitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            using MemoryStream outStream = new MemoryStream();
            encoder.Save(outStream);

            outStream.Seek(0, SeekOrigin.Begin);
            BitmapImage bmp = new BitmapImage { CacheOption = BitmapCacheOption.OnLoad };
            bmp.BeginInit();
            bmp.StreamSource = outStream;
            bmp.EndInit();

            return ConvertToBitmapSource(BitmapImage2Bitmap(bmp));
        }

        private Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {

            using MemoryStream outStream = new MemoryStream();
            BitmapEncoder enc = new BmpBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create(bitmapImage));
            enc.Save(outStream);
            Bitmap bitmap = new Bitmap(outStream);

            return new Bitmap(bitmap);
        }

    }
}
