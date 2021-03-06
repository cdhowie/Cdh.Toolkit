//
// SynchronizedCollection.cs
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
using System.Linq;
using System.Threading;

using Cdh.Toolkit.Extensions.ReaderWriterLockSlim;

namespace Cdh.Toolkit.Collections
{
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof(CollectionDebuggerView<>))]
    public class SynchronizedCollection<T> : ICollection<T>
    {
        public ReaderWriterLockSlim Lock { get; private set; }
        public EnumerateBehavior EnumerateBehavior { get; private set; }

        protected ICollection<T> Decorated { get; private set; }

        public SynchronizedCollection(ICollection<T> collection,
            EnumerateBehavior behavior, ReaderWriterLockSlim @lock)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            if (@lock == null)
                throw new ArgumentNullException("lock");

            Decorated = collection;
            EnumerateBehavior = behavior;
            Lock = @lock;
        }

        public SynchronizedCollection(ICollection<T> collection,
            EnumerateBehavior behavior) :
            this(collection, behavior, new ReaderWriterLockSlim())
        {
        }

        protected virtual void ValidateItem(T item) { }

        public virtual bool Add(T item)
        {
            ValidateItem(item);

            using (Lock.Write())
            {
                // This is kind of a hack, but it ensures that with e.g.
                // HashSet<T> this method will return true/false properly,
                // saving me from having to write HashSet-specific wrappers.

                int initialCount = Decorated.Count;

                Decorated.Add(item);

                return initialCount != Decorated.Count;
            }
        }

        public virtual void AddRange(IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            foreach (var item in items) { ValidateItem(item); }

            using (Lock.Write())
                foreach (var item in items)
                    Decorated.Add(item);
        }

        #region ICollection<T> Members

        void ICollection<T>.Add(T item)
        {
            Add(item);
        }

        public virtual void Clear()
        {
            using (Lock.Write())
                Decorated.Clear();
        }

        public virtual bool Contains(T item)
        {
            using (Lock.Read())
                return Decorated.Contains(item);
        }

        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            using (Lock.Read())
                Decorated.CopyTo(array, arrayIndex);
        }

        public virtual int Count
        {
            get
            {
                using (Lock.Read())
                    return Decorated.Count;
            }
        }

        public virtual bool IsReadOnly
        {
            get { return Decorated.IsReadOnly; }
        }

        public virtual bool Remove(T item)
        {
            using (Lock.Write())
                return Decorated.Remove(item);
        }

        #endregion

        #region IEnumerable<T> Members

        public virtual IEnumerator<T> GetEnumerator()
        {
            if (EnumerateBehavior == EnumerateBehavior.Copy)
                using (Lock.Read())
                    return Decorated.Where(i => true).ToList().GetEnumerator();

            return CreateGenericEnumerator();
        }

        private IEnumerator<T> CreateGenericEnumerator()
        {
            using (Lock.Read())
                foreach (T i in Decorated)
                    yield return i;
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (EnumerateBehavior == EnumerateBehavior.Copy)
                using (Lock.Read())
                    Decorated.Cast<object>().ToList().GetEnumerator();

            return CreateEnumerator();
        }

        private IEnumerator CreateEnumerator()
        {
            using (Lock.Read())
                foreach (object i in (IEnumerable)Decorated)
                    yield return i;
        }

        #endregion
    }
}
