using System;
using System.Collections.Generic;
using System.Collections;

namespace Cdh.Toolkit.Collections
{
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
