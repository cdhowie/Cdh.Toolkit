using System;
using System.Collections.Generic;
using System.Collections;

namespace Cdh.Toolkit.Collections
{
    public class ReadOnlyList<T> : ReadOnlyCollection<T>, IList<T>, IList
    {
        protected new IList<T> Decorated { get; private set; }
		protected new IList LegacyDecorated { get; private set; }

        public ReadOnlyList(IList<T> list)
            : base(list)
        {
            Decorated = list;
			LegacyDecorated = (IList)base.LegacyDecorated;
        }

		protected override ICollection CreateLegacyCollection(ICollection<T> collection)
		{
			var legacy = collection as IList;
			if (legacy != null)
				return legacy;

            return new ListWrapper<T>((IList<T>)collection);
		}

        #region IList<T> Members

        public virtual int IndexOf(T item)
        {
            return Decorated.IndexOf(item);
        }

        void IList<T>.Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        void IList<T>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        public virtual T this[int index]
        {
            get { return Decorated[index]; }
            set { throw new NotSupportedException(); }
        }

        #endregion

		#region IList Members

		int IList.Add(object value)
		{
			throw new NotSupportedException();
		}

		void IList.Clear()
		{
			throw new NotSupportedException();
		}

		bool IList.Contains(object value)
		{
			return LegacyDecorated.Contains(value);
		}

		int IList.IndexOf(object value)
		{
			return LegacyDecorated.IndexOf(value);
		}

		void IList.Insert(int index, object value)
		{
			throw new NotSupportedException();
		}

		bool IList.IsFixedSize
		{
			get { return LegacyDecorated.IsFixedSize; }
		}

		void IList.Remove(object value)
		{
			throw new NotSupportedException();
		}

		void IList.RemoveAt(int index)
		{
			throw new NotSupportedException();
		}

		object IList.this[int index]
		{
			get { return LegacyDecorated[index]; }
			set { LegacyDecorated[index] = value; }
		}

		#endregion
	}
}
