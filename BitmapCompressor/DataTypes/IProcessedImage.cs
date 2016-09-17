
namespace BitmapCompressor.DataTypes
{
    /// <summary>
    /// Represents a processed image in memory which can be saved to disk.
    /// </summary>
    public interface IProcessedImage
    {
        /// <summary>
        /// The width of the image in pixels.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// The height of the image in pixels.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Saves the image to disk with the specified file name.
        /// </summary>
        void Save(string fileName);
    }
}
