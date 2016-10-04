using System;
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
            ICompressedImage actual;

            using (Profiler.MeasureTime())
            {
                actual = _compressor.Compress(bitmap);
            }

            var expected = LoadResourceDDS(CityscapeDDSFile);

            AssertEqual(expected, actual);
        }

        [Test]
        public void CompressBMPToDDSWithBC1WhenSourceHasAlpha()
        {
            var bitmap = LoadResourceBMP(MarsBMPFile);
            ICompressedImage actual;

            using (Profiler.MeasureTime())
            {
                actual = _compressor.Compress(bitmap);
            }

            var expected = LoadResourceDDS(MarsDDSFile);

            AssertEqual(expected, actual);
        }

        [Test]
        public void UncompressDDStoBMPWithBC1WhenSourceHasNoAlpha()
        {
            var dds = LoadResourceDDS(CityscapeDDSFile);
            IUncompressedImage actual;

            using (Profiler.MeasureTime())
            {
                actual = _compressor.Decompress(dds);
            }

            var expected = LoadResourceBMP(CityscapeBMPFromDDSFile);

            AssertEqual(expected, actual);
        }

        [Test]
        public void UncompressDDStoBMPWithBC1WhenSourceHasAlpha()
        {
            var dds = LoadResourceDDS(MarsDDSFile);
            IUncompressedImage actual;

            using (Profiler.MeasureTime())
            {
                actual = _compressor.Decompress(dds);
            }

            var expected = LoadResourceBMP(MarsBMPFromDDSFile);

            AssertEqual(expected, actual);
        }

        private static IUncompressedImage LoadResourceBMP(string fileName)
        {
            return DirectBitmap.FromFile(TestResourceDirectory.GetFilePath(fileName));
        }

        private static ICompressedImage LoadResourceDDS(string fileName)
        {
            return DDSImage.FromFile(TestResourceDirectory.GetFilePath(fileName));
        }

        private static void AssertEqual(IImage expected, IImage actual)
        {
            Assert.AreEqual(expected.Width, actual.Width);
            Assert.AreEqual(expected.Height, actual.Height);

            CollectionAssert.AreEqual(expected.GetBuffer(), actual.GetBuffer());
        }
    }
}
