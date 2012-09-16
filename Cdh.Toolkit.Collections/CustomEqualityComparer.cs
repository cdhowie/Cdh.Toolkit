//
// CustomEqualityComparer.cs
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

namespace Cdh.Toolkit.Collections
{
    public class CustomEqualityComparer<TObject, TKey> : IEqualityComparer<TObject>
    {
        private Func<TObject, TKey> keySelector;

        public CustomEqualityComparer(Func<TObject, TKey> keySelector)
        {
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");

            this.keySelector = keySelector;
        }

        #region IEqualityComparer<TObject> Members

        public bool Equals(TObject x, TObject y)
        {
            if (!typeof(TObject).IsValueType) {
                // Reference semantics.
                if (object.ReferenceEquals(x, y)) {
                    return true;
                }

                if (object.ReferenceEquals(x, default(TObject))) {
                    return object.ReferenceEquals(y, default(TObject));
                }

                if (object.ReferenceEquals(y, default(TObject))) {
                    return false;
                }
            }

            return EqualityComparer<TKey>.Default.Equals(keySelector(x), keySelector(y));
        }

        public int GetHashCode(TObject obj)
        {
            if (!typeof(TObject).IsValueType) {
                if (object.ReferenceEquals(obj, default(TObject))) {
                    throw new ArgumentNullException("obj");
                }
            }

            return EqualityComparer<TKey>.Default.GetHashCode(keySelector(obj));
        }

        #endregion
    }
}
