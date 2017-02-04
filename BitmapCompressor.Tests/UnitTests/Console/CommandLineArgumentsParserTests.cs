using System;
using BitmapCompressor.Console.CommandLine;
using NUnit.Framework;

namespace BitmapCompressor.Tests.UnitTests.Console
{
    [TestFixture(Category = "Console")]
    public class CommandLineArgumentsParserTests
    {
        [TestCase(CommandLineArguments.Keys.BC1Format, CommandLineAction.CompressBC1, 
                  TestName = "ParseCompressOptionForBC1")]
        [TestCase(CommandLineArguments.Keys.BC2Format, CommandLineAction.CompressBC2,
                  TestName = "ParseCompressOptionForBC2")]
        [TestCase(CommandLineArguments.Keys.BC3Format, CommandLineAction.CompressBC3,
                  TestName = "ParseCompressOptionForBC3")]
        public void ParsingCompressOption(char key, CommandLineAction action)
        {
            var parser = new CommandLineArgumentsParser();
            var args = new string[]
            {
                $"{CommandLineArguments.Keys.Compress}{key}",
                "file.bmp",
                "file.dds"
            };

            var result = parser.Parse(args);

            Assert.AreEqual(action, result.Action);
            Assert.AreEqual("file.bmp", result.BMPFileName);
            Assert.AreEqual("file.dds", result.DDSFileName);
        }

        [Test]
        public void ParseDecompressOption()
        {
            var parser = new CommandLineArgumentsParser();
            var args = new string[]
            {
                CommandLineArguments.Keys.Decompress,
                "file.bmp",
                "file.dds"
            };

            var result = parser.Parse(args);

            Assert.AreEqual(CommandLineAction.Decompress, result.Action);
            Assert.AreEqual(false, result.Overwrite);
            Assert.AreEqual("file.bmp", result.BMPFileName);
            Assert.AreEqual("file.dds", result.DDSFileName);
        }

        [Test]
        public void ParseOverwriteOption()
        {
            var parser = new CommandLineArgumentsParser();
            var args = new string[]
            {
                $"{CommandLineArguments.Keys.Compress}{CommandLineArguments.Keys.BC1Format}",
                CommandLineArguments.Keys.Overwrite,
                "file.bmp",
                "file.dds"
            };

            var result = parser.Parse(args);

            Assert.AreEqual(CommandLineAction.CompressBC1, result.Action);
            Assert.AreEqual(true, result.Overwrite);
            Assert.AreEqual("file.bmp", result.BMPFileName);
            Assert.AreEqual("file.dds", result.DDSFileName);
        }

        [Test]
        public void ParsingThrowsExceptionWhenNoArguments()
        {
            var parser = new CommandLineArgumentsParser();
            var args = new string[] { };

            Assert.Throws<ArgumentException>(() => parser.Parse(args));
        }

        [Test]
        public void ParsingThrowsExceptionWhenOnlyOneArgument()
        {
            var parser = new CommandLineArgumentsParser();
            var args = new string[]
            {
                CommandLineArguments.Keys.Compress,
            };

            Assert.Throws<ArgumentException>(() => parser.Parse(args));
        }

        [Test]
        public void ParsingThrowsExceptionWhenFormatMissing()
        {
            var parser = new CommandLineArgumentsParser();
            var args = new string[]
            {
                CommandLineArguments.Keys.Compress,
                "file.bmp",
                "file.dds"
            };

            Assert.Throws<ArgumentException>(() => parser.Parse(args));
        }

        [Test]
        public void ParsingThrowsExceptionWhenUnknownFormat()
        {
            const char UnknownFormat = '9';

            var parser = new CommandLineArgumentsParser();
            var args = new string[]
            {
                $"{CommandLineArguments.Keys.Compress}{UnknownFormat}",
                "file.bmp",
                "file.dds"
            };

            Assert.Throws<ArgumentException>(() => parser.Parse(args));
        }

        [Test]
        public void ParsingThrowsExceptionWhenTooManyArguments()
        {
            var parser = new CommandLineArgumentsParser();
            var args = new string[]
            {
                CommandLineArguments.Keys.Compress,
                CommandLineArguments.Keys.Decompress,
                CommandLineArguments.Keys.Overwrite,
                "file.bmp",
                "file.dds"
            };

            Assert.Throws<ArgumentException>(() => parser.Parse(args));
        }

        [Test]
        public void ParsingThrowsExceptionWhenConflictingOptions()
        {
            var parser = new CommandLineArgumentsParser();
            var args = new string[]
            {
                CommandLineArguments.Keys.Compress,
                CommandLineArguments.Keys.Decompress,
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
                CommandLineArguments.Keys.Compress,
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
                CommandLineArguments.Keys.Compress,
                "file.bmp",
                "file.txt"
            };

            Assert.Throws<ArgumentException>(() => parser.Parse(args));
        }
    }
}
