using System;

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

        public void Dispose() => _disposable.Dispose();
    }
}
