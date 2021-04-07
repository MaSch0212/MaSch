using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

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

    public sealed class DisposableEqualityComparer<T> : IEqualityComparer<T>, IDisposable
    {
        private readonly IEqualityComparer<T> _comparer;
        private readonly IDisposable _disposable;

        public DisposableEqualityComparer(IEqualityComparer<T> comparer, IDisposable disposable)
        {
            _comparer = comparer;
            _disposable = disposable;
        }

        public bool Equals(T? x, T? y) => _comparer.Equals(x, y);
        public int GetHashCode([DisallowNull] T obj) => _comparer.GetHashCode(obj);
        public void Dispose() => _disposable.Dispose();
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

        public new bool Equals(object? x, object? y) => _comparer.Equals(x, y);
        public int GetHashCode(object obj) => _comparer.GetHashCode(obj);
        public void Dispose() => _disposable.Dispose();
    }
}
