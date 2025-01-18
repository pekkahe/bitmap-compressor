namespace BitmapCompressor.Lib.DataTypes;

public interface IImage
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
    /// Returns the main surface data of the image.
    /// </summary>
    byte[] GetBuffer();

    /// <summary>
    /// Saves the image to disk with the specified file name.
    /// </summary>
    void Save(string fileName);
}
