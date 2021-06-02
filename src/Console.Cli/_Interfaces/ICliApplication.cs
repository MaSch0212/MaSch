using MaSch.Console.Cli.Runtime;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MaSch.Console.Cli
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Base interface")]
    public interface ICliApplicationBase
    {
        ICliCommandInfoFactory CommandFactory { get; set; }
        ICliArgumentParser Parser { get; set; }
        ICliHelpPage HelpPage { get; set; }
        IReadOnlyCliCommandInfoCollection Commands { get; }
        CliApplicationOptions Options { get; }

        void RegisterCommand(ICliCommandInfo command);
        void RegisterCommand(Type commandType);
        void RegisterCommand(Type commandType, object? optionsInstance);
        void RegisterCommand(Type commandType, Type? executorType);
        void RegisterCommand(Type commandType, object? optionsInstance, Type? executorType);
        void RegisterCommand(Type commandType, Type? executorType, object? executorInstance);
        void RegisterCommand(Type commandType, object? optionsInstance, Type? executorType, object? executorInstance);
    }

    public interface ICliApplication : ICliApplicationBase
    {
        int Run(string[] args);

        void RegisterCommand(Type commandType, Func<object, int> executorFunction);
        void RegisterCommand(Type commandType, object? optionsInstance, Func<object, int> executorFunction);
        void RegisterCommand<TCommand>(Func<TCommand, int> executorFunction);
        void RegisterCommand<TCommand>(TCommand optionsInstance, Func<TCommand, int> executorFunction);
        void RegisterCommand<TCommand>();
        void RegisterCommand<TCommand>(TCommand optionsInstance);
        void RegisterCommand<TCommand, TExecutor>()
            where TExecutor : ICliCommandExecutor<TCommand>;
        void RegisterCommand<TCommand, TExecutor>(TCommand optionsInstance)
            where TExecutor : ICliCommandExecutor<TCommand>;
        void RegisterCommand<TCommand, TExecutor>(TExecutor executorInstance)
            where TExecutor : ICliCommandExecutor<TCommand>;
        void RegisterCommand<TCommand, TExecutor>(TCommand optionsInstance, TExecutor executorInstance)
            where TExecutor : ICliCommandExecutor<TCommand>;
    }

    public interface ICliAsyncApplication : ICliApplicationBase
    {
        Task<int> RunAsync(string[] args);

        void RegisterCommand(Type commandType, Func<object, Task<int>> executorFunction);
        void RegisterCommand(Type commandType, object? optionsInstance, Func<object, Task<int>> executorFunction);
        void RegisterCommand<TCommand>(Func<TCommand, Task<int>> executorFunction);
        void RegisterCommand<TCommand>(TCommand optionsInstance, Func<TCommand, Task<int>> executorFunction);
        void RegisterCommand<TCommand>();
        void RegisterCommand<TCommand>(TCommand optionsInstance);
        void RegisterCommand<TCommand, TExecutor>()
            where TExecutor : ICliAsyncCommandExecutor<TCommand>;
        void RegisterCommand<TCommand, TExecutor>(TCommand optionsInstance)
            where TExecutor : ICliAsyncCommandExecutor<TCommand>;
        void RegisterCommand<TCommand, TExecutor>(TExecutor optionsInstance)
            where TExecutor : ICliAsyncCommandExecutor<TCommand>;
        void RegisterCommand<TCommand, TExecutor>(TCommand optionsInstance, TExecutor executorInstance)
            where TExecutor : ICliAsyncCommandExecutor<TCommand>;
    }
}
