using System;
using System.Diagnostics;
using BitmapCompressor.DataTypes;

namespace BitmapCompressor.Formats
{
    /// <summary>
    /// Represents the data layout for an 8-byte BC1/DXT1 block.
    /// </summary>
    /// <remarks><para>
    /// BC1/DXT1 format specification: https://www.opengl.org/wiki/S3_Texture_Compression
    /// 63       55       47       39       31       23       15       7        0    Bit
    /// | c0-low | c0-hi  | c1-low | c1-hi  | codes0 | codes1 | codes2 | codes3 |
    /// -------------------------------------------------------------------------
    /// The color values and 2-bit indexes are stored in little-endian format.
    /// </para><para>
    /// Bit layout for the color indexes and pixel values (cXY, where Y goes bottom to top):
    /// 31 30 29 28 27 26 25 24 23 22 21 20 19 18 17 16 15 14 13 12 11 10  9  8  7  6  5  4  3  2  1  0     Bit
    ///  | c00 | c10 | c20 | c30 | c01 | c11 | c21 | c31 | c02 | c12 | c22 | c32 | c03 | c13 | c23 | c33    Pixel
    ///  |       codes3          |       codes2          |       codes1          |       codes0
    ///  ------------------------------------------------------------------------------------------------
    /// </para></remarks>
    public class BC1BlockLayout
    {
        /// <summary>
        /// The number of bytes consumed by a BC1 block.
        /// </summary>
        public const int ByteSize = 8;

        /// <summary>
        /// The bit indexes for the 16 color indexes within the 4x4 block, in row-major order.
        /// The array value specifies how much the 2-bit color index needs to be shifted to 
        /// place it at the correct bit position in the 32-bit integer bit layout.
        /// </summary>
        /// <remarks><para>
        /// The lookup table is built based on the color index bit layout of the BC1/DXT1 format.
        /// The first index [0] represents the top left image pixel (0,0) and the last index [15]
        /// represents the bottom right pixel (3,3) within the 4x4 block. Therefore, the 2-bit index 
        /// for pixel (0,0) is stored to bits 24 and 25, the pixel (1,0) is stored to bits 26 and 27,
        /// and so forth.
        /// </para><para>
        /// Note that in the lookup table the pixel coordinate Y goes top to bottom, natural to .NET,
        /// in contrast to the OpenGL notation used in the BC1/DXT1 specification.
        /// </para></remarks>
        private static readonly int[] PixelToBitIndex = new int[16]
        {
            24, 26, 28, 30, // codes0
            16, 18, 20, 22, // codes1
            08, 10, 12, 14, // codes2
            00, 02, 04, 06  // codes3
        };

        /// <summary>
        /// The 64-bit unsigned integer storing the BC1 block data.
        /// </summary>
        private ulong _data;

        /// <summary>
        /// Instantiates an empty <see cref="BC1BlockLayout"/> data layout.
        /// </summary>
        public BC1BlockLayout()
        {
            ColorIndexes = new BC1ColorIndexIndexer(this);
        }

        /// <summary>
        /// Instantiates a new <see cref="BC1BlockLayout"/> data structure with
        /// the given 8-byte BC1 data buffer.
        /// </summary>
        public BC1BlockLayout(byte[] data) : this()
        {
            // BitConverter operates in LE order on LE machines. To preserve
            // the original order of our data, reverse the byte array prior 
            // converting to 64-bit unsigned integer.
            if (BitConverter.IsLittleEndian)
                Array.Reverse(data);

            _data = BitConverter.ToUInt64(data, 0);
        }

        /// <summary>
        /// The 2-bit index values of the four colors in a BC1 color palette
        /// assigned to each pixel in the block.
        /// </summary>
        public BC1ColorIndexIndexer ColorIndexes { get; }

        /// <summary>
        /// The first reference color of the 4x4 BC1 block's color palette.
        /// </summary>
        public Color565 Color0
        {
            get
            {
                ushort color = (ushort) ((_data & 0xFFFF000000000000) >> 48);

                color = ReverseByteOrder(color);

                return Color565.FromValue(color);
            }
            set
            {
                ushort colorValue = value.Value;

                ulong mask = (ulong) ReverseByteOrder(colorValue) << 48;

                _data |= mask;
            }
        }

        /// <summary>
        /// The second reference color of the 4x4 BC1 block's color palette.
        /// </summary>
        public Color565 Color1
        {
            get
            {
                ushort color = (ushort) ((_data & 0x0000FFFF00000000) >> 32);

                color = ReverseByteOrder(color);

                return Color565.FromValue(color);
            }
            set
            {
                ushort colorValue = value.Value;

                ulong mask = (ulong) ReverseByteOrder(colorValue) << 32;

                _data |= mask;
            }
        }
        
        /// <summary>
        /// Reverses the byte order of the given 16-bit unsigned integer.
        /// </summary>
        /// <remarks>
        /// BC1 color information is stored in little-endian format, so before receiving
        /// or passing data outside the layout we must remember to reverse the byte order.
        /// </remarks>
        private ushort ReverseByteOrder(ushort color)
        {
            color = (ushort) ((color & 0x00FF) << 8 | (color & 0xFF00) >> 8);

            return color;
        }

        /// <summary>
        /// Returns the byte array containing the 8-byte properly laid out
        /// BC1 block data of this instance.
        /// </summary>
        public byte[] GetBuffer()
        {
            byte[] bytes = BitConverter.GetBytes(_data);

            // BitConverter operates in LE order on LE machines. To preserve
            // the original order of our data, reverse the byte array prior 
            // returning the data.
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            return bytes;
        }

        public class BC1ColorIndexIndexer
        {
            private BC1BlockLayout _instance;

            public BC1ColorIndexIndexer(BC1BlockLayout instance)
            {
                _instance = instance;
            }

            /// <summary>
            /// Gets or sets the BC1 color index value for the specified pixel in the 4x4 block.
            /// </summary>
            public int this[int pixelIndex]
            {
                get
                {
                    Debug.Assert(pixelIndex >= 0 && pixelIndex <= 15);

                    int bitIndex = PixelToBitIndex[pixelIndex];
                    int colorIndex = (int) (_instance._data >> bitIndex) & 0x03;

                    return colorIndex;
                }
                set
                {
                    Debug.Assert(pixelIndex >= 0 && pixelIndex <= 15);
                    Debug.Assert(value >= 0 && value <= 3);

                    int bitIndex = PixelToBitIndex[pixelIndex];
                    int colorIndex = value & 0x03;

                    ulong mask = (ulong) colorIndex << bitIndex;

                    _instance._data |= mask;
                }
            }
        }
    }
}
