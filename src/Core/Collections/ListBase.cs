using System;
using System.Collections;
using System.Collections.Generic;

namespace MaSch.Core.Collections
{
    /// <summary>
    /// A base class for a list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <seealso cref="IList{T}" />
    /// <seealso cref="IReadOnlyList{T}" />
    /// <seealso cref="IList" />
    public abstract class ListBase<T> : IList<T>, IReadOnlyList<T>, IList
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListBase{T}"/> class.
        /// </summary>
        /// <param name="internalList">The internal list.</param>
        protected ListBase(IList<T> internalList)
        {
            InternalList = internalList;
            DefaultSyncRoot = (internalList as ICollection)?.SyncRoot ?? new object();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBase{T}"/> class.
        /// </summary>
        protected ListBase()
            : this(new List<T>())
        {
        }

        /// <inheritdoc />
        public virtual int Count => InternalList.Count;

        /// <inheritdoc />
        bool IList.IsReadOnly => IsReadOnly;

        /// <inheritdoc />
        bool ICollection<T>.IsReadOnly => IsReadOnly;

        /// <inheritdoc />
        bool IList.IsFixedSize => IsFixedSize;

        /// <inheritdoc />
        bool ICollection.IsSynchronized => IsSynchronized;

        /// <inheritdoc />
        object ICollection.SyncRoot => SyncRoot;

        /// <summary>
        /// Gets the default value for the <see cref="SyncRoot"/> property.
        /// </summary>
        protected object DefaultSyncRoot { get; }

        /// <summary>
        /// Gets or sets the internal list.
        /// </summary>
        protected IList<T> InternalList { get; set; }

        /// <summary>
        /// Gets a value indicating whether the System.Collections.IList is read-only.
        /// </summary>
        protected virtual bool IsReadOnly => InternalList.IsReadOnly;

        /// <summary>
        /// Gets a value indicating whether the System.Collections.IList has a fixed size.
        /// </summary>
        protected virtual bool IsFixedSize => (InternalList as IList)?.IsFixedSize ?? false;

        /// <summary>
        /// Gets a value indicating whether access to the System.Collections.ICollection is synchronized (thread safe).
        /// </summary>
        protected virtual bool IsSynchronized => (InternalList as ICollection)?.IsSynchronized ?? false;

        /// <summary>
        /// Gets an object that can be used to synchronize access to the System.Collections.ICollection.
        /// </summary>
        protected virtual object SyncRoot => DefaultSyncRoot;

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
        int IList.Add(object? value)
        {
            var castedValue = (T)value!;
            Add(castedValue);
            return IndexOf(castedValue);
        }

        /// <inheritdoc />
        public virtual void Add(T item)
        {
            InternalList.Add(item);
        }

        /// <inheritdoc />
        public virtual void Clear()
        {
            InternalList.Clear();
        }

        /// <inheritdoc />
        bool IList.Contains(object? value)
        {
            return value is T t && Contains(t);
        }

        /// <inheritdoc />
        public virtual bool Contains(T item)
        {
            return InternalList.Contains(item);
        }

        /// <inheritdoc />
        void ICollection.CopyTo(Array array, int index)
        {
            CopyTo(array, index);
        }

        /// <inheritdoc />
        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            InternalList.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public virtual IEnumerator<T> GetEnumerator()
        {
            return InternalList.GetEnumerator();
        }

        /// <inheritdoc />
        int IList.IndexOf(object? value)
        {
            return IndexOf((T)value!);
        }

        /// <inheritdoc />
        public virtual int IndexOf(T item)
        {
            return InternalList.IndexOf(item);
        }

        /// <inheritdoc />
        void IList.Insert(int index, object? value)
        {
            Insert(index, (T)value!);
        }

        /// <inheritdoc />
        public virtual void Insert(int index, T item)
        {
            InternalList.Insert(index, item);
        }

        /// <inheritdoc />
        void IList.Remove(object? value)
        {
            _ = Remove((T)value!);
        }

        /// <inheritdoc />
        public virtual bool Remove(T item)
        {
            return InternalList.Remove(item);
        }

        /// <inheritdoc />
        public virtual void RemoveAt(int index)
        {
            InternalList.RemoveAt(index);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        protected virtual void CopyTo(Array array, int index)
        {
            if (InternalList is ICollection collection)
                collection.CopyTo(array, index);
            else
                throw new NotImplementedException();
        }
    }
}
