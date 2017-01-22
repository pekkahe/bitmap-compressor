using System;
using System.IO;
using BitmapCompressor.DataTypes;
using BitmapCompressor.Console;
using BitmapCompressor.Console.CommandLine;
using BitmapCompressor.Console.Utilities;
using BitmapCompressor.Formats;
using Moq;
using NUnit.Framework;

namespace BitmapCompressor.Tests.UnitTests.Console
{
    [TestFixture(Category = "Console")]
    public class ProgramTests
    {
        private Mock<IFileSystem> _fileSystem;
        private Mock<IInputSystem> _inputSystem;
        private Mock<IBlockCompressor> _compressor;
        private CommandLineArguments _args;
        private Program _program;

        [SetUp]
        public void Setup()
        {
            _args = new CommandLineArguments();
            _args.BMPFileName = "file.bmp";
            _args.DDSFileName = "file.dds";

            _fileSystem = new Mock<IFileSystem>();
            _fileSystem.Setup(f => f.LoadBitmap(It.IsAny<string>())).Returns(It.IsAny<IUncompressedImage>());
            _fileSystem.Setup(f => f.LoadDDS(It.IsAny<string>())).Returns(It.IsAny<ICompressedImage>());

            _inputSystem = new Mock<IInputSystem>();

            _compressor = new Mock<IBlockCompressor>();
            _compressor.Setup(p => p.Compress(It.IsAny<IUncompressedImage>(), It.IsAny<IBlockCompressionFormat>()))
                       .Returns(new Mock<ICompressedImage>().Object);
            _compressor.Setup(p => p.Decompress(It.IsAny<ICompressedImage>()))
                       .Returns(new Mock<IUncompressedImage>().Object);

            _program = new Program(_fileSystem.Object, _inputSystem.Object, _compressor.Object);
        }

        [Test]
        public void RunningWithCompressOptionRunsCompression()
        {
            _args.Action = CommandLineAction.CompressBC1;

            _fileSystem.Setup(f => f.Exists(_args.BMPFileName)).Returns(true);
            _fileSystem.Setup(f => f.Exists(_args.DDSFileName)).Returns(false);

            _program.Run(_args);

            _compressor.Verify(f => f.Compress(It.IsAny<IUncompressedImage>(), It.IsAny<IBlockCompressionFormat>()), Times.Once);
            _compressor.Verify(f => f.Decompress(It.IsAny<ICompressedImage>()), Times.Never);
        }

        [Test]
        public void RunningWithDecompressOptionRunsDecompression()
        {
            _args.Action = CommandLineAction.Decompress;

            _fileSystem.Setup(f => f.Exists(_args.BMPFileName)).Returns(false);
            _fileSystem.Setup(f => f.Exists(_args.DDSFileName)).Returns(true);

            _program.Run(_args);

            _compressor.Verify(f => f.Compress(It.IsAny<IUncompressedImage>(), It.IsAny<IBlockCompressionFormat>()), Times.Never);
            _compressor.Verify(f => f.Decompress(It.IsAny<ICompressedImage>()), Times.Once);
        }

        [Test]
        public void RunningCompressionPromptsForOverwriteWhenDDSFileExists()
        {
            _args.Action = CommandLineAction.Decompress;

            _fileSystem.Setup(f => f.Exists(_args.BMPFileName)).Returns(true);
            _fileSystem.Setup(f => f.Exists(_args.DDSFileName)).Returns(true);

            _inputSystem.Setup(i => i.PromptYesOrNo()).Returns(true);

            try
            {
                _program.Run(_args);
            }
            catch
            { }

            _inputSystem.Verify(s => s.PromptYesOrNo(), Times.Once);
        }

        [Test]
        public void RunningCompressionThrowsExceptionWhenBMPFileDoesNotExist()
        {
            _args.Action = CommandLineAction.CompressBC1;

            _fileSystem.Setup(f => f.Exists(_args.BMPFileName)).Returns(false);

            Assert.Throws<FileNotFoundException>(() => _program.Run(_args));
        }
        
        [Test]
        public void RunningDecompressionPromptsForOverwriteWhenBMPFileExists()
        {
            _args.Action = CommandLineAction.Decompress;

            _fileSystem.Setup(f => f.Exists(_args.BMPFileName)).Returns(true);
            _fileSystem.Setup(f => f.Exists(_args.DDSFileName)).Returns(true);

            _inputSystem.Setup(i => i.PromptYesOrNo()).Returns(true);

            try
            {
                _program.Run(_args);
            }
            catch
            { }

            _inputSystem.Verify(s => s.PromptYesOrNo(), Times.Once);
        }

        [Test]
        public void RunningDecompressionThrowsExceptionWhenDDSFileDoesNotExist()
        {
            _args.Action = CommandLineAction.Decompress;

            _fileSystem.Setup(f => f.Exists(_args.DDSFileName)).Returns(false);

            Assert.Throws<FileNotFoundException>(() => _program.Run(_args));
        }
        
        [Test]
        public void RunningThrowsExceptionWhenTargetFileExistsAndOverwriteDeclined()
        {
            _args.Action = CommandLineAction.CompressBC1;
            _args.Overwrite = false;

            _fileSystem.Setup(f => f.Exists(_args.DDSFileName)).Returns(true);
            _fileSystem.Setup(f => f.Exists(_args.BMPFileName)).Returns(true);

            _inputSystem.Setup(i => i.PromptYesOrNo()).Returns(false);

            Assert.Throws<OperationCanceledException>(() => _program.Run(_args));
        }

        [Test]
        public void RunningDoesNotThrowExceptionWhenTargetFileExistsAndOverwriteSet()
        {
            _args.Action = CommandLineAction.CompressBC1;
            _args.Overwrite = true;

            _fileSystem.Setup(f => f.Exists(_args.DDSFileName)).Returns(true);
            _fileSystem.Setup(f => f.Exists(_args.BMPFileName)).Returns(true);

            Assert.DoesNotThrow(() => _program.Run(_args));
        }
    }
}
