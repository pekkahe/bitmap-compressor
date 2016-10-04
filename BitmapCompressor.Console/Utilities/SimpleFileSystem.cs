using System;
using System.IO;
using BitmapCompressor.DataTypes;

namespace BitmapCompressor.Console.Utilities
{
    public class SimpleFileSystem : IFileSystem
    {
        public bool Exists(string fileName)
        {
            return File.Exists(fileName);
        }

        public IUncompressedImage LoadBitmap(string filePath)
        {
            return DirectBitmap.FromFile(filePath);
        }

        public ICompressedImage LoadDDS(string filePath)
        {
            return DDSImage.FromFile(filePath);
        }
    }
}
