using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using BitmapCompressor.Formats;
using BitmapCompressor.DataTypes;
using BitmapCompressor.Tests.Helpers;
using NUnit.Framework;

namespace BitmapCompressor.IntegrationTests.Integration
{
    [TestFixture(Category = "Integration")]
    public class BlockCompressorTests
    {
        private const string MarsBMPFile = "mars.bmp";
        private const string MarsBMPFromDDSFile = "mars-fromdds.bmp";
        private const string MarsDDSFile = "mars.dds";
        private const string CityscapeBMPFile = "cityscape.bmp";
        private const string CityscapeBMPFromDDSFile = "cityscape-fromdds.bmp";
        private const string CityscapeDDSFile = "cityscape.dds";

        private BlockCompressor _compressor;

        [SetUp]
        public void Setup()
        {
            _compressor = new BlockCompressor(new BC1CompressionFormat());
        }

        [Test]
        public void CompressBMPToDDSWithBC1WhenSourceHasNoAlpha()
        {
            var bitmap = LoadResourceBMP(CityscapeBMPFile);
            IProcessedImage actual;

            using (Profiler.MeasureTime())
            {
                actual = _compressor.Compress(bitmap);
            }

            var expected = LoadResourceDDS(CityscapeDDSFile);

            AssertEqual(expected, actual as DDSImage);
        }

        [Test]
        public void CompressBMPToDDSWithBC1WhenSourceHasAlpha()
        {
            var bitmap = LoadResourceBMP(MarsBMPFile);
            IProcessedImage actual;

            using (Profiler.MeasureTime())
            {
                actual = _compressor.Compress(bitmap);
            }

            var expected = LoadResourceDDS(MarsDDSFile);

            AssertEqual(expected, actual as DDSImage);
        }

        [Test]
        public void UncompressDDStoBMPWithBC1WhenSourceHasNoAlpha()
        {
            var dds = LoadResourceDDS(CityscapeDDSFile);
            IProcessedImage actual;

            using (Profiler.MeasureTime())
            {
                actual = _compressor.Decompress(dds);
            }

            var expected = LoadResourceBMP(CityscapeBMPFromDDSFile);

            AssertEqual(expected, actual as BMPImage);
        }

        [Test]
        public void UncompressDDStoBMPWithBC1WhenSourceHasAlpha()
        {
            var dds = LoadResourceDDS(MarsDDSFile);
            IProcessedImage actual;

            using (Profiler.MeasureTime())
            {
                actual = _compressor.Decompress(dds);
            }

            var expected = LoadResourceBMP(MarsBMPFromDDSFile);

            AssertEqual(expected, actual as BMPImage);
        }

        private static BMPImage LoadResourceBMP(string fileName)
        {
            return BMPImage.Load(TestResourceDirectory.GetFilePath(fileName));
        }

        private static DDSImage LoadResourceDDS(string fileName)
        {
            return DDSImage.Load(TestResourceDirectory.GetFilePath(fileName));
        }

        private static void AssertEqual(DDSImage expected, DDSImage actual)
        {
            Assert.AreEqual(expected.Width, actual.Width);
            Assert.AreEqual(expected.Height, actual.Height);

            CollectionAssert.AreEqual(expected.Buffer, actual.Buffer);
        }

        private static void AssertEqual(BMPImage expected, BMPImage actual)
        {
            Assert.AreEqual(expected.Width, actual.Width);
            Assert.AreEqual(expected.Height, actual.Height);

            var expectedBitmap = expected.GetBitmap();
            var actualBitmap = actual.GetBitmap();

            var size = new Rectangle(0, 0, expected.Width, expected.Height);

            var expectedData = expectedBitmap.LockBits(size, ImageLockMode.ReadOnly, expectedBitmap.PixelFormat);
            var actualData = actualBitmap.LockBits(size, ImageLockMode.ReadOnly, actualBitmap.PixelFormat);

            var expectedRgbValues = new byte[expectedData.Stride * size.Height];
            var actualRgbValues = new byte[expectedData.Stride * size.Height];

            Marshal.Copy(expectedData.Scan0, expectedRgbValues, 0, expectedData.Stride * size.Height);
            Marshal.Copy(actualData.Scan0, actualRgbValues, 0, actualData.Stride * size.Height);

            try
            {
                CollectionAssert.AreEqual(expectedRgbValues, actualRgbValues);
            }
            finally
            {
                expectedBitmap.UnlockBits(expectedData);
                actualBitmap.UnlockBits(actualData);
            }
        }
    }
}
