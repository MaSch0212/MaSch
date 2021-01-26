using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MaSch.Core.Extensions;

namespace MaSch.Core.Collections
{
    /// <summary>
    /// A wrapper class for an array, so it is resizable.
    /// </summary>
    /// <typeparam name="T">The type of the array elements</typeparam>
    public class ResizableArray<T> : IList<T>
    {
        private T[] _array;

        /// <summary>
        /// The internal array.
        /// </summary>
        public T[] InternalArray => _array;
        /// <inheritdoc />
        public int Count { get; private set; }
        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResizableArray{T}"/> class.
        /// </summary>
        /// <param name="initialCapacity">The starting capacity of the array. Default is 4.</param>
        public ResizableArray(int initialCapacity = 4) : this(new T[initialCapacity]) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ResizableArray{T}"/> class.
        /// </summary>
        /// <param name="array">The array to wrap.</param>
        public ResizableArray(T[] array)
        {
            Guard.NotNull(array, nameof(array));

            _array = array;
        }

        /// <summary>
        /// Resizes the array to the actual Count of this List.
        /// </summary>
        public void ShrinkArray()
        {
            Array.Resize(ref _array, Count);
        }

        #region IList<T> Members
        /// <inheritdoc />
        public void Add(T element)
        {
            if (Count == _array.Length)
            {
                Array.Resize(ref _array, _array.Length * 2);
            }

            _array[Count++] = element;
        }

        /// <inheritdoc />
        public bool Remove(T item)
        {
            var index = IndexOf(item);
            if (index < 0)
                return false;
            RemoveAt(index);
            return true;
        }

        /// <inheritdoc />
        public void Clear()
        {
            Count = 0;
            for (int i = 0; i < _array.Length; i++)
                _array[0] = default!;
        }

        /// <inheritdoc />
        public bool Contains(T item)
        {
            return _array.Contains(item);
        }

        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex)
        {
            _array.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>)_array.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public int IndexOf(T item)
        {
            return _array.IndexOf(item);
        }

        /// <inheritdoc />
        public void Insert(int index, T item)
        {
            Guard.NotOutOfRange(index, nameof(index), 0, Count);
            if (Count == _array.Length)
            {
                Array.Resize(ref _array, _array.Length * 2);
            }
            for (int i = index; i < Count; i++)
                _array[i + 1] = _array[i];
            _array[index] = item;
        }

        /// <inheritdoc />
        public void RemoveAt(int index)
        {
            Guard.NotOutOfRange(index, nameof(index), 0, Count - 1);
            for (int i = index; i < Count - 1; i++)
                _array[i] = _array[i + 1];
            _array[Count - 1] = default!;
        }

        /// <inheritdoc />
        public T this[int index]
        {
            get
            {
                Guard.NotOutOfRange(index, nameof(index), 0, Count - 1);
                return _array[index];
            }
            set
            {
                Guard.NotOutOfRange(index, nameof(index), 0, Count - 1);
                _array[index] = value;
            }
        }
        #endregion
    }
}
