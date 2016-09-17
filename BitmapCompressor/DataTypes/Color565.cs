using System;

namespace BitmapCompressor.DataTypes
{
    /// <summary>
    /// Represents a 16-bit RGB color in the format R5:G6:B5.
    /// </summary>
    public struct Color565
    {
        private readonly ushort _value;

        private Color565(ushort value)
        {
            _value = value;
        }

        /// <summary>
        /// Returns the color with the value #0000.
        /// </summary>
        public static Color565 Black => new Color565(0);

        /// <summary>
        /// The 5-bit red component value for this color.
        /// </summary>
        public byte R => (byte) ((_value & 0xF800) >> 11);

        /// <summary>
        /// The 6-bit green component value for this color.
        /// </summary>
        public byte G => (byte) ((_value & 0x7E0) >> 5);

        /// <summary>
        /// The 5-bit green component value for this color.
        /// </summary>
        public byte B => (byte) (_value & 0x1F);

        /// <summary>
        /// Returns the 16-bit RGB value for this color.
        /// </summary>
        public ushort Value => _value;

        /// <summary>
        /// Creates a 16-bit RGB565 color from the given components. Although this method
        /// allows a 32-bit value to be passed for each color component, the value of each
        /// component is limited to 5:6:5 bits, respectively.
        /// </summary>
        public static Color565 FromRgb(int red, int green, int blue)
        {
            int r = red   > 0x1F ? 0x1F : red   & 0x1F;
            int g = green > 0x3F ? 0x3F : green & 0x3F;
            int b = blue  > 0x1F ? 0x1F : blue  & 0x1F;

            ushort value = (ushort) ((r << 11) | (g << 5) | b);

            return new Color565(value);
        }

        /// <summary>
        /// Creates a 16-bit RGB565 color from the given components. Although this method
        /// allows a 8-bit value to be passed for each color component, the value of each
        /// component is limited to 5:6:5 bits, respectively.
        /// </summary>
        public static Color565 FromRgb(byte red, byte green, byte blue)
        {
            return FromRgb((int) red, (int) green, (int) blue);
        }

        /// <summary>
        /// Creates a 16-bit RGB565 color from the given 16-bit unsigned integer.
        /// </summary>
        public static Color565 FromValue(ushort value)
        {
            return new Color565(value);
        }
    }
}
