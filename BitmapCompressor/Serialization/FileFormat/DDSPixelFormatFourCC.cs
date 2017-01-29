using System;
using BitmapCompressor.Formats;

namespace BitmapCompressor.Serialization.FileFormat
{
    /// <summary>
    /// FourCC values for <see cref="DDSPixelFormat.FourCC"/>.
    /// </summary>
    public static class DDSPixelFormatFourCC
    {
        /// <summary>
        /// FourCC value for BC1 compression with 1-bit alpha.
        /// </summary>
        public static readonly uint FOURCC_BC1_UNORM = MakeFourCC('D', 'X', 'T', '1');

        /// <summary>
        /// FourCC value for BC2 compression with 4-bit nonpremultiplied alpha.
        /// </summary>
        public static readonly uint FOURCC_BC2_UNORM = MakeFourCC('D', 'X', 'T', '3');

        /// <summary>
        /// FourCC value for BC3 compression with interpolated nonpremultiplied alpha.
        /// </summary>
        public static readonly uint FOURCC_BC3_UNORM = MakeFourCC('D', 'X', 'T', '5');

        /// <summary>
        /// FourCC value for BC4 compression.
        /// </summary>
        public static readonly uint FOURCC_BC4_UNORM = MakeFourCC('B', 'C', '4', 'U');

        /// <summary>
        /// FourCC value for BC5 compression.
        /// </summary>
        public static readonly uint FOURCC_BC5_UNORM = MakeFourCC('B', 'C', '5', 'U');             

        /// <summary>
        /// Generates a FourCC code from four character integer values.
        /// </summary>
        private static uint MakeFourCC(int char0, int char1, int char2, int char3)
        {
            return (uint)  ((byte) (char0) | 
                            (byte) (char1) << 8 | 
                            (byte) (char2) << 16 | 
                            (byte) (char3) << 24);
        }

        /// <summary>
        /// Maps a <see cref="CompressionFormat"/> into its appropriate
        /// DirectX FourCC (four-character code) value.
        /// </summary>
        public static uint ToFourCC(CompressionFormat format)
        {
            switch (format)
            {
                case CompressionFormat.BC1:
                    return FOURCC_BC1_UNORM;
                case CompressionFormat.BC2:
                    return FOURCC_BC2_UNORM;
                case CompressionFormat.BC3:
                    return FOURCC_BC3_UNORM;
                case CompressionFormat.BC4:
                    return FOURCC_BC4_UNORM;
                case CompressionFormat.BC5:
                    return FOURCC_BC5_UNORM;
                default:
                    throw new ArgumentOutOfRangeException(nameof(format));
            }
        }

        /// <summary>
        /// Maps a DirectX FourCC (four-character code) value into a new instance of
        /// a <see cref="IBlockCompressionFormat"/> which can operate on the format.
        /// </summary>
        public static IBlockCompressionFormat ToCompressionFormat(uint fourCC)
        {
            if (fourCC == FOURCC_BC1_UNORM)
                return new BC1Format();
            if (fourCC == FOURCC_BC2_UNORM)
                return new BC2Format();

            throw new ArgumentOutOfRangeException("Unable to determine compression " +
                                                 $"format for unknown FourCC code: {fourCC}.");
        }
    }
}
