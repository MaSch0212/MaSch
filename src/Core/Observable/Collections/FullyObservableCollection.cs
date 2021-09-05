using MaSch.Core.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace MaSch.Core.Observable.Collections
{
    /// <summary>
    /// Represents a dynamic data collection that provides notifications when items get added, removed, are changed, or when the whole list is refreshed.
    /// </summary>
    /// <typeparam name="T">The type of elements in this <see cref="ObservableCollection{T}"/>.</typeparam>
    /// <seealso cref="ObservableCollection{T}" />
    public class FullyObservableCollection<T> : ObservableCollection<T>
        where T : INotifyPropertyChanged
    {
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
        public FullyObservableCollection(IEnumerable<T> collection)
            : base(collection)
        {
            foreach (var item in this)
                item.PropertyChanged += Item_PropertyChanged;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FullyObservableCollection{T}"/> class that contains elements copied from the specified list.
        /// </summary>
        /// <param name="list">The list from which the elements are copied.</param>
        public FullyObservableCollection(List<T> list)
            : base(list)
        {
            foreach (var item in this)
                item.PropertyChanged += Item_PropertyChanged;
        }

        /// <summary>
        /// Occurs when a property of a item has changed.
        /// </summary>
        public event CollectionItemPropertyChangedEventHandler? CollectionItemPropertyChanged;

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

        private void Item_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is T t)
            {
                var eventArgs = new CollectionItemPropertyChangedEventArgs(IndexOf(t), sender, e.PropertyName);
                OnCollectionItemPropertyChanged(eventArgs);
            }
        }
    }
}
