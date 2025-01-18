using System;
using System.Drawing;
using BitmapCompressor.Lib.DataTypes;
using BitmapCompressor.Lib.Formats;
using BitmapCompressor.Lib.Utilities;
using BitmapCompressor.Tests.Helpers;
using NUnit.Framework;

namespace BitmapCompressor.Tests.UnitTests.Compressor.Formats;

[TestFixture(Category = "Formats")]
public class BC1FormatTests
{
    [Test]
    public void CompressionReturnsByteArrayOfCorrectSize()
    {
        var colors = new Color[BlockFormat.TexelCount];

        var data = new BC1Format().Compress(colors);

        Assert.That(data.Length, Is.EqualTo(BlockFormat.BC1ByteSize));
    }

    [Test]
    public void DecompressionReturnsColorArrayOfCorrectSize()
    {
        var bytes = new byte[BlockFormat.BC1ByteSize];

        var colors = new BC1Format().Decompress(bytes);

        Assert.That(colors.Length, Is.EqualTo(BlockFormat.TexelCount));
    }

    [Test]
    public void CompressionOrdersReferenceColors()
    {
        var expectedMin     = Color.FromArgb(10, 10, 10);
        var expectedMax     = Color.FromArgb(250, 250, 250);
        var expectedColor0  = ColorUtility.To16Bit(expectedMax);
        var expectedColor1  = ColorUtility.To16Bit(expectedMin);
        var colors          = ColorHelper.CreateRandomColorsBetween(expectedMin, expectedMax);

        // Place expected min and max color in test input
        colors[4]   = expectedMin;   
        colors[10]  = expectedMax;

        var data = new BC1Format().Compress(colors);

        var color0 = Color565.FromValue((ushort) ((data[1] << 8) | data[0]));
        var color1 = Color565.FromValue((ushort) ((data[3] << 8) | data[2]));

        Assert.That(color0.Value, Is.GreaterThan(color1.Value));
        Assert.That(color0.Value, Is.EqualTo(expectedColor0.Value));
        Assert.That(color1.Value, Is.EqualTo(expectedColor1.Value));
    }

    [Test]
    public void CompressionSwitchesReferenceColorOrderWhenAlpha()
    {
        var expectedMin     = Color.FromArgb(10, 10, 10);
        var expectedMax     = Color.FromArgb(250, 250, 250);
        var expectedColor0  = ColorUtility.To16Bit(expectedMin);
        var expectedColor1  = ColorUtility.To16Bit(expectedMax);
        var colors          = ColorHelper.CreateRandomColorsBetween(expectedMin, expectedMax);

        // Place expected min and max color in test input
        colors[4]   = expectedMin;
        colors[10]  = expectedMax;

        // Add an arbitrary alpha value to some color
        ColorHelper.AddAlpha(ref colors[5]);

        var data = new BC1Format().Compress(colors);

        var color0 = Color565.FromValue((ushort) ((data[1] << 8) | data[0]));
        var color1 = Color565.FromValue((ushort) ((data[3] << 8) | data[2]));

        Assert.That(color0.Value, Is.LessThanOrEqualTo(color1.Value));
        Assert.That(color0.Value, Is.EqualTo(expectedColor0.Value));
        Assert.That(color1.Value, Is.EqualTo(expectedColor1.Value));
    }

