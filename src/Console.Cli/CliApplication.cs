using MaSch.Console.Cli.Runtime;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MaSch.Console.Cli
{
    /// <summary>
    /// An application that is using a command line interface.
    /// </summary>
    public class CliApplication : CliApplicationBase, ICliApplication
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CliApplication"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider to use.</param>
        /// <param name="options">The options to use.</param>
        /// <param name="commandsCollection">The command collection to use.</param>
        protected internal CliApplication(IServiceProvider serviceProvider, CliApplicationOptions options, ICliCommandInfoCollection commandsCollection)
            : base(serviceProvider, options, commandsCollection)
        {
        }

        /// <inheritdoc/>
        protected override Type ExecutorType { get; } = typeof(ICliExecutable);

        /// <inheritdoc/>
        protected override Type GenericExecutorType { get; } = typeof(ICliExecutor<>);

        /// <inheritdoc/>
        public int Run(string[] args)
        {
            using var scope = ServiceProvider.CreateScope();
            if (TryParseArguments(scope.ServiceProvider, args, out var context, out var options, out int errorCode))
                return context.Command.Execute(context, options);
            return errorCode;
        }
    }
}
