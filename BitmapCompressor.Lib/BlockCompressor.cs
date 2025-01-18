global using System;
global using System.Linq;
global using System.Collections;
global using System.Collections.Generic;
global using System.Diagnostics;
global using System.Drawing;

using System.Threading.Tasks;
using BitmapCompressor.Lib.DataTypes;
using BitmapCompressor.Lib.Diagnostics;
using BitmapCompressor.Lib.Formats;
using BitmapCompressor.Lib.Utilities;

namespace BitmapCompressor.Lib;

/// <summary>
/// Compresses images by breaking the texture into 4x4 blocks and applying
/// a block compression algorithm to each block in row-major order.
/// </summary>
public class BlockCompressor : IBlockCompressor
{
    /// <summary>
    /// Option to run parallelized loops in a single thread for easier debugging.
    /// </summary>
    private static readonly ParallelOptions RunInSingleThread = 
        new ParallelOptions { MaxDegreeOfParallelism = 1 };

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
            RunInSingleThread,
#endif       
            (i) =>
        {
            var blockIndex = PointUtility.FromRowMajor(i, numberOfHorizontalBlocks);
            var blockColors = image.GetBlockColors(blockIndex);
            var blockData = format.Compress(blockColors);

            dds.SetBlockData(blockIndex, blockData);
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

        int numberOfVerticalBlocks = image.Height / BlockFormat.Dimension;
        int numberOfHorizontalBlocks = image.Width / BlockFormat.Dimension;
        int numberOfBlocks = numberOfVerticalBlocks * numberOfHorizontalBlocks;

        Parallel.For(0, numberOfBlocks,
#if DEBUG
            RunInSingleThread,
#endif
            (i) =>
        {
            var blockIndex = PointUtility.FromRowMajor(i, numberOfHorizontalBlocks);
            var blockData = image.GetBlockData(blockIndex);
            var blockColors = image.CompressionFormat.Decompress(blockData);

            bmp.SetBlockColors(blockIndex, blockColors);
        });

        Logger.Default.Log("Decompression successful.");

        return bmp;
    }
}
