using System;
using System.Text;
using System.Collections.Generic;
using BitmapCompressor.Extensions;

namespace BitmapCompressor.Console.Utilities
{
    public class CommandLineArgumentsParser
    {
        private const int NumberOfMaximumArguments = 4;
        public const string CompressOption = "-c";
        public const string DecompressOption = "-d";
        public const string OverwriteOption = "-w";

        private readonly string _applicationName;
        private Stack<string> _inputArguments;
        private CommandLineArguments _resultArguments;

        public CommandLineArgumentsParser()
        {
            _applicationName = StripFileSuffix(AppDomain.CurrentDomain.FriendlyName.ToLower());
        }
    
        private string StripFileSuffix(string value)
        {
            return value.Substring(0, value.LastIndexOf('.'));
        }

        /// <summary>
        /// Parses the command line string arguments into a <see cref="CommandLineArguments"/>
        /// data structure or throws an exception if the command line is invalid.
        /// </summary>
        public CommandLineArguments Parse(string[] args)
        {
            if (args.Length > NumberOfMaximumArguments)
                throw UnrecognizedCommandLine();

            Array.Reverse(args);

            _inputArguments = new Stack<string>(args);
            _resultArguments = new CommandLineArguments();

            ParseMainOption();

            ParseOverwriteOption();
            
            ParseFileNames();

            return _resultArguments;
        }

        private void ParseMainOption()
        {
            switch (PopNextArgument())
            {
                case CompressOption:
                    _resultArguments.Compress = true;
                    break;

                case DecompressOption:
                    _resultArguments.Decompress = true;
                    break;

                default:
                    throw UnrecognizedCommandLine();
            }
        }

        private void ParseOverwriteOption()
        {
            if (PeekNextArgument() == OverwriteOption)
            {
                _resultArguments.Overwrite = true;
                PopNextArgument();
            }
        }

        private void ParseFileNames()
        {
            _resultArguments.BMPFileName = PopNextArgument();

            if (!_resultArguments.BMPFileName.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase))
                throw ArgumentException($"Unrecognized BMP file name '{_resultArguments.BMPFileName}'.");

            _resultArguments.DDSFileName = PopNextArgument();

            if (!_resultArguments.DDSFileName.EndsWith(".dds", StringComparison.OrdinalIgnoreCase))
                throw ArgumentException($"Unrecognized DDS file name '{_resultArguments.DDSFileName}'.");
        }

        private string PeekNextArgument()
        {
            if (_inputArguments.Count == 0)
                throw UnrecognizedCommandLine();

            return _inputArguments.Peek();
        }

        private string PopNextArgument()
        {
            if (_inputArguments.Count == 0)
                throw UnrecognizedCommandLine();

            return _inputArguments.Pop();
        }

        private ArgumentException UnrecognizedCommandLine()
        {
            return ArgumentException("Unrecognized or incomplete command line.");
        }

        private ArgumentException ArgumentException(string message)
        {
            return new ArgumentException("Error: {0}\n\n{1}".Parameters(message, BuildHelpMessage()));
        }

        private string BuildHelpMessage()
        {
            var builder = new StringBuilder();

            builder.AppendLine("This program lets you compress BMP files into DXT1 (BC1) compressed DDS files and vice versa.");
            builder.AppendLine();
            builder.AppendLine("USAGE:");
            builder.AppendLine("    {0} ({1} | {2}) [{3}] bmp_filename dds_filename".Parameters(_applicationName, CompressOption, DecompressOption, OverwriteOption));
            builder.AppendLine();
            builder.AppendLine("    Options:");
            builder.AppendLine("        {0}        Compress the BMP file into a DXT1 (BC1) compressed DDS file.".Parameters(CompressOption));
            builder.AppendLine("        {0}        Decompress the DDS file into an uncompressed BMP file.".Parameters(DecompressOption));
            builder.AppendLine("        {0}        Overwrites the target BMP or DDS file if it exists.".Parameters(OverwriteOption));
            builder.AppendLine();
            builder.AppendLine("Examples:");
            builder.AppendLine("    > {0} {1} file1.bmp file2.dds".Parameters(_applicationName, CompressOption));
            builder.AppendLine("      Compresses the BMP 'file1.bmp' into a DDS file named 'file2.dds'.");
            builder.AppendLine();
            builder.AppendLine("    > {0} {1} {2} file1.bmp file2.dds".Parameters(_applicationName, DecompressOption, OverwriteOption));
            builder.AppendLine("      Decompresses the DDS 'file2.dds' into a BMP file named 'file1.bmp' overwriting the file if it exists.");

            return builder.ToString();
        }
    }
}
