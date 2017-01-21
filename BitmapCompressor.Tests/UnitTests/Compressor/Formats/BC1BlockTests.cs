using System;
using BitmapCompressor.Formats;
using BitmapCompressor.DataTypes;
using NUnit.Framework;

namespace BitmapCompressor.Tests.UnitTests.Compressor.Formats
{
    [TestFixture(Category = "Formats")]
    public class BC1BlockTests
    {
        [Test]
        public void ReturnsByteArrayOfCorrectSize()
        {
            var block = new BC1Block();

            var buffer = block.ToBytes();

            Assert.AreEqual(BlockFormat.BC1ByteSize, buffer.Length);
            CollectionAssert.AreEqual(buffer, new byte[8] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 });
        }

        [Test]
        public void SettingColor0()
        {
            const byte color16Low = 200;
            const byte color16High = 150;
            const ushort color16 = (color16High << 8) + color16Low;
            var block = new BC1Block();

            block.Color0 = Color565.FromValue(color16);

            var buffer = block.ToBytes();
            byte color0Low = buffer[0];
            byte color0High = buffer[1];

            Assert.AreEqual(color16Low, color0Low, "Expected 16-bit Color0 low 8-bits to be {0}", color16Low);
            Assert.AreEqual(color16High, color0High, "Expected 16-bit Color0 high 8-bits to be {0}", color16High);
        }

        [Test]
        public void GettingColor0()
        {
            var color16 = Color565.FromRgb(30, 50, 30);
            var block = new BC1Block();
            block.Color0 = color16;

            var color0 = block.Color0;

            Assert.AreEqual(color16, color0);
        }

        [Test]
        public void SettingColor1()
        {
            const byte color16Low = 200;
            const byte color16High = 150;
            const ushort color16 = (color16High << 8) + color16Low;
            var block = new BC1Block();

            block.Color1 = Color565.FromValue(color16);

            var buffer = block.ToBytes();
            byte color1Low = buffer[2];
            byte color1High = buffer[3];
            
            Assert.AreEqual(color16Low, color1Low, "Expected 16-bit Color1 low 8-bits to be {0}", color16Low);
            Assert.AreEqual(color16High, color1High, "Expected 16-bit Color1 high 8-bits to be {0}", color16High);
        }

        [Test]
        public void GettingColor1()
        {
            var color16 = Color565.FromRgb(30, 50, 30);
            var block = new BC1Block();
            block.Color1 = color16;

            var color1 = block.Color1;

            Assert.AreEqual(color16, color1);
        }

        [Test]
        public void SettingColorIndexes()
        {
            var block = new BC1Block();

            // Setup the following color index 32-bit test layout:
            //    codes0        codes1       codes2        codes3
            // 11 10 01 00 | 00 11 10 01 | 01 00 11 10 | 10 01 00 11 
            byte expectedCodes0 = Convert.ToByte("11100100", 2);
            byte expectedCodes1 = Convert.ToByte("00111001", 2);
            byte expectedCodes2 = Convert.ToByte("01001110", 2);
            byte expectedCodes3 = Convert.ToByte("10010011", 2);

                                          // pixel  | codes
            block.ColorIndexes[0] =  0x0; // p(0,0)   codes0 LSB
            block.ColorIndexes[1] =  0x1;
            block.ColorIndexes[2] =  0x2;
            block.ColorIndexes[3] =  0x3; // p(3,0)   codes0 MSB
            block.ColorIndexes[4] =  0x1; // p(0,1)   codes1 LSB
            block.ColorIndexes[5] =  0x2;
            block.ColorIndexes[6] =  0x3;
            block.ColorIndexes[7] =  0x0; // p(3,1)   codes1 MSB
            block.ColorIndexes[8] =  0x2; // p(0,2)   codes2 LSB
            block.ColorIndexes[9] =  0x3;
            block.ColorIndexes[10] = 0x0;
            block.ColorIndexes[11] = 0x1; // p(3,2)   codes2 MSB
            block.ColorIndexes[12] = 0x3; // p(0,3)   codes3 LSB
            block.ColorIndexes[13] = 0x0;
            block.ColorIndexes[14] = 0x1;
            block.ColorIndexes[15] = 0x2; // p(3,3)   codes3 MSB

            var buffer = block.ToBytes();
            byte codes0 = buffer[4];
            byte codes1 = buffer[5];
            byte codes2 = buffer[6];
            byte codes3 = buffer[7];

            Assert.AreEqual(expectedCodes0, codes0);
            Assert.AreEqual(expectedCodes1, codes1);
            Assert.AreEqual(expectedCodes2, codes2);
            Assert.AreEqual(expectedCodes3, codes3);
        }

        [Test]
        public void GettingColorIndexes()
        {
            var block = new BC1Block();
            var expectedIndexes = new int[16]
            {
                0x0, 0x1, 0x2, 0x3, 0x1, 0x2, 0x3, 0x0,
                0x2, 0x3, 0x0, 0x1, 0x3, 0x0, 0x1, 0x2
            };

            for (int i = 0; i < expectedIndexes.Length; ++i)
            {
                block.ColorIndexes[i] = expectedIndexes[i];

                var index = block.ColorIndexes[i];

                Assert.AreEqual(expectedIndexes[i], index);
            }
        }
    }
}
