using System;
using System.Diagnostics.CodeAnalysis;

namespace MaSch.Test.Assertion
{
    internal static class Guard
    {
        [return: NotNull]
        public static T NotNull<T>([NotNull] T value, string name)
        {
            if (value == null)
                throw new ArgumentNullException(name);
            return value;
        }
    }
}
