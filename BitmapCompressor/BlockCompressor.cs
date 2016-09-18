using System;
using System.Drawing;
using BitmapCompressor.DataTypes;
using BitmapCompressor.Diagnostics;
using BitmapCompressor.Formats;

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

        public IProcessedImage Compress(BMPImage image)
        {
            if (!AreDimensionsMultipleOfFour(image))
                throw new InvalidOperationException("Only textures with dimensions that are multiples of " +
                                                   $"{BlockFormat.Dimension} can be block compressed.");

            Logger.Default.Log("Compressing BMP to DDS.");

            var ddsImage = new DDSImage(image.Width, image.Height, CalculateBufferSize(image));

            int numberOfVerticalBlocks = image.Height / BlockFormat.Dimension;
            int numberOfHorizontalBlocks = image.Width / BlockFormat.Dimension;

            for (int y = 0; y < numberOfVerticalBlocks; ++y)
            {
                for (int x = 0; x < numberOfHorizontalBlocks; ++x)
                {
                    var colors = image.GetColors(new Point(x, y));

                    var data = _compressionFormat.Compress(colors);

                    ddsImage.WriteBlockData(data, new Point(x, y));
                }
            }

            Logger.Default.Log("Compression successful.");

            return ddsImage;
        }

        private int CalculateBufferSize(BMPImage image)
        {
            int numberOfPixels = image.Height * image.Width;
            int numberOfRequiredBlocks = numberOfPixels / BlockFormat.PixelCount;

            return numberOfRequiredBlocks * _compressionFormat.BlockSize;
        }

        private bool AreDimensionsMultipleOfFour(BMPImage image)
        {
            return image.Width != 0 && (image.Width % BlockFormat.Dimension) == 0 &&
                   image.Height != 0 && (image.Height % BlockFormat.Dimension) == 0;
        }

        public IProcessedImage Decompress(DDSImage image)
        {
            Logger.Default.Log("Decompressing DDS to BMP.");

            var bmpImage = new BMPImage(image.Width, image.Height);

            int numberOfVerticalBlocks = image.Height / BlockFormat.Dimension;
            int numberOfHorizontalBlocks = image.Width / BlockFormat.Dimension;

            for (int y = 0; y < numberOfVerticalBlocks; ++y)
            {
                for (int x = 0; x < numberOfHorizontalBlocks; ++x)
                {
                    var data = image.ReadBlockData(new Point(x, y), _compressionFormat.BlockSize);

                    var colors = _compressionFormat.Decompress(data);

                    bmpImage.SetColors(colors, new Point(x, y));
                }
            }

            Logger.Default.Log("Decompression successful.");

            return bmpImage;
        }
    }
}
