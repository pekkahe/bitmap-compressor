using System;
using System.Drawing;
using BitmapCompressor.Formats;

namespace BitmapCompressor.DataTypes
{
    public interface ICompressedImage : IImage
    {
        /// <summary>
        /// Returns the <see cref="IBlockCompressionFormat"/> which operates on this image.
        /// </summary>
        IBlockCompressionFormat GetFormat();

        /// <summary>
        /// Reads the block-compressed data from the image's main surface buffer 
        /// for the specified 4x4 block coordinates.
        /// </summary>
        /// <param name="block">The coordinates for the 4x4 block to read the layout data for.</param>
        byte[] GetBlockData(Point block);

        /// <summary>
        /// Writes the block-compressed data to the image's main surface buffer 
        /// to the specified 4x4 block coordinates.
        /// </summary>
        /// <param name="data">The block-compressed data to write to the image buffer.</param>
        /// <param name="block">The coordinates for the 4x4 block the layout data represents.</param>
        void SetBlockData(Point block, byte[] data);
    }
}
