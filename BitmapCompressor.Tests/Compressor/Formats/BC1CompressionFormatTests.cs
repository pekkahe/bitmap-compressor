using System;
using BitmapCompressor.Formats;
using NUnit.Framework;

namespace BitmapCompressor.Tests.Compression.Formats
{
    [TestFixture]
    public class BC1CompressionFormatTests
    {
        [Test]
        public void BC1CompressionFormat_Compress_ReturnsByteArrayOfCorrectSize()
        {
            var format = new BC1CompressionFormat();
            var colors = TestHelpers.CreateRandomColors();

            var data = format.Compress(colors);

            Assert.AreEqual(BC1BlockLayout.ByteSize, data.Length);
        }

        [Test]
        public void BC1CompressionFormat_Decompress_Returns16ArgbColors()
        {
            var format = new BC1CompressionFormat();

            var colors = format.Decompress(new byte[BC1BlockLayout.ByteSize]);

            Assert.AreEqual(BlockFormat.PixelCount, colors.Length);
        }
    }
}
