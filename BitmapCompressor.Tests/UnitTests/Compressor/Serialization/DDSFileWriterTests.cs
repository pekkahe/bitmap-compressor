using System;
using System.IO;
using System.Runtime.InteropServices;
using BitmapCompressor.DataTypes;
using BitmapCompressor.Formats;
using BitmapCompressor.Serialization;
using BitmapCompressor.Serialization.FileFormat;
using NUnit.Framework;
using Moq;

namespace BitmapCompressor.Tests.UnitTests.Compressor.Serialization
{
    [TestFixture(Category = "Serialization")]
    public class DDSFileWriterTests
    {
        [Test]
        public void WriteImageToMemory()
        {
            var format = new Mock<IBlockCompressionFormat>();
            format.Setup(f => f.FourCC).Returns(FourCC.BC1Unorm);

            var imageData = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
            int imageWidth = 4;
            int imageHeight = 4;

            var ddsImage = DDSImage.CreateFromData(imageWidth, imageHeight, imageData, format.Object);
            var stream = new MemoryStream();
            var writer = new DDSFileWriter(stream);

            writer.Write(ddsImage);

            // Reset stream position for read
            stream.Position = 0;

            var reader = new BinaryReader(stream);

            {
                int size = Marshal.SizeOf(DDSFile.MagicNumber);
                var data = reader.ReadBytes(size);
                var magicNumber = BitConverter.ToUInt32(data, 0);

                Assert.That(magicNumber, Is.EqualTo(DDSFile.MagicNumber));
            }
            {
                int size = Marshal.SizeOf(typeof(DDSFileHeader));
                var data = reader.ReadBytes(size);
                var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
                var header = (DDSFileHeader) Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(DDSFileHeader));
                handle.Free();

                Assert.That(header.Width, Is.EqualTo(imageWidth));
                Assert.That(header.Height, Is.EqualTo(imageHeight));
            }
            {
                var data = reader.ReadBytes(imageData.Length);

                Assert.That(imageData, Is.EquivalentTo(imageData));
            }

            writer.Dispose();
            reader.Dispose();
        }

        [Test]
        public void CreateFileHeaderForBC1()
        {
            var expectedFlags = DDSFileHeaderFlags.DDSD_CAPS | DDSFileHeaderFlags.DDSD_HEIGHT |
                                DDSFileHeaderFlags.DDSD_WIDTH | DDSFileHeaderFlags.DDSD_PIXELFORMAT;
            var expectedPixelFormat = new DDSPixelFormat
            {
                Size = 32,
                Flags = DDSPixelFormatFlags.DDPF_FOURCC,
                FourCC = FourCC.BC1Unorm.Value
            };

            var header = DDSFileWriter.CreateHeader(1024, 512, FourCC.BC1Unorm.Value);

            Assert.That(header.Size, Is.EqualTo(124));
            Assert.That(header.Flags, Is.EqualTo(expectedFlags));
            Assert.That(header.Height, Is.EqualTo(512));
            Assert.That(header.Width, Is.EqualTo(1024));
            Assert.That(header.PitchOrLinearSize, Is.EqualTo(0));
            Assert.That(header.Depth, Is.EqualTo(0));
            Assert.That(header.MipMapCount, Is.EqualTo(0));
            unsafe
            {
                Assert.That(header.Reserved1[0], Is.EqualTo(0));
                Assert.That(header.Reserved1[1], Is.EqualTo(0));
                Assert.That(header.Reserved1[2], Is.EqualTo(0));
                Assert.That(header.Reserved1[3], Is.EqualTo(0));
                Assert.That(header.Reserved1[4], Is.EqualTo(0));
                Assert.That(header.Reserved1[5], Is.EqualTo(0));
                Assert.That(header.Reserved1[6], Is.EqualTo(0));
                Assert.That(header.Reserved1[7], Is.EqualTo(0));
                Assert.That(header.Reserved1[8], Is.EqualTo(0));
                Assert.That(header.Reserved1[9], Is.EqualTo(0));
                Assert.That(header.Reserved1[10], Is.EqualTo(0));
            }
            Assert.That(header.Caps, Is.EqualTo(DDSCapsFlags.DDSCAPS_TEXTURE));
            Assert.That(header.Caps2, Is.EqualTo(0));
            Assert.That(header.Caps3, Is.EqualTo(0));
            Assert.That(header.Caps4, Is.EqualTo(0));
            Assert.That(header.Reserved2, Is.EqualTo(0));
            
            Assert.That(header.PixelFormat.Size, Is.EqualTo(expectedPixelFormat.Size));
            Assert.That(header.PixelFormat.Flags, Is.EqualTo(expectedPixelFormat.Flags));
            Assert.That(header.PixelFormat.FourCC, Is.EqualTo(expectedPixelFormat.FourCC));
            Assert.That(header.PixelFormat.RGBBitCount, Is.EqualTo(0));
            Assert.That(header.PixelFormat.RBitMask, Is.EqualTo(0));
            Assert.That(header.PixelFormat.GBitMask, Is.EqualTo(0));
            Assert.That(header.PixelFormat.BBitMask, Is.EqualTo(0));
            Assert.That(header.PixelFormat.ABitMask, Is.EqualTo(0));
        }
    }
}
