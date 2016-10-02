using System;
using BitmapCompressor.Extensions;
using NUnit.Framework;

namespace BitmapCompressor.Tests.UnitTests.Compression.Extensions
{
    [TestFixture(Category = "Extensions")]
    public class StringExtensionsTests
    {
        [TestCase("Hello {0}",   new object[] { "world" },          "Hello world",    
                  TestName = "ParametersWithOneArgument")]
        [TestCase("{0} is {1}",  new object[] { "Testing", "fun" }, "Testing is fun", 
                  TestName = "ParametersWithMultipleArguments")]
        [TestCase("{0}+{1}={2}", new object[] { 5, 3, 8 },          "5+3=8",          
                  TestName = "ParametersWithIntegerArguments")]
        public void StringExtensions_Parameters(string source, object[] args, string expected)
        {
            var result = source.Parameters(args);

            Assert.AreEqual(expected, result);
        }
    }
}
