namespace MaSch.Console.Cli.Runtime
{
    /// <summary>
    /// Represents an executor that can execute a given command.
    /// </summary>
    /// <typeparam name="TCommand">The type of commands the executor can execute.</typeparam>
    public interface ICliExecutor<TCommand> : ICliExecutorBase<TCommand>
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
