namespace MaSch.Test.Assertion.UnitTests.TestHelper;

public sealed class DisposableWrapper<T> : IDisposable
{
    private readonly IDisposable _disposable;

    public DisposableWrapper(T @object, IDisposable disposable)
    {
        _disposable = disposable;
        Object = @object;
    }

    public T Object { get; }

    [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP007:Don't dispose injected.", Justification = "This behavior is expected by the caller.")]
    public void Dispose()
    {
        _disposable.Dispose();
    }
}
