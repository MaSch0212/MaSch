using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace MaSch.Core.Observable.Collections
{
    /// <summary>
    /// Represents an observable <see cref="Queue{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the <see cref="ObservableQueue{T}"/>.</typeparam>
    /// <seealso cref="ICollection" />
    /// <seealso cref="IReadOnlyCollection{T}" />
    /// <seealso cref="INotifyPropertyChanged" />
    /// <seealso cref="INotifyCollectionChanged" />
    [SuppressMessage("Minor Code Smell", "S4136:Method overloads should be grouped together", Justification = "This ordering does not make sense here.")]
    public class ObservableQueue<T> : ICollection, IReadOnlyCollection<T>, INotifyPropertyChanged, INotifyCollectionChanged
    {
        private readonly Queue<T> _queue;

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableQueue{T}"/> class that
        /// is empty and has the default initial capacity.
        /// </summary>
        public ObservableQueue()
        {
            _queue = new Queue<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableQueue{T}"/> class that
        /// contains elements copied from the specified collection and has sufficient capacity
        /// to accommodate the number of elements copied.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new <see cref="ObservableQueue{T}"/>.</param>
        /// <exception cref="ArgumentNullException">collection is null.</exception>
        public ObservableQueue(IEnumerable<T> collection)
        {
            _queue = new Queue<T>(collection);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableQueue{T}"/> class that
        /// is empty and has the specified initial capacity.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="ObservableQueue{T}"/> can contain.</param>
        /// <exception cref="ArgumentOutOfRangeException">capacity is less than zero.</exception>
        public ObservableQueue(int capacity)
        {
            _queue = new Queue<T>(capacity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableQueue{T}"/> class that
        /// uses the given queue as internal queue.
        /// </summary>
        /// <param name="queue">
        /// The queue that is used as internal queue. Changing the queue outside of this class will not notify
        /// subscribers.
        /// </param>
        /// <exception cref="ArgumentNullException">the queue is null.</exception>
        public ObservableQueue(Queue<T> queue)
        {
            Guard.NotNull(queue, nameof(queue));
            _queue = queue;
        }

        #region IEnumerable

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator() => _queue.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => _queue.GetEnumerator();

        #endregion

        #region ICollection

        /// <summary>
        /// Gets the number of elements contained in the <see cref="ObservableQueue{T}"/>.
        /// </summary>
        /// <returns>The number of elements contained in the <see cref="ObservableQueue{T}"/>.</returns>
        public int Count => _queue.Count;

        /// <summary>
        /// Gets a value indicating whether access to the <see cref="ICollection"/>
        /// is synchronized (thread safe).
        /// </summary>
        /// <returns>
        /// true if access to the System.Collections.ICollection is synchronized (thread
        /// safe); otherwise, false.
        /// </returns>
        public bool IsSynchronized => ((ICollection)_queue).IsSynchronized;

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="ICollection"/>.
        /// </summary>
        /// <returns>An object that can be used to synchronize access to the <see cref="ICollection"/>.</returns>
        public object SyncRoot => ((ICollection)_queue).SyncRoot;

        /// <summary>
        /// Copies the elements of the <see cref="ICollection"/> to an System.Array,
        /// starting at a particular System.Array index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="Array"/> that is the destination of the elements copied
        /// from <see cref="ICollection"/>. The <see cref="Array"/> must have zero-based indexing.
        /// </param>
        /// <param name="index">The zero-based index in array at which copying begins.</param>
        /// <exception cref="ArgumentNullException">array is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">index is less than zero.</exception>
        /// <exception cref="ArgumentException">
        /// array is multidimensional.-or- The number of elements in the source <see cref="ICollection"/>
        /// is greater than the available space from index to the end of the destination
        /// array.-or-The type of the source <see cref="ICollection"/> cannot be cast
        /// automatically to the type of the destination array.
        /// </exception>
        public void CopyTo(Array array, int index) => ((ICollection)_queue).CopyTo(array, index);

        #endregion

        /// <summary>
        /// Removes all objects from the <see cref="ObservableQueue{T}"/>.
        /// </summary>
        public void Clear()
        {
            _queue.Clear();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
        }

        /// <summary>
        /// Determines whether an element is in the <see cref="ObservableQueue{T}"/>.
        /// </summary>
        /// <param name="item">
        /// The object to locate in the <see cref="ObservableQueue{T}"/>. The value can
        /// be null for reference types.
        /// </param>
        /// <returns>true if item is found in the <see cref="ObservableQueue{T}"/>; otherwise, false.</returns>
        public bool Contains(T item) => _queue.Contains(item);

        /// <summary>
        /// Copies the <see cref="ObservableQueue{T}"/> elements to an existing one-dimensional
        /// <see cref="Array"/>, starting at the specified array index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="Array"/> that is the destination of the elements copied
        /// from <see cref="ObservableQueue{T}"/>. The <see cref="Array"/> must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="ArgumentNullException">array is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">index is less than zero.</exception>
        /// <exception cref="ArgumentException">
        /// The number of elements in the source <see cref="ObservableQueue{T}"/> is greater
        /// than the available space from arrayIndex to the end of the destination array.
        /// </exception>
        public void CopyTo(T[] array, int arrayIndex) => _queue.CopyTo(array, arrayIndex);

        /// <summary>
        /// Removes and returns the object at the beginning of the <see cref="ObservableQueue{T}"/>.
        /// </summary>
        /// <returns>The object that is removed from the beginning of the <see cref="ObservableQueue{T}"/>.</returns>
        /// <exception cref="InvalidOperationException">The <see cref="ObservableQueue{T}"/> is empty.</exception>
        public T Dequeue()
        {
            var item = _queue.Dequeue();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, 0));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
            return item;
        }

        /// <summary>
        /// Adds an object to the end of the <see cref="ObservableQueue{T}"/>.
        /// </summary>
        /// <param name="item">
        /// The object to add to the <see cref="ObservableQueue{T}"/>. The value can be
        /// null for reference types.
        /// </param>
        public void Enqueue(T item)
        {
            _queue.Enqueue(item);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, Count - 1));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
        }

        /// <summary>
        /// Returns the object at the beginning of the <see cref="ObservableQueue{T}"/>
        /// without removing it.
        /// </summary>
        /// <returns>The object at the beginning of the <see cref="ObservableQueue{T}"/>.</returns>
        /// <exception cref="InvalidOperationException">The <see cref="ObservableQueue{T}"/> is empty.</exception>
        public T Peek() => _queue.Peek();

        /// <summary>
        /// Copies the <see cref="ObservableQueue{T}"/> elements to a new array.
        /// </summary>
        /// <returns>A new array containing elements copied from the <see cref="ObservableQueue{T}"/>.</returns>
        public T[] ToArray() => _queue.ToArray();

        /// <summary>
        /// Sets the capacity to the actual number of elements in the System.Collections.Generic.Queue`1,
        /// if that number is less than 90 percent of current capacity.
        /// </summary>
        public void TrimExcess() => _queue.TrimExcess();
    }
}
