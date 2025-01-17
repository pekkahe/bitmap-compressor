using System;
using System.Drawing;
using BitmapCompressor.Formats;
using BitmapCompressor.Tests.Helpers;
using NUnit.Framework;

namespace BitmapCompressor.Tests.UnitTests.Compressor.Formats
{
    [TestFixture(Category = "Formats")]
    public class BC3BlockDataTests
    {
        [Test]
        public void ConversionToBytesReturnsArrayOfCorrectSize()
        {
            var block = new BC3BlockData();

            var buffer = block.ToBytes();

            Assert.That(buffer.Length, Is.EqualTo(BlockFormat.BC3ByteSize));
            Assert.That(buffer, Is.EquivalentTo(new byte[BlockFormat.BC3ByteSize]));
        }

        [Test]
        public void ConversionToBytesSetsColor0()
        {
            var color = new Color565Helper(40500);

            var block = new BC3BlockData();
            block.Color0 = color.Color;

            var buffer = block.ToBytes();

            byte c0Low = buffer[8];
            byte c0High = buffer[9];

            Assert.That(c0Low, Is.EqualTo(color.LowByte));
            Assert.That(c0High, Is.EqualTo(color.HighByte));
        }

        [Test]
        public void ConversionToBytesSetsColor1()
        {
            var color = new Color565Helper(40500);

            var block = new BC3BlockData();
            block.Color1 = color.Color;

            var buffer = block.ToBytes();

            byte c1Low = buffer[10];
            byte c1High = buffer[11];

            Assert.That(c1Low, Is.EqualTo(color.LowByte));
            Assert.That(c1High, Is.EqualTo(color.HighByte));
        }

        [Test]
        public void ConversionToBytesSetsAlpha0()
        {
            const byte alphaValue = 215;

            var block = new BC3BlockData();
            block.Alpha0 = alphaValue;

            var buffer = block.ToBytes();

            byte alpha0 = buffer[0];

            Assert.That(alpha0, Is.EqualTo(alphaValue));
        }

        [Test]
        public void ConversionToBytesSetsAlpha1()
        {
            const byte alphaValue = 215;

            var block = new BC3BlockData();
            block.Alpha1 = alphaValue;

            var buffer = block.ToBytes();

            byte alpha1 = buffer[1];

            Assert.That(alpha1, Is.EqualTo(alphaValue));
        }

        [Test]
        public void ConversionToBytesSetsColorIndexes()
        {
            byte expectedIndexes0 = "11 10 01 00".AsByte(); // d c b a 
            byte expectedIndexes1 = "00 11 10 01".AsByte(); // h g f e 
            byte expectedIndexes2 = "01 00 11 10".AsByte(); // l k j i 
            byte expectedIndexes3 = "10 01 00 11".AsByte(); // p o n m 

            var block = new BC3BlockData();
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

            Assert.That(indexes0, Is.EqualTo(expectedIndexes0));
            Assert.That(indexes1, Is.EqualTo(expectedIndexes1));
            Assert.That(indexes2, Is.EqualTo(expectedIndexes2));
            Assert.That(indexes3, Is.EqualTo(expectedIndexes3));
        }
        
