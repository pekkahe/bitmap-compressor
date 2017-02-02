using System;
using System.Diagnostics;
using BitmapCompressor.DataTypes;

namespace BitmapCompressor.Formats
{
    /// <summary>
    /// Represents the data layout for a 16-byte BC3 compressed block.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The block stores two 16-bit reference colors and a 32-bit color index table, 
    /// similar to <see cref="BC1BlockData"/>. It also stores alpha as two 8-bit 
    /// reference values and a 48-bit index table for mapping a 3-bit alpha table
    /// index to each pixel in the block.
    /// </para>
    /// <para>
    /// 128-bit block layout:
    /// -------------------------------------------------------------------------
    /// 127      119      111      103      95       87       79       71       63
    /// | alpha0 | alpha1 | a-idx2 | a-idx1 | a-idx0 | a-idx5 | a-idx4 | a-idx3 |
    /// 63       55       47       39       31       23       15       7        0 
    /// | c0-low | c0-hi  | c1-low | c1-hi  | c-idx0 | c-idx1 | c-idx2 | c-idx3 |
    /// -------------------------------------------------------------------------
    /// </para>
    /// <para>
    /// 3-bit alpha index values per pixel a-p (0-15):
    /// -----------------------------------------------------------------------
    /// 47 46 45 44 43 42 41 40 39 38 37 36 35 34 33 32 31 30 29 28 27 26 25 24  
    ///  |   h    |   g    |   f    |   e    |   d    |   c    |   b    |   a  
    ///           a-idx2         |         a-idx1        |        a-idx0
    /// 23 22 21 20 19 18 17 16 15 14 13 12 11 10  9  8  7  6  5  4  3  2  1  0
    ///  |   p    |   o    |   n    |   m    |   l    |   k    |   j    |   i
    ///           a-idx5         |         a-idx4        |        a-idx3
    /// -----------------------------------------------------------------------
    /// </para>
    /// <para>
    /// 2-bit color index values per pixel a-p (0-15):
    /// -----------------------------------------------
    /// 31 30 29 28 27 26 25 24 23 22 21 20 19 18 17 16 
    ///  |  d  |  c  |  b  |  a  |  h  |  g  |  f  |  e 
    ///           c-idx0         |        c-idx1
    /// 15 14 13 12 11 10  9  8  7  6  5  4  3  2  1  0
    ///  |  l  |  k  |  j  |  i  |  p  |  o  |  n  |  m
    ///           c-idx2         |        c-idx3
    /// -----------------------------------------------
    /// </para>
    /// </remarks>
    public class BC3BlockData
    {
        /// <summary>
        /// Instantiates an empty <see cref="BC3BlockData"/> representing
        /// the data layout of a BC3 compressed block. 
        /// </summary>
        public BC3BlockData()
        { }

        /// <summary>
        /// The first compressed reference alpha value.
        /// </summary>
        public byte Alpha0 { get; set; }

        /// <summary>
        /// The second compressed reference alpha value.
        /// </summary>
        public byte Alpha1 { get; set; }

        /// <summary>
        /// The first compressed 16-bit reference color.
        /// </summary>
        public Color565 Color0 { get; set; }

        /// <summary>
        /// The second compressed 16-bit reference color.
        /// </summary>
        public Color565 Color1 { get; set; }

        /// <summary>
        /// An array of 16 2-bit color index values, ordered by pixel index p0-15, 
        /// following row-major order within the 4x4 block.
        /// </summary>
        /// <remarks>
        /// Values higher than 2-bits are automatically stripped to 2-bits when
        /// the block instance is converted to bytes.
        /// </remarks>
        public int[] ColorIndexes { get; } = new int[BlockFormat.PixelCount];

        /// <summary>
        /// An array of 16 3-bit color alpha index values, ordered by pixel index p0-15, 
        /// following row-major order within the 4x4 block.
        /// </summary>
        /// <remarks>
        /// Values higher than 3-bits are automatically stripped to 3-bits when
        /// the block instance is converted to bytes.
        /// </remarks>
        public int[] AlphaIndexes { get; } = new int[BlockFormat.PixelCount];

