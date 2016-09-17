using BitmapCompressor.DataTypes;

namespace BitmapCompressor
{
    /// <summary>
    /// Provides block compression and decompression operations for uncompressed bitmaps.
    /// </summary>
    public interface IBlockCompressor
    {
        /// <summary>
        /// Compresses an uncompressed bitmap image into a block compressed DDS file.
        /// </summary>
        /// <param name="image">The bitmap image to compress.</param>
        IProcessedImage Compress(BMPImage image);

        /// <summary>
        /// Decompresses a block compressed DDS file into an uncompressed bitmap.
        /// </summary>
        /// <param name="image">The DDS image to decompress</param>
        IProcessedImage Decompress(DDSImage image);
    }
}
