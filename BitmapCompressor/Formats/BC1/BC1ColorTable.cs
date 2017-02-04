using System;
using BitmapCompressor.DataTypes;
using BitmapCompressor.Utilities;

namespace BitmapCompressor.Formats
{
    /// <summary>
    /// Factory class to create BC1 color tables which contain four 16-bit RGB colors.
    /// </summary>
    /// <remarks>
    /// A BC1 color table consists of two reference colors (color0 and color1) and two
    /// intermediate colors (color2 and color3). Only the two reference colors are stored 
    /// in the compressed data and the intermediate colors are interpolated between them.
    /// </remarks>
    public static class BC1ColorTable
    {
        /// <summary>
        /// Creates a BC1 color table from two 16-bit reference colors. The table contains
        /// four 16-bit colors where the third and fourth colors are linear interpolations
        /// between the two reference colors.
        /// </summary>
        /// <param name="color0">The first reference color.</param>
        /// <param name="color1">The second reference color.</param>
        public static Color565[] Create(Color565 color0, Color565 color1)
        {
            var colors = new Color565[4];
            colors[0] = color0;
            colors[1] = color1;
            colors[2] = ColorUtility.LerpTwoThirds(color1, color0);
            colors[3] = ColorUtility.LerpTwoThirds(color0, color1);
            return colors;
        }

        /// <summary>
        /// Creates a BC1 color table from two 16-bit reference colors for 1-bit alpha mode.
        /// The table contains four 16-bit colors where the third color is a blend between the 
        /// two reference colors and the last color is full black #0000.
        /// </summary>
        /// <param name="color0">The first reference color.</param>
        /// <param name="color1">The second reference color.</param>
        public static Color565[] CreateFor1BitAlpha(Color565 color0, Color565 color1)
        {
            var colors = new Color565[4];
            colors[0] = color0;
            colors[1] = color1;
            colors[2] = ColorUtility.Blend(color0, color1);
            colors[3] = Color565.Black;
            return colors;
        }
    }
}
