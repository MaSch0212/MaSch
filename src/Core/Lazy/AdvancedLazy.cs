using System;
using System.Diagnostics.CodeAnalysis;

namespace MaSch.Core.Lazy
{
    /// <summary>
    /// More advanced class than <see cref="Lazy{T}"/>.
    /// </summary>
    /// <typeparam name="T1">The type of the first element.</typeparam>
    public class AdvancedLazy<T1>
    {
        private readonly Func<T1> _value1Factory;

        private bool _hasValue1;
        private T1? _value1;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedLazy{T1}"/> class.
        /// </summary>
        /// <param name="value1Factory">The factory for the first item.</param>
        public AdvancedLazy(Func<T1> value1Factory)
            : this(value1Factory, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedLazy{T1}"/> class.
        /// </summary>
        /// <param name="value1Factory">The factory for the first item.</param>
        /// <param name="useCaching">if set to <c>true</c> the first value returned by the factory function is cached.</param>
        public AdvancedLazy(Func<T1> value1Factory, bool useCaching)
        {
            _value1Factory = Guard.NotNull(value1Factory, nameof(value1Factory));
            UseCaching = useCaching;
        }

        /// <summary>
        /// Gets the first item.
        /// </summary>
        public T1 Item1 => GetValue(ref _hasValue1, ref _value1, _value1Factory);

        /// <summary>
        /// Gets a value indicating whether the first value returned by the factory function should be cached.
        /// </summary>
        protected bool UseCaching { get; }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        public virtual void ClearCache()
        {
            _hasValue1 = false;
            _value1 = default;
        }

        /// <summary>
        /// Gets the specific value using the factory.
        /// </summary>
        /// <typeparam name="T">The type of the value to get.</typeparam>
        /// <param name="isCachedField">The field that indicates whether the value is cached.</param>
        /// <param name="valueField">The value field.</param>
        /// <param name="factory">The factory.</param>
        /// <returns>The created or cached value.</returns>
        protected T GetValue<T>(ref bool isCachedField, ref T? valueField, Func<T> factory)
        {
            if (UseCaching)
            {
                if (!isCachedField)
                    valueField = factory();
                isCachedField = true;
                return valueField!;
            }

            return factory();
        }
    }

    /// <summary>
    /// More advanced class than <see cref="Lazy{T}"/>.
    /// </summary>
    /// <typeparam name="T1">The type of the first element.</typeparam>
    /// <typeparam name="T2">The type of the second element.</typeparam>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Generic representation can be in same file.")]
    public class AdvancedLazy<T1, T2> : AdvancedLazy<T1>
    {
        private readonly Func<T2> _value2Factory;

        private bool _hasValue2;
        private T2? _value2;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedLazy{T1, T2}"/> class.
        /// </summary>
        /// <param name="value1Factory">The factory for the first item.</param>
        /// <param name="value2Factory">The factory for the second item.</param>
        public AdvancedLazy(Func<T1> value1Factory, Func<T2> value2Factory)
            : this(value1Factory, value2Factory, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedLazy{T1, T2}"/> class.
        /// </summary>
        /// <param name="value1Factory">The factory for the first item.</param>
        /// <param name="value2Factory">The factory for the second item.</param>
        /// <param name="useCaching">if set to <c>true</c> the first value returned by the factory function is cached.</param>
        public AdvancedLazy(Func<T1> value1Factory, Func<T2> value2Factory, bool useCaching)
            : base(value1Factory, useCaching)
        {
            _value2Factory = Guard.NotNull(value2Factory, nameof(value2Factory));
        }

        /// <summary>
        /// Gets the second item.
        /// </summary>
        public T2 Item2 => GetValue(ref _hasValue2, ref _value2, _value2Factory);

        /// <inheritdoc />
        public override void ClearCache()
        {
            base.ClearCache();
            _hasValue2 = false;
            _value2 = default;
        }
    }

