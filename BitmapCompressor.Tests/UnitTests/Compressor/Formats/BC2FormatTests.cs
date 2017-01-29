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

                                             //                8-bit     4-bit
            Assert.AreEqual(0xF5, data[0]);  // alphas0 [d c] [250  80] [15  5] -> [1111 0101]
            Assert.AreEqual(0x63, data[1]);  // alphas1 [b a] [100  55] [6   3] -> [0110 0011]
            Assert.AreEqual(0xF1, data[2]);  // alphas2 [h g] [255  30] [15  1] -> [1111 0001]
            Assert.AreEqual(0xCB, data[3]);  // alphas3 [f e] [200 190] [12 13] -> [1100 1011]
            Assert.AreEqual(0x8A, data[4]);  // alphas4 [l k] [140 160] [8  10] -> [1000 1010]
            Assert.AreEqual(0x27, data[5]);  // alphas5 [j i] [40  120] [2   7] -> [0010 0111]
            Assert.AreEqual(0x4D, data[6]);  // alphas6 [p o] [70  220] [4  13] -> [0100 1101]
            Assert.AreEqual(0x08, data[7]);  // alphas7 [n m] [10  130] [0   8] -> [0000 1000]
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
        public void DecompressionOfBytesWithoutAlpha()
        {
            var bytes = new byte[BlockFormat.BC2ByteSize];
            bytes[0]    = 0xFF; // alphas0
            bytes[1]    = 0xFF; // alphas1
            bytes[2]    = 0xFF; // alphas2
            bytes[3]    = 0xFF; // alphas3
            bytes[4]    = 0xFF; // alphas4
            bytes[5]    = 0xFF; // alphas5
            bytes[6]    = 0xFF; // alphas6
            bytes[7]    = 0xFF; // alphas7
            bytes[8]    = 0x7B; // c0Low
            bytes[9]    = 0xB7; // c0Hi
            bytes[10]   = 0xCC; // c1Low
            bytes[11]   = 0x01; // c1Hi
            bytes[12]   = 0xAD; // indexes0
            bytes[13]   = 0xEB; // indexes1
            bytes[14]   = 0x22; // indexes2
            bytes[15]   = 0x83; // indexes3

            var colors = new BC2Format().Decompress(bytes);

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
        public void DecompressionOfBytesWithAlpha()
        {
            var bytes   = new byte[BlockFormat.BC2ByteSize];

                                //                            4-bit      8-bit
            bytes[0]    = 0xF5; // alphas0 [d c] [1111 0101] [15  5] -> [255  85]
            bytes[1]    = 0x63; // alphas1 [b a] [0110 0011] [6   3] -> [102  51]
            bytes[2]    = 0xF1; // alphas2 [h g] [1111 0001] [15  1] -> [255  17]
            bytes[3]    = 0xCB; // alphas3 [f e] [1100 1011] [12 13] -> [204 187]
            bytes[4]    = 0x8A; // alphas4 [l k] [1000 1010] [8  10] -> [136 170]
            bytes[5]    = 0x27; // alphas5 [j i] [0010 0111] [2   7] -> [34  119]
            bytes[6]    = 0x4D; // alphas6 [p o] [0100 1101] [4  13] -> [68  221]
            bytes[7]    = 0x08; // alphas7 [n m] [0000 1000] [0   8] -> [ 0  136]
            bytes[8]    = 0x7B; // c0Low
            bytes[9]    = 0xB7; // c0Hi
            bytes[10]   = 0xCC; // c1Low
            bytes[11]   = 0x01; // c1Hi
            bytes[12]   = 0xAD; // indexes0
            bytes[13]   = 0xEB; // indexes1
            bytes[14]   = 0x22; // indexes2
            bytes[15]   = 0x83; // indexes3

            var colors = new BC2Format().Decompress(bytes);

            Assert.AreEqual(Color.FromArgb(51, 0, 56, 99),      colors[0]);
            Assert.AreEqual(Color.FromArgb(102, 57, 117, 140),  colors[1]);
            Assert.AreEqual(Color.FromArgb(85, 123, 178, 181),  colors[2]);
            Assert.AreEqual(Color.FromArgb(255, 123, 178, 181), colors[3]);
            Assert.AreEqual(Color.FromArgb(187, 57, 117, 140),  colors[4]);
            Assert.AreEqual(Color.FromArgb(204, 123, 178, 181), colors[5]);
            Assert.AreEqual(Color.FromArgb(17, 123, 178, 181),  colors[6]);
            Assert.AreEqual(Color.FromArgb(255, 57, 117, 140),  colors[7]);
            Assert.AreEqual(Color.FromArgb(119, 123, 178, 181), colors[8]);
            Assert.AreEqual(Color.FromArgb(34, 181, 239, 222),  colors[9]);
            Assert.AreEqual(Color.FromArgb(170, 123, 178, 181), colors[10]);
            Assert.AreEqual(Color.FromArgb(136, 181, 239, 222), colors[11]);
            Assert.AreEqual(Color.FromArgb(136, 57, 117, 140),  colors[12]);
            Assert.AreEqual(Color.FromArgb(0, 181, 239, 222),   colors[13]);
            Assert.AreEqual(Color.FromArgb(221, 181, 239, 222), colors[14]);
            Assert.AreEqual(Color.FromArgb(68, 123, 178, 181),  colors[15]);
        }
    }
}
