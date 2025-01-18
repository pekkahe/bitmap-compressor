namespace BitmapCompressor.Lib.Serialization.FileFormat;

/// <summary>
/// Flags used in <see cref="DDSFileHeader.Flags"/>.
/// </summary>
public static class DDSFileHeaderFlags
{
    /// <summary>
    /// Required in every .dds file.
    /// </summary>
    public const uint DDSD_CAPS = 0x1;

    /// <summary>
    /// Required in every .dds file.
    /// </summary>
    public const uint DDSD_HEIGHT = 0x2;

    /// <summary>
    /// Required in every .dds file.
    /// </summary>
    public const uint DDSD_WIDTH = 0x4;

    /// <summary>
    /// Required when pitch is provided for an uncompressed texture.
    /// </summary>
    public const uint DDSD_PITCH = 0x8;

    /// <summary>
    /// Required in every .dds file.
    /// </summary>
    public const uint DDSD_PIXELFORMAT = 0x1000;

    /// <summary>
    /// Required in a mipmapped texture.
    /// </summary>
    public const uint DDSD_MIPMAPCOUNT = 0x20000;

    /// <summary>
    /// Required when pitch is provided for a compressed texture.
    /// </summary>
    public const uint DDSD_LINEARSIZE = 0x80000;

    /// <summary>
    /// Required in a depth texture.
    /// </summary>
    public const uint DDSD_DEPTH = 0x800000;
}
