using System;
using System.Linq;
using System.Drawing;
using BitmapCompressor.Formats;
using BitmapCompressor.DataTypes;
using Moq;
using NUnit.Framework;

namespace BitmapCompressor.Tests.UnitTests.Compressor
{
    [TestFixture(Category = "Compressor")]
    public class BlockCompressorTests
    {
        #region Test colors

        private static readonly Color Red =     Color.FromArgb(255, 0, 0);
        private static readonly Color Green =   Color.FromArgb(0, 255, 0);
        private static readonly Color Blue =    Color.FromArgb(0, 0, 255);
        private static readonly Color White =   Color.FromArgb(255, 255, 255);
        private static readonly Color Black =   Color.FromArgb(0, 0, 0);
        private static readonly Color Gray =    Color.FromArgb(150, 150, 150);
        
        #endregion

        [Test]
        public void CompressionThrowsExceptionWhenImageHasInvalidDimensions()
        {
            var compressor = new BlockCompressor();
            var format = new Mock<IBlockCompressionFormat>();
            
            var bitmap = new Mock<IUncompressedImage>();
            bitmap.Setup(b => b.Width).Returns(10);
            bitmap.Setup(b => b.Height).Returns(16);

            Assert.Throws<InvalidOperationException>(() => compressor.Compress(bitmap.Object, format.Object));
        }

        [Test]
        public void RunsCompressionForEachBlock()
        {
            const int byteCount = 8;

            var format = new Mock<IBlockCompressionFormat>();
            format.Setup(f => f.BlockSize).Returns(byteCount);
            format.Setup(f => f.Compress(It.IsAny<Color[]>())).Returns(new byte[byteCount]);

            var compressor = new BlockCompressor();

            compressor.Compress(CreateBitmapMock(), format.Object);

            format.Verify(f => f.Compress(It.IsAny<Color[]>()), Times.Exactly(6));
            format.Verify(f => f.Compress(It.Is<Color[]>(colors => colors.All(c => c == Red))));
            format.Verify(f => f.Compress(It.Is<Color[]>(colors => colors.All(c => c == Green))));
            format.Verify(f => f.Compress(It.Is<Color[]>(colors => colors.All(c => c == Blue))));
            format.Verify(f => f.Compress(It.Is<Color[]>(colors => colors.All(c => c == White))));
            format.Verify(f => f.Compress(It.Is<Color[]>(colors => colors.All(c => c == Black))));
            format.Verify(f => f.Compress(It.Is<Color[]>(colors => colors.All(c => c == Gray))));
        }

        /// <summary>
        /// Creates a <see cref="IUncompressedImage"/> mock of 3x2 blocks (48 texels)
        /// which returns a different color for each block.
        /// </summary>
        /// <remarks>
        /// Block colors:
        ///  _____  _____  _____
        /// |     ||     ||     | 
        /// | red ||green||blue |
        /// |_____||_____||_____|
        /// |     ||     ||     |
        /// |white||black||gray |
        /// |_____||_____||_____|
        /// </remarks>
        private static IUncompressedImage CreateBitmapMock()
        {
            var blockColors = new[]
            {
                new [] { Red,   Green, Blue },
                new [] { White, Black, Gray },
            };

            const int numberOfHorizontalBlocks = 3;
            const int numberOfVerticalBlocks = 2;

            var mock = new Mock<IUncompressedImage>();
            mock.Setup(m => m.Width).Returns(numberOfHorizontalBlocks * BlockFormat.Dimension);
            mock.Setup(m => m.Height).Returns(numberOfVerticalBlocks * BlockFormat.Dimension);

            for (int y = 0; y < numberOfVerticalBlocks; ++y)
            {
                for (int x = 0; x < numberOfHorizontalBlocks; ++x)
                {
                    var color = blockColors[y][x];
                    var colors = Enumerable.Repeat(color, BlockFormat.TexelCount).ToArray();

                    mock.Setup(m => m.GetBlockColors(It.Is<Point>(p => p == new Point(x, y)))).Returns(colors);
                }
            }

            return mock.Object;
        }

        [Test]
        public void RunsDecompressionForEachBlock()
        {
            const int byteCount = 8;

            var format = new Mock<IBlockCompressionFormat>();
            format.Setup(f => f.BlockSize).Returns(byteCount);
            format.Setup(f => f.Decompress(It.IsAny<byte[]>())).Returns(new Color[BlockFormat.TexelCount]);

            var compressor = new BlockCompressor();

            compressor.Decompress(CreateDDSImageMock(format.Object));

            format.Verify(f => f.Decompress(It.IsAny<byte[]>()), Times.Exactly(6));
            format.Verify(f => f.Decompress(It.Is<byte[]>(bytes => bytes.All(b => b == 1))));
            format.Verify(f => f.Decompress(It.Is<byte[]>(bytes => bytes.All(b => b == 2))));
            format.Verify(f => f.Decompress(It.Is<byte[]>(bytes => bytes.All(b => b == 3))));
            format.Verify(f => f.Decompress(It.Is<byte[]>(bytes => bytes.All(b => b == 4))));
            format.Verify(f => f.Decompress(It.Is<byte[]>(bytes => bytes.All(b => b == 5))));
            format.Verify(f => f.Decompress(It.Is<byte[]>(bytes => bytes.All(b => b == 6))));
        }

        /// <summary>
        /// Creates a <see cref="ICompressedImage"/> mock of 3x2 blocks (48 texels)
        /// with a different set of arbitrary byte values in each block.
        /// </summary>
        /// <remarks>
        /// Block bytes:
        ///  _____  _____  _____
        /// |     ||     ||     | 
        /// | 0x1 || 0x2 || 0x3 |
        /// |_____||_____||_____|
        /// |     ||     ||     |
        /// | 0x4 || 0x5 || 0x6 |
        /// |_____||_____||_____|
        /// </remarks>
        private static ICompressedImage CreateDDSImageMock(IBlockCompressionFormat format)
        {
            var blockBytes = new byte[]
            {
                1, 2, 3,
                4, 5, 6
            };
            
            const int blockSize = 8;
            const int numberOfBlocks = 6;

            var buffer = new byte[numberOfBlocks * blockSize];

            for (int i = 0, bufferIndex = 0; i < blockBytes.Length; ++i, bufferIndex += blockSize)
            {
                for (int j = bufferIndex; j < blockSize + bufferIndex; ++j)
                {
                    buffer[j] = blockBytes[i];
                }
            }

            // Assert bytes have been set correctly
            // (for start and end byte value of each block only)
            Assert.AreEqual(1, buffer[0]);
            Assert.AreEqual(1, buffer[7]);
            Assert.AreEqual(2, buffer[8]);
            Assert.AreEqual(2, buffer[15]);
            Assert.AreEqual(3, buffer[16]);
            Assert.AreEqual(3, buffer[23]);

            Assert.AreEqual(4, buffer[24]);
            Assert.AreEqual(4, buffer[31]);
            Assert.AreEqual(5, buffer[32]);
            Assert.AreEqual(5, buffer[39]);
            Assert.AreEqual(6, buffer[40]);
            Assert.AreEqual(6, buffer[47]);

            const int width = 12;
            const int height = 8;

            return DDSImage.CreateFromData(width, height, buffer, format);
        }
    }
}
