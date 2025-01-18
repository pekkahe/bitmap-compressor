namespace BitmapCompressor.Lib.Diagnostics;

public class Logger
{
    private static ILogger _instance;

    public static ILogger Default => _instance ?? (_instance = new ConsoleLogger());

    private Logger()
    { }

    public static void OverrideDefault(ILogger logger)
    {
        _instance = logger;
    }
}
