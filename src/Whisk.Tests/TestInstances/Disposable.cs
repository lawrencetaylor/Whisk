using System;

namespace Whisk.Tests
{
    internal class Disposable : IDisposable
    {
        public void Dispose()
        {
            IsDisposed = true;
        }

        public bool IsDisposed { get; private set; }
    }
}