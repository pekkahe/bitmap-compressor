using System;
using NUnit.Framework.Compatibility;
using NUnit.Framework;

namespace BitmapCompressor.Tests.Helpers
{
    public static class Profiler
    { 
        /// <summary>
        /// Returns a disposable object which prints the elapsed time to the NLog output when disposed.
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
    }
}
