using System;
using BitmapCompressor.Formats;
using BitmapCompressor.DataTypes;
using BitmapCompressor.Tests.Helpers;
using NUnit.Framework;

namespace BitmapCompressor.Tests.UnitTests.Compressor.Formats
{
    [TestFixture(Category = "Formats")]
    public class BC1BlockDataTests
    {
        [Test]
        public void ConversionToBytesReturnsArrayOfCorrectSize()
        {
            var block = new BC1BlockData();

            var buffer = block.ToBytes();

            Assert.AreEqual(BlockFormat.BC1ByteSize, buffer.Length);
            CollectionAssert.AreEqual(buffer, new byte[8] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 });
        }

        [Test]
        public void ConversionToBytesSetsColor0()
        {
            var color = new Color565Helper(40500);

            var block = new BC1BlockData();
            block.Color0 = color.Color;

            var buffer = block.ToBytes();

            byte c0Low = buffer[0];
            byte c0Hi = buffer[1];

            Assert.AreEqual(color.LowByte, c0Low);
            Assert.AreEqual(color.HighByte, c0Hi);
        }

        [Test]
        public void ConversionToBytesSetsColor1()
        {
            var color = new Color565Helper(40500);

            var block = new BC1BlockData();
            block.Color1 = color.Color;

            var buffer = block.ToBytes();

            byte c1Low = buffer[2];
            byte c1Hi = buffer[3];

            Assert.AreEqual(color.LowByte, c1Low);
            Assert.AreEqual(color.HighByte, c1Hi);
        }

        [Test]
        public void ConversionToBytesSetsColorIndexes()
        {
            byte expectedIndexes0 = Convert.ToByte("11100100", 2); // d c b a
            byte expectedIndexes1 = Convert.ToByte("00111001", 2); // h g f e
            byte expectedIndexes2 = Convert.ToByte("01001110", 2); // l k j i
            byte expectedIndexes3 = Convert.ToByte("10010011", 2); // p o n m  

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

        [Test]
        public void ConstructionFromBytesSetsColor0()
        {
            var color = new Color565Helper(40500);
            var bytes = new byte[BlockFormat.BC1ByteSize];
            bytes[0] = color.LowByte;
            bytes[1] = color.HighByte;

            var block = BC1BlockData.FromBytes(bytes);

            Assert.AreEqual(color.Color.Value, block.Color0.Value);
        }

        [Test]
        public void ConstructionFromBytesSetsColor1()
        {
            var color = new Color565Helper(40500);
            var bytes = new byte[BlockFormat.BC1ByteSize];
            bytes[2] = color.LowByte;
            bytes[3] = color.HighByte;

            var block = BC1BlockData.FromBytes(bytes);

            Assert.AreEqual(color.Color.Value, block.Color1.Value);
        }

        [Test]
        public void ConstructionFromBytesSetsColorIndexes()
        {
            byte indexes0 = Convert.ToByte("11100100", 2); // d c b a 
            byte indexes1 = Convert.ToByte("00111001", 2); // h g f e 
            byte indexes2 = Convert.ToByte("01001110", 2); // l k j i 
            byte indexes3 = Convert.ToByte("10010011", 2); // p o n m 

            var bytes = new byte[BlockFormat.BC1ByteSize];
            bytes[4] = indexes0;
            bytes[5] = indexes1;
            bytes[6] = indexes2;
            bytes[7] = indexes3;

            var block = BC1BlockData.FromBytes(bytes);

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
    }
}
