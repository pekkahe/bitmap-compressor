using System;
using System.Drawing;
using BitmapCompressor.Lib.Formats;
using BitmapCompressor.Tests.Helpers;
using NUnit.Framework;

namespace BitmapCompressor.Tests.UnitTests.Compressor.Formats;

[TestFixture(Category = "Formats")]
public class BC2BlockDataTests
{
    [Test]
    public void ConversionToBytesReturnsArrayOfCorrectSize()
    {
        var block = new BC2BlockData();

        var buffer = block.ToBytes();

        Assert.That(buffer.Length, Is.EqualTo(BlockFormat.BC2ByteSize));
        Assert.That(buffer, Is.EquivalentTo(new byte[BlockFormat.BC2ByteSize]));
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

        Assert.That(c0Low, Is.EqualTo(color.LowByte));
        Assert.That(c0High, Is.EqualTo(color.HighByte));
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

        Assert.That(c1Low, Is.EqualTo(color.LowByte));
        Assert.That(c1High, Is.EqualTo(color.HighByte));
    }

    [Test]
    public void ConversionToBytesSetsColorIndexes()
    {
        byte expectedIndexes0 = "11 10 01 00".AsByte(); // d c b a 
        byte expectedIndexes1 = "00 11 10 01".AsByte(); // h g f e 
        byte expectedIndexes2 = "01 00 11 10".AsByte(); // l k j i 
        byte expectedIndexes3 = "10 01 00 11".AsByte(); // p o n m 
                                                                          
        var block = new BC2BlockData();
                                        // texel    value    byte
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
    public void ConversionToBytesSetsColorAlphas()
    {
        byte expectedAlphas0 = "0011 0010".AsByte(); // d   c   
        byte expectedAlphas1 = "0001 0000".AsByte(); // b   a   
        byte expectedAlphas2 = "0111 0110".AsByte(); // h   g   
        byte expectedAlphas3 = "0101 0100".AsByte(); // f   e   
        byte expectedAlphas4 = "1011 1010".AsByte(); // l   k   
        byte expectedAlphas5 = "1001 1000".AsByte(); // j   i   
        byte expectedAlphas6 = "1111 1110".AsByte(); // p   o   
        byte expectedAlphas7 = "1101 1100".AsByte(); // n   m   

        var block = new BC2BlockData();
                                        // texel    value    byte
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

        Assert.That(alphas0, Is.EqualTo(expectedAlphas0));
        Assert.That(alphas1, Is.EqualTo(expectedAlphas1));
        Assert.That(alphas2, Is.EqualTo(expectedAlphas2));
        Assert.That(alphas3, Is.EqualTo(expectedAlphas3));
        Assert.That(alphas4, Is.EqualTo(expectedAlphas4));
        Assert.That(alphas5, Is.EqualTo(expectedAlphas5));
        Assert.That(alphas6, Is.EqualTo(expectedAlphas6));
        Assert.That(alphas7, Is.EqualTo(expectedAlphas7));
    }

    [Test]
    public void ConstructionFromBytesSetsColor0()
    {
        var color = new Color565Helper(40500);
        var bytes = new byte[BlockFormat.BC2ByteSize];
        bytes[8] = color.LowByte;
        bytes[9] = color.HighByte;

        var block = BC2BlockData.FromBytes(bytes);

        Assert.That(block.Color0.Value, Is.EqualTo(color.Color.Value));
    }

    [Test]
    public void ConstructionFromBytesSetsColor1()
    {
        var color = new Color565Helper(40500);
        var bytes = new byte[BlockFormat.BC2ByteSize];
        bytes[10] = color.LowByte;
        bytes[11] = color.HighByte;

        var block = BC2BlockData.FromBytes(bytes);

        Assert.That(block.Color1.Value, Is.EqualTo(color.Color.Value));
    }

    [Test]
    public void ConstructionFromBytesSetsColorIndexes()
    {
        byte indexes0 = "11 10 01 00".AsByte(); // d c b a 
        byte indexes1 = "00 11 10 01".AsByte(); // h g f e 
        byte indexes2 = "01 00 11 10".AsByte(); // l k j i 
        byte indexes3 = "10 01 00 11".AsByte(); // p o n m 

        var bytes = new byte[BlockFormat.BC2ByteSize];
        bytes[12] = indexes0;
        bytes[13] = indexes1;
        bytes[14] = indexes2;
        bytes[15] = indexes3;

        var block = BC2BlockData.FromBytes(bytes);

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
    public void ConstructionFromBytesSetsColorAlphas()
    {
        byte alphas0 = "0011 0010".AsByte(); // d   c   
        byte alphas1 = "0001 0000".AsByte(); // b   a   
        byte alphas2 = "0111 0110".AsByte(); // h   g   
        byte alphas3 = "0101 0100".AsByte(); // f   e   
        byte alphas4 = "1011 1010".AsByte(); // l   k   
        byte alphas5 = "1001 1000".AsByte(); // j   i   
        byte alphas6 = "1111 1110".AsByte(); // p   o   
        byte alphas7 = "1101 1100".AsByte(); // n   m   

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

        Assert.That(block.ColorAlphas[0], Is.EqualTo(0));  // a    0000
        Assert.That(block.ColorAlphas[1], Is.EqualTo(1));  // b    0001
        Assert.That(block.ColorAlphas[2], Is.EqualTo(2));  // c    0010
        Assert.That(block.ColorAlphas[3], Is.EqualTo(3));  // d    0011
        Assert.That(block.ColorAlphas[4], Is.EqualTo(4));  // e    0100
        Assert.That(block.ColorAlphas[5], Is.EqualTo(5));  // f    0101
        Assert.That(block.ColorAlphas[6], Is.EqualTo(6));  // g    0110
        Assert.That(block.ColorAlphas[7], Is.EqualTo(7));  // h    0111
        Assert.That(block.ColorAlphas[8], Is.EqualTo(8));  // i    1000
        Assert.That(block.ColorAlphas[9], Is.EqualTo(9));  // j    1001
        Assert.That(block.ColorAlphas[10], Is.EqualTo(10)); // k    1010
        Assert.That(block.ColorAlphas[11], Is.EqualTo(11)); // l    1011
        Assert.That(block.ColorAlphas[12], Is.EqualTo(12)); // m    1100
        Assert.That(block.ColorAlphas[13], Is.EqualTo(13)); // n    1101
        Assert.That(block.ColorAlphas[14], Is.EqualTo(14)); // o    1110
        Assert.That(block.ColorAlphas[15], Is.EqualTo(15)); // p    1111          
    }
}
