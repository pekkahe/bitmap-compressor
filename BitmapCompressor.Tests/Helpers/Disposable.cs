using System;

namespace BitmapCompressor.Tests.Helpers
{
    public class Disposable : IDisposable
    {
        private readonly Action _action;

        private Disposable(Action action)
        {
            _action = action;
        }

        public void Dispose()
        {
            _action();
        }

        public static IDisposable Create(Action action)
        {
            return new Disposable(action);
        }
    }
}
