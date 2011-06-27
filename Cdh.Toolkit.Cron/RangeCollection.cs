//
// RangeCollection.cs
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

namespace Cdh.Toolkit.Cron
{
    public class RangeCollection : ICollection<int>
    {
        public int Minimum { get; private set; }
        public int Maximum { get; private set; }

        private bool[] range;

        public RangeCollection(int minimum, int maximum)
        {
            if (maximum < minimum)
                throw new ArgumentOutOfRangeException("maximum < minimum");

            Minimum = minimum;
            Maximum = maximum;

            range = new bool[maximum - minimum + 1];
        }

        public bool Validate(int number)
        {
            return number >= Minimum && number <= Maximum;
        }

        #region ICollection<int> Members

        public void Add(int item)
        {
            if (!Validate(item))
                throw new ArgumentOutOfRangeException("item: Not within range minimum and maximum.");

            range[item - Minimum] = true;
        }

        public void Clear()
        {
            Array.Clear(range, 0, range.Length);
        }

        public bool Contains(int item)
        {
            if (!Validate(item))
                return false;

            return range[item - Minimum];
        }

        public void CopyTo(int[] array, int arrayIndex)
        {
            for (int i = 0; i < range.Length; i++)
            {
                if (range[i])
                    array[arrayIndex++] = Minimum + i;
            }
        }

        public int Count
        {
            get
            {
                int count = 0;

                foreach (bool i in range)
                    if (i)
                        count++;

                return count;
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(int item)
        {
            if (!Validate(item))
                return false;

            item -= Minimum;

            if (!range[item])
                return false;

            range[item] = false;
            return true;
        }

        #endregion

        #region IEnumerable<int> Members

        public IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < range.Length; i++)
                if (range[i])
                    yield return i + Minimum;
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
