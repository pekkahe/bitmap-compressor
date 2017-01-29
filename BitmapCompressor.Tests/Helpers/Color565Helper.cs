using System;
using BitmapCompressor.DataTypes;

namespace BitmapCompressor.Tests.Helpers
{
    public class Color565Helper
    {
        public Color565Helper(ushort value)
        {
            Color       = Color565.FromValue(value);
            LowByte     = (byte) (Color.Value & 0x00FF);
            HighByte    = (byte) (Color.Value >> 8);
        }

        public Color565 Color { get; }

        public byte LowByte { get; }

        public byte HighByte { get; }
    }
}
