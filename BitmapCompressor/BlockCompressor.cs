using System;
using System.Drawing;
using System.Threading.Tasks;
using BitmapCompressor.DataTypes;
using BitmapCompressor.Diagnostics;
using BitmapCompressor.Formats;
using BitmapCompressor.Utilities;

namespace BitmapCompressor
{
    /// <summary>
    /// Compresses images by breaking the texture into 4x4 blocks and applying
    /// a block compression algorithm to each block in row-major order.
    /// </summary>
    public class BlockCompressor : IBlockCompressor
    {
        private readonly IBlockCompressionFormat _compressionFormat;

        /// <summary>
        /// Instantiates a new block compressor for a specific compression format.
        /// </summary>
        /// <param name="format">The block compression format this compressor uses.</param>
        public BlockCompressor(IBlockCompressionFormat format)
        {
            _compressionFormat = format;
        }

        public ICompressedImage Compress(IUncompressedImage image)
        {
            if (!AreDimensionsMultipleOfFour(image))
                throw new InvalidOperationException("Only textures with dimensions that are multiples of " +
                                                   $"{BlockFormat.Dimension} can be block compressed.");

            Logger.Default.Log("Compressing BMP to DDS.");

            var dds = new DDSImage(image.Width, image.Height, _compressionFormat.BlockSize);

            int numberOfVerticalBlocks = image.Height / BlockFormat.Dimension;
            int numberOfHorizontalBlocks = image.Width / BlockFormat.Dimension;
            int numberOfBlocks = numberOfVerticalBlocks * numberOfHorizontalBlocks;

            Parallel.For(0, numberOfBlocks, (i) =>
            {
                var block = PointUtility.FromRowMajor(i, numberOfHorizontalBlocks);

                var colors = image.GetBlockPixels(block);

                var data = _compressionFormat.Compress(colors);

                dds.SetBlockData(block, data);
            });

            Logger.Default.Log("Compression successful.");

            return dds;
        }

        private bool AreDimensionsMultipleOfFour(IImage image)
        {
            return image.Width != 0 && (image.Width % BlockFormat.Dimension) == 0 &&
                   image.Height != 0 && (image.Height % BlockFormat.Dimension) == 0;
        }

        public IUncompressedImage Decompress(ICompressedImage image)
        {
            Logger.Default.Log("Decompressing DDS to BMP.");

            var bmp = new DirectBitmap(image.Width, image.Height);

            int numberOfVerticalBlocks = image.Height / BlockFormat.Dimension;
            int numberOfHorizontalBlocks = image.Width / BlockFormat.Dimension;
            int numberOfBlocks = numberOfVerticalBlocks * numberOfHorizontalBlocks;

            Parallel.For(0, numberOfBlocks, (i) =>
            {
                var block = PointUtility.FromRowMajor(i, numberOfHorizontalBlocks);

                var data = image.GetBlockData(block, _compressionFormat.BlockSize);

                var colors = _compressionFormat.Decompress(data);

                bmp.SetBlockPixels(block, colors);
            });

            Logger.Default.Log("Decompression successful.");

            return bmp;
        }
    }
}