    /// <summary>
    /// More advanced class than <see cref="Lazy{T}"/>.
    /// </summary>
    /// <typeparam name="T1">The type of the first element.</typeparam>
    /// <typeparam name="T2">The type of the second element.</typeparam>
    /// <typeparam name="T3">The type of the third element.</typeparam>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Generic representation can be in same file.")]
    public class AdvancedLazy<T1, T2, T3> : AdvancedLazy<T1, T2>
    {
        private readonly Func<T3> _valuei3Factory;

        private bool _hasValue3;
        private T3? _value3;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedLazy{T1, T2, T3}"/> class.
        /// </summary>
        /// <param name="value1Factory">The factory for the first item.</param>
        /// <param name="value2Factory">The factory for the second item.</param>
        /// <param name="value3Factory">The factory for the third item.</param>
        public AdvancedLazy(Func<T1> value1Factory, Func<T2> value2Factory, Func<T3> value3Factory)
            : this(value1Factory, value2Factory, value3Factory, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedLazy{T1, T2, T3}"/> class.
        /// </summary>
        /// <param name="value1Factory">The factory for the first item.</param>
        /// <param name="value2Factory">The factory for the second item.</param>
        /// <param name="value3Factory">The factory for the third item.</param>
        /// <param name="useCaching">if set to <c>true</c> the first value returned by the factory function is cached.</param>
        public AdvancedLazy(Func<T1> value1Factory, Func<T2> value2Factory, Func<T3> value3Factory, bool useCaching)
            : base(value1Factory, value2Factory, useCaching)
        {
            _valuei3Factory = Guard.NotNull(value3Factory, nameof(value3Factory));
        }

        /// <summary>
        /// Gets the third item.
        /// </summary>
        public T3 Item3 => GetValue(ref _hasValue3, ref _value3, _valuei3Factory);

        /// <inheritdoc />
        public override void ClearCache()
        {
            base.ClearCache();
            _hasValue3 = false;
            _value3 = default;
        }
    }

    /// <summary>
    /// More advanced class than <see cref="Lazy{T}"/>.
    /// </summary>
    /// <typeparam name="T1">The type of the first element.</typeparam>
    /// <typeparam name="T2">The type of the second element.</typeparam>
    /// <typeparam name="T3">The type of the third element.</typeparam>
    /// <typeparam name="T4">The type of the fourth element.</typeparam>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Generic representation can be in same file.")]
    public class AdvancedLazy<T1, T2, T3, T4> : AdvancedLazy<T1, T2, T3>
    {
        private readonly Func<T4> _value4Factory;

        private bool _hasValue4;
        private T4? _value4;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedLazy{T1, T2, T3, T4}"/> class.
        /// </summary>
        /// <param name="value1Factory">The factory for the first item.</param>
        /// <param name="value2Factory">The factory for the second item.</param>
        /// <param name="value3Factory">The factory for the third item.</param>
        /// <param name="value4Factory">The factory for the fourth item.</param>
        public AdvancedLazy(Func<T1> value1Factory, Func<T2> value2Factory, Func<T3> value3Factory, Func<T4> value4Factory)
            : this(value1Factory, value2Factory, value3Factory, value4Factory, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedLazy{T1, T2, T3, T4}"/> class.
        /// </summary>
        /// <param name="value1Factory">The factory for the first item.</param>
        /// <param name="value2Factory">The factory for the second item.</param>
        /// <param name="value3Factory">The factory for the third item.</param>
        /// <param name="value4Factory">The factory for the fourth item.</param>
        /// <param name="useCaching">if set to <c>true</c> the first value returned by the factory function is cached.</param>
        public AdvancedLazy(Func<T1> value1Factory, Func<T2> value2Factory, Func<T3> value3Factory, Func<T4> value4Factory, bool useCaching)
            : base(value1Factory, value2Factory, value3Factory, useCaching)
        {
            _value4Factory = Guard.NotNull(value4Factory, nameof(value4Factory));
        }

        /// <summary>
        /// Gets the fourth item.
        /// </summary>
        public T4 Item4 => GetValue(ref _hasValue4, ref _value4, _value4Factory);

        /// <inheritdoc />
        public override void ClearCache()
        {
            base.ClearCache();
            _hasValue4 = false;
            _value4 = default;
        }
    }
}
