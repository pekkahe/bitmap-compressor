using System;
using System.Drawing;
using BitmapCompressor.Extensions;
using BitmapCompressor.Formats;
using BitmapCompressor.Utilities;
using BitmapCompressor.Serialization;

namespace BitmapCompressor.DataTypes
{
    public sealed class DDSImage : ICompressedImage
    {
        private readonly byte[] _data;

        /// <summary>
        /// Instantiates a DDS image with an empty main surface data buffer.
        /// </summary>
        public DDSImage(int width, int height, int blockSize)
        {
            Width = width;
            Height = height;

            _data = new byte[CalculateBufferSize(blockSize)];
        }

        private int CalculateBufferSize(int blockSize)
        {
            int numberOfPixels = Width * Height;
            int numberOfRequiredBlocks = numberOfPixels / BlockFormat.PixelCount;

            return numberOfRequiredBlocks * blockSize;
        }

        /// <summary>
        /// Instantiates a DDS image with the specified main surface data buffer.
        /// </summary>
        public DDSImage(int width, int height, byte[] buffer)
        {
            Width = width;
            Height = height;
            _data = buffer;
        }

        public int Width { get; }

        public int Height { get; }

        public byte[] GetBuffer()
        {
            return _data;
        }
        
        public byte[] GetBlockData(Point block, int blockSize)
        {
            int numberOfHorizontalBlocks = Width / BlockFormat.Dimension;

            int blockIndex = PointUtility.ToRowMajor(block, numberOfHorizontalBlocks);
            int bufferIndex = blockIndex * blockSize;

            var blockData = _data.SubArray(bufferIndex, blockSize);

            return blockData;
        }
        
        public void SetBlockData(Point block, byte[] data)
        {
            int blockSize = data.Length;
            int numberOfHorizontalBlocks = Width / BlockFormat.Dimension;

            int blockIndex = PointUtility.ToRowMajor(block, numberOfHorizontalBlocks);
            int bufferIndex = blockIndex * blockSize;

            Array.Copy(data, 0, _data, bufferIndex, blockSize);
        }

        public void Save(string fileName)
        {
            using (var writer = new DDSFileWriter(fileName))
            {
                writer.Write(this);
            }
        }

        public static DDSImage FromFile(string fileName)
        {
            using (var reader = new DDSFileReader(fileName))
            {
                return reader.Read();
            }
        }
    }
}
