using System;

namespace BitmapCompressor.Extensions
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Copies a range of elements from this array into another new array.
        /// </summary>
        /// <param name="sourceIndex">The start index for the copied elements in this array.</param>
        /// <param name="length">The number of elements copied to the new array.</param>
        public static T[] SubArray<T>(this T[] source, int sourceIndex, int length)
        {
            T[] result = new T[length];
            Array.Copy(source, sourceIndex, result, 0, length);
            return result;
        }

        /// <summary>
        /// Copies the elements from another array to this array.
        /// </summary>
        /// <param name="source">The array to copy the elements from.</param>
        /// <param name="destinationIndex">The start index for the copied elements on this array.</param>
        public static void CopyFrom<T>(this T[] destination, T[] source, int destinationIndex)
        {
            Array.Copy(source, 0, destination, destinationIndex, source.Length);
        }
    }
}
