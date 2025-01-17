using System;
using System.Drawing;
using BitmapCompressor.Extensions;
using NUnit.Framework;

namespace BitmapCompressor.Tests.UnitTests.Compressor.Extensions
{
    [TestFixture(Category = "Extensions")]
    public class PointExtensionsTests
    {
        [TestCase(2,3,    1, 5,    3,8,     TestName = "AddTwoPoints")]
        [TestCase(1,5,    0, 0,    1,5,     TestName = "AddTwoPointsWithZeros")]
        [TestCase(9,7,   -8,-6,    1,1,     TestName = "AddTwoPointsWithNegativeValues")]
        public void Add(int x1, int y1, int x2, int y2, int expectedX, int expectedY)
        {
            var result = new Point(x1, y1).Add(new Point(x2, y2));

            Assert.That(result.X, Is.EqualTo(expectedX));
            Assert.That(result.Y, Is.EqualTo(expectedY));
        }

        [TestCase(2,3,    1, 5,     1,-2,   TestName = "SubtractTwoPoints")]
        [TestCase(1,5,    0, 0,     1, 5,   TestName = "SubtractTwoPointsWithZeros")]
        [TestCase(9,7,   -8,-6,    17,13,   TestName = "SubtractTwoPointsWithNegativeValues")]
        public void Subtract(int x1, int y1, int x2, int y2, int expectedX, int expectedY)
        {
            var result = new Point(x1, y1).Subtract(new Point(x2, y2));

            Assert.That(result.X, Is.EqualTo(expectedX));
            Assert.That(result.Y, Is.EqualTo(expectedY));
        }
    }
}
