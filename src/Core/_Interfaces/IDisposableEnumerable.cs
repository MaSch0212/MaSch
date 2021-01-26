using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MaSch.Common
{
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
        event EventHandler? Disposing;

        /// <summary>
        /// Occurs after the <see cref="IDisposableEnumerable"/> has been disposed.
        /// </summary>
        event EventHandler? Disposed;
    }

    /// <summary>
    /// Exposes a disposable enumerator, which supports a simple iteration over a collection of a specified type.
    /// </summary>
    /// <typeparam name="T">The type of objects to enumerate.</typeparam>
    /// <seealso cref="IEnumerable" />
    /// <seealso cref="IDisposable" />
    public interface IDisposableEnumerable<out T> : IDisposableEnumerable, IEnumerable<T> { }

    /// <summary>
    /// Represents a sorted, disposable sequence.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the sequence.</typeparam>
    /// <seealso cref="IDisposableEnumerable" />
    /// <seealso cref="IOrderedEnumerable{T}" />
    public interface IOrderedDisposableEnumerable<T> : IDisposableEnumerable, IOrderedEnumerable<T> { }
}
