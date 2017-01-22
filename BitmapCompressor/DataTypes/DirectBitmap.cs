using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using BitmapCompressor.Formats;
using BitmapCompressor.Utilities;

namespace BitmapCompressor.DataTypes
{
    /// <summary>
    /// A bitmap class which provides direct and fast access to bitmap pixel data.
    /// </summary>
    public sealed class DirectBitmap : IUncompressedImage, IDisposable
    {
        private readonly byte[] _data;
        private readonly GCHandle _dataHandle;

        private bool _disposed;

        /// <summary>
        /// Instantiates an empty bitmap image with the specified dimensions.
        /// </summary>
        public DirectBitmap(int width, int height)
        {
            _data = new byte[width * height * 4];
            _dataHandle = GCHandle.Alloc(_data, GCHandleType.Pinned);

            Width = width;
            Height = height;
            Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppArgb, _dataHandle.AddrOfPinnedObject());
        }

        ~DirectBitmap()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                Bitmap.Dispose();
            }

            _dataHandle.Free();
            _disposed = true;
        }

        public int Width { get; }

        public int Height { get; }

        public Bitmap Bitmap { get; }

        public byte[] GetBuffer()
        {
            return _data;
        }
        
        public Color[] GetBlockPixels(Point block)
        {
            var firstPixel = new Point(
                block.X * BlockFormat.Dimension,
                block.Y * BlockFormat.Dimension);

            var lastPixel = new Point(
                firstPixel.X + BlockFormat.Dimension,
                firstPixel.Y + BlockFormat.Dimension);

            var colors = new Color[BlockFormat.PixelCount];
            int colorIndex = 0;

            for (int y = firstPixel.Y; y < lastPixel.Y; ++y)
            {
                for (int x = firstPixel.X; x < lastPixel.X; ++x)
                {
                    var pixel = new Point(x, y);

                    int index = PointUtility.ToRowMajor(pixel, Width) * 4;

                    // Bytes are stored in inverted (BGRA) order
                    byte a = _data[index + 3];
                    byte r = _data[index + 2];
                    byte g = _data[index + 1];
                    byte b = _data[index];

                    var values = new byte[] { a, r, g, b };

                    // BitConverter operates in LE order on LE machines. To preserve
                    // the original order of our data, reverse the byte array prior 
                    // converting to 64-bit unsigned integer.
                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(values);

                    int argb = BitConverter.ToInt32(values, 0);

                    colors[colorIndex++] = Color.FromArgb(argb);
                }
            }

            return colors;
        }

        public void SetBlockPixels(Point block, Color[] colors)
        {
            var firstPixel = new Point(
                block.X * BlockFormat.Dimension,
                block.Y * BlockFormat.Dimension);

            var lastPixel = new Point(
                firstPixel.X + BlockFormat.Dimension,
                firstPixel.Y + BlockFormat.Dimension);

            int colorIndex = 0;

            for (int y = firstPixel.Y; y < lastPixel.Y; ++y)
            {
                for (int x = firstPixel.X; x < lastPixel.X; ++x)
                {
                    var pixel = new Point(x, y);

                    int index = PointUtility.ToRowMajor(pixel, Width) * 4;

                    var color = colors[colorIndex++];

                    // Bytes are stored in inverted (BGRA) order
                    _data[index + 3] = color.A;
                    _data[index + 2] = color.R;
                    _data[index + 1] = color.G;
                    _data[index] = color.B;
                }
            }
        }

        public static DirectBitmap FromFile(string fileName)
        {
            using (var bitmap = (Bitmap) Image.FromFile(fileName))
            {
                return FromBitmap(bitmap);
            }
        }

        public static DirectBitmap FromBitmap(Bitmap sourceBitmap)
        {
            var direct = new DirectBitmap(sourceBitmap.Width, sourceBitmap.Height);

            CopyPixelData(sourceBitmap, direct.Bitmap);

            return direct;
        }

        private static void CopyPixelData(Bitmap from, Bitmap to)
        {
            var size = new Rectangle(0, 0, from.Width, from.Height);
            
            var fromData = from.LockBits(size, ImageLockMode.ReadOnly, from.PixelFormat);
            var fromValues = new byte[fromData.Stride * size.Height];

            try
            {
                Marshal.Copy(fromData.Scan0, fromValues, 0, fromData.Stride * size.Height);
            }
            finally
            {
                from.UnlockBits(fromData);
            }

            var toData = to.LockBits(size, ImageLockMode.WriteOnly, to.PixelFormat);

            try
            {
                Marshal.Copy(fromValues, 0, toData.Scan0, toData.Stride * size.Height);
            }
            finally
            {
                to.UnlockBits(toData);
            }
        }

        public void Save(string fileName)
        {
            Bitmap.Save(fileName);
        }
    }
}
