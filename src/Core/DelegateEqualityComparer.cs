﻿namespace MaSch.Core;

/// <summary>
/// Represents a <see cref="IEqualityComparer{T}"/> that uses delegates to compare values.
/// </summary>
/// <typeparam name="T">The type of value to compare.</typeparam>
/// <seealso cref="IEqualityComparer{T}" />
public class DelegateEqualityComparer<T> : IEqualityComparer<T>
{
    private readonly Func<T?, T?, bool> _equalsFunc;
    private readonly Func<T, int> _getHashCodeFunc;

    /// <summary>
    /// Initializes a new instance of the <see cref="DelegateEqualityComparer{T}"/> class.
    /// </summary>
    /// <param name="equalsFunc">The equals function.</param>
    /// <param name="getHashCodeFunc">The get hash code function.</param>
    public DelegateEqualityComparer(Func<T?, T?, bool> equalsFunc, Func<T, int> getHashCodeFunc)
    {
        _ = Guard.NotNull(equalsFunc);
        _ = Guard.NotNull(getHashCodeFunc);

        _equalsFunc = equalsFunc;
        _getHashCodeFunc = getHashCodeFunc;
    }

    /// <inheritdoc/>
    public bool Equals(T? x, T? y)
    {
        return _equalsFunc(x, y);
    }

    /// <inheritdoc/>
    public int GetHashCode(T obj)
    {
        return _getHashCodeFunc(obj);
    }
}
