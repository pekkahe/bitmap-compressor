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

        public DDSFileWriter(Stream stream)
        {
            _binaryWriter = new BinaryWriter(stream);
        }

        public DDSFileWriter(string fileName) : this(new FileStream(fileName, FileMode.Create))
        { }

        /// <summary>
        /// Writes the main image data stored in the specified image into a DDS file.
        /// </summary>
        public unsafe void Write(ICompressedImage image)
        {
            WriteMagicNumber();

            var header = DDSFileHeaderFactory.CreateHeader(image.Width, image.Height, image.GetFormat().Name);

            WriteHeader(header);

            WriteSurfaceData(image.GetBuffer());
        }

        private void WriteMagicNumber()
        {
            _binaryWriter.Write(DDSFile.MagicNumber);
        }

        private void WriteHeader(DDSFileHeader header)
        {
            var headerSize = Marshal.SizeOf(typeof(DDSFileHeader));
            var headerBuffer = new byte[headerSize];

            // Write the header data directly from the DDS file header structure
            GCHandle handle = GCHandle.Alloc(headerBuffer, GCHandleType.Pinned);
            Marshal.StructureToPtr(header, handle.AddrOfPinnedObject(), true);

            _binaryWriter.Write(headerBuffer);

            handle.Free();
        }

        private void WriteSurfaceData(byte[] surfaceData)
        {
            _binaryWriter.Write(surfaceData);
        }

        public void Dispose()
        {
            _binaryWriter.Close();
        }
    }
}
