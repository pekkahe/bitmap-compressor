using System;
using BitmapCompressor.DataTypes;
using BitmapCompressor.Formats;
using BitmapCompressor.Tests.Helpers;
using NUnit.Framework;

namespace BitmapCompressor.Tests.IntegrationTests
{
    public class BaseIntegrationTests
    {
        public static ICompressedImage Compress(IUncompressedImage bitmap, IBlockCompressionFormat format)
        {
            using (Profiler.MeasureTime())
            {
                return new BlockCompressor().Compress(bitmap, format);
            }
        }

        public static IUncompressedImage Decompress(ICompressedImage dds)
        {
            using (Profiler.MeasureTime())
            {
                return new BlockCompressor().Decompress(dds);
            }
        }

        public static IUncompressedImage LoadResourceBMP(string fileName)
        {
            return DirectBitmap.CreateFromFile(TestResourceDirectory.GetFilePath(fileName));
        }

        public static ICompressedImage LoadResourceDDS(string fileName)
        {
            return DDSImage.CreateFromFile(TestResourceDirectory.GetFilePath(fileName));
        }

        public static void AssertEqual(IImage expected, IImage actual)
        {
            Assert.That(actual.Width, Is.EqualTo(expected.Width));
            Assert.That(actual.Height, Is.EqualTo(expected.Height));
            Assert.That(actual.GetBuffer(), Is.EquivalentTo(expected.GetBuffer()));
        }

        /// <summary>
        /// Contains file names for test images located in the tests resource directory.
        /// </summary>
        public static class Images
        {
            /// <summary>
            /// Original source bitmap image.
            /// </summary>
            public const string CityscapeBmp = "cityscape.bmp";

            /// <summary>
            /// DDS file compressed from <see cref="CityscapeBmp"/> using BC1.
            /// </summary>
            public const string CityscapeDdsBc1 = "cityscape.bc1.dds";

            /// <summary>
            /// BMP file decompressed from <see cref="CityscapeDdsBc1"/>.
            /// </summary>
            public const string CityscapeBmpBc1 = "cityscape.bc1.bmp";

            /// <summary>
            /// DDS file compressed from <see cref="CityscapeBmp"/> using BC2.
            /// </summary>
            public const string CityscapeDdsBc2 = "cityscape.bc2.dds";

            /// <summary>
            /// BMP file decompressed from <see cref="CityscapeDdsBc2"/>.
            /// </summary>
            public const string CityscapeBmpBc2 = "cityscape.bc2.bmp";

            /// <summary>
            /// DDS file compressed from <see cref="CityscapeBmp"/> using BC3.
            /// </summary>
            public const string CityscapeDdsBc3 = "cityscape.bc3.dds";

            /// <summary>
            /// BMP file decompressed from <see cref="CityscapeDdsBc3"/>.
            /// </summary>
            public const string CityscapeBmpBc3 = "cityscape.bc3.bmp";

            /// <summary>
            /// Original source bitmap image with transparency.
            /// </summary>
            public const string CityscapeAlphaBmp = "cityscape-alpha.bmp";

            /// <summary>
            /// DDS file compressed from <see cref="CityscapeAlphaBmp"/> using BC1.
            /// </summary>
            public const string CityscapeAlphaDdsBc1 = "cityscape-alpha.bc1.dds";

            /// <summary>
            /// BMP file decompressed from <see cref="CityscapeAlphaDdsBc1"/>.
            /// </summary>
            public const string CityscapeAlphaBmpBc1 = "cityscape-alpha.bc1.bmp";

            /// <summary>
            /// DDS file compressed from <see cref="CityscapeAlphaBmp"/> using BC2.
            /// </summary>
            public const string CityscapeAlphaDdsBc2 = "cityscape-alpha.bc2.dds";

            /// <summary>
            /// BMP file decompressed from <see cref="CityscapeAlphaDdsBc2"/>.
            /// </summary>
            public const string CityscapeAlphaBmpBc2 = "cityscape-alpha.bc2.bmp";

            /// <summary>
            /// DDS file compressed from <see cref="CityscapeAlphaBmp"/> using BC3.
            /// </summary>
            public const string CityscapeAlphaDdsBc3 = "cityscape-alpha.bc3.dds";

            /// <summary>
            /// BMP file decompressed from <see cref="CityscapeAlphaDdsBc3"/>.
            /// </summary>
            public const string CityscapeAlphaBmpBc3 = "cityscape-alpha.bc3.bmp";

            /// <summary>
            /// Original source bitmap image with transparency.
            /// </summary>
            public const string MarsAlphaBmp = "mars-alpha.bmp";

            /// <summary>
            /// DDS file compressed from <see cref="MarsAlphaBmp"/> using BC1.
            /// </summary>
            public const string MarsAlphaDdsBc1 = "mars-alpha.bc1.dds";

            /// <summary>
            /// BMP file decompressed from <see cref="MarsAlphaDdsBc1"/>.
            /// </summary>
            public const string MarsAlphaBmpBc1 = "mars-alpha.bc1.bmp";

            /// <summary>
            /// DDS file compressed from <see cref="MarsAlphaBmp"/> using BC2.
            /// </summary>
            public const string MarsAlphaDdsBc2 = "mars-alpha.bc2.dds";

            /// <summary>
            /// BMP file decompressed from <see cref="MarsAlphaDdsBc2"/>.
            /// </summary>
            public const string MarsAlphaBmpBc2 = "mars-alpha.bc2.bmp";

            /// <summary>
            /// DDS file compressed from <see cref="MarsAlphaBmp"/> using BC3.
            /// </summary>
            public const string MarsAlphaDdsBc3 = "mars-alpha.bc3.dds";

            /// <summary>
            /// BMP file decompressed from <see cref="MarsAlphaDdsBc3"/>.
            /// </summary>
            public const string MarsAlphaBmpBc3 = "mars-alpha.bc3.bmp";
        }
    }
}
