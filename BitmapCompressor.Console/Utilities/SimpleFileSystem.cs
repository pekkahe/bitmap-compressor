using BitmapCompressor.Lib.DataTypes;

namespace BitmapCompressor.Console.Utilities;

public class SimpleFileSystem : IFileSystem
{
    public bool Exists(string fileName)
    {
        return File.Exists(fileName);
    }

    public IUncompressedImage LoadBitmap(string filePath)
    {
        return DirectBitmap.CreateFromFile(filePath);
    }

    public ICompressedImage LoadDDS(string filePath)
    {
        return DDSImage.CreateFromFile(filePath);
    }
}
