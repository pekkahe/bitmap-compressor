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
        public void WritingDDSImageWritesDataToMemory()
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

                Assert.AreEqual(DDSFile.MagicNumber, magicNumber);
            }
            {
                int size = Marshal.SizeOf(typeof(DDSFileHeader));
                var data = reader.ReadBytes(size);
                var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
                var header = (DDSFileHeader) Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(DDSFileHeader));
                handle.Free();

                Assert.AreEqual(imageWidth, header.Width);
                Assert.AreEqual(imageHeight, header.Height);
            }
            {
                var data = reader.ReadBytes(imageData.Length);

                CollectionAssert.AreEqual(imageData, imageData);
            }

            writer.Dispose();
            reader.Dispose();
        }

        [Test]
        public void CreatingBC1HeaderCreatesDDSFileHeaderStructure()
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
