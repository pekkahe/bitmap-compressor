using System;
using System.IO;
using System.Runtime.InteropServices;
using BitmapCompressor.Formats;
using BitmapCompressor.Serialization;
using BitmapCompressor.Serialization.FileFormat;
using NUnit.Framework;

namespace BitmapCompressor.Tests.UnitTests.Compressor.Serialization
{
    [TestFixture(Category = "Serialization")]
    public class DDSFileReaderTests
    {
        [Test]
        public void ReadingDataReturnsDDSImage()
        {
            int imageWidth = 4;
            int imageHeight = 4;
            var mainImage = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };

            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);

            // Write the DDS "magic number"
            writer.Write(DDSFile.MagicNumber);

            // Write the header data structure directly to the buffer
            var header = DDSFileHeaderFactory.CreateHeader(imageWidth, imageHeight, CompressionFormat.BC1);
            var headerData = new byte[Marshal.SizeOf(typeof(DDSFileHeader))];
            var handle = GCHandle.Alloc(headerData, GCHandleType.Pinned);
            Marshal.StructureToPtr(header, handle.AddrOfPinnedObject(), true);

            writer.Write(headerData);

            // Write main image data
            writer.Write(mainImage);

            // Flush data and reset stream position for reader
            writer.Flush();
            stream.Position = 0;

            // Act against test stream
            var reader = new DDSFileReader(stream);
            var ddsImage = reader.Read();

            Assert.AreEqual(imageWidth, ddsImage.Width);
            Assert.AreEqual(imageHeight, ddsImage.Height);
            CollectionAssert.AreEqual(mainImage, ddsImage.GetBuffer());

            reader.Dispose();
            writer.Dispose();
        }

        [Test]
        public void ReadingDataThrowsExceptionWhenMagicNumberIsInvalid()
        {
            var stream = new MemoryStream();

            var writer = new BinaryWriter(stream);
            writer.Write(DDSFile.MagicNumber + 1);

            // Flush data and reset stream position for reader
            writer.Flush();
            stream.Position = 0;

            var reader = new DDSFileReader(stream);

            Assert.Throws<InvalidOperationException>(() => reader.Read());

            reader.Dispose();
            writer.Dispose();
        }
    }
}
