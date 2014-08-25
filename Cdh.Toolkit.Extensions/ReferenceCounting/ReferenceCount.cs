using System;
using System.Threading;

#pragma warning disable 420

namespace Cdh.Toolkit.Extensions.ReferenceCounting
{
    internal sealed class ReferenceCount
    {
        private volatile int referenceCount = 1;

        public ReferenceCount() { }

        public int AddReference()
        {
            return AdjustCount(1);
        }

        public int RemoveReference()
        {
            return AdjustCount(-1);
        }

        private int AdjustCount(int delta)
        {
            for (;;) {
                var count = referenceCount;

                if (count == 0) {
                    return 0;
                }

                var newCount = checked(count + delta);

                if (Interlocked.CompareExchange(ref referenceCount, newCount, count) == count) {
                    return newCount;
                }
            }
        }
    }
}

