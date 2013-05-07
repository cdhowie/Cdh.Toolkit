//
// ReadOnlyList.cs
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
using System.Collections;
using System.Diagnostics;

namespace Cdh.Toolkit.Collections
{
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof(CollectionDebuggerView<>))]
    public class ReadOnlyList<T> : ReadOnlyCollection<T>, IList<T>, IList
    {
        protected new IList<T> Decorated { get; private set; }
        protected new IList LegacyDecorated { get; private set; }

        public ReadOnlyList(IList<T> list)
            : base(list)
        {
            Decorated = list;
            LegacyDecorated = (IList)base.LegacyDecorated;
        }

        protected override ICollection CreateLegacyCollection(ICollection<T> collection)
        {
            var legacy = collection as IList;
            if (legacy != null)
                return legacy;

            return new ListWrapper<T>((IList<T>)collection);
        }

        #region IList<T> Members

        public virtual int IndexOf(T item)
        {
            return Decorated.IndexOf(item);
        }

        void IList<T>.Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        void IList<T>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        public virtual T this[int index]
        {
            get { return Decorated[index]; }
            set { throw new NotSupportedException(); }
        }

        #endregion

        #region IList Members

        int IList.Add(object value)
        {
            throw new NotSupportedException();
        }

        void IList.Clear()
        {
            throw new NotSupportedException();
        }

        bool IList.Contains(object value)
        {
            return LegacyDecorated.Contains(value);
        }

        int IList.IndexOf(object value)
        {
            return LegacyDecorated.IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
            throw new NotSupportedException();
        }

        bool IList.IsFixedSize
        {
            get { return LegacyDecorated.IsFixedSize; }
        }

        void IList.Remove(object value)
        {
            throw new NotSupportedException();
        }

        void IList.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        object IList.this[int index]
        {
            get { return LegacyDecorated[index]; }
            set { LegacyDecorated[index] = value; }
        }

        #endregion
    }
}