        /// <summary>
        /// Convert the block data into a 16-byte BC3 format byte array.
        /// </summary>
        public byte[] ToBytes()
        {
            byte c0Low  = (byte) ((Color0.Value & 0x00FF) >> 0);
            byte c0Hi   = (byte) ((Color0.Value & 0xFF00) >> 8);
            byte c1Low  = (byte) ((Color1.Value & 0x00FF) >> 0);
            byte c1Hi   = (byte) ((Color1.Value & 0xFF00) >> 8);

            // Setup alpha index bytes (a-idx[0-5]) from 3-bit
            // index values given for each pixel a-p (0-15) 
            byte[] alphas = new byte[6]; 

            alphas[0] = (byte)  ((AlphaIndexes[0]  & 0x07) |        // xxxx x111
                                ((AlphaIndexes[1]  & 0x07) << 3) |  // xx11 1xxx      
                                ((AlphaIndexes[2]  & 0x03) << 6));  // 11xx xxxx LSB
            alphas[1] = (byte) (((AlphaIndexes[2]  & 0x07) >> 2) |  // xxxx xxx1 MSB
                                ((AlphaIndexes[3]  & 0x07) << 1) |  // xxxx 111x
                                ((AlphaIndexes[4]  & 0x07) << 4) |  // x111 xxxx     
                                ((AlphaIndexes[5]  & 0x01) << 7));  // 1xxx xxxx LSB
            alphas[2] = (byte) (((AlphaIndexes[5]  & 0x07) >> 1) |  // xxxx xx11 MSB
                                ((AlphaIndexes[6]  & 0x07) << 2) |  // xxx1 11xx
                                ((AlphaIndexes[7]  & 0x07) << 5));  // 111x xxxx
            alphas[3] = (byte) (((AlphaIndexes[8]  & 0x07)) |       // xxxx x111
                                ((AlphaIndexes[9]  & 0x07) << 3) |  // xx11 1xxx       
                                ((AlphaIndexes[10] & 0x03) << 6));  // 11xx xxxx LSB
            alphas[4] = (byte) (((AlphaIndexes[10] & 0x07) >> 2) |  // xxxx xxx1 MSB
                                ((AlphaIndexes[11] & 0x07) << 1) |  // xxxx 111x
                                ((AlphaIndexes[12] & 0x07) << 4) |  // x111 xxxx
                                ((AlphaIndexes[13] & 0x01) << 7));  // 1xxx xxxx LSB
            alphas[5] = (byte) (((AlphaIndexes[13] & 0x07) >> 1) |  // xxxx xx11 MSB
                                ((AlphaIndexes[14] & 0x07) << 2) |  // xxx1 11xx
                                ((AlphaIndexes[15] & 0x07) << 5));  // 111x xxxx

            // Setup color index bytes (c-idx[0-3]) from 2-bit
            // index values given for each pixel a-p (0-15)
            byte[] indexes = new byte[4];

            for (int p = 0, row = 0; p < BlockFormat.PixelCount; p += BlockFormat.Dimension, ++row)
            {
                int a = p;
                int b = p + 1;
                int c = p + 2;
                int d = p + 3;

                indexes[row] = (byte)   ((ColorIndexes[a] & 0x03) |
                                        ((ColorIndexes[b] & 0x03) << 2) |
                                        ((ColorIndexes[c] & 0x03) << 4) |
                                        ((ColorIndexes[d] & 0x03) << 6));
            }
            
            var bytes = new byte[16];

            bytes[0] = Alpha0;
            bytes[1] = Alpha1;
            bytes[2] = alphas[2];
            bytes[3] = alphas[1];
            bytes[4] = alphas[0];
            bytes[5] = alphas[5];
            bytes[6] = alphas[4];
            bytes[7] = alphas[3];
            bytes[8] = c0Low;
            bytes[9] = c0Hi;
            bytes[10] = c1Low;
            bytes[11] = c1Hi;
            bytes[12] = indexes[0];
            bytes[13] = indexes[1];
            bytes[14] = indexes[2];
            bytes[15] = indexes[3];

            return bytes;
        }

