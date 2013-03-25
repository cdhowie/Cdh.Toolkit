using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Cdh.Toolkit.Extensions.Events;
using Cdh.Toolkit.Extensions.ReaderWriterLockSlim;

namespace Cdh.Toolkit.Collections.Observable
{
    public class ObservableList<T> : SynchronizedList<T>, IObservableList<T>
    {
        public ObservableList(IList<T> list, EnumerateBehavior behavior, ReaderWriterLockSlim @lock)
            : base(list, behavior, @lock) { }

        public ObservableList(IList<T> list, EnumerateBehavior behavior)
            : base(list, behavior) { }

        public event EventHandler<ObservableListChangedEventArgs<T>> Changed;

        private event EventHandler<ObservableCollectionChangedEventArgs<T>> CollectionChanged;

        event EventHandler<ObservableCollectionChangedEventArgs<T>> IObservableCollection<T>.Changed
        {
            add { CollectionChanged += value; }
            remove { CollectionChanged -= value; }
        }

        private void FireChanged(T item, ObservableChangeType changeType, int index)
        {
            var args = new ObservableListChangedEventArgs<T>(item, changeType, index);

            Changed.Fire(this, args);
            CollectionChanged.Fire(this, args);
        }

        protected virtual void FireInserted(T item, int index)
        {
            FireChanged(item, ObservableChangeType.Add, index);
        }

        protected virtual void FireRemoved(T item, int index)
        {
            FireChanged(item, ObservableChangeType.Remove, index);
        }

        protected virtual void FireReplaced(T item, T replacedItem, int index)
        {
            var args = new ObservableListChangedEventArgs<T>(item, replacedItem, index);

            Changed.Fire(this, args);
            CollectionChanged.Fire(this, args);
        }

        public override bool Add(T item)
        {
            using (Lock.Write()) {
                var success = base.Add(item);

                if (success) {
                    FireInserted(item, Decorated.Count - 1);
                }

                return success;
            }
        }

        public override void Clear()
        {
            using (Lock.Write()) {
                if (Decorated.Count == 0) {
                    return;
                }

                var h1 = CollectionChanged;
                var h2 = Changed;

                var args = new List<ObservableListChangedEventArgs<T>>(Decorated.Count);

                if (h1 != null || h2 != null) {
                    // Start at the back for optimal performance in most observation patterns.
                    for (int i = Decorated.Count - 1; i >= 0; --i) {
                        args.Add(new ObservableListChangedEventArgs<T>(Decorated[i], ObservableChangeType.Remove, i));
                    }
                }

                Decorated.Clear();

                foreach (var arg in args) {
                    if (h1 != null) { h1(this, arg); }
                    if (h2 != null) { h2(this, arg); }
                }
            }
        }

        public override void Insert(int index, T item)
        {
            using (Lock.Write()) {
                Decorated.Insert(index, item);

                FireInserted(item, index);
            }
        }

        public override bool Remove(T item)
        {
            using (Lock.Write()) {
                var index = Decorated.IndexOf(item);

                if (index == -1) {
                    return false;
                }

                Decorated.RemoveAt(index);

                FireRemoved(item, index);

                return true;
            }
        }

        public override void RemoveAt(int index)
        {
            using (Lock.Write()) {
                var item = Decorated[index];

                Decorated.RemoveAt(index);

                FireRemoved(item, index);
            }
        }

        public override T this[int index]
        {
            get { return base[index]; }
            set
            {
                using (Lock.Write()) {
                    var oldItem = Decorated[index];

                    Decorated[index] = value;

                    FireReplaced(value, oldItem, index);
                }
            }
        }
    }
}
