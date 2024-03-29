﻿using System.Collections.Specialized;
using System.ComponentModel;

namespace MaSch.Core.Observable.Collections;

/// <summary>
/// Represents a generic, observable collection of key/value pairs.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
/// <typeparam name="TValue">The type of the value.</typeparam>
/// <seealso cref="INotifyCollectionChanged" />
/// <seealso cref="INotifyPropertyChanged" />
/// <seealso cref="IDictionary{TKey, TValue}" />
/// <seealso cref="IDictionary" />
/// <seealso cref="IReadOnlyDictionary{TKey, TValue}" />
/// <seealso cref="ISerializable" />
/// <seealso cref="IDeserializationCallback" />
public class ObservableDictionary<TKey, TValue> : INotifyCollectionChanged, INotifyPropertyChanged, IDictionary<TKey, TValue>, IDictionary, IReadOnlyDictionary<TKey, TValue>
    where TKey : notnull
{
    private readonly Dictionary<TKey, TValue> _dictionary;

    /// <summary>
    /// Initializes a new instance of the <see cref="ObservableDictionary{TKey, TValue}"/> class.
    /// </summary>
    public ObservableDictionary()
    {
        _dictionary = new Dictionary<TKey, TValue>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ObservableDictionary{TKey, TValue}"/> class.
    /// </summary>
    /// <param name="enumerable">The enumerable which contains the data to copy to this <see cref="ObservableDictionary{TKey, TValue}"/>.</param>
    public ObservableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> enumerable)
    {
        _dictionary = enumerable.ToDictionary(x => x.Key, x => x.Value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ObservableDictionary{TKey, TValue}"/> class.
    /// </summary>
    /// <param name="dictionary">The dictionary to wrap in this <see cref="ObservableDictionary{TKey, TValue}"/>.</param>
    public ObservableDictionary(Dictionary<TKey, TValue> dictionary)
    {
        _dictionary = dictionary;
    }

    /// <summary>
    /// Occurs when a dictionary item changed.
    /// </summary>
    public event DictionaryItemChangedEventHandler<TKey, TValue>? DictionaryItemChanged;

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <inheritdoc/>
    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    /// <summary>
    /// Gets or sets a value indicating whether the subscribers should be notified on property change.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is notifying subscribers about property changes; otherwise, <c>false</c>.
    /// </value>
    public virtual bool IsNotifyEnabled { get; set; } = true;

    /// <inheritdoc/>
    public ICollection<TKey> Keys => _dictionary.Keys;

    /// <inheritdoc/>
    public ICollection<TValue> Values => _dictionary.Values;

    /// <inheritdoc/>
    ICollection IDictionary.Keys => ((IDictionary)_dictionary).Keys;

    /// <inheritdoc/>
    ICollection IDictionary.Values => ((IDictionary)_dictionary).Values;

    /// <inheritdoc/>
    IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => ((IReadOnlyDictionary<TKey, TValue>)_dictionary).Keys;

    /// <inheritdoc/>
    IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => ((IReadOnlyDictionary<TKey, TValue>)_dictionary).Values;

    /// <inheritdoc/>
    public int Count => _dictionary.Count;

    /// <inheritdoc/>
    bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).IsReadOnly;

    /// <inheritdoc/>
    bool IDictionary.IsReadOnly => ((IDictionary)_dictionary).IsReadOnly;

    /// <inheritdoc/>
    bool IDictionary.IsFixedSize => ((IDictionary)_dictionary).IsFixedSize;

    /// <inheritdoc/>
    object ICollection.SyncRoot => ((ICollection)_dictionary).SyncRoot;

    /// <inheritdoc/>
    bool ICollection.IsSynchronized => ((ICollection)_dictionary).IsSynchronized;

    /// <inheritdoc/>
    public virtual TValue this[TKey key]
    {
        get => _dictionary[key];
        set
        {
            var oldValue = _dictionary.TryGetValue(key);
            _dictionary[key] = value;
            OnDictionaryItemChanged(key, oldValue, value);
        }
    }

    /// <inheritdoc/>
    object? IDictionary.this[object key]
    {
        get => this[(TKey)key];
        set => this[(TKey)key] = (TValue)value!;
    }

    /// <inheritdoc/>
    public virtual void Add(TKey key, [AllowNull] TValue value)
    {
        ((ICollection<KeyValuePair<TKey, TValue>>)this).Add(new KeyValuePair<TKey, TValue>(key, value!));
    }

    /// <inheritdoc/>
    void IDictionary.Add(object key, object? value)
    {
        ((ICollection<KeyValuePair<TKey, TValue>>)this).Add(new KeyValuePair<TKey, TValue>((TKey)key, (TValue)value!));
    }

    /// <inheritdoc/>
    void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
    {
        ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).Add(item);
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new[] { item }));
        CountChanged();
    }

    /// <inheritdoc/>
    public virtual void Clear()
    {
        var oldValues = _dictionary.ToArray();
        _dictionary.Clear();
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset, Array.Empty<object>(), oldValues, 0));
        CountChanged();
    }

    /// <inheritdoc/>
    bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
    {
        return ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).Contains(item);
    }

    /// <inheritdoc/>
    bool IDictionary.Contains(object key)
    {
        return ((IDictionary)_dictionary).Contains(key);
    }

    /// <inheritdoc/>
    public virtual bool ContainsKey(TKey key)
    {
        return _dictionary.ContainsKey(key);
    }

    /// <inheritdoc/>
    void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).CopyTo(array, arrayIndex);
    }

    /// <inheritdoc/>
    void ICollection.CopyTo(Array array, int index)
    {
        ((ICollection)_dictionary).CopyTo(array, index);
    }

    /// <inheritdoc/>
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return _dictionary.GetEnumerator();
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_dictionary).GetEnumerator();
    }

    /// <inheritdoc/>
    IDictionaryEnumerator IDictionary.GetEnumerator()
    {
        return ((IDictionary)_dictionary).GetEnumerator();
    }

    /// <inheritdoc/>
    public virtual bool Remove(TKey key)
    {
        return ((ICollection<KeyValuePair<TKey, TValue>>)this).Remove(_dictionary.FirstOrDefault(x => x.Key.Equals(key)));
    }

    /// <inheritdoc/>
    public void Remove(object key)
    {
        _ = ((ICollection<KeyValuePair<TKey, TValue>>)this).Remove(_dictionary.FirstOrDefault(x => x.Key.Equals(key)));
    }

    /// <inheritdoc/>
    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
    {
        var result = ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).Remove(item);
        if (result)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new[] { item }));
            CountChanged();
        }

        return result;
    }

    /// <inheritdoc/>
    public virtual bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        return _dictionary.TryGetValue(key, out value);
    }

    /// <summary>
    /// Raises the <see cref="PropertyChanged" /> event.
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        if (IsNotifyEnabled)
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Raises the <see cref="CollectionChanged" /> event.
    /// </summary>
    /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs" /> instance containing the event data.</param>
    public virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        if (IsNotifyEnabled)
            CollectionChanged?.Invoke(this, e);
    }

    /// <summary>
    /// Raises the <see cref="DictionaryItemChanged"/> event.
    /// </summary>
    /// <param name="changedKey">The changed key.</param>
    /// <param name="oldValue">The old value.</param>
    /// <param name="newValue">The new value.</param>
    public virtual void OnDictionaryItemChanged(TKey changedKey, TValue? oldValue, TValue? newValue)
    {
        if (IsNotifyEnabled)
        {
            DictionaryItemChanged?.Invoke(this, new DictionaryItemChangedEventArgs<TKey, TValue>(Keys.IndexOf(changedKey), changedKey, oldValue, newValue));
            OnPropertyChanged(nameof(Values));
        }
    }

    private void CountChanged()
    {
        OnPropertyChanged(nameof(Count));
        OnPropertyChanged(nameof(Keys));
        OnPropertyChanged(nameof(Values));
    }
}
