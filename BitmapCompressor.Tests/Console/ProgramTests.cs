using System;
using System.IO;
using BitmapCompressor.DataTypes;
using BitmapCompressor.Console;
using BitmapCompressor.Console.Utilities;
using Moq;
using NUnit.Framework;

namespace BitmapCompressor.Tests.Console
{
    [TestFixture]
    public class ProgramTests
    {
        private Mock<IFileSystem> _fileSystem;
        private Mock<IInputSystem> _inputSystem;
        private Mock<IBlockCompressor> _compressor;
        private Mock<IProcessedImage> _image;
        private CommandLineArguments _args;
        private Program _program;

        [SetUp]
        public void Setup()
        {
            _args = new CommandLineArguments();
            _args.BMPFileName = "file.bmp";
            _args.DDSFileName = "file.dds";

            _fileSystem = new Mock<IFileSystem>();
            _fileSystem.Setup(f => f.LoadBitmap(It.IsAny<string>())).Returns(It.IsAny<BMPImage>());
            _fileSystem.Setup(f => f.LoadDDS(It.IsAny<string>())).Returns(It.IsAny<DDSImage>());

            _inputSystem = new Mock<IInputSystem>();
            _image = new Mock<IProcessedImage>();

            _compressor = new Mock<IBlockCompressor>();

            _compressor.Setup(p => p.Compress(It.IsAny<BMPImage>())).Returns(_image.Object);
            _compressor.Setup(p => p.Decompress(It.IsAny<DDSImage>())).Returns(_image.Object);

            _program = new Program(_fileSystem.Object, _inputSystem.Object, _compressor.Object);
        }

        [Test]
        public void Program_RunWithCompressOption_RunsCompression()
        {
            _args.Compress = true;

            _fileSystem.Setup(f => f.Exists(_args.BMPFileName)).Returns(true);
            _fileSystem.Setup(f => f.Exists(_args.DDSFileName)).Returns(false);

            _program.Run(_args);

            _compressor.Verify(f => f.Compress(It.IsAny<BMPImage>()),     Times.Once);
            _compressor.Verify(f => f.Decompress(It.IsAny<DDSImage>()),   Times.Never);
        }

        [Test]
        public void Program_RunWithDecompressOption_RunsDecompression()
        {
            _args.Decompress = true;

            _fileSystem.Setup(f => f.Exists(_args.BMPFileName)).Returns(false);
            _fileSystem.Setup(f => f.Exists(_args.DDSFileName)).Returns(true);

            _program.Run(_args);

            _compressor.Verify(f => f.Compress(It.IsAny<BMPImage>()),     Times.Never);
            _compressor.Verify(f => f.Decompress(It.IsAny<DDSImage>()),   Times.Once);
        }

        [Test]
        public void Program_RunCompressionWhenDDSFileExists_PromptsForOverwrite()
        {
            _args.Compress = true;

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
        public void Program_RunDecompressionWhenBMPFileExists_PromptsForOverwrite()
        {
            _args.Decompress = true;

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
        public void Program_RunWhenTargetFileExistsAndOverwriteSpecified_DoesNotThrowException()
        {
            _args.Compress = true;
            _args.Overwrite = true;

            _fileSystem.Setup(f => f.Exists(_args.DDSFileName)).Returns(true);
            _fileSystem.Setup(f => f.Exists(_args.BMPFileName)).Returns(true);

            Assert.DoesNotThrow(() => _program.Run(_args));
        }

        [Test]
        public void Program_RunWhenTargetFileExistsAndOverwriteDeclined_ThrowsException()
        {
            _args.Compress = true;
            _args.Overwrite = false;

            _fileSystem.Setup(f => f.Exists(_args.DDSFileName)).Returns(true);
            _fileSystem.Setup(f => f.Exists(_args.BMPFileName)).Returns(true);

            _inputSystem.Setup(i => i.PromptYesOrNo()).Returns(false);

            Assert.Throws<OperationCanceledException>(() => _program.Run(_args));
        }

        [Test]
        public void Program_RunCompressionWhenBMPFileDoesNotExist_ThrowsException()
        {
            _args.Compress = true;

            _fileSystem.Setup(f => f.Exists(_args.BMPFileName)).Returns(false);

            Assert.Throws<FileNotFoundException>(() => _program.Run(_args));
        }

        [Test]
        public void Program_RunDecompressionWhenDDSFileDoesNotExist_ThrowsException()
        {
            _args.Decompress = true;

            _fileSystem.Setup(f => f.Exists(_args.DDSFileName)).Returns(false);

            Assert.Throws<FileNotFoundException>(() => _program.Run(_args));
        }
    }
}
