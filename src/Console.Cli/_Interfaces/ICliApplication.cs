using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MaSch.Console.Cli.Runtime
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Base interface")]
    public interface ICliApplicationBase
    {
        IReadOnlyCollection<ICliCommandInfo> Commands { get; }
        CliApplicationOptions Options { get; }

        void RegisterCommand(Type commandType);
        void RegisterCommand(Type commandType, Type? executorType);
        void RegisterCommand(Type commandType, object? optionsInstance, Type? executorType);
        void RegisterCommand(Type commandType, Type? executorType, object? executorInstance);
        void RegisterCommand(Type commandType, object? optionsInstance, Type? executorType, object? executorInstance);
    }

    public interface ICliApplication : ICliApplicationBase
    {
        int Run(string[] args);

        void RegisterCommand(Type commandType, Func<object, int> executorFunction);
        void RegisterCommand<TCommand>(Func<TCommand, int> executorFunction);
        void RegisterCommand<TCommand>()
            where TCommand : ICliCommandExecutor;
        void RegisterCommand<TCommand>(TCommand commandInstance)
            where TCommand : ICliCommandExecutor;
        void RegisterCommand<TCommand, TExecutor>()
            where TExecutor : ICliCommandExecutor<TCommand>;
        void RegisterCommand<TCommand, TExecutor>(TCommand commandInstance)
            where TExecutor : ICliCommandExecutor<TCommand>;
        void RegisterCommand<TCommand, TExecutor>(TExecutor executorInstance)
            where TExecutor : ICliCommandExecutor<TCommand>;
        void RegisterCommand<TCommand, TExecutor>(TCommand commandInstance, TExecutor executorInstance)
            where TExecutor : ICliCommandExecutor<TCommand>;
    }

    public interface ICliAsyncApplication : ICliApplicationBase
    {
        Task<int> RunAsync(string[] args);

        void RegisterCommand(Type commandType, Func<object, Task<int>> executorFunction);
        void RegisterCommand<TCommand>(Func<TCommand, Task<int>> executorFunction);
        void RegisterCommand<TCommand>()
            where TCommand : ICliAsyncCommandExecutor;
        void RegisterCommand<TCommand>(TCommand commandInstance)
            where TCommand : ICliAsyncCommandExecutor;
        void RegisterCommand<TCommand, TExecutor>()
            where TExecutor : ICliAsyncCommandExecutor<TCommand>;
        void RegisterCommand<TCommand, TExecutor>(TCommand commandInstance)
            where TExecutor : ICliAsyncCommandExecutor<TCommand>;
        void RegisterCommand<TCommand, TExecutor>(TExecutor executorInstance)
            where TExecutor : ICliAsyncCommandExecutor<TCommand>;
        void RegisterCommand<TCommand, TExecutor>(TCommand commandInstance, TExecutor executorInstance)
            where TExecutor : ICliAsyncCommandExecutor<TCommand>;
    }
}
