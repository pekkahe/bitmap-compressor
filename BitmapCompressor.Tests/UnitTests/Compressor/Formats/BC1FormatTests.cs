using System;
using System.Drawing;
using BitmapCompressor.Formats;
using BitmapCompressor.Tests.Helpers;
using NUnit.Framework;

namespace BitmapCompressor.Tests.UnitTests.Compressor.Formats
{
    [TestFixture(Category = "Formats")]
    public class BC1FormatTests
    {
        [Test]
        public void CompressionOfHandPickedColorsWithoutAlpha()
        {
            var format = new BC1Format();
            var colors = new Color[16];
            colors[0]   = Color.FromArgb(3, 59, 101);
            colors[1]   = Color.FromArgb(131, 154, 20);
            colors[2]   = Color.FromArgb(87, 192, 127);
            colors[3]   = Color.FromArgb(203, 148, 14);
            colors[4]   = Color.FromArgb(237, 62, 69);
            colors[5]   = Color.FromArgb(173, 159, 102);
            colors[6]   = Color.FromArgb(176, 199, 73);
            colors[7]   = Color.FromArgb(118, 87, 228);
            colors[8]   = Color.FromArgb(79, 169, 87);
            colors[9]   = Color.FromArgb(179, 238, 219);
            colors[10]  = Color.FromArgb(70, 207, 12);
            colors[11]  = Color.FromArgb(81, 243, 109);
            colors[12]  = Color.FromArgb(38, 79, 189);
            colors[13]  = Color.FromArgb(249, 229, 125);
            colors[14]  = Color.FromArgb(205, 237, 57);
            colors[15]  = Color.FromArgb(190, 158, 111);

            var data = format.Compress(colors);

            Assert.AreEqual(BC1BlockLayout.ByteSize, data.Length);
            Assert.AreEqual(0x7B, data[0]);
            Assert.AreEqual(0xB7, data[1]);
            Assert.AreEqual(0xCC, data[2]);
            Assert.AreEqual(0x01, data[3]);
            Assert.AreEqual(0xAD, data[4]);
            Assert.AreEqual(0xEB, data[5]);
            Assert.AreEqual(0x22, data[6]);
            Assert.AreEqual(0x83, data[7]);
        }

        [Test]
        public void CompressionOfHandPickedColorsWithAlpha()
        {
            var format = new BC1Format();
            var colors = new Color[16];
            colors[0]   = Color.FromArgb(200, 3, 59, 101);
            colors[1]   = Color.FromArgb(200, 131, 154, 20);
            colors[2]   = Color.FromArgb(255, 87, 192, 127);
            colors[3]   = Color.FromArgb(255, 203, 148, 14);
            colors[4]   = Color.FromArgb(200, 237, 62, 69);
            colors[5]   = Color.FromArgb(200, 173, 159, 102);
            colors[6]   = Color.FromArgb(255, 176, 199, 73);
            colors[7]   = Color.FromArgb(255, 118, 87, 228);
            colors[8]   = Color.FromArgb(200, 79, 169, 87);
            colors[9]   = Color.FromArgb(200, 179, 238, 219);
            colors[10]  = Color.FromArgb(255, 70, 207, 12);
            colors[11]  = Color.FromArgb(255, 81, 243, 109);
            colors[12]  = Color.FromArgb(200, 38, 79, 189);
            colors[13]  = Color.FromArgb(200, 249, 229, 125);
            colors[14]  = Color.FromArgb(255, 205, 237, 57);
            colors[15]  = Color.FromArgb(255, 190, 158, 111);

            var data = format.Compress(colors);

            Assert.AreEqual(BC1BlockLayout.ByteSize, data.Length);
            Assert.AreEqual(0xCC, data[0]);
            Assert.AreEqual(0x01, data[1]);
            Assert.AreEqual(0x7B, data[2]);
            Assert.AreEqual(0xB7, data[3]);
            Assert.AreEqual(0xAF, data[4]);
            Assert.AreEqual(0xAF, data[5]);
            Assert.AreEqual(0x6F, data[6]);
            Assert.AreEqual(0x9F, data[7]);
        }

