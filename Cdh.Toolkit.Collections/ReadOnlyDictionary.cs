//
// ReadOnlyDictionary.cs
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
    [DebuggerTypeProxy(typeof(CollectionDebuggerView<,>))]
    public class ReadOnlyDictionary<TKey, TValue> :
        ReadOnlyCollection<KeyValuePair<TKey, TValue>>,
        IDictionary<TKey, TValue>,
        IDictionary
    {
        protected new IDictionary<TKey, TValue> Decorated { get; private set; }
        protected new IDictionary LegacyDecorated { get; private set; }

        public ReadOnlyDictionary(IDictionary<TKey, TValue> dictionary)
            : base(dictionary)
        {
            Decorated = dictionary;
            LegacyDecorated = (IDictionary)base.LegacyDecorated;
        }

        protected override ICollection CreateLegacyCollection(ICollection<KeyValuePair<TKey, TValue>> collection)
        {
            var legacy = collection as IDictionary;
            if (legacy != null)
                return legacy;

            return new DictionaryWrapper<TKey, TValue>((IDictionary<TKey, TValue>)collection);
        }

        #region IDictionary<TKey,TValue> Members

        void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            throw new NotSupportedException();
        }

        public virtual bool ContainsKey(TKey key)
        {
            return Decorated.ContainsKey(key);
        }

        public virtual ICollection<TKey> Keys
        {
            get { return Decorated.Keys; }
        }

        bool IDictionary<TKey, TValue>.Remove(TKey key)
        {
            throw new NotSupportedException();
        }

        public virtual bool TryGetValue(TKey key, out TValue value)
        {
            return Decorated.TryGetValue(key, out value);
        }

        public virtual ICollection<TValue> Values
        {
            get { return Decorated.Values; }
        }

        public virtual TValue this[TKey key]
        {
            get { return Decorated[key]; }
            set { throw new NotSupportedException(); }
        }

        #endregion

        #region IDictionary Members

        void IDictionary.Add(object key, object value)
        {
            throw new NotSupportedException();
        }

        void IDictionary.Clear()
        {
            throw new NotSupportedException();
        }

        bool IDictionary.Contains(object key)
        {
            return LegacyDecorated.Contains(key);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return LegacyDecorated.GetEnumerator();
        }

        bool IDictionary.IsFixedSize
        {
            get { return LegacyDecorated.IsFixedSize; }
        }

        bool IDictionary.IsReadOnly
        {
            get { return true; }
        }

        ICollection IDictionary.Keys
        {
            get { return LegacyDecorated.Keys; }
        }

        void IDictionary.Remove(object key)
        {
            throw new NotSupportedException();
        }

        ICollection IDictionary.Values
        {
            get { return LegacyDecorated.Values; }
        }

        object IDictionary.this[object key]
        {
            get { return LegacyDecorated[key]; }
            set { LegacyDecorated[key] = value; }
        }

        #endregion
    }
}
