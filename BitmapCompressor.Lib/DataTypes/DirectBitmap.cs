using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using BitmapCompressor.Lib.Formats;
using BitmapCompressor.Lib.Utilities;

namespace BitmapCompressor.Lib.DataTypes;

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
            Bitmap.Dispose();

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
    
    public Color[] GetBlockColors(Point blockIndex)
    {
        var firstPixel = new Point(
            blockIndex.X * BlockFormat.Dimension,
            blockIndex.Y * BlockFormat.Dimension);

        var lastPixel = new Point(
            firstPixel.X + BlockFormat.Dimension,
            firstPixel.Y + BlockFormat.Dimension);

        var colors = new Color[BlockFormat.TexelCount];
        int colorIndex = 0;

        for (int y = firstPixel.Y; y < lastPixel.Y; ++y)
        {
            for (int x = firstPixel.X; x < lastPixel.X; ++x)
            {
                var pixel = new Point(x, y);
                int pixelIndex = PointUtility.ToRowMajor(pixel, Width) * 4;

                // Bytes are stored in inverted (BGRA) order
                byte a = _data[pixelIndex + 3];
                byte r = _data[pixelIndex + 2];
                byte g = _data[pixelIndex + 1];
                byte b = _data[pixelIndex];

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

    public void SetBlockColors(Point blockIndex, Color[] colors)
    {
        var firstPixel = new Point(
            blockIndex.X * BlockFormat.Dimension,
            blockIndex.Y * BlockFormat.Dimension);

        var lastPixel = new Point(
            firstPixel.X + BlockFormat.Dimension,
            firstPixel.Y + BlockFormat.Dimension);

        int colorIndex = 0;

        for (int y = firstPixel.Y; y < lastPixel.Y; ++y)
        {
            for (int x = firstPixel.X; x < lastPixel.X; ++x)
            {
                var pixel = new Point(x, y);
                int pixelIndex = PointUtility.ToRowMajor(pixel, Width) * 4;

                var color = colors[colorIndex++];

                // Bytes are stored in inverted (BGRA) order
                _data[pixelIndex + 3] = color.A;
                _data[pixelIndex + 2] = color.R;
                _data[pixelIndex + 1] = color.G;
                _data[pixelIndex] = color.B;
            }
        }
    }

    public void Save(string fileName)
    {
        Bitmap.Save(fileName);
    }

    public static DirectBitmap CreateFromFile(string fileName)
    {
        using var bitmap = (Bitmap)Image.FromFile(fileName);
        return CreateFromBitmap(bitmap);
    }

    public static DirectBitmap CreateFromBitmap(Bitmap sourceBitmap)
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
}
