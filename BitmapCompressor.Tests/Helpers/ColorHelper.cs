using System;
using System.Drawing;
using BitmapCompressor.Formats;

namespace BitmapCompressor.Tests.Helpers
{
    public class ColorHelper
    {
        private static readonly Random Random = new Random();

        public static Color[] CreateRandomColors()
        {
            return CreateRandomColorsBetween(Color.Black, Color.White);
        }

        public static Color[] CreateRandomColorsBetween(Color min, Color max)
        {
            var colors = new Color[BlockFormat.PixelCount];
            for (int i = 0; i < colors.Length; ++i)
            {
                colors[i] = CreateRandomColor(min, max);
            }
            return colors;
        }

        private static Color CreateRandomColor(Color min, Color max)
        {
            return Color.FromArgb(
                    Random.Next(min.R + 1, max.R), // min [incl.],  max [excl.]
                    Random.Next(min.G + 1, max.G),
                    Random.Next(min.B + 1, max.B));
        }

        public static void AddAlpha(ref Color color)
        {
            color = Color.FromArgb(50, color.R, color.G, color.B);
        }
    }
}
