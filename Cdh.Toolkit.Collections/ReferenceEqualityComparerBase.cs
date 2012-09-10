using System;
using System.Collections.Generic;

namespace Cdh.Toolkit.Collections
{
    public abstract class ReferenceEqualityComparerBase<T> : IEqualityComparer<T>
        where T : class
    {
        public ReferenceEqualityComparerBase() { }

        public bool Equals(T x, T y)
        {
            if (object.ReferenceEquals(x, y)) {
                return true;
            }

            if (x == null) {
                return y == null;
            }

            if (y == null) {
                return false;
            }

            return EqualityTest(x, y);
        }

        protected abstract bool EqualityTest(T x, T y);

        public int GetHashCode(T obj)
        {
            if (obj == null) {
                throw new ArgumentNullException("obj");
            }

            return ComputeHashCode(obj);
        }

        protected abstract int ComputeHashCode(T obj);
    }
}

