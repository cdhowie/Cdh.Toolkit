using System;
using System.Threading;

namespace Cdh.Toolkit.Extensions.Threading
{
    public static class Disposable
    {
        public static void DisposeIfNotNull(this IDisposable self)
        {
            if (self != null) {
                self.Dispose();
            }
        }

        public static void DisposeAndSetNullThreadsafe<T>(ref T disposable)
            where T : class, IDisposable
        {
            var d = Interlocked.Exchange(ref disposable, null);

            DisposeIfNotNull(d);
        }
    }
}

