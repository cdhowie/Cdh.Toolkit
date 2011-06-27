//
// CollectionWrapper.cs
//
// Author:
//       Chris Howie <me@chrishowie.com>
//
// Copyright (c) 2010 Chris Howie
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Cdh.Toolkit.Collections
{
    internal class CollectionWrapper<T> : ICollection<T>, ICollection
    {
        private ICollection<T> wrapped;

        public CollectionWrapper(ICollection<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            wrapped = collection;
        }

        #region ICollection<T> Members

        public void Add(T item)
        {
            wrapped.Add(item);
        }

        public void Clear()
        {
            wrapped.Clear();
        }

        public bool Contains(T item)
        {
            return wrapped.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            wrapped.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return wrapped.Count; }
        }

        public bool IsReadOnly
        {
            get { return wrapped.IsReadOnly; }
        }

        public bool Remove(T item)
        {
            return wrapped.Remove(item);
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return wrapped.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region ICollection Members

        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            if (index < 0)
                throw new ArgumentOutOfRangeException("index", index, "Must be >= 0.");

            if (array.Length - index > wrapped.Count)
                throw new ArgumentOutOfRangeException("index", index, "The destination array is too small to copy into at the index provided.");

            var elementType = array.GetType().GetElementType();
            if (!elementType.IsAssignableFrom(typeof(T)))
                throw new ArgumentException("The array's element type is not compatible with this collection's element type.", "array");

            foreach (var thing in wrapped)
                array.SetValue(thing, index++);
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get { return wrapped; }
        }

        #endregion
    }
}
