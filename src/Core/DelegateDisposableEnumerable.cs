namespace MaSch.Core;

/// <summary>
/// Represents a wrappoer for an <see cref="IEnumerable{T}"/> which executes an action if it is disposed.
/// </summary>
/// <typeparam name="T">The type of the elements in the <see cref="IEnumerable{T}"/>.</typeparam>
/// <seealso cref="IDisposableEnumerable{T}" />
public class DelegateDisposableEnumerable<T> : IDisposableEnumerable<T>
{
    private readonly IEnumerable<T> _enumerable;
    private readonly Action _actionOnDispose;

    /// <summary>
    /// Initializes a new instance of the <see cref="DelegateDisposableEnumerable{T}"/> class.
    /// </summary>
    /// <param name="enumerable">The enumerable to wrap.</param>
    /// <param name="actionOnDispose">The action that is executed on dispose.</param>
    public DelegateDisposableEnumerable(IEnumerable<T> enumerable, Action actionOnDispose)
    {
        _ = Guard.NotNull(enumerable, nameof(enumerable));
        _ = Guard.NotNull(actionOnDispose, nameof(actionOnDispose));

        _enumerable = enumerable;
        _actionOnDispose = actionOnDispose;
    }

    /// <inheritdoc/>
    public event EventHandler<DisposeEventArgs>? Disposing;

    /// <inheritdoc/>
    public event EventHandler<DisposeEventArgs>? Disposed;

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator()
    {
        return _enumerable.GetEnumerator();
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_enumerable).GetEnumerator();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Disposing?.Invoke(this, new DisposeEventArgs(disposing));
            _actionOnDispose();
            Disposed?.Invoke(this, new DisposeEventArgs(disposing));
        }
    }
}

/// <summary>
/// Represents a wrappoer for an <see cref="IOrderedDisposableEnumerable{T}"/> which executes an action if it is disposed.
/// </summary>
/// <typeparam name="T">The type of the elements in the <see cref="IOrderedDisposableEnumerable{T}"/>.</typeparam>
/// <seealso cref="IOrderedDisposableEnumerable{T}" />
public class DelegateOrderedDisposableEnumerable<T> : IOrderedDisposableEnumerable<T>
{
    private readonly IOrderedEnumerable<T> _enumerable;
    private readonly Action _actionOnDispose;

    /// <summary>
    /// Initializes a new instance of the <see cref="DelegateOrderedDisposableEnumerable{T}"/> class.
    /// </summary>
    /// <param name="enumerable">The enumerable to wrap.</param>
    /// <param name="actionOnDispose">The action that is executed on dispose.</param>
    public DelegateOrderedDisposableEnumerable(IOrderedEnumerable<T> enumerable, Action actionOnDispose)
    {
        _ = Guard.NotNull(enumerable, nameof(enumerable));
        _ = Guard.NotNull(actionOnDispose, nameof(actionOnDispose));

        _enumerable = enumerable;
        _actionOnDispose = actionOnDispose;
    }

    /// <inheritdoc/>
    public event EventHandler<DisposeEventArgs>? Disposing;

    /// <inheritdoc/>
    public event EventHandler<DisposeEventArgs>? Disposed;

    /// <inheritdoc/>
    public IOrderedEnumerable<T> CreateOrderedEnumerable<TKey>(Func<T, TKey> keySelector, IComparer<TKey>? comparer, bool @descending)
    {
        return _enumerable.CreateOrderedEnumerable(keySelector, comparer, @descending);
    }

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator()
    {
        return _enumerable.GetEnumerator();
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_enumerable).GetEnumerator();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Disposing?.Invoke(this, new DisposeEventArgs(disposing));
            _actionOnDispose();
            Disposed?.Invoke(this, new DisposeEventArgs(disposing));
        }
    }
}
