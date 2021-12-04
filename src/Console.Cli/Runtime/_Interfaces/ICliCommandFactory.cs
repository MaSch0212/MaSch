using MaSch.Console.Cli.Configuration;

namespace MaSch.Console.Cli.Runtime;

/// <summary>
/// Represents a factory that is used to create <see cref="ICliCommandInfo"/> objects.
/// </summary>
public interface ICliCommandFactory
{
    /// <summary>
    /// Creates a <see cref="ICliCommandInfo"/> from an executable command type.
    /// </summary>
    /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
    /// <returns>The created <see cref="ICliCommandInfo"/> instance.</returns>
    ICliCommandInfo Create<TCommand>();

    /// <summary>
    /// Creates a <see cref="ICliCommandInfo"/> from an executable command type.
    /// </summary>
    /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
    /// <param name="optionsInstance">An instance of <typeparamref name="TCommand"/> that should be used when the command is executed.</param>
    /// <returns>The created <see cref="ICliCommandInfo"/> instance.</returns>
    ICliCommandInfo Create<TCommand>(TCommand optionsInstance);

    /// <summary>
    /// Creates a <see cref="ICliCommandInfo"/> from an executable command type.
    /// </summary>
    /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/> and implements either the <see cref="ICliExecutable"/> or the <see cref="ICliAsyncExecutable"/> interface.</param>
    /// <returns>The created <see cref="ICliCommandInfo"/> instance.</returns>
    ICliCommandInfo Create(Type commandType);

    /// <summary>
    /// Creates a <see cref="ICliCommandInfo"/> from an executable command type.
    /// </summary>
    /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/> and implements either the <see cref="ICliExecutable"/> or the <see cref="ICliAsyncExecutable"/> interface.</param>
    /// <param name="optionsInstance">An instance of <paramref name="commandType"/> that should be used when the command is executed.</param>
    /// <returns>The created <see cref="ICliCommandInfo"/> instance.</returns>
    ICliCommandInfo Create(Type commandType, object? optionsInstance);

    /// <summary>
    /// Creates a <see cref="ICliCommandInfo"/> from a command and executor type.
    /// </summary>
    /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
    /// <typeparam name="TExecutor">The executor type.</typeparam>
    /// <returns>The created <see cref="ICliCommandInfo"/> instance.</returns>
    ICliCommandInfo Create<TCommand, TExecutor>()
        where TExecutor : ICliExecutorBase<TCommand>;

    /// <summary>
    /// Creates a <see cref="ICliCommandInfo"/> from a command and executor type.
    /// </summary>
    /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
    /// <typeparam name="TExecutor">The executor type.</typeparam>
    /// <param name="executorInstance">An instance of <typeparamref name="TExecutor"/> that should be used when the command is executed.</param>
    /// <returns>The created <see cref="ICliCommandInfo"/> instance.</returns>
    ICliCommandInfo Create<TCommand, TExecutor>(TExecutor executorInstance)
        where TExecutor : ICliExecutorBase<TCommand>;

    /// <summary>
    /// Creates a <see cref="ICliCommandInfo"/> from a command and executor type.
    /// </summary>
    /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
    /// <typeparam name="TExecutor">The executor type.</typeparam>
    /// <param name="optionsInstance">An instance of <typeparamref name="TCommand"/> that should be used when the command is executed.</param>
    /// <returns>The created <see cref="ICliCommandInfo"/> instance.</returns>
    ICliCommandInfo Create<TCommand, TExecutor>(TCommand optionsInstance)
        where TExecutor : ICliExecutorBase<TCommand>;

    /// <summary>
    /// Creates a <see cref="ICliCommandInfo"/> from a command and executor type.
    /// </summary>
    /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
    /// <typeparam name="TExecutor">The executor type.</typeparam>
    /// <param name="optionsInstance">An instance of <typeparamref name="TCommand"/> that should be used when the command is executed.</param>
    /// <param name="executorInstance">An instance of <typeparamref name="TExecutor"/> that should be used when the command is executed.</param>
    /// <returns>The created <see cref="ICliCommandInfo"/> instance.</returns>
    ICliCommandInfo Create<TCommand, TExecutor>(TCommand optionsInstance, TExecutor executorInstance)
        where TExecutor : ICliExecutorBase<TCommand>;

    /// <summary>
    /// Creates a <see cref="ICliCommandInfo"/> from a command and executor type.
    /// </summary>
    /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
    /// <param name="executorType">The executor type that implements either the <see cref="ICliExecutor{TCommand}"/> or the <see cref="ICliAsyncExecutor{TCommand}"/> interface for the <paramref name="commandType"/>.</param>
    /// <returns>The created <see cref="ICliCommandInfo"/> instance.</returns>
    ICliCommandInfo Create(Type commandType, Type? executorType);

    /// <summary>
    /// Creates a <see cref="ICliCommandInfo"/> from a command and executor type.
    /// </summary>
    /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
    /// <param name="executorType">The executor type that implements either the <see cref="ICliExecutor{TCommand}"/> or the <see cref="ICliAsyncExecutor{TCommand}"/> interface for the <paramref name="commandType"/>.</param>
    /// <param name="executorInstance">An instance of <paramref name="executorType"/> that should be used when the command is executed.</param>
    /// <returns>The created <see cref="ICliCommandInfo"/> instance.</returns>
    ICliCommandInfo Create(Type commandType, Type? executorType, object? executorInstance);

