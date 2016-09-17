using System;
using System.IO;
using System.Runtime.InteropServices;
using BitmapCompressor.DataTypes;
using BitmapCompressor.Serialization;
using BitmapCompressor.Serialization.FileFormat;
using NUnit.Framework;

namespace BitmapCompressor.Tests.Compression.Serialization
{
    [TestFixture]
    public class DDSFileWriterTests
    {
        [Test]
        public void DDSFileWriter_WriteDDSImage_WritesDataToMemory()
        {
            var imageData = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
            int imageWidth = 4;
            int imageHeight = 4;

            var ddsImage = new DDSImage(imageWidth, imageHeight, imageData);
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
    }
}
