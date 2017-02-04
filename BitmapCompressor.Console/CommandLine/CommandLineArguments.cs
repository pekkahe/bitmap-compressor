
namespace BitmapCompressor.Console.CommandLine
{
    public class CommandLineArguments
    {
        /// <summary>
        /// The keywords of all supported command line arguments.
        /// </summary>
        public static class Keys
        {
            /// <summary>
            /// The command line prefix argument for compression.
            /// </summary>
            public const string Compress = "-c";

            /// <summary>
            /// The command line suffix argument for BC2 compression.
            /// </summary>
            public const char BC1Format = '1';

            /// <summary>
            /// The command line suffix argument for BC2 compression.
            /// </summary>
            public const char BC2Format = '2';

            /// <summary>
            /// The command line suffix argument for BC3 compression.
            /// </summary>
            public const char BC3Format = '3';

            /// <summary>
            /// The command line argument for decompression.
            /// </summary>
            public const string Decompress = "-d";

            /// <summary>
            /// The command line argument for overwriting an existing output file.
            /// </summary>
            public const string Overwrite = "-w";
        }

        /// <summary>
        /// The selected image processing action.
        /// </summary>
        public CommandLineAction Action { get; set; }

        /// <summary>
        /// Whether to overwrite an existing output file or not.
        /// </summary>
        public bool Overwrite { get; set; }

        /// <summary>
        /// The file name specified for the BMP file.
        /// </summary>
        public string BMPFileName { get; set; }

        /// <summary>
        /// The file name specified for the DDS file.
        /// </summary>
        public string DDSFileName { get; set; }
    }
}
