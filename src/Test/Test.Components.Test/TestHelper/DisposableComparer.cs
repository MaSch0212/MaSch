using System;
using System.Collections.Generic;

namespace MaSch.Test.Components.Test.TestHelper
{
    public sealed class DisposableComparer<T> : IComparer<T>, IDisposable
    {
        private readonly IComparer<T> _comparer;
        private readonly IDisposable _disposable;

        public DisposableComparer(IComparer<T> comparer, IDisposable disposable)
        {
            _comparer = comparer;
            _disposable = disposable;
        }

        public int Compare(T? x, T? y) => _comparer.Compare(x, y);
        public void Dispose() => _disposable.Dispose();
    }
}
