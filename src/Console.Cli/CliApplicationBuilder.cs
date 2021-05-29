using MaSch.Console.Cli.Runtime;
using MaSch.Core;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

#pragma warning disable SA1402 // File may only contain a single type

namespace MaSch.Console.Cli
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Base class")]
    public abstract class CliApplicationBuilderBase<TApplication, TBuilder>
        where TApplication : ICliApplicationBase
        where TBuilder : CliApplicationBuilderBase<TApplication, TBuilder>
    {
        protected TApplication Application { get; }

        protected CliApplicationBuilderBase(TApplication application)
        {
            Guard.NotNull(application, nameof(application));
            Application = application;
        }

        public virtual TBuilder WithCommand(ICliCommandInfo command)
            => Exec(x => x.RegisterCommand(command));

        public virtual TBuilder WithCommand(Type commandType)
            => Exec(x => x.RegisterCommand(commandType));

        public virtual TBuilder WithCommand(Type commandType, object? optionsInstance)
            => Exec(x => ((ICliApplicationBase)x).RegisterCommand(commandType, optionsInstance));

        public virtual TBuilder WithCommand(Type commandType, Type? executorType)
            => Exec(x => ((ICliApplicationBase)x).RegisterCommand(commandType, executorType));

        public virtual TBuilder WithCommand(Type commandType, object? optionsInstance, Type? executorType)
            => Exec(x => x.RegisterCommand(commandType, optionsInstance, executorType));

        public virtual TBuilder WithCommand(Type commandType, Type? executorType, object? executorInstance)
            => Exec(x => x.RegisterCommand(commandType, executorType, executorInstance));

        public virtual TBuilder WithCommand(Type commandType, object? optionsInstance, Type? executorType, object? executorInstance)
            => Exec(x => x.RegisterCommand(commandType, optionsInstance, executorType, executorInstance));

        public virtual TApplication Build()
            => Application;

        protected TBuilder Exec(Action<TApplication> action)
        {
            action(Application);
            return (TBuilder)this;
        }
    }

    public class CliApplicationBuilder : CliApplicationBuilderBase<ICliApplication, CliApplicationBuilder>
    {
        public CliApplicationBuilder()
            : base(new CliApplication())
        {
        }

        public CliApplicationBuilder WithCommand(Type commandType, Func<object, int> executorFunction)
            => Exec(x => x.RegisterCommand(commandType, executorFunction));

        public CliApplicationBuilder WithCommand(Type commandType, object? optionsInstance, Func<object, int> executorFunction)
            => Exec(x => x.RegisterCommand(commandType, optionsInstance, executorFunction));

        public CliApplicationBuilder WithCommand<TCommand>(Func<TCommand, int> executorFunction)
            => Exec(x => x.RegisterCommand(executorFunction));

        public CliApplicationBuilder WithCommand<TCommand>(TCommand optionsInstance, Func<TCommand, int> executorFunction)
            => Exec(x => x.RegisterCommand(optionsInstance, executorFunction));

        public CliApplicationBuilder WithCommand<TCommand>()
            where TCommand : ICliCommandExecutor
            => Exec(x => x.RegisterCommand<TCommand>());

        public CliApplicationBuilder WithCommand<TCommand>(TCommand optionsInstance)
            where TCommand : ICliCommandExecutor
            => Exec(x => x.RegisterCommand(optionsInstance));

        public CliApplicationBuilder WithCommand<TCommand, TExecutor>()
            where TExecutor : ICliCommandExecutor<TCommand>
            => Exec(x => x.RegisterCommand<TCommand, TExecutor>());

        public CliApplicationBuilder WithCommand<TCommand, TExecutor>(TExecutor executorInstance)
            where TExecutor : ICliCommandExecutor<TCommand>
            => Exec(x => x.RegisterCommand<TCommand, TExecutor>(executorInstance));

        public CliApplicationBuilder WithCommand<TCommand, TExecutor>(TCommand optionsInstance)
            where TExecutor : ICliCommandExecutor<TCommand>
            => Exec(x => x.RegisterCommand<TCommand, TExecutor>(optionsInstance));

        public CliApplicationBuilder WithCommand<TCommand, TExecutor>(TCommand optionsInstance, TExecutor executorInstance)
            where TExecutor : ICliCommandExecutor<TCommand>
            => Exec(x => x.RegisterCommand(optionsInstance, executorInstance));

        public CliApplicationBuilder Configure(Action<CliApplicationOptions> action)
            => Exec(x => action(x.Options));
    }

    public class CliAsyncApplicationBuilder : CliApplicationBuilderBase<ICliAsyncApplication, CliAsyncApplicationBuilder>
    {
        public CliAsyncApplicationBuilder()
            : base(new CliAsyncApplication())
        {
        }

        public CliAsyncApplicationBuilder WithCommand(Type commandType, Func<object, Task<int>> executorFunction)
            => Exec(x => x.RegisterCommand(commandType, executorFunction));

        public CliAsyncApplicationBuilder WithCommand(Type commandType, object? optionsInstance, Func<object, Task<int>> executorFunction)
            => Exec(x => x.RegisterCommand(commandType, optionsInstance, executorFunction));

        public CliAsyncApplicationBuilder WithCommand<TCommand>(Func<TCommand, Task<int>> executorFunction)
            => Exec(x => x.RegisterCommand(executorFunction));

        public CliAsyncApplicationBuilder WithCommand<TCommand>(TCommand optionsInstance, Func<TCommand, Task<int>> executorFunction)
            => Exec(x => x.RegisterCommand(optionsInstance, executorFunction));

        public CliAsyncApplicationBuilder WithCommand<TCommand>()
            where TCommand : ICliAsyncCommandExecutor
            => Exec(x => x.RegisterCommand<TCommand>());

        public CliAsyncApplicationBuilder WithCommand<TCommand>(TCommand optionsInstance)
            where TCommand : ICliAsyncCommandExecutor
            => Exec(x => x.RegisterCommand(optionsInstance));

        public CliAsyncApplicationBuilder WithCommand<TCommand, TExecutor>()
            where TExecutor : ICliAsyncCommandExecutor<TCommand>
            => Exec(x => x.RegisterCommand<TCommand, TExecutor>());

        public CliAsyncApplicationBuilder WithCommand<TCommand, TExecutor>(TExecutor executorInstance)
            where TExecutor : ICliAsyncCommandExecutor<TCommand>
            => Exec(x => x.RegisterCommand<TCommand, TExecutor>(executorInstance));

        public CliAsyncApplicationBuilder WithCommand<TCommand, TExecutor>(TCommand optionsInstance)
            where TExecutor : ICliAsyncCommandExecutor<TCommand>
            => Exec(x => x.RegisterCommand<TCommand, TExecutor>(optionsInstance));

        public CliAsyncApplicationBuilder WithCommand<TCommand, TExecutor>(TCommand optionsInstance, TExecutor executorInstance)
            where TExecutor : ICliAsyncCommandExecutor<TCommand>
            => Exec(x => x.RegisterCommand(optionsInstance, executorInstance));

        public CliAsyncApplicationBuilder Configure(Action<CliApplicationOptions> action)
            => Exec(x => action(x.Options));
    }
}
