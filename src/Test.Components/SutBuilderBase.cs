using MaSch.Core;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MaSch.Test
{
    /// <summary>
    /// Base class for a class that builds a sut.
    /// </summary>
    /// <typeparam name="TSut">The type of the sut class that is built by the builder.</typeparam>
    /// <typeparam name="TBuilder">The actual builder class.</typeparam>
    public abstract class SutBuilderBase<TSut, TBuilder>
        where TBuilder : SutBuilderBase<TSut, TBuilder>
    {
        private readonly Cache _cache = new();

        /// <summary>
        /// Builds the sut.
        /// </summary>
        /// <returns>Returns the sut instance.</returns>
        public virtual TSut Build()
        {
            return (TSut)Activator.CreateInstance(typeof(TSut), this)!;
        }

        /// <summary>
        /// Sets the value for the method that called this method.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="methodName">The method name (auto detected).</param>
        /// <returns>A self reference to this builder.</returns>
        protected TBuilder Set<T>(T value, [CallerMemberName] string? methodName = null)
        {
            _cache.SetValue(value, methodName!);
            return (TBuilder)this;
        }

        /// <summary>
        /// Adds a value to the values for the method that called this method.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="methodName">The method name (auto detected).</param>
        /// <returns>A self reference to this builder.</returns>
        protected TBuilder Add<T>(T value, [CallerMemberName] string? methodName = null)
        {
            var list = _cache.GetValue<IList<T>>(() => new List<T>(), methodName!)!;
            list.Add(value);
            return (TBuilder)this;
        }

        /// <summary>
        /// Gets the value for the given method.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="methodName">The method name.</param>
        /// <returns>The value for the given method.</returns>
        public T? Get<T>(string methodName) => _cache.GetValue<T>(methodName);

        /// <summary>
        /// Tries to get the value for the given method.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="methodName">The method name.</param>
        /// <param name="value">The value.</param>
        /// <returns>A value indicating whether a value existed for the given method.</returns>
        public bool TryGet<T>(string methodName, out T? value) => _cache.TryGetValue<T>(out value, methodName);

        /// <summary>
        /// Gets the values for the given method.
        /// </summary>
        /// <typeparam name="T">The type of values.</typeparam>
        /// <param name="methodName">The method name.</param>
        /// <returns>The values for the given method.</returns>
        public IEnumerable<T> GetMany<T>(string methodName) => Get<IEnumerable<T>>(methodName) ?? Array.Empty<T>();

        /// <summary>
        /// Tries to get the values for the given method.
        /// </summary>
        /// <typeparam name="T">The type of values.</typeparam>
        /// <param name="methodName">The method name.</param>
        /// <param name="value">The values.</param>
        /// <returns>A value indicating whether a value existed for the given method.</returns>
        public bool TryGetMany<T>(string methodName, out IEnumerable<T> value)
        {
            var result = TryGet<IEnumerable<T>>(methodName, out var v);
            value = v ?? Array.Empty<T>();
            return result;
        }

        /// <summary>
        /// Checks whether a value is available for a given method.
        /// </summary>
        /// <param name="methodName">The method name.</param>
        /// <returns>A value indicating whether a value existed for the given method.</returns>
        public bool Has(string methodName) => _cache.HasValue(methodName);
    }
}
