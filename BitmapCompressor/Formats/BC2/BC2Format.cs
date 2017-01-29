﻿using System;
using System.Drawing;
using System.Diagnostics;
using BitmapCompressor.DataTypes;
using BitmapCompressor.Utilities;

namespace BitmapCompressor.Formats
{
    /// <summary>
    /// Represents the BC2 format which compresses 4x4 pixel blocks into 16-byte data blocks.
    /// The format stores RGB data as two 16-bit colors and alpha as a separate 4-bit value
    /// for each pixel.
    /// </summary>
    /// <remarks><para>
    /// The BC2 format stores colors with the same number of bits and data layout as the
    /// <see cref="BC1Format"/>. However, BC2 stores 4-bit alpha for each color and requires
    /// an additional 64-bits of memory to store the alpha data.
    /// </para></remarks>
    public class BC2Format : IBlockCompressionFormat
    {
        public int BlockSize => BlockFormat.BC2ByteSize;

        public FourCC FourCC => FourCC.BC2Unorm;

        public byte[] Compress(Color[] colors)
        {
            Debug.Assert(colors.Length == BlockFormat.PixelCount);

            var colorSpace = new ColorSpace(colors);
            var colorTable = BC1ColorTable.Create(colorSpace.MaxColor, colorSpace.MinColor);

            var block = new BC2BlockData();
            block.Color0 = colorTable[0];
            block.Color1 = colorTable[1];

            for (int i = 0; i < colors.Length; ++i)
            {
                var color32 = colors[i];
                var color16 = ColorUtility.To16Bit(color32);

                block.ColorIndexes[i] = ColorUtility.GetIndexOfClosest(colorTable, color16);
                block.ColorAlphas[i]  = color32.A >> 4; // Convert 8-bit alpha to 4-bit
            }

            return block.ToBytes();
        }

        public Color[] Decompress(byte[] blockData)
        {
            Debug.Assert(blockData.Length == BlockFormat.BC2ByteSize);

            var block = BC2BlockData.FromBytes(blockData);

            var colorTable = BC1ColorTable.Create(block.Color0, block.Color1);

            var colors = new Color[BlockFormat.PixelCount];

            for (int i = 0; i < colors.Length; ++i)
            {
                int index = block.ColorIndexes[i];
                int alpha = block.ColorAlphas[i];

                // Convert 4-bit alpha to 8-bit by shifting the bits left and ORing
                // with the most significant bits to calculate the remaining values
                alpha = (alpha << 4) | (alpha & 0x0F);

                var color16 = colorTable[index];
                var color32 = Color.FromArgb(alpha, ColorUtility.To32Bit(color16));

                colors[i] = color32;
            }

            return colors;
        }
    }
}
