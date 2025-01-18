using System;
using BitmapCompressor.Lib.Formats;
using NUnit.Framework;

namespace BitmapCompressor.Tests.IntegrationTests;

[TestFixture(Category = "Integration")]
public class BC2CompressionTests : BaseIntegrationTests
{
    [Test]
    public void CompressBitmapWithoutAlpha()
    {
        var source   = LoadResourceBMP(Images.CityscapeBmp);
        var expected = LoadResourceDDS(Images.CityscapeDdsBc2);

        var actual = Compress(source, new BC2Format());
        
        AssertEqual(expected, actual);
    }

    [TestCase(Images.CityscapeAlphaBmp, Images.CityscapeAlphaDdsBc2, TestName = "CompressBitmapWithAlpha#1")]
    [TestCase(Images.MarsAlphaBmp,      Images.MarsAlphaDdsBc2,      TestName = "CompressBitmapWithAlpha#2")]
    public void CompressBitmapWithAlpha(string sourceFile, string expectedFile)
    {
        var source   = LoadResourceBMP(sourceFile);
        var expected = LoadResourceDDS(expectedFile);

        var actual = Compress(source, new BC2Format());

        AssertEqual(expected, actual);
    }

    [Test]
    public void DecompressDDSWithoutAlpha()
    {
        var source   = LoadResourceDDS(Images.CityscapeDdsBc2);
        var expected = LoadResourceBMP(Images.CityscapeBmpBc2);

        var actual = Decompress(source);

        AssertEqual(expected, actual);
    }

    [TestCase(Images.CityscapeAlphaDdsBc2, Images.CityscapeAlphaBmpBc2, TestName = "DecompressDDSWithAlpha#1")]
    [TestCase(Images.MarsAlphaDdsBc2,      Images.MarsAlphaBmpBc2,      TestName = "DecompressDDSWithAlpha#2")]
    public void DecompressDDSWithAlpha(string sourceFile, string expectedFile)
    {
        var source   = LoadResourceDDS(sourceFile);
        var expected = LoadResourceBMP(expectedFile);

        var actual = Decompress(source);

        AssertEqual(expected, actual);
    }
}
