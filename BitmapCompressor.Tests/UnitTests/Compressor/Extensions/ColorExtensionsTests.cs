using System;
using System.Drawing;
using BitmapCompressor.Extensions;
using NUnit.Framework;

namespace BitmapCompressor.Tests.UnitTests.Compressor.Extensions
{
    [TestFixture(Category = "Extensions")]
    public class ColorExtensionsTests
    {
        [TestCase(255,  false,  TestName = "ReportsAlphaWhenFullyOpaque")]
        [TestCase(200,  true,   TestName = "ReportsAlphaWhenPartiallyTransparent")]
        [TestCase(0,    true,   TestName = "ReportsAlphaWhenFullyTransparent")]
        public void HasAlpha(int alpha, bool expected)
        {
            var color = Color.FromArgb(alpha, Color.Green);

            var result = color.HasAlpha();

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
