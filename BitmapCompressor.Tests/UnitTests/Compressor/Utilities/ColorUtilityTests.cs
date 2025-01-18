using System;
using System.Drawing;
using BitmapCompressor.Lib.Utilities;
using BitmapCompressor.Lib.DataTypes;
using NUnit.Framework;

namespace BitmapCompressor.Tests.UnitTests.Compressor.Utilities;

[TestFixture(Category = "Utilities")]
public class ColorUtilityTests
{
    [Test]
    public void Blend32BitColors()
    {
        var a = Color.FromArgb(250, 100, 150);
        var b = Color.FromArgb(150, 100, 200);
        var expected = Color.FromArgb(200, 100, 175);

        var result = ColorUtility.Blend(a, b);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Blend16BitColors()
    {
        var a = Color565.FromRgb(30, 60, 20);
        var b = Color565.FromRgb(20, 60, 25);
        var expected = Color565.FromRgb(25, 60, 23);

        var result = ColorUtility.Blend(a, b);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Lerp32BitColors()
    {
        var a = Color.FromArgb( 50, 100, 200);
        var b = Color.FromArgb(250, 100, 150);
        var expected = Color.FromArgb(183, 100, 167);

        var result = ColorUtility.LerpTwoThirds(a, b);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Lerp16BitColors()
    {
        var a = Color565.FromRgb(10, 60, 20);
        var b = Color565.FromRgb(30, 60, 15);
        var expected = Color565.FromRgb(23, 60, 17);

        var result = ColorUtility.LerpTwoThirds(a, b);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Convert16BitColorTo32Bit()
    {
        var color1 = Color565.FromRgb(  9,  51,  28);
        var expected1 = Color.FromArgb(74, 207, 231);

        var color2 = Color565.FromRgb( 0,  63,  31);
        var expected2 = Color.FromArgb(0, 255, 255);

        var result1 = ColorUtility.To32Bit(color1);
        var result2 = ColorUtility.To32Bit(color2);

        Assert.That(result1, Is.EqualTo(expected1));
        Assert.That(result2, Is.EqualTo(expected2));
    }

    [Test]
    public void Convert32BitColorTo16Bit()
    {
        var color1 = Color.FromArgb(     74, 207, 231);
        var expected1 = Color565.FromRgb( 9,  51,  28);

        var color2 = Color.FromArgb(     0, 255, 255);
        var expected2 = Color565.FromRgb(0,  63,  31);

        var result1 = ColorUtility.To16Bit(color1);
        var result2 = ColorUtility.To16Bit(color2);

        Assert.That(result1, Is.EqualTo(expected1));
        Assert.That(result2, Is.EqualTo(expected2));
    }

    [Test]
    public void CalculateDistanceBetween32BitColors()
    {
        var a = Color.FromArgb(100, 130, 60);
        var b = Color.FromArgb(150, 255, 200);
        var expected = Math.Sqrt((100-150) * (100-150) +
                                 (130-255) * (130-255) + 
                                  (60-200) *  (60-200));

        var result = ColorUtility.Distance(a, b);

        Assert.That(result, Is.EqualTo(expected).Within(0.0001d));
    }

    [Test]
    public void CalculateDistanceBetween16BitColors()
    {
        var a = Color565.FromRgb(10, 16, 9);
        var b = Color565.FromRgb(20, 63, 31);
        var expected = Math.Sqrt((10-20) * (10-20) + 
                                 (16-63) * (16-63) + 
                                  (9-31) *  (9-31));

        var result = ColorUtility.Distance(a, b);

        Assert.That(result, Is.EqualTo(expected).Within(0.0001d));
    }

    [Test]
    public void CalculateClosestColor()
    {
        var target =   Color565.FromRgb(15, 31, 15);
        var expected = Color565.FromRgb(13, 29, 14);
        var colors = new []
        {
            Color565.FromRgb( 0,  0,  0),
            Color565.FromRgb(31, 63, 31),
            Color565.FromRgb( 9, 56, 45),
            expected,
            Color565.FromRgb(24, 45, 14),
        };
        
        var closest = ColorUtility.GetClosestColor(colors, target);

        Assert.That(closest, Is.EqualTo(expected));
    }

    [Test]
    public void CalculateClosestAlpha()
    {
        byte target = 80;
        byte expected = 70;
        var alphas = new byte[]
        {
            255,
            0,
            140,
            90,
            expected,
            90,
            60,
            255,
            0
        };

        var closest = ColorUtility.GetClosestAlpha(alphas, target);

        Assert.That(closest, Is.EqualTo(expected));
    }

    [Test]
    public void DifferentiationIncreasesMaxColorByOneWhenMaxCanBeIncreased()
    {
        var minOriginal = Color565.FromRgb(15, 43, 9);
        var maxOriginal = Color565.FromRgb(15, 43, 9);
        var min = minOriginal;
        var max = maxOriginal;

        ColorUtility.TryDifferentiateByOne(ref min, ref max);

        Assert.That(min, Is.EqualTo(minOriginal));
        Assert.That(max, Is.EqualTo(Color565.FromRgb(15, 44, 9)));
    }

    [Test]
    public void DifferentiationDecreasesMinColorByOneWhenMaxCannotBeIncreased()
    {
        var minOriginal = Color565.FromRgb(31, 63, 31);
        var maxOriginal = Color565.FromRgb(31, 63, 31);
        var min = minOriginal;
        var max = maxOriginal;

        ColorUtility.TryDifferentiateByOne(ref min, ref max);

        Assert.That(min, Is.EqualTo(Color565.FromRgb(31, 62, 31)));
        Assert.That(max, Is.EqualTo(maxOriginal));
    }

    [Test]
    public void DifferentiationDoesNothingWhenColorsAreDifferent()
    {
        var minOriginal = Color565.FromRgb(15, 31, 21);
        var maxOriginal = Color565.FromRgb(13, 24, 14);
        var min = minOriginal;
        var max = maxOriginal;

        ColorUtility.TryDifferentiateByOne(ref min, ref max);

        Assert.That(min, Is.EqualTo(minOriginal));
        Assert.That(max, Is.EqualTo(maxOriginal));
    }
}
