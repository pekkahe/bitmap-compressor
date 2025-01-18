using System;
using BitmapCompressor.Lib.Extensions;
using NUnit.Framework;

namespace BitmapCompressor.Tests.UnitTests.Compressor.Extensions;

[TestFixture(Category = "Extensions")]
public class ArrayExtensionsTests
{
    [TestCase(4, 1, new[] { 5 },             TestName = "SubArrayOfOneElement")]
    [TestCase(2, 5, new[] { 3, 4, 5, 6, 7 }, TestName = "SubArrayOfMultipleElements")]
    public void SubArray(int sourceIndex, int length, int[] expected)
    {
        var array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        var result = array.SubArray(sourceIndex, length);

        Assert.That(result, Is.EquivalentTo(expected));
    }

    [TestCase(new[] { 1, 2, 3 }, 0, new[] { 1, 2, 3, 0, 0, 0 },       TestName = "CopyFromSourceToStart")]
    [TestCase(new[] { 1, 2, 3 }, 5, new[] { 0, 0, 0, 0, 0, 1, 2, 3 }, TestName = "CopyFromSourceToEnd")]
    [TestCase(new[] { 1, 2, 3 }, 2, new[] { 0, 0, 1, 2, 3, 0, 0 },    TestName = "CopyFromSourceToMiddle")]
    public void CopyFrom(int[] source, int destinationIndex, int[] expected)
    {
        var array = new int[expected.Length];

        array.CopyFrom(source, destinationIndex);

        Assert.That(array, Is.EquivalentTo(expected));
    }
}
