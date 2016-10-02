using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using BitmapCompressor.Utilities;
using BitmapCompressor.Extensions;
using BitmapCompressor.Formats;

namespace BitmapCompressor.DataTypes
{
    /// <summary>
    /// Represents an uncompressed BMP image.
    /// </summary>
    public class BMPImage : IProcessedImage
    {
        private readonly Bitmap _bitmap;

        public BMPImage(Bitmap bitmap)
        {
            _bitmap = bitmap;
        }

        public BMPImage(int width, int height) : this(new Bitmap(width, height))
        { }

        public int Height => _bitmap.Height;

        public int Width => _bitmap.Width;

        public Bitmap GetBitmap()
        {
            return _bitmap;
        }

        /// <summary>
        /// Reads the 16 ARGB (32-bit) colors from the specified 4x4 pixel block of the source image.
        /// </summary>
        public Color[] GetColors(Point blockPosition)
        {
            var colors = new Color[BlockFormat.PixelCount];

            var firstPixel = new Point(
                blockPosition.X * BlockFormat.Dimension,
                blockPosition.Y * BlockFormat.Dimension);

            var lastPixel = new Point(
                firstPixel.X + BlockFormat.Dimension,
                firstPixel.Y + BlockFormat.Dimension);

            for (int y = firstPixel.Y; y < lastPixel.Y; ++y)
            {
                for (int x = firstPixel.X; x < lastPixel.X; ++x)
                {
                    var pixel = new Point(x, y);
                    var local = pixel.Subtract(firstPixel);
                    int index = PointUtility.ToRowMajor(local, BlockFormat.Dimension);

                    var color = _bitmap.GetPixel(x, y);
                    colors[index] = color;
                }
            }

            return colors;
        }
        
        /// <summary>
        /// Sets the 16 ARGB (32-bit) colors at the specified 4x4 pixel block to the source image.
        /// </summary>
        public void SetColors(Color[] colors, Point blockPosition)
        {
            var firstPixel = new Point(
                blockPosition.X * BlockFormat.Dimension,
                blockPosition.Y * BlockFormat.Dimension);

            for (int i = 0; i < BlockFormat.PixelCount; ++i)
            {
                var pixel = PointUtility.FromRowMajor(i, BlockFormat.Dimension);
                var pixelWithOffset = firstPixel.Add(pixel);

                _bitmap.SetPixel(pixelWithOffset.X, pixelWithOffset.Y, colors[i]);
            }
        }

        public void Save(string fileName)
        {
            var bmp = ToArgb(_bitmap);

            bmp.Save(fileName);
        }

        public static BMPImage Load(string fileName)
        {
            var bmp = ToArgb(Image.FromFile(fileName) as Bitmap);

            return new BMPImage(bmp);
        }

        /// <summary>
        /// Converts a bitmap into a new bitmap with an alpha channel if the original
        /// bitmap has a 32-bit pixel format.
        /// </summary>
        /// <remarks><para>
        /// .NET does not have support for alpha transparency in bitmaps. 
        /// See note in MSDN: https://msdn.microsoft.com/en-us/library/4sahykhd.aspx
        /// </para><para>
        /// Luckily we can overcome this issue by creating a new bitmap with an alpha channel and
        /// copying the RGB values of the original bitmap using <see cref="BitmapData"/> objects.
        /// </para></remarks>
        private static Bitmap ToArgb(Bitmap bmp)
        {
            var is32Bit = Image.GetPixelFormatSize(bmp.PixelFormat) == 32;
            if (!is32Bit)
            {
                return bmp;
            }

            var size = new Rectangle(0, 0, bmp.Width, bmp.Height);

            // Copy the RGB values of the original bitmap into an array
            var bmpData = bmp.LockBits(size, ImageLockMode.WriteOnly, bmp.PixelFormat);
            var rgbValues = new byte[bmpData.Stride * bmp.Height];

            Marshal.Copy(bmpData.Scan0, rgbValues, 0, bmpData.Stride * bmp.Height);  // Scan0 is the address of the
                                                                                     // first pixel data in the bitmap
            bmp.UnlockBits(bmpData);                                                 

            // Allocate the destination bitmap in ARGB format and copy the RGB values
            // of the original bitmap back to the bitmap with alpha channel
            var bmpArgb = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format32bppArgb);
            var bmpArgbData = bmpArgb.LockBits(size, ImageLockMode.WriteOnly, bmpArgb.PixelFormat);

            Marshal.Copy(rgbValues, 0, bmpArgbData.Scan0, bmpArgbData.Stride * bmpArgb.Height);

            bmpArgb.UnlockBits(bmpArgbData);

            return bmpArgb;
        }
    }
}
