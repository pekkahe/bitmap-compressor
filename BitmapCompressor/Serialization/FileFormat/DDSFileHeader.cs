
namespace BitmapCompressor.Serialization.FileFormat
{
    /// <summary>
    /// Describes a DDS file header as specified by MSDN:
    /// https://msdn.microsoft.com/en-us/library/bb943982(v=vs.85).aspx.
    /// </summary>
    /// <remarks>
    /// The size and order of the fields should match exactly with the specification 
    /// as instances of this structure are written directly into the memory stream.
    /// </remarks>
    public unsafe struct DDSFileHeader
    {
        /// <summary>
        /// The structure size. Must be set to 124 (bytes).
        /// </summary>
        public uint Size;

        /// <summary>
        /// Flags to indicate which members contain valid data.
        /// See <see cref="DDSFileHeaderFlags"/> for values.
        /// </summary>
        public uint Flags;

        /// <summary>
        /// Surface height in pixels.
        /// </summary>
        public uint Height;

        /// <summary>
        /// Surface width in pixels.
        /// </summary>
        public uint Width;

        /// <summary>
        /// The pitch or number of bytes per scan line in an uncompressed texture;
        /// the total number of bytes in the top level texture for a compressed texture.
        /// </summary>
        public uint PitchOrLinearSize;

        /// <summary>
        /// Depth of a volume texture in pixels, otherwise unused.
        /// </summary>
        public uint Depth;

        /// <summary>
        /// Number of mipmap levels, otherwise unused.
        /// </summary>
        public uint MipMapCount;

        /// <summary>
        /// Unused. Reserved space for 11 items.
        /// </summary>
        public fixed uint Reserved1[11];

        /// <summary>
        /// The pixel format.
        /// </summary>
        public DDSPixelFormat PixelFormat;

        /// <summary>
        /// Flags to indicate the complexity of the surfaces stored.
        /// See <see cref="DDSCapsFlags"/> for values.
        /// </summary>
        public uint Caps;

        /// <summary>
        /// Flags to indicate additional detail about the surfaces stored.
        /// See <see cref="DDSCapsAdditionalFlags"/> for values.
        /// </summary>
        public uint Caps2;

        /// <summary>
        /// Unused.
        /// </summary>
        public uint Caps3;

        /// <summary>
        /// Unused.
        /// </summary>
        public uint Caps4;

        /// <summary>
        /// Unused.
        /// </summary>
        public uint Reserved2;
    }
}
