using System;
using System.Diagnostics.CodeAnalysis;

namespace MaSch.Core.Observable.Collections
{
    /// <summary>
    /// The event handler delegate for the <see cref="ObservableDictionary{TKey, TValue}.DictionaryItemChanged"/> event.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="DictionaryItemChangedEventArgs{TKey, TValue}" /> instance containing the event data.</param>
    public delegate void DictionaryItemChangedEventHandler<TKey, TValue>(object sender, DictionaryItemChangedEventArgs<TKey, TValue> e)
        where TKey : notnull;

    /// <summary>
    /// The event data for the <see cref="DictionaryItemChangedEventHandler{TKey, TValue}"/> event handler.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class DictionaryItemChangedEventArgs<TKey, TValue> : EventArgs
        where TKey : notnull
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryItemChangedEventArgs{TKey,TValue}" /> class.
        /// </summary>
        /// <param name="itemIndex">The index of the item that changed.</param>
        /// <param name="key">The key of the item that changed..</param>
        /// <param name="oldValue">The old value of the item that changed.</param>
        /// <param name="newValue">The new value of the item that chanegd.</param>
        public DictionaryItemChangedEventArgs(int itemIndex, TKey key, [AllowNull] TValue oldValue, [AllowNull] TValue newValue)
        {
            ItemIndex = itemIndex;
            Key = key;
            OldValue = oldValue;
            NewValue = newValue;
        }

        /// <summary>
        /// Gets the index of the item that changed.
        /// </summary>
        public int ItemIndex { get; }

        /// <summary>
        /// Gets the key of the item that changed.
        /// </summary>
        public TKey Key { get; }

        /// <summary>
        /// Gets the old value of the item that changed.
        /// </summary>
        [MaybeNull]
        [AllowNull]
        public TValue OldValue { get; }

        /// <summary>
        /// Gets the new value of the item that chanegd.
        /// </summary>
        [MaybeNull]
        [AllowNull]
        public TValue NewValue { get; }
    }
}
