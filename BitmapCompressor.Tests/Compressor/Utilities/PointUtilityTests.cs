using System.Drawing;
using BitmapCompressor.Utilities;
using NUnit.Framework;

namespace BitmapCompressor.Tests.Compression.Utilities
{
    [TestFixture]
    public class PointUtilityTests
    {
        [TestCase(1, 0, 2,  1,  TestName = "PointUtility_ConvertPointToRowMajorIndexCase1_ReturnsIndex")]
        [TestCase(1, 1, 3,  4,  TestName = "PointUtility_ConvertPointToRowMajorIndexCase2_ReturnsIndex")]
        [TestCase(0, 0, 4,  0,  TestName = "PointUtility_ConvertPointToRowMajorIndexCase3_ReturnsIndex")]
        [TestCase(0, 3, 4, 12,  TestName = "PointUtility_ConvertPointToRowMajorIndexCase4_ReturnsIndex")]
        [TestCase(1, 2, 4,  9,  TestName = "PointUtility_ConvertPointToRowMajorIndexCase5_ReturnsIndex")]
        [TestCase(2, 2, 4, 10,  TestName = "PointUtility_ConvertPointToRowMajorIndexCase6_ReturnsIndex")]
        [TestCase(3, 3, 4, 15,  TestName = "PointUtility_ConvertPointToRowMajorIndexCase7_ReturnsIndex")]
        public void PointUtility_ToRowMajor(int x, int y, int columns, int result)
        {
            var point = new Point(x, y);

            var index = PointUtility.ToRowMajor(point, columns);

            Assert.AreEqual(result, index);
        }

        [TestCase(0, 2, 0, 0, TestName = "PointUtility_ConvertRowMajorIndexToPointCase1_ReturnsPoint")]
        [TestCase(0, 4, 0, 0, TestName = "PointUtility_ConvertRowMajorIndexToPointCase2_ReturnsPoint")]
        [TestCase(3, 4, 3, 0, TestName = "PointUtility_ConvertRowMajorIndexToPointCase3_ReturnsPoint")]
        [TestCase(3, 2, 1, 1, TestName = "PointUtility_ConvertRowMajorIndexToPointCase4_ReturnsPoint")]
        [TestCase(4, 4, 0, 1, TestName = "PointUtility_ConvertRowMajorIndexToPointCase5_ReturnsPoint")]
        [TestCase(7, 4, 3, 1, TestName = "PointUtility_ConvertRowMajorIndexToPointCase6_ReturnsPoint")]
        [TestCase(7, 1, 0, 7, TestName = "PointUtility_ConvertRowMajorIndexToPointCase7_ReturnsPoint")]
        public void PointUtility_FromRowMajor(int index, int columns, int resultX, int resultY)
        {
            var point = PointUtility.FromRowMajor(index, columns);

            Assert.AreEqual(resultX, point.X);
            Assert.AreEqual(resultY, point.Y);
        }
    }
}
