using System;
using System.Threading;

namespace Cdh.Toolkit.Extensions.ReferenceCounting
{
    internal sealed class ReferenceCount
    {
        private int referenceCount = 1;

        public ReferenceCount() { }

        public int AddReference()
        {
            // Locking on this is usually discouraged but in this case the use of this class is limited (as it is
            // internal) and we know that it will never be locked on.
            lock (this) {
                if (referenceCount == 0) {
                    return 0;
                }

                return ++referenceCount;
            }
        }

        public int RemoveReference()
        {
            lock (this) {
                return --referenceCount;
            }
        }
    }
}

