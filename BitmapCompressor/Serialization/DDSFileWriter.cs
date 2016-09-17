using System;
using System.IO;
using System.Runtime.InteropServices;
using BitmapCompressor.DataTypes;
using BitmapCompressor.Serialization.FileFormat;

namespace BitmapCompressor.Serialization
{
    public class DDSFileWriter : IDisposable
    {
        private readonly BinaryWriter _binaryWriter;
        private DDSImage _image;

        public DDSFileWriter(Stream stream)
        {
            _binaryWriter = new BinaryWriter(stream);
        }

        public DDSFileWriter(string fileName) : this(new FileStream(fileName, FileMode.Create))
        { }

        /// <summary>
        /// Writes the main image data stored in the file handle into a DDS file.
        /// </summary>
        public unsafe void Write(DDSImage image)
        {
            _image = image;

            WriteMagicNumber();

            WriteHeader();

            WriteMainImage();
        }

        private void WriteMagicNumber()
        {
            _binaryWriter.Write(DDSFile.MagicNumber);
        }

        private void WriteHeader()
        {
            var header = DDSFileHeaderFactory.CreateDXT1Header(_image.Width, _image.Height);
            var headerSize = Marshal.SizeOf(typeof(DDSFileHeader));
            var headerBuffer = new byte[headerSize];

            // Write the header data directly from the DDS file header structure
            GCHandle handle = GCHandle.Alloc(headerBuffer, GCHandleType.Pinned);
            Marshal.StructureToPtr(header, handle.AddrOfPinnedObject(), true);

            _binaryWriter.Write(headerBuffer);

            handle.Free();
        }

        private void WriteMainImage()
        {
            _binaryWriter.Write(_image.Buffer);
        }

        public void Dispose()
        {
            _binaryWriter.Close();
        }
    }
}
