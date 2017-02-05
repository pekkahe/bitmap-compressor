using System;
using System.IO;
using System.Runtime.InteropServices;
using BitmapCompressor.DataTypes;
using BitmapCompressor.Formats;
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

            var header = ReadHeader();

            int imageWidth = (int) header.Width;
            int imageHeight = (int) header.Height;

            var format = CreateCompressionFormat(header.PixelFormat.FourCC);
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

        /// <summary>
        /// Instantiates the appropriate <see cref="IBlockCompressionFormat"/> for
        /// the specified DirectX FourCC (four-character code) value.
        /// </summary>
        public static IBlockCompressionFormat CreateCompressionFormat(uint fourCC)
        {
            if (fourCC == FourCC.BC1Unorm.Value)
                return new BC1Format();
            if (fourCC == FourCC.BC2Unorm.Value)
                return new BC2Format();
            if (fourCC == FourCC.BC3Unorm.Value)
                return new BC3Format();

            throw new ArgumentException(
                $"Unable to determine compression format for unknown FourCC code: {fourCC}.");
        }

        private byte[] ReadSurfaceData(int width, int height, IBlockCompressionFormat format)
        {
            int pixelsInImage = width * height;
            int blocksInImage = pixelsInImage / BlockFormat.TexelCount;
            int mainImageSize = blocksInImage * format.BlockSize;

            return _binaryReader.ReadBytes(mainImageSize);
        }

        public void Dispose()
        {
            _binaryReader.Close();
        }
    }
}
