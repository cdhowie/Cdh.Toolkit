using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Cdh.Toolkit.Collections
{
	internal class ListWrapper<T> : CollectionWrapper<T>, IList<T>, IList
	{
		private IList<T> wrapped;

		public ListWrapper(IList<T> list)
			: base(list)
		{
			wrapped = list;
		}

		#region IList<T> Members

		public int IndexOf(T item)
		{
			return wrapped.IndexOf(item);
		}

		public void Insert(int index, T item)
		{
			wrapped.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			wrapped.RemoveAt(index);
		}

		public T this[int index]
		{
			get { return wrapped[index]; }
			set { wrapped[index] = value; }
		}

		#endregion

		#region IList Members

		int IList.Add(object value)
		{
			T t;
			if (!Rocks.ConvertObject(value, out t))
				return -1;

			try {
				var index = wrapped.Count;
				Insert(index, t);

				return index;
			} catch {
				return -1;
			}
		}

		bool IList.Contains(object value)
		{
			T t;

			return Rocks.ConvertObject(value, out t) ?
				wrapped.Contains(t) :
				false;
		}

		int IList.IndexOf(object value)
		{
			T t;

			return Rocks.ConvertObject(value, out t) ?
				wrapped.IndexOf(t) :
				-1;
		}

		void IList.Insert(int index, object value)
		{
			T t;
			if (!Rocks.ConvertObject(value, out t))
				throw new ArgumentException("Object is not convertible to the element type of this list.", "value");

			wrapped.Insert(index, t);
		}

		bool IList.IsFixedSize
		{
			// This may actually be true but we have no way of knowing.
			get { return false; }
		}

		void IList.Remove(object value)
		{
			T t;
			if (Rocks.ConvertObject(value, out t))
				wrapped.Remove(t);
		}

		object IList.this[int index]
		{
			get { return wrapped[index]; }
			set
			{
				T t;
				if (!Rocks.ConvertObject(value, out t))
					throw new ArgumentException("Object is not convertible to the element type of this list.", "value");

				wrapped[index] = t;
			}
		}

		#endregion
	}
}
