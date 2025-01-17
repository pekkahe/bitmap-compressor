using System;
using System.Drawing;
using System.Linq;
using BitmapCompressor.Formats;
using BitmapCompressor.DataTypes;
using NUnit.Framework;

namespace BitmapCompressor.Tests.UnitTests.Compressor.DataTypes
{
    [TestFixture(Category = "DataTypes")]
    public class DirectBitmapTests
    {
        [Test]
        public void GetColorsForEachPixelInArea()
        {
            var color = Color.FromArgb(50, 100, 150);
            var block = new Point(0, 0);

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

            var bmp = DirectBitmap.CreateFromBitmap(bitmap);

            var colors = bmp.GetBlockColors(new Point(0, 0));

            Assert.That(colors.Length, Is.EqualTo(BlockFormat.TexelCount));
            Assert.That(colors.All(c => c.Equals(color)));
        }

        [Test]
        public void GetColorsWithAlphaForEachPixelInArea()
        {
            var color = Color.FromArgb(170, 50, 100, 150);
            var block = new Point(0, 0);

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

            var bmp = DirectBitmap.CreateFromBitmap(bitmap);

            var colors = bmp.GetBlockColors(new Point(0, 0));

            Assert.That(colors.Length, Is.EqualTo(BlockFormat.TexelCount));
            Assert.That(colors.All(c => c.Equals(color)));
        }

        [Test]
        public void GetColorsForEachPixelInOffsetArea()
        {
            var color = Color.FromArgb(50, 100, 150);

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

            var bmp = DirectBitmap.CreateFromBitmap(bitmap);

            var colors = bmp.GetBlockColors(new Point(1, 1));

            Assert.That(colors.Length, Is.EqualTo(BlockFormat.TexelCount));
            Assert.That(colors.All(c => c.Equals(color)));
        }

        [Test]
        public void GetDifferentColorsForEachPixelInArea()
        {
            var color1 = Color.FromArgb(50, 100, 150);
            var color2 = Color.FromArgb(150, 100, 50);
            var color3 = Color.FromArgb(70, 140, 210);
            var color4 = Color.FromArgb(20, 150, 230);

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

            var bmp = DirectBitmap.CreateFromBitmap(bitmap);

            var colors = bmp.GetBlockColors(new Point(0, 0));

            Assert.That(colors.Length, Is.EqualTo(BlockFormat.TexelCount));
            Assert.That(colors.Count(c => c.Equals(color1)), Is.EqualTo(4));
            Assert.That(colors.Count(c => c.Equals(color2)), Is.EqualTo(4));
            Assert.That(colors.Count(c => c.Equals(color3)), Is.EqualTo(4));
            Assert.That(colors.Count(c => c.Equals(color4)), Is.EqualTo(4));
        }

        [Test]
        public void SetColorsForEachPixelInArea()
        {
            var color = Color.FromArgb(50, 100, 150);
            var colors = Enumerable.Repeat(color, BlockFormat.TexelCount).ToArray();

            var bmp = new DirectBitmap(BlockFormat.Dimension, BlockFormat.Dimension);

            bmp.SetBlockColors(new Point(0, 0), colors);

            Assert.That(bmp.Bitmap.GetPixel(0, 0), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(0, 1), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(0, 2), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(0, 3), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(1, 0), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(1, 1), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(1, 2), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(1, 3), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(2, 0), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(2, 1), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(2, 2), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(2, 3), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(3, 0), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(3, 1), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(3, 2), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(3, 3), Is.EqualTo(color));
        }

        [Test]
        public void SetColorsWithAlphaForEachPixelInArea()
        {
            var color = Color.FromArgb(170, 50, 100, 150);
            var colors = Enumerable.Repeat(color, BlockFormat.TexelCount).ToArray();

            var bmp = new DirectBitmap(BlockFormat.Dimension, BlockFormat.Dimension);

            bmp.SetBlockColors(new Point(0, 0), colors);

            Assert.That(bmp.Bitmap.GetPixel(0, 0), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(0, 1), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(0, 2), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(0, 3), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(1, 0), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(1, 1), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(1, 2), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(1, 3), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(2, 0), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(2, 1), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(2, 2), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(2, 3), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(3, 0), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(3, 1), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(3, 2), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(3, 3), Is.EqualTo(color));
        }

        [Test]
        public void SetColorsForEachPixelInOffsetArea()
        {
            var color = Color.FromArgb(50, 100, 150);
            var colors = Enumerable.Repeat(color, BlockFormat.TexelCount).ToArray();

            var bmp = new DirectBitmap(2 * BlockFormat.Dimension, 2 * BlockFormat.Dimension);

            bmp.SetBlockColors(new Point(1, 1), colors);

            Assert.That(bmp.Bitmap.GetPixel(4, 4), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(4, 5), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(4, 6), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(4, 7), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(5, 4), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(5, 5), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(5, 6), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(5, 7), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(6, 4), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(6, 5), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(6, 6), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(6, 7), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(7, 4), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(7, 5), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(7, 6), Is.EqualTo(color));
            Assert.That(bmp.Bitmap.GetPixel(7, 7), Is.EqualTo(color));
        }

        [Test]
        public void SetDifferentColorsForEachPixelInArea()
        {
            var color1 = Color.FromArgb(50, 100, 150);
            var color2 = Color.FromArgb(150, 100, 50);
            var color3 = Color.FromArgb(70, 140, 210);
            var color4 = Color.FromArgb(20, 150, 230);

            var colors = new Color[BlockFormat.TexelCount];
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

            var bmp = new DirectBitmap(BlockFormat.Dimension, BlockFormat.Dimension);

            bmp.SetBlockColors(new Point(0, 0), colors);

            Assert.That(bmp.Bitmap.GetPixel(0, 0), Is.EqualTo(color1));
            Assert.That(bmp.Bitmap.GetPixel(1, 0), Is.EqualTo(color1));
            Assert.That(bmp.Bitmap.GetPixel(2, 0), Is.EqualTo(color1));
            Assert.That(bmp.Bitmap.GetPixel(3, 0), Is.EqualTo(color1));
            Assert.That(bmp.Bitmap.GetPixel(0, 1), Is.EqualTo(color2));
            Assert.That(bmp.Bitmap.GetPixel(1, 1), Is.EqualTo(color2));
            Assert.That(bmp.Bitmap.GetPixel(2, 1), Is.EqualTo(color2));
            Assert.That(bmp.Bitmap.GetPixel(3, 1), Is.EqualTo(color2));
            Assert.That(bmp.Bitmap.GetPixel(0, 2), Is.EqualTo(color3));
            Assert.That(bmp.Bitmap.GetPixel(1, 2), Is.EqualTo(color3));
            Assert.That(bmp.Bitmap.GetPixel(2, 2), Is.EqualTo(color3));
            Assert.That(bmp.Bitmap.GetPixel(3, 2), Is.EqualTo(color3));
            Assert.That(bmp.Bitmap.GetPixel(0, 3), Is.EqualTo(color4));
            Assert.That(bmp.Bitmap.GetPixel(1, 3), Is.EqualTo(color4));
            Assert.That(bmp.Bitmap.GetPixel(2, 3), Is.EqualTo(color4));
            Assert.That(bmp.Bitmap.GetPixel(3, 3), Is.EqualTo(color4));
        }
    }
}
