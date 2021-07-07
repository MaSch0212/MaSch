namespace MaSch.Console.Cli.Runtime
{
    /// <summary>
    /// Represents a command that is executable.
    /// </summary>
    public interface ICliExecutable : ICliExecutableBase
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="context">The execution context.</param>
        /// <returns>The exit code.</returns>
        int ExecuteCommand(CliExecutionContext context);
    }
}
