using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace MaSch.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="ICollection{T}"/>, <see cref="IList{T}"/> and <see cref="IList"/>.
    /// </summary>
    public static class CollectionExtensions
    {
        #region ICollection<T> Extensions

        /// <summary>
        /// Adds items to the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <see cref="ICollection{T}"/>.</typeparam>
        /// <param name="collection">The <see cref="ICollection{T}"/> to add the items to.</param>
        /// <param name="items">The objects to add to the <see cref="ICollection{T}"/>.</param>
        /// <exception cref="NotSupportedException"><see cref="ICollection{T}"/> is read-only.</exception>
        public static void Add<T>(this ICollection<T> collection, params T[] items) => Add(collection, (IEnumerable<T>)items);

        /// <summary>
        /// Adds items to the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <see cref="ICollection{T}"/>.</typeparam>
        /// <param name="collection">The <see cref="ICollection{T}"/> to add the items to.</param>
        /// <param name="items">The objects to add to the <see cref="ICollection{T}"/>.</param>
        /// <exception cref="NotSupportedException"><see cref="ICollection{T}"/> is read-only.</exception>
        public static void Add<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            Guard.NotNull(collection, nameof(collection));
            Guard.NotNull(items, nameof(items));

            foreach (var item in items)
            {
                collection.Add(item);
            }
        }

        /// <summary>
        /// Sets the content of the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <see cref="ICollection{T}"/>.</typeparam>
        /// <param name="collection">The <see cref="ICollection{T}"/> the set the data to.</param>
        /// <param name="items">The objects to set to the <see cref="ICollection{T}"/>.</param>
        /// <exception cref="NotSupportedException"><see cref="ICollection{T}"/> is read-only.</exception>
        public static void Set<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            Guard.NotNull(collection, nameof(collection));
            Guard.NotNull(items, nameof(items));

            collection.Clear();
            collection.Add(items);
        }

        /// <summary>
        /// Adds an item to the <see cref="ICollection{T}"/> if it does not exist in the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <see cref="ICollection{T}"/>.</typeparam>
        /// <param name="collection">The <see cref="ICollection{T}"/> to add the item to.</param>
        /// <param name="itemToAdd">The object to add to the <see cref="ICollection{T}"/>.</param>
        /// <returns>Return true if the item was added; otherwise false.</returns>
        public static bool AddIfNotExists<T>(this ICollection<T> collection, T itemToAdd)
        {
            Guard.NotNull(collection, nameof(collection));

            return AddIfNotExistsImpl(collection, itemToAdd);
        }

        private static bool AddIfNotExistsImpl<T>(ICollection<T> collection, T itemToAdd)
        {
            if (!collection.Contains(itemToAdd))
            {
                collection.Add(itemToAdd);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds items to the <see cref="ICollection{T}"/> if they does not exist in the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <see cref="ICollection{T}"/>.</typeparam>
        /// <param name="collection">The <see cref="ICollection{T}"/> to add the items to.</param>
        /// <param name="items">The objects to add to the <see cref="ICollection{T}"/>.</param>
        /// <returns>Return the count of added items.</returns>
        public static int AddIfNotExists<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            Guard.NotNull(collection, nameof(collection));
            Guard.NotNull(items, nameof(items));

            return items.Count(x => AddIfNotExists(collection, x));
        }

        /// <summary>
        /// Removes an item from the <see cref="ICollection{T}"/> if it exists in the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <see cref="ICollection{T}"/>.</typeparam>
        /// <param name="collection">The <see cref="ICollection{T}"/> to remove the item from.</param>
        /// <param name="itemToRemove">The object to remove from the <see cref="ICollection{T}"/>.</param>
        /// <returns>Returns true if the item was removed; otherwise false.</returns>
        public static bool TryRemove<T>(this ICollection<T> collection, T itemToRemove)
        {
            Guard.NotNull(collection, nameof(collection));

            return TryRemoveImpl(collection, itemToRemove);
        }

        /// <summary>
        /// Removes an item from the <see cref="ICollection{T}"/> if it exists in the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <see cref="ICollection{T}"/>.</typeparam>
        /// <param name="collection">The <see cref="ICollection{T}"/> to remove the item from.</param>
        /// <param name="itemToRemove">The object to remove from the <see cref="ICollection{T}"/>.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> that should be used for comparison.</param>
        /// <returns>Returns true if the item was removed; otherwise false.</returns>
        public static bool TryRemove<T>(this ICollection<T> collection, T itemToRemove, IEqualityComparer<T> comparer)
        {
            Guard.NotNull(collection, nameof(collection));
            Guard.NotNull(comparer, nameof(comparer));

            return TryRemoveImpl(collection, itemToRemove, comparer);
        }

        private static bool TryRemoveImpl<T>(ICollection<T> collection, T itemToRemove)
        {
            if (collection.Contains(itemToRemove))
            {
                collection.Remove(itemToRemove);
                return true;
            }

            return false;
        }

        private static bool TryRemoveImpl<T>(ICollection<T> collection, T itemToRemove, IEqualityComparer<T> comparer)
        {
            foreach (var element in collection)
            {
                if (comparer.Equals(element, itemToRemove))
                {
                    collection.Remove(element);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Removes items from the <see cref="ICollection{T}"/> if they exists in the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <see cref="ICollection{T}"/>.</typeparam>
        /// <param name="collection">The <see cref="ICollection{T}"/> to remove the items from.</param>
        /// <param name="items">The objects to remove from the <see cref="ICollection{T}"/>.</param>
        /// <returns>Return the count of removed items.</returns>
        public static int Remove<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            Guard.NotNull(collection, nameof(collection));

            return items.Count(x => TryRemoveImpl(collection, x));
        }

        /// <summary>
        /// Removes items from the <see cref="ICollection{T}"/> if they exists in the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <see cref="ICollection{T}"/>.</typeparam>
        /// <param name="collection">The <see cref="ICollection{T}"/> to remove the items from.</param>
        /// <param name="items">The objects to remove from the <see cref="ICollection{T}"/>.</param>
        /// <param name="comparer">The comparer to use.</param>
        /// <returns>Return the count of removed items.</returns>
        public static int Remove<T>(this ICollection<T> collection, IEnumerable<T> items, IEqualityComparer<T> comparer)
        {
            Guard.NotNull(collection, nameof(collection));
            Guard.NotNull(comparer, nameof(comparer));

            return items.Count(x => TryRemoveImpl(collection, x, comparer));
        }

        /// <summary>
        /// Removes items from the <see cref="ICollection{T}"/> that matches a given condition.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <see cref="ICollection{T}"/>.</typeparam>
        /// <param name="collection">The <see cref="ICollection{T}"/> to remove the items from.</param>
        /// <param name="condition">The condition that determines which items are to be removed from the <see cref="ICollection{T}"/>.</param>
        /// <returns>Return the count of removed items.</returns>
        public static int RemoveWhere<T>(this ICollection<T> collection, Func<T, bool> condition)
        {
            Guard.NotNull(collection, nameof(collection));
            Guard.NotNull(condition, nameof(condition));

            return collection.Where(condition).ToArray().Count(x => TryRemoveImpl(collection, x));
        }

        #endregion

        #region IList<T> Extensions

        /// <summary>
        /// Removes an item from the <see cref="IList{T}"/> if it exists in the <see cref="IList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <see cref="IList{T}"/>.</typeparam>
        /// <param name="list">The <see cref="IList{T}"/> to remove the item from.</param>
        /// <param name="itemToRemove">The object to remove from the <see cref="IList{T}"/>.</param>
        /// <returns>Returns true if the item was removed; otherwise false.</returns>
        public static bool TryRemove<T>(this IList<T> list, T itemToRemove)
        {
            Guard.NotNull(list, nameof(list));

            return TryRemoveImpl(list, itemToRemove);
        }

        private static bool TryRemoveImpl<T>(IList<T> list, T itemToRemove)
        {
            var index = list.IndexOf(itemToRemove);
            if (index >= 0)
                list.RemoveAt(index);
            return index >= 0;
        }

        /// <summary>
        /// Removes items from the <see cref="IList{T}"/> if they exists in the <see cref="IList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <see cref="IList{T}"/>.</typeparam>
        /// <param name="list">The <see cref="IList{T}"/> to remove the items from.</param>
        /// <param name="items">The objects to remove from the <see cref="IList{T}"/>.</param>
        /// <returns>Return the count of removed items.</returns>
        public static int Remove<T>(this IList<T> list, IEnumerable<T> items)
        {
            Guard.NotNull(list, nameof(list));
            Guard.NotNull(items, nameof(items));

            return items.Count(x => TryRemoveImpl(list, x));
        }

        /// <summary>
        /// Removes items from the <see cref="IList{T}"/> if they exists in the <see cref="IList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <see cref="IList{T}"/>.</typeparam>
        /// <param name="list">The <see cref="IList{T}"/> to remove the items from.</param>
        /// <param name="items">The objects to remove from the <see cref="IList{T}"/>.</param>
        /// <param name="removedItems">The items that were deleted from the collection.</param>
        /// <returns>Return the count of removed items.</returns>
        public static int Remove<T>(this IList<T> list, IEnumerable<T> items, out IList<T> removedItems)
        {
            Guard.NotNull(list, nameof(list));
            Guard.NotNull(items, nameof(items));

            removedItems = items.Where(x => TryRemoveImpl(list, x)).ToList();
            return removedItems.Count;
        }

        /// <summary>
        /// Removes items from the <see cref="IList{T}"/> that matches a given condition.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <see cref="IList{T}"/>.</typeparam>
        /// <param name="list">The <see cref="IList{T}"/> to remove the items from.</param>
        /// <param name="condition">The condition that determines which items are to be removed from the <see cref="IList{T}"/>.</param>
        /// <returns>Return the count of removed items.</returns>
        public static int RemoveWhere<T>(this IList<T> list, Func<T, bool> condition)
        {
            Guard.NotNull(list, nameof(list));
            Guard.NotNull(condition, nameof(condition));

            return list.Where(condition).ToArray().Count(x => TryRemoveImpl(list, x));
        }

        #endregion

        #region IList Extensions

        /// <summary>
        /// Adds an item to the <see cref="IList"/> if it does not exist in the <see cref="IList"/>.
        /// </summary>
        /// <param name="list">The <see cref="IList"/> to add the item to.</param>
        /// <param name="itemToAdd">The object to add to the <see cref="IList"/>.</param>
        /// <returns>Return true if the item was added; otherwise false.</returns>
        public static bool AddIfNotExists(this IList list, object? itemToAdd)
        {
            Guard.NotNull(list, nameof(list));

            return AddIfNotExistsImpl(list, itemToAdd);
        }

        private static bool AddIfNotExistsImpl(IList list, object? itemToAdd)
        {
            if (!list.Contains(itemToAdd))
            {
                list.Add(itemToAdd);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds items to the <see cref="IList"/> if they does not exist in the <see cref="IList"/>.
        /// </summary>
        /// <param name="list">The <see cref="IList"/> to add the items to.</param>
        /// <param name="items">The objects to add to the <see cref="IList"/>.</param>
        /// <returns>Return the count of added items.</returns>
        public static int AddIfNotExists(this IList list, IEnumerable items)
        {
            Guard.NotNull(list, nameof(list));
            Guard.NotNull(items, nameof(items));

            return items.Cast<object?>().Count(x => AddIfNotExistsImpl(list, x));
        }

        /// <summary>
        /// Removes an item from the <see cref="IList"/> if it exists in the <see cref="IList"/>.
        /// </summary>
        /// <param name="list">The <see cref="IList"/> to remove the item from.</param>
        /// <param name="itemToRemove">The object to remove from the <see cref="IList"/>.</param>
        /// <returns>Returns true if the item was removed; otherwise false.</returns>
        public static bool TryRemove(this IList list, object? itemToRemove)
        {
            Guard.NotNull(list, nameof(list));

            return TryRemoveImpl(list, itemToRemove);
        }

        private static bool TryRemoveImpl(IList list, object? itemToRemove)
        {
            var index = list.IndexOf(itemToRemove);
            if (index >= 0)
                list.RemoveAt(index);
            return index >= 0;
        }

        /// <summary>
        /// Removes items from the <see cref="IList"/> if they exists in the <see cref="IList"/>.
        /// </summary>
        /// <param name="list">The <see cref="IList"/> to remove the items from.</param>
        /// <param name="items">The objects to remove from the <see cref="IList"/>.</param>
        /// <returns>Return the count of removed items.</returns>
        public static int Remove(this IList list, IEnumerable items)
        {
            Guard.NotNull(list, nameof(list));
            Guard.NotNull(items, nameof(items));

            return items.Cast<object?>().Count(x => TryRemoveImpl(list, x));
        }

        /// <summary>
        /// Removes items from the <see cref="IList"/> that matches a given condition.
        /// </summary>
        /// <param name="list">The <see cref="IList"/> to remove the items from.</param>
        /// <param name="condition">The condition that determines which items are to be removed from the <see cref="IList"/>.</param>
        /// <returns>Return the count of removed items.</returns>
        public static int RemoveWhere(this IList list, Func<object?, bool> condition)
        {
            Guard.NotNull(list, nameof(list));
            Guard.NotNull(condition, nameof(condition));

            return list.Cast<object?>().Where(condition).ToArray().Count(x => TryRemoveImpl(list, x));
        }

        #endregion

        #region Special Types

        /// <summary>
        /// Converts the <see cref="IEnumerable{T}"/> to a <see cref="StringCollection"/>.
        /// </summary>
        /// <param name="enumerable">The <see cref="IEnumerable{T}"/> to convert.</param>
        /// <returns>Returns a <see cref="StringCollection"/> with the content of the <see cref="IEnumerable{T}"/>.</returns>
        public static StringCollection ToStringCollection(this IEnumerable<string> enumerable)
        {
            Guard.NotNull(enumerable, nameof(enumerable));

            var result = new StringCollection();
            foreach (var i in enumerable)
                result.Add(i);
            return result;
        }

        #endregion
    }
}
