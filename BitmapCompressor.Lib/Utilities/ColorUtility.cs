using BitmapCompressor.Lib.DataTypes;

namespace BitmapCompressor.Lib.Utilities;

public static class ColorUtility
{
    /// <summary>
    /// Blends the two colors by taking the average of each color component.
    /// </summary>
    public static Color Blend(Color a, Color b)
    {
        var c = BlendComponents(
            [a.R, a.G, a.B],
            [b.R, b.G, b.B]);

        return Color.FromArgb(c[0], c[1], c[2]);
    }

    /// <summary>
    /// Blends the two colors by taking the average of each color component.
    /// </summary>
    public static Color565 Blend(Color565 a, Color565 b)
    {
        var c = BlendComponents(
            [a.R, a.G, a.B],
            [b.R, b.G, b.B]);

        return Color565.FromRgb(c[0], c[1], c[2]);
    }

    private static byte[] BlendComponents(byte[] a, byte[] b)
    {
        var components = new byte[3];
        components[0] = ((a[0] + b[0]) / 2d).Round();
        components[1] = ((a[1] + b[1]) / 2d).Round();
        components[2] = ((a[2] + b[2]) / 2d).Round();
        return components;
    }

    /// <summary>
    /// Linearly interpolates to two thirds of the distance from the
    /// first color to the second color for each color component.
    /// </summary>
    public static Color LerpTwoThirds(Color a, Color b)
    {
        var c = LerpComponents(
            [a.R, a.G, a.B],
            [b.R, b.G, b.B]);

        return Color.FromArgb(c[0], c[1], c[2]);
    }

    /// <summary>
    /// Linearly interpolates to two thirds of the distance from the
    /// first color to the second color for each color component.
    /// </summary>
    public static Color565 LerpTwoThirds(Color565 a, Color565 b)
    {
        var c = LerpComponents(
            [a.R, a.G, a.B],
            [b.R, b.G, b.B]);

        return Color565.FromRgb(c[0], c[1], c[2]);
    }

    private static byte[] LerpComponents(byte[] a, byte[] b)
    {
        var components = new byte[3];
        components[0] = ((a[0] + (2 * b[0])) / 3d).Round();
        components[1] = ((a[1] + (2 * b[1])) / 3d).Round();
        components[2] = ((a[2] + (2 * b[2])) / 3d).Round();
        return components;
    }

    private static byte Round(this double value)
    {
        var rounded = Math.Round(value, MidpointRounding.AwayFromZero);
        return (byte) (rounded > 255 ? 255 : rounded);
    }

    /// <summary>
    /// Converts the 16-bit color into a 32-bit color by shifting the bits left for each color
    /// component and ORing with the most significant bits to calculate the remaining values.
    /// </summary>
    public static Color To32Bit(Color565 color)
    {
        int r = color.R << 3;
        r |= (r & 0xE0) >> 5;

        int g = color.G << 2;
        g |= (g & 0xC0) >> 6;

        int b = color.B << 3;
        b |= (b & 0xE0) >> 5;

        return Color.FromArgb(r, g, b);
    }

    /// <summary>
    /// Converts the 32-bit color into a 16-bit R5:G6:B5 color by shifting the bits right for
    /// each color component.
    /// </summary>
    public static Color565 To16Bit(Color color)
    {
        int r = color.R >> 3;
        int g = color.G >> 2;
        int b = color.B >> 3;

        return Color565.FromRgb(r, g, b);
    }

    /// <summary>
    /// Calculates the Euclidean distance between the two 32-bit colors.
    /// </summary>
    /// <remarks>
    /// Euclidean distance: https://en.wikipedia.org/wiki/Euclidean_distance.
    /// </remarks>
    public static double Distance(Color a, Color b)
    {
        return DistanceByComponents(
            [a.R, a.G, a.B],
            [b.R, b.G, b.B]);
    }

    /// <summary>
    /// Calculates the Euclidean distance between the two 16-bit colors.
    /// </summary>
    /// <remarks>
    /// Euclidean distance: https://en.wikipedia.org/wiki/Euclidean_distance.
    /// </remarks>
    public static double Distance(Color565 a, Color565 b)
    {
        return DistanceByComponents(
            [a.R, a.G, a.B],
            [b.R, b.G, b.B]);
    }

    private static double DistanceByComponents(byte[] a, byte[] b)
    {
        int expression =
            (a[0] - b[0]) * (a[0] - b[0]) +
            (a[1] - b[1]) * (a[1] - b[1]) +
            (a[2] - b[2]) * (a[2] - b[2]);

        return Math.Sqrt(expression);
    }

