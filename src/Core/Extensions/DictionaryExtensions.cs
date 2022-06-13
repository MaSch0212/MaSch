using MaSch.Core.Observable.Collections;

namespace MaSch.Core.Extensions;

/// <summary>
/// Provides extension methods for <see cref="IDictionary{TKey,TValue}"/> and <see cref="IDictionary"/>.
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// Tries to get the value for a given key from the <see cref="IDictionary{TKey,TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the dictionaries keys.</typeparam>
    /// <typeparam name="TValue">The type of the dictionaries values.</typeparam>
    /// <param name="dict">The <see cref="IDictionary{TKey,TValue}"/> to optain the value.</param>
    /// <param name="key">The key.</param>
    /// <returns>Return the value for the key if the key exists in the <see cref="IDictionary{TKey,TValue}"/>; otherwise <see langword="null"/>.</returns>
    public static TValue? TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        where TKey : notnull
    {
        _ = Guard.NotNull(dict);

        return dict.ContainsKey(key) ? dict[key] : default;
    }

    /// <summary>
    /// Tries to get the value for a given key from the <see cref="IDictionary"/>.
    /// </summary>
    /// <param name="dict">The <see cref="IDictionary"/> to optain the value.</param>
    /// <param name="key">The key.</param>
    /// <returns>Return the value for the key if the key exists in the <see cref="IDictionary"/>; otherwise <see langword="null"/>.</returns>
    public static object? TryGetValue(this IDictionary dict, object key)
    {
        _ = Guard.NotNull(dict);

        return dict.Contains(key) ? dict[key] : null;
    }

    /// <summary>
    /// Tries to get the value for a given key from the <see cref="IDictionary{TKey,TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the dictionaries keys.</typeparam>
    /// <typeparam name="TValue">The type of the dictionaries values.</typeparam>
    /// <param name="dict">The <see cref="IDictionary{TKey,TValue}"/> to optain the value.</param>
    /// <param name="key">The key.</param>
    /// <param name="keyFound">Determines if the key was found.</param>
    /// <returns>Return the value for the key if the key exists in the <see cref="IDictionary{TKey,TValue}"/>; otherwise <see langword="null"/>.</returns>
    public static TValue? TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, out bool keyFound)
        where TKey : notnull
    {
        _ = Guard.NotNull(dict);

        if (dict.ContainsKey(key))
        {
            keyFound = true;
            return dict[key];
        }

        keyFound = false;
        return default;
    }

    /// <summary>
    /// Removes keys from the <see cref="IDictionary{TKey,TValue}"/> if they exists.
    /// </summary>
    /// <typeparam name="TKey">The type of the dictionaries keys.</typeparam>
    /// <typeparam name="TValue">The type of the dictionaries values.</typeparam>
    /// <param name="dict">The <see cref="IDictionary{TKey,TValue}"/> from which the keys should be removed.</param>
    /// <param name="keysToRemove">The keys to remove from the <see cref="IDictionary{TKey,TValue}"/>.</param>
    /// <returns>Returns bool values determining which elements were removed successfully.</returns>
    public static bool[] TryRemove<TKey, TValue>(this IDictionary<TKey, TValue> dict, params TKey[] keysToRemove)
        where TKey : notnull
    {
        _ = Guard.NotNull(dict);
        _ = Guard.NotNull(keysToRemove);

        var result = new bool[keysToRemove.Length];
        for (int i = 0; i < keysToRemove.Length; i++)
        {
            if (dict.ContainsKey(keysToRemove[i]))
            {
                _ = dict.Remove(keysToRemove[i]);
                result[i] = true;
            }
        }

        return result;
    }

    /// <summary>
    /// Removes keys from the <see cref="IDictionary"/> if they exists.
    /// </summary>
    /// <param name="dict">The <see cref="IDictionary"/> from which the keys should be removed.</param>
    /// <param name="keysToRemove">The keys to remove from the <see cref="IDictionary"/>.</param>
    /// <returns>Returns bool values determining which elements were removed successfully.</returns>
    public static bool[] TryRemove(this IDictionary dict, params object[] keysToRemove)
    {
        _ = Guard.NotNull(dict);
        _ = Guard.NotNull(keysToRemove);

        var result = new bool[keysToRemove.Length];
        for (int i = 0; i < keysToRemove.Length; i++)
        {
            if (dict.Contains(keysToRemove[i]))
            {
                dict.Remove(keysToRemove[i]);
                result[i] = true;
            }
        }

        return result;
    }

    /// <summary>
    /// Removes the element with the specified key from the <see cref="IDictionary{TKey,TValue}"/> if it exists.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="dict">The dictionary.</param>
    /// <param name="keyToRemove">The key to remove.</param>
    /// <param name="value">The value that has been removed.</param>
    /// <returns><c>true</c> if an element has been removed; otherwise, <c>false</c>.</returns>
    public static bool TryRemove<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey keyToRemove, [MaybeNullWhen(false)] out TValue value)
        where TKey : notnull
    {
        _ = Guard.NotNull(dict);

        if (dict.ContainsKey(keyToRemove))
        {
            value = dict[keyToRemove];
            _ = dict.Remove(keyToRemove);
            return true;
        }

        value = default;
        return false;
    }

    /// <summary>
    /// Adds the specified value if the key does not already exists in the <see cref="IDictionary{TKey,TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the dictionaries keys.</typeparam>
    /// <typeparam name="TValue">The type of the dictionaries values.</typeparam>
    /// <param name="dict">The <see cref="IDictionary{TKey,TValue}"/> to add the value to.</param>
    /// <param name="key">The key which to check.</param>
    /// <param name="value">The value to add.</param>
    /// <returns>Returns true, if the value was added to the <see cref="IDictionary{TKey,TValue}"/>; otherwise false.</returns>
    public static bool AddIfNotExists<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value)
        where TKey : notnull
    {
        _ = Guard.NotNull(dict);

        if (dict.ContainsKey(key))
            return false;
        dict.Add(key, value);
        return true;
    }

    /// <summary>
    /// Adds the specified items to the <see cref="IDictionary{TKey, TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <typeparam name="TItem">The type of items to add.</typeparam>
    /// <param name="dict">The dictionary to add the items to.</param>
    /// <param name="itemsToAdd">The items to add.</param>
    /// <param name="keySelector">A function to extract a key from each element.</param>
    public static void Add<TKey, TValue, TItem>(this IDictionary<TKey, TValue> dict, IEnumerable<TItem> itemsToAdd, Func<TItem, TKey> keySelector)
        where TItem : TValue
        where TKey : notnull
    {
        Add(dict, itemsToAdd, keySelector, x => x);
    }

    /// <summary>
    /// Adds the specified items to the <see cref="IDictionary{TKey, TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <typeparam name="TItem">The type of items to add.</typeparam>
    /// <param name="dict">The dictionary to add the items to.</param>
    /// <param name="itemsToAdd">The items to add.</param>
    /// <param name="keySelector">A function to extract a key from each element.</param>
    /// <param name="elementSelector">A transform function to produce a result element value from each element.</param>
    public static void Add<TKey, TValue, TItem>(this IDictionary<TKey, TValue> dict, IEnumerable<TItem> itemsToAdd, Func<TItem, TKey> keySelector, Func<TItem, TValue> elementSelector)
        where TKey : notnull
    {
        _ = Guard.NotNull(dict);
        _ = Guard.NotNull(itemsToAdd);
        _ = Guard.NotNull(keySelector);
        _ = Guard.NotNull(elementSelector);

        foreach (var item in itemsToAdd)
        {
            dict.Add(keySelector(item), elementSelector(item));
        }
    }

    /// <summary>
    /// Adds an element to the list of a dictionary entry.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="dict">The dictionary.</param>
    /// <param name="key">The key.</param>
    /// <param name="valueToAdd">The value to add.</param>
    public static void AddToList<TKey, TValue>(this IDictionary<TKey, IList<TValue>> dict, TKey key, TValue valueToAdd)
        where TKey : notnull
    {
        _ = Guard.NotNull(dict);

        if (!dict.ContainsKey(key))
            dict.Add(key, new List<TValue>());
        dict[key].Add(valueToAdd);
    }

    /// <summary>
    /// Converts the enumerable into an <see cref="ObservableDictionary{TKey, TValue}"/>.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enumerable items.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="enumerable">The enumerable to convert.</param>
    /// <param name="keySelector">The key selector.</param>
    /// <param name="valueSelector">The value selector.</param>
    /// <returns>The converted <see cref="ObservableDictionary{TKey, TValue}"/>.</returns>
    public static ObservableDictionary<TKey, TValue> ToObservableDictionary<TEnum, TKey, TValue>(this IEnumerable<TEnum> enumerable, Func<TEnum, TKey> keySelector, Func<TEnum, TValue> valueSelector)
        where TKey : notnull
    {
        var result = new ObservableDictionary<TKey, TValue>();
        foreach (var item in enumerable)
        {
            result.Add(keySelector(item), valueSelector(item));
        }

        return result;
    }

    /// <summary>
    /// Converts the dictionary into an <see cref="ObservableDictionary{TKey, TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="dict">The dictionary.</param>
    /// <returns>The converted <see cref="ObservableDictionary{TKey, TValue}"/>.</returns>
    public static ObservableDictionary<TKey, TValue> ToObservableDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> dict)
        where TKey : notnull
    {
        return new(dict);
    }
}
