using BitmapCompressor.DataTypes;

namespace BitmapCompressor.Console.Utilities
{
    /// <summary>
    /// Provides file system operations for the console application.
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        /// Determines whether the specified file exists.
        /// </summary>
        bool Exists(string filePath);

        /// <summary>
        /// Loads the specified bitmap file.
        /// </summary>
        BMPImage LoadBitmap(string filePath);

        /// <summary>
        /// Loads the specified Direct Draw Surface file.
        /// </summary>
        DDSImage LoadDDS(string filePath);
    }
}
