
namespace BitmapCompressor.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Replaces the format item in this string with the string
        /// representation of a corresponding object in a specified array.
        /// </summary>
        /// <param name="args">Array that contains zero or more objects to format.</param>
        public static string Parameters(this string source, params object[] args)
        {
            return string.Format(source, args);
        }
    }
}
