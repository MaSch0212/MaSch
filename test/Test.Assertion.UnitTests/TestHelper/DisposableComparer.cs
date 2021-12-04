namespace MaSch.Test.Assertion.UnitTests.TestHelper;

public sealed class DisposableComparer<T> : IComparer<T>, IDisposable
{
    private readonly IComparer<T> _comparer;
    private readonly IDisposable _disposable;

    public DisposableComparer(IComparer<T> comparer, IDisposable disposable)
    {
        _comparer = comparer;
        _disposable = disposable;
    }

    public int Compare(T? x, T? y)
    {
        return _comparer.Compare(x, y);
    }

    [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP007:Don't dispose injected.", Justification = "This behavior is expected by the caller.")]
    public void Dispose()
    {
        _disposable.Dispose();
    }
}

public sealed class DisposableEqualityComparer<T> : IEqualityComparer<T>, IDisposable
{
    private readonly IEqualityComparer<T> _comparer;
    private readonly IDisposable _disposable;

    public DisposableEqualityComparer(IEqualityComparer<T> comparer, IDisposable disposable)
    {
        _comparer = comparer;
        _disposable = disposable;
    }

    public bool Equals(T? x, T? y)
    {
        return _comparer.Equals(x, y);
    }

    [ExcludeFromCodeCoverage]
    public int GetHashCode([DisallowNull] T obj)
    {
        return _comparer.GetHashCode(obj);
    }

    [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP007:Don't dispose injected.", Justification = "This behavior is expected by the caller.")]
    public void Dispose()
    {
        _disposable.Dispose();
    }
}

public sealed class DisposableEqualityComparer : IEqualityComparer, IDisposable
{
    private readonly IEqualityComparer _comparer;
    private readonly IDisposable _disposable;

    public DisposableEqualityComparer(IEqualityComparer comparer, IDisposable disposable)
    {
        _comparer = comparer;
        _disposable = disposable;
    }

    public new bool Equals(object? x, object? y)
    {
        return _comparer.Equals(x, y);
    }

    [ExcludeFromCodeCoverage]
    public int GetHashCode(object obj)
    {
        return _comparer.GetHashCode(obj);
    }

    [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP007:Don't dispose injected.", Justification = "This behavior is expected by the caller.")]
    public void Dispose()
    {
        _disposable.Dispose();
    }
}
