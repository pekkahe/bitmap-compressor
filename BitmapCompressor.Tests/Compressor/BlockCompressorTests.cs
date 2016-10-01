using System;
using System.Linq;
using System.Drawing;
using BitmapCompressor.Formats;
using BitmapCompressor.DataTypes;
using Moq;
using NUnit.Framework;

namespace BitmapCompressor.Tests.Compressor
{
    [TestFixture(Category = "Compressor")]
    public class BlockCompressorTests
    {
        #region Test colors

        // Let's use custom colors instead of the static properties provided
        // by Color because comparing against the statics fails when the color
        // is read from a bitmap.

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
            var compressor = new BlockCompressor(new Mock<IBlockCompressionFormat>().Object);
            var bmpImage = new BMPImage(new Bitmap(10, 16));

            Assert.Throws<InvalidOperationException>(() => compressor.Compress(bmpImage));
        }

        [Test]
        public void CompressionCalculatesImageDimensionsAndBufferSize()
        {
            const int byteCount = 8;
            const int blockCount = 4;
            const int imageWidth = 2 * BlockFormat.Dimension;  // Two horizontal and
            const int imageHeight = 2 * BlockFormat.Dimension; // vertical blocks

            var format = new Mock<IBlockCompressionFormat>();
            format.Setup(f => f.BlockSize).Returns(byteCount);
            format.Setup(f => f.Compress(It.IsAny<Color[]>())).Returns(new byte[byteCount]);

            var compressor = new BlockCompressor(format.Object);
            var bmpImage = new BMPImage(new Bitmap(imageWidth, imageHeight));

            var ddsImage = compressor.Compress(bmpImage) as DDSImage;

            Assert.IsNotNull(ddsImage);
            Assert.AreEqual(imageWidth, ddsImage.Width);
            Assert.AreEqual(imageHeight, ddsImage.Height);
            Assert.AreEqual(blockCount * byteCount, ddsImage.Buffer.Length);
        }

        [Test]
        public void CompressionRunsBlockCompressionForEachBlock()
        {
            const int byteCount = 8;

            var format = new Mock<IBlockCompressionFormat>();
            format.Setup(f => f.BlockSize).Returns(byteCount);
            format.Setup(f => f.Compress(It.IsAny<Color[]>())).Returns(new byte[byteCount]);

            var compressor = new BlockCompressor(format.Object);
            var bmpImage = new BMPImage(CreateTestBitmap());

            compressor.Compress(bmpImage);

            format.Verify(f => f.Compress(It.IsAny<Color[]>()), Times.Exactly(6));
            format.Verify(f => f.Compress(It.Is<Color[]>(colors => colors.All(c => c == Red))));
            format.Verify(f => f.Compress(It.Is<Color[]>(colors => colors.All(c => c == Green))));
            format.Verify(f => f.Compress(It.Is<Color[]>(colors => colors.All(c => c == Blue))));
            format.Verify(f => f.Compress(It.Is<Color[]>(colors => colors.All(c => c == White))));
            format.Verify(f => f.Compress(It.Is<Color[]>(colors => colors.All(c => c == Black))));
            format.Verify(f => f.Compress(It.Is<Color[]>(colors => colors.All(c => c == Gray))));
        }

        /// <summary>
        /// Creates a test <see cref="Bitmap"/> of 3x2 blocks (48 pixels) with 
        /// a different color in each block.
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
        private static Bitmap CreateTestBitmap()
        {
            var blockColors = new[]
            {
                new [] { Red,   Green, Blue },
                new [] { White, Black, Gray },
            };

            const int numberOfHorizontalBlocks = 3;
            const int numberOfVerticalBlocks = 2;

            var bitmap = new Bitmap(
                numberOfHorizontalBlocks * BlockFormat.Dimension,
                numberOfVerticalBlocks * BlockFormat.Dimension);

            for (int y = 0; y < numberOfVerticalBlocks; ++y)
            {
                for (int x = 0; x < numberOfHorizontalBlocks; ++x)
                {
                    var color = blockColors[y][x];

                    var firstPixel = new Point(
                        x * BlockFormat.Dimension,
                        y * BlockFormat.Dimension);

                    var lastPixel = new Point(
                        firstPixel.X + BlockFormat.Dimension,
                        firstPixel.Y + BlockFormat.Dimension);

                    for (int py = firstPixel.Y; py < lastPixel.Y; ++py)
                    {
                        for (int px = firstPixel.X; px < lastPixel.X; ++px)
                        {
                            bitmap.SetPixel(px, py, color);
                        }
                    }
                }
            }

            // Assert colors have been set correctly
            // (for top-left and bottom-right color of each block only)
            Assert.AreEqual(Red, bitmap.GetPixel(0, 0));
            Assert.AreEqual(Red, bitmap.GetPixel(3, 3));
            Assert.AreEqual(Green, bitmap.GetPixel(4, 0));
            Assert.AreEqual(Green, bitmap.GetPixel(7, 3));
            Assert.AreEqual(Blue, bitmap.GetPixel(8, 0));
            Assert.AreEqual(Blue, bitmap.GetPixel(11, 3));

            Assert.AreEqual(White, bitmap.GetPixel(0, 4));
            Assert.AreEqual(White, bitmap.GetPixel(3, 7));
            Assert.AreEqual(Black, bitmap.GetPixel(4, 4));
            Assert.AreEqual(Black, bitmap.GetPixel(7, 7));
            Assert.AreEqual(Gray, bitmap.GetPixel(8, 4));
            Assert.AreEqual(Gray, bitmap.GetPixel(11, 7));

            return bitmap;
        }

        [Test]
        public void DecompressionRunsBlockDecompressionForEachBlock()
        {
            const int byteCount = 8;

            var format = new Mock<IBlockCompressionFormat>();
            format.Setup(f => f.BlockSize).Returns(byteCount);
            format.Setup(f => f.Decompress(It.IsAny<byte[]>())).Returns(new Color[BlockFormat.PixelCount]);

            var compressor = new BlockCompressor(format.Object);

            compressor.Decompress(CreateTestDDSImage());

            format.Verify(f => f.Decompress(It.IsAny<byte[]>()), Times.Exactly(6));
            format.Verify(f => f.Decompress(It.Is<byte[]>(bytes => bytes.All(b => b == 1))));
            format.Verify(f => f.Decompress(It.Is<byte[]>(bytes => bytes.All(b => b == 2))));
            format.Verify(f => f.Decompress(It.Is<byte[]>(bytes => bytes.All(b => b == 3))));
            format.Verify(f => f.Decompress(It.Is<byte[]>(bytes => bytes.All(b => b == 4))));
            format.Verify(f => f.Decompress(It.Is<byte[]>(bytes => bytes.All(b => b == 5))));
            format.Verify(f => f.Decompress(It.Is<byte[]>(bytes => bytes.All(b => b == 6))));
        }

        /// <summary>
        /// Creates a test <see cref="DDSImage"/> of 3x2 blocks (48 pixels) with
        /// a different set of arbitrary byte values in each block.
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
        private static DDSImage CreateTestDDSImage()
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

            return new DDSImage(width, height, buffer);
        }
    }
}
