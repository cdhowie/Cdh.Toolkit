﻿//
// ReadOnlyCollection.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Cdh.Toolkit.Collections
{
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof(CollectionDebuggerView<>))]
    public class ReadOnlyCollection<T> : ICollection<T>, ICollection
    {
        protected ICollection<T> Decorated { get; private set; }
        protected ICollection LegacyDecorated { get; private set; }

        public ReadOnlyCollection(ICollection<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            Decorated = collection;

            LegacyDecorated = CreateLegacyCollection(collection);
            if (LegacyDecorated == null)
                throw new ArgumentException("Unable to convert collection to legacy collection type.", "collection");
        }

        protected virtual ICollection CreateLegacyCollection(ICollection<T> collection)
        {
            var legacy = collection as ICollection;
            if (legacy != null)
                return legacy;

            return new CollectionWrapper<T>(collection);
        }

        #region ICollection<T> Members

        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException();
        }

        void ICollection<T>.Clear()
        {
            throw new NotSupportedException();
        }

        public virtual bool Contains(T item)
        {
            return Decorated.Contains(item);
        }

        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            Decorated.CopyTo(array, arrayIndex);
        }

        public virtual int Count
        {
            get { return Decorated.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region IEnumerable<T> Members

        public virtual IEnumerator<T> GetEnumerator()
        {
            return Decorated.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Decorated).GetEnumerator();
        }

        #endregion

        #region ICollection Members

        void ICollection.CopyTo(Array array, int index)
        {
            LegacyDecorated.CopyTo(array, index);
        }

        bool ICollection.IsSynchronized
        {
            get { return LegacyDecorated.IsSynchronized; }
        }

        object ICollection.SyncRoot
        {
            get { return LegacyDecorated.SyncRoot; }
        }

        #endregion
    }
}
