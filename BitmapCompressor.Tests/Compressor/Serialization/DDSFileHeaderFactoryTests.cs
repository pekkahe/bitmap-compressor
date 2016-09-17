using System;
using BitmapCompressor.Serialization;
using BitmapCompressor.Serialization.FileFormat;
using NUnit.Framework;

namespace BitmapCompressor.Tests.Compression.Serialization
{
    [TestFixture]
    public class DDSFileHeaderFactoryTests
    {
        [Test]
        public void DDSFileHeaderFactory_CreateDXT1Header_CreatesDDSFileHeaderStructure()
        {
            var expectedFlags = DDSFileHeaderFlags.DDSD_CAPS | DDSFileHeaderFlags.DDSD_HEIGHT |
                                DDSFileHeaderFlags.DDSD_WIDTH | DDSFileHeaderFlags.DDSD_PIXELFORMAT;
            var expectedPixelFormat = new DDSPixelFormat
            {
                Size = 32,
                Flags = DDSPixelFormatFlags.DDPF_FOURCC,
                FourCC = DDSPixelFormatFourCC.FOURCC_DXT1
            };

            var header = DDSFileHeaderFactory.CreateDXT1Header(1024, 512);

            Assert.AreEqual(124, header.Size);
            Assert.AreEqual(expectedFlags, header.Flags);
            Assert.AreEqual(512, header.Height);
            Assert.AreEqual(1024, header.Width);
            Assert.AreEqual(0, header.PitchOrLinearSize);
            Assert.AreEqual(0, header.Depth);
            Assert.AreEqual(0, header.MipMapCount);
            unsafe
            {
                Assert.AreEqual(0, header.Reserved1[0]);
                Assert.AreEqual(0, header.Reserved1[1]);
                Assert.AreEqual(0, header.Reserved1[2]);
                Assert.AreEqual(0, header.Reserved1[3]);
                Assert.AreEqual(0, header.Reserved1[4]);
                Assert.AreEqual(0, header.Reserved1[5]);
                Assert.AreEqual(0, header.Reserved1[6]);
                Assert.AreEqual(0, header.Reserved1[7]);
                Assert.AreEqual(0, header.Reserved1[8]);
                Assert.AreEqual(0, header.Reserved1[9]);
                Assert.AreEqual(0, header.Reserved1[10]);
            }
            Assert.AreEqual(DDSCapsFlags.DDSCAPS_TEXTURE, header.Caps);
            Assert.AreEqual(0, header.Caps2);
            Assert.AreEqual(0, header.Caps3);
            Assert.AreEqual(0, header.Caps4);
            Assert.AreEqual(0, header.Reserved2);

            Assert.AreEqual(expectedPixelFormat.Size, header.PixelFormat.Size);
            Assert.AreEqual(expectedPixelFormat.Flags, header.PixelFormat.Flags);
            Assert.AreEqual(expectedPixelFormat.FourCC, header.PixelFormat.FourCC);
            Assert.AreEqual(0, header.PixelFormat.RGBBitCount);
            Assert.AreEqual(0, header.PixelFormat.RBitMask);
            Assert.AreEqual(0, header.PixelFormat.GBitMask);
            Assert.AreEqual(0, header.PixelFormat.BBitMask);
            Assert.AreEqual(0, header.PixelFormat.ABitMask);
        }
    }
}
