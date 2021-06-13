namespace MaSch.Console.Cli.Runtime
{
    /// <summary>
    /// Represents a command that is executable.
    /// </summary>
    public interface ICliCommandExecutor : ICliCommandExecutorBase
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <returns>The exit code.</returns>
        int ExecuteCommand();
    }

    /// <summary>
    /// Represents an executor that can execute a given command.
    /// </summary>
    /// <typeparam name="TCommand">The type of commands the executor can execute.</typeparam>
    public interface ICliCommandExecutor<TCommand> : ICliCommandExecutorBase<TCommand>
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameters">The command to execute.</param>
        /// <returns>The exit code.</returns>
        int ExecuteCommand(TCommand parameters);
    }
}
