using System;
using System.Drawing;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using BitmapCompressor.Extensions;
using BitmapCompressor.Utilities;

namespace BitmapCompressor.DataTypes
{
    /// <summary>
    /// A utility class which provides color space information for a set of colors.
    /// </summary>
    /// <remarks>
    /// The minimum and maximum colors <see cref="MinColor"/> and <see cref="MaxColor"/> are 
    /// represented in RGB565 because the order might change if we compared the integer values
    /// of the 32-bit RGB colors.
    /// </remarks>
    public class ColorSpace : IReadOnlyList<Color>
    {
        /// <summary>
        /// The color used in calculating the two endpoints for this color space.
        /// </summary>
        public static readonly Color ReferenceColor = Color.FromArgb(0, 0, 0);

        private readonly List<Color> _colors;

        /// <summary>
        /// Instantiates a <see cref="ColorSpace"/> instance from the given colors which
        /// stores information about the color space and provides access to each individual 
        /// color by index.
        /// </summary>
        /// <param name="colors">The colors to calculate the color space from.</param>
        public ColorSpace(IEnumerable<Color> colors)
        {
            _colors = colors.ToList();

            HasAlpha = CalculateColorEndpoints();

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

        /// <summary>
        /// Calculates <see cref="LowColor"/> and <see cref="HighColor"/> and
        /// returns true whether the colors in this color space contain alpha.
        /// </summary>
        private bool CalculateColorEndpoints()
        {
            var low = Color.FromArgb(255, 255, 255);
            var lowDistance = ColorUtility.Distance(low, ReferenceColor);

            var high = Color.FromArgb(0, 0, 0);
            var highDistance = ColorUtility.Distance(high, ReferenceColor);

            bool hasAlpha = false;

            foreach (var color in _colors)
            {
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

                hasAlpha = hasAlpha || color.HasAlpha();
            }

            LowColor = low;
            HighColor = high;

            return hasAlpha;
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
        public bool HasAlpha { get; }

        public int Count => _colors.Count;

        public Color this[int index] => _colors[index];

        public IEnumerator<Color> GetEnumerator()
        {
            return _colors.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
