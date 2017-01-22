using System;
using System.Drawing;

namespace BitmapCompressor.Formats
{
    /// <summary>
    /// Represents a block compression algorithm which compresses the 32-bit colors of a  
    /// single 4x4 pixel block into an n-byte data block according to a specific BCn format.
    /// </summary>
    /// <remarks>
    /// Block compression references:
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/bb694531(v=vs.85).aspx
    /// https://www.opengl.org/wiki/S3_Texture_Compression
    /// http://www.reedbeta.com/blog/2012/02/12/understanding-bcn-texture-compression-formats/
    /// </remarks>
    public interface IBlockCompressionFormat
    {
        /// <summary>
        /// The name of this compression format.
        /// </summary>
        CompressionFormat Name { get; }

        /// <summary>
        /// The number of bytes a single BCn block of this format consumes.
        /// </summary>
        int BlockSize { get; }

        /// <summary>
        /// Compresses the 16 RGB colors in a 4x4 pixel block into a BCn compressed data buffer.
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
