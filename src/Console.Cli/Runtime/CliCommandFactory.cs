namespace MaSch.Console.Cli.Runtime;

/// <inheritdoc/>
public class CliCommandFactory : ICliCommandFactory
{
    /// <inheritdoc/>
    public ICliCommandInfo Create<TCommand>()
    {
        return new CliCommandInfo(typeof(TCommand), null, null, null, null);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create<TCommand>(TCommand optionsInstance)
    {
        return new CliCommandInfo(typeof(TCommand), null, optionsInstance, null, null);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create(Type commandType)
    {
        return new CliCommandInfo(commandType, null, null, null, null);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create(Type commandType, object? optionsInstance)
    {
        return new CliCommandInfo(commandType, null, optionsInstance, null, null);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create<TCommand, TExecutor>()
        where TExecutor : ICliExecutorBase<TCommand>
    {
        return new CliCommandInfo(typeof(TCommand), typeof(TExecutor), null, null, null);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create<TCommand, TExecutor>(TExecutor executorInstance)
        where TExecutor : ICliExecutorBase<TCommand>
    {
        return new CliCommandInfo(typeof(TCommand), typeof(TExecutor), null, null, executorInstance);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create<TCommand, TExecutor>(TCommand optionsInstance)
        where TExecutor : ICliExecutorBase<TCommand>
    {
        return new CliCommandInfo(typeof(TCommand), typeof(TExecutor), optionsInstance, null, null);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create<TCommand, TExecutor>(TCommand optionsInstance, TExecutor executorInstance)
        where TExecutor : ICliExecutorBase<TCommand>
    {
        return new CliCommandInfo(typeof(TCommand), typeof(TExecutor), optionsInstance, null, executorInstance);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create(Type commandType, Type? executorType)
    {
        return new CliCommandInfo(commandType, executorType, null, null, null);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create(Type commandType, Type? executorType, object? executorInstance)
    {
        return new CliCommandInfo(commandType, executorType, null, null, executorInstance);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create(Type commandType, object? optionsInstance, Type? executorType)
    {
        return new CliCommandInfo(commandType, executorType, optionsInstance, null, null);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create(Type commandType, object? optionsInstance, Type? executorType, object? executorInstance)
    {
        return new CliCommandInfo(commandType, executorType, optionsInstance, null, executorInstance);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create<TCommand>(Func<CliExecutionContext, TCommand, int> executorFunction)
    {
        return new CliCommandInfo(typeof(TCommand), null, null, executorFunction, null);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create<TCommand>(TCommand optionsInstance, Func<CliExecutionContext, TCommand, int> executorFunction)
    {
        return new CliCommandInfo(typeof(TCommand), null, optionsInstance, executorFunction, null);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create<TCommand>(Func<CliExecutionContext, TCommand, Task<int>> executorFunction)
    {
        return new CliCommandInfo(typeof(TCommand), null, null, executorFunction, null);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create<TCommand>(TCommand optionsInstance, Func<CliExecutionContext, TCommand, Task<int>> executorFunction)
    {
        return new CliCommandInfo(typeof(TCommand), null, optionsInstance, executorFunction, null);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create(Type commandType, Func<CliExecutionContext, object, int> executorFunction)
    {
        return new CliCommandInfo(commandType, null, null, executorFunction, null);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create(Type commandType, object? optionsInstance, Func<CliExecutionContext, object, int> executorFunction)
    {
        return new CliCommandInfo(commandType, null, optionsInstance, executorFunction, null);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create(Type commandType, Func<CliExecutionContext, object, Task<int>> executorFunction)
    {
        return new CliCommandInfo(commandType, null, null, executorFunction, null);
    }

    /// <inheritdoc/>
    public ICliCommandInfo Create(Type commandType, object? optionsInstance, Func<CliExecutionContext, object, Task<int>> executorFunction)
    {
        return new CliCommandInfo(commandType, null, optionsInstance, executorFunction, null);
    }
}
