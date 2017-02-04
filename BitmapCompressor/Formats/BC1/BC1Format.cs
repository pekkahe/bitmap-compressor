using System;
using System.Drawing;
using System.Diagnostics;
using BitmapCompressor.DataTypes;
using BitmapCompressor.Utilities;
using BitmapCompressor.Extensions;

namespace BitmapCompressor.Formats
{
    /// <summary>
    /// The BC1 format compresses 4x4 texel areas into 8-byte data blocks. The format
    /// stores RGB data as two 16-bit colors and supports 1-bit alpha for each texel.
    /// </summary>
    /// <remarks><para>
    /// The BC1 format creates a color table which contains four 16-bit colors and
    /// stores the table index of the closest matching color of each texel in the block.
    /// The first two colors and the index map are stored in the compressed data while
    /// the remaining two colors are interpolated between the two reference colors.
    /// </para><para>
    /// The alpha channel of each color is not stored in the compressed data. BC1 supports
    /// 1-bit per-texel alpha by altering the layout order of the two reference colors in 
    /// the compressed data.
    /// </para></remarks>
    public class BC1Format : IBlockCompressionFormat
    {
        /// <summary>
        /// The third BC1 color index which is also used to mark transparency in 1-bit alpha.
        /// </summary>
        private const int AlphaColorIndex = 0x3;

        public int BlockSize => BlockFormat.BC1ByteSize;

        public FourCC FourCC => FourCC.BC1Unorm;

        public byte[] Compress(Color[] colors)
        {
            Debug.Assert(colors.Length == BlockFormat.TexelCount);
            
            var colorTable = CreateColorTable(colors);

            var block = new BC1BlockData();
            block.Color0 = colorTable[0];
            block.Color1 = colorTable[1];

            for (int i = 0; i < colors.Length; ++i)
            {
                var color32 = colors[i];

                // If color has alpha, use a specific index 
                // to identify the color when decompressed
                block.ColorIndexes[i] = color32.HasAlpha() ? AlphaColorIndex :
                    ColorUtility.GetIndexOfClosest(colorTable, ColorUtility.To16Bit(color32));
            }

            return block.ToBytes();
        }

        public Color[] Decompress(byte[] blockData)
        {
            Debug.Assert(blockData.Length == BlockFormat.BC1ByteSize);

            var block = BC1BlockData.FromBytes(blockData);

            var colorTable  = CreateColorTable(block.Color0, block.Color1);
            var is1BitAlpha = colorTable[0].Value <= colorTable[1].Value;

            var colors = new Color[BlockFormat.TexelCount];

            for (int i = 0; i < colors.Length; ++i)
            {
                int index = block.ColorIndexes[i];
                int alpha = is1BitAlpha && index == AlphaColorIndex ? 0 : 255;

                var color16 = colorTable[index];
                var color32 = Color.FromArgb(alpha, ColorUtility.To32Bit(color16));

                colors[i] = color32;
            }

            return colors;
        }

        /// <summary>
        /// Creates a BC1 color table from the 32-bit colors of a block. If the color space
        /// contains alpha values, the color table is built according to BC1 1-bit alpha
        /// specification.
        /// </summary>
        private static Color565[] CreateColorTable(Color[] colors)
        {
            var colorSpace = new ColorSpace(colors);

            return colorSpace.HasAlpha?
                BC1ColorTable.CreateFor1BitAlpha(colorSpace.MinColor, colorSpace.MaxColor) :
                BC1ColorTable.Create(colorSpace.MaxColor, colorSpace.MinColor);
        }

        /// <summary>
        /// Creates a BC1 color table from two compressed 16-bit reference colors.
        /// If the integer value of <paramref name="color0"/> is lower or equal to
        /// that of <paramref name="color1"/>, the color table is built according
        /// to the 1-bit alpha specification.
        /// </summary>
        private static Color565[] CreateColorTable(Color565 color0, Color565 color1)
        {
            return color0.Value <= color1.Value ?
                BC1ColorTable.CreateFor1BitAlpha(color0, color1) :
                BC1ColorTable.Create(color0, color1);
        }
    }
}