        [Test]
        public void ConversionToBytesSetsAlphaIndexes()
        {
            // Increment until max value, then decrement
            byte expectedAlphas0 = "10  001 000".AsByte(); // c b a     
            byte expectedAlphas1 = "1 100 011 0".AsByte(); // f e d c   
            byte expectedAlphas2 = "111 110  10".AsByte(); // h g f
            byte expectedAlphas3 = "01  110 111".AsByte(); // k j i            
            byte expectedAlphas4 = "0 011 100 1".AsByte(); // n m l k  
            byte expectedAlphas5 = "000 001  01".AsByte(); // p o n  

            var block = new BC3BlockData();
            block.AlphaIndexes[0]   = 0;    // a    000  
            block.AlphaIndexes[1]   = 1;    // b    001
            block.AlphaIndexes[2]   = 2;    // c    010
            block.AlphaIndexes[3]   = 3;    // d    011
            block.AlphaIndexes[4]   = 4;    // e    100
            block.AlphaIndexes[5]   = 5;    // f    101
            block.AlphaIndexes[6]   = 6;    // g    110
            block.AlphaIndexes[7]   = 7;    // h    111
            block.AlphaIndexes[8]   = 7;    // i    111
            block.AlphaIndexes[9]   = 6;    // j    110
            block.AlphaIndexes[10]  = 5;    // k    101
            block.AlphaIndexes[11]  = 4;    // l    100
            block.AlphaIndexes[12]  = 3;    // m    011
            block.AlphaIndexes[13]  = 2;    // n    010
            block.AlphaIndexes[14]  = 1;    // o    001
            block.AlphaIndexes[15]  = 0;    // p    000

            var buffer = block.ToBytes();

            byte alphas2 = buffer[2];
            byte alphas1 = buffer[3];
            byte alphas0 = buffer[4];
            byte alphas5 = buffer[5];
            byte alphas4 = buffer[6];
            byte alphas3 = buffer[7];

            Assert.That(alphas0, Is.EqualTo(expectedAlphas0));
            Assert.That(alphas1, Is.EqualTo(expectedAlphas1));
            Assert.That(alphas2, Is.EqualTo(expectedAlphas2));
            Assert.That(alphas3, Is.EqualTo(expectedAlphas3));
            Assert.That(alphas4, Is.EqualTo(expectedAlphas4));
            Assert.That(alphas5, Is.EqualTo(expectedAlphas5));
        }

        [Test]
        public void ConstructionFromBytesSetsColor0()
        {
            var color = new Color565Helper(40500);
            var bytes = new byte[BlockFormat.BC2ByteSize];
            bytes[8] = color.LowByte;
            bytes[9] = color.HighByte;

            var block = BC3BlockData.FromBytes(bytes);

            Assert.That(block.Color0.Value, Is.EqualTo(color.Color.Value));
        }

        [Test]
        public void ConstructionFromBytesSetsColor1()
        {
            var color = new Color565Helper(40500);
            var bytes = new byte[BlockFormat.BC2ByteSize];
            bytes[10] = color.LowByte;
            bytes[11] = color.HighByte;

            var block = BC3BlockData.FromBytes(bytes);

            Assert.That(block.Color1.Value, Is.EqualTo(color.Color.Value));
        }

        [Test]
        public void ConstructionFromBytesSetsAlpha0()
        {
            const byte alphaValue = 215;

            var bytes = new byte[BlockFormat.BC3ByteSize];
            bytes[0] = alphaValue;

            var block = BC3BlockData.FromBytes(bytes);

            Assert.That(block.Alpha0, Is.EqualTo(alphaValue));
        }

        [Test]
        public void ConstructionFromBytesSetsAlpha1()
        {
            const byte alphaValue = 215;

            var bytes = new byte[BlockFormat.BC3ByteSize];
            bytes[1] = alphaValue;

            var block = BC3BlockData.FromBytes(bytes);

            Assert.That(block.Alpha1, Is.EqualTo(alphaValue));
        }

