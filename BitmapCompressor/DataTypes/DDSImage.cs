using System;
using System.Drawing;
using BitmapCompressor.Extensions;
using BitmapCompressor.Formats;
using BitmapCompressor.Utilities;
using BitmapCompressor.Serialization;

namespace BitmapCompressor.DataTypes
{
    /// <summary>
    /// Represents a compressed DDS image.
    /// </summary>
    public class DDSImage : IProcessedImage
    {
        /// <summary>
        /// Instantiates a compressed DDS image with an empty main surface data
        /// buffer reserved for the given size.
        /// </summary>
        public DDSImage(int width, int height, int bufferSize)
        {
            Width = width;
            Height = height;
            Buffer = new byte[bufferSize];
        }

        /// <summary>
        /// Instantiates a compressed DDS image with the specified main surface data buffer.
        /// </summary>
        public DDSImage(int width, int height, byte[] buffer)
        {
            Width = width;
            Height = height;
            Buffer = buffer;
        }

        public int Width { get; }

        public int Height { get; }

        /// <summary>
        /// The main surface data of the image.
        /// </summary>
        public byte[] Buffer { get; }

        /// <summary>
        /// Reads the block-compressed data from the image's main surface buffer 
        /// for the specified 4x4 block coordinates.
        /// </summary>
        /// <param name="blockPosition">The coordinates for the 4x4 block to read the layout data for.</param>
        /// <param name="blockSize">The number of bytes the block consumes.</param>
        public byte[] ReadBlockData(Point blockPosition, int blockSize)
        {
            int numberOfHorizontalBlocks = Width / BlockFormat.Dimension;

            int blockIndex = PointUtility.ToRowMajor(blockPosition, numberOfHorizontalBlocks);
            int bufferIndex = blockIndex * blockSize;

            var blockData = Buffer.SubArray(bufferIndex, blockSize);

            return blockData;
        }

        /// <summary>
        /// Writes the block-compressed data to the image's main surface buffer 
        /// to the specified 4x4 block coordinates.
        /// </summary>
        /// <param name="blockData">The block-compressed data to write to the image buffer.</param>
        /// <param name="blockPosition">The coordinates for the 4x4 block the layout data represents.</param>
        public void WriteBlockData(byte[] blockData, Point blockPosition)
        {
            int blockSize = blockData.Length;
            int numberOfHorizontalBlocks = Width / BlockFormat.Dimension;

            int blockIndex = PointUtility.ToRowMajor(blockPosition, numberOfHorizontalBlocks);
            int bufferIndex = blockIndex * blockSize;

            Array.Copy(blockData, 0, Buffer, bufferIndex, blockSize);
        }

        public void Save(string fileName)
        {
            using (var writer = new DDSFileWriter(fileName))
            {
                writer.Write(this);
            }
        }

        public static DDSImage Load(string fileName)
        {
            using (var reader = new DDSFileReader(fileName))
            {
                return reader.Read();
            }
        }
    }
}
