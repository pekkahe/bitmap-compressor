using System.Drawing;
using BitmapCompressor.Lib.DataTypes;
using BitmapCompressor.Lib.Utilities;
using NUnit.Framework;

namespace BitmapCompressor.Tests.UnitTests.Compressor.DataTypes;

[TestFixture(Category = "DataTypes")]
public class ColorSpaceTests
{
    [Test]
    public void CalculatesMinAndMaxColors()
    {
        var min     = Color.FromArgb(50, 50, 50);
        var colorA  = Color.FromArgb(70, 60, 90);
        var colorB  = Color.FromArgb(100, 100, 100);
        var colorC  = Color.FromArgb(130, 110, 130);
        var max     = Color.FromArgb(150, 150, 150);

        var colorSpace = new ColorSpace(new[] 
        {
            colorA,
            min,
            colorB,
            colorC,
            max
        });

        Assert.That(colorSpace.MinColor, Is.EqualTo(ColorUtility.To16Bit(min)));
        Assert.That(colorSpace.MaxColor, Is.EqualTo(ColorUtility.To16Bit(max)));
    }

    [Test]
    public void CalculatesMinAndMaxAlpha()
    {
        var min     = Color.FromArgb(30, 50, 150, 250);
        var colorA  = Color.FromArgb(170, 170, 60, 90);
        var colorB  = Color.FromArgb(90, 140, 100, 190);
        var colorC  = Color.FromArgb(45, 130, 110, 130);
        var max     = Color.FromArgb(210, 50, 150, 150);

        var colorSpace = new ColorSpace(new[]
        {
            colorA,
            min,
            colorB,
            colorC,
            max
        });

        Assert.That(colorSpace.MinAlpha, Is.EqualTo(min.A));
        Assert.That(colorSpace.MaxAlpha, Is.EqualTo(max.A));
    }

    [Test]
    public void Recognizes16BitColorOrderWhen32BitOrderIsInverted()
    {
        var min = Color.FromArgb(50, 255, 255);
        var max = Color.FromArgb(55, 0, 0);
        var minAs16ShouldBeMax = ColorUtility.To16Bit(min);
        var maxAs16ShouldBeMin = ColorUtility.To16Bit(max);

        var colorSpace = new ColorSpace(new[] { min, max });

        Assert.That(min.ToArgb() < max.ToArgb());
        Assert.That(minAs16ShouldBeMax.Value > maxAs16ShouldBeMin.Value);
        Assert.That(colorSpace.MinColor, Is.EqualTo(maxAs16ShouldBeMin));
        Assert.That(colorSpace.MaxColor, Is.EqualTo(minAs16ShouldBeMax));
    }

    [TestCase(255, false, TestName = "ReportsAlphaForColorsWithoutAlpha")]
    [TestCase(150, true,  TestName = "ReportsAlphaForColorsWithAlpha")]
    public void ReportsAlpha(int alpha, bool expected)
    {
        var color = Color.FromArgb(alpha, 150, 150, 150);

        var colorSpace = new ColorSpace([color]);

        Assert.That(colorSpace.HasAlpha, Is.EqualTo(expected));
    }
}
