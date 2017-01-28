using BitmapCompressor.Formats;

namespace BitmapCompressor.Console.CommandLine
{
    /// <summary>
    /// Enumeration of available command line actions for this application.
    /// </summary>
    public enum CommandLineAction
    {
        /// <summary>
        /// Compress a BMP image into a DDS file using <see cref="BC1Format"/>
        /// block compression.
        /// </summary>
        CompressBC1,

        /// <summary>
        /// Compress a BMP image into a DDS file using <see cref="BC2Format"/>
        /// block compression.
        /// </summary>
        CompressBC2,

        /// <summary>
        /// Decompress a DDS file into a BMP file using the same <see cref="IBlockCompressionFormat"/>
        /// which the BMP file was compressed with.
        /// </summary>
        Decompress
    }
}
