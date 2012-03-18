using System;
using System.Collections.Generic;

namespace Cdh.Toolkit.Collections.Builders
{
    public static class DictionaryBuilder
    {
        public static DictionaryBuilder<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value)
        {
            return new DictionaryBuilder<TKey, TValue>().Add(key, value);
        }
    }

    public class DictionaryBuilder<TKey, TValue>
    {
        public Dictionary<TKey, TValue> Dictionary { get; private set; }

        public DictionaryBuilder()
        {
            Dictionary = new Dictionary<TKey, TValue>();
        }

        public DictionaryBuilder<TKey, TValue> Add(TKey key, TValue value)
        {
            Dictionary.Add(key, value);

            return this;
        }

        public IDictionary<TKey, TValue> AsReadOnly()
        {
            return new ReadOnlyDictionary<TKey, TValue>(Dictionary);
        }
    }
}

