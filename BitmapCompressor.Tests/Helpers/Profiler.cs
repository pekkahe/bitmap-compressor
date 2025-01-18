using System;
using System.Diagnostics;
using NUnit.Framework;

namespace BitmapCompressor.Tests.Helpers;

public static class Profiler
{ 
    /// <summary>
    /// Returns a disposable object which prints the elapsed milliseconds to the NLog output when disposed.
    /// </summary>
    public static IDisposable MeasureTime()
    {
        var stopwatch = new Stopwatch();

        TestContext.WriteLine($"Profiling started.");
        stopwatch.Start();

        return Disposable.Create(() =>
        {
            stopwatch.Stop();
            TestContext.WriteLine($"Profiling finished. Elapsed {stopwatch.ElapsedMilliseconds} ms.");
        });
    }

    /// <summary>
    /// Returns a disposable object which prints the elapsed ticks to the NLog output when disposed.
    /// </summary>
    public static IDisposable MeasureTicks()
    {
        var stopwatch = new Stopwatch();

        TestContext.WriteLine($"Profiling started.");
        stopwatch.Start();

        return Disposable.Create(() =>
        {
            stopwatch.Stop();
            TestContext.WriteLine($"Profiling finished. Elapsed {stopwatch.ElapsedTicks} ticks.");
        });
    }
}
