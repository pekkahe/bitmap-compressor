using System.Drawing;
using BitmapCompressor.Utilities;
using NUnit.Framework;

namespace BitmapCompressor.Tests.UnitTests.Compression.Utilities
{
    [TestFixture(Category = "Utilities")]
    public class PointUtilityTests
    {
        [TestCase(1, 0, 2,  1,  TestName = "ConvertPointToRowMajorIndex#1")]
        [TestCase(1, 1, 3,  4,  TestName = "ConvertPointToRowMajorIndex#2")]
        [TestCase(0, 0, 4,  0,  TestName = "ConvertPointToRowMajorIndex#3")]
        [TestCase(0, 3, 4, 12,  TestName = "ConvertPointToRowMajorIndex#4")]
        [TestCase(1, 2, 4,  9,  TestName = "ConvertPointToRowMajorIndex#5")]
        [TestCase(2, 2, 4, 10,  TestName = "ConvertPointToRowMajorIndex#6")]
        [TestCase(3, 3, 4, 15,  TestName = "ConvertPointToRowMajorIndex#7")]
        public void ToRowMajor(int x, int y, int columns, int result)
        {
            var point = new Point(x, y);

            var index = PointUtility.ToRowMajor(point, columns);

            Assert.AreEqual(result, index);
        }

        [TestCase(0, 2, 0, 0, TestName = "ConvertRowMajorIndexToPoint#1")]
        [TestCase(0, 4, 0, 0, TestName = "ConvertRowMajorIndexToPoint#2")]
        [TestCase(3, 4, 3, 0, TestName = "ConvertRowMajorIndexToPoint#3")]
        [TestCase(3, 2, 1, 1, TestName = "ConvertRowMajorIndexToPoint#4")]
        [TestCase(4, 4, 0, 1, TestName = "ConvertRowMajorIndexToPoint#5")]
        [TestCase(7, 4, 3, 1, TestName = "ConvertRowMajorIndexToPoint#6")]
        [TestCase(7, 1, 0, 7, TestName = "ConvertRowMajorIndexToPoint#7")]
        public void FromRowMajor(int index, int columns, int resultX, int resultY)
        {
            var point = PointUtility.FromRowMajor(index, columns);

            Assert.AreEqual(resultX, point.X);
            Assert.AreEqual(resultY, point.Y);
        }
    }
}
