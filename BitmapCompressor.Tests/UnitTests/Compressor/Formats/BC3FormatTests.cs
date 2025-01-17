using System;
using System.Drawing;
using BitmapCompressor.Formats;
using BitmapCompressor.DataTypes;
using BitmapCompressor.Tests.Helpers;
using BitmapCompressor.Utilities;
using NUnit.Framework;

namespace BitmapCompressor.Tests.UnitTests.Compressor.Formats
{
    [TestFixture(Category = "Formats")]
    public class BC3FormatTests
    {
        [Test]
        public void CompressionReturnsByteArrayOfCorrectSize()
        {
            var colors = new Color[BlockFormat.TexelCount];

            var data = new BC3Format().Compress(colors);

            Assert.That(data.Length, Is.EqualTo(BlockFormat.BC3ByteSize));
        }

        [Test]
        public void DecompressionReturnsColorArrayOfCorrectSize()
        {
            var bytes = new byte[BlockFormat.BC3ByteSize];

            var colors = new BC3Format().Decompress(bytes);

            Assert.That(colors.Length, Is.EqualTo(BlockFormat.TexelCount));
        }

        [Test]
        public void CompressionOrdersReferenceColors()
        {
            var expectedMin = Color.FromArgb(10, 10, 10);
            var expectedMax = Color.FromArgb(250, 250, 250);
            var expectedColor0 = ColorUtility.To16Bit(expectedMax);
            var expectedColor1 = ColorUtility.To16Bit(expectedMin);
            var colors = ColorHelper.CreateRandomColorsBetween(expectedMin, expectedMax);

            // Place expected min and max color in test input
            colors[4] = expectedMin;
            colors[10] = expectedMax;

            var data = new BC3Format().Compress(colors);

            var color0 = Color565.FromValue((ushort) ((data[9] << 8) | data[8]));
            var color1 = Color565.FromValue((ushort) ((data[11] << 8) | data[10]));

            Assert.That(color1.Value, Is.GreaterThan(color0.Value));
            Assert.That(color0.Value, Is.EqualTo(expectedColor0.Value));
            Assert.That(color1.Value, Is.EqualTo(expectedColor1.Value));
        }

        [Test]
        public void CompressColorsWithoutAlpha()
        {
            var colors  = new Color[16];
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

            var data = new BC3Format().Compress(colors);

            // Assert against data hard-coded from a successful test
            // run where the data was built using unit tested components.

            Assert.That(data[0], Is.EqualTo(0xFF));  // alpha0
            Assert.That(data[1], Is.EqualTo(0xFF));  // alpha1
            Assert.That(data[2], Is.EqualTo(0x00));  // a-idx2
            Assert.That(data[3], Is.EqualTo(0x00));  // a-idx1
            Assert.That(data[4], Is.EqualTo(0x00));  // a-idx0
            Assert.That(data[5], Is.EqualTo(0x00));  // a-idx5
            Assert.That(data[6], Is.EqualTo(0x00));  // a-idx4
            Assert.That(data[7], Is.EqualTo(0x00));  // a-idx3
            Assert.That(data[8], Is.EqualTo(0x7B));  // c0Low
            Assert.That(data[9], Is.EqualTo(0xB7));  // c0Hi
            Assert.That(data[10], Is.EqualTo(0xCC)); // c1Low
            Assert.That(data[11], Is.EqualTo(0x01)); // c1Hi
            Assert.That(data[12], Is.EqualTo(0xAD)); // c-idx0
            Assert.That(data[13], Is.EqualTo(0xEB)); // c-idx1
            Assert.That(data[14], Is.EqualTo(0x22)); // c-idx2
            Assert.That(data[15], Is.EqualTo(0x83)); // c-idx3
        }

