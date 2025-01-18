namespace BitmapCompressor.Lib.DataTypes;

/// <summary>
/// Represents a DirectX FourCC (four-character code) used to
/// describe a BCn compression format for DDS file serialization.
/// </summary>
public class FourCC
{
    /// <summary>
    /// FourCC value for BC1 compression with 1-bit alpha.
    /// </summary>
    public static readonly FourCC BC1Unorm = new(MakeFourCC('D', 'X', 'T', '1'));

    /// <summary>
    /// FourCC value for BC2 compression with 4-bit nonpremultiplied alpha.
    /// </summary>
    public static readonly FourCC BC2Unorm = new(MakeFourCC('D', 'X', 'T', '3'));

    /// <summary>
    /// FourCC value for BC3 compression with interpolated nonpremultiplied alpha.
    /// </summary>
    public static readonly FourCC BC3Unorm = new(MakeFourCC('D', 'X', 'T', '5'));

    /// <summary>
    /// FourCC value for BC4 compression.
    /// </summary>
    public static readonly FourCC BC4Unorm = new(MakeFourCC('B', 'C', '4', 'U'));

    /// <summary>
    /// FourCC value for BC5 compression.
    /// </summary>
    public static readonly FourCC BC5Unorm = new(MakeFourCC('B', 'C', '5', 'U'));

    private FourCC(uint value)
    {
        Value = value;
    }

    /// <summary>
    /// The FourCC code generated from four character integer values.
    /// </summary>
    public readonly uint Value;

    /// <summary>
    /// Generates a FourCC code from four character integer values.
    /// </summary>
    private static uint MakeFourCC(int char0, int char1, int char2, int char3)
    {
        return (uint) ((byte) (char0) |
                       (byte) (char1) << 8 |
                       (byte) (char2) << 16 |
                       (byte) (char3) << 24);
    }
}
