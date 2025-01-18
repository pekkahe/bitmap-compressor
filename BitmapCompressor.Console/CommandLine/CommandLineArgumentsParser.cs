using System.Text;
using BitmapCompressor.Lib.Extensions;
using BitmapCompressor.Lib.Formats;

namespace BitmapCompressor.Console.CommandLine;

public partial class CommandLineArguments
{
    public static partial CommandLineArguments Parse(string[] args)
    {
        const int numberOfMaximumArguments = 4;

        if (args.Length > numberOfMaximumArguments)
            throw UnrecognizedCommandLine();

        Array.Reverse(args);

        var argsStack = new Stack<string>(args);
        var commandLineArgs = new CommandLineArguments();

        var firstArg = PopNextArgument(argsStack);
        if (firstArg == Keys.Decompress)
        {
            commandLineArgs.Operation = ImageOperation.Decompress;
        }
        else
        {
            commandLineArgs.Operation = ImageOperation.Compress;
            commandLineArgs.Format = ParseCompressionFormat(firstArg);
        }

        if (PeekNextArgument(argsStack) == Keys.Overwrite)
        {
            PopNextArgument(argsStack);
            commandLineArgs.Overwrite = true;
        }

        commandLineArgs.BMPFileName = ParseBMPFileName(PopNextArgument(argsStack));
        commandLineArgs.DDSFileName = ParseDDSFileName(PopNextArgument(argsStack));

        return commandLineArgs;
    }

    private static IBlockCompressionFormat ParseCompressionFormat(string arg)
    {
        if (arg.StartsWith(Keys.Compress))
        {
            var formatChar = arg.Length == 3 ? arg[2] : default;
            if (formatChar == Keys.BC1Format)
                return new BC1Format();
            if (formatChar == Keys.BC2Format)
                return new BC2Format();
            if (formatChar == Keys.BC3Format)
                return new BC3Format();
        }

        throw UnrecognizedCommandLine();
    }

    private static string PeekNextArgument(Stack<string> args)
    {
        if (args.Count == 0)
            throw UnrecognizedCommandLine();

        return args.Peek();
    }

    private static string PopNextArgument(Stack<string> args)
    {
        if (args.Count == 0)
            throw UnrecognizedCommandLine();

        return args.Pop();
    }

    private static string ParseBMPFileName(string arg)
    {
        if (arg.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase))
            return arg;

        throw ArgumentException($"Unrecognized BMP file name '{arg}'.");
    }

    private static string ParseDDSFileName(string arg)
    {
        if (arg.EndsWith(".dds", StringComparison.OrdinalIgnoreCase))
            return arg;

        throw ArgumentException($"Unrecognized DDS file name '{arg}'.");
    }

    private static ArgumentException UnrecognizedCommandLine()
    {
        return ArgumentException("Unrecognized or incomplete command line.");
    }

    private static ArgumentException ArgumentException(string message)
    {
        return new ArgumentException("Error: {0}\n\n{1}".Parameters(message, BuildHelpMessage()));
    }

    private static string BuildHelpMessage()
    {
        var applicationName = Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName.ToLower());

        var builder = new StringBuilder();

        builder.AppendLine("This program lets you compress BMP files into BCn compressed DDS files and vice versa.");
        builder.AppendLine();
        builder.AppendLine("USAGE:");
        builder.AppendLine("    {0} ({1}N | {2}) [{3}] bmp_filename dds_filename".Parameters(
            applicationName, Keys.Compress, Keys.Decompress, Keys.Overwrite));
        builder.AppendLine();
        builder.AppendLine("    Options:");
        builder.AppendLine("        {0}{1}    Compress the BMP file into a BC1 compressed DDS file.".Parameters(
            Keys.Compress, Keys.BC1Format));
        builder.AppendLine("        {0}{1}    Compress the BMP file into a BC2 compressed DDS file.".Parameters(
            Keys.Compress, Keys.BC2Format));
        builder.AppendLine("        {0}{1}    Compress the BMP file into a BC3 compressed DDS file.".Parameters(
            Keys.Compress, Keys.BC3Format));
        builder.AppendLine("        {0}     Decompress the DDS file into an uncompressed BMP file.".Parameters(
            Keys.Decompress));
        builder.AppendLine("        {0}     Overwrites the target BMP or DDS file if it exists.".Parameters(
            Keys.Overwrite));
        builder.AppendLine();
        builder.AppendLine("Examples:");
        builder.AppendLine("    > {0} {1}{2} file1.bmp file2.dds".Parameters(
            applicationName, Keys.Compress, Keys.BC1Format));
        builder.AppendLine("      Compresses the BMP 'file1.bmp' into a DDS file named 'file2.dds' using BC1.");
        builder.AppendLine();
        builder.AppendLine("    > {0} {1} {2} file1.bmp file2.dds".Parameters(
            applicationName, Keys.Decompress, Keys.Overwrite));
        builder.AppendLine("      Decompresses the DDS 'file2.dds' into a BMP file named 'file1.bmp' overwriting the file if it exists.");

        return builder.ToString();
    }
}
