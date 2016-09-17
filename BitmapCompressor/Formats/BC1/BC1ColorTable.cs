using System;
using System.Drawing;
using System.Diagnostics;
using BitmapCompressor.DataTypes;
using BitmapCompressor.Utilities;
using BitmapCompressor.Extensions;

namespace BitmapCompressor.Formats
{
    /// <summary>
    /// Represents a BC1 color table which contains four 16-bit RGB colors.
    /// </summary>
    /// <remarks><para>
    /// The BC1 color table consists of two reference colors (color0 and color1) and two
    /// intermediate colors (color2 and color3). Only the two reference colors are stored 
    /// and compressed, and the intermediate colors are calculated from them.
    /// </para><para>
    /// 1-bit alpha is triggered by the order of the two compressed reference colors:
    /// if the integer value of color0 &lt;= color1, all pixels marked with index 0x3
    /// are set fully transparent. The formula to calculate the second intermediate color
    /// is changed to compensate the loss of one index (and therefore color).
    /// </para><para>
    /// When the block contains only one type of color there's an issue with 1-bit alpha 
    /// being triggered even if there are no transparent pixels, because the colors integer 
    /// value comparison convention doesn't hold.
    /// </para><para>
    /// We could ensure that color0 and color1 are always differentiated by one, but for the
    /// sake of simplicity we'll just use the color table array index as the BC1 color index.
    /// This ensures that index 0x0 is always selected over the transparency marker 0x3.
    /// </para></remarks>
    public class BC1ColorTable
    {
        /// <summary>
        /// The third BC1 color index which is also used to mark transparent pixels in 1-bit alpha.
        /// </summary>
        private const int AlphaColorIndex = 0x3;

        /// <summary>
        /// The four colors of the BC1 color table where
        /// [0] = color0, 
        /// [1] = color1, 
        /// [2] = color2, 
        /// [3] = color3.
        /// </summary>
        private readonly Color565[] _colors = new Color565[4];

        /// <summary>
        /// Instantiates a new <see cref="BC1ColorTable"/> from the two specified 16-bit 
        /// reference colors and two automatically calculated intermediate colors.
        /// </summary>
        public BC1ColorTable(Color565 color0, Color565 color1)
        {
            Is1BitAlpha = color0.Value <= color1.Value;

            _colors[0] = color0;
            _colors[1] = color1;

            if (Is1BitAlpha)
            {
                _colors[2] = ColorUtility.Blend(_colors[0], _colors[1]);
                _colors[3] = Color565.Black;
            }
            else
            {
                _colors[2] = ColorUtility.LerpTwoThirds(_colors[1], _colors[0]);
                _colors[3] = ColorUtility.LerpTwoThirds(_colors[0], _colors[1]);
            }
        }

        /// <summary>
        /// Instantiates a new <see cref="BC1ColorTable"/> from the four 16-bit colors.
        /// </summary>
        public BC1ColorTable(Color565[] colors)
        {
            Debug.Assert(colors.Length == 4, 
                $"BC1 color table should contain {4} colors instead of {colors.Length}.");

            _colors = colors;

            Is1BitAlpha = _colors[0].Value <= _colors[1].Value;
        }

        /// <summary>
        /// Returns the color for the given BC1 color index.
        /// </summary>
        public Color565 this[int colorIndex] => _colors[colorIndex];

        /// <summary>
        /// Whether the order of the two reference colors has triggered 1-bit alpha or not.
        /// </summary>
        public bool Is1BitAlpha { get; }

        /// <summary>
        /// Returns the appropriate BC1 color index for the given 32-bit ARGB color.
        /// </summary>
        public int IndexFor(Color color)
        {
            if (color.HasAlpha())
            {
                return AlphaColorIndex;
            }

            var closest = ColorUtility.GetClosest(_colors, ColorUtility.To16Bit(color));

            return Array.IndexOf(_colors, closest);
        }

        /// <summary>
        /// Returns the appropriate 32-bit ARGB color for the given BC1 color index.
        /// </summary>
        public Color ColorFor(int index)
        {
            var color = _colors[index];

            if (Is1BitAlpha && index == AlphaColorIndex)
            {
                return Color.FromArgb(0, ColorUtility.To32Bit(color));
            }

            return ColorUtility.To32Bit(color);
        }

        #region Factory methods

        /// <summary>
        /// Instantiates a new <see cref="BC1ColorTable"/> from the 16 32-bit colors of an image block.
        /// </summary>
        /// <param name="colors">The 16 pixel colors of the 4x4 image block in row-major order.</param>
        public static BC1ColorTable FromArgb(Color[] colors)
        {
            var colorSpace = new ColorSpace(colors);

            // Switch the reference colors around depending on alpha
            // 1-bit alpha: color0 <= color1
            // No alpha:    color0 > color1

            if (colorSpace.HasAlpha)
            {
                return new BC1ColorTable(colorSpace.MinColor, colorSpace.MaxColor);
            }
            
            return new BC1ColorTable(colorSpace.MaxColor, colorSpace.MinColor);
        }

        #endregion
    }
}
