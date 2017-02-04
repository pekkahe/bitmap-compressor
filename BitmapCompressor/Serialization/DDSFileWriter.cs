using System;
using System.IO;
using System.Runtime.InteropServices;
using BitmapCompressor.DataTypes;
using BitmapCompressor.Serialization.FileFormat;

namespace BitmapCompressor.Serialization
{
    public class DDSFileWriter : IDisposable
    {
        private readonly BinaryWriter _binaryWriter;

        public DDSFileWriter(Stream stream)
        {
            _binaryWriter = new BinaryWriter(stream);
        }

        public DDSFileWriter(string fileName) : this(new FileStream(fileName, FileMode.Create))
        { }

        /// <summary>
        /// Writes the main image data stored in the specified image into a DDS file.
        /// </summary>
        public unsafe void Write(ICompressedImage image)
        {
            WriteMagicNumber();

            var header = CreateHeader(image.Width, image.Height, 
                image.CompressionFormat.FourCC.Value);

            WriteHeader(header);

            WriteSurfaceData(image.GetBuffer());
        }

        private void WriteMagicNumber()
        {
            _binaryWriter.Write(DDSFile.MagicNumber);
        }
        
        /// <summary>
        /// Creates a DDS file header data structure with a single RGB surface.
        /// </summary>
        /// <param name="pixelWidth">The width of the DDS image described by this header.</param>
        /// <param name="pixelHeight">The height of the DDS image described by this header.</param>
        /// <param name="fourCC">The four-character code which specifies the BCn compression format.</param>
        public static DDSFileHeader CreateHeader(int pixelWidth, int pixelHeight, uint fourCC)
        {
            if (pixelHeight < 0 || pixelWidth < 0)
                throw new ArgumentException("Received negative image dimension.");
            
            var pixelFormat = new DDSPixelFormat();
            pixelFormat.Size = 32;
            pixelFormat.Flags = DDSPixelFormatFlags.DDPF_FOURCC;
            pixelFormat.FourCC = fourCC;

            var header = new DDSFileHeader();
            header.Size = 124;
            header.Flags = MinimumRequiredFlags();
            header.Height = (uint) pixelHeight;
            header.Width = (uint) pixelWidth;
            header.PixelFormat = pixelFormat;
            header.Caps = DDSCapsFlags.DDSCAPS_TEXTURE;

            return header;
        }

        /// <summary>
        /// Generates a FourCC code from four character integer values.
        /// </summary>
        private static uint MakeFourCC(int char0, int char1, int char2, int char3)
        {
            return (uint) ((byte) (char0) |
                           (byte) (char1) << 8 |
                           (byte) (char2) << 16 |
                           (byte) (char3) << 24);
        }

        private static uint MinimumRequiredFlags()
        {
            return DDSFileHeaderFlags.DDSD_CAPS | DDSFileHeaderFlags.DDSD_HEIGHT |
                   DDSFileHeaderFlags.DDSD_WIDTH | DDSFileHeaderFlags.DDSD_PIXELFORMAT;
        }

        private void WriteHeader(DDSFileHeader header)
        {
            var headerSize = Marshal.SizeOf(typeof(DDSFileHeader));
            var headerBuffer = new byte[headerSize];

            // Write the header data directly from the DDS file header structure
            GCHandle handle = GCHandle.Alloc(headerBuffer, GCHandleType.Pinned);
            Marshal.StructureToPtr(header, handle.AddrOfPinnedObject(), true);

            _binaryWriter.Write(headerBuffer);

            handle.Free();
        }
        
        private void WriteSurfaceData(byte[] surfaceData)
        {
            _binaryWriter.Write(surfaceData);
        }

        public void Dispose()
        {
            _binaryWriter.Close();
        }
    }
}
