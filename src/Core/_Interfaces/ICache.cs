using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MaSch.Core
{
    /// <summary>
    /// Describes a class that contains cached data.
    /// </summary>
    /// <seealso cref="IDisposable" />
    public interface ICache : IDisposable
    {
        /// <summary>
        /// Sets the value for a specific key on this <see cref="ICache"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value to set.</typeparam>
        /// <param name="value">The value to set.</param>
        /// <param name="key">The key of the value to set.</param>
        void SetValue<T>(T value, [CallerMemberName] string key = "<Unknown>");

        /// <summary>
        /// Determines whether a value exists for a specific key.
        /// </summary>
        /// <param name="key">The key to check for a value.</param>
        /// <returns>A value indicating whether a value exists for the specified <paramref name="key"/>.</returns>
        bool HasValue([CallerMemberName] string key = "<Unknown>");

        /// <summary>
        /// Gets a value of a specified key.
        /// </summary>
        /// <typeparam name="T">The type of the value to get.</typeparam>
        /// <param name="key">The key to get the value from.</param>
        /// <returns>The value of the <paramref name="key"/> typed as <typeparamref name="T"/>.</returns>
        /// <exception cref="KeyNotFoundException">A value for the specified <paramref name="key"/> does not exist in the <see cref="ICache"/>.</exception>
        /// <exception cref="InvalidCastException">The value in the <see cref="ICache"/> can not be cast to type <typeparamref name="T"/>.</exception>
        T GetValue<T>([CallerMemberName] string key = "<Unknown>");

        /// <summary>
        /// Gets a value of a specified key. If no value exists a specified getter is used to insert the value to the <see cref="ICache"/> and return it.
        /// </summary>
        /// <typeparam name="T">The type of the value to get.</typeparam>
        /// <param name="valueGetter">The getter which is used if no value for the key exists in the <see cref="ICache"/>.</param>
        /// <param name="key">The key to get the value from.</param>
        /// <returns>The value of the <paramref name="key"/> typed as <typeparamref name="T"/>.</returns>
        /// <exception cref="InvalidCastException">The value in the <see cref="ICache"/> can not be cast to type <typeparamref name="T"/>.</exception>
        T GetValue<T>(Func<T> valueGetter, [CallerMemberName] string key = "<Unknown>");

        /// <summary>
        /// Gets a value of a specified key. If no value exists a specified getter is used to insert the value to the <see cref="ICache"/> and return it.
        /// </summary>
        /// <typeparam name="T">The type of the value to get.</typeparam>
        /// <param name="valueGetter">The getter which is used if no value for the key exists in the <see cref="ICache"/>.</param>
        /// <param name="key">The key to get the value from.</param>
        /// <returns>The value of the <paramref name="key"/> typed as <typeparamref name="T"/>.</returns>
        /// <exception cref="InvalidCastException">The value in the <see cref="ICache"/> can not be cast to type <typeparamref name="T"/>.</exception>
        Task<T> GetValueAsync<T>(Func<Task<T>> valueGetter, [CallerMemberName] string key = "<Unknown>");

        /// <summary>
        /// Tries to get the value of a specified key.
        /// </summary>
        /// <typeparam name="T">The type of the value to get.</typeparam>
        /// <param name="value">The value of the <paramref name="key"/> types as <typeparamref name="T"/> if it exists; otherwise <code>default(T)</code>.</param>
        /// <param name="key">The key to get the value from.</param>
        /// <returns><code>true</code> if a value exists for the <paramref name="key"/>; otherwise <code>false</code>.</returns>
        /// <exception cref="InvalidCastException">The value in the <see cref="ICache"/> can not be cast to type <typeparamref name="T"/>.</exception>
        bool TryGetValue<T>([MaybeNull] out T value, [CallerMemberName] string key = "<Unknown>");

        /// <summary>
        /// Removes the value of a specified key if one exists.
        /// </summary>
        /// <param name="key">The key for which the value should be removed.</param>
        void RemoveValue([CallerMemberName] string key = "<Unknown>");

        /// <summary>
        /// Removes and disposes the value of a specified key if one exists.
        /// </summary>
        /// <param name="key">The key for which the value should be removed.</param>
        /// <remarks>Dispose is executed only if the value is of type <see cref="IDisposable"/>; otherwise the value is just removed from the cache.</remarks>
        void RemoveAndDisposeValue([CallerMemberName] string key = "<Unknown>");

        /// <summary>
        /// Clears all data from the <see cref="ICache"/>.
        /// </summary>
        void Clear();

        /// <summary>
        /// Clears and disposes all data from the <see cref="ICache"/>.
        /// </summary>
        /// <remarks>Dispose is executed only if the value is of type <see cref="IDisposable"/>; otherwise the value is just removed from the cache.</remarks>
        void ClearAndDispose();
    }
}
