namespace BitmapCompressor.Lib.Extensions;

/// <summary>
/// Provides extension methods for the .NET Framework's <see cref="Point"/> structure.
/// </summary>
public static class PointExtensions
{
    public static Point Add(this Point source, Point other)
    {
        return new Point(source.X + other.X, source.Y + other.Y);
    }

    public static Point Subtract(this Point source, Point other)
    {
        return new Point(source.X - other.X, source.Y - other.Y);
    }
}
