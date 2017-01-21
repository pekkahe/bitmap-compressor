//using System;
//using System.Drawing;
//using System.Collections.Generic;
//using BitmapCompressor.Formats;
//using BitmapCompressor.DataTypes;
//using BitmapCompressor.Utilities;
//using BitmapCompressor.Tests.Helpers;
//using NUnit.Framework;

//namespace BitmapCompressor.Tests.UnitTests.Compressor.Formats
//{
//    [TestFixture(Category = "Formats")]
//    public class BC1ColorTableTests
//    {
//        [Test]
//        public void GettingIndexForColorReturnsIndexOfClosestColorWhenNoAlpha()
//        {
//            var colors = ColorHelper.CreateRandomColors();
//            var expectedIndex = new int[BlockFormat.PixelCount];

//            // Pick four random colors for the color table
//            var colorTableRaw = new Color565[4];
//            colorTableRaw[0] = ColorUtility.To16Bit(colors[4]);
//            colorTableRaw[1] = ColorUtility.To16Bit(colors[7]);
//            colorTableRaw[2] = ColorUtility.To16Bit(colors[12]);
//            colorTableRaw[3] = ColorUtility.To16Bit(colors[15]);

//            for (int i = 0; i < BlockFormat.PixelCount; ++i)
//            {
//                var closest = ColorUtility.GetClosest(colorTableRaw, ColorUtility.To16Bit(colors[i]));
//                expectedIndex[i] = Array.IndexOf(colorTableRaw, closest);
//            }

//            var colorTable = new BC1ColorTableFactory(colorTableRaw);

//            for (int i = 0; i < BlockFormat.PixelCount; ++i)
//            {
//                var index = colorTable.IndexFor(colors[i]);

//                Assert.AreEqual(expectedIndex[i], index);
//            }
//        }

//        [Test]
//        public void GettingIndexForColorReturnsIndexOfColor3WhenAlpha()
//        {
//            const int expectedIndex = 3;

//            var table = BC1ColorTableFactory.FromArgb(ColorHelper.CreateRandomColors());
//            var colorWithAlpha = Color.FromArgb(100, 255, 255, 255);

//            int index = table.IndexFor(colorWithAlpha);

//            Assert.AreEqual(expectedIndex, index);
//        }

//        [Test]
//        public void GettingColorForIndexReturnsColorFromTableWhenNoAlpha()
//        {
//            var expectedColors = new Color565[4];
//            expectedColors[0] = Color565.FromRgb(100, 100, 100);
//            expectedColors[1] = Color565.FromRgb(50, 50, 50);

//            var colorTable = new BC1ColorTableFactory(expectedColors[0], expectedColors[1]);
//            expectedColors[2] = colorTable[2];
//            expectedColors[3] = colorTable[3];

//            var pixelToIndex = new Dictionary<int, int>();
//            var pixelToColor = new Dictionary<int, Color>();

//            for (int i = 0; i < BlockFormat.PixelCount; ++i)
//            {
//                int index = i % 4;
//                var expectedColor = expectedColors[index];

//                pixelToIndex[i] = index;
//                pixelToColor[i] = ColorUtility.To32Bit(expectedColor);
//            }

//            for (int i = 0; i < BlockFormat.PixelCount; ++i)
//            {
//                var color = colorTable.ColorFor(pixelToIndex[i]);

//                Assert.AreEqual(pixelToColor[i], color);
//            }
//        }

//        [Test]
//        public void GettingColorForIndexReturnsTransparentColorWhenAlpha()
//        {
//            var expectedColors = new Color565[4];
//            expectedColors[0] = Color565.FromRgb(50, 50, 50);
//            expectedColors[1] = Color565.FromRgb(100, 100, 100);

//            var colorTable = new BC1ColorTableFactory(expectedColors[0], expectedColors[1]);
//            expectedColors[2] = colorTable[2];
//            expectedColors[3] = colorTable[3];

//            var pixelToIndex = new Dictionary<int, int>();
//            var pixelToColor = new Dictionary<int, Color>();

//            for (int i = 0; i < BlockFormat.PixelCount; ++i)
//            {
//                int index = i % 4;
//                var expectedColor = expectedColors[index];

//                pixelToIndex[i] = index;
//                pixelToColor[i] = ColorUtility.To32Bit(expectedColor);

//                if (pixelToIndex[i] == 3)
//                {
//                    pixelToColor[i] = Color.FromArgb(0, pixelToColor[i]);
//                }
//            }

//            for (int i = 0; i < BlockFormat.PixelCount; ++i)
//            {
//                var color = colorTable.ColorFor(pixelToIndex[i]);

//                Assert.AreEqual(pixelToColor[i], color);
//            }
//        }
//    }
//}
