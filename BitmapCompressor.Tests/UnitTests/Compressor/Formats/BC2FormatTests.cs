using System;
using System.Drawing;
using BitmapCompressor.Formats;
using BitmapCompressor.Tests.Helpers;
using NUnit.Framework;
using BitmapCompressor.Utilities;
using BitmapCompressor.DataTypes;

namespace BitmapCompressor.Tests.UnitTests.Compressor.Formats
{
    [TestFixture(Category = "Formats")]
    public class BC2FormatTests
    {
        [Test]
        public void CompressionReturnsByteArrayOfCorrectSize()
        {
            var colors = new Color[BlockFormat.PixelCount];

            var data = new BC2Format().Compress(colors);

            Assert.AreEqual(BlockFormat.BC2ByteSize, data.Length);
        }

        [Test]
        public void DecompressionReturnsColorArrayOfCorrectSize()
        {
            var bytes = new byte[BlockFormat.BC2ByteSize];

            var colors = new BC2Format().Decompress(bytes);

            Assert.AreEqual(BlockFormat.PixelCount, colors.Length);
        }

        [Test]
        public void CompressionSetsReferenceColors()
        {
            var expectedMin     = Color.FromArgb(10, 10, 10);
            var expectedMax     = Color.FromArgb(250, 250, 250);
            var expectedColor0  = ColorUtility.To16Bit(expectedMax);
            var expectedColor1  = ColorUtility.To16Bit(expectedMin);
            var colors          = ColorHelper.CreateRandomColorsBetween(expectedMin, expectedMax);

            // Place expected min and max color in test input
            colors[4]   = expectedMin;
            colors[10]  = expectedMax;

            var data = new BC2Format().Compress(colors);

            var color0 = Color565.FromValue((ushort) ((data[9]  << 8) | data[8]));
            var color1 = Color565.FromValue((ushort) ((data[11] << 8) | data[10]));

            Assert.Greater(color0.Value, color1.Value);
            Assert.AreEqual(expectedColor0.Value, color0.Value);
            Assert.AreEqual(expectedColor1.Value, color1.Value);
        }

        [Test]
        public void CompressionOfColorsWithoutAlpha()
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

            var data = new BC2Format().Compress(colors);

            Assert.AreEqual(0xFF, data[0]);  // alphas0
            Assert.AreEqual(0xFF, data[1]);  // alphas1
            Assert.AreEqual(0xFF, data[2]);  // alphas2
            Assert.AreEqual(0xFF, data[3]);  // alphas3
            Assert.AreEqual(0xFF, data[4]);  // alphas4
            Assert.AreEqual(0xFF, data[5]);  // alphas5
            Assert.AreEqual(0xFF, data[6]);  // alphas6
            Assert.AreEqual(0xFF, data[7]);  // alphas7
            Assert.AreEqual(0x7B, data[8]);  // c0Low
            Assert.AreEqual(0xB7, data[9]);  // c0Hi
            Assert.AreEqual(0xCC, data[10]); // c1Low
            Assert.AreEqual(0x01, data[11]); // c1Hi
            Assert.AreEqual(0xAD, data[12]); // indexes0
            Assert.AreEqual(0xEB, data[13]); // indexes1
            Assert.AreEqual(0x22, data[14]); // indexes2
            Assert.AreEqual(0x83, data[15]); // indexes3
        }

        [Test]
        public void CompressionOfColorsWithAlpha()
        {
            var colors = new Color[16];
            colors[0]   = Color.FromArgb(55, 3, 59, 101);
            colors[1]   = Color.FromArgb(100, 131, 154, 20);
            colors[2]   = Color.FromArgb(80, 87, 192, 127);
            colors[3]   = Color.FromArgb(250, 203, 148, 14);
            colors[4]   = Color.FromArgb(190, 237, 62, 69);
            colors[5]   = Color.FromArgb(200, 173, 159, 102);
            colors[6]   = Color.FromArgb(30, 176, 199, 73);
            colors[7]   = Color.FromArgb(255, 118, 87, 228);
            colors[8]   = Color.FromArgb(120, 79, 169, 87);
            colors[9]   = Color.FromArgb(40, 179, 238, 219);
            colors[10]  = Color.FromArgb(160, 70, 207, 12);
            colors[11]  = Color.FromArgb(140, 81, 243, 109);
            colors[12]  = Color.FromArgb(130, 38, 79, 189);
            colors[13]  = Color.FromArgb(10, 249, 229, 125);
            colors[14]  = Color.FromArgb(220, 205, 237, 57);
            colors[15]  = Color.FromArgb(70, 190, 158, 111);

            var data = new BC2Format().Compress(colors);

                                             //                 8-bit   4-bit
            Assert.AreEqual(0xF5, data[0]);  // alphas0     d   250     15      1111
                                             //             c   80      5       0101
            Assert.AreEqual(0x63, data[1]);  // alphas1     b   100     6       0110
                                             //             a   55      3       0011
            Assert.AreEqual(0xF1, data[2]);  // alphas2     h   255     15      1111
                                             //             g   30      1       0001
            Assert.AreEqual(0xCB, data[3]);  // alphas3     f   200     12      1100
                                             //             e   190     13      1011
            Assert.AreEqual(0x8A, data[4]);  // alphas4     l   140     8       1000
                                             //             k   160     10      1010
            Assert.AreEqual(0x27, data[5]);  // alphas5     j   40      2       0010
                                             //             i   120     7       0111
            Assert.AreEqual(0x4D, data[6]);  // alphas6     p   70      4       0100
                                             //             o   220     13      1101
            Assert.AreEqual(0x08, data[7]);  // alphas7     n   10      0       0000
                                             //             m   130     8       1000
            Assert.AreEqual(0x7B, data[8]);  // c0Low
            Assert.AreEqual(0xB7, data[9]);  // c0Hi
            Assert.AreEqual(0xCC, data[10]); // c1Low
            Assert.AreEqual(0x01, data[11]); // c1Hi
            Assert.AreEqual(0xAD, data[12]); // indexes0
            Assert.AreEqual(0xEB, data[13]); // indexes1
            Assert.AreEqual(0x22, data[14]); // indexes2
            Assert.AreEqual(0x83, data[15]); // indexes3
        }

        // TODO: Decompression
    }
}
