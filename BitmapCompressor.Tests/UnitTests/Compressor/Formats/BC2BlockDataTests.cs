using System;
using BitmapCompressor.Formats;
using BitmapCompressor.DataTypes;
using BitmapCompressor.Tests.Helpers;
using NUnit.Framework;

namespace BitmapCompressor.Tests.UnitTests.Compressor.Formats
{
    [TestFixture(Category = "Formats")]
    public class BC2BlockDataTests
    {
        [Test]
        public void ConversionToBytesReturnsArrayOfCorrectSize()
        {
            var block = new BC2BlockData();

            var buffer = block.ToBytes();

            Assert.AreEqual(BlockFormat.BC2ByteSize, buffer.Length);
            CollectionAssert.AreEqual(buffer, new byte[16] {
                0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
                0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 });
        }

        [Test]
        public void ConversionToBytesSetsColor0()
        {
            var color = new Color565Helper(40500);

            var block = new BC2BlockData();
            block.Color0 = color.Color;

            var buffer = block.ToBytes();

            byte c0Low = buffer[8];
            byte c0High = buffer[9];

            Assert.AreEqual(color.LowByte, c0Low);
            Assert.AreEqual(color.HighByte, c0High);
        }

        [Test]
        public void ConversionToBytesSetsColor1()
        {
            var color = new Color565Helper(40500);

            var block = new BC2BlockData();
            block.Color1 = color.Color;

            var buffer = block.ToBytes();

            byte c1Low = buffer[10];
            byte c1High = buffer[11];

            Assert.AreEqual(color.LowByte, c1Low);
            Assert.AreEqual(color.HighByte, c1High);
        }

