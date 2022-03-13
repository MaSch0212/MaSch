using MaSch.Core;

#pragma warning disable SA1402 // File may only contain a single type

namespace MaSch.Test;

/// <summary>
/// Base class for a class that builds a sut.
/// </summary>
public abstract class SutBuilderBase
{
    private readonly Cache _cache = new();

    /// <summary>
    /// Sets the value for the method that called this method.
    /// </summary>
    /// <typeparam name="T">The type of value.</typeparam>
    /// <param name="value">The value.</param>
    /// <param name="methodName">The method name (auto detected).</param>
    /// <returns>A self reference to this builder.</returns>
    protected virtual SutBuilderBase Set<T>(T value, [CallerMemberName] string? methodName = null)
    {
        _cache.SetValue(value, methodName!);
        return this;
    }

    /// <summary>
    /// Adds a value to the values for the method that called this method.
    /// </summary>
    /// <typeparam name="T">The type of value.</typeparam>
    /// <param name="value">The value.</param>
    /// <param name="methodName">The method name (auto detected).</param>
    /// <returns>A self reference to this builder.</returns>
    protected virtual SutBuilderBase Add<T>(T value, [CallerMemberName] string? methodName = null)
    {
        var list = _cache.GetValue<IList<T>>(() => new List<T>(), methodName!)!;
        list.Add(value);
        return this;
    }

    /// <summary>
    /// Accessor for this builder. An instance of this class is provided as a constructor argument to the created Sut.
    /// </summary>
    public class Accessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Accessor"/> class.
        /// </summary>
        /// <param name="builder">The builder that should be accessed.</param>
        internal protected Accessor(SutBuilderBase builder)
        {
            Builder = builder;
        }

        /// <summary>
        /// Gets the builder that is accessed.
        /// </summary>
        protected SutBuilderBase Builder { get; }

        /// <summary>
        /// Gets the value for the given method.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="methodName">The method name.</param>
        /// <returns>The value for the given method.</returns>
        public T? Get<T>(string methodName)
        {
            return Builder._cache.GetValue<T>(methodName);
        }

        /// <summary>
        /// Tries to get the value for the given method.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="methodName">The method name.</param>
        /// <param name="value">The value.</param>
        /// <returns>A value indicating whether a value existed for the given method.</returns>
        public bool TryGet<T>(string methodName, out T? value)
        {
            return Builder._cache.TryGetValue(out value, methodName);
        }

        /// <summary>
        /// Gets the values for the given method.
        /// </summary>
        /// <typeparam name="T">The type of values.</typeparam>
        /// <param name="methodName">The method name.</param>
        /// <returns>The values for the given method.</returns>
        public IEnumerable<T> GetMany<T>(string methodName)
        {
            return Builder._cache.GetValue<IEnumerable<T>>(methodName) ?? Array.Empty<T>();
        }

        /// <summary>
        /// Tries to get the values for the given method.
        /// </summary>
        /// <typeparam name="T">The type of values.</typeparam>
        /// <param name="methodName">The method name.</param>
        /// <param name="value">The values.</param>
        /// <returns>A value indicating whether a value existed for the given method.</returns>
        public bool TryGetMany<T>(string methodName, out IEnumerable<T> value)
        {
            var result = Builder._cache.TryGetValue<IEnumerable<T>>(out var v, methodName);
            value = v ?? Array.Empty<T>();
            return result;
        }

        /// <summary>
        /// Checks whether a value is available for a given method.
        /// </summary>
        /// <param name="methodName">The method name.</param>
        /// <returns>A value indicating whether a value existed for the given method.</returns>
        public bool Has(string methodName)
        {
            return Builder._cache.HasValue(methodName);
        }
    }
}

/// <summary>
/// Base class for a class that builds a sut.
/// </summary>
/// <typeparam name="TSut">The type of the sut class that is built by the builder.</typeparam>
/// <typeparam name="TBuilder">The actual builder class.</typeparam>
public abstract class SutBuilderBase<TSut, TBuilder> : SutBuilderBase
    where TBuilder : SutBuilderBase<TSut, TBuilder>
{
    /// <summary>
    /// Builds the sut.
    /// </summary>
    /// <returns>Returns the sut instance.</returns>
    public virtual TSut Build()
    {
        return (TSut)Activator.CreateInstance(typeof(TSut), new Accessor(this))!;
    }

    /// <summary>
    /// Sets the value for the method that called this method.
    /// </summary>
    /// <typeparam name="T">The type of value.</typeparam>
    /// <param name="value">The value.</param>
    /// <param name="methodName">The method name (auto detected).</param>
    /// <returns>A self reference to this builder.</returns>
#if NETFRAMEWORK || NETSTANDARD
    protected new TBuilder Set<T>(T value, [CallerMemberName] string? methodName = null)
#else
    protected override TBuilder Set<T>(T value, [CallerMemberName] string? methodName = null)
#endif
    {
        return (TBuilder)base.Set(value, methodName);
    }

    /// <summary>
    /// Adds a value to the values for the method that called this method.
    /// </summary>
    /// <typeparam name="T">The type of value.</typeparam>
    /// <param name="value">The value.</param>
    /// <param name="methodName">The method name (auto detected).</param>
    /// <returns>A self reference to this builder.</returns>
#if NETFRAMEWORK || NETSTANDARD
    protected new TBuilder Add<T>(T value, [CallerMemberName] string? methodName = null)
#else
    protected override TBuilder Add<T>(T value, [CallerMemberName] string? methodName = null)
#endif
    {
        return (TBuilder)base.Add(value, methodName);
    }
}
