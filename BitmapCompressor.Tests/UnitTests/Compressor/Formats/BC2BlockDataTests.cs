using System;
using BitmapCompressor.Formats;
using BitmapCompressor.DataTypes;
using NUnit.Framework;

namespace BitmapCompressor.Tests.UnitTests.Compressor.Formats
{
    [TestFixture(Category = "Formats")]
    public class BC2BlockDataTests
    {
        [Test]
        public void ByteDataIsOfCorrectSize()
        {
            var block = new BC2BlockData();

            var buffer = block.ToBytes();

            Assert.AreEqual(BlockFormat.BC2ByteSize, buffer.Length);
            CollectionAssert.AreEqual(buffer, new byte[16] {
                0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
                0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 });
        }

        [Test]
        public void ByteDataContainsColor0()
        {
            const byte color16Low = 200;
            const byte color16High = 150;
            const ushort color16 = (color16High << 8) + color16Low;

            var block = new BC2BlockData();
            block.Color0 = Color565.FromValue(color16);

            var buffer = block.ToBytes();

            byte color0Low = buffer[8];
            byte color0High = buffer[9];

            Assert.AreEqual(color16Low, color0Low, "Expected 16-bit Color0 low 8-bits to be {0}", color16Low);
            Assert.AreEqual(color16High, color0High, "Expected 16-bit Color0 high 8-bits to be {0}", color16High);
        }

        [Test]
        public void ByteDataContainsColor1()
        {
            const byte color16Low = 200;
            const byte color16High = 150;
            const ushort color16 = (color16High << 8) + color16Low;

            var block = new BC2BlockData();
            block.Color1 = Color565.FromValue(color16);

            var buffer = block.ToBytes();

            byte color1Low = buffer[10];
            byte color1High = buffer[11];

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

            var block = new BC2BlockData();
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

            byte indexes0 = buffer[12];
            byte indexes1 = buffer[13];
            byte indexes2 = buffer[14];
            byte indexes3 = buffer[15];

            Assert.AreEqual(expectedIndexes0, indexes0);
            Assert.AreEqual(expectedIndexes1, indexes1);
            Assert.AreEqual(expectedIndexes2, indexes2);
            Assert.AreEqual(expectedIndexes3, indexes3);
        }

        [Test]
        public void ByteDataContainsColorAlphas()
        {
            byte expectedAlphas0 = Convert.ToByte("00110010", 2); // d___c___
            byte expectedAlphas1 = Convert.ToByte("00010000", 2); // b___a___
            byte expectedAlphas2 = Convert.ToByte("01110110", 2); // h___g___ 
            byte expectedAlphas3 = Convert.ToByte("01010100", 2); // f___e___
            byte expectedAlphas4 = Convert.ToByte("10111010", 2); // l___k___
            byte expectedAlphas5 = Convert.ToByte("10011000", 2); // j___i___
            byte expectedAlphas6 = Convert.ToByte("11111110", 2); // p___o___ 
            byte expectedAlphas7 = Convert.ToByte("11011100", 2); // n___m___

            var block = new BC2BlockData();
                                            // pixel    value    byte
            block.ColorAlphas[0]   = 0;     // a(0,3)   0000     alphas1    
            block.ColorAlphas[1]   = 1;     // b(1,3)   0001     
            block.ColorAlphas[2]   = 2;     // c(2,3)   0010     alphas0    
            block.ColorAlphas[3]   = 3;     // d(3,3)   0011     
            block.ColorAlphas[4]   = 4;     // e(0,2)   0100     alphas2
            block.ColorAlphas[5]   = 5;     // f(1,2)   0101     
            block.ColorAlphas[6]   = 6;     // g(2,2)   0110     alphas3
            block.ColorAlphas[7]   = 7;     // h(3,2)   0111     
            block.ColorAlphas[8]   = 8;     // i(0,1)   1000     alphas4
            block.ColorAlphas[9]   = 9;     // j(1,1)   1001     
            block.ColorAlphas[10]  = 10;    // k(2,1)   1010     alphas5
            block.ColorAlphas[11]  = 11;    // l(3,1)   1011     
            block.ColorAlphas[12]  = 12;    // m(0,0)   1100     alphas6
            block.ColorAlphas[13]  = 13;    // n(1,0)   1101     
            block.ColorAlphas[14]  = 14;    // o(2,0)   1110     alphas7
            block.ColorAlphas[15]  = 15;    // p(3,0)   1111               

            var buffer = block.ToBytes();

            byte alphas0 = buffer[0];
            byte alphas1 = buffer[1];
            byte alphas2 = buffer[2];
            byte alphas3 = buffer[3];
            byte alphas4 = buffer[4];
            byte alphas5 = buffer[5];
            byte alphas6 = buffer[6];
            byte alphas7 = buffer[7];

            Assert.AreEqual(expectedAlphas0, alphas0);
            Assert.AreEqual(expectedAlphas1, alphas1);
            Assert.AreEqual(expectedAlphas2, alphas2);
            Assert.AreEqual(expectedAlphas3, alphas3);
            Assert.AreEqual(expectedAlphas4, alphas4);
            Assert.AreEqual(expectedAlphas5, alphas5);
            Assert.AreEqual(expectedAlphas6, alphas6);
            Assert.AreEqual(expectedAlphas7, alphas7);
        }
    }
}
