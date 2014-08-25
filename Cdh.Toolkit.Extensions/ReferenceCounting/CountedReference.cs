using System;
using System.Threading;

#pragma warning disable 420

namespace Cdh.Toolkit.Extensions.ReferenceCounting
{
    public sealed class CountedReference<T> : IDisposable
        where T : class, IDisposable
    {
        private volatile T obj;

        public T Object
        {
            get {
                var o = obj;
                if (o == null) {
                    Disposed();
                }

                return o;
            }
        }

        private ReferenceCount referenceCount;

        public CountedReference(T obj)
        {
            Check.ArgumentIsNotNull(obj, "obj");

            this.obj = obj;

            referenceCount = new ReferenceCount();
        }

        private CountedReference() { }

        private void Disposed()
        {
            throw new ObjectDisposedException(GetType().FullName);
        }

        ~CountedReference()
        {
            Dispose();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            var obj = Interlocked.Exchange(ref this.obj, null);

            if (obj == null) { return; }

            if (referenceCount.RemoveReference() == 0) {
                obj.Dispose();
            }

            referenceCount = null;
        }

        public CountedReference<T> CopyReference()
        {
            var copy = TryCopyReference();
            if (copy == null) {
                Disposed();
            }

            return copy;
        }

        public CountedReference<T> TryCopyReference()
        {
            var otherObj = obj;
            var otherCount = referenceCount;

            if (otherObj == null || otherCount == null || otherCount.AddReference() == 0) {
                return null;
            }

            return new CountedReference<T>() {
                obj = otherObj,
                referenceCount = otherCount
            };
        }
    }
}

