# Bitmap Compressor
**Bitmap Compressor** is a C# console application for compressing BMP image files into [DXT1 (BC1)](http://msdn.microsoft.com/en-us/library/bb694531(v=VS.85).aspx) compressed [DDS](http://msdn.microsoft.com/en-us/library/bb943990(v=VS.85).aspx) files and vice versa.

The application is developed with Visual Studio 2015 using .NET Framework 4.6.

## Usage
You can test the application by running the compiled executable from the Windows Command Prompt:

    bitmapcompressor.console.exe (-c | -d) [-w] bmp_filename dds_filename

Command line options:
  
`-c` Compress the BMP file into a DXT1 (BC1) compressed DDS file.

`-d` Decompress the DDS file into an uncompressed BMP file.

`-w` Overwrites the target BMP or DDS file if it exists.

### Examples
Compress a BMP file named *file1.bmp* into a DDS file named *file2.dds*:

    bitmapcompressor.console -c file1.bmp file2.dds

Decompress a DDS file named *file2.dds* into a BMP file named *file1.bmp* overwriting the file if it exists:

    bitmapcompressor.console -d -w file1.bmp file2.dds
