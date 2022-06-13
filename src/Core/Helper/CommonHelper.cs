namespace MaSch.Core.Helper;

/// <summary>
/// Provides common helper methods.
/// </summary>
public static class CommonHelper
{
    /// <summary>
    /// Returns a combined hash code for the specified elements.
    /// </summary>
    /// <param name="values">The values to create a combined hash code of.</param>
    /// <returns>
    /// A combined hash code for the <paramref name="values"/>, suitable for use in hashing algorithms and data structures like a hash table.
    /// </returns>
    public static int GetHashCode(params object?[]? values)
    {
        if (values.IsNullOrEmpty())
            return 0;
        int hashcode = values[0]?.GetHashCode() ?? 0;
        unchecked
        {
            for (int i = 1; i < values.Length; i++)
                hashcode = (hashcode * 397) ^ (values[i]?.GetHashCode() ?? 0);
        }

        return hashcode;
    }

    /// <summary>
    /// Returns a combined hash code for the specified elements.
    /// </summary>
    /// <param name="values">The values to create a combined hash code of.</param>
    /// <returns>
    /// A combined hash code for the <paramref name="values"/>, suitable for use in hashing algorithms and data structures like a hash table.
    /// </returns>
    public static int GetHashCode(IEnumerable<object>? values)
    {
        return GetHashCode(values?.ToArray());
    }

    /// <summary>
    /// Merges the specified objects (includes flattening nested lists).
    /// </summary>
    /// <typeparam name="T">The type of the resulting elements.</typeparam>
    /// <param name="objects">The objects to merge.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains all elements of type <typeparamref name="T"/> of <paramref name="objects"/>.</returns>
    /// <exception cref="InvalidCastException">
    ///     An element is not of type <typeparamref name="T"/> or any <see cref="IEnumerable"/>.
    /// </exception>
    public static IEnumerable<T?> Merge<T>(params object?[] objects)
    {
        return Merge<T>(false, objects);
    }

    /// <summary>
    /// Merges the specified objects (includes flattening nested lists).
    /// </summary>
    /// <typeparam name="T">The type of the resulting elements.</typeparam>
    /// <param name="ignoreInvalidObjects">Determines wether to ignore elements that are not castable to <typeparamref name="T"/>.</param>
    /// <param name="objects">The objects to merge.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains all elements of type <typeparamref name="T"/> of <paramref name="objects"/>.</returns>
    /// <exception cref="InvalidCastException">
    ///     An element is not of type <typeparamref name="T"/> or any <see cref="IEnumerable"/> and <paramref name="ignoreInvalidObjects"/> is set to <see langword="false"/>.
    /// </exception>
    public static IEnumerable<T?> Merge<T>(bool ignoreInvalidObjects, params object?[] objects)
    {
        _ = Guard.NotNull(objects);
        foreach (var o in objects)
        {
            if (Equals(o, default(T)))
            {
                yield return default;
            }
            else if (o is T t)
            {
                yield return t;
            }
            else if (o is IEnumerable enumerable)
            {
                foreach (var item in Merge<T>(enumerable.OfType<object>().ToArray()))
                    yield return item;
            }
            else if (!ignoreInvalidObjects)
            {
                throw new InvalidCastException($"Cannot convert type '{o?.GetType().FullName}' to '{typeof(T).FullName}'");
            }
        }
    }

    /// <summary>
    /// Swaps the specified values.
    /// </summary>
    /// <typeparam name="T">The type of the values.</typeparam>
    /// <param name="a">The first value.</param>
    /// <param name="b">The second value.</param>
    public static void Swap<T>(ref T a, ref T b)
    {
        var tmp = a;
        a = b;
        b = tmp;
    }
}
