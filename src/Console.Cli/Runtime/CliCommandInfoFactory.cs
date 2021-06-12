using System;
using System.Threading.Tasks;

namespace MaSch.Console.Cli.Runtime
{
    public class CliCommandInfoFactory : ICliCommandInfoFactory
    {
        public ICliCommandInfo Create<TCommand>()
            where TCommand : ICliCommandExecutorBase
            => new CliCommandInfo(typeof(TCommand), null, null, null, null);

        public ICliCommandInfo Create<TCommand>(TCommand optionsInstance)
            where TCommand : ICliCommandExecutorBase
            => new CliCommandInfo(typeof(TCommand), null, optionsInstance, null, null);

        public ICliCommandInfo Create(Type commandType)
            => new CliCommandInfo(commandType, null, null, null, null);

        public ICliCommandInfo Create(Type commandType, object? optionsInstance)
            => new CliCommandInfo(commandType, null, optionsInstance, null, null);

        public ICliCommandInfo Create<TCommand, TExecutor>()
            where TExecutor : ICliCommandExecutorBase<TCommand>
            => new CliCommandInfo(typeof(TCommand), typeof(TExecutor), null, null, null);

        public ICliCommandInfo Create<TCommand, TExecutor>(TExecutor executorInstance)
            where TExecutor : ICliCommandExecutorBase<TCommand>
            => new CliCommandInfo(typeof(TCommand), typeof(TExecutor), null, null, executorInstance);

        public ICliCommandInfo Create<TCommand, TExecutor>(TCommand optionsInstance)
            where TExecutor : ICliCommandExecutorBase<TCommand>
            => new CliCommandInfo(typeof(TCommand), typeof(TExecutor), optionsInstance, null, null);

        public ICliCommandInfo Create<TCommand, TExecutor>(TCommand optionsInstance, TExecutor executorInstance)
            where TExecutor : ICliCommandExecutorBase<TCommand>
            => new CliCommandInfo(typeof(TCommand), typeof(TExecutor), optionsInstance, null, executorInstance);

        public ICliCommandInfo Create(Type commandType, Type? executorType)
            => new CliCommandInfo(commandType, executorType, null, null, null);

        public ICliCommandInfo Create(Type commandType, Type? executorType, object? executorInstance)
            => new CliCommandInfo(commandType, executorType, null, null, executorInstance);

        public ICliCommandInfo Create(Type commandType, object? optionsInstance, Type? executorType)
            => new CliCommandInfo(commandType, executorType, optionsInstance, null, null);

        public ICliCommandInfo Create(Type commandType, object? optionsInstance, Type? executorType, object? executorInstance)
            => new CliCommandInfo(commandType, executorType, optionsInstance, null, executorInstance);

        public ICliCommandInfo Create<TCommand>(Func<TCommand, int> executorFunction)
            => new CliCommandInfo(typeof(TCommand), null, null, executorFunction, null);

        public ICliCommandInfo Create<TCommand>(TCommand optionsInstance, Func<TCommand, int> executorFunction)
            => new CliCommandInfo(typeof(TCommand), null, optionsInstance, executorFunction, null);

        public ICliCommandInfo Create<TCommand>(Func<TCommand, Task<int>> executorFunction)
            => new CliCommandInfo(typeof(TCommand), null, null, executorFunction, null);

        public ICliCommandInfo Create<TCommand>(TCommand optionsInstance, Func<TCommand, Task<int>> executorFunction)
            => new CliCommandInfo(typeof(TCommand), null, optionsInstance, executorFunction, null);

        public ICliCommandInfo Create(Type commandType, Func<object, int> executorFunction)
            => new CliCommandInfo(commandType, null, null, executorFunction, null);

        public ICliCommandInfo Create(Type commandType, object? optionsInstance, Func<object, int> executorFunction)
            => new CliCommandInfo(commandType, null, optionsInstance, executorFunction, null);

        public ICliCommandInfo Create(Type commandType, Func<object, Task<int>> executorFunction)
            => new CliCommandInfo(commandType, null, null, executorFunction, null);

        public ICliCommandInfo Create(Type commandType, object? optionsInstance, Func<object, Task<int>> executorFunction)
            => new CliCommandInfo(commandType, null, optionsInstance, executorFunction, null);
    }
}
