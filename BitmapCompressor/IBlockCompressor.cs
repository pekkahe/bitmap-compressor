using BitmapCompressor.DataTypes;

namespace BitmapCompressor
{
    /// <summary>
    /// Provides block compression and decompression operations for uncompressed bitmaps.
    /// </summary>
    public interface IBlockCompressor
    {
        /// <summary>
        /// Compresses an uncompressed bitmap image into a block compressed DDS file stored in memory.
        /// </summary>
        /// <param name="image">The image to compress.</param>
        ICompressedImage Compress(IUncompressedImage image);

        /// <summary>
        /// Decompresses a block compressed DDS file into an uncompressed bitmap stored in memory.
        /// </summary>
        /// <param name="image">The image to decompress</param>
        IUncompressedImage Decompress(ICompressedImage image);
    }
}
