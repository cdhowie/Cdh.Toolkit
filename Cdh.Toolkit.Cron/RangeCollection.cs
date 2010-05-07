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
