using System;
using System.Threading;

namespace Cdh.Toolkit.Extensions.ReferenceCounting
{
    public sealed class CountedReference<T> : IDisposable
        where T : class, IDisposable
    {
        private T obj;

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

        private CountedReference(CountedReference<T> other)
        {
            Check.ArgumentIsNotNull(other, "other");

            var otherObj = other.obj;
            var otherCount = other.referenceCount;

            if (otherObj == null || otherCount == null || otherCount.AddReference() == 0) {
                Disposed();
            }

            obj = otherObj;
            referenceCount = otherCount;
        }

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
            return new CountedReference<T>(this);
        }

        public CountedReference<T> MaybeCopyReference()
        {
            try {
                return new CountedReference<T>(this);
            } catch (ObjectDisposedException) {
                return null;
            }
        }
    }
}

