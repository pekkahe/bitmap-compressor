namespace BitmapCompressor.Console.CommandLine;

/// <summary>
/// An image operation provided by this application.
/// </summary>
public enum ImageOperation
{
    /// <summary>
    /// Compress a BMP image into a DDS file using a specific block
    /// compression algorithm.
    /// </summary>
    Compress,

    /// <summary>
    /// Decompress a DDS file into a BMP image using the same block
    /// compression algorithm that the DDS file was compressed with.
    /// </summary>
    Decompress
}
