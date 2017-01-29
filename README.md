# Bitmap Compressor
**Bitmap Compressor** is a Windows console application for compressing BMP image files into [BC1 (DXT1) or BC2 (DXT2/3)](http://msdn.microsoft.com/en-us/library/bb694531(v=VS.85).aspx) compressed [DDS](http://msdn.microsoft.com/en-us/library/bb943990(v=VS.85).aspx) files and vice versa.

The application is developed with Visual Studio 2015 using C# and the .NET Framework 4.6.

## Usage
You can test the application by running the compiled executable from the Windows Command Prompt:

    bitmapcompressor.console.exe (-cN | -d) [-w] bmp_filename dds_filename

Command line options:
  
`-c1` Compress the BMP file into a BC1 compressed DDS file.

`-c2` Compress the BMP file into a BC2 compressed DDS file.

`-d` Decompress the DDS file into an uncompressed BMP file.

`-w` Overwrites the target BMP or DDS file if it exists.

To open compressed DDS files you will need an application which understands the file format, e.g. the *DirectX Texture Tool* contained in the [DirectX SDK](https://www.microsoft.com/en-us/download/details.aspx?id=6812).

### Examples
Compress a BMP file named *file1.bmp* into a DDS file named *file2.dds* using BC1:

    bitmapcompressor.console -c1 file1.bmp file2.dds

Decompress a DDS file named *file2.dds* into a BMP file named *file1.bmp* overwriting the file if it exists:

    bitmapcompressor.console -d -w file1.bmp file2.dds