    [Test]
    public void CompressColorsWithoutAlpha()
    {
        var colors = new Color[16];
        colors[0]   = Color.FromArgb(3, 59, 101);
        colors[1]   = Color.FromArgb(131, 154, 20);
        colors[2]   = Color.FromArgb(87, 192, 127);
        colors[3]   = Color.FromArgb(203, 148, 14);
        colors[4]   = Color.FromArgb(237, 62, 69);
        colors[5]   = Color.FromArgb(173, 159, 102);
        colors[6]   = Color.FromArgb(176, 199, 73);
        colors[7]   = Color.FromArgb(118, 87, 228);
        colors[8]   = Color.FromArgb(79, 169, 87);
        colors[9]   = Color.FromArgb(179, 238, 219);
        colors[10]  = Color.FromArgb(70, 207, 12);
        colors[11]  = Color.FromArgb(81, 243, 109);
        colors[12]  = Color.FromArgb(38, 79, 189);
        colors[13]  = Color.FromArgb(249, 229, 125);
        colors[14]  = Color.FromArgb(205, 237, 57);
        colors[15]  = Color.FromArgb(190, 158, 111);

        var data = new BC1Format().Compress(colors);

        // Assert against data hard-coded from a successful test
        // run where the data was built using unit tested components.

        Assert.That(data[0], Is.EqualTo(0x7B)); // c0Low
        Assert.That(data[1], Is.EqualTo(0xB7)); // c0Hi
        Assert.That(data[2], Is.EqualTo(0xCC)); // c1Low
        Assert.That(data[3], Is.EqualTo(0x01)); // c1Hi
        Assert.That(data[4], Is.EqualTo(0xAD)); // indexes0
        Assert.That(data[5], Is.EqualTo(0xEB)); // indexes1
        Assert.That(data[6], Is.EqualTo(0x22)); // indexes2
        Assert.That(data[7], Is.EqualTo(0x83)); // indexes3
    }

    [Test]
    public void CompressColorsWithAlpha()
    {
        var colors = new Color[16];
        colors[0]   = Color.FromArgb(200, 3, 59, 101);
        colors[1]   = Color.FromArgb(200, 131, 154, 20);
        colors[2]   = Color.FromArgb(255, 87, 192, 127);
        colors[3]   = Color.FromArgb(255, 203, 148, 14);
        colors[4]   = Color.FromArgb(200, 237, 62, 69);
        colors[5]   = Color.FromArgb(200, 173, 159, 102);
        colors[6]   = Color.FromArgb(255, 176, 199, 73);
        colors[7]   = Color.FromArgb(255, 118, 87, 228);
        colors[8]   = Color.FromArgb(200, 79, 169, 87);
        colors[9]   = Color.FromArgb(200, 179, 238, 219);
        colors[10]  = Color.FromArgb(255, 70, 207, 12);
        colors[11]  = Color.FromArgb(255, 81, 243, 109);
        colors[12]  = Color.FromArgb(200, 38, 79, 189);
        colors[13]  = Color.FromArgb(200, 249, 229, 125);
        colors[14]  = Color.FromArgb(255, 205, 237, 57);
        colors[15]  = Color.FromArgb(255, 190, 158, 111);

        var data = new BC1Format().Compress(colors);

        // Assert against data hard-coded from a successful test
        // run where the data was built using unit tested components.

        Assert.That(data[0], Is.EqualTo(0xCC)); // c0Low
        Assert.That(data[1], Is.EqualTo(0x01)); // c0Hi
        Assert.That(data[2], Is.EqualTo(0x7B)); // c1Low
        Assert.That(data[3], Is.EqualTo(0xB7)); // c1Hi
        Assert.That(data[4], Is.EqualTo(0xAF)); // indexes0
        Assert.That(data[5], Is.EqualTo(0xAF)); // indexes1
        Assert.That(data[6], Is.EqualTo(0x6F)); // indexes2
        Assert.That(data[7], Is.EqualTo(0x9F)); // indexes3
    }

