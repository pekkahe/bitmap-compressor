using System;
using BitmapCompressor.Lib.DataTypes;
using NUnit.Framework;

namespace BitmapCompressor.Tests.UnitTests.Compressor.DataTypes;

[TestFixture(Category = "DataTypes")]
public class Color565Tests
{
    private const int MaxValue16Bit = 65535;

    [Test]
    public void FormatIsR5G6B5()
    {
        var color = Color565.FromRgb(10, 51, 23);

        // 10 = _0 1010 -> R5
        // 51 = 11 0011 -> G6
        // 23 = _1 0111 -> B5
        //   R5      G6     B5      
        // 0101 0|110 011|1 0111   16-bit layout
        const int expected = 22135;

        var value = color.Value;

        Assert.That(value, Is.EqualTo(expected));
    }

    [Test]
    public void ConstructFromBytes()
    {
        var color = Color565.FromRgb((byte) 10, (byte) 51, (byte) 23);

        Assert.That(color.R, Is.EqualTo(10));
        Assert.That(color.G, Is.EqualTo(51));
        Assert.That(color.B, Is.EqualTo(23));
    }

    [Test]
    public void ConstructFrom32BitIntegers()
    {
        var color = Color565.FromRgb(10, 51, 23);

        Assert.That(color.R, Is.EqualTo(10));
        Assert.That(color.G, Is.EqualTo(51));
        Assert.That(color.B, Is.EqualTo(23));
    }

    [Test]
    public void ConstructFrom16BitUnsignedInteger()
    {
        var color = Color565.FromValue(22135);

        Assert.That(color.R, Is.EqualTo(10));
        Assert.That(color.G, Is.EqualTo(51));
        Assert.That(color.B, Is.EqualTo(23));
        Assert.That(color.Value, Is.EqualTo(22135));
    }

    [Test]
    public void ConstructionClampsComponentsForBytes()
    {
        var expected = Color565.FromRgb((byte) 31, (byte) 63, (byte) 31);

        var color1 = Color565.FromRgb((byte) 255, (byte) 255, (byte) 255);
        var color2 = Color565.FromRgb((byte) 150, (byte) 230, (byte) 130);

        Assert.That(color1, Is.EqualTo(expected));
        Assert.That(color2, Is.EqualTo(expected));
        Assert.That(color1.Value, Is.EqualTo(MaxValue16Bit));
        Assert.That(color2.Value, Is.EqualTo(MaxValue16Bit));
    }

    [Test]
    public void ConstructionClampsComponentsFor32BitIntegers()
    {
        var expected = Color565.FromRgb(31, 63, 31);

        var color1 = Color565.FromRgb(3455, 23145, 55132);
        var color2 = Color565.FromRgb(255, 255, 255);
        var color3 = Color565.FromRgb(150, 230, 130);

        Assert.That(color1, Is.EqualTo(expected));
        Assert.That(color2, Is.EqualTo(expected));
        Assert.That(color3, Is.EqualTo(expected));
        Assert.That(color1.Value, Is.EqualTo(MaxValue16Bit));
        Assert.That(color2.Value, Is.EqualTo(MaxValue16Bit));
        Assert.That(color3.Value, Is.EqualTo(MaxValue16Bit));
    }
}
