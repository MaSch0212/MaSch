namespace MaSch.Core.Lazy;

/// <summary>
/// A modifiable counterpart to the <see cref="AdvancedLazy{T1}"/> class.
/// </summary>
/// <typeparam name="T1">The type of the first element.</typeparam>
public class ModifiableAdvancedLazy<T1>
{
    private readonly Func<T1> _value1Factory;
    private readonly Action<T1> _value1Callback;

    private bool _hasValue1;
    private T1? _value1;

    /// <summary>
    /// Initializes a new instance of the <see cref="ModifiableAdvancedLazy{T1}"/> class.
    /// </summary>
    /// <param name="value1Factory">The factory for the first item.</param>
    /// <param name="value1Callback">The callback action that is called when the first item changed.</param>
    public ModifiableAdvancedLazy(Func<T1> value1Factory, Action<T1> value1Callback)
        : this(value1Factory, value1Callback, true)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ModifiableAdvancedLazy{T1}"/> class.
    /// </summary>
    /// <param name="value1Factory">The factory for the first item.</param>
    /// <param name="value1Callback">The callback action that is called when the first item changed.</param>
    /// <param name="useCaching">if set to <c>true</c> the first value returned by the factory function is cached.</param>
    public ModifiableAdvancedLazy(Func<T1> value1Factory, Action<T1> value1Callback, bool useCaching)
    {
        _value1Factory = Guard.NotNull(value1Factory);
        _value1Callback = Guard.NotNull(value1Callback);
        UseCaching = useCaching;
    }

    /// <summary>
    /// Gets or sets the first item.
    /// </summary>
    public T1 Item1
    {
        get => GetValue(ref _hasValue1, ref _value1, _value1Factory);
        set => SetValue(ref _hasValue1, ref _value1, value, _value1Callback);
    }

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
    /// <param name="hasValueField">The field that indicates whether the value is cached.</param>
    /// <param name="valueField">The value field.</param>
    /// <param name="factory">The factory.</param>
    /// <returns>The created or cached value.</returns>
    protected T GetValue<T>(ref bool hasValueField, ref T? valueField, Func<T> factory)
    {
        if (UseCaching)
        {
            if (!hasValueField)
                valueField = factory();
            hasValueField = true;
            return valueField!;
        }

        return factory();
    }

