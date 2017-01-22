using System;
using System.Linq;
using System.Drawing;
using BitmapCompressor.Extensions;
using BitmapCompressor.DataTypes;
using BitmapCompressor.Formats;
using NUnit.Framework;
using Moq;

namespace BitmapCompressor.Tests.UnitTests.Compressor.DataTypes
{
    [TestFixture(Category = "DataTypes")]
    public class DDSImageTests
    {
        private const int BlockSize = 8;
        private const int BlockDimension = 4;

        [Test]
        public void ConstructionCalculatesBufferSizeWhenBC1()
        {
            var format = new Mock<IBlockCompressionFormat>();
            format.Setup(f => f.BlockSize).Returns(BlockFormat.BC1ByteSize);

            const int byteCount = 8;
            const int blockCount = 4;
            const int width = 2 * BlockFormat.Dimension;  // Two horizontal and
            const int height = 2 * BlockFormat.Dimension; // vertical blocks

            var dds = DDSImage.CreateEmpty(width, height, format.Object);

            Assert.AreEqual(width, dds.Width);
            Assert.AreEqual(height, dds.Height);
            Assert.AreEqual(blockCount * byteCount, dds.GetBuffer().Length);
        }

        [Test]
        public void ReadDataForSoleBlock()
        {
            var format = new Mock<IBlockCompressionFormat>();

            var buffer = Enumerable.Repeat((byte) 255, BlockSize).ToArray();
            var ddsImage = DDSImage.CreateFromData(BlockDimension, BlockDimension, buffer, format.Object);

            var data = ddsImage.GetBlockData(new Point(0, 0), BlockSize);

            CollectionAssert.AreEqual(buffer, data);
        }

        [Test]
        public void ReadDataForLastBlock()
        {
            const int numberOfBlocks = 4;

            var format = new Mock<IBlockCompressionFormat>();

            var bytes = Enumerable.Repeat((byte) 200, BlockSize).ToArray();
            var buffer = new byte[numberOfBlocks * BlockSize];
            buffer.CopyFrom(bytes, buffer.Length - BlockSize);

            var dds = DDSImage.CreateFromData(numberOfBlocks * BlockDimension, BlockDimension, buffer, format.Object);

            var blockData = dds.GetBlockData(new Point(3, 0), BlockSize);

            CollectionAssert.AreEqual(bytes, blockData);
        }

        [Test]
        public void WriteDataForSoleBlock()
        {
            var format = new Mock<IBlockCompressionFormat>();
            format.Setup(f => f.BlockSize).Returns(BlockFormat.BC1ByteSize);

            var bytes = Enumerable.Repeat((byte) 200, BlockSize).ToArray();
            var block = BC1BlockData.FromBytes(bytes);

            var dds = DDSImage.CreateEmpty(BlockDimension, BlockDimension, format.Object);

            dds.SetBlockData(new Point(0, 0), block.ToBytes());

            CollectionAssert.AreEqual(dds.GetBuffer(), bytes);
        }

        [Test]
        public void WriteDataForMultipleBlocks()
        {
            const int numberOfBlocks = 4;

            var format = new Mock<IBlockCompressionFormat>();
            format.Setup(f => f.BlockSize).Returns(BlockFormat.BC1ByteSize);

            var block1 = Enumerable.Repeat((byte) 50, BlockSize).ToArray();
            var block2 = Enumerable.Repeat((byte) 100, BlockSize).ToArray();
            var block3 = Enumerable.Repeat((byte) 150, BlockSize).ToArray();
            var block4 = Enumerable.Repeat((byte) 200, BlockSize).ToArray();

            var buffer = new byte[numberOfBlocks * BlockSize];
            Array.Copy(block1, 0, buffer, 0, BlockSize);
            Array.Copy(block2, 0, buffer, 8, BlockSize);
            Array.Copy(block3, 0, buffer, 16, BlockSize);
            Array.Copy(block4, 0, buffer, 24, BlockSize);

            var data1 = BC1BlockData.FromBytes(block1).ToBytes();
            var data2 = BC1BlockData.FromBytes(block2).ToBytes();
            var data3 = BC1BlockData.FromBytes(block3).ToBytes();
            var data4 = BC1BlockData.FromBytes(block4).ToBytes();

            int widthAndHeight = 2 * BlockDimension;

            var ddsImage = DDSImage.CreateEmpty(widthAndHeight, widthAndHeight, format.Object);

            ddsImage.SetBlockData(new Point(0, 0), data1);
            ddsImage.SetBlockData(new Point(1, 0), data2);
            ddsImage.SetBlockData(new Point(0, 1), data3);
            ddsImage.SetBlockData(new Point(1, 1), data4);

            CollectionAssert.AreEqual(ddsImage.GetBuffer(), buffer);
        }
    }
}
