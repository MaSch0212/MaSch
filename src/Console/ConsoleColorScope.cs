namespace MaSch.Console;

/// <summary>
/// Scopes the background and foreground color of a specific <see cref="IConsoleService"/> instance.
/// </summary>
/// <seealso cref="IDisposable" />
public sealed class ConsoleColorScope : IDisposable
{
    private readonly IConsoleService _console;
    private readonly ConsoleColor _backColor;
    private readonly ConsoleColor _foreColor;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleColorScope"/> class.
    /// </summary>
    /// <param name="console">The console to scope.</param>
    public ConsoleColorScope(IConsoleService console)
    {
        _console = console;
        _backColor = console.BackgroundColor;
        _foreColor = console.ForegroundColor;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleColorScope"/> class.
    /// </summary>
    /// <param name="console">The console to scope.</param>
    /// <param name="newForegroundColor">The foreground color that should be used inside this scope.</param>
    public ConsoleColorScope(IConsoleService console, ConsoleColor newForegroundColor)
        : this(console)
    {
        console.ForegroundColor = newForegroundColor;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleColorScope"/> class.
    /// </summary>
    /// <param name="console">The console to scope.</param>
    /// <param name="newForegroundColor">The foreground color that should be used inside this scope.</param>
    /// <param name="newBackgroundColor">The background color that should be used inside this scope.</param>
    public ConsoleColorScope(IConsoleService console, ConsoleColor newForegroundColor, ConsoleColor newBackgroundColor)
        : this(console, newForegroundColor)
    {
        console.BackgroundColor = newBackgroundColor;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _console.ForegroundColor = _foreColor;
        _console.BackgroundColor = _backColor;
    }
}
