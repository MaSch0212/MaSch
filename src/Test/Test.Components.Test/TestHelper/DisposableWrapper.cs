using System;
using System.Diagnostics.CodeAnalysis;

namespace MaSch.Test.Components.Test.TestHelper
{
    public sealed class DisposableWrapper<T> : IDisposable
    {
        private readonly IDisposable _disposable;

        public T Object { get; }

        public DisposableWrapper(T @object, IDisposable disposable)
        {
            _disposable = disposable;
            Object = @object;
        }

        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP007:Don't dispose injected.", Justification = "This behavior is expected by the caller.")]
        public void Dispose() => _disposable.Dispose();
    }
}