        /// <summary>
        /// Instantiates a <see cref="BC3BlockData"/> from compressed BC3 block data.
        /// </summary>
        /// <param name="bytes">The data of a BC3 compressed block.</param>
        public static BC3BlockData FromBytes(byte[] bytes)
        {
            Debug.Assert(bytes.Length == BlockFormat.BC3ByteSize,
                "Mismatching number of bytes for format.");

            byte alpha0     = bytes[0];
            byte alpha1     = bytes[1];
            byte[] alphas   = new byte[6];
            alphas[2]       = bytes[2];
            alphas[1]       = bytes[3];
            alphas[0]       = bytes[4];
            alphas[5]       = bytes[5];
            alphas[4]       = bytes[6];
            alphas[3]       = bytes[7];
            byte c0Low      = bytes[8];
            byte c0Hi       = bytes[9];
            byte c1Low      = bytes[10];
            byte c1Hi       = bytes[11];
            byte[] indexes  = new byte[4];
            indexes[0]      = bytes[12];
            indexes[1]      = bytes[13];
            indexes[2]      = bytes[14];
            indexes[3]      = bytes[15];

            var block = new BC3BlockData();
            block.Alpha0 = alpha0;
            block.Alpha1 = alpha1;
            block.Color0 = Color565.FromValue((ushort) ((c0Hi << 8) | c0Low));
            block.Color1 = Color565.FromValue((ushort) ((c1Hi << 8) | c1Low));

            block.AlphaIndexes[0]   =   alphas[0] & 0x07;           // xxxx x111            
            block.AlphaIndexes[1]   =  (alphas[0] >> 3) & 0x07;     // xx11 1xxx            
            block.AlphaIndexes[2]   = ((alphas[0] >> 6) & 0x03) |   // 11xx xxxx LSB        
                                      ((alphas[1] & 0x01) << 2);    // xxxx xxx1 MSB        
            block.AlphaIndexes[3]   =  (alphas[1] >> 1) & 0x07;     // xxxx 111x            
            block.AlphaIndexes[4]   =  (alphas[1] >> 4) & 0x07;     // x111 xxxx            
            block.AlphaIndexes[5]   = ((alphas[1] >> 7) & 0x01) |   // 1xxx xxxx LSB        
                                      ((alphas[2] & 0x03) << 1);    // xxxx xx11 MSB        
            block.AlphaIndexes[6]   =  (alphas[2] >> 2) & 0x07;     // xxx1 11xx            
            block.AlphaIndexes[7]   =  (alphas[2] >> 5) & 0x07;     // 111x xxxx            
            block.AlphaIndexes[8]   =   alphas[3] & 0x07;           // xxxx x111            
            block.AlphaIndexes[9]   =  (alphas[3] >> 3) & 0x07;     // xx11 1xxx            
            block.AlphaIndexes[10]  = ((alphas[3] >> 6) & 0x03) |   // 11xx xxxx LSB        
                                      ((alphas[4] & 0x01) << 2);    // xxxx xxx1 MSB        
            block.AlphaIndexes[11]  =  (alphas[4] >> 1) & 0x07;     // xxxx 111x            
            block.AlphaIndexes[12]  =  (alphas[4] >> 4) & 0x07;     // x111 xxxx            
            block.AlphaIndexes[13]  = ((alphas[4] >> 7) & 0x01) |   // 1xxx xxxx LSB        
                                      ((alphas[5] & 0x03) << 1);    // xxxx xx11 MSB        
            block.AlphaIndexes[14]  =  (alphas[5] >> 2) & 0x07;     // xxx1 11xx            
            block.AlphaIndexes[15]  =  (alphas[5] >> 5) & 0x07;     // 111x xxxx                      

            for (int p = 0, row = 0; p < BlockFormat.PixelCount; p += BlockFormat.Dimension, ++row)
            {
                int a = p;
                int b = p + 1;
                int c = p + 2;
                int d = p + 3;

                block.ColorIndexes[a] = indexes[row] & 0x03;
                block.ColorIndexes[b] = (indexes[row] >> 2) & 0x03;
                block.ColorIndexes[c] = (indexes[row] >> 4) & 0x03;
                block.ColorIndexes[d] = (indexes[row] >> 6) & 0x03;
            }

            return block;
        }
    }
}
