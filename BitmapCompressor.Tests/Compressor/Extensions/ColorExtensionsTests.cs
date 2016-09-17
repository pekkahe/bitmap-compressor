using System;
using System.Drawing;
using BitmapCompressor.Extensions;
using NUnit.Framework;

namespace BitmapCompressor.Tests.Compression.Extensions
{
    [TestFixture]
    public class ColorExtensionsTests
    {
        [TestCase(255,  false,  TestName = "ColorExtensions_HasAlphaWhenFullyOpaque_ReturnsFalse")]
        [TestCase(200,  true,   TestName = "ColorExtensions_HasAlphaWhenPartiallyTransparent_ReturnsTrue")]
        [TestCase(0,    true,   TestName = "ColorExtensions_HasAlphaWhenFullyTransparent_ReturnsTrue")]
        public void ColorExtensions_HasAlpha(int alpha, bool expected)
        {
            var color = Color.FromArgb(alpha, Color.Green);

            var result = color.HasAlpha();

            Assert.AreEqual(expected, result);
        }
    }
}
