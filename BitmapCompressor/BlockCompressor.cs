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
        public ICompressedImage Compress(IUncompressedImage image, IBlockCompressionFormat format)
        {
            if (!AreDimensionsMultipleOfFour(image))
                throw new InvalidOperationException("Only textures with dimensions that are multiples of " +
                                                   $"{BlockFormat.Dimension} can be block compressed.");

            Logger.Default.Log("Compressing BMP to DDS.");

            var dds = DDSImage.CreateEmpty(image.Width, image.Height, format);

            int numberOfVerticalBlocks = image.Height / BlockFormat.Dimension;
            int numberOfHorizontalBlocks = image.Width / BlockFormat.Dimension;
            int numberOfBlocks = numberOfVerticalBlocks * numberOfHorizontalBlocks;

            Parallel.For(0, numberOfBlocks,
#if DEBUG
                RunInSingleThreadOption(),
#endif       
                (i) =>
            {
                var block = PointUtility.FromRowMajor(i, numberOfHorizontalBlocks);

                var colors = image.GetBlockPixels(block);

                var data = format.Compress(colors);

                dds.SetBlockData(block, data);
            });

            Logger.Default.Log("Compression successful.");

            return dds;
        }

        private static bool AreDimensionsMultipleOfFour(IImage image)
        {
            return image.Width != 0 && (image.Width % BlockFormat.Dimension) == 0 &&
                   image.Height != 0 && (image.Height % BlockFormat.Dimension) == 0;
        }

        public IUncompressedImage Decompress(ICompressedImage image)
        {
            Logger.Default.Log("Decompressing DDS to BMP.");

            var bmp = new DirectBitmap(image.Width, image.Height);
            var format = image.GetFormat();

            int numberOfVerticalBlocks = image.Height / BlockFormat.Dimension;
            int numberOfHorizontalBlocks = image.Width / BlockFormat.Dimension;
            int numberOfBlocks = numberOfVerticalBlocks * numberOfHorizontalBlocks;

            Parallel.For(0, numberOfBlocks,
#if DEBUG
                RunInSingleThreadOption(),
#endif
                (i) =>
            {
                var block = PointUtility.FromRowMajor(i, numberOfHorizontalBlocks);

                var data = image.GetBlockData(block, format.BlockSize);

                var colors = format.Decompress(data);

                bmp.SetBlockPixels(block, colors);
            });

            Logger.Default.Log("Decompression successful.");

            return bmp;
        }

        private static ParallelOptions RunInSingleThreadOption()
        {
            return new ParallelOptions { MaxDegreeOfParallelism = 1 };
        }
    }
}
