using BitmapCompressor.Lib.DataTypes;

namespace BitmapCompressor.Lib.Formats;

/// <summary>
/// Represents the data layout for a 8-byte BC1 compressed block. 
/// </summary>
/// <remarks>
/// <para>
/// The block stores two 16-bit reference colors and a 32-bit index table
/// for mapping a 2-bit color table index to each texel in the block.
/// </para>
/// <para>
/// 64-bit block layout:
/// -------------------------------------------------------------------------
/// 63       55       47       39       31       23       15       7        0 
/// | c0-low | c0-hi  | c1-low | c1-hi  | c-idx0 | c-idx1 | c-idx2 | c-idx3 |
/// -------------------------------------------------------------------------
/// </para>
/// <para>
/// 2-bit color index values per texel a-p (0-15):
/// -----------------------------------------------
/// 31 30 29 28 27 26 25 24 23 22 21 20 19 18 17 16 
///  |  d  |  c  |  b  |  a  |  h  |  g  |  f  |  e 
///           c-idx0         |        c-idx1
/// 15 14 13 12 11 10  9  8  7  6  5  4  3  2  1  0
///  |  l  |  k  |  j  |  i  |  p  |  o  |  n  |  m
///           c-idx2         |        c-idx3
/// -----------------------------------------------
/// </para>
/// </remarks>
public class BC1BlockData
{
    /// <summary>
    /// Instantiates an empty <see cref="BC1BlockData"/> representing
    /// the data layout of a BC1 compressed block. 
    /// </summary>
    public BC1BlockData()
    { }

    /// <summary>
    /// The first compressed 16-bit reference color.
    /// </summary>
    public Color565 Color0 { get; set; }

    /// <summary>
    /// The second compressed 16-bit reference color.
    /// </summary>
    public Color565 Color1 { get; set; }

    /// <summary>
    /// An array of 16 2-bit color index values, ordered by texel index p0-15, 
    /// following row-major order within the 4x4 block.
    /// </summary>
    /// <remarks>
    /// Values higher than 2-bits are automatically stripped to 2-bits when
    /// the block instance is converted to bytes.
    /// </remarks>
    public int[] ColorIndexes { get; } = new int[BlockFormat.TexelCount];

    /// <summary>
    /// Convert the block data into a 8-byte BC1 format byte array.
    /// </summary>
    public byte[] ToBytes()
    {
        byte c0Low      = (byte) ((Color0.Value & 0x00FF) >> 0);
        byte c0Hi       = (byte) ((Color0.Value & 0xFF00) >> 8);
        byte c1Low      = (byte) ((Color1.Value & 0x00FF) >> 0);
        byte c1Hi       = (byte) ((Color1.Value & 0xFF00) >> 8);
        byte[] indexes  = new byte[4];

        for (int p = 0, row = 0; p < BlockFormat.TexelCount; p += BlockFormat.Dimension, ++row)
        {
            int a = p;
            int b = p + 1;
            int c = p + 2;
            int d = p + 3;

            indexes[row] = (byte) ((ColorIndexes[a] & 0x03) |
                                  ((ColorIndexes[b] & 0x03) << 2) |
                                  ((ColorIndexes[c] & 0x03) << 4) |
                                  ((ColorIndexes[d] & 0x03) << 6));
        }
        
        var bytes = new byte[8];

        bytes[0] = c0Low;
        bytes[1] = c0Hi;
        bytes[2] = c1Low;
        bytes[3] = c1Hi;
        bytes[4] = indexes[0];
        bytes[5] = indexes[1];
        bytes[6] = indexes[2];
        bytes[7] = indexes[3];

        return bytes;
    }

    /// <summary>
    /// Instantiates a <see cref="BC1BlockData"/> from compressed BC1 block data.
    /// </summary>
    /// <param name="bytes">The data of a BC1 compressed block.</param>
    public static BC1BlockData FromBytes(byte[] bytes)
    {
        Debug.Assert(bytes.Length == BlockFormat.BC1ByteSize,
            "Mismatching number of bytes for format.");

        byte c0Low      = bytes[0];
        byte c0Hi       = bytes[1];
        byte c1Low      = bytes[2];
        byte c1Hi       = bytes[3];
        byte[] indexes  = new byte[4];
        indexes[0]      = bytes[4];
        indexes[1]      = bytes[5];
        indexes[2]      = bytes[6];
        indexes[3]      = bytes[7];

        var block = new BC1BlockData();
        block.Color0 = Color565.FromValue((ushort) ((c0Hi << 8) | c0Low));
        block.Color1 = Color565.FromValue((ushort) ((c1Hi << 8) | c1Low));

        for (int p = 0, row = 0; p < BlockFormat.TexelCount; p += BlockFormat.Dimension, ++row)
        {
            block.ColorIndexes[p]     =  indexes[row]       & 0x03;
            block.ColorIndexes[p + 1] = (indexes[row] >> 2) & 0x03;
            block.ColorIndexes[p + 2] = (indexes[row] >> 4) & 0x03;
            block.ColorIndexes[p + 3] = (indexes[row] >> 6) & 0x03;
        }

        return block;
    }
}
