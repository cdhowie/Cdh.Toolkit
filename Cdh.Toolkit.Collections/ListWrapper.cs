//
// ListWrapper.cs
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
    internal class ListWrapper<T> : CollectionWrapper<T>, IList<T>, IList
    {
        private IList<T> wrapped;

        public ListWrapper(IList<T> list)
            : base(list)
        {
            wrapped = list;
        }

        #region IList<T> Members

        public int IndexOf(T item)
        {
            return wrapped.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            wrapped.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            wrapped.RemoveAt(index);
        }

        public T this[int index]
        {
            get { return wrapped[index]; }
            set { wrapped[index] = value; }
        }

        #endregion

        #region IList Members

        int IList.Add(object value)
        {
            T t;
            if (!Rocks.ConvertObject(value, out t))
                return -1;

            try {
                var index = wrapped.Count;
                Insert(index, t);

                return index;
            } catch {
                return -1;
            }
        }

        bool IList.Contains(object value)
        {
            T t;

            return Rocks.ConvertObject(value, out t) ?
                wrapped.Contains(t) :
                false;
        }

        int IList.IndexOf(object value)
        {
            T t;

            return Rocks.ConvertObject(value, out t) ?
                wrapped.IndexOf(t) :
                -1;
        }

        void IList.Insert(int index, object value)
        {
            T t;
            if (!Rocks.ConvertObject(value, out t))
                throw new ArgumentException("Object is not convertible to the element type of this list.", "value");

            wrapped.Insert(index, t);
        }

        bool IList.IsFixedSize
        {
            // This may actually be true but we have no way of knowing.
            get { return false; }
        }

        void IList.Remove(object value)
        {
            T t;
            if (Rocks.ConvertObject(value, out t))
                wrapped.Remove(t);
        }

        object IList.this[int index]
        {
            get { return wrapped[index]; }
            set
            {
                T t;
                if (!Rocks.ConvertObject(value, out t))
                    throw new ArgumentException("Object is not convertible to the element type of this list.", "value");

                wrapped[index] = t;
            }
        }

        #endregion
    }
}
