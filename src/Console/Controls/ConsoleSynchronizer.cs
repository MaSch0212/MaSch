using MaSch.Core;

namespace MaSch.Console.Controls;

/// <summary>
/// Synchronizes console actions like <see cref="System.Console.WriteLine(string?)"/>.
/// </summary>
public static class ConsoleSynchronizer
{
    private static readonly object Lock = new();

    /// <summary>
    /// Creates a scope to synchronize console actions.
    /// </summary>
    /// <returns>A scope to synchronize console actions.</returns>
    public static IDisposable Scope()
    {
        Monitor.Enter(Lock);
        return new ActionOnDispose(() => Monitor.Exit(Lock));
    }
}
