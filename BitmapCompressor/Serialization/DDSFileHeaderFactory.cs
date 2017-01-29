using System;
using BitmapCompressor.Serialization.FileFormat;
using BitmapCompressor.Formats;

namespace BitmapCompressor.Serialization
{
    public static class DDSFileHeaderFactory
    {
        /// <summary>
        /// Creates a DDS file header data structure for a BCn compression format
        /// with a single RGB surface of the specified dimensions.
        /// </summary>
        /// <param name="pixelWidth">The width of the DDS image described by this header.</param>
        /// <param name="pixelHeight">The height of the DDS image described by this header.</param>
        /// <param name="format">The BCn compression format described by this header.</param>
        public static DDSFileHeader CreateHeader(int pixelWidth, int pixelHeight, CompressionFormat format)
        {
            if (pixelHeight < 0 || pixelWidth < 0)
                throw new ArgumentException("Received negative image dimension.");

            var header = new DDSFileHeader();
            header.Size = 124;
            header.Flags = MinimumRequiredFlags();
            header.Height = (uint) pixelHeight;
            header.Width = (uint) pixelWidth;
            header.PixelFormat = CreatePixelFormat(format);
            header.Caps = DDSCapsFlags.DDSCAPS_TEXTURE;
            return header;
        }

        private static uint MinimumRequiredFlags()
        {
            return DDSFileHeaderFlags.DDSD_CAPS  | DDSFileHeaderFlags.DDSD_HEIGHT | 
                   DDSFileHeaderFlags.DDSD_WIDTH | DDSFileHeaderFlags.DDSD_PIXELFORMAT;
        }

        private static DDSPixelFormat CreatePixelFormat(CompressionFormat format)
        {
            var pixelFormat = new DDSPixelFormat();
            pixelFormat.Size = 32;
            pixelFormat.Flags = DDSPixelFormatFlags.DDPF_FOURCC;
            pixelFormat.FourCC = DDSPixelFormatFourCC.ToFourCC(format);
            return pixelFormat;
        }
    }
}
