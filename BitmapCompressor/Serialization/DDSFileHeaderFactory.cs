using System;
using BitmapCompressor.Serialization.FileFormat;
using BitmapCompressor.Formats;

namespace BitmapCompressor.Serialization
{
    public static class DDSFileHeaderFactory
    {
        /// <summary>
        /// Creates a DDS file header data structure for a BC compression format
        /// with a single RGB surface of the specified dimensions.
        /// </summary>
        /// <param name="pixelWidth">The width of the DDS image described by this header.</param>
        /// <param name="pixelHeight">The height of the DDS image described by this header.</param>
        /// <param name="format">The height of the DDS image described by this header.</param>
        public static DDSFileHeader CreateHeader(int pixelWidth, int pixelHeight, CompressionFormat format)
        {
            switch (format)
            {
                case CompressionFormat.BC1:
                    return CreateHeader(pixelWidth, pixelHeight, DDSPixelFormatFourCC.FOURCC_DXT1);
                case CompressionFormat.BC2:
                    return CreateHeader(pixelWidth, pixelHeight, DDSPixelFormatFourCC.FOURCC_DXT2);
                case CompressionFormat.BC3:
                    return CreateHeader(pixelWidth, pixelHeight, DDSPixelFormatFourCC.FOURCC_DXT3);
                case CompressionFormat.BC4:
                    return CreateHeader(pixelWidth, pixelHeight, DDSPixelFormatFourCC.FOURCC_DXT4);
                case CompressionFormat.BC5:
                    return CreateHeader(pixelWidth, pixelHeight, DDSPixelFormatFourCC.FOURCC_DXT5);
            }

            throw new ArgumentOutOfRangeException(nameof(format));
        }

        /// <summary>
        /// Creates a DDS header file structure for the specified FourCC (four-character code)
        /// value. Supported values are specified in <see cref="DDSPixelFormatFourCC"/>.
        /// </summary>
        /// <remarks>
        /// The name DXT1 is a FourCC (literally, four-character code) code originally assigned
        /// by Microsoft and synonym to BC1, which is the compression format's DirectX 10 name.
        /// </remarks>
        private static DDSFileHeader CreateHeader(int pixelWidth, int pixelHeight, uint fourCC)
        {
            if (pixelHeight < 0 || pixelWidth < 0)
                throw new ArgumentException("Received negative image dimension.");

            var header = new DDSFileHeader();
            header.Size = 124;
            header.Flags = MinimumRequiredFlags();
            header.Height = (uint) pixelHeight;
            header.Width = (uint) pixelWidth;
            header.PixelFormat = CreatePixelFormat(fourCC);
            header.Caps = DDSCapsFlags.DDSCAPS_TEXTURE;
            return header;
        }

        private static uint MinimumRequiredFlags()
        {
            return DDSFileHeaderFlags.DDSD_CAPS  | DDSFileHeaderFlags.DDSD_HEIGHT | 
                   DDSFileHeaderFlags.DDSD_WIDTH | DDSFileHeaderFlags.DDSD_PIXELFORMAT;
        }

        private static DDSPixelFormat CreatePixelFormat(uint fourCC)
        {
            var pixelFormat = new DDSPixelFormat();
            pixelFormat.Size = 32;
            pixelFormat.Flags = DDSPixelFormatFlags.DDPF_FOURCC;
            pixelFormat.FourCC = fourCC;
            return pixelFormat;
        }
    }
}
