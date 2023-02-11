using static MaSch.Console.Cli.Globals;

namespace MaSch.Console.Cli.Runtime;

/// <inheritdoc/>
public class CliCommandFactory : ICliCommandFactory
{
    /// <inheritdoc/>
    public ICliCommandInfo Create<[DynamicallyAccessedMembers(CommandTypeDAMT)] TCommand>()
    {
        return new CliCommandInfo(typeof(TCommand), null, null, null, null);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create<[DynamicallyAccessedMembers(CommandTypeDAMT)] TCommand>(TCommand optionsInstance)
    {
        return new CliCommandInfo(typeof(TCommand), null, optionsInstance, null, null);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create([DynamicallyAccessedMembers(CommandTypeDAMT)] Type commandType)
    {
        return new CliCommandInfo(commandType, null, null, null, null);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create([DynamicallyAccessedMembers(CommandTypeDAMT)] Type commandType, object? optionsInstance)
    {
        return new CliCommandInfo(commandType, null, optionsInstance, null, null);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create<[DynamicallyAccessedMembers(CommandTypeDAMT)] TCommand, [DynamicallyAccessedMembers(ExecutorTypeDAMT)] TExecutor>()
        where TExecutor : ICliExecutorBase<TCommand>
    {
        return new CliCommandInfo(typeof(TCommand), typeof(TExecutor), null, null, null);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create<[DynamicallyAccessedMembers(CommandTypeDAMT)] TCommand, [DynamicallyAccessedMembers(ExecutorTypeDAMT)] TExecutor>(TExecutor executorInstance)
        where TExecutor : ICliExecutorBase<TCommand>
    {
        return new CliCommandInfo(typeof(TCommand), typeof(TExecutor), null, null, executorInstance);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create<[DynamicallyAccessedMembers(CommandTypeDAMT)] TCommand, [DynamicallyAccessedMembers(ExecutorTypeDAMT)] TExecutor>(TCommand optionsInstance)
        where TExecutor : ICliExecutorBase<TCommand>
    {
        return new CliCommandInfo(typeof(TCommand), typeof(TExecutor), optionsInstance, null, null);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create<[DynamicallyAccessedMembers(CommandTypeDAMT)] TCommand, [DynamicallyAccessedMembers(ExecutorTypeDAMT)] TExecutor>(TCommand optionsInstance, TExecutor executorInstance)
        where TExecutor : ICliExecutorBase<TCommand>
    {
        return new CliCommandInfo(typeof(TCommand), typeof(TExecutor), optionsInstance, null, executorInstance);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create([DynamicallyAccessedMembers(CommandTypeDAMT)] Type commandType, [DynamicallyAccessedMembers(ExecutorTypeDAMT)] Type? executorType)
    {
        return new CliCommandInfo(commandType, executorType, null, null, null);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create([DynamicallyAccessedMembers(CommandTypeDAMT)] Type commandType, [DynamicallyAccessedMembers(ExecutorTypeDAMT)] Type? executorType, object? executorInstance)
    {
        return new CliCommandInfo(commandType, executorType, null, null, executorInstance);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create([DynamicallyAccessedMembers(CommandTypeDAMT)] Type commandType, object? optionsInstance, [DynamicallyAccessedMembers(ExecutorTypeDAMT)] Type? executorType)
    {
        return new CliCommandInfo(commandType, executorType, optionsInstance, null, null);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create([DynamicallyAccessedMembers(CommandTypeDAMT)] Type commandType, object? optionsInstance, [DynamicallyAccessedMembers(ExecutorTypeDAMT)] Type? executorType, object? executorInstance)
    {
        return new CliCommandInfo(commandType, executorType, optionsInstance, null, executorInstance);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create<[DynamicallyAccessedMembers(CommandTypeDAMT)] TCommand>(Func<CliExecutionContext, TCommand, int> executorFunction)
    {
        return new CliCommandInfo(typeof(TCommand), null, null, executorFunction, null);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create<[DynamicallyAccessedMembers(CommandTypeDAMT)] TCommand>(TCommand optionsInstance, Func<CliExecutionContext, TCommand, int> executorFunction)
    {
        return new CliCommandInfo(typeof(TCommand), null, optionsInstance, executorFunction, null);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create<[DynamicallyAccessedMembers(CommandTypeDAMT)] TCommand>(Func<CliExecutionContext, TCommand, Task<int>> executorFunction)
    {
        return new CliCommandInfo(typeof(TCommand), null, null, executorFunction, null);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create<[DynamicallyAccessedMembers(CommandTypeDAMT)] TCommand>(TCommand optionsInstance, Func<CliExecutionContext, TCommand, Task<int>> executorFunction)
    {
        return new CliCommandInfo(typeof(TCommand), null, optionsInstance, executorFunction, null);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create([DynamicallyAccessedMembers(CommandTypeDAMT)] Type commandType, Func<CliExecutionContext, object, int> executorFunction)
    {
        return new CliCommandInfo(commandType, null, null, executorFunction, null);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create([DynamicallyAccessedMembers(CommandTypeDAMT)] Type commandType, object? optionsInstance, Func<CliExecutionContext, object, int> executorFunction)
    {
        return new CliCommandInfo(commandType, null, optionsInstance, executorFunction, null);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create([DynamicallyAccessedMembers(CommandTypeDAMT)] Type commandType, Func<CliExecutionContext, object, Task<int>> executorFunction)
    {
        return new CliCommandInfo(commandType, null, null, executorFunction, null);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create([DynamicallyAccessedMembers(CommandTypeDAMT)] Type commandType, object? optionsInstance, Func<CliExecutionContext, object, Task<int>> executorFunction)
    {
        return new CliCommandInfo(commandType, null, optionsInstance, executorFunction, null);
    }
}
