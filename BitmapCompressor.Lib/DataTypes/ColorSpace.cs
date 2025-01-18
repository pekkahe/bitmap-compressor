using BitmapCompressor.Lib.Utilities;

namespace BitmapCompressor.Lib.DataTypes;

/// <summary>
/// Provides color space information for a set of colors.
/// </summary>
public class ColorSpace : IReadOnlyList<Color>
{
    private readonly List<Color> _colors;

    /// <summary>
    /// Instantiates a new <see cref="ColorSpace"/> instance from the specified colors.
    /// </summary>
    public ColorSpace(IEnumerable<Color> colors)
    {
        _colors = colors.ToList();
        CalculateColorSpace();
    }

    private void CalculateColorSpace()
    {
        var referenceColor = Color.FromArgb(0, 0, 0);
        var lowColor = Color.FromArgb(255, 255, 255);
        var highColor = Color.FromArgb(0, 0, 0);
        var lowDistance = ColorUtility.Distance(lowColor, referenceColor);
        var highDistance = ColorUtility.Distance(highColor, referenceColor);

        MinAlpha = 255;
        MaxAlpha = 0;

        foreach (var color in _colors)
        {
            var distance = ColorUtility.Distance(color, referenceColor);
            if (distance < lowDistance)
            {
                lowColor = color;
                lowDistance = distance;
            }
            if (distance > highDistance)
            {
                highColor = color;
                highDistance = distance;
            }

            if (color.A < MinAlpha)
                MinAlpha = color.A;
            if (color.A > MaxAlpha)
                MaxAlpha = color.A;
        }

        // Calculate min and max colors from 16-bit colors because their 
        // order might change if the comparison was made with 32-bit colors.

        var lowColor16 = ColorUtility.To16Bit(lowColor);
        var highColor16 = ColorUtility.To16Bit(highColor);

        if (lowColor16.Value < highColor16.Value)
        {
            MinColor = lowColor16;
            MaxColor = highColor16;
        }
        else
        {
            MinColor = highColor16;
            MaxColor = lowColor16;
        }
    }

    /// <summary>
    /// The 16-bit color with the lower integer value out of the two 32-bit color
    /// endpoints in this color space. The endpoints are the colors with the closest 
    /// and furthest Euclidean distances to a reference color (#000).
    /// </summary>
    public Color565 MinColor { get; private set; }

    /// <summary>
    /// The 16-bit color with the higher integer value out of the two 32-bit color
    /// endpoints in this color space. The endpoints are the colors with the closest 
    /// and furthest Euclidean distances to a reference color (#000).
    /// </summary>
    public Color565 MaxColor { get; private set; }

    /// <summary>
    /// The lowest alpha value in this color space.
    /// </summary>
    public byte MinAlpha { get; private set; }

    /// <summary>
    /// The highest alpha value in this color space.
    /// </summary>
    public byte MaxAlpha { get; private set; }

    /// <summary>
    /// Whether this color space has any alpha values or not.
    /// </summary>
    public bool HasAlpha => MinAlpha < byte.MaxValue;

    /// <summary>
    /// The number of colors in this color space.
    /// </summary>
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