    [Test]
    public void DecompressDataWithoutAlpha()
    {
        var bytes = new byte[BlockFormat.BC1ByteSize];
        bytes[0] = 0x7B; // c0Low
        bytes[1] = 0xB7; // c0Hi
        bytes[2] = 0xCC; // c1Low
        bytes[3] = 0x01; // c1Hi
        bytes[4] = 0xAD; // indexes0
        bytes[5] = 0xEB; // indexes1
        bytes[6] = 0x22; // indexes2
        bytes[7] = 0x83; // indexes3

        var colors = new BC1Format().Decompress(bytes);

        // Assert against data hard-coded from a successful test
        // run where the data was built using unit tested components.

        Assert.That(colors[0], Is.EqualTo(Color.FromArgb(255, 0, 56, 99)));
        Assert.That(colors[1], Is.EqualTo(Color.FromArgb(255, 57, 117, 140)));
        Assert.That(colors[2], Is.EqualTo(Color.FromArgb(255, 123, 178, 181)));
        Assert.That(colors[3], Is.EqualTo(Color.FromArgb(255, 123, 178, 181)));
        Assert.That(colors[4], Is.EqualTo(Color.FromArgb(255, 57, 117, 140)));
        Assert.That(colors[5], Is.EqualTo(Color.FromArgb(255, 123, 178, 181)));
        Assert.That(colors[6], Is.EqualTo(Color.FromArgb(255, 123, 178, 181)));
        Assert.That(colors[7], Is.EqualTo(Color.FromArgb(255, 57, 117, 140)));
        Assert.That(colors[8], Is.EqualTo(Color.FromArgb(255, 123, 178, 181)));
        Assert.That(colors[9], Is.EqualTo(Color.FromArgb(255, 181, 239, 222)));
        Assert.That(colors[10], Is.EqualTo(Color.FromArgb(255, 123, 178, 181)));
        Assert.That(colors[11], Is.EqualTo(Color.FromArgb(255, 181, 239, 222)));
        Assert.That(colors[12], Is.EqualTo(Color.FromArgb(255, 57, 117, 140)));
        Assert.That(colors[13], Is.EqualTo(Color.FromArgb(255, 181, 239, 222)));
        Assert.That(colors[14], Is.EqualTo(Color.FromArgb(255, 181, 239, 222)));
        Assert.That(colors[15], Is.EqualTo(Color.FromArgb(255, 123, 178, 181)));
    }

    [Test]
    public void DecompressDataWithAlpha()
    {
        var bytes = new byte[BlockFormat.BC1ByteSize];
        bytes[0] = 0xCC; // c0Low
        bytes[1] = 0x01; // c0Hi
        bytes[2] = 0x7B; // c1Low
        bytes[3] = 0xB7; // c1Hi
        bytes[4] = 0xAF; // indexes0
        bytes[5] = 0xAF; // indexes1
        bytes[6] = 0x6F; // indexes2
        bytes[7] = 0x9F; // indexes3

        var colors = new BC1Format().Decompress(bytes);

        // Assert against data hard-coded from a successful test
        // run where the data was built using unit tested components.

        Assert.That(colors[0], Is.EqualTo(Color.FromArgb(0, 0, 0, 0)));
        Assert.That(colors[1], Is.EqualTo(Color.FromArgb(0, 0, 0, 0)));
        Assert.That(colors[2], Is.EqualTo(Color.FromArgb(255, 90, 150, 165)));
        Assert.That(colors[3], Is.EqualTo(Color.FromArgb(255, 90, 150, 165)));
        Assert.That(colors[4], Is.EqualTo(Color.FromArgb(0, 0, 0, 0)));
        Assert.That(colors[5], Is.EqualTo(Color.FromArgb(0, 0, 0, 0)));
        Assert.That(colors[6], Is.EqualTo(Color.FromArgb(255, 90, 150, 165)));
        Assert.That(colors[7], Is.EqualTo(Color.FromArgb(255, 90, 150, 165)));
        Assert.That(colors[8], Is.EqualTo(Color.FromArgb(0, 0, 0, 0)));
        Assert.That(colors[9], Is.EqualTo(Color.FromArgb(0, 0, 0, 0)));
        Assert.That(colors[10], Is.EqualTo(Color.FromArgb(255, 90, 150, 165)));
        Assert.That(colors[11], Is.EqualTo(Color.FromArgb(255, 181, 239, 222)));
        Assert.That(colors[12], Is.EqualTo(Color.FromArgb(0, 0, 0, 0)));
        Assert.That(colors[13], Is.EqualTo(Color.FromArgb(0, 0, 0, 0)));
        Assert.That(colors[14], Is.EqualTo(Color.FromArgb(255, 181, 239, 222)));
        Assert.That(colors[15], Is.EqualTo(Color.FromArgb(255, 90, 150, 165)));
    }
}
