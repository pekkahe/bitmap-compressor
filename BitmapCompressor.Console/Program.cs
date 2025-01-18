global using System;
global using System.Collections.Generic;
global using System.Diagnostics;
global using System.IO;

using BitmapCompressor.Console.CommandLine;
using BitmapCompressor.Console.Utilities;
using BitmapCompressor.Lib;

namespace BitmapCompressor.Console;

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

            Run(CommandLineArguments.Parse(args));

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
        switch (args.Operation)
        {
            case ImageOperation.Compress:
            {
                CheckIfFilesExist(args.BMPFileName, args.DDSFileName, args.Overwrite);

                var inputImage  = _fileSystem.LoadBitmap(args.BMPFileName);
                var outputImage = _blockCompressor.Compress(inputImage, args.Format);

                outputImage.Save(args.DDSFileName);
                break;
            }
            case ImageOperation.Decompress:
            {
                CheckIfFilesExist(args.DDSFileName, args.BMPFileName, args.Overwrite);

                var inputImage  = _fileSystem.LoadDDS(args.DDSFileName);
                var outputImage = _blockCompressor.Decompress(inputImage);

                outputImage.Save(args.BMPFileName);
                break;
            }
            default:
                throw new ArgumentException($"Unsupported command line action: {args.Operation}");
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
}
