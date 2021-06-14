using MaSch.Core;

namespace MaSch.Console.Cli.Runtime
{
    /// <summary>
    /// Represents a context for command executions and validations.
    /// </summary>
    public class CliExecutionContext
    {
        /// <summary>
        /// Gets the application.
        /// </summary>
        public ICliApplicationBase Application { get; }

        /// <summary>
        /// Gets the command information.
        /// </summary>
        public ICliCommandInfo Command { get; }

        /// <summary>
        /// Gets the console service.
        /// </summary>
        public IConsoleService Console => Application.Options.ConsoleService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CliExecutionContext"/> class.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="command">The command information.</param>
        public CliExecutionContext(ICliApplicationBase application, ICliCommandInfo command)
        {
            Application = Guard.NotNull(application, nameof(application));
            Command = Guard.NotNull(command, nameof(command));
        }
    }
}
