namespace BitmapCompressor.Lib.Diagnostics;

public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine(message);
    }
}
