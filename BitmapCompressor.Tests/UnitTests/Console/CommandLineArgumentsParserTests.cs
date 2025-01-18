using System;
using BitmapCompressor.Console.CommandLine;
using BitmapCompressor.Lib.Formats;
using NUnit.Framework;

namespace BitmapCompressor.Tests.UnitTests.Console;

[TestFixture(Category = "Console")]
public class CommandLineArgumentsParserTests
{
    [TestCase(CommandLineArguments.Keys.BC1Format, typeof(BC1Format), 
              TestName = "ParseCompressOptionForBC1")]
    [TestCase(CommandLineArguments.Keys.BC2Format, typeof(BC2Format),
              TestName = "ParseCompressOptionForBC2")]
    [TestCase(CommandLineArguments.Keys.BC3Format, typeof(BC3Format),
              TestName = "ParseCompressOptionForBC3")]
    public void ParsingCompressOption(char key, Type compressionFormatType)
    {
        var args = new string[]
        {
            $"{CommandLineArguments.Keys.Compress}{key}",
            "file.bmp",
            "file.dds"
        };

        var result = CommandLineArguments.Parse(args);

        Assert.That(result.Operation, Is.EqualTo(ImageOperation.Compress));
        Assert.That(result.Format, Is.Not.Null);
        Assert.That(result.Format.GetType(), Is.EqualTo(compressionFormatType));
        Assert.That(result.BMPFileName, Is.EqualTo("file.bmp"));
        Assert.That(result.DDSFileName, Is.EqualTo("file.dds"));
    }

    [Test]
    public void ParseDecompressOption()
    {
        var args = new string[]
        {
            CommandLineArguments.Keys.Decompress,
            "file.bmp",
            "file.dds"
        };

        var result = CommandLineArguments.Parse(args);

        Assert.That(result.Operation, Is.EqualTo(ImageOperation.Decompress));
        Assert.That(result.Format, Is.Null);
        Assert.That(result.Overwrite, Is.False);
        Assert.That(result.BMPFileName, Is.EqualTo("file.bmp"));
        Assert.That(result.DDSFileName, Is.EqualTo("file.dds"));
    }

    [Test]
    public void ParseOverwriteOption()
    {
        var args = new string[]
        {
            $"{CommandLineArguments.Keys.Compress}{CommandLineArguments.Keys.BC1Format}",
            CommandLineArguments.Keys.Overwrite,
            "file.bmp",
            "file.dds"
        };

        var result = CommandLineArguments.Parse(args);

        Assert.That(result.Operation, Is.EqualTo(ImageOperation.Compress));
        Assert.That(result.Format, Is.Not.Null);
        Assert.That(result.Overwrite, Is.True);
        Assert.That(result.BMPFileName, Is.EqualTo("file.bmp"));
        Assert.That(result.DDSFileName, Is.EqualTo("file.dds"));
    }

    [Test]
    public void ParsingThrowsExceptionWhenNoArguments()
    {
        var args = new string[] { };

        Assert.Throws<ArgumentException>(() => CommandLineArguments.Parse(args));
    }

    [Test]
    public void ParsingThrowsExceptionWhenOnlyOneArgument()
    {
        var args = new string[]
        {
            CommandLineArguments.Keys.Compress,
        };

        Assert.Throws<ArgumentException>(() => CommandLineArguments.Parse(args));
    }

    [Test]
    public void ParsingThrowsExceptionWhenFormatMissing()
    {
        var args = new string[]
        {
            CommandLineArguments.Keys.Compress,
            "file.bmp",
            "file.dds"
        };

        Assert.Throws<ArgumentException>(() => CommandLineArguments.Parse(args));
    }

    [Test]
    public void ParsingThrowsExceptionWhenUnknownFormat()
    {
        const char UnknownFormat = '9';

        var args = new string[]
        {
            $"{CommandLineArguments.Keys.Compress}{UnknownFormat}",
            "file.bmp",
            "file.dds"
        };

        Assert.Throws<ArgumentException>(() => CommandLineArguments.Parse(args));
    }

    [Test]
    public void ParsingThrowsExceptionWhenTooManyArguments()
    {
        var args = new string[]
        {
            CommandLineArguments.Keys.Compress,
            CommandLineArguments.Keys.Decompress,
            CommandLineArguments.Keys.Overwrite,
            "file.bmp",
            "file.dds"
        };

        Assert.Throws<ArgumentException>(() => CommandLineArguments.Parse(args));
    }

    [Test]
    public void ParsingThrowsExceptionWhenConflictingOptions()
    {
        var args = new string[]
        {
            CommandLineArguments.Keys.Compress,
            CommandLineArguments.Keys.Decompress,
            "file.bmp",
            "file.dds"
        };

        Assert.Throws<ArgumentException>(() => CommandLineArguments.Parse(args));
    }

    [Test]
    public void ParsingThrowsExceptionWhenBMPFileHasInvalidExtension()
    {
        var args = new string[]
        {
            CommandLineArguments.Keys.Compress,
            "file.txt",
            "file.dds"
        };

        Assert.Throws<ArgumentException>(() => CommandLineArguments.Parse(args));
    }

    [Test]
    public void ParsingThrowsExceptionWhenDDSFileHasInvalidExtension()
    {
        var args = new string[]
        {
            CommandLineArguments.Keys.Compress,
            "file.bmp",
            "file.txt"
        };

        Assert.Throws<ArgumentException>(() => CommandLineArguments.Parse(args));
    }
}
