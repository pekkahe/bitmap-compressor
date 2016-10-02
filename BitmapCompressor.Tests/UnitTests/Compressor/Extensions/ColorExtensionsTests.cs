using System;
using System.Drawing;
using BitmapCompressor.Extensions;
using NUnit.Framework;

namespace BitmapCompressor.Tests.UnitTests.Compression.Extensions
{
    [TestFixture(Category = "Extensions")]
    public class ColorExtensionsTests
    {
        [TestCase(255,  false,  TestName = "HasAlphaIsFalseWhenFullyOpaque")]
        [TestCase(200,  true,   TestName = "HasAlphaIsTrueWhenPartiallyTransparent")]
        [TestCase(0,    true,   TestName = "HasAlphaIsTrueWhenFullyTransparent")]
        public void HasAlpha(int alpha, bool expected)
        {
            var color = Color.FromArgb(alpha, Color.Green);

            var result = color.HasAlpha();

            Assert.AreEqual(expected, result);
        }
    }
}
