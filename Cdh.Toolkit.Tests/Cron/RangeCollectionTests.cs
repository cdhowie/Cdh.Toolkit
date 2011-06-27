//
// RangeCollectionTests.cs
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
using NUnit.Framework;
using Cdh.Toolkit.Cron;

namespace Cdh.Toolkit.Tests.Cron
{
    [TestFixture]
    public class RangeCollectionTests
    {
        internal static void AssertContains<T>(ICollection<T> expected, ICollection<T> actual, string name)
        {
            Assert.AreEqual(expected.Count, actual.Count, name + ".Count");

            foreach (T item in expected)
                Assert.IsTrue(actual.Contains(item), "Items (expected)");

            foreach (T item in actual)
                Assert.IsTrue(expected.Contains(item), "Items (actual)");
        }

        [Test]
        public void Empty()
        {
            var range = new RangeCollection(1, 50);

            Assert.AreEqual(0, range.Count, "range.Count");

            for (int i = 1; i <= 50; i++)
                Assert.IsFalse(range.Contains(i), "range.Contains(" + i + ")");
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BadConstructor()
        {
            new RangeCollection(1, 0);
        }

        [Test]
        public void Basic()
        {
            var range = new RangeCollection(1, 50);

            int[] items = { 1, 3, 5, 10 };
            foreach (var i in items)
                range.Add(i);

            AssertContains(items, range, "range");
        }

        [Test]
        public void DoubleAdd()
        {
            var range = new RangeCollection(1, 50);

            int[] items = { 1, 3, 5, 10, 5 };
            foreach (var i in items)
                range.Add(i);

            AssertContains(new[] { 1, 3, 5, 10 }, range, "range");
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddBelowMinimum()
        {
            var range = new RangeCollection(1, 50);
            range.Add(0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddAboveMaximum()
        {
            var range = new RangeCollection(1, 50);
            range.Add(51);
        }

        [Test]
        public void Complex()
        {
            var range = new RangeCollection(1, 50);

            range.Add(1);
            range.Add(2);
            range.Add(4);
            range.Add(10);

            AssertContains(new[] { 1, 2, 4, 10 }, range, "range");

            range.Remove(1);
            range.Remove(4);

            AssertContains(new[] { 2, 10 }, range, "range");

            range.Remove(4);
            range.Remove(5);

            AssertContains(new[] { 2, 10 }, range, "range");

            range.Clear();

            AssertContains(new int[0], range, "range");
        }
    }
}
