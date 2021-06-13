using MaSch.Console.Cli.Configuration;
using MaSch.Console.Cli.Runtime;
using MaSch.Core;
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
        private ICliCommandInfoFactory? _commandFactory;
        private ICliArgumentParser? _parser;
        private ICliHelpPage? _helpPage;
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
        [AllowNull]
        public ICliCommandInfoFactory CommandFactory
        {
            get => _commandFactory ??= new CliCommandInfoFactory();
            set => _commandFactory = value;
        }

        /// <inheritdoc/>
        [AllowNull]
        public ICliArgumentParser Parser
        {
            get => _parser ??= new CliArgumentParser();
            set => _parser = value;
        }

        /// <inheritdoc/>
        [AllowNull]
        public ICliHelpPage HelpPage
        {
            get => _helpPage ??= new CliHelpPage(new ConsoleService());
            set => _helpPage = value;
        }

        /// <inheritdoc/>
        public IReadOnlyCliCommandInfoCollection Commands => _readOnlyCommands ??= CommandsCollection.AsReadOnly();

        /// <inheritdoc/>
        public CliApplicationOptions Options { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliApplicationBase"/> class.
        /// </summary>
        protected CliApplicationBase()
            : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliApplicationBase"/> class.
        /// </summary>
        /// <param name="options">The options to use.</param>
        protected CliApplicationBase(CliApplicationOptions? options)
            : this(options, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliApplicationBase"/> class.
        /// </summary>
        /// <param name="options">The options to use.</param>
        /// <param name="commandsCollection">The command collection to use.</param>
        protected CliApplicationBase(CliApplicationOptions? options, ICliCommandInfoCollection? commandsCollection)
        {
            VerifyTypes();
            Options = options ?? new CliApplicationOptions();
            CommandsCollection = commandsCollection ?? new CliCommandInfoCollection();
        }

        /// <inheritdoc/>
        public void RegisterCommand(ICliCommandInfo command)
            => CommandsCollection.Add(command);

        /// <inheritdoc/>
        public void RegisterCommand(Type commandType)
            => CommandsCollection.Add(CommandFactory.Create(commandType));

        /// <inheritdoc/>
        public void RegisterCommand(Type commandType, object? optionsInstance)
            => CommandsCollection.Add(CommandFactory.Create(commandType, optionsInstance));

        /// <inheritdoc/>
        public void RegisterCommand(Type commandType, Type? executorType)
            => CommandsCollection.Add(CommandFactory.Create(commandType, executorType));

        /// <inheritdoc/>
        public void RegisterCommand(Type commandType, object? optionsInstance, Type? executorType)
            => CommandsCollection.Add(CommandFactory.Create(commandType, optionsInstance, executorType));

        /// <inheritdoc/>
        public void RegisterCommand(Type commandType, Type? executorType, object? executorInstance)
            => CommandsCollection.Add(CommandFactory.Create(commandType, executorType, executorInstance));

        /// <inheritdoc/>
        public void RegisterCommand(Type commandType, object? optionsInstance, Type? executorType, object? executorInstance)
            => CommandsCollection.Add(CommandFactory.Create(commandType, optionsInstance, executorType, executorInstance));

        /// <summary>
        /// Tries to parse specified command line arguments.
        /// </summary>
        /// <param name="args">The command line arguments to parse.</param>
        /// <param name="command">The command that is referenced by the command line arguments.</param>
        /// <param name="options">The options object containing all values and option values set by the command line arguments.</param>
        /// <param name="errorCode">The exit code to use when parsing failed. Is always 0 when <c>true</c> is returned.</param>
        /// <returns><c>true</c> when the command line arguments have been parsed successfully; otherwise <c>false</c>.</returns>
        protected virtual bool TryParseArguments(string[] args, [NotNullWhen(true)] out ICliCommandInfo? command, [NotNullWhen(true)] out object? options, out int errorCode)
        {
            var result = Parser.Parse(this, args);
            if (result.Success)
            {
                command = result.Command!;
                options = result.Options!;
                errorCode = 0;
                return true;
            }
            else
            {
                options = command = null;
                var isHelpPage = HelpPage.Write(this, result.Errors);
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
        protected override Type ExecutorType { get; } = typeof(ICliCommandExecutor);

        /// <inheritdoc/>
        protected override Type GenericExecutorType { get; } = typeof(ICliCommandExecutor<>);

        /// <summary>
        /// Initializes a new instance of the <see cref="CliApplication"/> class.
        /// </summary>
        public CliApplication()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliApplication"/> class.
        /// </summary>
        /// <param name="options">The options to use.</param>
        public CliApplication(CliApplicationOptions? options)
            : base(options)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliApplication"/> class.
        /// </summary>
        /// <param name="options">The options to use.</param>
        /// <param name="commandsCollection">The command collection to use.</param>
        protected CliApplication(CliApplicationOptions? options, ICliCommandInfoCollection? commandsCollection)
            : base(options, commandsCollection)
        {
        }

        /// <inheritdoc/>
        public new void RegisterCommand(ICliCommandInfo command)
            => base.RegisterCommand(command);

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from an executable command type to this application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/> and implements the <see cref="ICliCommandExecutor"/> interface.</param>
        public new void RegisterCommand(Type commandType)
            => base.RegisterCommand(commandType);

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from an executable command type to this application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/> and implements the <see cref="ICliCommandExecutor"/> interface.</param>
        /// <param name="optionsInstance">An instance of <paramref name="commandType"/> that should be used when the command is executed.</param>
        public new void RegisterCommand(Type commandType, object? optionsInstance)
            => base.RegisterCommand(commandType, optionsInstance);

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from a command and executor type to this application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="executorType">The executor type that implements the <see cref="ICliCommandExecutor{TCommand}"/> interface for the <paramref name="commandType"/>.</param>
        public new void RegisterCommand(Type commandType, Type? executorType)
            => base.RegisterCommand(commandType, executorType);

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from a command and executor type to this application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="optionsInstance">An instance of <paramref name="commandType"/> that should be used when the command is executed.</param>
        /// <param name="executorType">The executor type that implements the <see cref="ICliCommandExecutor{TCommand}"/> interface for the <paramref name="commandType"/>.</param>
        public new void RegisterCommand(Type commandType, object? optionsInstance, Type? executorType)
            => base.RegisterCommand(commandType, optionsInstance, executorType);

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from a command and executor type to this application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="executorType">The executor type that implements the <see cref="ICliCommandExecutor{TCommand}"/> interface for the <paramref name="commandType"/>.</param>
        /// <param name="executorInstance">An instance of <paramref name="executorType"/> that should be used when the command is executed.</param>
        public new void RegisterCommand(Type commandType, Type? executorType, object? executorInstance)
            => base.RegisterCommand(commandType, executorType, executorInstance);

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from a command and executor type to this application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="optionsInstance">An instance of <paramref name="commandType"/> that should be used when the command is executed.</param>
        /// <param name="executorType">The executor type that implements the <see cref="ICliCommandExecutor{TCommand}"/> interface for the <paramref name="commandType"/>.</param>
        /// <param name="executorInstance">An instance of <paramref name="executorType"/> that should be used when the command is executed.</param>
        public new void RegisterCommand(Type commandType, object? optionsInstance, Type? executorType, object? executorInstance)
            => base.RegisterCommand(commandType, optionsInstance, executorType, executorInstance);

        /// <inheritdoc/>
        public void RegisterCommand(Type commandType, Func<object, int> executorFunction)
            => CommandsCollection.Add(CommandFactory.Create(commandType, executorFunction));

        /// <inheritdoc/>
        public void RegisterCommand(Type commandType, object? optionsInstance, Func<object, int> executorFunction)
            => CommandsCollection.Add(CommandFactory.Create(commandType, optionsInstance, executorFunction));

        /// <inheritdoc/>
        public void RegisterCommand<TCommand>(Func<TCommand, int> executorFunction)
            => CommandsCollection.Add(CommandFactory.Create(executorFunction));

        /// <inheritdoc/>
        public void RegisterCommand<TCommand>(TCommand optionsInstance, Func<TCommand, int> executorFunction)
            => CommandsCollection.Add(CommandFactory.Create(optionsInstance, executorFunction));

        /// <inheritdoc/>
        public void RegisterCommand<TCommand>()
            => CommandsCollection.Add(CommandFactory.Create<TCommand>());

        /// <inheritdoc/>
        public void RegisterCommand<TCommand>(TCommand optionsInstance)
            => CommandsCollection.Add(CommandFactory.Create(optionsInstance));

        /// <inheritdoc/>
        public void RegisterCommand<TCommand, TExecutor>()
            where TExecutor : ICliCommandExecutor<TCommand>
            => CommandsCollection.Add(CommandFactory.Create<TCommand, TExecutor>());

        /// <inheritdoc/>
        public void RegisterCommand<TCommand, TExecutor>(TExecutor executorInstance)
            where TExecutor : ICliCommandExecutor<TCommand>
            => CommandsCollection.Add(CommandFactory.Create<TCommand, TExecutor>(executorInstance));

        /// <inheritdoc/>
        public void RegisterCommand<TCommand, TExecutor>(TCommand optionsInstance)
            where TExecutor : ICliCommandExecutor<TCommand>
            => CommandsCollection.Add(CommandFactory.Create<TCommand, TExecutor>(optionsInstance));

        /// <inheritdoc/>
        public void RegisterCommand<TCommand, TExecutor>(TCommand optionsInstance, TExecutor executorInstance)
            where TExecutor : ICliCommandExecutor<TCommand>
            => CommandsCollection.Add(CommandFactory.Create(optionsInstance, executorInstance));

        /// <inheritdoc/>
        public int Run(string[] args)
        {
            if (TryParseArguments(args, out var command, out var options, out int errorCode))
                return command.Execute(options);
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
        protected override Type ExecutorType { get; } = typeof(ICliAsyncCommandExecutor);

        /// <inheritdoc/>
        protected override Type GenericExecutorType { get; } = typeof(ICliAsyncCommandExecutor<>);

        /// <summary>
        /// Initializes a new instance of the <see cref="CliAsyncApplication"/> class.
        /// </summary>
        public CliAsyncApplication()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliAsyncApplication"/> class.
        /// </summary>
        /// <param name="options">The options to use.</param>
        public CliAsyncApplication(CliApplicationOptions? options)
            : base(options)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliAsyncApplication"/> class.
        /// </summary>
        /// <param name="options">The options to use.</param>
        /// <param name="commandsCollection">The command collection to use.</param>
        protected CliAsyncApplication(CliApplicationOptions? options, ICliCommandInfoCollection? commandsCollection)
            : base(options, commandsCollection)
        {
        }

        /// <inheritdoc/>
        public new void RegisterCommand(ICliCommandInfo command)
            => base.RegisterCommand(command);

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from an executable command type to this application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/> and implements the <see cref="ICliAsyncCommandExecutor"/> interface.</param>
        public new void RegisterCommand(Type commandType)
            => base.RegisterCommand(commandType);

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from an executable command type to this application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/> and implements the <see cref="ICliAsyncCommandExecutor"/> interface.</param>
        /// <param name="optionsInstance">An instance of <paramref name="commandType"/> that should be used when the command is executed.</param>
        public new void RegisterCommand(Type commandType, object? optionsInstance)
            => base.RegisterCommand(commandType, optionsInstance);

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from a command and executor type to this application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="executorType">The executor type that implements the <see cref="ICliAsyncCommandExecutor{TCommand}"/> interface for the <paramref name="commandType"/>.</param>
        public new void RegisterCommand(Type commandType, Type? executorType)
            => base.RegisterCommand(commandType, executorType);

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from a command and executor type to this application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="optionsInstance">An instance of <paramref name="commandType"/> that should be used when the command is executed.</param>
        /// <param name="executorType">The executor type that implements the <see cref="ICliAsyncCommandExecutor{TCommand}"/> interface for the <paramref name="commandType"/>.</param>
        public new void RegisterCommand(Type commandType, object? optionsInstance, Type? executorType)
            => base.RegisterCommand(commandType, optionsInstance, executorType);

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from a command and executor type to this application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="executorType">The executor type that implements the <see cref="ICliAsyncCommandExecutor{TCommand}"/> interface for the <paramref name="commandType"/>.</param>
        /// <param name="executorInstance">An instance of <paramref name="executorType"/> that should be used when the command is executed.</param>
        public new void RegisterCommand(Type commandType, Type? executorType, object? executorInstance)
            => base.RegisterCommand(commandType, executorType, executorInstance);

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from a command and executor type to this application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="optionsInstance">An instance of <paramref name="commandType"/> that should be used when the command is executed.</param>
        /// <param name="executorType">The executor type that implements the <see cref="ICliAsyncCommandExecutor{TCommand}"/> interface for the <paramref name="commandType"/>.</param>
        /// <param name="executorInstance">An instance of <paramref name="executorType"/> that should be used when the command is executed.</param>
        public new void RegisterCommand(Type commandType, object? optionsInstance, Type? executorType, object? executorInstance)
            => base.RegisterCommand(commandType, optionsInstance, executorType, executorInstance);

        /// <inheritdoc/>
        public void RegisterCommand(Type commandType, Func<object, Task<int>> executorFunction)
            => CommandsCollection.Add(CommandFactory.Create(commandType, executorFunction));

        /// <inheritdoc/>
        public void RegisterCommand(Type commandType, object? optionsInstance, Func<object, Task<int>> executorFunction)
            => CommandsCollection.Add(CommandFactory.Create(commandType, optionsInstance, executorFunction));

        /// <inheritdoc/>
        public void RegisterCommand<TCommand>(Func<TCommand, Task<int>> executorFunction)
            => CommandsCollection.Add(CommandFactory.Create(executorFunction));

        /// <inheritdoc/>
        public void RegisterCommand<TCommand>(TCommand optionsInstance, Func<TCommand, Task<int>> executorFunction)
            => CommandsCollection.Add(CommandFactory.Create(optionsInstance, executorFunction));

        /// <inheritdoc/>
        public void RegisterCommand<TCommand>()
            => CommandsCollection.Add(CommandFactory.Create<TCommand>());

        /// <inheritdoc/>
        public void RegisterCommand<TCommand>(TCommand optionsInstance)
            => CommandsCollection.Add(CommandFactory.Create(optionsInstance));

        /// <inheritdoc/>
        public void RegisterCommand<TCommand, TExecutor>()
            where TExecutor : ICliAsyncCommandExecutor<TCommand>
            => CommandsCollection.Add(CommandFactory.Create<TCommand, TExecutor>());

        /// <inheritdoc/>
        public void RegisterCommand<TCommand, TExecutor>(TExecutor executorInstance)
            where TExecutor : ICliAsyncCommandExecutor<TCommand>
            => CommandsCollection.Add(CommandFactory.Create<TCommand, TExecutor>(executorInstance));

        /// <inheritdoc/>
        public void RegisterCommand<TCommand, TExecutor>(TCommand optionsInstance)
            where TExecutor : ICliAsyncCommandExecutor<TCommand>
            => CommandsCollection.Add(CommandFactory.Create<TCommand, TExecutor>(optionsInstance));

        /// <inheritdoc/>
        public void RegisterCommand<TCommand, TExecutor>(TCommand optionsInstance, TExecutor executorInstance)
            where TExecutor : ICliAsyncCommandExecutor<TCommand>
            => CommandsCollection.Add(CommandFactory.Create(optionsInstance, executorInstance));

        /// <inheritdoc/>
        public async Task<int> RunAsync(string[] args)
        {
            if (TryParseArguments(args, out var command, out var options, out int errorCode))
                return await command.ExecuteAsync(options);
            return errorCode;
        }
    }
}
