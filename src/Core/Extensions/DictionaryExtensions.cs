﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MaSch.Common.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="IDictionary{TKey,TValue}"/> and <see cref="IDictionary"/>.
    /// </summary>
    public static class DictionaryExtensions
    {
        #region IDictionary<TKey, TValue> extensions
        /// <summary>
        /// Tries to get the value for a given key from the <see cref="IDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <param name="dict">The <see cref="IDictionary{TKey,TValue}"/> to optain the value.</param>
        /// <param name="key">The key.</param>
        /// <returns>Return the value for the key if the key exists in the <see cref="IDictionary{TKey,TValue}"/>; otherwise <see langword="null"/></returns>
        public static TValue? TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key) where TKey : notnull
        {
            Guard.NotNull(dict, nameof(dict));

            return dict.ContainsKey(key) ? dict[key] : default;
        }

        /// <summary>
        /// Tries to get the value for a given key from the <see cref="IDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <param name="dict">The <see cref="IDictionary{TKey,TValue}"/> to optain the value.</param>
        /// <param name="key">The key.</param>
        /// <param name="keyFound">Determines if the key was found.</param>
        /// <returns>Return the value for the key if the key exists in the <see cref="IDictionary{TKey,TValue}"/>; otherwise <see langword="null"/></returns>
        public static TValue? TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, out bool keyFound) where TKey : notnull
        {
            Guard.NotNull(dict, nameof(dict));

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
        /// <param name="dict">The <see cref="IDictionary{TKey,TValue}"/> from which the keys should be removed.</param>
        /// <param name="keysToRemove">The keys to remove from the <see cref="IDictionary{TKey,TValue}"/>.</param>
        /// <returns>Returns bool values determining which elements were removed successfully.</returns>
        public static bool[] TryRemove<TKey, TValue>(this IDictionary<TKey, TValue> dict, params TKey[] keysToRemove) where TKey : notnull
        {
            Guard.NotNull(dict, nameof(dict));
            Guard.NotNull(keysToRemove, nameof(keysToRemove));

            var result = new bool[keysToRemove.Length];
            for (int i = 0; i < keysToRemove.Length; i++)
            {
                if (dict.ContainsKey(keysToRemove[i]))
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
        /// <returns><code>true</code> if an element has been removed; otherwise <code>false</code>.</returns>
        public static bool TryRemove<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey keyToRemove, [MaybeNullWhen(false)] out TValue value) where TKey : notnull
        {
            Guard.NotNull(dict, nameof(dict));

            if (dict.ContainsKey(keyToRemove))
            {
                value = dict[keyToRemove];
                dict.Remove(keyToRemove);
                return true;
            }

            value = default;
            return false;
        }

        /// <summary>
        /// Adds the specified value if the key does not already exists in the <see cref="IDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <param name="dict">The <see cref="IDictionary{TKey,TValue}"/> to add the value to.</param>
        /// <param name="key">The key which to check.</param>
        /// <param name="value">The value to add.</param>
        /// <returns>Returns true, if the value was added to the <see cref="IDictionary{TKey,TValue}"/>; otherwise false.</returns>
        public static bool AddIfNotExists<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value) where TKey : notnull
        {
            Guard.NotNull(dict, nameof(dict));

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
        public static void Add<TKey, TValue, TItem>(this IDictionary<TKey, TValue> dict, IEnumerable<TItem> itemsToAdd, Func<TItem, TKey> keySelector) where TItem : TValue where TKey : notnull
            => Add(dict, itemsToAdd, keySelector, x => x);

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
        public static void Add<TKey, TValue, TItem>(this IDictionary<TKey, TValue> dict, IEnumerable<TItem> itemsToAdd, Func<TItem, TKey> keySelector, Func<TItem, TValue> elementSelector) where TKey : notnull
        {
            Guard.NotNull(dict, nameof(dict));
            Guard.NotNull(itemsToAdd, nameof(itemsToAdd));
            Guard.NotNull(keySelector, nameof(keySelector));
            Guard.NotNull(elementSelector, nameof(elementSelector));

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
        public static void AddToList<TKey, TValue>(this IDictionary<TKey, IList<TValue>> dict, TKey key, TValue valueToAdd) where TKey : notnull
        {
            Guard.NotNull(dict, nameof(dict));

            if (!dict.ContainsKey(key))
                dict.Add(key, new List<TValue>());
            dict[key].Add(valueToAdd);
        }
        #endregion

        #region IDictionary extensions
        /// <summary>
        /// Tries to get the value for a given key from the <see cref="IDictionary"/>.
        /// </summary>
        /// <param name="dict">The <see cref="IDictionary"/> to optain the value.</param>
        /// <param name="key">The key.</param>
        /// <returns>Return the value for the key if the key exists in the <see cref="IDictionary"/>; otherwise <see langword="null"/></returns>
        public static object? TryGetValue(this IDictionary dict, object key)
        {
            Guard.NotNull(dict, nameof(dict));

            return dict.Contains(key) ? dict[key] : null;
        }

        /// <summary>
        /// Removes keys from the <see cref="IDictionary"/> if they exists.
        /// </summary>
        /// <param name="dict">The <see cref="IDictionary"/> from which the keys should be removed.</param>
        /// <param name="keysToRemove">The keys to remove from the <see cref="IDictionary"/>.</param>
        /// <returns>Returns bool values determining which elements were removed successfully.</returns>
        public static bool[] TryRemove(this IDictionary dict, params object[] keysToRemove)
        {
            Guard.NotNull(dict, nameof(dict));
            Guard.NotNull(keysToRemove, nameof(keysToRemove));

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
        #endregion
    }
}
