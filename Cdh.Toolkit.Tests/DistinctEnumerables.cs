﻿//
// DistinctEnumerables.cs
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

using Cdh.Toolkit.Collections;
using NUnit.Framework;

namespace Cdh.Toolkit.Tests
{
    /// <summary>
    /// Ensures that ReadOnlyCollection and SynchronizedCollection treat
    /// <see cref="IEnumerable<T>.GetEnumerator"/> and
    /// <see cref="IEnumerable.GetEnumerator"/> differently.
    /// </summary>
    [TestFixture]
    public class DistinctEnumerables
    {
        private ListImplementation list = new ListImplementation();

        public DistinctEnumerables() { }

        [Test]
        public void ReadOnlyCollection()
        {
            var col = new ReadOnlyCollection<int>(list);
            var e = new EnumerableWrapper<int>(col);

            RunTest(e);
        }

        [Test]
        public void SynchronizedCollectionCopy()
        {
            var col = new SynchronizedCollection<int>(list, EnumerateBehavior.Copy);
            var e = new EnumerableWrapper<int>(col);

            RunTest(e);
        }

        [Test]
        public void SynchronizedCollectionLock()
        {
            var col = new SynchronizedCollection<int>(list, EnumerateBehavior.Lock);
            var e = new EnumerableWrapper<int>(col);

            RunTest(e);
        }

        private void RunTest(IEnumerable<int> e)
        {
            Assert.AreEqual(2, e.Count(), "e.Count()");
            Assert.AreEqual(3, e.Cast<object>().Count(), "e.Cast<object>().Count()");
        }

        private class ListImplementation : IList<int>
        {
            #region IList<int> Members

            public int IndexOf(int item)
            {
                throw new NotImplementedException();
            }

            public void Insert(int index, int item)
            {
                throw new NotImplementedException();
            }

            public void RemoveAt(int index)
            {
                throw new NotImplementedException();
            }

            public int this[int index]
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            #endregion

            #region ICollection<int> Members

            public void Add(int item)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(int item)
            {
                throw new NotImplementedException();
            }

            public void CopyTo(int[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public int Count
            {
                get { throw new NotImplementedException(); }
            }

            public bool IsReadOnly
            {
                get { throw new NotImplementedException(); }
            }

            public bool Remove(int item)
            {
                throw new NotImplementedException();
            }

            #endregion

            #region IEnumerable<int> Members

            public IEnumerator<int> GetEnumerator()
            {
                yield return 1;
                yield return 3;
            }

            #endregion

            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                yield return 2;
                yield return 4;
                yield return 6;
            }

            #endregion
        }
    }
}
