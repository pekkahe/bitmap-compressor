using System.Drawing;
using BitmapCompressor.DataTypes;
using BitmapCompressor.Utilities;
using NUnit.Framework;

namespace BitmapCompressor.Tests.UnitTests.Compressor.DataTypes
{
    [TestFixture(Category = "DataTypes")]
    public class ColorSpaceTests
    {
        [Test]
        public void CalculatesEndpointColors()
        {
            var low =  Color.FromArgb(50,  50,  50);
            var col =  Color.FromArgb(100, 100, 100);
            var high = Color.FromArgb(150, 150, 150);

            var colorSpace = new ColorSpace(new[] { col, low, high });

            Assert.AreEqual(low,  colorSpace.LowColor);
            Assert.AreEqual(high, colorSpace.HighColor);
        }

        [Test]
        public void CalculatesMinAndMaxColors()
        {
            var low =  Color.FromArgb(50,  50,  50);
            var col =  Color.FromArgb(100, 100, 100);
            var high = Color.FromArgb(150, 150, 150);

            var colorSpace = new ColorSpace(new[] { col, low, high });

            Assert.AreEqual(ColorUtility.To16Bit(low),  colorSpace.MinColor);
            Assert.AreEqual(ColorUtility.To16Bit(high), colorSpace.MaxColor);
        }

        [Test]
        public void Recognizes16BitColorOrderWhen32BitOrderIsInverted()
        {
            var min =     Color.FromArgb(50, 255, 255);
            var max =     Color.FromArgb(55, 0,   0);
            var minAs16ShouldBeMax = ColorUtility.To16Bit(min);
            var maxAs16ShouldBeMin = ColorUtility.To16Bit(max);

            var colorSpace = new ColorSpace(new[] { min, max });

            Assert.IsTrue(min.ToArgb() < max.ToArgb());
            Assert.IsTrue(minAs16ShouldBeMax.Value > maxAs16ShouldBeMin.Value);
            Assert.AreEqual(maxAs16ShouldBeMin, colorSpace.MinColor);
            Assert.AreEqual(minAs16ShouldBeMax, colorSpace.MaxColor);
        }

        [TestCase(255, false, TestName = "ReportsAlphaForColorsWithoutAlpha")]
        [TestCase(150, true,  TestName = "ReportsAlphaForColorsWithAlpha")]
        public void ReportsAlpha(int alpha, bool expected)
        {
            var color = Color.FromArgb(alpha, 150, 150, 150);

            var colorSpace = new ColorSpace(new[] { color });

            Assert.AreEqual(expected, colorSpace.HasAlpha);
        }
    }
}
