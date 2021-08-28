using MaSch.Console.Cli.Runtime;
using MaSch.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MaSch.Console.Cli
{
    /// <summary>
    /// Default Implementation of the <see cref="ICliApplicationBase"/> interface.
    /// </summary>
    public abstract class CliApplicationBase : ICliApplicationBase
    {
        private IReadOnlyCliCommandInfoCollection? _readOnlyCommands;

        /// <summary>
        /// Initializes a new instance of the <see cref="CliApplicationBase"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider to use.</param>
        /// <param name="options">The options to use.</param>
        /// <param name="commandsCollection">The command collection to use.</param>
        protected CliApplicationBase(IServiceProvider serviceProvider, ICliApplicationOptions options, ICliCommandInfoCollection commandsCollection)
        {
            VerifyTypes();
            ServiceProvider = Guard.NotNull(serviceProvider, nameof(serviceProvider));
            Options = Guard.NotNull(options, nameof(options));
            CommandsCollection = Guard.NotNull(commandsCollection, nameof(commandsCollection));
        }

        /// <inheritdoc/>
        public IReadOnlyCliCommandInfoCollection Commands => _readOnlyCommands ??= CommandsCollection.AsReadOnly();

        /// <inheritdoc/>
        public ICliApplicationOptions Options { get; }

        /// <inheritdoc/>
        public IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Gets the modifiable collection of commands.
        /// </summary>
        protected ICliCommandInfoCollection CommandsCollection { get; }

        /// <summary>
        /// Gets the expected executor type that an executable command must implement or derive from.
        /// </summary>
        protected abstract Type ExecutorType { get; }

        /// <summary>
        /// Gets the expected executor type that an executor must implement or derive from.
        /// </summary>
        protected abstract Type GenericExecutorType { get; }

        /// <summary>
        /// Tries to parse specified command line arguments.
        /// </summary>
        /// <param name="serviceProvider">The service provider to use.</param>
        /// <param name="args">The command line arguments to parse.</param>
        /// <param name="context">The execution context created by the parser.</param>
        /// <param name="options">The options object containing all values and option values set by the command line arguments.</param>
        /// <param name="errorCode">The exit code to use when parsing failed. Is always 0 when <c>true</c> is returned.</param>
        /// <returns><c>true</c> when the command line arguments have been parsed successfully; otherwise <c>false</c>.</returns>
        protected virtual bool TryParseArguments(IServiceProvider serviceProvider, string[] args, [NotNullWhen(true)] out CliExecutionContext? context, [NotNullWhen(true)] out object? options, out int errorCode)
        {
            var parser = serviceProvider.GetRequiredService<ICliArgumentParser>();
            var result = parser.Parse(args);
            if (result.Success)
            {
                context = result.ExecutionContext!;
                options = result.Options!;
                errorCode = 0;
                return true;
            }
            else
            {
                options = context = null;
                var helpPage = serviceProvider.GetRequiredService<ICliHelpPage>();
                var isHelpPage = helpPage.Write(result.Errors);
                errorCode = isHelpPage ? 0 : Options.ParseErrorExitCode;
                return false;
            }
        }

        private void VerifyTypes()
        {
            Guard.NotNull(ExecutorType, nameof(ExecutorType));
            Guard.NotNull(GenericExecutorType, nameof(GenericExecutorType));

            if (!GenericExecutorType.IsGenericType || GenericExecutorType.GetGenericArguments().Length != 1)
                throw new ArgumentException($"The generic executor type needs to be a generic type with exactly one gneric argument.");
        }
    }
}
