namespace MaSch.Core;

/// <summary>
/// Represents an object that executes an action if disposed.
/// </summary>
/// <seealso cref="IDisposable" />
public sealed class ActionOnDispose : IDisposable
{
    private readonly DateTime _start;
    private readonly Action? _action;
    private readonly Action<TimeSpan>? _actionWithTime;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActionOnDispose"/> class.
    /// </summary>
    /// <param name="action">The action to execute when disposing.</param>
    public ActionOnDispose(Action action)
    {
        _action = Guard.NotNull(action);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ActionOnDispose"/> class.
    /// </summary>
    /// <param name="action">The action to execute when disposing.</param>
    public ActionOnDispose(Action<TimeSpan> action)
    {
        _actionWithTime = Guard.NotNull(action);
        _start = DateTime.Now;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _action?.Invoke();
        _actionWithTime?.Invoke(DateTime.Now - _start);
    }
}
