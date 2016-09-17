
namespace BitmapCompressor.Serialization.FileFormat
{
    /// <summary>
    /// Describes a DDS file format structure as specified by MSDN:
    /// https://msdn.microsoft.com/en-us/library/bb943991(v=vs.85).aspx.
    /// </summary>
    /// <remarks>
    /// The size and order of the fields should match exactly with the specification 
    /// as instances of this structure are written directly into the memory stream.
    /// </remarks>
    public struct DDSFile
    {
        /// <summary>
        /// The magic number specific to the DDS file format specification.
        /// </summary>
        public const uint MagicNumber = 0x20534444;

        /// <summary>
        /// Description of the data in the file.
        /// </summary>
        public DDSFileHeader Header;

        /// <summary>
        /// The main surface data.
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// The remaining surface data such as the mipmap levels, faces in a cube map
        /// and depths in a volume texture.
        /// </summary>
        public byte[] Data2;
    }
}
