using System.Drawing;
using System.Linq;
using BitmapCompressor.DataTypes;
using BitmapCompressor.Formats;
using NUnit.Framework;

namespace BitmapCompressor.Tests.UnitTests.Compression.DataTypes
{
    [TestFixture(Category = "DataTypes")]
    public class BMPImageTests
    {
        [Test]
        public void GettingColorsReturnsEachPixelInBlockWhenColorsAreSame()
        {
            var color = Color.FromArgb(50, 50, 50);

            var bitmap = new Bitmap(BlockFormat.Dimension, BlockFormat.Dimension);
            bitmap.SetPixel(0, 0, color);
            bitmap.SetPixel(0, 1, color);
            bitmap.SetPixel(0, 2, color);
            bitmap.SetPixel(0, 3, color);
            bitmap.SetPixel(1, 0, color);
            bitmap.SetPixel(1, 1, color);
            bitmap.SetPixel(1, 2, color);
            bitmap.SetPixel(1, 3, color);
            bitmap.SetPixel(2, 0, color);
            bitmap.SetPixel(2, 1, color);
            bitmap.SetPixel(2, 2, color);
            bitmap.SetPixel(2, 3, color);
            bitmap.SetPixel(3, 0, color);
            bitmap.SetPixel(3, 1, color);
            bitmap.SetPixel(3, 2, color);
            bitmap.SetPixel(3, 3, color);

            var bmp = new BMPImage(bitmap);

            var colors = bmp.GetColors(new Point(0, 0));

            Assert.AreEqual(BlockFormat.PixelCount, colors.Length);
            Assert.IsTrue(colors.All(c => c.Equals(color)));
        }
                    
        [Test]
        public void GettingColorsReturnsEachPixelInBlockWhenOffset()
        {
            var color = Color.FromArgb(50, 50, 50);

            var bitmap = new Bitmap(2 * BlockFormat.Dimension, 2 * BlockFormat.Dimension);
            bitmap.SetPixel(4, 4, color);
            bitmap.SetPixel(4, 5, color);
            bitmap.SetPixel(4, 6, color);
            bitmap.SetPixel(4, 7, color);
            bitmap.SetPixel(5, 4, color);
            bitmap.SetPixel(5, 5, color);
            bitmap.SetPixel(5, 6, color);
            bitmap.SetPixel(5, 7, color);
            bitmap.SetPixel(6, 4, color);
            bitmap.SetPixel(6, 5, color);
            bitmap.SetPixel(6, 6, color);
            bitmap.SetPixel(6, 7, color);
            bitmap.SetPixel(7, 4, color);
            bitmap.SetPixel(7, 5, color);
            bitmap.SetPixel(7, 6, color);
            bitmap.SetPixel(7, 7, color);

            var bmp = new BMPImage(bitmap);

            var colors = bmp.GetColors(new Point(1, 1));

            Assert.AreEqual(BlockFormat.PixelCount, colors.Length);
            Assert.IsTrue(colors.All(c => c.Equals(color)));
        }

        [Test]
        public void GettingColorsReturnsEachPixelInBlockWhenColorsAreDifferent()
        {
            var color1 = Color.FromArgb(50, 50, 50);
            var color2 = Color.FromArgb(100, 100, 100);
            var color3 = Color.FromArgb(150, 150, 150);
            var color4 = Color.FromArgb(200, 200, 200);

            var bitmap = new Bitmap(BlockFormat.Dimension, BlockFormat.Dimension);
            bitmap.SetPixel(0, 0, color1);
            bitmap.SetPixel(0, 1, color1);
            bitmap.SetPixel(0, 2, color1);
            bitmap.SetPixel(0, 3, color1);
            bitmap.SetPixel(1, 0, color2);
            bitmap.SetPixel(1, 1, color2);
            bitmap.SetPixel(1, 2, color2);
            bitmap.SetPixel(1, 3, color2);
            bitmap.SetPixel(2, 0, color3);
            bitmap.SetPixel(2, 1, color3);
            bitmap.SetPixel(2, 2, color3);
            bitmap.SetPixel(2, 3, color3);
            bitmap.SetPixel(3, 0, color4);
            bitmap.SetPixel(3, 1, color4);
            bitmap.SetPixel(3, 2, color4);
            bitmap.SetPixel(3, 3, color4);

            var bmp = new BMPImage(bitmap);

            var colors = bmp.GetColors(new Point(0, 0));

            Assert.AreEqual(BlockFormat.PixelCount, colors.Length);
            Assert.AreEqual(4, colors.Count(c => c.Equals(color1)));
            Assert.AreEqual(4, colors.Count(c => c.Equals(color2)));
            Assert.AreEqual(4, colors.Count(c => c.Equals(color3)));
            Assert.AreEqual(4, colors.Count(c => c.Equals(color4)));
        }

