using System;
using BitmapCompressor.Formats;
using NUnit.Framework;

namespace BitmapCompressor.Tests.IntegrationTests
{
    [TestFixture(Category = "Integration")]
    public class BC1CompressionTests : BaseIntegrationTests
    {
        [Test]
        public void CompressBitmapWithoutAlpha()
        {
            var source   = LoadResourceBMP(Images.CityscapeBmp);
            var expected = LoadResourceDDS(Images.CityscapeDdsBc1);

            var actual = Compress(source, new BC1Format());
            
            AssertEqual(expected, actual);
        }

        [TestCase(Images.CityscapeAlphaBmp, Images.CityscapeAlphaDdsBc1,    TestName = "CompressBitmapWithAlpha#1")]
        [TestCase(Images.MarsAlphaBmp,      Images.MarsAlphaDdsBc1,         TestName = "CompressBitmapWithAlpha#2")]
        public void CompressBitmapWithAlpha(string sourceFile, string expectedFile)
        {
            var source   = LoadResourceBMP(sourceFile);
            var expected = LoadResourceDDS(expectedFile);

            var actual = Compress(source, new BC1Format());
                
            AssertEqual(expected, actual);
        }

        [Test]
        public void DecompressDDSWithoutAlpha()
        {
            var source   = LoadResourceDDS(Images.CityscapeDdsBc1);
            var expected = LoadResourceBMP(Images.CityscapeBmpBc1);

            var actual = Decompress(source);
            
            AssertEqual(expected, actual);
        }

        [TestCase(Images.CityscapeAlphaDdsBc1,  Images.CityscapeAlphaBmpBc1,    TestName = "DecompressDDSWithAlpha#1")]
        [TestCase(Images.MarsAlphaDdsBc1,       Images.MarsAlphaBmpBc1,         TestName = "DecompressDDSWithAlpha#2")]
        public void DecompressDDSWithAlpha(string sourceFile, string expectedFile)
        {
            var source   = LoadResourceDDS(sourceFile);
            var expected = LoadResourceBMP(expectedFile);

            var actual = Decompress(source);
                
            AssertEqual(expected, actual);
        }
    }
}
