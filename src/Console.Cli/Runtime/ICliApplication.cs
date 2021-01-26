using MaSch.Console.Cli.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MaSch.Console.Cli.Runtime
{
    public interface ICliApplication
    {
        IReadOnlyCollection<CliCommandInfo> Commands { get; }

        int Run(string[] args);

        void RegisterCommand(Type commandType);
        void RegisterCommand(Type commandType, Type? executorType);
        void RegisterCommand(Type commandType, Func<object, int> executorFunction);
        void RegisterCommand<TCommand>(Func<TCommand, int> executorFunction);
        void RegisterCommand<TCommand>() where TCommand : ICliCommandExecutor;
        void RegisterCommand<TCommand, TExecutor>() where TExecutor : ICliCommandExecutor<TCommand>;
    }

    public interface ICliAsyncApplication
    {
        IReadOnlyCollection<CliCommandInfo> Commands { get; }

        Task<int> RunAsync(string[] args);

        void RegisterCommand(Type commandType);
        void RegisterCommand(Type commandType, Type? executorType);
        void RegisterCommand(Type commandType, Func<object, Task<int>> executorFunction);
        void RegisterCommand<TCommand>(Func<TCommand, Task<int>> executorFunction);
        void RegisterCommand<TCommand>() where TCommand : ICliAsyncCommandExecutor;
        void RegisterCommand<TCommand, TExecutor>() where TExecutor : ICliAsyncCommandExecutor<TCommand>;
    }
}
