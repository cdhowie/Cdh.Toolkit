//
// RangeCollectionExtensionsTests.cs
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
    public class RangeCollectionExtensionsTests
    {
        [Test]
        public void SingleNumber()
        {
            var range = new RangeCollection(1, 50);

            range.AddRangeSpecifier("5");

            RangeCollectionTests.AssertContains(new[] { 5 }, range, "range");
        }

        [Test]
        public void MultipleNumbers()
        {
            var range = new RangeCollection(1, 50);

            range.AddRangeSpecifier("5,7,9,5");

            RangeCollectionTests.AssertContains(new[] { 5, 7, 9 }, range, "range");
        }

        [Test]
        public void Range()
        {
            var range = new RangeCollection(1, 50);

            range.AddRangeSpecifier("10-20");

            RangeCollectionTests.AssertContains(
                new[] { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, range, "range");
        }

        [Test]
        public void RangeWithStep()
        {
            var range = new RangeCollection(1, 50);

            range.AddRangeSpecifier("10-20/3");

            RangeCollectionTests.AssertContains(
                new[] { 10, 13, 16, 19 }, range, "range");
        }

        [Test]
        public void All()
        {
            var range = new RangeCollection(1, 10);

            range.AddRangeSpecifier("*");

            RangeCollectionTests.AssertContains(
                new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, range, "range");
        }

        [Test]
        public void AllWithStep()
        {
            var range = new RangeCollection(1, 10);

            range.AddRangeSpecifier("*/2");

            RangeCollectionTests.AssertContains(
                new[] { 1, 3, 5, 7, 9 }, range, "range");
        }

        [Test]
        public void MultipleRanges()
        {
            var range = new RangeCollection(1, 50);

            range.AddRangeSpecifier("4-8,25-27");

            RangeCollectionTests.AssertContains(
                new[] { 4, 5, 6, 7, 8, 25, 26, 27 }, range, "range");
        }

        [Test]
        public void MultipleRangesWithStep()
        {
            var range = new RangeCollection(1, 50);

            range.AddRangeSpecifier("4-8,25-27,10-20/4");

            RangeCollectionTests.AssertContains(
                new[] { 4, 5, 6, 7, 8, 10, 14, 18, 25, 26, 27 }, range, "range");
        }

        [Test]
        public void OverlappingRangesWithStep()
        {
            var range = new RangeCollection(1, 50);

            range.AddRangeSpecifier("10-20/3,11-19/2");

            RangeCollectionTests.AssertContains(
                new[] { 10, 11, 13, 15, 16, 17, 19 }, range, "range");
        }

        [Test]
        public void MixedRange()
        {
            var range = new RangeCollection(1, 50);

            range.AddRangeSpecifier("5,10-13,15-20/2,*/10");

            RangeCollectionTests.AssertContains(
                new[] { 1, 5, 10, 11, 12, 13, 15, 17, 19, 21, 31, 41 }, range, "range");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void NumberWithRangeThrows()
        {
            var range = new RangeCollection(1, 50);

            range.AddRangeSpecifier("1/5");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void EmptyStringThrows()
        {
            var range = new RangeCollection(1, 50);

            range.AddRangeSpecifier("");
        }
    }
}
