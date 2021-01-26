using System;
using System.Collections;
using System.Collections.Generic;

namespace MaSch.Core.Collections
{
    public abstract class ListBase<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IList<T>, IReadOnlyCollection<T>, IReadOnlyList<T>, ICollection, IList
    {
        protected readonly object DefaultSyncRoot;
        protected IList<T> InternalList { get; set; }

        protected ListBase(IList<T> internalList)
        {
            InternalList = internalList;
            DefaultSyncRoot = (internalList as ICollection)?.SyncRoot ?? new object();
        }
        protected ListBase()
            : this(new List<T>())
        {
        }

        /// <inheritdoc />
        public virtual T this[int index]
        {
            get => InternalList[index];
            set => InternalList[index] = value;
        }
        /// <inheritdoc />
        object? IList.this[int index]
        {
            get => this[index];
            set => this[index] = (T)value!;
        }

        /// <inheritdoc />
        public virtual int Count => InternalList.Count;

        protected virtual bool IsReadOnly => InternalList.IsReadOnly;
        bool IList.IsReadOnly => IsReadOnly;
        bool ICollection<T>.IsReadOnly => IsReadOnly;

        protected virtual bool IsFixedSize => (InternalList as IList)?.IsFixedSize ?? false;
        bool IList.IsFixedSize => IsFixedSize;

        protected virtual bool IsSynchronized => (InternalList as ICollection)?.IsSynchronized ?? false;
        bool ICollection.IsSynchronized => IsSynchronized;

        protected virtual object SyncRoot => DefaultSyncRoot;
        object ICollection.SyncRoot => SyncRoot;

        int IList.Add(object? value)
        {
            var tValue = (T)value!;
            Add(tValue);
            return IndexOf(tValue);
        }
        /// <inheritdoc />
        public virtual void Add(T item) => InternalList.Add(item);

        /// <inheritdoc />
        public virtual void Clear() => InternalList.Clear();

        bool IList.Contains(object? value) => value is T t && Contains(t);
        /// <inheritdoc />
        public virtual bool Contains(T item) => InternalList.Contains(item);

        protected virtual void CopyTo(Array array, int index)
        {
            if (InternalList is ICollection collection)
                collection.CopyTo(array, index);
            else
                throw new NotImplementedException();
        }
        void ICollection.CopyTo(Array array, int index) => CopyTo(array, index);
        /// <inheritdoc />
        public virtual void CopyTo(T[] array, int arrayIndex) => InternalList.CopyTo(array, arrayIndex);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        /// <inheritdoc />
        public virtual IEnumerator<T> GetEnumerator() => InternalList.GetEnumerator();

        int IList.IndexOf(object? value) => IndexOf((T)value!);
        /// <inheritdoc />
        public virtual int IndexOf(T item) => InternalList.IndexOf(item);

        void IList.Insert(int index, object? value) => Insert(index, (T)value!);
        /// <inheritdoc />
        public virtual void Insert(int index, T item) => InternalList.Insert(index, item);

        void IList.Remove(object? value) => Remove((T)value!);
        /// <inheritdoc />
        public virtual bool Remove(T item) => InternalList.Remove(item);

        /// <inheritdoc />
        public virtual void RemoveAt(int index) => InternalList.RemoveAt(index);
    }
}
