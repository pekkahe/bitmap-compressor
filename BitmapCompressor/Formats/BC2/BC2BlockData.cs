using System;
using System.Diagnostics;
using BitmapCompressor.DataTypes;

namespace BitmapCompressor.Formats
{
    /// <summary>
    /// Represents the data layout for a BC2 compressed block.
    /// </summary>
    /// <remarks>
    /// Bit layout:
    /// 127      119      111      103      95       87       79       71       63
    /// | alpha0 | alpha1 | alpha2 | alpha3 | alpha4 | alpha5 | alpha6 | alpha7 |
    /// 63       55       47       39       31       23       15       7        0 
    /// | c0-low | c0-hi  | c1-low | c1-hi  | index0 | index1 | index2 | index3 |
    /// -------------------------------------------------------------------------
    /// </remarks>
    public class BC2BlockData
    {
        /// <summary>
        /// Instantiates an empty <see cref="BC2BlockData"/> representing
        /// the data specification of a BC2 compressed block. 
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
        /// An array of 16 2-bit color index values, ordered by pixel index p0-15, 
        /// following row-major order within the 4x4 block.
        /// </summary>
        /// <remarks>
        /// Values higher than 2-bits are automatically stripped to 2-bits when
        /// the block instance is converted to bytes.
        /// </remarks>
        public int[] ColorIndexes { get; } = new int[BlockFormat.PixelCount];

        /// <summary>
        /// An array of 16 4-bit color alpha values, ordered by pixel index p0-15, 
        /// following row-major order within the 4x4 block.
        /// </summary>
        /// <remarks>
        /// Values higher than 4-bits are automatically stripped to 4-bits when
        /// the block instance is converted to bytes.
        /// </remarks>
        public int[] ColorAlphas { get; } = new int[BlockFormat.PixelCount];

        /// <summary>
        /// Convert the block data into a 16-byte BC2 format byte array, storing two 16-bit
        /// reference colors and two tables mapping a color index and 4-bit alpha value, 
        /// respectively, to each pixel in a block.
        /// </summary>
        public byte[] ToBytes()
        {
            byte c0Low  = (byte) ((Color0.Value & 0x00FF) >> 0);
            byte c0Hi   = (byte) ((Color0.Value & 0xFF00) >> 8);
            byte c1Low  = (byte) ((Color1.Value & 0x00FF) >> 0);
            byte c1Hi   = (byte) ((Color1.Value & 0xFF00) >> 8);

            byte[,] alphas  = new byte[4, 2];
            byte[] indexes  = new byte[4];

            for (int p = 0, row = 0; p < BlockFormat.PixelCount; p += BlockFormat.Dimension, ++row)
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
                "Mismatching number of bytes for BC2 format.");

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

            for (int p = 0, row = 0; p < BlockFormat.PixelCount; p += BlockFormat.Dimension, ++row)
            {
                int a = p;
                int b = p + 1;
                int c = p + 2;
                int d = p + 3;

                block.ColorIndexes[a] =  indexes[row]       & 0x03;
                block.ColorIndexes[b] = (indexes[row] >> 2) & 0x03;
                block.ColorIndexes[c] = (indexes[row] >> 4) & 0x03;
                block.ColorIndexes[d] = (indexes[row] >> 6) & 0x03;

                block.ColorAlphas[a] = (byte)  (alphas[row, 1] & 0x00FF);
                block.ColorAlphas[b] = (byte) ((alphas[row, 1] & 0xFF00) >> 4);
                block.ColorAlphas[c] = (byte)  (alphas[row, 0] & 0x00FF);
                block.ColorAlphas[d] = (byte) ((alphas[row, 0] & 0xFF00) >> 4);
            }

            return block;
        }
    }
}
