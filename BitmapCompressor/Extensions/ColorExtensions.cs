using System;
using System.Drawing;

namespace BitmapCompressor.Extensions
{
    public static class ColorExtensions
    {
        public static bool HasAlpha(this Color color)
        {
            return color.A < 0xFF;
        }
    }
}
