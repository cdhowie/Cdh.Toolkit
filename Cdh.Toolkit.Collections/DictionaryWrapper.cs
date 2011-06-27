//
// DictionaryWrapper.cs
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
    internal class DictionaryWrapper<TKey, TValue> :
        CollectionWrapper<KeyValuePair<TKey, TValue>>,
        IDictionary<TKey, TValue>,
        IDictionary
    {
        private IDictionary<TKey, TValue> wrapped;

        public DictionaryWrapper(IDictionary<TKey, TValue> dictionary)
            : base(dictionary)
        {
            wrapped = dictionary;
        }

        #region IDictionary<TKey,TValue> Members

        public void Add(TKey key, TValue value)
        {
            wrapped.Add(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return wrapped.ContainsKey(key);
        }

        public ICollection<TKey> Keys
        {
            get { return wrapped.Keys; }
        }

        public bool Remove(TKey key)
        {
            return wrapped.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return wrapped.TryGetValue(key, out value);
        }

        public ICollection<TValue> Values
        {
            get { return wrapped.Values; }
        }

        public TValue this[TKey key]
        {
            get { return wrapped[key]; }
            set { wrapped[key] = value; }
        }

        #endregion

        #region IDictionary Members

        void IDictionary.Add(object key, object value)
        {
            TKey tKey;
            TValue tValue;

            if (!Rocks.ConvertObject(key, out tKey))
                throw new ArgumentException("Key is not convertible to this dictionary's key type.", "key");

            if (!Rocks.ConvertObject(value, out tValue))
                throw new ArgumentException("Value is not convertible to this dictionary's value type.", "value");

            wrapped.Add(tKey, tValue);
        }

        bool IDictionary.Contains(object key)
        {
            TKey tKey;

            return Rocks.ConvertObject(key, out tKey) ?
                wrapped.ContainsKey(tKey) :
                false;
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return new DictionaryEnumerator(wrapped.GetEnumerator());
        }

        bool IDictionary.IsFixedSize
        {
            // This may actually be true but we have no way of knowing.
            get { return false; }
        }

        ICollection IDictionary.Keys
        {
            get { return new CollectionWrapper<TKey>(wrapped.Keys); }
        }

        void IDictionary.Remove(object key)
        {
            TKey tKey;
            if (Rocks.ConvertObject(key, out tKey))
                wrapped.Remove(tKey);
        }

        ICollection IDictionary.Values
        {
            get { return new CollectionWrapper<TValue>(wrapped.Values); }
        }

        object IDictionary.this[object key]
        {
            get
            {
                TKey tKey;

                if (!Rocks.ConvertObject(key, out tKey))
                    return null;

                TValue tValue;
                return wrapped.TryGetValue(tKey, out tValue) ?
                    (object)tValue :
                    null;
            }
            set
            {
                TKey tKey;
                TValue tValue;

                if (!Rocks.ConvertObject(key, out tKey))
                    throw new ArgumentException("Key type is not convertible to this dictionary's key type.", "key");

                if (!Rocks.ConvertObject(value, out tValue))
                    throw new ArgumentException("Value type is not convertible to this dictionary's value type.", "value");

                wrapped[tKey] = tValue;
            }
        }

        #endregion

        private class DictionaryEnumerator : IDictionaryEnumerator
        {
            private IEnumerator<KeyValuePair<TKey, TValue>> enumerator;

            public DictionaryEnumerator(IEnumerator<KeyValuePair<TKey, TValue>> enumerator)
            {
                if (enumerator == null)
                    throw new ArgumentNullException("enumerator");

                this.enumerator = enumerator;
            }

            private void CheckHasValue()
            {
                if (current == null)
                    throw new InvalidOperationException("Enumerator has no value.");
            }

            #region IDictionaryEnumerator Members

            private DictionaryEntry entry;
            public DictionaryEntry Entry
            {
                get
                {
                    CheckHasValue();
                    return entry;
                }
            }

            private object key;
            public object Key
            {
                get
                {
                    CheckHasValue();
                    return key;
                }
            }

            private object value;
            public object Value
            {
                get
                {
                    CheckHasValue();
                    return value;
                }
            }

            #endregion

            #region IEnumerator Members

            private object current;
            public object Current
            {
                get
                {
                    CheckHasValue();
                    return current;
                }
            }

            private void Invalidate()
            {
                current = null;
                entry = new DictionaryEntry();
                key = null;
                value = null;
            }

            public bool MoveNext()
            {
                var hasNext = enumerator.MoveNext();

                if (hasNext) {
                    var pair = enumerator.Current;

                    key = pair.Key;
                    value = pair.Value;
                    entry = new DictionaryEntry(key, value);
                    current = entry;
                } else {
                    Invalidate();
                }

                return hasNext;
            }

            public void Reset()
            {
                Invalidate();
                enumerator.Reset();
            }

            #endregion
        }
    }
}
