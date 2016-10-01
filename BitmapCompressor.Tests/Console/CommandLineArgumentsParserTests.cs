using System;
using BitmapCompressor.Console.Utilities;
using NUnit.Framework;

namespace BitmapCompressor.Tests.Console
{
    [TestFixture(Category = "Console")]
    public class CommandLineArgumentsParserTests
    {
        [Test]
        public void ParseStringsWithCompressOption()
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
        public void ParseStringsWithDecompressOption()
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
        public void ParseStringsWithOverwriteOption()
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
        public void ParsingThrowsExceptionWhenOnlyOneArgument()
        {
            var parser = new CommandLineArgumentsParser();
            var args = new string[]
            {
                CommandLineArgumentsParser.CompressOption,
            };

            Assert.Throws<ArgumentException>(() => parser.Parse(args));
        }

        [Test]
        public void ParsingThrowsExceptionWhenTooManyArguments()
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
        public void ParsingThrowsExceptionWhenOptionsConflict()
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
        public void ParsingThrowsExceptionWhenBMPFileHasInvalidExtension()
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
        public void ParsingThrowsExceptionWhenDDSFileHasInvalidExtension()
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
    }
}
