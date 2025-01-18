using System;
using System.Drawing;
using BitmapCompressor.Lib.Formats;
using BitmapCompressor.Tests.Helpers;
using NUnit.Framework;

namespace BitmapCompressor.Tests.UnitTests.Compressor.Formats;

[TestFixture(Category = "Formats")]
public class BC1BlockDataTests
{
    [Test]
    public void ConversionToBytesReturnsArrayOfCorrectSize()
    {
        var block = new BC1BlockData();

        var buffer = block.ToBytes();

        Assert.That(buffer.Length, Is.EqualTo(BlockFormat.BC1ByteSize));
        Assert.That(buffer, Is.EquivalentTo(new byte[BlockFormat.BC1ByteSize]));
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

        Assert.That(c0Low, Is.EqualTo(color.LowByte));
        Assert.That(c0Hi, Is.EqualTo(color.HighByte));
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

        Assert.That(c1Low, Is.EqualTo(color.LowByte));
        Assert.That(c1Hi, Is.EqualTo(color.HighByte));
    }

    [Test]
    public void ConversionToBytesSetsColorIndexes()
    {
        byte expectedIndexes0 = "11 10 01 00".AsByte(); // d c b a
        byte expectedIndexes1 = "00 11 10 01".AsByte(); // h g f e
        byte expectedIndexes2 = "01 00 11 10".AsByte(); // l k j i
        byte expectedIndexes3 = "10 01 00 11".AsByte(); // p o n m  

        var block = new BC1BlockData();
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

        byte indexes0 = buffer[4];
        byte indexes1 = buffer[5];
        byte indexes2 = buffer[6];
        byte indexes3 = buffer[7];

        Assert.That(indexes0, Is.EqualTo(expectedIndexes0));
        Assert.That(indexes1, Is.EqualTo(expectedIndexes1));
        Assert.That(indexes2, Is.EqualTo(expectedIndexes2));
        Assert.That(indexes3, Is.EqualTo(expectedIndexes3));
    }

    [Test]
    public void ConstructionFromBytesSetsColor0()
    {
        var color = new Color565Helper(40500);
        var bytes = new byte[BlockFormat.BC1ByteSize];
        bytes[0] = color.LowByte;
        bytes[1] = color.HighByte;

        var block = BC1BlockData.FromBytes(bytes);

        Assert.That(block.Color0.Value, Is.EqualTo(color.Color.Value));
    }

    [Test]
    public void ConstructionFromBytesSetsColor1()
    {
        var color = new Color565Helper(40500);
        var bytes = new byte[BlockFormat.BC1ByteSize];
        bytes[2] = color.LowByte;
        bytes[3] = color.HighByte;

        var block = BC1BlockData.FromBytes(bytes);

        Assert.That(block.Color1.Value, Is.EqualTo(color.Color.Value));
    }

    [Test]
    public void ConstructionFromBytesSetsColorIndexes()
    {
        byte indexes0 = "11 10 01 00".AsByte(); // d c b a 
        byte indexes1 = "00 11 10 01".AsByte(); // h g f e 
        byte indexes2 = "01 00 11 10".AsByte(); // l k j i 
        byte indexes3 = "10 01 00 11".AsByte(); // p o n m 

        var bytes = new byte[BlockFormat.BC1ByteSize];
        bytes[4] = indexes0;
        bytes[5] = indexes1;
        bytes[6] = indexes2;
        bytes[7] = indexes3;

        var block = BC1BlockData.FromBytes(bytes);

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
}