        [Test]
        public void SettingColorsSetsEachPixelInBlockWhenColorsAreSame()
        {
            var color = Color.FromArgb(50, 50, 50);
            var colors = Enumerable.Repeat(color, BlockFormat.PixelCount).ToArray();

            var bitmap = new Bitmap(BlockFormat.Dimension, BlockFormat.Dimension);
            var bmp = new BMPImage(bitmap);

            bmp.SetColors(colors, new Point(0, 0));

            Assert.AreEqual(color, bitmap.GetPixel(0, 0));
            Assert.AreEqual(color, bitmap.GetPixel(0, 1));
            Assert.AreEqual(color, bitmap.GetPixel(0, 2));
            Assert.AreEqual(color, bitmap.GetPixel(0, 3));
            Assert.AreEqual(color, bitmap.GetPixel(1, 0));
            Assert.AreEqual(color, bitmap.GetPixel(1, 1));
            Assert.AreEqual(color, bitmap.GetPixel(1, 2));
            Assert.AreEqual(color, bitmap.GetPixel(1, 3));
            Assert.AreEqual(color, bitmap.GetPixel(2, 0));
            Assert.AreEqual(color, bitmap.GetPixel(2, 1));
            Assert.AreEqual(color, bitmap.GetPixel(2, 2));
            Assert.AreEqual(color, bitmap.GetPixel(2, 3));
            Assert.AreEqual(color, bitmap.GetPixel(3, 0));
            Assert.AreEqual(color, bitmap.GetPixel(3, 1));
            Assert.AreEqual(color, bitmap.GetPixel(3, 2));
            Assert.AreEqual(color, bitmap.GetPixel(3, 3));
        }

        [Test]
        public void SettingColorsSetsEachPixelInBlockWhenOffset()
        {
            var color = Color.FromArgb(50, 50, 50);
            var colors = Enumerable.Repeat(color, BlockFormat.PixelCount).ToArray();

            var bitmap = new Bitmap(2 * BlockFormat.Dimension, 2 * BlockFormat.Dimension);
            var bmp = new BMPImage(bitmap);

            bmp.SetColors(colors, new Point(1, 1));

            Assert.AreEqual(color, bitmap.GetPixel(4, 4));
            Assert.AreEqual(color, bitmap.GetPixel(4, 5));
            Assert.AreEqual(color, bitmap.GetPixel(4, 6));
            Assert.AreEqual(color, bitmap.GetPixel(4, 7));
            Assert.AreEqual(color, bitmap.GetPixel(5, 4));
            Assert.AreEqual(color, bitmap.GetPixel(5, 5));
            Assert.AreEqual(color, bitmap.GetPixel(5, 6));
            Assert.AreEqual(color, bitmap.GetPixel(5, 7));
            Assert.AreEqual(color, bitmap.GetPixel(6, 4));
            Assert.AreEqual(color, bitmap.GetPixel(6, 5));
            Assert.AreEqual(color, bitmap.GetPixel(6, 6));
            Assert.AreEqual(color, bitmap.GetPixel(6, 7));
            Assert.AreEqual(color, bitmap.GetPixel(7, 4));
            Assert.AreEqual(color, bitmap.GetPixel(7, 5));
            Assert.AreEqual(color, bitmap.GetPixel(7, 6));
            Assert.AreEqual(color, bitmap.GetPixel(7, 7));
        }

        [Test]
        public void SettingColorsSetsEachPixelInBlockWhenColorsAreDifferent()
        {
            var color1 = Color.FromArgb(50, 50, 50);
            var color2 = Color.FromArgb(100, 100, 100);
            var color3 = Color.FromArgb(150, 150, 150);
            var color4 = Color.FromArgb(200, 200, 200);
            var colors = new Color[BlockFormat.PixelCount];
            colors[0] = color1;
            colors[1] = color1;
            colors[2] = color1;
            colors[3] = color1;
            colors[4] = color2;
            colors[5] = color2;
            colors[6] = color2;
            colors[7] = color2;
            colors[8] = color3;
            colors[9] = color3;
            colors[10] = color3;
            colors[11] = color3;
            colors[12] = color4;
            colors[13] = color4;
            colors[14] = color4;
            colors[15] = color4;

            var bitmap = new Bitmap(BlockFormat.Dimension, BlockFormat.Dimension);
            var bmp = new BMPImage(bitmap);

            bmp.SetColors(colors, new Point(0, 0));

            Assert.AreEqual(color1, bitmap.GetPixel(0, 0));
            Assert.AreEqual(color1, bitmap.GetPixel(1, 0));
            Assert.AreEqual(color1, bitmap.GetPixel(2, 0));
            Assert.AreEqual(color1, bitmap.GetPixel(3, 0));
            Assert.AreEqual(color2, bitmap.GetPixel(0, 1));
            Assert.AreEqual(color2, bitmap.GetPixel(1, 1));
            Assert.AreEqual(color2, bitmap.GetPixel(2, 1));
            Assert.AreEqual(color2, bitmap.GetPixel(3, 1));
            Assert.AreEqual(color3, bitmap.GetPixel(0, 2));
            Assert.AreEqual(color3, bitmap.GetPixel(1, 2));
            Assert.AreEqual(color3, bitmap.GetPixel(2, 2));
            Assert.AreEqual(color3, bitmap.GetPixel(3, 2));
            Assert.AreEqual(color4, bitmap.GetPixel(0, 3));
            Assert.AreEqual(color4, bitmap.GetPixel(1, 3));
            Assert.AreEqual(color4, bitmap.GetPixel(2, 3));
            Assert.AreEqual(color4, bitmap.GetPixel(3, 3));
        }
    }
}
