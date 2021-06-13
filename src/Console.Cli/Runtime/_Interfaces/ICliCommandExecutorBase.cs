namespace MaSch.Console.Cli.Runtime
{
    /// <summary>
    /// Base interface for the <see cref="ICliCommandExecutor"/> and <see cref="ICliAsyncCommandExecutor"/> interfaces.
    /// </summary>
    public interface ICliCommandExecutorBase
    {
    }

    /// <summary>
    /// Base interface for the <see cref="ICliCommandExecutor{TCommand}"/> and <see cref="ICliCommandExecutorBase{TCommand}"/> interfaces.
    /// </summary>
    /// <typeparam name="TCommand">The type of commands the executor can execute.</typeparam>
    public interface ICliCommandExecutorBase<TCommand>
    {
    }
}
