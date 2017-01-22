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
        private readonly byte[] _surfaceData;
        private readonly IBlockCompressionFormat _format;

        private DDSImage(int width, int height, byte[] surfaceData, IBlockCompressionFormat format)
        {
            Width = width;
            Height = height;
            _surfaceData = surfaceData;
            _format = format;
        }
        
        public int Width { get; }

        public int Height { get; }

        public byte[] GetBuffer() => _surfaceData;

        public IBlockCompressionFormat GetFormat() => _format;
        
        public byte[] GetBlockData(Point block, int blockSize)
        {
            int numberOfHorizontalBlocks = Width / BlockFormat.Dimension;

            int blockIndex = PointUtility.ToRowMajor(block, numberOfHorizontalBlocks);
            int bufferIndex = blockIndex * blockSize;

            var blockData = _surfaceData.SubArray(bufferIndex, blockSize);

            return blockData;
        }
        
        public void SetBlockData(Point block, byte[] data)
        {
            int blockSize = data.Length;
            int numberOfHorizontalBlocks = Width / BlockFormat.Dimension;

            int blockIndex = PointUtility.ToRowMajor(block, numberOfHorizontalBlocks);
            int bufferIndex = blockIndex * blockSize;

            Array.Copy(data, 0, _surfaceData, bufferIndex, blockSize);
        }

        public void Save(string fileName)
        {
            using (var writer = new DDSFileWriter(fileName))
            {
                writer.Write(this);
            }
        }

        /// <summary>
        /// Instantiates a <see cref="DDSImage"/> with an empty main surface data
        /// buffer reserved for the size of the specified dimensions.
        /// </summary>
        /// <param name="width">The pixel width of the image.</param>
        /// <param name="height">The pixel height of the image.</param>
        /// <param name="format">The block compression algorithm for the DDS image.</param>
        public static DDSImage CreateEmpty(int width, int height, IBlockCompressionFormat format)
        {
            int numberOfPixels = width * height;
            int numberOfRequiredBlocks = numberOfPixels / BlockFormat.PixelCount;
            int bufferSize = numberOfRequiredBlocks * format.BlockSize;

            return new DDSImage(width, height, new byte[bufferSize], format);
        }

        /// <summary>
        /// Instantiates a <see cref="DDSImage"/> with the specified dimensions
        /// and raw main surface data. 
        /// </summary>
        /// <param name="width">The pixel width of the image.</param>
        /// <param name="height">The pixel height of the image.</param>
        /// <param name="data">The main surface data of the DDS image.</param>
        /// <param name="format">The block compression algorithm for the DDS image.</param>
        public static DDSImage CreateFromData(int width, int height, byte[] data, IBlockCompressionFormat format)
        {
            return new DDSImage(width, height, data, format);
        }

        /// <summary>
        /// Instantiates a <see cref="DDSImage"/> from the data of the specified DDS file.
        /// </summary>
        /// <param name="filePath">The file path of the DDS file to read.</param>
        public static DDSImage CreateFromFile(string filePath)
        {
            using (var reader = new DDSFileReader(filePath))
            {
                return reader.Read();
            }
        }
    }
}
