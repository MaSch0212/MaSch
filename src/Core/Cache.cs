﻿namespace MaSch.Core;

/// <summary>
/// Represents a cache that stores runtime information.
/// </summary>
/// <seealso cref="ICache" />
public class Cache : ICache
{
    private bool _isDisposed;

    /// <summary>
    /// Gets an object that is used for locking the <see cref="Objects"/> object.
    /// </summary>
    protected object ObjectsLock { get; } = new object();

    /// <summary>
    /// Gets the objects in this <see cref="Cache"/>.
    /// </summary>
    protected IDictionary<string, object?> Objects { get; } = new Dictionary<string, object?>();

    /// <summary>
    /// Gets a value of a specified key.
    /// </summary>
    /// <typeparam name="T">The type of the value to get.</typeparam>
    /// <param name="key">The key to get the value from.</param>
    /// <returns>
    /// The value of the <paramref name="key" /> typed as <typeparamref name="T" />.
    /// </returns>
    public virtual T? GetValue<T>([CallerMemberName] string key = "<Unknown>")
    {
        _ = Guard.NotNull(key);
        lock (ObjectsLock)
            return (T?)Objects[key];
    }

    /// <summary>
    /// Gets a value of a specified key. If no value exists a specified getter is used to insert the value to the <see cref="T:MaSch.Core.ICache" /> and return it.
    /// </summary>
    /// <typeparam name="T">The type of the value to get.</typeparam>
    /// <param name="valueGetter">The getter which is used if no value for the key exists in the <see cref="T:MaSch.Core.ICache" />.</param>
    /// <param name="key">The key to get the value from.</param>
    /// <returns>
    /// The value of the <paramref name="key" /> typed as <typeparamref name="T" />.
    /// </returns>
    public virtual T? GetValue<T>(Func<T> valueGetter, [CallerMemberName] string key = "<Unknown>")
    {
        _ = Guard.NotNull(valueGetter);
        _ = Guard.NotNull(key);

        lock (ObjectsLock)
        {
            if (!Objects.ContainsKey(key))
                Objects[key] = valueGetter();
            return (T?)Objects[key];
        }
    }

    /// <summary>
    /// Gets a value of a specified key. If no value exists a specified getter is used to insert the value to the <see cref="T:MaSch.Core.ICache" /> and return it.
    /// </summary>
    /// <typeparam name="T">The type of the value to get.</typeparam>
    /// <param name="valueGetter">The getter which is used if no value for the key exists in the <see cref="T:MaSch.Core.ICache" />.</param>
    /// <param name="key">The key to get the value from.</param>
    /// <returns>
    /// The value of the <paramref name="key" /> typed as <typeparamref name="T" />.
    /// </returns>
    public virtual async Task<T?> GetValueAsync<T>(Func<Task<T>> valueGetter, [CallerMemberName] string key = "<Unknown>")
    {
        _ = Guard.NotNull(valueGetter);
        _ = Guard.NotNull(key);

        Monitor.Enter(ObjectsLock);
        try
        {
            if (!Objects.ContainsKey(key))
                Objects[key] = await valueGetter();
            return (T?)Objects[key];
        }
        finally
        {
            Monitor.Exit(ObjectsLock);
        }
    }

    /// <summary>
    /// Tries to get the value of a specified key.
    /// </summary>
    /// <typeparam name="T">The type of the value to get.</typeparam>
    /// <param name="value">The value of the <paramref name="key" /> types as <typeparamref name="T" /> if it exists; otherwise <c>default(T)</c>.</param>
    /// <param name="key">The key to get the value from.</param>
    /// <returns>
    ///   <c>true</c> if a value exists for the <paramref name="key" />; otherwise <c>false</c>.
    /// </returns>
    public virtual bool TryGetValue<T>([NotNullWhen(true)] out T? value, [CallerMemberName] string key = "<Unknown>")
    {
        _ = Guard.NotNull(key);

        bool result;
        object? objValue;
        lock (ObjectsLock)
            result = Objects.TryGetValue(key, out objValue);

        if (result && objValue is T castedValue)
        {
            value = castedValue;
            return true;
        }

        value = default;
        return false;
    }

    /// <summary>
    /// Determines whether a value exists for a specific key.
    /// </summary>
    /// <param name="key">The key to check for a value.</param>
    /// <returns>
    /// A value indicating whether a value exists for the specified <paramref name="key" />.
    /// </returns>
    public virtual bool HasValue([CallerMemberName] string key = "<Unknown>")
    {
        _ = Guard.NotNull(key);
        lock (ObjectsLock)
            return Objects.ContainsKey(key);
    }

    /// <summary>
    /// Removes the value of a specified key if one exists.
    /// </summary>
    /// <param name="key">The key for which the value should be removed.</param>
    public virtual void RemoveValue([CallerMemberName] string key = "<Unknown>")
    {
        _ = Guard.NotNull(key);
        lock (ObjectsLock)
            _ = Objects.TryRemove(key);
    }

    /// <summary>
    /// Removes and disposes the value of a specified key if one exists.
    /// </summary>
    /// <param name="key">The key for which the value should be removed.</param>
    /// <remarks>
    /// Dispose is executed only if the value is of type <see cref="T:System.IDisposable" />; otherwise the value is just removed from the cache.
    /// </remarks>
    public virtual void RemoveAndDisposeValue([CallerMemberName] string key = "<Unknown>")
    {
        _ = Guard.NotNull(key);

        object? value;
        lock (ObjectsLock)
            _ = Objects.TryRemove(key, out value);

        if (value is IDisposable disposableValue)
            disposableValue.Dispose();
    }

    /// <summary>
    /// Sets the value for a specific key on this <see cref="T:MaSch.Core.ICache" />.
    /// </summary>
    /// <typeparam name="T">The type of the value to set.</typeparam>
    /// <param name="value">The value to set.</param>
    /// <param name="key">The key of the value to set.</param>
    public virtual void SetValue<T>(T value, [CallerMemberName] string key = "<Unknown>")
    {
        _ = Guard.NotNull(key);
        lock (ObjectsLock)
            Objects[key] = value;
    }

    /// <summary>
    /// Clears all data from the <see cref="T:MaSch.Core.ICache" />.
    /// </summary>
    public virtual void Clear()
    {
        lock (ObjectsLock)
            Objects.Clear();
    }

    /// <summary>
    /// Clears and disposes all data from the <see cref="T:MaSch.Core.ICache" />.
    /// </summary>
    /// <remarks>
    /// Dispose is executed only if the value is of type <see cref="T:System.IDisposable" />; otherwise the value is just removed from the cache.
    /// </remarks>
    public virtual void ClearAndDispose()
    {
        IEnumerable<IDisposable> toDispose;

        lock (ObjectsLock)
        {
            toDispose = Objects.Values.OfType<IDisposable>().ToList();
            Objects.Clear();
        }

        toDispose.ForEach(x => x.Dispose());
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Dispose(true);
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed)
            return;

        if (disposing)
        {
            lock (ObjectsLock)
            {
                Objects.Values.OfType<IDisposable>().ForEach(x => x.Dispose());
                Objects.Clear();
            }
        }

        _isDisposed = true;
    }
}
