
namespace BitmapCompressor.Serialization.FileFormat
{
    /// <summary>
    /// Flags used in <see cref="DDSFileHeader.Caps"/>.
    /// </summary>
    public static class DDSCapsFlags
    {
        /// <summary>
        /// Optional; must be used on any file that contains more than one surface
        /// (a mipmap, a cubic environment map, or mipmapped volume texture).
        /// </summary>
        public const uint DDSCAPS_COMPLEX = 0x8;

        /// <summary>
        /// Optional; should be used for a mipmap.
        /// </summary>
        public const uint DDSCAPS_MIPMAP = 0x400000;

        /// <summary>
        /// Required.
        /// </summary>
        public const uint DDSCAPS_TEXTURE = 0x1000;
    }
}