    /// <summary>
    /// Returns the color with the closest Euclidean distance to <paramref name="targetColor"/> 
    /// from the given set of colors.
    /// </summary>
    /// <param name="colors">The colors whose distances are compared against
    ///                      <paramref name="targetColor"/>.</param>
    /// <param name="targetColor">The target color of the comparison.</param>
    public static Color565 GetClosestColor(ICollection<Color565> colors, Color565 targetColor)
    {
        var closest = colors.FirstOrDefault();
        var closestDistance = double.MaxValue;

        foreach (var color in colors)
        {
            var distance = Distance(color, targetColor);
            if (distance < closestDistance)
            {
                closest = color;
                closestDistance = distance;
            }
        }

        return closest;
    }

    /// <summary>
    /// Returns the array index of the color with the closest Euclidean distance to 
    /// <paramref name="targetColor"/> from the given array of colors.
    /// </summary>
    /// <param name="colors">The colors whose distances are compared against
    ///                      <paramref name="targetColor"/>.</param>
    /// <param name="targetColor">The target color of the comparison.</param>
    public static int GetIndexOfClosestColor(Color565[] colors, Color565 targetColor)
    {
        return Array.IndexOf(colors, GetClosestColor(colors, targetColor));
    }

    /// <summary>
    /// Returns the alpha value which matches closest to <paramref name="targetAlpha"/> 
    /// from the given set of alphas.
    /// </summary>
    /// <param name="alphas">The alphas whose values are compared against 
    ///                      <paramref name="targetAlpha"/>.</param>
    /// <param name="targetColor">The target alpha of the comparison.</param>
    public static byte GetClosestAlpha(ICollection<byte> alphas, byte targetAlpha)
    {
        var closest = alphas.FirstOrDefault();
        var closestDifference = byte.MaxValue;

        foreach (var alpha in alphas)
        {
            var difference = (byte) Math.Abs(targetAlpha - alpha);
            if (difference < closestDifference)
            {
                closest = alpha;
                closestDifference = difference;
            }
            else if (difference == closestDifference)
            {
                // Prefer smaller alpha in ties
                closest = alpha < closest ? alpha : closest;
                closestDifference = difference;
            }
        }

        return closest;
    }

    /// <summary>
    /// Returns the array index of the alpha value which matches closest to
    /// <paramref name="targetAlpha"/> from the given set of alphas.
    /// </summary>
    /// <param name="alphas">The alphas whose values are compared against 
    ///                      <paramref name="targetAlpha"/>.</param>
    /// <param name="targetColor">The target alpha of the comparison.</param>
    public static int GetIndexOfClosestAlpha(byte[] alphas, byte targetAlpha)
    {
        return Array.IndexOf(alphas, GetClosestAlpha(alphas, targetAlpha));
    }

    /// <summary>
    /// Tries to differentiate the two colors by one integer value. Does nothing if the
    /// integer values of the two colors are different.
    /// </summary>
    /// <param name="min">The color which is decreased by one, if increasing 
    ///                   <code>max</code> fails.</param>
    /// <param name="max">The color which is increased by one.</param>
    public static void TryDifferentiateByOne(ref Color565 min, ref Color565 max)
    {
        if (min.Value == max.Value)
        {
            if (!TryIncrease(ref max, 1))
            {
                TryIncrease(ref min, -1);
            }
        }
    }

    /// <summary>
    /// Tries to increase a component of the 16-bit color by the given value,
    /// starting from the green component with the highest value range.
    /// </summary>
    /// <param name="color">The 16-bit color to increase.</param>
    /// <param name="value">The value of how much to attempt to increase a component.</param>
    /// <returns>True if any of the components could be increased by the given value,
    ///          false otherwise.</returns>
    private static bool TryIncrease(ref Color565 color, int value)
    {
        const int maxValue6Bit = 63;
        const int maxValue5Bit = 31;

        if (color.G + value > 0 &&
            color.G + value <= maxValue6Bit)
        {
            color = Color565.FromRgb(color.R, color.G + value, color.B);
            return true;
        }

        if (color.R + value > 0 &&
            color.R + value <= maxValue5Bit)
        {
            color = Color565.FromRgb(color.R + value, color.G, color.B);
            return true;
        }

        if (color.B + value > 0 &&
            color.B + value <= maxValue5Bit)
        {
            color = Color565.FromRgb(color.R, color.G, color.B + value);
            return true;
        }

        return false;
    }
}
