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

        public BMPImage LoadBitmap(string filePath)
        {
            return BMPImage.Load(filePath);
        }

        public DDSImage LoadDDS(string filePath)
        {
            return DDSImage.Load(filePath);
        }
    }
}
