using System.Drawing;
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

        public int Height => _bitmap.Height;

        public int Width => _bitmap.Width;

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

                    colors[index] = _bitmap.GetPixel(x, y);
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
            _bitmap.Save(fileName);
        }

        public static BMPImage Load(string fileName)
        {
            return new BMPImage(new Bitmap(fileName));
        }
    }
}
