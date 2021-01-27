using System;
using System.Diagnostics.CodeAnalysis;

#pragma warning disable SA1402 // File may only contain a single type

namespace MaSch.Core.Lazy
{
    /// <summary>
    /// A modifiable counterpart to the <see cref="AdvancedLazy{T1}"/> class.
    /// </summary>
    /// <typeparam name="T1">The type of the first element.</typeparam>
    public class ModifiableAdvancedLazy<T1>
    {
        private readonly Func<T1> _i1Factory;
        private readonly Action<T1> _i1Callback;

        private bool _b1;
        private T1? _i1;

        /// <summary>
        /// Gets a value indicating whether the first value returned by the factory function should be cached.
        /// </summary>
        protected bool UseCaching { get; }

        /// <summary>
        /// Gets or sets the first item.
        /// </summary>
        public T1 Item1
        {
            get => GetValue(ref _b1, ref _i1, _i1Factory);
            set => SetValue(ref _b1, ref _i1, value, _i1Callback);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifiableAdvancedLazy{T1}"/> class.
        /// </summary>
        /// <param name="i1Factory">The factory for the first item.</param>
        /// <param name="i1Callback">The callback action that is called when the first item changed.</param>
        public ModifiableAdvancedLazy(Func<T1> i1Factory, Action<T1> i1Callback)
            : this(i1Factory, i1Callback, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifiableAdvancedLazy{T1}"/> class.
        /// </summary>
        /// <param name="i1Factory">The factory for the first item.</param>
        /// <param name="i1Callback">The callback action that is called when the first item changed.</param>
        /// <param name="useCaching">if set to <c>true</c> the first value returned by the factory function is cached.</param>
        public ModifiableAdvancedLazy(Func<T1> i1Factory, Action<T1> i1Callback, bool useCaching)
        {
            _i1Factory = Guard.NotNull(i1Factory, nameof(i1Factory));
            _i1Callback = Guard.NotNull(i1Callback, nameof(i1Callback));
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

        /// <summary>
        /// Sets the specific value.
        /// </summary>
        /// <typeparam name="T">The type of the value to set.</typeparam>
        /// <param name="bField">The field that indicates whether the value is cached.</param>
        /// <param name="vField">The value field.</param>
        /// <param name="value">The value.</param>
        /// <param name="callback">The callback.</param>
        protected void SetValue<T>(ref bool bField, ref T? vField, T value, Action<T> callback)
        {
            if (UseCaching)
            {
                bField = true;
                callback(vField = value);
            }
            else
            {
                callback(value);
            }
        }
    }

    /// <summary>
    /// A modifiable counterpart to the <see cref="AdvancedLazy{T1, T2}"/> class.
    /// </summary>
    /// <typeparam name="T1">The type of the first element.</typeparam>
    /// <typeparam name="T2">The type of the second element.</typeparam>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Generic representation can be in same file.")]
    public class ModifiableAdvancedLazy<T1, T2> : ModifiableAdvancedLazy<T1>
    {
        private readonly Func<T2> _i2Factory;
        private readonly Action<T2> _i2Callback;

        private bool _b2;
        private T2? _i2;

        /// <summary>
        /// Gets or sets the second item.
        /// </summary>
        public T2 Item2
        {
            get => GetValue(ref _b2, ref _i2, _i2Factory);
            set => SetValue(ref _b2, ref _i2, value, _i2Callback);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifiableAdvancedLazy{T1, T2}"/> class.
        /// </summary>
        /// <param name="i1Factory">The factory for the first item.</param>
        /// <param name="i1Callback">The callback action that is called when the first item changed.</param>
        /// <param name="i2Factory">The factory for the second item.</param>
        /// <param name="i2Callback">The callback action that is called when the second item changed.</param>
        public ModifiableAdvancedLazy(Func<T1> i1Factory, Action<T1> i1Callback, Func<T2> i2Factory, Action<T2> i2Callback)
            : this(i1Factory, i1Callback, i2Factory, i2Callback, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifiableAdvancedLazy{T1, T2}"/> class.
        /// </summary>
        /// <param name="i1Factory">The factory for the first item.</param>
        /// <param name="i1Callback">The callback action that is called when the first item changed.</param>
        /// <param name="i2Factory">The factory for the second item.</param>
        /// <param name="i2Callback">The callback action that is called when the second item changed.</param>
        /// <param name="useCaching">if set to <c>true</c> the first value returned by the factory function is cached.</param>
        public ModifiableAdvancedLazy(Func<T1> i1Factory, Action<T1> i1Callback, Func<T2> i2Factory, Action<T2> i2Callback, bool useCaching)
            : base(i1Factory, i1Callback, useCaching)
        {
            _i2Factory = Guard.NotNull(i2Factory, nameof(i2Factory));
            _i2Callback = Guard.NotNull(i2Callback, nameof(i2Callback));
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
    /// A modifiable counterpart to the <see cref="AdvancedLazy{T1, T2, T3}"/> class.
    /// </summary>
    /// <typeparam name="T1">The type of the first element.</typeparam>
    /// <typeparam name="T2">The type of the second element.</typeparam>
    /// <typeparam name="T3">The type of the third element.</typeparam>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Generic representation can be in same file.")]
    public class ModifiableAdvancedLazy<T1, T2, T3> : ModifiableAdvancedLazy<T1, T2>
    {
        private readonly Func<T3> _i3Factory;
        private readonly Action<T3> _i3Callback;

        private bool _b3;
        private T3? _i3;

        /// <summary>
        /// Gets or sets the third item.
        /// </summary>
        public T3 Item3
        {
            get => GetValue(ref _b3, ref _i3, _i3Factory);
            set => SetValue(ref _b3, ref _i3, value, _i3Callback);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifiableAdvancedLazy{T1, T2, T3}"/> class.
        /// </summary>
        /// <param name="i1Factory">The factory for the first item.</param>
        /// <param name="i1Callback">The callback action that is called when the first item changed.</param>
        /// <param name="i2Factory">The factory for the second item.</param>
        /// <param name="i2Callback">The callback action that is called when the second item changed.</param>
        /// <param name="i3Factory">The factory for the third item.</param>
        /// <param name="i3Callback">The callback action that is called when the third item changed.</param>
        public ModifiableAdvancedLazy(Func<T1> i1Factory, Action<T1> i1Callback, Func<T2> i2Factory, Action<T2> i2Callback, Func<T3> i3Factory, Action<T3> i3Callback)
            : this(i1Factory, i1Callback, i2Factory, i2Callback, i3Factory, i3Callback, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifiableAdvancedLazy{T1, T2, T3}"/> class.
        /// </summary>
        /// <param name="i1Factory">The factory for the first item.</param>
        /// <param name="i1Callback">The callback action that is called when the first item changed.</param>
        /// <param name="i2Factory">The factory for the second item.</param>
        /// <param name="i2Callback">The callback action that is called when the second item changed.</param>
        /// <param name="i3Factory">The factory for the third item.</param>
        /// <param name="i3Callback">The callback action that is called when the third item changed.</param>
        /// <param name="useCaching">if set to <c>true</c> the first value returned by the factory function is cached.</param>
        public ModifiableAdvancedLazy(Func<T1> i1Factory, Action<T1> i1Callback, Func<T2> i2Factory, Action<T2> i2Callback, Func<T3> i3Factory, Action<T3> i3Callback, bool useCaching)
            : base(i1Factory, i1Callback, i2Factory, i2Callback, useCaching)
        {
            _i3Factory = Guard.NotNull(i3Factory, nameof(i3Factory));
            _i3Callback = Guard.NotNull(i3Callback, nameof(i3Callback));
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
    /// A modifiable counterpart to the <see cref="AdvancedLazy{T1, T2, T3, T4}"/> class.
    /// </summary>
    /// <typeparam name="T1">The type of the first element.</typeparam>
    /// <typeparam name="T2">The type of the second element.</typeparam>
    /// <typeparam name="T3">The type of the third element.</typeparam>
    /// <typeparam name="T4">The type of the fourth element.</typeparam>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Generic representation can be in same file.")]
    public class ModifiableAdvancedLazy<T1, T2, T3, T4> : ModifiableAdvancedLazy<T1, T2, T3>
    {
        private readonly Func<T4> _i4Factory;
        private readonly Action<T4> _i4Callback;

        private bool _b4;
        private T4? _i4;

        /// <summary>
        /// Gets or sets the fourth item.
        /// </summary>
        public T4 Item4
        {
            get => GetValue(ref _b4, ref _i4, _i4Factory);
            set => SetValue(ref _b4, ref _i4, value, _i4Callback);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifiableAdvancedLazy{T1, T2, T3, T4}"/> class.
        /// </summary>
        /// <param name="i1Factory">The factory for the first item.</param>
        /// <param name="i1Callback">The callback action that is called when the first item changed.</param>
        /// <param name="i2Factory">The factory for the second item.</param>
        /// <param name="i2Callback">The callback action that is called when the second item changed.</param>
        /// <param name="i3Factory">The factory for the third item.</param>
        /// <param name="i3Callback">The callback action that is called when the third item changed.</param>
        /// <param name="i4Factory">The factory for the fourth item.</param>
        /// <param name="i4Callback">The callback action that is called when the fourth item changed.</param>
        public ModifiableAdvancedLazy(Func<T1> i1Factory, Action<T1> i1Callback, Func<T2> i2Factory, Action<T2> i2Callback, Func<T3> i3Factory, Action<T3> i3Callback, Func<T4> i4Factory, Action<T4> i4Callback)
            : this(i1Factory, i1Callback, i2Factory, i2Callback, i3Factory, i3Callback, i4Factory, i4Callback, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifiableAdvancedLazy{T1, T2, T3, T4}"/> class.
        /// </summary>
        /// <param name="i1Factory">The factory for the first item.</param>
        /// <param name="i1Callback">The callback action that is called when the first item changed.</param>
        /// <param name="i2Factory">The factory for the second item.</param>
        /// <param name="i2Callback">The callback action that is called when the second item changed.</param>
        /// <param name="i3Factory">The factory for the third item.</param>
        /// <param name="i3Callback">The callback action that is called when the third item changed.</param>
        /// <param name="i4Factory">The factory for the fourth item.</param>
        /// <param name="i4Callback">The callback action that is called when the fourth item changed.</param>
        /// <param name="useCaching">if set to <c>true</c> the first value returned by the factory function is cached.</param>
        public ModifiableAdvancedLazy(Func<T1> i1Factory, Action<T1> i1Callback, Func<T2> i2Factory, Action<T2> i2Callback, Func<T3> i3Factory, Action<T3> i3Callback, Func<T4> i4Factory, Action<T4> i4Callback, bool useCaching)
            : base(i1Factory, i1Callback, i2Factory, i2Callback, i3Factory, i3Callback, useCaching)
        {
            _i4Factory = Guard.NotNull(i4Factory, nameof(i4Factory));
            _i4Callback = Guard.NotNull(i4Callback, nameof(i4Callback));
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
