namespace BitmapCompressor.Lib.Utilities;

public static class PointUtility
{
    /// <summary>
    /// Transforms the <see cref="Point"/> into an index number
    /// in a two-dimensional plane arranged in row-major order.
    /// </summary>
    /// <param name="columns">The number of columns used for the row-major transformation.</param>
    /// <returns>A 0-based index number for the point.</returns>
    public static int ToRowMajor(Point point, int columns)
    {
        return point.X + (point.Y * columns);
    }

    /// <summary>
    /// Transforms the index number of a two-dimensional plane
    /// arranged in row-major order into a <see cref="Point"/>.
    /// </summary>
    /// <param name="index">The 0-based index number to transform into a point.</param>
    /// <param name="columns">The number of columns used for the row-major transformation.</param>
    /// <returns>A two-dimensional <see cref="Point"/> for the index number.</returns>
    public static Point FromRowMajor(int index, int columns)
    {
        int y = index / columns;
        int x = index - (y * columns);

        return new Point(x, y);
    }
}