        [Test]
        public void CompressColorsWithAlpha()
        {
            var colors  = new Color[16];
            colors[0]   = Color.FromArgb(55, 3, 59, 101);
            colors[1]   = Color.FromArgb(100, 131, 154, 20);
            colors[2]   = Color.FromArgb(80, 87, 192, 127);
            colors[3]   = Color.FromArgb(220, 203, 148, 14);
            colors[4]   = Color.FromArgb(190, 237, 62, 69);
            colors[5]   = Color.FromArgb(200, 173, 159, 102);
            colors[6]   = Color.FromArgb(30, 176, 199, 73);
            colors[7]   = Color.FromArgb(200, 118, 87, 228);
            colors[8]   = Color.FromArgb(120, 79, 169, 87);
            colors[9]   = Color.FromArgb(40, 179, 238, 219);
            colors[10]  = Color.FromArgb(160, 70, 207, 12);
            colors[11]  = Color.FromArgb(140, 81, 243, 109);
            colors[12]  = Color.FromArgb(130, 38, 79, 189);
            colors[13]  = Color.FromArgb(10, 249, 229, 125);
            colors[14]  = Color.FromArgb(220, 205, 237, 57);
            colors[15]  = Color.FromArgb(70, 190, 158, 111);

            var data = new BC3Format().Compress(colors);

            // Assert against data hard-coded from a successful test
            // run where the data was built using unit tested components.

            Assert.That(data[0], Is.EqualTo(0xDC));   // alpha0
            Assert.That(data[1], Is.EqualTo(0x0A));   // alpha1
            Assert.That(data[2], Is.EqualTo(0x04));   // a-idx2
            Assert.That(data[3], Is.EqualTo(0x00));   // a-idx1
            Assert.That(data[4], Is.EqualTo(0x49));   // a-idx0
            Assert.That(data[5], Is.EqualTo(0x20));   // a-idx5
            Assert.That(data[6], Is.EqualTo(0x80));   // a-idx4
            Assert.That(data[7], Is.EqualTo(0x08));   // a-idx3        
            Assert.That(data[8], Is.EqualTo(0x7B));   // c0Low
            Assert.That(data[9], Is.EqualTo(0xB7));   // c0Hi
            Assert.That(data[10], Is.EqualTo(0xCC));  // c1Low
            Assert.That(data[11], Is.EqualTo(0x01));  // c1Hi
            Assert.That(data[12], Is.EqualTo(0xAD));  // c-idx0
            Assert.That(data[13], Is.EqualTo(0xEB));  // c-idx1
            Assert.That(data[14], Is.EqualTo(0x22));  // c-idx2
            Assert.That(data[15], Is.EqualTo(0x83));  // c-idx3
        }

        [Test]
        public void DecompressDataWithoutAlpha()
        {
            var bytes = new byte[BlockFormat.BC3ByteSize];
            bytes[0]    = 0xFF;  // alpha0
            bytes[1]    = 0xFF;  // alpha1
            bytes[2]    = 0x00;  // a-idx2
            bytes[3]    = 0x00;  // a-idx1
            bytes[4]    = 0x00;  // a-idx0
            bytes[5]    = 0x00;  // a-idx5
            bytes[6]    = 0x00;  // a-idx4
            bytes[7]    = 0x00;  // a-idx3
            bytes[8]    = 0x7B;  // c0Low
            bytes[9]    = 0xB7;  // c0Hi
            bytes[10]   = 0xCC;  // c1Low
            bytes[11]   = 0x01;  // c1Hi
            bytes[12]   = 0xAD;  // c-idx0
            bytes[13]   = 0xEB;  // c-idx1
            bytes[14]   = 0x22;  // c-idx2
            bytes[15]   = 0x83;  // c-idx3

            var colors = new BC3Format().Decompress(bytes);

            // Assert against data hard-coded from a successful test
            // run where the data was built using unit tested components.

            Assert.That(colors[0], Is.EqualTo(Color.FromArgb(255, 0, 56, 99)));
            Assert.That(colors[1], Is.EqualTo(Color.FromArgb(255, 57, 117, 140)));
            Assert.That(colors[2], Is.EqualTo(Color.FromArgb(255, 123, 178, 181)));
            Assert.That(colors[3], Is.EqualTo(Color.FromArgb(255, 123, 178, 181)));
            Assert.That(colors[4], Is.EqualTo(Color.FromArgb(255, 57, 117, 140)));
            Assert.That(colors[5], Is.EqualTo(Color.FromArgb(255, 123, 178, 181)));
            Assert.That(colors[6], Is.EqualTo(Color.FromArgb(255, 123, 178, 181)));
            Assert.That(colors[7], Is.EqualTo(Color.FromArgb(255, 57, 117, 140)));
            Assert.That(colors[8], Is.EqualTo(Color.FromArgb(255, 123, 178, 181)));
            Assert.That(colors[9], Is.EqualTo(Color.FromArgb(255, 181, 239, 222)));
            Assert.That(colors[10], Is.EqualTo(Color.FromArgb(255, 123, 178, 181)));
            Assert.That(colors[11], Is.EqualTo(Color.FromArgb(255, 181, 239, 222)));
            Assert.That(colors[12], Is.EqualTo(Color.FromArgb(255, 57, 117, 140)));
            Assert.That(colors[13], Is.EqualTo(Color.FromArgb(255, 181, 239, 222)));
            Assert.That(colors[14], Is.EqualTo(Color.FromArgb(255, 181, 239, 222)));
            Assert.That(colors[15], Is.EqualTo(Color.FromArgb(255, 123, 178, 181)));
        }

