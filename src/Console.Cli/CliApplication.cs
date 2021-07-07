using MaSch.Console.Cli.Runtime;
using MaSch.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MaSch.Console.Cli
{
    /// <summary>
    /// Default Implementation of the <see cref="ICliApplicationBase"/> interface.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Base class")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Base class")]
    public abstract class CliApplicationBase : ICliApplicationBase
    {
        private IReadOnlyCliCommandInfoCollection? _readOnlyCommands;

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

        /// <inheritdoc/>
        public IReadOnlyCliCommandInfoCollection Commands => _readOnlyCommands ??= CommandsCollection.AsReadOnly();

        /// <inheritdoc/>
        public ICliApplicationOptions Options { get; }

        /// <inheritdoc/>
        public IServiceProvider ServiceProvider { get; }

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

    /// <summary>
    /// An application that is using a command line interface.
    /// </summary>
    public class CliApplication : CliApplicationBase, ICliApplication
    {
        /// <inheritdoc/>
        protected override Type ExecutorType { get; } = typeof(ICliExecutable);

        /// <inheritdoc/>
        protected override Type GenericExecutorType { get; } = typeof(ICliExecutor<>);

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
        public int Run(string[] args)
        {
            using var scope = ServiceProvider.CreateScope();
            if (TryParseArguments(scope.ServiceProvider, args, out var context, out var options, out int errorCode))
                return context.Command.Execute(context, options);
            return errorCode;
        }
    }

    /// <summary>
    /// An asynchronous application that is using a command line interface.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Async counterpart to CliApplication.")]
    public class CliAsyncApplication : CliApplicationBase, ICliAsyncApplication
    {
        /// <inheritdoc/>
        protected override Type ExecutorType { get; } = typeof(ICliAsyncExecutable);

        /// <inheritdoc/>
        protected override Type GenericExecutorType { get; } = typeof(ICliAsyncExecutor<>);

        /// <summary>
        /// Initializes a new instance of the <see cref="CliAsyncApplication"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider to use.</param>
        /// <param name="options">The options to use.</param>
        /// <param name="commandsCollection">The command collection to use.</param>
        protected internal CliAsyncApplication(IServiceProvider serviceProvider, CliApplicationOptions options, ICliCommandInfoCollection commandsCollection)
            : base(serviceProvider, options, commandsCollection)
        {
        }

        /// <inheritdoc/>
        public async Task<int> RunAsync(string[] args)
        {
            using var scope = ServiceProvider.CreateScope();
            if (TryParseArguments(scope.ServiceProvider, args, out var context, out var options, out int errorCode))
                return await context.Command.ExecuteAsync(context, options);
            return errorCode;
        }
    }
}
