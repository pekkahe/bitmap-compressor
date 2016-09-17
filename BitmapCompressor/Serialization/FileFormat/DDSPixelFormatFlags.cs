
namespace BitmapCompressor.Serialization.FileFormat
{
    /// <summary>
    /// Flags used in <see cref="DDSPixelFormat.Flags"/>.
    /// </summary>
    public static class DDSPixelFormatFlags
    {
        /// <summary>
        /// Texture contains alpha data.
        /// </summary>
        /// <remarks>
        /// <see cref="DDSPixelFormat"/> RGBA bit masks contain valid data.
        /// </remarks>
        public const uint DDPF_ALPHAPIXELS = 0x1;

        /// <summary>
        /// Used in some older DDS files for alpha channel only uncompressed data. 
        /// </summary>
        /// <remarks>
        /// <see cref="DDSPixelFormat.RGBBitCount"/> contains the alpha channel bitcount;
        /// <see cref="DDSPixelFormat.ABitMask"/> contains valid data.
        /// </remarks>
        public const uint DDPF_ALPHA = 0x2;

        /// <summary>
        /// Texture contains compressed RGB data.
        /// </summary>
        /// <remarks>
        /// <see cref="DDSPixelFormat.FourCC"/> contains valid data.	
        /// </remarks>
        public const uint DDPF_FOURCC = 0x4;

        /// <summary>
        /// Texture contains uncompressed RGB data.
        /// </summary>
        /// <remarks>
        /// <see cref="DDSPixelFormat.RGBBitCount"/> and the RGB bit masks contain valid data.
        /// </remarks>
        public const uint DDPF_RGB = 0x40;

        /// <summary>
        /// Used in some older DDS files for YUV uncompressed data.
        /// </summary>
        /// <remarks>
        /// <see cref="DDSPixelFormat.RGBBitCount"/> contains the YUV bit count;
        /// <see cref="DDSPixelFormat.RBitMask"/> contains the Y mask;
        /// <see cref="DDSPixelFormat.GBitMask"/> contains the U mask;
        /// <see cref="DDSPixelFormat.BBitMask"/> contains the V mask.
        /// </remarks>
        public const uint DDPF_YUV = 0x200;

        /// <summary>
        /// Used in some older DDS files for single channel color uncompressed data.
        /// </summary>
        /// <remarks>
        /// <see cref="DDSPixelFormat.RGBBitCount"/> contains the luminance channel bit count;
        /// <see cref="DDSPixelFormat.RBitMask"/> contains the channel mask.
        /// Can be combined with <see cref="DDPF_ALPHAPIXELS"/> for a two channel DDS file.
        /// </remarks>
        public const uint DDPF_LUMINANCE = 0x20000;
    }
}
