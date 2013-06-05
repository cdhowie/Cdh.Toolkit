using System;
using System.Collections.Generic;
using System.Linq;
using Cdh.Toolkit.Extensions;

namespace Cdh.Toolkit.Collections
{
    public class ListEqualityComparer<T> : IEqualityComparer<IList<T>>
    {
        public static readonly ListEqualityComparer<T> Default = new ListEqualityComparer<T>();

        private IEqualityComparer<T> elementEqualityComparer;

        public ListEqualityComparer(IEqualityComparer<T> elementEqualityComparer)
        {
            if (elementEqualityComparer == null) {
                elementEqualityComparer = EqualityComparer<T>.Default;
            }

            this.elementEqualityComparer = elementEqualityComparer;
        }

        public ListEqualityComparer() : this(null) { }

        #region IEqualityComparer implementation

        public bool Equals(IList<T> x, IList<T> y)
        {
            if (x == null) {
                return y == null;
            }

            if (y == null) {
                return false;
            }

            if (x.Count != y.Count) {
                return false;
            }

            return x.SequenceEqual(y, elementEqualityComparer);
        }

        public int GetHashCode(IList<T> obj)
        {
            Check.ArgumentIsNotNull(obj, "obj");

            int hashCode = obj.Count;

            // Stagger each element's bitmask up to 16 elements.
            int shift = 0;
            foreach (var i in obj.Take(16)) {
                if (i != null) {
                    hashCode ^= elementEqualityComparer.GetHashCode(i) << shift;
                }

                shift += 1;
            }

            return hashCode;
        }

        #endregion

    }
}