        [Test]
        public void ConstructionFromBytesSetsColorIndexes()
        {
            byte indexes0 = "11 10 01 00".AsByte(); // d c b a 
            byte indexes1 = "00 11 10 01".AsByte(); // h g f e 
            byte indexes2 = "01 00 11 10".AsByte(); // l k j i 
            byte indexes3 = "10 01 00 11".AsByte(); // p o n m 

            var bytes = new byte[BlockFormat.BC3ByteSize];
            bytes[12] = indexes0;
            bytes[13] = indexes1;
            bytes[14] = indexes2;
            bytes[15] = indexes3;

            var block = BC3BlockData.FromBytes(bytes);

            Assert.That(block.ColorIndexes[0], Is.EqualTo(0));  // a    00
            Assert.That(block.ColorIndexes[1], Is.EqualTo(1));  // b    01
            Assert.That(block.ColorIndexes[2], Is.EqualTo(2));  // c    10
            Assert.That(block.ColorIndexes[3], Is.EqualTo(3));  // d    11
            Assert.That(block.ColorIndexes[4], Is.EqualTo(1));  // e    01
            Assert.That(block.ColorIndexes[5], Is.EqualTo(2));  // f    10
            Assert.That(block.ColorIndexes[6], Is.EqualTo(3));  // g    11
            Assert.That(block.ColorIndexes[7], Is.EqualTo(0));  // h    00
            Assert.That(block.ColorIndexes[8], Is.EqualTo(2));  // i    10
            Assert.That(block.ColorIndexes[9], Is.EqualTo(3));  // j    11
            Assert.That(block.ColorIndexes[10], Is.EqualTo(0)); // k    00
            Assert.That(block.ColorIndexes[11], Is.EqualTo(1)); // l    01
            Assert.That(block.ColorIndexes[12], Is.EqualTo(3)); // m    11
            Assert.That(block.ColorIndexes[13], Is.EqualTo(0)); // n    00
            Assert.That(block.ColorIndexes[14], Is.EqualTo(1)); // o    01
            Assert.That(block.ColorIndexes[15], Is.EqualTo(2)); // p    10                    
        }
        
        [Test]
        public void ConstructionFromBytesSetsAlphaIndexes()
        {
            // Increment until max value, then decrement
            byte alphas0 = "10  001 000".AsByte(); // c b a     
            byte alphas1 = "1 100 011 0".AsByte(); // f e d c   
            byte alphas2 = "111 110  10".AsByte(); // h g f
            byte alphas3 = "01  110 111".AsByte(); // k j i          
            byte alphas4 = "0 011 100 1".AsByte(); // n m l k  
            byte alphas5 = "000 001  01".AsByte(); // p o n  

            var bytes = new byte[BlockFormat.BC3ByteSize];
            bytes[2] = alphas2;
            bytes[3] = alphas1;
            bytes[4] = alphas0;
            bytes[5] = alphas5;
            bytes[6] = alphas4;
            bytes[7] = alphas3;

            var block = BC3BlockData.FromBytes(bytes);

            Assert.That(block.AlphaIndexes[0], Is.EqualTo(0));  // a    000  
            Assert.That(block.AlphaIndexes[1], Is.EqualTo(1));  // b    001
            Assert.That(block.AlphaIndexes[2], Is.EqualTo(2));  // c    010
            Assert.That(block.AlphaIndexes[3], Is.EqualTo(3));  // d    011
            Assert.That(block.AlphaIndexes[4], Is.EqualTo(4));  // e    100
            Assert.That(block.AlphaIndexes[5], Is.EqualTo(5));  // f    101
            Assert.That(block.AlphaIndexes[6], Is.EqualTo(6));  // g    110
            Assert.That(block.AlphaIndexes[7], Is.EqualTo(7));  // h    111
            Assert.That(block.AlphaIndexes[8], Is.EqualTo(7));  // i    111
            Assert.That(block.AlphaIndexes[9], Is.EqualTo(6));  // j    110
            Assert.That(block.AlphaIndexes[10], Is.EqualTo(5)); // k    101
            Assert.That(block.AlphaIndexes[11], Is.EqualTo(4)); // l    100
            Assert.That(block.AlphaIndexes[12], Is.EqualTo(3)); // m    011
            Assert.That(block.AlphaIndexes[13], Is.EqualTo(2)); // n    010
            Assert.That(block.AlphaIndexes[14], Is.EqualTo(1)); // o    001
            Assert.That(block.AlphaIndexes[15], Is.EqualTo(0)); // p    000              
        }
    }
}
