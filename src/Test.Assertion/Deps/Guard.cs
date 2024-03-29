﻿namespace MaSch.Test.Assertion;

internal static class Guard
{
    [return: NotNull]
    public static T NotNull<T>([NotNull] T value, [CallerArgumentExpression("value")] string name = "")
    {
        if (value == null)
            throw new ArgumentNullException(name);
        return value;
    }
}
