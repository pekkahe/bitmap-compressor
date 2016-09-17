using System;
using BitmapCompressor.Extensions;
using NUnit.Framework;

namespace BitmapCompressor.Tests.Compression.Extensions
{
    [TestFixture]
    public class ArrayExtensionsTests
    {
        [TestCase(4, 1, new[] { 5 },             TestName = "ArrayExtensions_SubArrayForOneElement_ReturnsArrayOfOneElement")]
        [TestCase(2, 5, new[] { 3, 4, 5, 6, 7 }, TestName = "ArrayExtensions_SubArrayForMultipleElements_ReturnsArrayOfMultipleElements")]
        public void ArrayExtensions_SubArray(int sourceIndex, int length, int[] expected)
        {
            var array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var result = array.SubArray(sourceIndex, length);

            CollectionAssert.AreEqual(expected, result);
        }

        [TestCase(new[] { 1, 2, 3 }, 0, new[] { 1, 2, 3, 0, 0, 0 },       TestName = "ArrayExtensions_CopyFromArrayToBeginning_CopiesElements")]
        [TestCase(new[] { 1, 2, 3 }, 5, new[] { 0, 0, 0, 0, 0, 1, 2, 3 }, TestName = "ArrayExtensions_CopyFromArrayToEnd_CopiesElements")]
        [TestCase(new[] { 1, 2, 3 }, 2, new[] { 0, 0, 1, 2, 3, 0, 0 },    TestName = "ArrayExtensions_CopyFromArrayToMiddle_CopiesElements")]
        public void ArrayExtensions_CopyFrom(int[] source, int destinationIndex, int[] expected)
        {
            var array = new int[expected.Length];

            array.CopyFrom(source, destinationIndex);

            CollectionAssert.AreEqual(array, expected);
        }
    }
}
