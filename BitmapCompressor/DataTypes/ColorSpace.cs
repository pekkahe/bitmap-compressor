using System;
using System.Drawing;
using BitmapCompressor.Extensions;
using BitmapCompressor.Utilities;

namespace BitmapCompressor.DataTypes
{
    /// <summary>
    /// A utility data structure which provides color space information for a set of colors.
    /// </summary>
    /// <remarks>
    /// The minimum and maximum colors <see cref="MinColor"/> and <see cref="MaxColor"/> are 
    /// represented in RGB565 because the order might change if we compared the integer values
    /// of the 32-bit RGB colors.
    /// </remarks>
    public class ColorSpace
    {
        /// <summary>
        /// The color used in calculating the two endpoints for this color space.
        /// </summary>
        private static readonly Color ReferenceColor = Color.FromArgb(0, 0, 0);

        /// <summary>
        /// Instantiates a <see cref="ColorSpace"/> object from the given colors.
        /// </summary>
        /// <param name="colors">The colors to calculate the color space from.</param>
        public ColorSpace(Color[] colors)
        {
            CalculateColorEndpoints(colors);

            var low = ColorUtility.To16Bit(LowColor);
            var high = ColorUtility.To16Bit(HighColor);

            if (low.Value < high.Value)
            {
                MinColor = low;
                MaxColor = high;
            }
            else
            {
                MinColor = high;
                MaxColor = low;
            }
        }

        private void CalculateColorEndpoints(Color[] colors)
        {
            var low = Color.FromArgb(255, 255, 255);
            var lowDistance = ColorUtility.Distance(low, ReferenceColor);

            var high = Color.FromArgb(0, 0, 0);
            var highDistance = ColorUtility.Distance(high, ReferenceColor);

            for (int i = 0; i < colors.Length; ++i)
            {
                var color = colors[i];
                var distance = ColorUtility.Distance(color, ReferenceColor);

                if (distance < lowDistance)
                {
                    low = color;
                    lowDistance = distance;
                }

                if (distance > highDistance)
                {
                    high = color;
                    highDistance = distance;
                }

                // Check alpha channel presence on same iteration
                if (color.HasAlpha())
                {
                    HasAlpha = true;
                }
            }

            LowColor = low;
            HighColor = high;
        }

        /// <summary>
        /// The 32-bit color endpoint with the closest Euclidean 
        /// distance to the reference color <see cref="ReferenceColor"/>.
        /// </summary>
        public Color LowColor { get; private set; }

        /// <summary>
        /// The 32-bit color endpoint with the furthest Euclidean 
        /// distance to the reference color <see cref="ReferenceColor"/>.
        /// </summary>
        public Color HighColor { get; private set; }

        /// <summary>
        /// The 16-bit color endpoint with the lower integer value.
        /// </summary>
        public Color565 MinColor { get; }

        /// <summary>
        /// The 16-bit color endpoint with the higher integer value.
        /// </summary>
        public Color565 MaxColor { get; }

        /// <summary>
        /// Whether this color space has an alpha channel or not.
        /// </summary>
        public bool HasAlpha { get; private set; }
    }
}
