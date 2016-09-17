using System;
using BitmapCompressor.Serialization.FileFormat;

namespace BitmapCompressor.Serialization
{
    public static class DDSFileHeaderFactory
    {
        /// <summary>
        /// Creates a DDS header file structure for a DXT1 compressed image with a single RGB
        /// surface and the specified pixel dimensions.
        /// </summary>
        /// <remarks>
        /// The name DXT1 is a FourCC (literally, four-character code) code originally assigned
        /// by Microsoft and synonym to BC1, which is the compression format's DirectX 10 name.
        /// </remarks>
        /// <param name="pixelWidth">The width of the DDS image described by this header.</param>
        /// <param name="pixelHeight">The height of the DDS image described by this header.</param>
        public static DDSFileHeader CreateDXT1Header(int pixelWidth, int pixelHeight)
        {
            if (pixelHeight < 0 || pixelWidth < 0)
                throw new ArgumentException("Negative pixel dimensions provided.");

            var header = new DDSFileHeader();
            header.Size = 124;
            header.Flags = MinimumRequiredFlags();
            header.Height = (uint) pixelHeight;
            header.Width = (uint) pixelWidth;
            header.PixelFormat = PixelFormatForDXT1();
            header.Caps = DDSCapsFlags.DDSCAPS_TEXTURE;
            return header;
        }

        private static uint MinimumRequiredFlags()
        {
            return DDSFileHeaderFlags.DDSD_CAPS  | DDSFileHeaderFlags.DDSD_HEIGHT | 
                   DDSFileHeaderFlags.DDSD_WIDTH | DDSFileHeaderFlags.DDSD_PIXELFORMAT;
        }

        private static DDSPixelFormat PixelFormatForDXT1()
        {
            var pixelFormat = new DDSPixelFormat();
            pixelFormat.Size = 32;
            pixelFormat.Flags = DDSPixelFormatFlags.DDPF_FOURCC;
            pixelFormat.FourCC = DDSPixelFormatFourCC.FOURCC_DXT1;
            return pixelFormat;
        }
    }
}