        [Test]
        public void DecompressDataWithAlpha()
        {
            var bytes = new byte[BlockFormat.BC3ByteSize];
            bytes[0]    = 0xDC; // alpha0
            bytes[1]    = 0x0A; // alpha1
            bytes[2]    = 0x04; // a-idx2
            bytes[3]    = 0x00; // a-idx1
            bytes[4]    = 0x49; // a-idx0
            bytes[5]    = 0x20; // a-idx5
            bytes[6]    = 0x80; // a-idx4
            bytes[7]    = 0x08; // a-idx3   
            bytes[8]    = 0x7B; // c0Low
            bytes[9]    = 0xB7; // c0Hi
            bytes[10]   = 0xCC; // c1Low
            bytes[11]   = 0x01; // c1Hi
            bytes[12]   = 0xAD; // c-idx0
            bytes[13]   = 0xEB; // c-idx1
            bytes[14]   = 0x22; // c-idx2
            bytes[15]   = 0x83; // c-idx3

            var colors = new BC3Format().Decompress(bytes);

            // Assert against data hard-coded from a successful test
            // run where the data was built using unit tested components.

            Assert.That(colors[0], Is.EqualTo(Color.FromArgb(10, 0, 56, 99)));
            Assert.That(colors[1], Is.EqualTo(Color.FromArgb(10, 57, 117, 140)));
            Assert.That(colors[2], Is.EqualTo(Color.FromArgb(10, 123, 178, 181)));
            Assert.That(colors[3], Is.EqualTo(Color.FromArgb(220, 123, 178, 181)));
            Assert.That(colors[4], Is.EqualTo(Color.FromArgb(220, 57, 117, 140)));
            Assert.That(colors[5], Is.EqualTo(Color.FromArgb(220, 123, 178, 181)));
            Assert.That(colors[6], Is.EqualTo(Color.FromArgb(10, 123, 178, 181)));
            Assert.That(colors[7], Is.EqualTo(Color.FromArgb(220, 57, 117, 140)));
            Assert.That(colors[8], Is.EqualTo(Color.FromArgb(220, 123, 178, 181)));
            Assert.That(colors[9], Is.EqualTo(Color.FromArgb(10, 181, 239, 222)));
            Assert.That(colors[10], Is.EqualTo(Color.FromArgb(220, 123, 178, 181)));
            Assert.That(colors[11], Is.EqualTo(Color.FromArgb(220, 181, 239, 222)));
            Assert.That(colors[12], Is.EqualTo(Color.FromArgb(220, 57, 117, 140)));
            Assert.That(colors[13], Is.EqualTo(Color.FromArgb(10, 181, 239, 222)));
            Assert.That(colors[14], Is.EqualTo(Color.FromArgb(220, 181, 239, 222)));
            Assert.That(colors[15], Is.EqualTo(Color.FromArgb(10, 123, 178, 181)));
        }
    }
}
