# Bitmap Compressor
**Bitmap Compressor** is a Windows console application for compressing BMP image files into [block compressed](http://msdn.microsoft.com/en-us/library/bb694531(v=VS.85).aspx) [DDS](http://msdn.microsoft.com/en-us/library/bb943990(v=VS.85).aspx) files and vice versa. 

Currently BC1, BC2, and BC3 compression formats are implemented.

The application is developed with Visual Studio 2015 using C# and the .NET Framework 4.6.

## Usage
You can use NuGet Package Manager to download the library dependencies when you open the solution in Visual Studio.

After building the solution you can test the application by running the console project's executable from Command Prompt:

    bitmapcompressor.console.exe (-cN | -d) [-w] bmp_filename dds_filename

Command line options:
  
`-c1` Compress the BMP file into a BC1 compressed DDS file.

`-c2` Compress the BMP file into a BC2 compressed DDS file.

`-c3` Compress the BMP file into a BC3 compressed DDS file.

`-d` Decompress the DDS file into an uncompressed BMP file.

`-w` Overwrites the target BMP or DDS file if it exists.

To open compressed DDS files you will need an application which understands the file format, for example the *DirectX Texture Tool* which comes with the [DirectX SDK](https://www.microsoft.com/en-us/download/details.aspx?id=6812).
