using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MaSch.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="IList{T}"/> and <see cref="IList"/>.
    /// </summary>
    public static class ListExtensions
    {
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
        /// Removes items from the <see cref="IList{T}"/> that matches a given condition.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <see cref="IList{T}"/>.</typeparam>
        /// <param name="list">The <see cref="IList{T}"/> to remove the items from.</param>
        /// <param name="condition">The condition that determines which items are to be removed from the <see cref="IList{T}"/>.</param>
        /// <returns>Return the count of removed items.</returns>
        [SuppressMessage("Major Code Smell", "S2971:\"IEnumerable\" LINQs should be simplified", Justification = "ToArray call is needed due to removal of elements in Count method.")]
        public static int RemoveWhere<T>(this IList<T> list, Func<T, bool> condition)
        {
            Guard.NotNull(list, nameof(list));
            Guard.NotNull(condition, nameof(condition));

            return list.Where(condition).ToArray().Count(x => TryRemoveImpl(list, x));
        }

        /// <summary>
        /// Removes items from the <see cref="IList"/> that matches a given condition.
        /// </summary>
        /// <param name="list">The <see cref="IList"/> to remove the items from.</param>
        /// <param name="condition">The condition that determines which items are to be removed from the <see cref="IList"/>.</param>
        /// <returns>Return the count of removed items.</returns>
        [SuppressMessage("Major Code Smell", "S2971:\"IEnumerable\" LINQs should be simplified", Justification = "ToArray call is needed due to removal of elements in Count method.")]
        public static int RemoveWhere(this IList list, Func<object?, bool> condition)
        {
            Guard.NotNull(list, nameof(list));
            Guard.NotNull(condition, nameof(condition));

            return list.Cast<object?>().Where(condition).ToArray().Count(x => TryRemoveImpl(list, x));
        }

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

        private static bool AddIfNotExistsImpl(IList list, object? itemToAdd)
        {
            if (!list.Contains(itemToAdd))
            {
                list.Add(itemToAdd);
                return true;
            }

            return false;
        }

        private static bool TryRemoveImpl<T>(IList<T> list, T itemToRemove)
        {
            var index = list.IndexOf(itemToRemove);
            if (index >= 0)
                list.RemoveAt(index);
            return index >= 0;
        }

        private static bool TryRemoveImpl(IList list, object? itemToRemove)
        {
            var index = list.IndexOf(itemToRemove);
            if (index >= 0)
                list.RemoveAt(index);
            return index >= 0;
        }
    }
}
