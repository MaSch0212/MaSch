using System.Collections.Specialized;

namespace MaSch.Core.Extensions;

/// <summary>
/// Provides extension methods for <see cref="ICollection{T}"/> and <see cref="StringCollection"/>.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Adds items to the <see cref="ICollection{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the <see cref="ICollection{T}"/>.</typeparam>
    /// <param name="collection">The <see cref="ICollection{T}"/> to add the items to.</param>
    /// <param name="items">The objects to add to the <see cref="ICollection{T}"/>.</param>
    /// <exception cref="NotSupportedException"><see cref="ICollection{T}"/> is read-only.</exception>
    public static void Add<T>(this ICollection<T> collection, params T[] items)
    {
        Add(collection, (IEnumerable<T>)items);
    }

    /// <summary>
    /// Adds items to the <see cref="ICollection{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the <see cref="ICollection{T}"/>.</typeparam>
    /// <param name="collection">The <see cref="ICollection{T}"/> to add the items to.</param>
    /// <param name="items">The objects to add to the <see cref="ICollection{T}"/>.</param>
    /// <exception cref="NotSupportedException"><see cref="ICollection{T}"/> is read-only.</exception>
    public static void Add<T>(this ICollection<T> collection, IEnumerable<T> items)
    {
        _ = Guard.NotNull(collection);
        _ = Guard.NotNull(items);

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
        _ = Guard.NotNull(collection);
        _ = Guard.NotNull(items);

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
        _ = Guard.NotNull(collection);

        return AddIfNotExistsImpl(collection, itemToAdd);
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
        _ = Guard.NotNull(collection);
        _ = Guard.NotNull(items);

        return items.Count(x => AddIfNotExistsImpl(collection, x));
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
        _ = Guard.NotNull(collection);

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
        _ = Guard.NotNull(collection);
        _ = Guard.NotNull(comparer);

        return TryRemoveImpl(collection, itemToRemove, comparer);
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
        _ = Guard.NotNull(collection);

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
        _ = Guard.NotNull(collection);
        _ = Guard.NotNull(comparer);

        return items.Count(x => TryRemoveImpl(collection, x, comparer));
    }

    /// <summary>
    /// Removes items from the <see cref="ICollection{T}"/> that matches a given condition.
    /// </summary>
    /// <typeparam name="T">The type of elements in the <see cref="ICollection{T}"/>.</typeparam>
    /// <param name="collection">The <see cref="ICollection{T}"/> to remove the items from.</param>
    /// <param name="condition">The condition that determines which items are to be removed from the <see cref="ICollection{T}"/>.</param>
    /// <returns>Return the count of removed items.</returns>
    [SuppressMessage("Major Code Smell", "S2971:\"IEnumerable\" LINQs should be simplified", Justification = "ToArray call is needed due to removal of elements in Count method.")]
    public static int RemoveWhere<T>(this ICollection<T> collection, Func<T, bool> condition)
    {
        _ = Guard.NotNull(collection);
        _ = Guard.NotNull(condition);

        return collection.Where(condition).ToArray().Count(x => TryRemoveImpl(collection, x));
    }

    /// <summary>
    /// Converts the <see cref="IEnumerable{T}"/> to a <see cref="StringCollection"/>.
    /// </summary>
    /// <param name="enumerable">The <see cref="IEnumerable{T}"/> to convert.</param>
    /// <returns>Returns a <see cref="StringCollection"/> with the content of the <see cref="IEnumerable{T}"/>.</returns>
    public static StringCollection ToStringCollection(this IEnumerable<string> enumerable)
    {
        _ = Guard.NotNull(enumerable);

        var result = new StringCollection();
        foreach (var i in enumerable)
            _ = result.Add(i);
        return result;
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

    private static bool TryRemoveImpl<T>(ICollection<T> collection, T itemToRemove)
    {
        if (collection.Contains(itemToRemove))
        {
            _ = collection.Remove(itemToRemove);
            return true;
        }

        return false;
    }

    private static bool TryRemoveImpl<T>(ICollection<T> collection, T itemToRemove, IEqualityComparer<T> comparer)
    {
        if (collection.TryFirst(x => comparer.Equals(x, itemToRemove), out T? element))
        {
            _ = collection.Remove(element);
            return true;
        }

        return false;
    }
}
