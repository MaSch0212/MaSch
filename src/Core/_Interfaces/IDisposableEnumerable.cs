﻿namespace MaSch.Core;

/// <summary>
/// Exposes a disposable enumerator, which support a single iteration over a non-generic collection.
/// </summary>
/// <seealso cref="IEnumerable" />
/// <seealso cref="IDisposable" />
public interface IDisposableEnumerable : IEnumerable, IDisposable
{
    /// <summary>
    /// Occurs when the <see cref="IDisposableEnumerable"/> is being disposed.
    /// </summary>
    event EventHandler<DisposeEventArgs>? Disposing;

    /// <summary>
    /// Occurs after the <see cref="IDisposableEnumerable"/> has been disposed.
    /// </summary>
    event EventHandler<DisposeEventArgs>? Disposed;
}

/// <summary>
/// Exposes a disposable enumerator, which supports a simple iteration over a collection of a specified type.
/// </summary>
/// <typeparam name="T">The type of objects to enumerate.</typeparam>
/// <seealso cref="IEnumerable" />
/// <seealso cref="IDisposable" />
public interface IDisposableEnumerable<out T>
    : IDisposableEnumerable, IEnumerable<T>
{
}

/// <summary>
/// Represents a sorted, disposable sequence.
/// </summary>
/// <typeparam name="T">The type of the elements of the sequence.</typeparam>
/// <seealso cref="IDisposableEnumerable" />
/// <seealso cref="IOrderedEnumerable{T}" />
[SuppressMessage("Major Code Smell", "S3246:Generic type parameters should be co/contravariant when possible", Justification = "Can not fix. Will produce CS1961.")]
public interface IOrderedDisposableEnumerable<T>
    : IDisposableEnumerable, IOrderedEnumerable<T>
{
}

/// <summary>
/// Event arguments for events related to <see cref="IDisposable.Dispose"/>.
/// </summary>
/// <seealso cref="EventArgs" />
public class DisposeEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DisposeEventArgs"/> class.
    /// </summary>
    /// <param name="isDisposing">If set to <c>true</c> the object that sent this event is disposing.</param>
    public DisposeEventArgs(bool isDisposing)
    {
        IsDisposing = isDisposing;
    }

    /// <summary>
    /// Gets a value indicating whether the object that sent this event is disposing.
    /// </summary>
    public bool IsDisposing { get; }
}
