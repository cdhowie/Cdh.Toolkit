﻿//
// ReadOnlyObservableList.cs
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

using Cdh.Toolkit.Extensions.Events;

namespace Cdh.Toolkit.Collections.Observable
{
    public class ReadOnlyObservableList<T> : ReadOnlyList<T>, IObservableList<T>
    {
        protected new IObservableList<T> Decorated { get; private set; }

        public ReadOnlyObservableList(IObservableList<T> list)
            : base(list)
        {
            Decorated = list;

            Decorated.Changed += OnDecoratedChanged;
        }

        void OnDecoratedChanged(object sender, ObservableListChangedEventArgs<T> e)
        {
            Changed.Fire(this, e);
            CollectionChanged.Fire(this, e);
        }

        #region IObservableList<T> Members

        public event EventHandler<ObservableListChangedEventArgs<T>> Changed;

        #endregion

        #region IObservableCollection<T> Members

        private event EventHandler<ObservableCollectionChangedEventArgs<T>> CollectionChanged;

        event EventHandler<ObservableCollectionChangedEventArgs<T>> IObservableCollection<T>.Changed
        {
            add { CollectionChanged += value; }
            remove { CollectionChanged -= value; }
        }

        void IObservableCollection<T>.AddRange(IEnumerable<T> items)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
