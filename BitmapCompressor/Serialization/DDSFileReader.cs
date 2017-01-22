using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using BitmapCompressor.DataTypes;
using BitmapCompressor.Serialization.FileFormat;
using BitmapCompressor.Formats;

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

            var header = ReadHeader();
            var size = new Size((int) header.Width, (int) header.Height);

            var surfaceData = ReadSurfaceData(size);
            var format = DetermineCompressionFormat(header.PixelFormat);

            return DDSImage.CreateFromData(size.Width, size.Height, surfaceData, format);
        }

        private IBlockCompressionFormat DetermineCompressionFormat(DDSPixelFormat pixelFormat)
        {
            if (pixelFormat.FourCC == DDSPixelFormatFourCC.FOURCC_DXT1)
                return new BC1Format();

            if (pixelFormat.FourCC == DDSPixelFormatFourCC.FOURCC_DXT2)
                return new BC2Format();

            throw new ArgumentOutOfRangeException(nameof(pixelFormat.FourCC));
        }

        private void ReadMagicNumber()
        { 
            int size = Marshal.SizeOf(DDSFile.MagicNumber);
            var buffer = _binaryReader.ReadBytes(size);

            var magicNumber = BitConverter.ToUInt32(buffer, 0);
            if (magicNumber != DDSFile.MagicNumber)
                throw new InvalidOperationException("Unrecognizable DDS file format.");
        }

        private DDSFileHeader ReadHeader()
        {
            int size = Marshal.SizeOf(typeof(DDSFileHeader));
            var buffer = _binaryReader.ReadBytes(size);

            // Read the header data directly into a DDS file header structure
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            var header = (DDSFileHeader) Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(DDSFileHeader));
            handle.Free();

            return header;
        }

        private byte[] ReadSurfaceData(Size size)
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
