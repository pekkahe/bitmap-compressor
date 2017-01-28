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

            int imageWidth = (int) header.Width;
            int imageHeight = (int) header.Height;

            var format = DetermineCompressionFormat(header.PixelFormat);
            var surfaceData = ReadSurfaceData(imageWidth, imageHeight, format);

            return DDSImage.CreateFromData(imageWidth, imageHeight, surfaceData, format);
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

        private IBlockCompressionFormat DetermineCompressionFormat(DDSPixelFormat pixelFormat)
        {
            if (pixelFormat.FourCC == DDSPixelFormatFourCC.FOURCC_DXT1)
                return new BC1Format();

            if (pixelFormat.FourCC == DDSPixelFormatFourCC.FOURCC_DXT2)
                return new BC2Format();

            throw new ArgumentOutOfRangeException(nameof(pixelFormat.FourCC));
        }

        private byte[] ReadSurfaceData(int width, int height, IBlockCompressionFormat format)
        {
            int pixelsInImage = width * height;
            int blocksInImage = pixelsInImage / BlockFormat.PixelCount;
            int mainImageSize = blocksInImage * format.BlockSize;

            return _binaryReader.ReadBytes(mainImageSize);
        }

        public void Dispose()
        {
            _binaryReader.Close();
        }
    }
}
