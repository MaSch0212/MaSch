using MaSch.Console.Cli.Configuration;
using MaSch.Console.Cli.Runtime;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MaSch.Console.Cli
{
    /// <summary>
    /// Represents base functionality for the <see cref="ICliApplication"/> and <see cref="ICliAsyncApplication"/> interfaces.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Base interface")]
    public interface ICliApplicationBase
    {
        /// <summary>
        /// Gets or sets the <see cref="ICliCommandInfoFactory"/> to use when creating <see cref="ICliCommandInfo"/> objects for this application.
        /// </summary>
        ICliCommandInfoFactory CommandFactory { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ICliArgumentParser"/> that is used to parse command line arguments for this application.
        /// </summary>
        ICliArgumentParser Parser { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ICliHelpPage"/> that handles rendering the help page for this application.
        /// </summary>
        ICliHelpPage HelpPage { get; set; }

        /// <summary>
        /// Gets the list of all commands specified for this application.
        /// </summary>
        IReadOnlyCliCommandInfoCollection Commands { get; }

        /// <summary>
        /// Gets the options for this application.
        /// </summary>
        CliApplicationOptions Options { get; }

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> to this application.
        /// </summary>
        /// <param name="command">The command to register.</param>
        void RegisterCommand(ICliCommandInfo command);

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from an executable command type to this application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/> and implements either the <see cref="ICliCommandExecutor"/> or the <see cref="ICliAsyncCommandExecutor"/> interface.</param>
        void RegisterCommand(Type commandType);

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from an executable command type to this application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/> and implements either the <see cref="ICliCommandExecutor"/> or the <see cref="ICliAsyncCommandExecutor"/> interface.</param>
        /// <param name="optionsInstance">An instance of <paramref name="commandType"/> that should be used when the command is executed.</param>
        void RegisterCommand(Type commandType, object? optionsInstance);

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from a command and executor type to this application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="executorType">The executor type that implements either the <see cref="ICliCommandExecutor{TCommand}"/> or the <see cref="ICliAsyncCommandExecutor{TCommand}"/> interface for the <paramref name="commandType"/>.</param>
        void RegisterCommand(Type commandType, Type? executorType);

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from a command and executor type to this application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="optionsInstance">An instance of <paramref name="commandType"/> that should be used when the command is executed.</param>
        /// <param name="executorType">The executor type that implements either the <see cref="ICliCommandExecutor{TCommand}"/> or the <see cref="ICliAsyncCommandExecutor{TCommand}"/> interface for the <paramref name="commandType"/>.</param>
        void RegisterCommand(Type commandType, object? optionsInstance, Type? executorType);

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from a command and executor type to this application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="executorType">The executor type that implements either the <see cref="ICliCommandExecutor{TCommand}"/> or the <see cref="ICliAsyncCommandExecutor{TCommand}"/> interface for the <paramref name="commandType"/>.</param>
        /// <param name="executorInstance">An instance of <paramref name="executorType"/> that should be used when the command is executed.</param>
        void RegisterCommand(Type commandType, Type? executorType, object? executorInstance);

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from a command and executor type to this application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="optionsInstance">An instance of <paramref name="commandType"/> that should be used when the command is executed.</param>
        /// <param name="executorType">The executor type that implements either the <see cref="ICliCommandExecutor{TCommand}"/> or the <see cref="ICliAsyncCommandExecutor{TCommand}"/> interface for the <paramref name="commandType"/>.</param>
        /// <param name="executorInstance">An instance of <paramref name="executorType"/> that should be used when the command is executed.</param>
        void RegisterCommand(Type commandType, object? optionsInstance, Type? executorType, object? executorInstance);
    }

    /// <summary>
    /// Represents an application that is using a command line interface.
    /// </summary>
    public interface ICliApplication : ICliApplicationBase
    {
        /// <summary>
        /// Runs the current application.
        /// </summary>
        /// <param name="args">The command line arguments to parse.</param>
        /// <returns>The exit code of this application.</returns>
        int Run(string[] args);

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from a command type and an executor function to this application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="executorFunction">The executor function that is called when the created command is executed.</param>
        void RegisterCommand(Type commandType, Func<CliExecutionContext, object, int> executorFunction);

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from a command type and an executor function to this application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="optionsInstance">An instance of <paramref name="commandType"/> that should be used when the command is executed.</param>
        /// <param name="executorFunction">The executor function that is called when the created command is executed.</param>
        void RegisterCommand(Type commandType, object? optionsInstance, Func<CliExecutionContext, object, int> executorFunction);

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from a command type and an executor function to this application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <param name="executorFunction">The executor function that is called when the created command is executed.</param>
        void RegisterCommand<TCommand>(Func<CliExecutionContext, TCommand, int> executorFunction);

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from a command type and an executor function to this application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <param name="optionsInstance">An instance of <typeparamref name="TCommand"/> that should be used when the command is executed.</param>
        /// <param name="executorFunction">The executor function that is called when the created command is executed.</param>
        void RegisterCommand<TCommand>(TCommand optionsInstance, Func<CliExecutionContext, TCommand, int> executorFunction);

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from an executable command type to this application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        void RegisterCommand<TCommand>();

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from an executable command type to this application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <param name="optionsInstance">An instance of <typeparamref name="TCommand"/> that should be used when the command is executed.</param>
        void RegisterCommand<TCommand>(TCommand optionsInstance);

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from a command and executor type to this application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <typeparam name="TExecutor">The executor type.</typeparam>
        void RegisterCommand<TCommand, TExecutor>()
            where TExecutor : ICliCommandExecutor<TCommand>;

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from a command and executor type to this application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <typeparam name="TExecutor">The executor type.</typeparam>
        /// <param name="optionsInstance">An instance of <typeparamref name="TCommand"/> that should be used when the command is executed.</param>
        void RegisterCommand<TCommand, TExecutor>(TCommand optionsInstance)
            where TExecutor : ICliCommandExecutor<TCommand>;

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from a command and executor type to this application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <typeparam name="TExecutor">The executor type.</typeparam>
        /// <param name="executorInstance">An instance of <typeparamref name="TExecutor"/> that should be used when the command is executed.</param>
        void RegisterCommand<TCommand, TExecutor>(TExecutor executorInstance)
            where TExecutor : ICliCommandExecutor<TCommand>;

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from a command and executor type to this application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <typeparam name="TExecutor">The executor type.</typeparam>
        /// <param name="optionsInstance">An instance of <typeparamref name="TCommand"/> that should be used when the command is executed.</param>
        /// <param name="executorInstance">An instance of <typeparamref name="TExecutor"/> that should be used when the command is executed.</param>
        void RegisterCommand<TCommand, TExecutor>(TCommand optionsInstance, TExecutor executorInstance)
            where TExecutor : ICliCommandExecutor<TCommand>;
    }

    /// <summary>
    /// Represents an asynchronous application that is using a command line interface.
    /// </summary>
    public interface ICliAsyncApplication : ICliApplicationBase
    {
        /// <summary>
        /// Runs the current application asynchronously.
        /// </summary>
        /// <param name="args">The command line arguments to parse.</param>
        /// <returns>The exit code of this application.</returns>
        Task<int> RunAsync(string[] args);

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from a command type and an executor function to this application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="executorFunction">The executor function that is called when the created command is executed.</param>
        void RegisterCommand(Type commandType, Func<CliExecutionContext, object, Task<int>> executorFunction);

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from a command type and an executor function to this application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="optionsInstance">An instance of <paramref name="commandType"/> that should be used when the command is executed.</param>
        /// <param name="executorFunction">The executor function that is called when the created command is executed.</param>
        void RegisterCommand(Type commandType, object? optionsInstance, Func<CliExecutionContext, object, Task<int>> executorFunction);

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from a command type and an executor function to this application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <param name="executorFunction">The executor function that is called when the created command is executed.</param>
        void RegisterCommand<TCommand>(Func<CliExecutionContext, TCommand, Task<int>> executorFunction);

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from a command type and an executor function to this application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <param name="optionsInstance">An instance of <typeparamref name="TCommand"/> that should be used when the command is executed.</param>
        /// <param name="executorFunction">The executor function that is called when the created command is executed.</param>
        void RegisterCommand<TCommand>(TCommand optionsInstance, Func<CliExecutionContext, TCommand, Task<int>> executorFunction);

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from an executable command type to this application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        void RegisterCommand<TCommand>();

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from an executable command type to this application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <param name="optionsInstance">An instance of <typeparamref name="TCommand"/> that should be used when the command is executed.</param>
        void RegisterCommand<TCommand>(TCommand optionsInstance);

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from a command and executor type to this application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <typeparam name="TExecutor">The executor type.</typeparam>
        void RegisterCommand<TCommand, TExecutor>()
            where TExecutor : ICliAsyncCommandExecutor<TCommand>;

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from a command and executor type to this application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <typeparam name="TExecutor">The executor type.</typeparam>
        /// <param name="optionsInstance">An instance of <typeparamref name="TCommand"/> that should be used when the command is executed.</param>
        void RegisterCommand<TCommand, TExecutor>(TCommand optionsInstance)
            where TExecutor : ICliAsyncCommandExecutor<TCommand>;

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from a command and executor type to this application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <typeparam name="TExecutor">The executor type.</typeparam>
        /// <param name="executorInstance">An instance of <typeparamref name="TExecutor"/> that should be used when the command is executed.</param>
        void RegisterCommand<TCommand, TExecutor>(TExecutor executorInstance)
            where TExecutor : ICliAsyncCommandExecutor<TCommand>;

        /// <summary>
        /// Registers a <see cref="ICliCommandInfo"/> created from a command and executor type to this application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <typeparam name="TExecutor">The executor type.</typeparam>
        /// <param name="optionsInstance">An instance of <typeparamref name="TCommand"/> that should be used when the command is executed.</param>
        /// <param name="executorInstance">An instance of <typeparamref name="TExecutor"/> that should be used when the command is executed.</param>
        void RegisterCommand<TCommand, TExecutor>(TCommand optionsInstance, TExecutor executorInstance)
            where TExecutor : ICliAsyncCommandExecutor<TCommand>;
    }
}
