using BitmapCompressor.DataTypes;
using BitmapCompressor.Formats;

namespace BitmapCompressor
{
    /// <summary>
    /// Provides block compression and decompression operations for uncompressed bitmaps
    /// and compressed DDS images, respectively.
    /// </summary>
    public interface IBlockCompressor
    {
        /// <summary>
        /// Compresses an uncompressed bitmap image into a block compressed DDS file.
        /// </summary>
        /// <param name="image">The image to compress.</param>
        /// <param name="format">The block compression format to use.</param>
        /// <returns>A representation of the compressed DDS image stored in memory.</returns>
        ICompressedImage Compress(IUncompressedImage image, IBlockCompressionFormat format);

        /// <summary>
        /// Decompresses a block compressed DDS file into an uncompressed bitmap.
        /// </summary>
        /// <param name="image">The image to decompress</param>
        /// <returns>A representation of the uncompressed bitmap image stored in memory.</returns>
        IUncompressedImage Decompress(ICompressedImage image);
    }
}
