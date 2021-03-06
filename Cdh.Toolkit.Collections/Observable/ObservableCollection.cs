﻿//
// ObservableCollection.cs
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
using System.Threading;

using Cdh.Toolkit.Extensions.Events;
using Cdh.Toolkit.Extensions.ReaderWriterLockSlim;
using System.Diagnostics;

namespace Cdh.Toolkit.Collections.Observable
{
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof(CollectionDebuggerView<>))]
    public class ObservableCollection<T> : SynchronizedCollection<T>, IObservableCollection<T>
    {
        public ObservableCollection(ICollection<T> collection, EnumerateBehavior behavior, ReaderWriterLockSlim @lock)
            : base(collection, behavior, @lock) { }

        public ObservableCollection(ICollection<T> collection, EnumerateBehavior behavior)
            : base(collection, behavior) { }

        protected virtual void OnAdded(T item)
        {
            var handler = Changed;
            if (handler != null)
                handler(this, new ObservableCollectionChangedEventArgs<T>(item, ObservableChangeType.Add));
        }

        protected virtual void OnRemoved(T item)
        {
            var handler = Changed;
            if (handler != null)
                handler(this, new ObservableCollectionChangedEventArgs<T>(item, ObservableChangeType.Remove));
        }

        public override bool Add(T item)
        {
            using (Lock.Write())
            {
                int originalCount = Decorated.Count;

                Decorated.Add(item);

                if (originalCount != Decorated.Count)
                {
                    OnAdded(item);
                    return true;
                }

                return false;
            }
        }

        public override void AddRange(IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            using (Lock.Write())
                foreach (var item in items)
                    Add(item);
        }

        public override void Clear()
        {
            using (Lock.Write())
            {
                var handler = Changed;
                if (handler != null)
                {
                    var args = Decorated.Select(i => new ObservableCollectionChangedEventArgs<T>(i, ObservableChangeType.Remove)).ToList();

                    Decorated.Clear();

                    foreach (var arg in args)
                        handler(this, arg);
                }
                else
                {
                    Decorated.Clear();
                }
            }
        }

        public override bool Remove(T item)
        {
            using (Lock.Write())
            {
                bool success;

                if (success = Decorated.Remove(item))
                    OnRemoved(item);

                return success;
            }
        }

        #region IObservableCollection<T> Members

        public event EventHandler<ObservableCollectionChangedEventArgs<T>> Changed;

        #endregion
    }
}
