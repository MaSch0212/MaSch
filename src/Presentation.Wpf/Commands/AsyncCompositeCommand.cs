namespace MaSch.Presentation.Wpf.Commands;

/// <summary>
/// Defines an asynchronous composite command.
/// </summary>
/// <seealso cref="IAsyncCommand" />
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Does not make sense in this case.")]
public interface IAsyncCompositeCommand : IAsyncCommand
{
    /// <summary>
    /// Gets the commands to execute.
    /// </summary>
    IReadOnlyCollection<IAsyncCommand> Commands { get; }

    /// <summary>
    /// Adds a command to this <see cref="IAsyncCompositeCommand"/>.
    /// </summary>
    /// <param name="command">The command to add.</param>
    void AddCommand(IAsyncCommand command);
}

/// <summary>
/// Default implementation for the <see cref="IAsyncCompositeCommand"/> interface composing <see cref="IAsyncCommand"/>s without parameters.
/// </summary>
/// <seealso cref="AsyncCommandBase" />
/// <seealso cref="IAsyncCompositeCommand" />
public class AsyncCompositeCommand : AsyncCommandBase, IAsyncCompositeCommand
{
    private readonly List<IAsyncCommand> _commands;

    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncCompositeCommand"/> class.
    /// </summary>
    public AsyncCompositeCommand()
    {
        _commands = new List<IAsyncCommand>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncCompositeCommand"/> class.
    /// </summary>
    /// <param name="commands">The commands to execute.</param>
    public AsyncCompositeCommand(params IAsyncCommand[] commands)
        : this((IEnumerable<IAsyncCommand>)commands)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncCompositeCommand"/> class.
    /// </summary>
    /// <param name="commands">The commands to execute.</param>
    public AsyncCompositeCommand(IEnumerable<IAsyncCommand> commands)
    {
        _commands = commands.ToList();
    }

    /// <summary>
    /// Gets the commands to execute.
    /// </summary>
    public IReadOnlyCollection<IAsyncCommand> Commands => _commands.AsReadOnly();

    /// <summary>
    /// Adds a command to this <see cref="IAsyncCompositeCommand" />.
    /// </summary>
    /// <param name="command">The command to add.</param>
    public void AddCommand(IAsyncCommand command)
    {
        _ = _commands.AddIfNotExists(command);
    }

    /// <summary>
    /// Executes the command asynchronously.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public override async Task Execute()
    {
        foreach (var c in _commands)
            await c.ExecuteAsync(null);
    }
}

/// <summary>
/// Default implementation for the <see cref="IAsyncCompositeCommand"/> interface composing <see cref="IAsyncCommand"/>s with parameters of a specific type.
/// </summary>
/// <typeparam name="T">The parameter type for this <see cref="IAsyncCommand"/>.</typeparam>
/// <seealso cref="AsyncCommandBase" />
/// <seealso cref="IAsyncCompositeCommand" />
[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Generic representation can be in same file.")]
public class AsyncCompositeCommand<T> : AsyncCommandBase<T>, IAsyncCompositeCommand
{
    private readonly List<IAsyncCommand> _commands;

    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncCompositeCommand{T}"/> class.
    /// </summary>
    public AsyncCompositeCommand()
    {
        _commands = new List<IAsyncCommand>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncCompositeCommand{T}"/> class.
    /// </summary>
    /// <param name="commands">The commands to execute.</param>
    public AsyncCompositeCommand(params IAsyncCommand[] commands)
        : this((IEnumerable<IAsyncCommand>)commands)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncCompositeCommand{T}"/> class.
    /// </summary>
    /// <param name="commands">The commands to execute.</param>
    public AsyncCompositeCommand(IEnumerable<IAsyncCommand> commands)
    {
        _commands = commands.ToList();
    }

    /// <summary>
    /// Gets the commands to execute.
    /// </summary>
    public IReadOnlyCollection<IAsyncCommand> Commands => _commands.AsReadOnly();

    /// <summary>
    /// Adds a command to this <see cref="IAsyncCompositeCommand" />.
    /// </summary>
    /// <param name="command">The command to add.</param>
    public void AddCommand(IAsyncCommand command)
    {
        _ = _commands.AddIfNotExists(command);
    }

    /// <summary>
    /// Executes the command asynchronously.
    /// </summary>
    /// <param name="parameter">The parameter for the command.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public override async Task Execute(T? parameter)
    {
        foreach (var c in _commands)
            await c.ExecuteAsync(parameter);
    }
}
