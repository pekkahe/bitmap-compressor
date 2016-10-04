using System;
using System.Linq;
using System.Drawing;
using BitmapCompressor.Extensions;
using BitmapCompressor.DataTypes;
using BitmapCompressor.Formats;
using NUnit.Framework;

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
            const int byteCount = 8;
            const int blockCount = 4;
            const int width = 2 * BlockFormat.Dimension;  // Two horizontal and
            const int height = 2 * BlockFormat.Dimension; // vertical blocks

            var dds = new DDSImage(width, height, BC1BlockLayout.ByteSize);

            Assert.AreEqual(width, dds.Width);
            Assert.AreEqual(height, dds.Height);
            Assert.AreEqual(blockCount * byteCount, dds.GetBuffer().Length);
        }

        [Test]
        public void ReadDataForSoleBlock()
        {
            var buffer = Enumerable.Repeat((byte) 255, BlockSize).ToArray();
            var ddsImage = new DDSImage(BlockDimension, BlockDimension, buffer);

            var data = ddsImage.GetBlockData(new Point(0, 0), BlockSize);

            CollectionAssert.AreEqual(buffer, data);
        }

        [Test]
        public void ReadDataForLastBlock()
        {
            const int numberOfBlocks = 4;

            var bytes = Enumerable.Repeat((byte) 200, BlockSize).ToArray();
            var buffer = new byte[numberOfBlocks * BlockSize];
            buffer.CopyFrom(bytes, buffer.Length - BlockSize);

            var dds = new DDSImage(numberOfBlocks * BlockDimension, BlockDimension, buffer);

            var blockData = dds.GetBlockData(new Point(3, 0), BlockSize);

            CollectionAssert.AreEqual(bytes, blockData);
        }

        [Test]
        public void WriteDataForSoleBlock()
        {
            var bytes = Enumerable.Repeat((byte) 200, BlockSize).ToArray();
            var blockData = new BC1BlockLayout(bytes).GetBuffer();

            var dds = new DDSImage(BlockDimension, BlockDimension, BC1BlockLayout.ByteSize);

            dds.SetBlockData(new Point(0, 0), blockData);

            CollectionAssert.AreEqual(dds.GetBuffer(), bytes);
        }

        [Test]
        public void WriteDataForMultipleBlocks()
        {
            const int numberOfBlocks = 4;
            
            var block1 = Enumerable.Repeat((byte) 50, BlockSize).ToArray();
            var block2 = Enumerable.Repeat((byte) 100, BlockSize).ToArray();
            var block3 = Enumerable.Repeat((byte) 150, BlockSize).ToArray();
            var block4 = Enumerable.Repeat((byte) 200, BlockSize).ToArray();

            var buffer = new byte[numberOfBlocks * BlockSize];
            Array.Copy(block1, 0, buffer, 0, BlockSize);
            Array.Copy(block2, 0, buffer, 8, BlockSize);
            Array.Copy(block3, 0, buffer, 16, BlockSize);
            Array.Copy(block4, 0, buffer, 24, BlockSize);

            var data1 = new BC1BlockLayout(block1).GetBuffer();
            var data2 = new BC1BlockLayout(block2).GetBuffer();
            var data3 = new BC1BlockLayout(block3).GetBuffer();
            var data4 = new BC1BlockLayout(block4).GetBuffer();

            //int bufferSize = data1.Length + data2.Length + data3.Length + data4.Length;
            int widthAndHeight = 2 * BlockDimension;

            var ddsImage = new DDSImage(widthAndHeight, widthAndHeight, BC1BlockLayout.ByteSize);

            ddsImage.SetBlockData(new Point(0, 0), data1);
            ddsImage.SetBlockData(new Point(1, 0), data2);
            ddsImage.SetBlockData(new Point(0, 1), data3);
            ddsImage.SetBlockData(new Point(1, 1), data4);

            CollectionAssert.AreEqual(ddsImage.GetBuffer(), buffer);
        }
    }
}
