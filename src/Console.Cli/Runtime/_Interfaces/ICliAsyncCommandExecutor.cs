using System.Threading.Tasks;

namespace MaSch.Console.Cli.Runtime
{
    /// <summary>
    /// Represents a command that is asynchronously executable.
    /// </summary>
    public interface ICliAsyncCommandExecutor : ICliCommandExecutorBase
    {
        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        /// <param name="context">The execution context.</param>
        /// <returns>The exit code.</returns>
        Task<int> ExecuteCommandAsync(CliExecutionContext context);
    }

    /// <summary>
    /// Represents an asynchronous executor that can execute a given command.
    /// </summary>
    /// <typeparam name="TCommand">The type of commands the executor can execute.</typeparam>
    public interface ICliAsyncCommandExecutor<TCommand> : ICliCommandExecutorBase<TCommand>
    {
        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        /// <param name="context">The execution context.</param>
        /// <param name="parameters">The command to execute.</param>
        /// <returns>The exit code.</returns>
        Task<int> ExecuteCommandAsync(CliExecutionContext context, TCommand parameters);
    }
}
