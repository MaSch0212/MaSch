#pragma warning disable SA1600 // Elements should be documented

namespace MaSch.Core;

[Obsolete("Use the switch statement from C# 8 instead.")]
[ExcludeFromCodeCoverage]
public class Switch<TSource, TTarget>
    where TSource : notnull
{
    private readonly IDictionary<TSource, Func<TTarget>> _caseFuncs = new Dictionary<TSource, Func<TTarget>>();
    private readonly bool _hasSource;
    private readonly TSource? _source;
    private Func<TTarget>? _defaultResultFunc;

    public Switch(TSource source)
    {
        _hasSource = true;
        _source = source;
    }

    public Switch()
    {
        _source = default;
    }

    public Switch<TSource, TTarget> Case(TSource value, Func<TTarget> resultFunc)
    {
        _caseFuncs[value] = resultFunc;
        return this;
    }

    public Switch<TSource, TTarget> Default(Func<TTarget> resultFunc)
    {
        if (_defaultResultFunc == null)
            _defaultResultFunc = resultFunc;
        else
            throw new InvalidOperationException("There can only be one Default statement.");

        return this;
    }

    public TTarget? GetResult()
    {
        return _hasSource ? GetResult(_source!) : throw new InvalidOperationException("This switch statement has no source object.");
    }

    public TTarget? GetResult(TSource source)
    {
        if (_caseFuncs.TryGetValue(source, out var func))
            return func();
        return _defaultResultFunc != null ? _defaultResultFunc() : default;
    }
}
