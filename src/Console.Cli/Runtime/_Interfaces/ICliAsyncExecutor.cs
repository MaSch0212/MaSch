using System.Threading.Tasks;

namespace MaSch.Console.Cli.Runtime
{
    /// <summary>
    /// Represents an asynchronous executor that can execute a given command.
    /// </summary>
    /// <typeparam name="TCommand">The type of commands the executor can execute.</typeparam>
    public interface ICliAsyncExecutor<TCommand> : ICliExecutorBase<TCommand>
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
