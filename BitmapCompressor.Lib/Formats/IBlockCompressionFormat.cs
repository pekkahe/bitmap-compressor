using BitmapCompressor.Lib.DataTypes;

namespace BitmapCompressor.Lib.Formats;

/// <summary>
/// Represents a block compression algorithm which compresses the 32-bit colors of a  
/// single 4x4 texel area into an n-byte data block according to a specific BCn format.
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
    /// The number of bytes a single block of this format consumes.
    /// </summary>
    int BlockSize { get; }

    /// <summary>
    /// The DirectX four-character code of this compression format.
    /// </summary>
    FourCC FourCC { get; }

    /// <summary>
    /// Compresses the 32-bit RGB colors of a 4x4 texel area into a byte layout
    /// specific to this compression format.
    /// </summary>
    /// <param name="colors">The 16 32-bit RGB colors to compress.</param>
    byte[] Compress(Color[] colors);

    /// <summary>
    /// Decompresses the byte layout specific to this compression format into
    /// 32-bit RGB colors in a 4x4 texel area.
    /// </summary>
    /// <param name="block">The block compressed data to uncompress.</param>
    Color[] Decompress(byte[] blockData);
}
