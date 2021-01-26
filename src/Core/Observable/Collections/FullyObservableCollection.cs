using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using MaSch.Core.Extensions;

namespace MaSch.Core.Observable.Collections
{
    /// <summary>
    /// Represents a dynamic data collection that provides notifications when items get added, removed, are changed, or when the whole list is refreshed.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="ObservableCollection{T}" />
    public class FullyObservableCollection<T> : ObservableCollection<T> where T : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property of a item has changed.
        /// </summary>
        public event CollectionItemPropertyChangedEventHandler? CollectionItemPropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="FullyObservableCollection{T}"/> class.
        /// </summary>
        public FullyObservableCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FullyObservableCollection{T}"/> class that contains elements copied from the specified collection.
        /// </summary>
        /// <param name="collection">The collection from which the elements are copied.</param>
        public FullyObservableCollection(IEnumerable<T> collection) : base(collection)
        {
            foreach (var item in this)
                item.PropertyChanged += Item_PropertyChanged;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FullyObservableCollection{T}"/> class that contains elements copied from the specified list.
        /// </summary>
        /// <param name="list">The list from which the elements are copied.</param>
        public FullyObservableCollection(List<T> list) : base(list)
        {
            foreach (var item in this)
                item.PropertyChanged += Item_PropertyChanged;
        }

        private void Item_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is T t)
            {
                var eventArgs = new CollectionItemPropertyChangedEventArgs(IndexOf(t), sender, e.PropertyName);
                OnCollectionItemPropertyChanged(eventArgs);
            }
        }

        /// <summary>
        /// Raises the <see cref="CollectionItemPropertyChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="CollectionItemPropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnCollectionItemPropertyChanged(CollectionItemPropertyChangedEventArgs e)
        {
            CollectionItemPropertyChanged?.Invoke(this, e);
        }

        /// <inheritdoc/>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems.OfType<T>())
                    item.PropertyChanged -= Item_PropertyChanged;
            }
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems.OfType<T>())
                    item.PropertyChanged += Item_PropertyChanged;
            }
        }

        /// <inheritdoc/>
        protected override void ClearItems()
        {
            this.ForEach(x => x.PropertyChanged -= Item_PropertyChanged);
            base.ClearItems();
        }
    }

    /// <summary>
    /// The event handler delegate for the <see cref="FullyObservableCollection{T}.CollectionItemPropertyChanged"/> event.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="e">The <see cref="CollectionItemPropertyChangedEventArgs"/> instance containing the event data.</param>
    public delegate void CollectionItemPropertyChangedEventHandler(object? sender, CollectionItemPropertyChangedEventArgs e);
    /// <summary>
    /// The event data for the <see cref="CollectionChangeEventHandler"/> event handler.
    /// </summary>
    /// <seealso cref="EventArgs" />
    public class CollectionItemPropertyChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the index of the item that changed.
        /// </summary>
        /// <value>
        /// The index of the item that changed.
        /// </value>
        public int ItemIndex { get; }
        /// <summary>
        /// Gets the item that changes.
        /// </summary>
        /// <value>
        /// The item that changed.
        /// </value>
        public object Item { get; }
        /// <summary>
        /// Gets the name of the property that changed.
        /// </summary>
        /// <value>
        /// The name of the property that changed.
        /// </value>
        public string? PropertyName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionItemPropertyChangedEventArgs"/> class.
        /// </summary>
        /// <param name="itemIndex">Index of the item that changed.</param>
        /// <param name="item">The item that changed.</param>
        /// <param name="propertyName">Name of the property that changed.</param>
        public CollectionItemPropertyChangedEventArgs(int itemIndex, object item, string? propertyName)
        {
            ItemIndex = itemIndex;
            Item = item;
            PropertyName = propertyName;
        }
    }
}
