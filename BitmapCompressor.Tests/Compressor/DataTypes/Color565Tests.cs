using System;
using BitmapCompressor.DataTypes;
using NUnit.Framework;

namespace BitmapCompressor.Tests.Compression.DataTypes
{
    [TestFixture(Category = "DataTypes")]
    public class Color565Tests
    {
        private const int MaxValue16Bit = 65535;

        [Test]
        public void ConstructionFromIntegerComponents()
        {
            var color = Color565.FromRgb(10, 51, 23);

            Assert.AreEqual(10, color.R);
            Assert.AreEqual(51, color.G);
            Assert.AreEqual(23, color.B);
        }

        [Test]
        public void ConstructionFromIntegerComponentsClampsValuesWhenOverLimit()
        {
            var expected = Color565.FromRgb(31, 63, 31);

            var color1 = Color565.FromRgb(3455, 23145, 55132);
            var color2 = Color565.FromRgb(255, 255, 255);
            var color3 = Color565.FromRgb(150, 230, 130);
            
            Assert.AreEqual(expected, color1);
            Assert.AreEqual(expected, color2);
            Assert.AreEqual(expected, color3);
            Assert.AreEqual(MaxValue16Bit, color1.Value);
            Assert.AreEqual(MaxValue16Bit, color2.Value);
            Assert.AreEqual(MaxValue16Bit, color3.Value);
        }

        [Test]
        public void ConstructionFromByteComponents()
        {
            var color = Color565.FromRgb((byte) 10, (byte) 51, (byte) 23);

            Assert.AreEqual(10, color.R);
            Assert.AreEqual(51, color.G);
            Assert.AreEqual(23, color.B);
        }

        [Test]
        public void ConstructionFromByteComponentsClampsValuesWhenOverLimit()
        {
            var expected = Color565.FromRgb((byte) 31, (byte) 63, (byte) 31);

            var color1 = Color565.FromRgb((byte) 255, (byte) 255, (byte) 255);
            var color2 = Color565.FromRgb((byte) 150, (byte) 230, (byte) 130);

            Assert.AreEqual(expected, color1);
            Assert.AreEqual(expected, color2);
            Assert.AreEqual(MaxValue16Bit, color1.Value);
            Assert.AreEqual(MaxValue16Bit, color2.Value);
        }

        [Test]
        public void ConstructionFromUnsignedInteger()
        {
            var color = Color565.FromValue(22135);

            Assert.AreEqual(10, color.R);
            Assert.AreEqual(51, color.G);
            Assert.AreEqual(23, color.B);
            Assert.AreEqual(22135, color.Value);
        }

        [Test]
        public void ValueFormatIsR5G6B5()
        {
            var color = Color565.FromRgb(10, 51, 23);

            // 10 = _0 1010 -> R5
            // 51 = 11 0011 -> G6
            // 23 = _1 0111 -> B5
            //   R5      G6     B5      
            // 0101 0|110 011|1 0111   16-bit layout
            const int expected = 22135;

            var value = color.Value;

            Assert.AreEqual(expected, value);
        }
    }
}
