using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using BitmapCompressor.DataTypes;
using BitmapCompressor.Serialization.FileFormat;

namespace BitmapCompressor.Serialization
{
    public class DDSFileReader : IDisposable
    {
        private readonly BinaryReader _binaryReader;

        public DDSFileReader(Stream stream)
        {
            _binaryReader = new BinaryReader(stream);
        }

        public DDSFileReader(string fileName) : this(new FileStream(fileName, FileMode.Open))
        { }

        /// <summary>
        /// Reads the DDS file of this reader and returns a handle to its main image data.
        /// </summary>
        public unsafe DDSImage Read()
        {
            ReadMagicNumber();

            var size = ReadHeader();

            var buffer = ReadMainImage(size);

            return new DDSImage(size.Width, size.Height, buffer);
        }

        private void ReadMagicNumber()
        { 
            int size = Marshal.SizeOf(DDSFile.MagicNumber);
            var buffer = _binaryReader.ReadBytes(size);

            var magicNumber = BitConverter.ToUInt32(buffer, 0);
            if (magicNumber != DDSFile.MagicNumber)
                throw new InvalidOperationException("Unrecognizable DDS file format.");
        }

        private Size ReadHeader()
        {
            int size = Marshal.SizeOf(typeof(DDSFileHeader));
            var buffer = _binaryReader.ReadBytes(size);

            // Read the header data directly into a DDS file header structure
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            var header = (DDSFileHeader) Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(DDSFileHeader));
            handle.Free();

            return new Size((int) header.Width, (int) header.Height);
        }

        private byte[] ReadMainImage(Size size)
        {
            const int pixelsInBlock = 16;
            const int sizePerBlock = 8;

            int pixelsInImage = size.Width * size.Height;
            int blocksInImage = pixelsInImage / pixelsInBlock;
            int mainImageSize = blocksInImage * sizePerBlock;

            return _binaryReader.ReadBytes(mainImageSize);
        }

        public void Dispose()
        {
            _binaryReader.Close();
        }
    }
}
