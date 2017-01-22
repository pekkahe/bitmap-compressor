using System;
using BitmapCompressor.Formats;
using BitmapCompressor.DataTypes;
using NUnit.Framework;

namespace BitmapCompressor.Tests.UnitTests.Compressor.Formats
{
    [TestFixture(Category = "Formats")]
    public class BC1BlockDataTests
    {
        [Test]
        public void ByteDataIsOfCorrectSize()
        {
            var block = new BC1BlockData();

            var buffer = block.ToBytes();

            Assert.AreEqual(BlockFormat.BC1ByteSize, buffer.Length);
            CollectionAssert.AreEqual(buffer, new byte[8] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 });
        }

        [Test]
        public void ByteDataContainsColor0()
        {
            const byte color16Low = 200;
            const byte color16High = 150;
            const ushort color16 = (color16High << 8) + color16Low;

            var block = new BC1BlockData();
            block.Color0 = Color565.FromValue(color16);
            var buffer = block.ToBytes();

            byte color0Low = buffer[0];
            byte color0High = buffer[1];

            Assert.AreEqual(color16Low, color0Low, "Expected 16-bit Color0 low 8-bits to be {0}", color16Low);
            Assert.AreEqual(color16High, color0High, "Expected 16-bit Color0 high 8-bits to be {0}", color16High);
        }

        [Test]
        public void ByteDataContainsColor1()
        {
            const byte color16Low = 200;
            const byte color16High = 150;
            const ushort color16 = (color16High << 8) + color16Low;

            var block = new BC1BlockData();
            block.Color1 = Color565.FromValue(color16);

            var buffer = block.ToBytes();

            byte color1Low = buffer[2];
            byte color1High = buffer[3];

            Assert.AreEqual(color16Low, color1Low, "Expected 16-bit Color1 low 8-bits to be {0}", color16Low);
            Assert.AreEqual(color16High, color1High, "Expected 16-bit Color1 high 8-bits to be {0}", color16High);
        }

        [Test]
        public void ByteDataContainsColorIndexes()
        {
            byte expectedIndexes0 = Convert.ToByte("11100100", 2); // d_c_b_a_
            byte expectedIndexes1 = Convert.ToByte("00111001", 2); // h_g_f_e_
            byte expectedIndexes2 = Convert.ToByte("01001110", 2); // l_k_j_i_
            byte expectedIndexes3 = Convert.ToByte("10010011", 2); // p_o_n_m_

            var block = new BC1BlockData();
                                            // pixel    value    byte
            block.ColorIndexes[0]   = 0;    // a(0,3)   00       indexes0
            block.ColorIndexes[1]   = 1;    // b(1,3)   01       
            block.ColorIndexes[2]   = 2;    // c(2,3)   10       
            block.ColorIndexes[3]   = 3;    // d(3,3)   11              
            block.ColorIndexes[4]   = 1;    // e(0,2)   01       indexes1
            block.ColorIndexes[5]   = 2;    // f(1,2)   10       
            block.ColorIndexes[6]   = 3;    // g(2,2)   11       
            block.ColorIndexes[7]   = 0;    // h(3,2)   00       
            block.ColorIndexes[8]   = 2;    // i(0,1)   10       indexes2
            block.ColorIndexes[9]   = 3;    // j(1,1)   11       
            block.ColorIndexes[10]  = 0;    // k(2,1)   00       
            block.ColorIndexes[11]  = 1;    // l(3,1)   01       
            block.ColorIndexes[12]  = 3;    // m(0,0)   11       indexes3   
            block.ColorIndexes[13]  = 0;    // n(1,0)   00    
            block.ColorIndexes[14]  = 1;    // o(2,0)   01    
            block.ColorIndexes[15]  = 2;    // p(3,0)   10    

            var buffer = block.ToBytes();

            byte indexes0 = buffer[4];
            byte indexes1 = buffer[5];
            byte indexes2 = buffer[6];
            byte indexes3 = buffer[7];

            Assert.AreEqual(expectedIndexes0, indexes0);
            Assert.AreEqual(expectedIndexes1, indexes1);
            Assert.AreEqual(expectedIndexes2, indexes2);
            Assert.AreEqual(expectedIndexes3, indexes3);
        }
    }
}
