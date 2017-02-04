using System;
using System.Diagnostics;
using BitmapCompressor.DataTypes;

namespace BitmapCompressor.Formats
{
    /// <summary>
    /// Represents the data layout for a 16-byte BC2 compressed block. 
    /// </summary>
    /// <remarks>
    /// <para>
    /// The block stores two 16-bit reference colors and a 32-bit color index table, 
    /// similar to <see cref="BC1BlockData"/>, and also a 64-bit index table for
    /// mapping a 4-bit alpha value to each texel in the block.
    /// </para>
    /// <para>
    /// 128-bit block layout:
    /// -------------------------------------------------------------------------
    /// 127      119      111      103      95       87       79       71       63
    /// | alpha0 | alpha1 | alpha2 | alpha3 | alpha4 | alpha5 | alpha6 | alpha7 |
    /// 63       55       47       39       31       23       15       7        0 
    /// | c0-low | c0-hi  | c1-low | c1-hi  | c-idx0 | c-idx1 | c-idx2 | c-idx3 |
    /// -------------------------------------------------------------------------
    /// </para>
    /// <para>
    /// 4-bit alpha values per texel a-p (0-15):
    /// -----------------------------------------------------------------------------------------------
    /// 63 62 61 60 59 58 57 56 55 54 53 52 51 50 49 48 47 46 45 44 43 42 41 40 39 38 37 36 35 34 33 32 
    ///  |     d     |     c     |     b     |     a     |     h     |     g     |     f     |     e  
    ///           alpha0         |        alpha1         |        alpha2         |        alpha3
    /// 31 30 29 28 27 26 25 24 23 22 21 20 19 18 17 16 15 14 13 12 11 10  9  8  7  6  5  4  3  2  1  0
    ///  |     l     |     k     |     j     |     i     |     p     |     o     |     n     |     m
    ///           alpha4         |        alpha5         |        alpha6         |        alpha7
    /// -----------------------------------------------------------------------------------------------
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
    public class BC2BlockData
    {
        /// <summary>
        /// Instantiates an empty <see cref="BC2BlockData"/> representing
        /// the data layout of a BC2 compressed block. 
        /// </summary>
        public BC2BlockData()
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
        /// An array of 16 2-bit color index values, ordered by texel index 0-15, 
        /// following row-major order within the 4x4 area.
        /// </summary>
        /// <remarks>
        /// Values higher than 2-bits are automatically stripped to 2-bits when
        /// the block instance is converted to bytes.
        /// </remarks>
        public int[] ColorIndexes { get; } = new int[BlockFormat.TexelCount];

        /// <summary>
        /// An array of 16 4-bit color alpha values, ordered by texel index p0-15, 
        /// following row-major order within the 4x4 area.
        /// </summary>
        /// <remarks>
        /// Values higher than 4-bits are automatically stripped to 4-bits when
        /// the block instance is converted to bytes.
        /// </remarks>
        public int[] ColorAlphas { get; } = new int[BlockFormat.TexelCount];

        /// <summary>
        /// Convert the block data into a 16-byte BC2 format byte array.
        /// </summary>
        public byte[] ToBytes()
        {
            byte c0Low  = (byte) ((Color0.Value & 0x00FF) >> 0);
            byte c0Hi   = (byte) ((Color0.Value & 0xFF00) >> 8);
            byte c1Low  = (byte) ((Color1.Value & 0x00FF) >> 0);
            byte c1Hi   = (byte) ((Color1.Value & 0xFF00) >> 8);

            byte[,] alphas  = new byte[4, 2];
            byte[] indexes  = new byte[4];

            for (int p = 0, row = 0; p < BlockFormat.TexelCount; p += BlockFormat.Dimension, ++row)
            {
                int a = p;
                int b = p + 1;
                int c = p + 2;
                int d = p + 3;

                alphas[row, 0] = (byte) (((ColorAlphas[d] & 0x0F) << 4) | 
                                          (ColorAlphas[c] & 0x0F));
                alphas[row, 1] = (byte) (((ColorAlphas[b] & 0x0F) << 4) | 
                                          (ColorAlphas[a] & 0x0F));

                indexes[row] = (byte)   ((ColorIndexes[a] & 0x03) |
                                        ((ColorIndexes[b] & 0x03) << 2) |
                                        ((ColorIndexes[c] & 0x03) << 4) |
                                        ((ColorIndexes[d] & 0x03) << 6));
            }

            var bytes = new byte[16];

            bytes[0]    = alphas[0, 0];
            bytes[1]    = alphas[0, 1];
            bytes[2]    = alphas[1, 0];
            bytes[3]    = alphas[1, 1];
            bytes[4]    = alphas[2, 0];
            bytes[5]    = alphas[2, 1];
            bytes[6]    = alphas[3, 0];
            bytes[7]    = alphas[3, 1];
            bytes[8]    = c0Low;
            bytes[9]    = c0Hi;
            bytes[10]   = c1Low;
            bytes[11]   = c1Hi;
            bytes[12]   = indexes[0];
            bytes[13]   = indexes[1];
            bytes[14]   = indexes[2];
            bytes[15]   = indexes[3];

            return bytes;
        }

        /// <summary>
        /// Instantiates a <see cref="BC2BlockData"/> from compressed BC2 block data.
        /// </summary>
        /// <param name="bytes">The data of a BC2 compressed block.</param>
        public static BC2BlockData FromBytes(byte[] bytes)
        {
            Debug.Assert(bytes.Length == BlockFormat.BC2ByteSize,
                "Mismatching number of bytes for format.");

            byte[,] alphas  = new byte[4, 2];
            alphas[0, 0]    = bytes[0];
            alphas[0, 1]    = bytes[1];
            alphas[1, 0]    = bytes[2];
            alphas[1, 1]    = bytes[3];
            alphas[2, 0]    = bytes[4];
            alphas[2, 1]    = bytes[5];
            alphas[3, 0]    = bytes[6];
            alphas[3, 1]    = bytes[7];
            byte c0Low      = bytes[8];
            byte c0Hi       = bytes[9];
            byte c1Low      = bytes[10];
            byte c1Hi       = bytes[11];
            byte[] indexes  = new byte[4];
            indexes[0]      = bytes[12];
            indexes[1]      = bytes[13];
            indexes[2]      = bytes[14];
            indexes[3]      = bytes[15];

            var block = new BC2BlockData();
            block.Color0 = Color565.FromValue((ushort) ((c0Hi << 8) | c0Low));
            block.Color1 = Color565.FromValue((ushort) ((c1Hi << 8) | c1Low));

            for (int p = 0, row = 0; p < BlockFormat.TexelCount; p += BlockFormat.Dimension, ++row)
            {
                int a = p;
                int b = p + 1;
                int c = p + 2;
                int d = p + 3;

                block.ColorIndexes[a] =  indexes[row]       & 0x03;
                block.ColorIndexes[b] = (indexes[row] >> 2) & 0x03;
                block.ColorIndexes[c] = (indexes[row] >> 4) & 0x03;
                block.ColorIndexes[d] = (indexes[row] >> 6) & 0x03;

                block.ColorAlphas[a] = (byte)  (alphas[row, 1] & 0x0F);
                block.ColorAlphas[b] = (byte) ((alphas[row, 1] & 0xF0) >> 4);
                block.ColorAlphas[c] = (byte)  (alphas[row, 0] & 0x0F);
                block.ColorAlphas[d] = (byte) ((alphas[row, 0] & 0xF0) >> 4);
            }

            return block;
        }
    }
}
