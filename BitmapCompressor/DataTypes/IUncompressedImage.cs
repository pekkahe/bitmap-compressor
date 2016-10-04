using System;
using System.Drawing;

namespace BitmapCompressor.DataTypes
{
    public interface IUncompressedImage : IImage
    {
        /// <summary>
        /// Reads the 16 ARGB (32-bit) colors from the specified 4x4 pixel block of the source image.
        /// </summary>
        Color[] GetBlockPixels(Point block);

        /// <summary>
        /// Sets the 16 ARGB (32-bit) colors at the specified 4x4 pixel block to the source image.
        /// </summary>
        void SetBlockPixels(Point block, Color[] colors);
    }
}
