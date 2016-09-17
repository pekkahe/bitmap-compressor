using System;
using System.Drawing;
using BitmapCompressor.Extensions;
using NUnit.Framework;

namespace BitmapCompressor.Tests.Compression.Extensions
{
    [TestFixture]
    public class PointExtensionsTests
    {
        [TestCase(0,0,    1, 5,    1,5,     TestName = "PointExtensions_AddWithZeroValues_ReturnsSum")]
        [TestCase(2,3,    1, 5,    3,8,     TestName = "PointExtensions_AddWithMixedValues_ReturnsSum")]
        [TestCase(9,7,   -8,-6,    1,1,     TestName = "PointExtensions_AddWithNegativeValues_ReturnsSum")]
        public void PointExtensions_Add(int x1, int y1, int x2, int y2, int expectedX, int expectedY)
        {
            var result = new Point(x1, y1).Add(new Point(x2, y2));

            Assert.AreEqual(expectedX, result.X);
            Assert.AreEqual(expectedY, result.Y);
        }

        [TestCase(0,0,    1, 5,    -1,-5,   TestName = "PointExtensions_SubtractWithZeroValues_ReturnsDeduction")]
        [TestCase(2,3,    1, 5,     1,-2,   TestName = "PointExtensions_SubtractWithMixedValues_ReturnsDeduction")]
        [TestCase(9,7,   -8,-6,    17,13,   TestName = "PointExtensions_SubtractWithNegativeValues_ReturnsDeduction")]
        public void PointExtensions_Subtract(int x1, int y1, int x2, int y2, int expectedX, int expectedY)
        {
            var result = new Point(x1, y1).Subtract(new Point(x2, y2));

            Assert.AreEqual(expectedX, result.X);
            Assert.AreEqual(expectedY, result.Y);
        }
    }
}
