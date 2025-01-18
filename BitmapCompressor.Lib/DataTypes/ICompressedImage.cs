using BitmapCompressor.Lib.Formats;

namespace BitmapCompressor.Lib.DataTypes;

public interface ICompressedImage : IImage
{
    /// <summary>
    /// Returns the <see cref="IBlockCompressionFormat"/> specified for this image.
    /// Provides block compression operations for the surface data.
    /// </summary>
    IBlockCompressionFormat CompressionFormat { get; }

    /// <summary>
    /// Reads the block-compressed data from the image's main surface buffer 
    /// for the specified 4x4 block coordinates.
    /// </summary>
    /// <param name="blockIndex">The index of the 4x4 block to read the layout data for.</param>
    byte[] GetBlockData(Point blockIndex);

    /// <summary>
    /// Writes the block-compressed data to the image's main surface buffer 
    /// to the specified 4x4 block coordinates.
    /// </summary>
    /// <param name="data">The block-compressed data to write to the image buffer.</param>
    /// <param name="blockIndex">The index of the 4x4 block the layout data represents.</param>
    void SetBlockData(Point blockIndex, byte[] data);
}
