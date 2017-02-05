using System;
using System.Drawing;
using BitmapCompressor.DataTypes;
using BitmapCompressor.Formats;
using BitmapCompressor.Tests.Helpers;
using BitmapCompressor.Utilities;
using NUnit.Framework;

namespace BitmapCompressor.Tests.UnitTests.Compressor.Formats
{
    [TestFixture(Category = "Formats")]
    public class BC1FormatTests
    {
        [Test]
        public void CompressionReturnsByteArrayOfCorrectSize()
        {
            var colors = new Color[BlockFormat.TexelCount];

            var data = new BC1Format().Compress(colors);

            Assert.AreEqual(BlockFormat.BC1ByteSize, data.Length);
        }

        [Test]
        public void DecompressionReturnsColorArrayOfCorrectSize()
        {
            var bytes = new byte[BlockFormat.BC1ByteSize];

            var colors = new BC1Format().Decompress(bytes);

            Assert.AreEqual(BlockFormat.TexelCount, colors.Length);
        }

        [Test]
        public void CompressionOrdersReferenceColors()
        {
            var expectedMin     = Color.FromArgb(10, 10, 10);
            var expectedMax     = Color.FromArgb(250, 250, 250);
            var expectedColor0  = ColorUtility.To16Bit(expectedMax);
            var expectedColor1  = ColorUtility.To16Bit(expectedMin);
            var colors          = ColorHelper.CreateRandomColorsBetween(expectedMin, expectedMax);

            // Place expected min and max color in test input
            colors[4]   = expectedMin;   
            colors[10]  = expectedMax;

            var data = new BC1Format().Compress(colors);

            var color0 = Color565.FromValue((ushort) ((data[1] << 8) | data[0]));
            var color1 = Color565.FromValue((ushort) ((data[3] << 8) | data[2]));

            Assert.Greater(color0.Value, color1.Value);
            Assert.AreEqual(expectedColor0.Value, color0.Value);
            Assert.AreEqual(expectedColor1.Value, color1.Value);
        }

        [Test]
        public void CompressionSwitchesReferenceColorOrderWhenAlpha()
        {
            var expectedMin     = Color.FromArgb(10, 10, 10);
            var expectedMax     = Color.FromArgb(250, 250, 250);
            var expectedColor0  = ColorUtility.To16Bit(expectedMin);
            var expectedColor1  = ColorUtility.To16Bit(expectedMax);
            var colors          = ColorHelper.CreateRandomColorsBetween(expectedMin, expectedMax);

            // Place expected min and max color in test input
            colors[4]   = expectedMin;
            colors[10]  = expectedMax;

            // Add an arbitrary alpha value to some color
            ColorHelper.AddAlpha(ref colors[5]);

            var data = new BC1Format().Compress(colors);

            var color0 = Color565.FromValue((ushort) ((data[1] << 8) | data[0]));
            var color1 = Color565.FromValue((ushort) ((data[3] << 8) | data[2]));

            Assert.LessOrEqual(color0.Value, color1.Value);
            Assert.AreEqual(expectedColor0.Value, color0.Value);
            Assert.AreEqual(expectedColor1.Value, color1.Value);
        }

        [Test]
        public void CompressColorsWithoutAlpha()
        {
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

            var data = new BC1Format().Compress(colors);

            // Assert against data hard-coded from a successful test
            // run where the data was built using unit tested components.

            Assert.AreEqual(0x7B, data[0]); // c0Low
            Assert.AreEqual(0xB7, data[1]); // c0Hi
            Assert.AreEqual(0xCC, data[2]); // c1Low
            Assert.AreEqual(0x01, data[3]); // c1Hi
            Assert.AreEqual(0xAD, data[4]); // indexes0
            Assert.AreEqual(0xEB, data[5]); // indexes1
            Assert.AreEqual(0x22, data[6]); // indexes2
            Assert.AreEqual(0x83, data[7]); // indexes3
        }

        [Test]
        public void CompressColorsWithAlpha()
        {
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

            var data = new BC1Format().Compress(colors);

            // Assert against data hard-coded from a successful test
            // run where the data was built using unit tested components.

            Assert.AreEqual(0xCC, data[0]); // c0Low
            Assert.AreEqual(0x01, data[1]); // c0Hi
            Assert.AreEqual(0x7B, data[2]); // c1Low
            Assert.AreEqual(0xB7, data[3]); // c1Hi
            Assert.AreEqual(0xAF, data[4]); // indexes0
            Assert.AreEqual(0xAF, data[5]); // indexes1
            Assert.AreEqual(0x6F, data[6]); // indexes2
            Assert.AreEqual(0x9F, data[7]); // indexes3
        }

        [Test]
        public void DecompressDataWithoutAlpha()
        {
            var bytes = new byte[BlockFormat.BC1ByteSize];
            bytes[0] = 0x7B; // c0Low
            bytes[1] = 0xB7; // c0Hi
            bytes[2] = 0xCC; // c1Low
            bytes[3] = 0x01; // c1Hi
            bytes[4] = 0xAD; // indexes0
            bytes[5] = 0xEB; // indexes1
            bytes[6] = 0x22; // indexes2
            bytes[7] = 0x83; // indexes3

            var colors = new BC1Format().Decompress(bytes);

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
            var bytes = new byte[BlockFormat.BC1ByteSize];
            bytes[0] = 0xCC; // c0Low
            bytes[1] = 0x01; // c0Hi
            bytes[2] = 0x7B; // c1Low
            bytes[3] = 0xB7; // c1Hi
            bytes[4] = 0xAF; // indexes0
            bytes[5] = 0xAF; // indexes1
            bytes[6] = 0x6F; // indexes2
            bytes[7] = 0x9F; // indexes3

            var colors = new BC1Format().Decompress(bytes);

            // Assert against data hard-coded from a successful test
            // run where the data was built using unit tested components.

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
