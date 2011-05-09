using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdh.Toolkit.Collections
{
    public class CustomEqualityComparer<TObject, TKey> : IEqualityComparer<TObject>
    {
        private Func<TObject, TKey> keySelector;

        public CustomEqualityComparer(Func<TObject, TKey> keySelector)
        {
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");

            this.keySelector = keySelector;
        }

        #region IEqualityComparer<TObject> Members

        public bool Equals(TObject x, TObject y)
        {
            return EqualityComparer<TKey>.Default.Equals(keySelector(x), keySelector(y));
        }

        public int GetHashCode(TObject obj)
        {
            return EqualityComparer<TKey>.Default.GetHashCode(keySelector(obj));
        }

        #endregion
    }
}
