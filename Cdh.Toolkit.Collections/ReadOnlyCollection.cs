using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Cdh.Toolkit.Collections
{
    [DebuggerDisplay("Count = {Count}")]
    public class ReadOnlyCollection<T> : ICollection<T>, ICollection
    {
        protected ICollection<T> Decorated { get; private set; }
		protected ICollection LegacyDecorated { get; private set; }

		public ReadOnlyCollection(ICollection<T> collection)
		{
			if (collection == null)
				throw new ArgumentNullException("collection");

			Decorated = collection;

			LegacyDecorated = CreateLegacyCollection(collection);
			if (LegacyDecorated == null)
				throw new ArgumentException("Unable to convert collection to legacy collection type.", "collection");
		}

		protected virtual ICollection CreateLegacyCollection(ICollection<T> collection)
		{
			var legacy = collection as ICollection;
			if (legacy != null)
				return legacy;

			return new CollectionWrapper<T>(collection);
		}

        #region ICollection<T> Members

        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException();
        }

        void ICollection<T>.Clear()
        {
            throw new NotSupportedException();
        }

        public virtual bool Contains(T item)
        {
            return Decorated.Contains(item);
        }

        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            Decorated.CopyTo(array, arrayIndex);
        }

        public virtual int Count
        {
            get { return Decorated.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region IEnumerable<T> Members

        public virtual IEnumerator<T> GetEnumerator()
        {
            return Decorated.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Decorated).GetEnumerator();
        }

        #endregion

		#region ICollection Members

		void ICollection.CopyTo(Array array, int index)
		{
			LegacyDecorated.CopyTo(array, index);
		}

		bool ICollection.IsSynchronized
		{
			get { return LegacyDecorated.IsSynchronized; }
		}

		object ICollection.SyncRoot
		{
			get { return LegacyDecorated.SyncRoot; }
		}

		#endregion
	}
}
