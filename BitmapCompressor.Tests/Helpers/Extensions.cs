using System;
using System.Text.RegularExpressions;

namespace BitmapCompressor.Tests.Helpers
{
    public static class Extensions
    {
        /// <summary>
        /// Converts a bit string e.g. "1001 0001" into a byte,
        /// or throws an exception if the conversion failed.
        /// </summary>
        public static byte AsByte(this string bitString)
        {
            return Convert.ToByte(bitString.RemoveWhitespace(), 2);
        }

        public static string RemoveWhitespace(this string source)
        {
            return Regex.Replace(source, @"\s+", "");
        }
    }
}
