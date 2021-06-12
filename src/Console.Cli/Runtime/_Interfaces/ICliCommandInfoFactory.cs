using System;
using System.Threading.Tasks;

namespace MaSch.Console.Cli.Runtime
{
    public interface ICliCommandInfoFactory
    {
        ICliCommandInfo Create<TCommand>()
            where TCommand : ICliCommandExecutorBase;
        ICliCommandInfo Create<TCommand>(TCommand optionsInstance)
            where TCommand : ICliCommandExecutorBase;
        ICliCommandInfo Create(Type commandType);
        ICliCommandInfo Create(Type commandType, object? optionsInstance);
        ICliCommandInfo Create<TCommand, TExecutor>()
            where TExecutor : ICliCommandExecutorBase<TCommand>;
        ICliCommandInfo Create<TCommand, TExecutor>(TExecutor executorInstance)
            where TExecutor : ICliCommandExecutorBase<TCommand>;
        ICliCommandInfo Create<TCommand, TExecutor>(TCommand optionsInstance)
            where TExecutor : ICliCommandExecutorBase<TCommand>;
        ICliCommandInfo Create<TCommand, TExecutor>(TCommand optionsInstance, TExecutor executorInstance)
            where TExecutor : ICliCommandExecutorBase<TCommand>;
        ICliCommandInfo Create(Type commandType, Type? executorType);
        ICliCommandInfo Create(Type commandType, Type? executorType, object? executorInstance);
        ICliCommandInfo Create(Type commandType, object? optionsInstance, Type? executorType);
        ICliCommandInfo Create(Type commandType, object? optionsInstance, Type? executorType, object? executorInstance);
        ICliCommandInfo Create<TCommand>(Func<TCommand, int> executorFunction);
        ICliCommandInfo Create<TCommand>(TCommand optionsInstance, Func<TCommand, int> executorFunction);
        ICliCommandInfo Create<TCommand>(Func<TCommand, Task<int>> executorFunction);
        ICliCommandInfo Create<TCommand>(TCommand optionsInstance, Func<TCommand, Task<int>> executorFunction);
        ICliCommandInfo Create(Type commandType, Func<object, int> executorFunction);
        ICliCommandInfo Create(Type commandType, object? optionsInstance, Func<object, int> executorFunction);
        ICliCommandInfo Create(Type commandType, Func<object, Task<int>> executorFunction);
        ICliCommandInfo Create(Type commandType, object? optionsInstance, Func<object, Task<int>> executorFunction);
    }
}
