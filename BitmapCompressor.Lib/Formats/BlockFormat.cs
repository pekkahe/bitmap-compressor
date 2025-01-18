namespace BitmapCompressor.Lib.Formats;

public static class BlockFormat
{
    /// <summary>
    /// The number of horizontal and vertical texel in a single block.
    /// </summary>
    public const int Dimension = 4;

    /// <summary>
    /// The number of texel in a single block.
    /// </summary>
    public const int TexelCount = 16;

    /// <summary>
    /// The number of bytes consumed by a BC1 block.
    /// </summary>
    public const int BC1ByteSize = 8;

    /// <summary>
    /// The number of bytes consumed by a BC2 block.
    /// </summary>
    public const int BC2ByteSize = 16;

    /// <summary>
    /// The number of bytes consumed by a BC3 block.
    /// </summary>
    public const int BC3ByteSize = 16;
}
