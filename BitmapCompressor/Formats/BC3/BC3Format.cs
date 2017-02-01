using System;
using System.Drawing;
using System.Diagnostics;
using BitmapCompressor.DataTypes;
using BitmapCompressor.Utilities;

namespace BitmapCompressor.Formats
{
    /// <summary>
    /// 
    /// </summary>
    public class BC3Format : IBlockCompressionFormat
    {
        public int BlockSize => BlockFormat.BC3ByteSize;

        public FourCC FourCC => FourCC.BC3Unorm;

        public byte[] Compress(Color[] colors)
        {
            throw new NotImplementedException();
        }

        public Color[] Decompress(byte[] blockData)
        {
            throw new NotImplementedException();
        }
    }
}
