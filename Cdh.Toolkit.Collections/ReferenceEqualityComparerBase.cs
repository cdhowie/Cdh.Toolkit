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

        #region Singleton implementation

        public static ReferenceEqualityComparerBase<T> Create(
            Func<T, T, bool> equalityFunction,
            Func<T, int> hashCodeFunction)
        {
            return new LambdaReferenceEqualityComparer(equalityFunction, hashCodeFunction);
        }

        private class LambdaReferenceEqualityComparer : ReferenceEqualityComparerBase<T>
        {
            private Func<T, T, bool> equalityFunction;
            private Func<T, int> hashCodeFunction;

            public LambdaReferenceEqualityComparer(Func<T, T, bool> equalityFunction, Func<T, int> hashCodeFunction)
            {
                if (equalityFunction == null)
                    throw new ArgumentNullException("equalityFunction");

                if (hashCodeFunction == null)
                    throw new ArgumentNullException("hashCodeFunction");

                this.equalityFunction = equalityFunction;
                this.hashCodeFunction = hashCodeFunction;
            }

            protected override bool EqualityTest(T x, T y)
            {
                return equalityFunction(x, y);
            }

            protected override int ComputeHashCode(T obj)
            {
                return hashCodeFunction(obj);
            }
        }

        #endregion
    }
}
