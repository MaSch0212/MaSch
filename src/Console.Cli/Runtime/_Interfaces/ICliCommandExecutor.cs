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
        /// <param name="context">The execution context.</param>
        /// <returns>The exit code.</returns>
        int ExecuteCommand(CliExecutionContext context);
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
        /// <param name="context">The execution context.</param>
        /// <param name="parameters">The command to execute.</param>
        /// <returns>The exit code.</returns>
        int ExecuteCommand(CliExecutionContext context, TCommand parameters);
    }
}
