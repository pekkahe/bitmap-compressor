using System;
using System.IO;
using System.Diagnostics;
using BitmapCompressor.Console.CommandLine;
using BitmapCompressor.Console.Utilities;
using BitmapCompressor.Formats;

namespace BitmapCompressor.Console
{
    public class Program
    {
        #region Entry point

        private static void Main(string[] args)
        {
            new Program().Run(args);
        }

        #endregion

        private readonly IFileSystem _fileSystem;
        private readonly IInputSystem _inputSystem;
        private readonly IBlockCompressor _blockCompressor;

        public Program() :
            this(new SimpleFileSystem(), new SimpleConsoleInput(), new BlockCompressor())
        { }

        public Program(IFileSystem fileSystem, IInputSystem inputSystem, IBlockCompressor blockCompressor)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;

            _fileSystem = fileSystem;
            _inputSystem = inputSystem;
            _blockCompressor = blockCompressor;
        }

        private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
                Debugger.Break();

            System.Console.WriteLine();
            System.Console.WriteLine(e.ExceptionObject.ToString());
            System.Console.WriteLine("Press Enter to continue");
            System.Console.ReadLine();

            Environment.Exit(1);
        }

        public void Run(string[] args)
        {
            try
            {
                System.Console.WriteLine();

                Run(new CommandLineArgumentsParser().Parse(args));

                System.Console.WriteLine();
            }
            catch (Exception e)
            {
                if (Debugger.IsAttached)
                    Debugger.Break();

                System.Console.WriteLine();
                System.Console.WriteLine(e.Message);

                Environment.Exit(1);
            }
        }

        public void Run(CommandLineArguments args)
        {
            switch (args.Action)
            {
                case CommandLineAction.CompressBC1:
                case CommandLineAction.CompressBC2:
                {
                    CheckIfFilesExist(args.BMPFileName, args.DDSFileName, args.Overwrite);

                    var format = ToCompressionFormat(args.Action);

                    var inputImage  = _fileSystem.LoadBitmap(args.BMPFileName);
                    var outputImage = _blockCompressor.Compress(inputImage, format);

                    outputImage.Save(args.DDSFileName);
                    break;
                }
                case CommandLineAction.Decompress:
                {
                    CheckIfFilesExist(args.DDSFileName, args.BMPFileName, args.Overwrite);

                    var inputImage  = _fileSystem.LoadDDS(args.DDSFileName);
                    var outputImage = _blockCompressor.Decompress(inputImage);

                    outputImage.Save(args.BMPFileName);
                    break;
                }
                default:
                    throw new ArgumentException("Unknown command line action.");
            }
        }

        private void CheckIfFilesExist(string inputFile, string outputFile, bool overwriteOutputFileIfExists = false)
        {
            if (!_fileSystem.Exists(inputFile))
                throw new FileNotFoundException($"Source file '{inputFile}' not found.");

            if (!PromptForOverwrite(outputFile, overwriteOutputFileIfExists))
                throw new OperationCanceledException("Operation cancelled.");
        }

        private bool PromptForOverwrite(string fileName, bool shouldOverwrite)
        {
            if (!_fileSystem.Exists(fileName) || shouldOverwrite)
                return true;

            System.Console.WriteLine();
            System.Console.WriteLine($"File '{fileName}' already exists. Do you wish to overwrite it?");

            return _inputSystem.PromptYesOrNo();
        }

        private static IBlockCompressionFormat ToCompressionFormat(CommandLineAction action)
        {
            switch (action)
            {
                case CommandLineAction.CompressBC1:
                    return new BC1Format();

                case CommandLineAction.CompressBC2:
                    return new BC2Format();

                default:
                    throw new ArgumentOutOfRangeException(nameof(action));
            }
        }
    }
}
