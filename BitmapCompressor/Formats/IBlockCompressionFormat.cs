using System;
using System.Drawing;

namespace BitmapCompressor.Formats
{
    /// <summary>
    /// Represents a block compression algorithm executed on 
    /// a 4x4 texel block according to a specific BCn format.
    /// </summary>
    public interface IBlockCompressionFormat
    {
        /// <summary>
        /// The number of bytes a single BCn block of this format consumes.
        /// </summary>
        int BlockSize { get; }

        /// <summary>
        /// Compresses the 16 RGB colors in a 4x4 texel block into a BCn compressed data buffer.
        /// </summary>
        /// <param name="colors">The 16 32-bit R8:G8:B8 colors to compress.</param>
        byte[] Compress(Color[] colors);

        /// <summary>
        /// Decompresses the BCn data buffer into 16 (A)RGB colors.
        /// </summary>
        /// <param name="block">The BCn block-compressed data to uncompress.</param>
        Color[] Decompress(byte[] blockData);
    }
}
