using System;
using System.Collections.Generic;
using Cdh.Toolkit.Extensions;

namespace Cdh.Toolkit.Collections
{
    public class SelfPopulatingDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        protected IDictionary<TKey, TValue> Wrapped { get; private set; }

        protected Func<TKey, TValue> ValueFunction { get; private set; }
        
        public SelfPopulatingDictionary(Func<TKey, TValue> valueFunction)
            : this(new Dictionary<TKey, TValue>(), valueFunction) { }

        public SelfPopulatingDictionary(IDictionary<TKey, TValue> dictionary, Func<TKey, TValue> valueFunction)
        {
            Check.ArgumentIsNotNull(dictionary, "dictionary");
            Check.ArgumentIsNotNull(valueFunction, "valueFunction");

            if (dictionary.IsReadOnly) {
                throw new ArgumentException("Must not be read only.", "dictionary");
            }

            Wrapped = dictionary;
            ValueFunction = valueFunction;
        }

        #region IDictionary implementation

        public void Add(TKey key, TValue value)
        {
            Wrapped.Add(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            /*
             * Conundrum:
             * 
             * If we always return true, then the collection returned by Keys will not contain all possible keys (how can it?).
             * 
             * If we return Wrapped.ContainsKey(key), then code that uses the return value of this method to determine whether or not this[key] will throw will malfunction.
             */

            return Wrapped.ContainsKey(key);
        }

        public bool Remove(TKey key)
        {
            return Wrapped.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            value = this[key];

            return true;
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue value;

                if (!Wrapped.TryGetValue(key, out value)) {
                    value = ValueFunction(key);

                    Wrapped[key] = value;
                }

                return value;
            }
            set { Wrapped[key] = value; }
        }

        public ICollection<TKey> Keys {
            get { return Wrapped.Keys; }
        }

        public ICollection<TValue> Values {
            get { return Wrapped.Values; }
        }

        #endregion

        #region ICollection implementation

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            Wrapped.Add(item);
        }

        public void Clear()
        {
            Wrapped.Clear();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return Wrapped.Contains(item);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            Wrapped.CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            return Wrapped.Remove(item);
        }

        public int Count {
            get { return Wrapped.Count; }
        }

        public bool IsReadOnly {
            get { return Wrapped.IsReadOnly; }
        }

        #endregion

        #region IEnumerable implementation

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return Wrapped.GetEnumerator();
        }

        #endregion

        #region IEnumerable implementation

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)Wrapped).GetEnumerator();
        }

        #endregion
    }
}

