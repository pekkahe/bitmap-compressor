namespace BitmapCompressor.Lib.Serialization.FileFormat;

/// <summary>
/// The surface pixel format structure used part of the <see cref="DDSFileHeader"/>.
/// </summary>
public struct DDSPixelFormat
{
    /// <summary>
    /// The structure size. Must be set to 32 (bytes).
    /// </summary>
    public uint Size;

    /// <summary>
    /// Flags to indicate what type of data is in the surface.
    /// See <see cref="DDSPixelFormatFlags"/> for values.
    /// </summary>
    public uint Flags;

    /// <summary>
    /// Four-character codes for specifying compressed or custom formats. 
    /// </summary>
    /// <remarks>
    /// When using a four-character code, <see cref="Flags"/> must include
    /// <see cref="DDSPixelFormatFlags.DDPF_FOURCC"/>.
    /// </remarks>
    public uint FourCC;

    /// <summary>
    /// Number of bits in an RGB (possibly including alpha) format. 
    /// </summary>
    /// <remarks>
    /// Valid when <see cref="Flags"/> includes 
    /// <see cref="DDSPixelFormatFlags.DDPF_RGB"/>,
    /// <see cref="DDSPixelFormatFlags.DDPF_LUMINANCE"/>, or
    /// <see cref="DDSPixelFormatFlags.DDPF_YUV"/>.
    /// </remarks>
    public uint RGBBitCount;

    /// <summary>
    /// Red (or luminance or Y) mask for reading color data. For instance, given the A8R8G8B8
    /// format, the red mask would be 0x00ff0000.
    /// </summary>
    public uint RBitMask;

    /// <summary>
    /// Green (or U) mask for reading color data. For instance, given the A8R8G8B8 format,
    /// the green mask would be 0x0000ff00.
    /// </summary>
    public uint GBitMask;

    /// <summary>
    /// Blue (or V) mask for reading color data. For instance, given the A8R8G8B8 format,
    /// the blue mask would be 0x000000ff.
    /// </summary>
    public uint BBitMask;

    /// <summary>
    /// Alpha mask for reading alpha data. <see cref="Flags"/> must include 
    /// <see cref="DDSPixelFormatFlags.DDPF_ALPHAPIXELS"/> or <see cref="DDSPixelFormatFlags.DDPF_ALPHA"/>
    /// For instance, given the A8R8G8B8 format, the alpha mask would be 0xff000000.
    /// </summary>
    public uint ABitMask;
}
