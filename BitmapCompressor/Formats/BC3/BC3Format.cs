using System;
using System.Drawing;
using System.Diagnostics;
using BitmapCompressor.DataTypes;
using BitmapCompressor.Utilities;

namespace BitmapCompressor.Formats
{
    /// <summary>
    /// The BC3 format compresses 4x4 texel areas into 16-byte data blocks. The format
    /// stores RGB data as two 16-bit colors and alpha as two 8-bit values.
    /// </summary>
    /// <remarks><para>
    /// The BC3 format stores colors with the same number of bits and data layout as the 
    /// <see cref="BC1Format"/>, however, alpha is stored as two 8-bit reference values 
    /// and an index table containing 3-bit indexes for each color. Compared to BC1, 
    /// the BC3 format requires an additional 64-bits of memory to store the alpha data.
    /// </para><para>
    /// The BC3 format handles alpha similarly to how BC1 stores RGB color; using a color
    /// table and interpolation.
    /// </para></remarks>
    public class BC3Format : IBlockCompressionFormat
    {
        public int BlockSize => BlockFormat.BC3ByteSize;

        public FourCC FourCC => FourCC.BC3Unorm;

        public byte[] Compress(Color[] colors)
        {
            Debug.Assert(colors.Length == BlockFormat.TexelCount);

            var colorSpace = new ColorSpace(colors);
            var colorTable = BC1ColorTable.Create(colorSpace.MaxColor, colorSpace.MinColor);

            // Interpolate 6 alpha values by passing max alpha as alpha0
            var alphaTable = CreateAlphaTable(colorSpace.MaxAlpha, colorSpace.MinAlpha);

            var block = new BC3BlockData();
            block.Color0 = colorTable[0];
            block.Color1 = colorTable[1];
            block.Alpha0 = alphaTable[0];
            block.Alpha1 = alphaTable[1];

            for (int i = 0; i < colors.Length; ++i)
            {
                var color32 = colors[i];
                var color16 = ColorUtility.To16Bit(color32);

                block.ColorIndexes[i] = ColorUtility.GetIndexOfClosestColor(colorTable, color16);
                block.AlphaIndexes[i] = ColorUtility.GetIndexOfClosestAlpha(alphaTable, color32.A);
            }

            return block.ToBytes();
        }

        public Color[] Decompress(byte[] blockData)
        {
            Debug.Assert(blockData.Length == BlockFormat.BC3ByteSize);

            var block = BC3BlockData.FromBytes(blockData);

            var colorTable = BC1ColorTable.Create(block.Color0, block.Color1);
            var alphaTable = CreateAlphaTable(block.Alpha0, block.Alpha1);

            var colors = new Color[BlockFormat.TexelCount];

            for (int i = 0; i < colors.Length; ++i)
            {
                int colorIndex = block.ColorIndexes[i];
                int alphaIndex = block.AlphaIndexes[i];

                var color = colorTable[colorIndex];
                var alpha = alphaTable[alphaIndex];

                colors[i] = Color.FromArgb(alpha, ColorUtility.To32Bit(color));
            }

            return colors;
        }

        /// <summary>
        /// Creates an 8-byte alpha value table from two reference alphas by interpolating 
        /// the remaining values.
        /// </summary>
        /// <remarks><para>
        /// The algorithm determines the number of interpolated alpha values by examining 
        /// the two reference alpha values. If alpha0 is greater than alpha1, then BC3 
        /// interpolates 6 alpha values; otherwise, it interpolates 4. When BC3 interpolates
        /// only 4 alpha values, it sets two additional alpha values (0 for fully transparent 
        /// and 255 for fully opaque).
        /// </para><para>
        /// BC3 compresses the alpha values in the 4×4 texel area by storing the bit code 
        /// corresponding to the interpolated alpha values which most closely matches the
        /// original alpha for a given texel.
        /// </para></remarks>
        /// <param name="alpha0">The first reference alpha.</param>
        /// <param name="alpha1">The second reference alpha.</param>
        public static byte[] CreateAlphaTable(byte alpha0, byte alpha1)
        {
            var alphas = new byte[8];
            alphas[0] = alpha0;
            alphas[1] = alpha1;

            if (alphas[0] > alphas[1])
            {
                alphas[2] = (byte) (6 / 7 * alphas[0] + 1 / 7 * alphas[1]); // bit code 010
                alphas[3] = (byte) (5 / 7 * alphas[0] + 2 / 7 * alphas[1]); // bit code 011
                alphas[4] = (byte) (4 / 7 * alphas[0] + 3 / 7 * alphas[1]); // bit code 100
                alphas[5] = (byte) (3 / 7 * alphas[0] + 4 / 7 * alphas[1]); // bit code 101
                alphas[6] = (byte) (2 / 7 * alphas[0] + 5 / 7 * alphas[1]); // bit code 110
                alphas[7] = (byte) (1 / 7 * alphas[0] + 6 / 7 * alphas[1]); // bit code 111
            }
            else
            {
                alphas[2] = (byte) (4 / 5 * alphas[0] + 1 / 5 * alphas[1]); // bit code 010
                alphas[3] = (byte) (3 / 5 * alphas[0] + 2 / 5 * alphas[1]); // bit code 011
                alphas[4] = (byte) (2 / 5 * alphas[0] + 3 / 5 * alphas[1]); // bit code 100
                alphas[5] = (byte) (1 / 5 * alphas[0] + 4 / 5 * alphas[1]); // bit code 101
                alphas[6] = (byte) (0);                                     // bit code 110
                alphas[7] = (byte) (255);                                   // bit code 111
            }

            return alphas;
        }
    }
}
