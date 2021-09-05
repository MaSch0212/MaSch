using MaSch.Core;
using System;

namespace MaSch.Console.Cli.Runtime
{
    /// <summary>
    /// Represents a context for command executions and validations.
    /// </summary>
    public class CliExecutionContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CliExecutionContext"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider for the context.</param>
        /// <param name="command">The command information.</param>
        public CliExecutionContext(IServiceProvider serviceProvider, ICliCommandInfo command)
        {
            ServiceProvider = Guard.NotNull(serviceProvider, nameof(serviceProvider));
            Command = Guard.NotNull(command, nameof(command));
        }

        /// <summary>
        /// Gets the service provider for this context.
        /// </summary>
        public IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Gets the command information.
        /// </summary>
        public ICliCommandInfo Command { get; }
    }
}