    /// <summary>
    /// Sets the specific value.
    /// </summary>
    /// <typeparam name="T">The type of the value to set.</typeparam>
    /// <param name="hasValueField">The field that indicates whether the value is cached.</param>
    /// <param name="valueField">The value field.</param>
    /// <param name="value">The value.</param>
    /// <param name="callback">The callback.</param>
    protected void SetValue<T>(ref bool hasValueField, ref T? valueField, T value, Action<T> callback)
    {
        if (UseCaching)
        {
            hasValueField = true;
            valueField = value;
            callback(valueField);
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
public class ModifiableAdvancedLazy<T1, T2> : ModifiableAdvancedLazy<T1>
{
    private readonly Func<T2> _value2Factory;
    private readonly Action<T2> _value2Callback;

    private bool _hasValue2;
    private T2? _value2;

    /// <summary>
    /// Initializes a new instance of the <see cref="ModifiableAdvancedLazy{T1, T2}"/> class.
    /// </summary>
    /// <param name="value1Factory">The factory for the first item.</param>
    /// <param name="value1Callback">The callback action that is called when the first item changed.</param>
    /// <param name="value2Factory">The factory for the second item.</param>
    /// <param name="value2Callback">The callback action that is called when the second item changed.</param>
    public ModifiableAdvancedLazy(Func<T1> value1Factory, Action<T1> value1Callback, Func<T2> value2Factory, Action<T2> value2Callback)
        : this(value1Factory, value1Callback, value2Factory, value2Callback, true)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ModifiableAdvancedLazy{T1, T2}"/> class.
    /// </summary>
    /// <param name="value1Factory">The factory for the first item.</param>
    /// <param name="value1Callback">The callback action that is called when the first item changed.</param>
    /// <param name="value2Factory">The factory for the second item.</param>
    /// <param name="value2Callback">The callback action that is called when the second item changed.</param>
    /// <param name="useCaching">if set to <c>true</c> the first value returned by the factory function is cached.</param>
    public ModifiableAdvancedLazy(Func<T1> value1Factory, Action<T1> value1Callback, Func<T2> value2Factory, Action<T2> value2Callback, bool useCaching)
        : base(value1Factory, value1Callback, useCaching)
    {
        _value2Factory = Guard.NotNull(value2Factory);
        _value2Callback = Guard.NotNull(value2Callback);
    }

    /// <summary>
    /// Gets or sets the second item.
    /// </summary>
    public T2 Item2
    {
        get => GetValue(ref _hasValue2, ref _value2, _value2Factory);
        set => SetValue(ref _hasValue2, ref _value2, value, _value2Callback);
    }

    /// <inheritdoc />
    public override void ClearCache()
    {
        base.ClearCache();
        _hasValue2 = false;
        _value2 = default;
    }
}

/// <summary>
/// A modifiable counterpart to the <see cref="AdvancedLazy{T1, T2, T3}"/> class.
/// </summary>
/// <typeparam name="T1">The type of the first element.</typeparam>
/// <typeparam name="T2">The type of the second element.</typeparam>
/// <typeparam name="T3">The type of the third element.</typeparam>
public class ModifiableAdvancedLazy<T1, T2, T3> : ModifiableAdvancedLazy<T1, T2>
{
    private readonly Func<T3> _value3Factory;
    private readonly Action<T3> _value3Callback;

    private bool _hasValue3;
    private T3? _value3;

    /// <summary>
    /// Initializes a new instance of the <see cref="ModifiableAdvancedLazy{T1, T2, T3}"/> class.
    /// </summary>
    /// <param name="value1Factory">The factory for the first item.</param>
    /// <param name="value1Callback">The callback action that is called when the first item changed.</param>
    /// <param name="value2Factory">The factory for the second item.</param>
    /// <param name="value2Callback">The callback action that is called when the second item changed.</param>
    /// <param name="value3Factory">The factory for the third item.</param>
    /// <param name="value3Callback">The callback action that is called when the third item changed.</param>
    public ModifiableAdvancedLazy(Func<T1> value1Factory, Action<T1> value1Callback, Func<T2> value2Factory, Action<T2> value2Callback, Func<T3> value3Factory, Action<T3> value3Callback)
        : this(value1Factory, value1Callback, value2Factory, value2Callback, value3Factory, value3Callback, true)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ModifiableAdvancedLazy{T1, T2, T3}"/> class.
    /// </summary>
    /// <param name="value1Factory">The factory for the first item.</param>
    /// <param name="value1Callback">The callback action that is called when the first item changed.</param>
    /// <param name="value2Factory">The factory for the second item.</param>
    /// <param name="value2Callback">The callback action that is called when the second item changed.</param>
    /// <param name="value3Factory">The factory for the third item.</param>
    /// <param name="value3Callback">The callback action that is called when the third item changed.</param>
    /// <param name="useCaching">if set to <c>true</c> the first value returned by the factory function is cached.</param>
    public ModifiableAdvancedLazy(Func<T1> value1Factory, Action<T1> value1Callback, Func<T2> value2Factory, Action<T2> value2Callback, Func<T3> value3Factory, Action<T3> value3Callback, bool useCaching)
        : base(value1Factory, value1Callback, value2Factory, value2Callback, useCaching)
    {
        _value3Factory = Guard.NotNull(value3Factory);
        _value3Callback = Guard.NotNull(value3Callback);
    }

    /// <summary>
    /// Gets or sets the third item.
    /// </summary>
    public T3 Item3
    {
        get => GetValue(ref _hasValue3, ref _value3, _value3Factory);
        set => SetValue(ref _hasValue3, ref _value3, value, _value3Callback);
    }

    /// <inheritdoc />
    public override void ClearCache()
    {
        base.ClearCache();
        _hasValue3 = false;
        _value3 = default;
    }
}

/// <summary>
/// A modifiable counterpart to the <see cref="AdvancedLazy{T1, T2, T3, T4}"/> class.
/// </summary>
/// <typeparam name="T1">The type of the first element.</typeparam>
/// <typeparam name="T2">The type of the second element.</typeparam>
/// <typeparam name="T3">The type of the third element.</typeparam>
/// <typeparam name="T4">The type of the fourth element.</typeparam>
public class ModifiableAdvancedLazy<T1, T2, T3, T4> : ModifiableAdvancedLazy<T1, T2, T3>
{
    private readonly Func<T4> _value4Factory;
    private readonly Action<T4> _value4Callback;

    private bool _hasValue4;
    private T4? _value4;

    /// <summary>
    /// Initializes a new instance of the <see cref="ModifiableAdvancedLazy{T1, T2, T3, T4}"/> class.
    /// </summary>
    /// <param name="value1Factory">The factory for the first item.</param>
    /// <param name="value1Callback">The callback action that is called when the first item changed.</param>
    /// <param name="value2Factory">The factory for the second item.</param>
    /// <param name="value2Callback">The callback action that is called when the second item changed.</param>
    /// <param name="value3Factory">The factory for the third item.</param>
    /// <param name="value3Callback">The callback action that is called when the third item changed.</param>
    /// <param name="value4Factory">The factory for the fourth item.</param>
    /// <param name="value4Callback">The callback action that is called when the fourth item changed.</param>
    public ModifiableAdvancedLazy(Func<T1> value1Factory, Action<T1> value1Callback, Func<T2> value2Factory, Action<T2> value2Callback, Func<T3> value3Factory, Action<T3> value3Callback, Func<T4> value4Factory, Action<T4> value4Callback)
        : this(value1Factory, value1Callback, value2Factory, value2Callback, value3Factory, value3Callback, value4Factory, value4Callback, true)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ModifiableAdvancedLazy{T1, T2, T3, T4}"/> class.
    /// </summary>
    /// <param name="value1Factory">The factory for the first item.</param>
    /// <param name="value1Callback">The callback action that is called when the first item changed.</param>
    /// <param name="value2Factory">The factory for the second item.</param>
    /// <param name="value2Callback">The callback action that is called when the second item changed.</param>
    /// <param name="value3Factory">The factory for the third item.</param>
    /// <param name="value3Callback">The callback action that is called when the third item changed.</param>
    /// <param name="value4Factory">The factory for the fourth item.</param>
    /// <param name="value4Callback">The callback action that is called when the fourth item changed.</param>
    /// <param name="useCaching">if set to <c>true</c> the first value returned by the factory function is cached.</param>
    public ModifiableAdvancedLazy(Func<T1> value1Factory, Action<T1> value1Callback, Func<T2> value2Factory, Action<T2> value2Callback, Func<T3> value3Factory, Action<T3> value3Callback, Func<T4> value4Factory, Action<T4> value4Callback, bool useCaching)
        : base(value1Factory, value1Callback, value2Factory, value2Callback, value3Factory, value3Callback, useCaching)
    {
        _value4Factory = Guard.NotNull(value4Factory);
        _value4Callback = Guard.NotNull(value4Callback);
    }

    /// <summary>
    /// Gets or sets the fourth item.
    /// </summary>
    public T4 Item4
    {
        get => GetValue(ref _hasValue4, ref _value4, _value4Factory);
        set => SetValue(ref _hasValue4, ref _value4, value, _value4Callback);
    }

    /// <inheritdoc />
    public override void ClearCache()
    {
        base.ClearCache();
        _hasValue4 = false;
        _value4 = default;
    }
}
