using System;
using System.Drawing;
using System.Diagnostics;
using BitmapCompressor.DataTypes;
using BitmapCompressor.Utilities;
using BitmapCompressor.Extensions;

namespace BitmapCompressor.Formats
{
    /// <summary>
    /// Represents the BC2 format which compresses 4x4 pixel blocks into 16-byte data blocks.
    /// The format stores RGB data as two 16-bit colors and alpha as a separate 4-bit value
    /// for each pixel.
    /// </summary>
    /// <remarks>
    /// The BC2 format stores colors with the same number of bits and data layout as the
    /// <see cref="BC1Format"/>. However, BC2 stores 4-bit alpha for each color and requires
    /// an additional 64-bits of memory to store the alpha data.
    /// </remarks>
    public class BC2Format : IBlockCompressionFormat
    {
        public int BlockSize => BlockFormat.BC2ByteSize;

        public byte[] Compress(Color[] colors)
        {
            Debug.Assert(colors.Length == BlockFormat.PixelCount,
                "Mismatching number of colors for block compression.");

            var colorSpace = new ColorSpace(colors);
            var colorTable = BC1Format.CreateColorTable(colorSpace.MinColor, colorSpace.MaxColor);

            var colorIndexes = new int[colors.Length];
            var colorAlphas = new byte[colors.Length];

            for (int i = 0; i < colors.Length; ++i)
            {
                var color32 = colors[i];
                var color16 = ColorUtility.To16Bit(color32);

                colorIndexes[i] = ColorUtility.GetIndexOfClosest(colorTable, color16);
                // Shift 8-bit alpha to 4-bits by taking away least significant bits
                colorAlphas[i]  = (byte) ((color32.A >> 4) & 0x00FF);
            }

            return CreateBC2Block(colorTable[0], colorTable[1], colorIndexes, colorAlphas);
        }

        /// <summary>
        /// Creates a 16-byte BC2 compressed data block from two 16-bit reference colors
        /// and two arrays mapping a color table index and 4-bit alpha value, respectively,
        /// to each pixel of the block.
        /// </summary>
        /// <param name="color0">The first compressed 16-bit reference color.</param>
        /// <param name="color1">The second compressed 16-bit reference color.</param>
        /// <param name="colorIndexes">An array of 16 color indexes, indexed by pixel index.</param>
        /// <param name="colorAlphas">An array of 16 alpha values, indexed by pixel index.</param>
        private byte[] CreateBC2Block(Color565 color0, Color565 color1, 
            int[] colorIndexes, byte[] colorAlphas)
        {
            byte c0Low      = (byte) ((color0.Value & 0x00FF) >> 0);
            byte c0Hi       = (byte) ((color0.Value & 0xFF00) >> 8);
            byte c1Low      = (byte) ((color1.Value & 0x00FF) >> 0);
            byte c1Hi       = (byte) ((color1.Value & 0xFF00) >> 8);

            byte[,] alphas  = new byte[4, 2];
            byte[] codes    = new byte[4];

            for (int p = 0, row = 0; p < BlockFormat.PixelCount; p += BlockFormat.Dimension, ++row)
            {
                int a = p;
                int b = p + 1;
                int c = p + 2;
                int d = p + 3;

                alphas[row, 0]  = (byte) ((colorAlphas[d] << 4) | colorAlphas[c]);
                alphas[row, 1]  = (byte) ((colorAlphas[b] << 4) | colorAlphas[a]);

                codes[row]      = (byte) (colorIndexes[a] |
                                         (colorIndexes[b] << 2) |
                                         (colorIndexes[c] << 4) |
                                         (colorIndexes[d] << 6));
            }    

            var blockData = new byte[16];

            blockData[0]    = alphas[0, 0];
            blockData[1]    = alphas[0, 1];
            blockData[2]    = alphas[1, 0];
            blockData[3]    = alphas[1, 1];
            blockData[4]    = alphas[2, 0];
            blockData[5]    = alphas[2, 1];
            blockData[6]    = alphas[3, 0];
            blockData[7]    = alphas[3, 1];
            blockData[8]    = c0Low;
            blockData[9]    = c0Hi;
            blockData[10]   = c1Low;
            blockData[11]   = c1Hi;
            blockData[12]   = codes[0];
            blockData[13]   = codes[1];
            blockData[14]   = codes[2];
            blockData[15]   = codes[3];

            return blockData;
        }

        public Color[] Decompress(byte[] blockData)
        {
            Debug.Assert(blockData.Length == BlockFormat.BC2ByteSize,
                "Mismatching number of bytes for BC2 format.");

            byte[,] alphas  = new byte[4, 2];
            alphas[0, 0]    = blockData[0];
            alphas[0, 1]    = blockData[1];
            alphas[1, 0]    = blockData[2];
            alphas[1, 1]    = blockData[3];
            alphas[2, 0]    = blockData[4];
            alphas[2, 1]    = blockData[5];
            alphas[3, 0]    = blockData[6];
            alphas[3, 1]    = blockData[7];
            byte c0Low      = blockData[8];
            byte c0Hi       = blockData[9];
            byte c1Low      = blockData[10];
            byte c1Hi       = blockData[11];
            byte[] codes    = new byte[4];
            codes[0]        = blockData[12];
            codes[1]        = blockData[13];
            codes[2]        = blockData[14];
            codes[3]        = blockData[15];

            var colorIndexes    = new int[BlockFormat.PixelCount];
            var colorAlphas     = new byte[BlockFormat.PixelCount];

            for (int p = 0, row = 0; p < BlockFormat.PixelCount; p += BlockFormat.Dimension, ++row)
            {
                int a = p;
                int b = p + 1;
                int c = p + 2;
                int d = p + 3;

                colorIndexes[a] =  codes[row]       & 0x03;
                colorIndexes[b] = (codes[row] >> 2) & 0x03;
                colorIndexes[c] = (codes[row] >> 4) & 0x03;
                colorIndexes[d] = (codes[row] >> 6) & 0x03;

                colorAlphas[a]  = (byte)  (alphas[row, 1] & 0x00FF);
                colorAlphas[b]  = (byte) ((alphas[row, 1] & 0xFF00) >> 4);
                colorAlphas[c]  = (byte)  (alphas[row, 0] & 0x00FF);
                colorAlphas[d]  = (byte) ((alphas[row, 0] & 0xFF00) >> 4);
            }

            var colors      = new Color[BlockFormat.PixelCount];
            var color0      = Color565.FromValue((ushort) ((c0Hi << 8) | c0Low));
            var color1      = Color565.FromValue((ushort) ((c1Hi << 8) | c1Low));
            var colorTable  = BC1Format.CreateColorTable(color0, color1);

            for (int i = 0; i < colors.Length; ++i)
            {
                int index = colorIndexes[i];
                int alpha = colorAlphas[i];

                var color16 = colorTable[index];
                var color32 = Color.FromArgb(alpha, ColorUtility.To32Bit(color16));

                colors[i] = color32;
            }

            return colors;
        }
    }
}