    /// <summary>
    /// Creates a <see cref="ICliCommandInfo"/> from a command and executor type.
    /// </summary>
    /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
    /// <param name="optionsInstance">An instance of <paramref name="commandType"/> that should be used when the command is executed.</param>
    /// <param name="executorType">The executor type that implements either the <see cref="ICliExecutor{TCommand}"/> or the <see cref="ICliAsyncExecutor{TCommand}"/> interface for the <paramref name="commandType"/>.</param>
    /// <returns>The created <see cref="ICliCommandInfo"/> instance.</returns>
    ICliCommandInfo Create(Type commandType, object? optionsInstance, Type? executorType);

    /// <summary>
    /// Creates a <see cref="ICliCommandInfo"/> from a command and executor type.
    /// </summary>
    /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
    /// <param name="optionsInstance">An instance of <paramref name="commandType"/> that should be used when the command is executed.</param>
    /// <param name="executorType">The executor type that implements either the <see cref="ICliExecutor{TCommand}"/> or the <see cref="ICliAsyncExecutor{TCommand}"/> interface for the <paramref name="commandType"/>.</param>
    /// <param name="executorInstance">An instance of <paramref name="executorType"/> that should be used when the command is executed.</param>
    /// <returns>The created <see cref="ICliCommandInfo"/> instance.</returns>
    ICliCommandInfo Create(Type commandType, object? optionsInstance, Type? executorType, object? executorInstance);

    /// <summary>
    /// Creates a <see cref="ICliCommandInfo"/> from a command type and an executor function.
    /// </summary>
    /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
    /// <param name="executorFunction">The executor function that is called when the created command is executed.</param>
    /// <returns>The created <see cref="ICliCommandInfo"/> instance.</returns>
    ICliCommandInfo Create<TCommand>(Func<CliExecutionContext, TCommand, int> executorFunction);

    /// <summary>
    /// Creates a <see cref="ICliCommandInfo"/> from a command type and an executor function.
    /// </summary>
    /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
    /// <param name="optionsInstance">An instance of <typeparamref name="TCommand"/> that should be used when the command is executed.</param>
    /// <param name="executorFunction">The executor function that is called when the created command is executed.</param>
    /// <returns>The created <see cref="ICliCommandInfo"/> instance.</returns>
    ICliCommandInfo Create<TCommand>(TCommand optionsInstance, Func<CliExecutionContext, TCommand, int> executorFunction);

    /// <summary>
    /// Creates a <see cref="ICliCommandInfo"/> from a command type and an asynchronous executor function.
    /// </summary>
    /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
    /// <param name="executorFunction">The asynchronous executor function that is called when the created command is executed.</param>
    /// <returns>The created <see cref="ICliCommandInfo"/> instance.</returns>
    ICliCommandInfo Create<TCommand>(Func<CliExecutionContext, TCommand, Task<int>> executorFunction);

    /// <summary>
    /// Creates a <see cref="ICliCommandInfo"/> from a command type and an asynchronous executor function.
    /// </summary>
    /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
    /// <param name="optionsInstance">An instance of <typeparamref name="TCommand"/> that should be used when the command is executed.</param>
    /// <param name="executorFunction">The asynchronous executor function that is called when the created command is executed.</param>
    /// <returns>The created <see cref="ICliCommandInfo"/> instance.</returns>
    ICliCommandInfo Create<TCommand>(TCommand optionsInstance, Func<CliExecutionContext, TCommand, Task<int>> executorFunction);

    /// <summary>
    /// Creates a <see cref="ICliCommandInfo"/> from a command type and an executor function.
    /// </summary>
    /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
    /// <param name="executorFunction">The executor function that is called when the created command is executed.</param>
    /// <returns>The created <see cref="ICliCommandInfo"/> instance.</returns>
    ICliCommandInfo Create(Type commandType, Func<CliExecutionContext, object, int> executorFunction);

    /// <summary>
    /// Creates a <see cref="ICliCommandInfo"/> from a command type and an executor function.
    /// </summary>
    /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
    /// <param name="optionsInstance">An instance of <paramref name="commandType"/> that should be used when the command is executed.</param>
    /// <param name="executorFunction">The executor function that is called when the created command is executed.</param>
    /// <returns>The created <see cref="ICliCommandInfo"/> instance.</returns>
    ICliCommandInfo Create(Type commandType, object? optionsInstance, Func<CliExecutionContext, object, int> executorFunction);

    /// <summary>
    /// Creates a <see cref="ICliCommandInfo"/> from a command type and an asynchronous executor function.
    /// </summary>
    /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
    /// <param name="executorFunction">The asynchronous executor function that is called when the created command is executed.</param>
    /// <returns>The created <see cref="ICliCommandInfo"/> instance.</returns>
    ICliCommandInfo Create(Type commandType, Func<CliExecutionContext, object, Task<int>> executorFunction);

    /// <summary>
    /// Creates a <see cref="ICliCommandInfo"/> from a command type and an asynchronous executor function.
    /// </summary>
    /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
    /// <param name="optionsInstance">An instance of <paramref name="commandType"/> that should be used when the command is executed.</param>
    /// <param name="executorFunction">The asynchronous executor function that is called when the created command is executed.</param>
    /// <returns>The created <see cref="ICliCommandInfo"/> instance.</returns>
    ICliCommandInfo Create(Type commandType, object? optionsInstance, Func<CliExecutionContext, object, Task<int>> executorFunction);
}
