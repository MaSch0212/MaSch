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
        private readonly Func<T1> _i1Factory;

        private bool _b1;
        private T1? _i1;

        /// <summary>
        /// Gets a value indicating whether the first value returned by the factory function should be cached.
        /// </summary>
        protected bool UseCaching { get; }

        /// <summary>
        /// Gets the first item.
        /// </summary>
        public T1 Item1 => GetValue(ref _b1, ref _i1, _i1Factory);

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedLazy{T1}"/> class.
        /// </summary>
        /// <param name="i1Factory">The factory for the first item.</param>
        public AdvancedLazy(Func<T1> i1Factory)
            : this(i1Factory, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedLazy{T1}"/> class.
        /// </summary>
        /// <param name="i1Factory">The factory for the first item.</param>
        /// <param name="useCaching">if set to <c>true</c> the first value returned by the factory function is cached.</param>
        public AdvancedLazy(Func<T1> i1Factory, bool useCaching)
        {
            _i1Factory = Guard.NotNull(i1Factory, nameof(i1Factory));
            UseCaching = useCaching;
        }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        public virtual void ClearCache()
        {
            _b1 = false;
            _i1 = default;
        }

        /// <summary>
        /// Gets the specific value using the factory.
        /// </summary>
        /// <typeparam name="T">The type of the value to get.</typeparam>
        /// <param name="bField">The field that indicates whether the value is cached.</param>
        /// <param name="vField">The value field.</param>
        /// <param name="factory">The factory.</param>
        /// <returns>The created or cached value.</returns>
        protected T GetValue<T>(ref bool bField, ref T? vField, Func<T> factory)
        {
            if (UseCaching)
            {
                if (!bField)
                    vField = factory();
                bField = true;
                return vField!;
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
        private readonly Func<T2> _i2Factory;

        private bool _b2;
        private T2? _i2;

        /// <summary>
        /// Gets the second item.
        /// </summary>
        public T2 Item2 => GetValue(ref _b2, ref _i2, _i2Factory);

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedLazy{T1, T2}"/> class.
        /// </summary>
        /// <param name="i1Factory">The factory for the first item.</param>
        /// <param name="i2Factory">The factory for the second item.</param>
        public AdvancedLazy(Func<T1> i1Factory, Func<T2> i2Factory)
            : this(i1Factory, i2Factory, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedLazy{T1, T2}"/> class.
        /// </summary>
        /// <param name="i1Factory">The factory for the first item.</param>
        /// <param name="i2Factory">The factory for the second item.</param>
        /// <param name="useCaching">if set to <c>true</c> the first value returned by the factory function is cached.</param>
        public AdvancedLazy(Func<T1> i1Factory, Func<T2> i2Factory, bool useCaching)
            : base(i1Factory, useCaching)
        {
            _i2Factory = Guard.NotNull(i2Factory, nameof(i2Factory));
        }

        /// <inheritdoc />
        public override void ClearCache()
        {
            base.ClearCache();
            _b2 = false;
            _i2 = default;
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
        private readonly Func<T3> _i3Factory;

        private bool _b3;
        private T3? _i3;

        /// <summary>
        /// Gets the third item.
        /// </summary>
        public T3 Item3 => GetValue(ref _b3, ref _i3, _i3Factory);

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedLazy{T1, T2, T3}"/> class.
        /// </summary>
        /// <param name="i1Factory">The factory for the first item.</param>
        /// <param name="i2Factory">The factory for the second item.</param>
        /// <param name="i3Factory">The factory for the third item.</param>
        public AdvancedLazy(Func<T1> i1Factory, Func<T2> i2Factory, Func<T3> i3Factory)
            : this(i1Factory, i2Factory, i3Factory, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedLazy{T1, T2, T3}"/> class.
        /// </summary>
        /// <param name="i1Factory">The factory for the first item.</param>
        /// <param name="i2Factory">The factory for the second item.</param>
        /// <param name="i3Factory">The factory for the third item.</param>
        /// <param name="useCaching">if set to <c>true</c> the first value returned by the factory function is cached.</param>
        public AdvancedLazy(Func<T1> i1Factory, Func<T2> i2Factory, Func<T3> i3Factory, bool useCaching)
            : base(i1Factory, i2Factory, useCaching)
        {
            _i3Factory = Guard.NotNull(i3Factory, nameof(i3Factory));
        }

        /// <inheritdoc />
        public override void ClearCache()
        {
            base.ClearCache();
            _b3 = false;
            _i3 = default;
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
        private readonly Func<T4> _i4Factory;

        private bool _b4;
        private T4? _i4;

        /// <summary>
        /// Gets the fourth item.
        /// </summary>
        public T4 Item4 => GetValue(ref _b4, ref _i4, _i4Factory);

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedLazy{T1, T2, T3, T4}"/> class.
        /// </summary>
        /// <param name="i1Factory">The factory for the first item.</param>
        /// <param name="i2Factory">The factory for the second item.</param>
        /// <param name="i3Factory">The factory for the third item.</param>
        /// <param name="i4Factory">The factory for the fourth item.</param>
        public AdvancedLazy(Func<T1> i1Factory, Func<T2> i2Factory, Func<T3> i3Factory, Func<T4> i4Factory)
            : this(i1Factory, i2Factory, i3Factory, i4Factory, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedLazy{T1, T2, T3, T4}"/> class.
        /// </summary>
        /// <param name="i1Factory">The factory for the first item.</param>
        /// <param name="i2Factory">The factory for the second item.</param>
        /// <param name="i3Factory">The factory for the third item.</param>
        /// <param name="i4Factory">The factory for the fourth item.</param>
        /// <param name="useCaching">if set to <c>true</c> the first value returned by the factory function is cached.</param>
        public AdvancedLazy(Func<T1> i1Factory, Func<T2> i2Factory, Func<T3> i3Factory, Func<T4> i4Factory, bool useCaching)
            : base(i1Factory, i2Factory, i3Factory, useCaching)
        {
            _i4Factory = Guard.NotNull(i4Factory, nameof(i4Factory));
        }

        /// <inheritdoc />
        public override void ClearCache()
        {
            base.ClearCache();
            _b4 = false;
            _i4 = default;
        }
    }
}
