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

            Assert.AreEqual(BlockFormat.BC3ByteSize, data.Length);
        }

        [Test]
        public void DecompressionReturnsColorArrayOfCorrectSize()
        {
            var bytes = new byte[BlockFormat.BC3ByteSize];

            var colors = new BC3Format().Decompress(bytes);

            Assert.AreEqual(BlockFormat.TexelCount, colors.Length);
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

            Assert.Greater(color0.Value, color1.Value);
            Assert.AreEqual(expectedColor0.Value, color0.Value);
            Assert.AreEqual(expectedColor1.Value, color1.Value);
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

            Assert.AreEqual(0xFF, data[0]);  // alpha0
            Assert.AreEqual(0xFF, data[1]);  // alpha1
            Assert.AreEqual(0x00, data[2]);  // a-idx2
            Assert.AreEqual(0x00, data[3]);  // a-idx1
            Assert.AreEqual(0x00, data[4]);  // a-idx0
            Assert.AreEqual(0x00, data[5]);  // a-idx5
            Assert.AreEqual(0x00, data[6]);  // a-idx4
            Assert.AreEqual(0x00, data[7]);  // a-idx3
            Assert.AreEqual(0x7B, data[8]);  // c0Low
            Assert.AreEqual(0xB7, data[9]);  // c0Hi
            Assert.AreEqual(0xCC, data[10]); // c1Low
            Assert.AreEqual(0x01, data[11]); // c1Hi
            Assert.AreEqual(0xAD, data[12]); // c-idx0
            Assert.AreEqual(0xEB, data[13]); // c-idx1
            Assert.AreEqual(0x22, data[14]); // c-idx2
            Assert.AreEqual(0x83, data[15]); // c-idx3
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

            Assert.AreEqual(0xDC,   data[0]);   // alpha0
            Assert.AreEqual(0x0A,   data[1]);   // alpha1
            Assert.AreEqual(0x04,   data[2]);   // a-idx2
            Assert.AreEqual(0x00,   data[3]);   // a-idx1
            Assert.AreEqual(0x49,   data[4]);   // a-idx0
            Assert.AreEqual(0x20,   data[5]);   // a-idx5
            Assert.AreEqual(0x80,   data[6]);   // a-idx4
            Assert.AreEqual(0x08,   data[7]);   // a-idx3        
            Assert.AreEqual(0x7B,   data[8]);   // c0Low
            Assert.AreEqual(0xB7,   data[9]);   // c0Hi
            Assert.AreEqual(0xCC,   data[10]);  // c1Low
            Assert.AreEqual(0x01,   data[11]);  // c1Hi
            Assert.AreEqual(0xAD,   data[12]);  // c-idx0
            Assert.AreEqual(0xEB,   data[13]);  // c-idx1
            Assert.AreEqual(0x22,   data[14]);  // c-idx2
            Assert.AreEqual(0x83,   data[15]);  // c-idx3
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

            Assert.AreEqual(Color.FromArgb(255, 0, 56, 99),      colors[0]);
            Assert.AreEqual(Color.FromArgb(255, 57, 117, 140),   colors[1]);
            Assert.AreEqual(Color.FromArgb(255, 123, 178, 181),  colors[2]);
            Assert.AreEqual(Color.FromArgb(255, 123, 178, 181),  colors[3]);
            Assert.AreEqual(Color.FromArgb(255, 57, 117, 140),   colors[4]);
            Assert.AreEqual(Color.FromArgb(255, 123, 178, 181),  colors[5]);
            Assert.AreEqual(Color.FromArgb(255, 123, 178, 181),  colors[6]);
            Assert.AreEqual(Color.FromArgb(255, 57, 117, 140),   colors[7]);
            Assert.AreEqual(Color.FromArgb(255, 123, 178, 181),  colors[8]);
            Assert.AreEqual(Color.FromArgb(255, 181, 239, 222),  colors[9]);
            Assert.AreEqual(Color.FromArgb(255, 123, 178, 181),  colors[10]);
            Assert.AreEqual(Color.FromArgb(255, 181, 239, 222),  colors[11]);
            Assert.AreEqual(Color.FromArgb(255, 57, 117, 140),   colors[12]);
            Assert.AreEqual(Color.FromArgb(255, 181, 239, 222),  colors[13]);
            Assert.AreEqual(Color.FromArgb(255, 181, 239, 222),  colors[14]);
            Assert.AreEqual(Color.FromArgb(255, 123, 178, 181),  colors[15]);
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

            Assert.AreEqual(Color.FromArgb(10, 0, 56, 99),      colors[0]);
            Assert.AreEqual(Color.FromArgb(10, 57, 117, 140),   colors[1]);
            Assert.AreEqual(Color.FromArgb(10, 123, 178, 181),  colors[2]);
            Assert.AreEqual(Color.FromArgb(220, 123, 178, 181), colors[3]);
            Assert.AreEqual(Color.FromArgb(220, 57, 117, 140),  colors[4]);
            Assert.AreEqual(Color.FromArgb(220, 123, 178, 181), colors[5]);
            Assert.AreEqual(Color.FromArgb(10, 123, 178, 181),  colors[6]);
            Assert.AreEqual(Color.FromArgb(220, 57, 117, 140),  colors[7]);
            Assert.AreEqual(Color.FromArgb(220, 123, 178, 181), colors[8]);
            Assert.AreEqual(Color.FromArgb(10, 181, 239, 222),  colors[9]);
            Assert.AreEqual(Color.FromArgb(220, 123, 178, 181), colors[10]);
            Assert.AreEqual(Color.FromArgb(220, 181, 239, 222), colors[11]);
            Assert.AreEqual(Color.FromArgb(220, 57, 117, 140),  colors[12]);
            Assert.AreEqual(Color.FromArgb(10, 181, 239, 222),  colors[13]);
            Assert.AreEqual(Color.FromArgb(220, 181, 239, 222), colors[14]);
            Assert.AreEqual(Color.FromArgb(10, 123, 178, 181),  colors[15]);
        }
    }
}