        [Test]
        public void ConversionToBytesSetsColorIndexes()
        {
            byte expectedIndexes0 = Convert.ToByte("11100100", 2); // d c b a 
            byte expectedIndexes1 = Convert.ToByte("00111001", 2); // h g f e 
            byte expectedIndexes2 = Convert.ToByte("01001110", 2); // l k j i 
            byte expectedIndexes3 = Convert.ToByte("10010011", 2); // p o n m 
                                                                              
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
        public void ConversionToBytesSetsColorAlphas()
        {
            byte expectedAlphas0 = Convert.ToByte("00110010", 2); // d   c   
            byte expectedAlphas1 = Convert.ToByte("00010000", 2); // b   a   
            byte expectedAlphas2 = Convert.ToByte("01110110", 2); // h   g   
            byte expectedAlphas3 = Convert.ToByte("01010100", 2); // f   e   
            byte expectedAlphas4 = Convert.ToByte("10111010", 2); // l   k   
            byte expectedAlphas5 = Convert.ToByte("10011000", 2); // j   i   
            byte expectedAlphas6 = Convert.ToByte("11111110", 2); // p   o   
            byte expectedAlphas7 = Convert.ToByte("11011100", 2); // n   m   

            var block = new BC2BlockData();
                                            // pixel    value    byte
            block.ColorAlphas[0]    = 0;    // a(0,3)   0000     alphas1    
            block.ColorAlphas[1]    = 1;    // b(1,3)   0001     
            block.ColorAlphas[2]    = 2;    // c(2,3)   0010     alphas0    
            block.ColorAlphas[3]    = 3;    // d(3,3)   0011     
            block.ColorAlphas[4]    = 4;    // e(0,2)   0100     alphas2
            block.ColorAlphas[5]    = 5;    // f(1,2)   0101     
            block.ColorAlphas[6]    = 6;    // g(2,2)   0110     alphas3
            block.ColorAlphas[7]    = 7;    // h(3,2)   0111     
            block.ColorAlphas[8]    = 8;    // i(0,1)   1000     alphas4
            block.ColorAlphas[9]    = 9;    // j(1,1)   1001     
            block.ColorAlphas[10]   = 10;   // k(2,1)   1010     alphas5
            block.ColorAlphas[11]   = 11;   // l(3,1)   1011     
            block.ColorAlphas[12]   = 12;   // m(0,0)   1100     alphas6
            block.ColorAlphas[13]   = 13;   // n(1,0)   1101     
            block.ColorAlphas[14]   = 14;   // o(2,0)   1110     alphas7
            block.ColorAlphas[15]   = 15;   // p(3,0)   1111               

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

        [Test]
        public void ConstructionFromBytesSetsColor0()
        {
            var color = new Color565Helper(40500);
            var bytes = new byte[BlockFormat.BC2ByteSize];
            bytes[8] = color.LowByte;
            bytes[9] = color.HighByte;

            var block = BC2BlockData.FromBytes(bytes);

            Assert.AreEqual(color.Color.Value, block.Color0.Value);
        }

        [Test]
        public void ConstructionFromBytesSetsColor1()
        {
            var color = new Color565Helper(40500);
            var bytes = new byte[BlockFormat.BC2ByteSize];
            bytes[10] = color.LowByte;
            bytes[11] = color.HighByte;

            var block = BC2BlockData.FromBytes(bytes);

            Assert.AreEqual(color.Color.Value, block.Color1.Value);
        }

        [Test]
        public void ConstructionFromBytesSetsColorIndexes()
        {
            byte indexes0 = Convert.ToByte("11100100", 2); // d c b a 
            byte indexes1 = Convert.ToByte("00111001", 2); // h g f e 
            byte indexes2 = Convert.ToByte("01001110", 2); // l k j i 
            byte indexes3 = Convert.ToByte("10010011", 2); // p o n m 

            var bytes = new byte[BlockFormat.BC2ByteSize];
            bytes[12] = indexes0;
            bytes[13] = indexes1;
            bytes[14] = indexes2;
            bytes[15] = indexes3;

            var block = BC2BlockData.FromBytes(bytes);

            Assert.AreEqual(0, block.ColorIndexes[0]);  // a    00
            Assert.AreEqual(1, block.ColorIndexes[1]);  // b    01
            Assert.AreEqual(2, block.ColorIndexes[2]);  // c    10
            Assert.AreEqual(3, block.ColorIndexes[3]);  // d    11
            Assert.AreEqual(1, block.ColorIndexes[4]);  // e    01
            Assert.AreEqual(2, block.ColorIndexes[5]);  // f    10
            Assert.AreEqual(3, block.ColorIndexes[6]);  // g    11
            Assert.AreEqual(0, block.ColorIndexes[7]);  // h    00
            Assert.AreEqual(2, block.ColorIndexes[8]);  // i    10
            Assert.AreEqual(3, block.ColorIndexes[9]);  // j    11
            Assert.AreEqual(0, block.ColorIndexes[10]); // k    00
            Assert.AreEqual(1, block.ColorIndexes[11]); // l    01
            Assert.AreEqual(3, block.ColorIndexes[12]); // m    11
            Assert.AreEqual(0, block.ColorIndexes[13]); // n    00
            Assert.AreEqual(1, block.ColorIndexes[14]); // o    01
            Assert.AreEqual(2, block.ColorIndexes[15]); // p    10                    
        }

        [Test]
        public void ConstructionFromBytesSetsColorAlphas()
        {
            byte alphas0 = Convert.ToByte("00110010", 2); // d   c   
            byte alphas1 = Convert.ToByte("00010000", 2); // b   a   
            byte alphas2 = Convert.ToByte("01110110", 2); // h   g   
            byte alphas3 = Convert.ToByte("01010100", 2); // f   e   
            byte alphas4 = Convert.ToByte("10111010", 2); // l   k   
            byte alphas5 = Convert.ToByte("10011000", 2); // j   i   
            byte alphas6 = Convert.ToByte("11111110", 2); // p   o   
            byte alphas7 = Convert.ToByte("11011100", 2); // n   m   

            var bytes = new byte[BlockFormat.BC2ByteSize];
            bytes[0] = alphas0;
            bytes[1] = alphas1;
            bytes[2] = alphas2;
            bytes[3] = alphas3;
            bytes[4] = alphas4;
            bytes[5] = alphas5;
            bytes[6] = alphas6;
            bytes[7] = alphas7;

            var block = BC2BlockData.FromBytes(bytes);

            Assert.AreEqual(0,  block.ColorAlphas[0]);  // a    0000
            Assert.AreEqual(1,  block.ColorAlphas[1]);  // b    0001
            Assert.AreEqual(2,  block.ColorAlphas[2]);  // c    0010
            Assert.AreEqual(3,  block.ColorAlphas[3]);  // d    0011
            Assert.AreEqual(4,  block.ColorAlphas[4]);  // e    0100
            Assert.AreEqual(5,  block.ColorAlphas[5]);  // f    0101
            Assert.AreEqual(6,  block.ColorAlphas[6]);  // g    0110
            Assert.AreEqual(7,  block.ColorAlphas[7]);  // h    0111
            Assert.AreEqual(8,  block.ColorAlphas[8]);  // i    1000
            Assert.AreEqual(9,  block.ColorAlphas[9]);  // j    1001
            Assert.AreEqual(10, block.ColorAlphas[10]); // k    1010
            Assert.AreEqual(11, block.ColorAlphas[11]); // l    1011
            Assert.AreEqual(12, block.ColorAlphas[12]); // m    1100
            Assert.AreEqual(13, block.ColorAlphas[13]); // n    1101
            Assert.AreEqual(14, block.ColorAlphas[14]); // o    1110
            Assert.AreEqual(15, block.ColorAlphas[15]); // p    1111          
        }
    }
}
