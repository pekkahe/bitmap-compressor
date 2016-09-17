using System;
using System.Drawing;
using System.Diagnostics;
using BitmapCompressor.Extensions;

namespace BitmapCompressor.Formats
{
    /// <summary>
    /// Represents the BC1 format which compresses the 32-bit RGB colors of a texture's 4x4
    /// texel block into an 8-byte data buffer.
    /// </summary>
    /// <remarks><para>
    /// The BC1 algorithm creates a color table which contains four 16-bit colors, and maps
    /// the index of the closest matching color for each pixel in the block. Only the first
    /// two reference colors and the index map are stored and compressed.
    /// </para><para>
    /// The alpha channel of each ARGB color is not stored into the compressed data. BC1
    /// supports 1-bit per-pixel alpha (on/off) which is specified by altering the order
    /// of the two stored reference colors. On decompression, the ARGB color will be given
    /// either full transparency or opacity depending on its alpha value during compression.
    /// </para><para>
    /// BC1 (aka DXT1) algorithm resources:
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/bb694531(v=vs.85).aspx
    /// https://www.opengl.org/wiki/S3_Texture_Compression
    /// http://www.reedbeta.com/blog/2012/02/12/understanding-bcn-texture-compression-formats/
    /// </para></remarks>
    public class BC1CompressionFormat : IBlockCompressionFormat
    {
        public int BlockSize => BC1BlockLayout.ByteSize;

        public byte[] Compress(Color[] colors)
        {
            Debug.Assert(colors.Length == BlockFormat.PixelCount, 
                "Compressible ARGB color count should be {0} instead of {1}"
                .Parameters(BlockFormat.PixelCount, colors.Length));

            var colorTable = BC1ColorTable.FromArgb(colors);

            var block = new BC1BlockLayout();
            block.Color0 = colorTable[0];
            block.Color1 = colorTable[1];

            for (int i = 0; i < colors.Length; ++i)
            {
                block.ColorIndexes[i] = colorTable.IndexFor(colors[i]);
            }

            return block.GetBuffer();
        }

        public Color[] Decompress(byte[] blockData)
        {
            Debug.Assert(blockData.Length == BlockSize,
                "BC1 block size should be {0} instead of {1}"
                .Parameters(BlockSize, blockData.Length));

            var block = new BC1BlockLayout(blockData);

            var colorTable = new BC1ColorTable(block.Color0, block.Color1);

            var colors = new Color[BlockFormat.PixelCount];

            for (int i = 0; i < colors.Length; ++i)
            {
                colors[i] = colorTable.ColorFor(block.ColorIndexes[i]);
            }

            return colors;
        }
    }
}
