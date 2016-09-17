using System;
using BitmapCompressor.Console.Utilities;
using NUnit.Framework;

namespace BitmapCompressor.Tests.Console
{
    [TestFixture]
    public class CommandLineArgumentsParserTests
    {
        [Test]
        public void CommandLineArgumentsParser_ParseStringsWithCompressOption_ReturnsArguments()
        {
            var parser = new CommandLineArgumentsParser();
            var args = new string[]
            {
                CommandLineArgumentsParser.CompressOption,
                "file.bmp",
                "file.dds"
            };

            var result = parser.Parse(args);

            Assert.AreEqual(true, result.Compress);
            Assert.AreEqual(false, result.Decompress);
            Assert.AreEqual(false, result.Overwrite);
            Assert.AreEqual("file.bmp", result.BMPFileName);
            Assert.AreEqual("file.dds", result.DDSFileName);
        }

        [Test]
        public void CommandLineArgumentsParser_ParseStringsWithDecompressOption_ReturnsArguments()
        {
            var parser = new CommandLineArgumentsParser();
            var args = new string[]
            {
                CommandLineArgumentsParser.DecompressOption,
                "file.bmp",
                "file.dds"
            };

            var result = parser.Parse(args);

            Assert.AreEqual(false, result.Compress);
            Assert.AreEqual(true, result.Decompress);
            Assert.AreEqual(false, result.Overwrite);
            Assert.AreEqual("file.bmp", result.BMPFileName);
            Assert.AreEqual("file.dds", result.DDSFileName);
        }

        [Test]
        public void CommandLineArgumentsParser_ParseStringsWithOverwriteOption_ReturnsArguments()
        {
            var parser = new CommandLineArgumentsParser();
            var args = new string[]
            {
                CommandLineArgumentsParser.CompressOption,
                CommandLineArgumentsParser.OverwriteOption,
                "file.bmp",
                "file.dds"
            };

            var result = parser.Parse(args);

            Assert.AreEqual(true, result.Compress);
            Assert.AreEqual(false, result.Decompress);
            Assert.AreEqual(true, result.Overwrite);
            Assert.AreEqual("file.bmp", result.BMPFileName);
            Assert.AreEqual("file.dds", result.DDSFileName);
        }

        [Test]
        public void CommandLineArgumentsParser_ParseStringsWithConflictingOptions_ThrowsException()
        {
            var parser = new CommandLineArgumentsParser();
            var args = new string[]
            {
                CommandLineArgumentsParser.CompressOption,
                CommandLineArgumentsParser.DecompressOption,
                "file.bmp",
                "file.dds"
            };

            Assert.Throws<ArgumentException>(() => parser.Parse(args));
        }

        [Test]
        public void CommandLineArgumentsParser_ParseStringsWithTooManyArguments_ThrowsException()
        {
            var parser = new CommandLineArgumentsParser();
            var args = new string[]
            {
                CommandLineArgumentsParser.CompressOption,
                CommandLineArgumentsParser.DecompressOption,
                CommandLineArgumentsParser.OverwriteOption,
                "file.bmp",
                "file.dds"
            };

            Assert.Throws<ArgumentException>(() => parser.Parse(args));
        }

        [Test]
        public void CommandLineArgumentsParser_ParseStringsWithInvalidBMPFileExtension_ThrowsException()
        {
            var parser = new CommandLineArgumentsParser();
            var args = new string[]
            {
                CommandLineArgumentsParser.CompressOption,
                "file.txt",
                "file.dds"
            };

            Assert.Throws<ArgumentException>(() => parser.Parse(args));
        }

        [Test]
        public void CommandLineArgumentsParser_ParseStringsWithInvalidDDSFileExtension_ThrowsException()
        {
            var parser = new CommandLineArgumentsParser();
            var args = new string[]
            {
                CommandLineArgumentsParser.CompressOption,
                "file.bmp",
                "file.txt"
            };

            Assert.Throws<ArgumentException>(() => parser.Parse(args));
        }

        [Test]
        public void CommandLineArgumentsParser_ParseStringsWithSingleArgument_ThrowsException()
        {
            var parser = new CommandLineArgumentsParser();
            var args = new string[]
            {
                CommandLineArgumentsParser.CompressOption,
            };

            Assert.Throws<ArgumentException>(() => parser.Parse(args));
        }
    }
}
