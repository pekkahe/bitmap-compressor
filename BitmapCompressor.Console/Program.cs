using System;
using System.IO;
using System.Diagnostics;
using BitmapCompressor.Formats;
using BitmapCompressor.Console.Utilities;
using BitmapCompressor.DataTypes;

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
            this(new SimpleFileSystem(), new SimpleConsoleInput(), new BlockCompressor(new BC1CompressionFormat()))
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
            if (!args.Compress && !args.Decompress)
                throw new ArgumentException("No image processing operation selected.");

            var fileNames = ReadInputAndOutputFiles(args);

            string inputFile = fileNames.Item1;
            string outputFile = fileNames.Item2;

            if (!_fileSystem.Exists(inputFile))
                throw new FileNotFoundException($"Source file '{inputFile}' not found.");

            if (!PromptForOverwrite(outputFile, args.Overwrite))
                throw new OperationCanceledException("Operation cancelled.");

            IImage image;
            if (args.Compress)
            {
                image = _blockCompressor.Compress(_fileSystem.LoadBitmap(inputFile));
            }
            else
            {
                image = _blockCompressor.Decompress(_fileSystem.LoadDDS(inputFile));
            }

            image.Save(outputFile);
        }

        private Tuple<string, string> ReadInputAndOutputFiles(CommandLineArguments args)
        {
            return args.Compress ? 
                new Tuple<string, string>(args.BMPFileName, args.DDSFileName):
                new Tuple<string, string>(args.DDSFileName, args.BMPFileName);
        }

        private bool PromptForOverwrite(string fileName, bool shouldOverwrite)
        {
            if (!_fileSystem.Exists(fileName) || shouldOverwrite)
                return true;

            System.Console.WriteLine();
            System.Console.WriteLine($"File '{fileName}' already exists. Do you wish to overwrite it?");

            return _inputSystem.PromptYesOrNo();
        }
    }
}