        [Test]
        public void DecompressionOfHandPickedBytesWithout1BitAlpha()
        {
            var format = new BC1Format();

            var bytes = new byte[BC1BlockLayout.ByteSize];
            bytes[0] = 0x7B;
            bytes[1] = 0xB7;
            bytes[2] = 0xCC;
            bytes[3] = 0x01;
            bytes[4] = 0xAD;
            bytes[5] = 0xEB;
            bytes[6] = 0x22;
            bytes[7] = 0x83;

            var colors = format.Decompress(bytes);

            Assert.AreEqual(BlockFormat.PixelCount, colors.Length);
            Assert.AreEqual(Color.FromArgb(0, 56, 99),      colors[0]);
            Assert.AreEqual(Color.FromArgb(57, 117, 140),   colors[1]);
            Assert.AreEqual(Color.FromArgb(123, 178, 181),  colors[2]);
            Assert.AreEqual(Color.FromArgb(123, 178, 181),  colors[3]);
            Assert.AreEqual(Color.FromArgb(57, 117, 140),   colors[4]);
            Assert.AreEqual(Color.FromArgb(123, 178, 181),  colors[5]);
            Assert.AreEqual(Color.FromArgb(123, 178, 181),  colors[6]);
            Assert.AreEqual(Color.FromArgb(57, 117, 140),   colors[7]);
            Assert.AreEqual(Color.FromArgb(123, 178, 181),  colors[8]);
            Assert.AreEqual(Color.FromArgb(181, 239, 222),  colors[9]);
            Assert.AreEqual(Color.FromArgb(123, 178, 181),  colors[10]);
            Assert.AreEqual(Color.FromArgb(181, 239, 222),  colors[11]);
            Assert.AreEqual(Color.FromArgb(57, 117, 140),   colors[12]);
            Assert.AreEqual(Color.FromArgb(181, 239, 222),  colors[13]);
            Assert.AreEqual(Color.FromArgb(181, 239, 222),  colors[14]);
            Assert.AreEqual(Color.FromArgb(123, 178, 181),  colors[15]);
        }

        [Test]
        public void DecompressionOfHandPickedBytesWith1BitAlpha()
        {
            var format = new BC1Format();

            var bytes = new byte[BC1BlockLayout.ByteSize];
            bytes[0] = 0xCC;
            bytes[1] = 0x01;
            bytes[2] = 0x7B;
            bytes[3] = 0xB7;
            bytes[4] = 0xAF;
            bytes[5] = 0xAF;
            bytes[6] = 0x6F;
            bytes[7] = 0x9F;

            var colors = format.Decompress(bytes);

            Assert.AreEqual(BlockFormat.PixelCount, colors.Length);
            Assert.AreEqual(Color.FromArgb(0, 0, 0, 0),         colors[0]);
            Assert.AreEqual(Color.FromArgb(0, 0, 0, 0),         colors[1]);
            Assert.AreEqual(Color.FromArgb(255, 90, 150, 165),  colors[2]);
            Assert.AreEqual(Color.FromArgb(255, 90, 150, 165),  colors[3]);
            Assert.AreEqual(Color.FromArgb(0, 0, 0, 0),         colors[4]);
            Assert.AreEqual(Color.FromArgb(0, 0, 0, 0),         colors[5]);
            Assert.AreEqual(Color.FromArgb(255, 90, 150, 165),  colors[6]);
            Assert.AreEqual(Color.FromArgb(255, 90, 150, 165),  colors[7]);
            Assert.AreEqual(Color.FromArgb(0, 0, 0, 0),         colors[8]);
            Assert.AreEqual(Color.FromArgb(0, 0, 0, 0),         colors[9]);
            Assert.AreEqual(Color.FromArgb(255, 90, 150, 165),  colors[10]);
            Assert.AreEqual(Color.FromArgb(255, 181, 239, 222), colors[11]);
            Assert.AreEqual(Color.FromArgb(0, 0, 0, 0),         colors[12]);
            Assert.AreEqual(Color.FromArgb(0, 0, 0, 0),         colors[13]);
            Assert.AreEqual(Color.FromArgb(255, 181, 239, 222), colors[14]);
            Assert.AreEqual(Color.FromArgb(255, 90, 150, 165),  colors[15]);
        }
    }
}
