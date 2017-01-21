
namespace BitmapCompressor.Formats
{
    public static class BlockFormat
    {
        /// <summary>
        /// The number of horizontal and vertical pixels in a single block.
        /// </summary>
        public const int Dimension = 4;

        /// <summary>
        /// The number of pixels in a single block.
        /// </summary>
        public const int PixelCount = 16;

        /// <summary>
        /// The number of bytes consumed by a BC1 block.
        /// </summary>
        public const int BC1ByteSize = 8;

        /// <summary>
        /// The number of bytes consumed by a BC2 block.
        /// </summary>
        public const int BC2ByteSize = 16;
    }
}
