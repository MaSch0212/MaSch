using System.Threading.Tasks;

namespace MaSch.Console.Cli.Runtime
{
    /// <summary>
    /// Represents a command that is asynchronously executable.
    /// </summary>
    public interface ICliAsyncExecutable : ICliExecutableBase
    {
        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        /// <param name="context">The execution context.</param>
        /// <returns>The exit code.</returns>
        Task<int> ExecuteCommandAsync(CliExecutionContext context);
    }
}
