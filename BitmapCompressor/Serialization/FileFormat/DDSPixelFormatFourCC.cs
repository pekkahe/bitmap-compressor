
namespace BitmapCompressor.Serialization.FileFormat
{
    /// <summary>
    /// FourCC values for <see cref="DDSPixelFormat.FourCC"/>.
    /// </summary>
    public static class DDSPixelFormatFourCC
    {
        /// <summary>
        /// FourCC value for DXT1 (BC1) compression.
        /// </summary>
        public static readonly uint FOURCC_DXT1 = MakeFourCC('D', 'X', 'T', '1');

        /// <summary>
        /// FourCC value for DXT2 (BC2) compression.
        /// </summary>
        public static readonly uint FOURCC_DXT2 = MakeFourCC('D', 'X', 'T', '2');

        /// <summary>
        /// FourCC value for DXT3 (BC3) compression.
        /// </summary>
        public static readonly uint FOURCC_DXT3 = MakeFourCC('D', 'X', 'T', '3');

        /// <summary>
        /// FourCC value for DXT4 (BC4) compression.
        /// </summary>
        public static readonly uint FOURCC_DXT4 = MakeFourCC('D', 'X', 'T', '4');

        /// <summary>
        /// FourCC value for DXT5 (BC5) compression.
        /// </summary>
        public static readonly uint FOURCC_DXT5 = MakeFourCC('D', 'X', 'T', '5');

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
    }
}
