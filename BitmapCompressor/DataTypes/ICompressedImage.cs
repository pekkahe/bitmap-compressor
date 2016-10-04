using System;
using System.Drawing;

namespace BitmapCompressor.DataTypes
{
    public interface ICompressedImage : IImage
    {
        /// <summary>
        /// Reads the block-compressed data from the image's main surface buffer 
        /// for the specified 4x4 block coordinates.
        /// </summary>
        /// <param name="block">The coordinates for the 4x4 block to read the layout data for.</param>
        /// <param name="blockSize">The number of bytes the block consumes.</param>
        byte[] GetBlockData(Point block, int blockSize);

        /// <summary>
        /// Writes the block-compressed data to the image's main surface buffer 
        /// to the specified 4x4 block coordinates.
        /// </summary>
        /// <param name="data">The block-compressed data to write to the image buffer.</param>
        /// <param name="block">The coordinates for the 4x4 block the layout data represents.</param>
        void SetBlockData(Point block, byte[] data);
    }
}
