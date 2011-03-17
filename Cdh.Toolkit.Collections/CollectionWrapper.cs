using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Cdh.Toolkit.Collections
{
	internal class CollectionWrapper<T> : ICollection<T>, ICollection
	{
		private ICollection<T> wrapped;

		public CollectionWrapper(ICollection<T> collection)
		{
			if (collection == null)
				throw new ArgumentNullException("collection");

			wrapped = collection;
		}

		#region ICollection<T> Members

		public void Add(T item)
		{
			wrapped.Add(item);
		}

		public void Clear()
		{
			wrapped.Clear();
		}

		public bool Contains(T item)
		{
			return wrapped.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			wrapped.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return wrapped.Count; }
		}

		public bool IsReadOnly
		{
			get { return wrapped.IsReadOnly; }
		}

		public bool Remove(T item)
		{
			return wrapped.Remove(item);
		}

		#endregion

		#region IEnumerable<T> Members

		public IEnumerator<T> GetEnumerator()
		{
			return wrapped.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		#region ICollection Members

		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
				throw new ArgumentNullException("array");

			if (index < 0)
				throw new ArgumentOutOfRangeException("index", index, "Must be >= 0.");

			if (array.Length - index > wrapped.Count)
				throw new ArgumentOutOfRangeException("index", index, "The destination array is too small to copy into at the index provided.");

			var elementType = array.GetType().GetElementType();
			if (!elementType.IsAssignableFrom(typeof(T)))
				throw new ArgumentException("The array's element type is not compatible with this collection's element type.", "array");

			foreach (var thing in wrapped)
				array.SetValue(thing, index++);
		}

		bool ICollection.IsSynchronized
		{
			get { return false; }
		}

		object ICollection.SyncRoot
		{
			get { return wrapped; }
		}

		#endregion
	}
}
