﻿
namespace BitmapCompressor.Serialization.FileFormat
{
    /// <summary>
    /// Flags used in <see cref="DDSFileHeader.Caps2"/>.
    /// </summary>
    public static class DDSCapsAdditionalFlags
    {
        /// <summary>
        /// Required for a cube map.
        /// </summary>
        public const uint DDSCAPS2_CUBEMAP = 0x200;

        /// <summary>
        /// Required when these surfaces are stored in a cube map.
        /// </summary>
        public const uint DDSCAPS2_CUBEMAP_POSITIVEX = 0x400;

        /// <summary>
        /// Required when these surfaces are stored in a cube map.
        /// </summary>
        public const uint DDSCAPS2_CUBEMAP_NEGATIVEX = 0x800;

        /// <summary>
        /// Required when these surfaces are stored in a cube map.
        /// </summary>
        public const uint DDSCAPS2_CUBEMAP_POSITIVEY = 0x1000;

        /// <summary>
        /// Required when these surfaces are stored in a cube map.
        /// </summary>
        public const uint DDSCAPS2_CUBEMAP_NEGATIVEY = 0x2000;

        /// <summary>
        /// Required when these surfaces are stored in a cube map.
        /// </summary>
        public const uint DDSCAPS2_CUBEMAP_POSITIVEZ = 0x4000;

        /// <summary>
        /// Required when these surfaces are stored in a cube map.
        /// </summary>
        public const uint DDSCAPS2_CUBEMAP_NEGATIVEZ = 0x8000;

        /// <summary>
        /// Required for a volume texture.
        /// </summary>
        public const uint DDSCAPS2_VOLUME = 0x200000;
    }
}
