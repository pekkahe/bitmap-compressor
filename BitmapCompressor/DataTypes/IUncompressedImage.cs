using System;
using System.Drawing;

namespace BitmapCompressor.DataTypes
{
    public interface IUncompressedImage : IImage
    {
        /// <summary>
        /// Gets the 32-bit colors from a 4x4 pixel area in the source image.
        /// </summary>
        /// <param name="blockIndex">The index of the 4x4 area in the source image.</param>
        /// <returns>16 32-bit colors of a 4x4 pixel area in the source image.</returns>
        Color[] GetBlockColors(Point blockIndex);

        /// <summary>
        /// Sets the 32-bit colors of a 4x4 pixel area in the source image.
        /// </summary>
        /// <param name="blockIndex">The index of the 4x4 area in the source image.</param>
        /// <param name="colors">The 16 32-bit colors to write to the area.</param>
        void SetBlockColors(Point blockIndex, Color[] colors);
    }
}
