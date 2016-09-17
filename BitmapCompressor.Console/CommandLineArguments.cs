
namespace BitmapCompressor.Console
{
    public class CommandLineArguments
    {
        /// <summary>
        /// Whether the BMP compression option was specified.
        /// </summary>
        public bool Compress { get; set; }

        /// <summary>
        /// Whether the DDS decompression option was specified.
        /// </summary>
        public bool Decompress { get; set; }

        /// <summary>
        /// Whether the file overwriting option was specified.
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
