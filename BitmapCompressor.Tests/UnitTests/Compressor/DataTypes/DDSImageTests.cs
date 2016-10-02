using System;
using System.Linq;
using System.Drawing;
using BitmapCompressor.Extensions;
using BitmapCompressor.DataTypes;
using BitmapCompressor.Formats;
using NUnit.Framework;

namespace BitmapCompressor.Tests.UnitTests.Compression.DataTypes
{
    [TestFixture(Category = "DataTypes")]
    public class DDSImageTests
    {
        private const int BlockSize = 8;
        private const int BlockDimension = 4;

        [Test]
        public void ReadDataForSoleBlock()
        {
            var buffer = Enumerable.Repeat((byte) 255, BlockSize).ToArray();
            var ddsImage = new DDSImage(BlockDimension, BlockDimension, buffer);

            var data = ddsImage.ReadBlockData(new Point(0, 0), BlockSize);

            CollectionAssert.AreEqual(buffer, data);
        }

        [Test]
        public void ReadDataForLastBlock()
        {
            const int numberOfBlocks = 4;

            var blockData = Enumerable.Repeat((byte) 200, BlockSize).ToArray();
            var buffer = new byte[numberOfBlocks * BlockSize];
            buffer.CopyFrom(blockData, buffer.Length - BlockSize);

            var ddsImage = new DDSImage(numberOfBlocks * BlockDimension, BlockDimension, buffer);

            var data = ddsImage.ReadBlockData(new Point(3, 0), BlockSize);

            CollectionAssert.AreEqual(blockData, data);
        }

        [Test]
        public void WriteDataForSoleBlock()
        {
            var buffer = Enumerable.Repeat((byte) 200, BlockSize).ToArray();
            var data = new BC1BlockLayout(buffer).GetBuffer();

            var ddsImage = new DDSImage(BlockDimension, BlockDimension, buffer.Length);

            ddsImage.WriteBlockData(data, new Point(0, 0));

            CollectionAssert.AreEqual(ddsImage.Buffer, buffer);
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

            int bufferSize = data1.Length + data2.Length + data3.Length + data4.Length;
            int dimension = 2 * BlockDimension;

            var ddsImage = new DDSImage(dimension, dimension, bufferSize);

            ddsImage.WriteBlockData(data1, new Point(0, 0));
            ddsImage.WriteBlockData(data2, new Point(1, 0));
            ddsImage.WriteBlockData(data3, new Point(0, 1));
            ddsImage.WriteBlockData(data4, new Point(1, 1));

            CollectionAssert.AreEqual(ddsImage.Buffer, buffer);
        }
    }
}
