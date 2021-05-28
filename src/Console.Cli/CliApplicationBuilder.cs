using MaSch.Console.Cli.Runtime;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MaSch.Console.Cli
{
    public class CliApplicationBuilder
    {
        private readonly ICliApplication _application = new CliApplication();

        public CliApplicationBuilder WithCommand(Type commandType)
            => Exec(x => x.RegisterCommand(commandType));

        public CliApplicationBuilder WithCommand(Type commandType, object? optionsInstance)
            => Exec(x => ((ICliApplicationBase)x).RegisterCommand(commandType, optionsInstance));

        public CliApplicationBuilder WithCommand(Type commandType, Type? executorType)
            => Exec(x => ((ICliApplicationBase)x).RegisterCommand(commandType, executorType));

        public CliApplicationBuilder WithCommand(Type commandType, object? optionsInstance, Type? executorType)
            => Exec(x => x.RegisterCommand(commandType, optionsInstance, executorType));

        public CliApplicationBuilder WithCommand(Type commandType, Type? executorType, object? executorInstance)
            => Exec(x => x.RegisterCommand(commandType, executorType, executorInstance));

        public CliApplicationBuilder WithCommand(Type commandType, object? optionsInstance, Type? executorType, object? executorInstance)
            => Exec(x => x.RegisterCommand(commandType, optionsInstance, executorType, executorInstance));

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

        public ICliApplication Build()
            => _application;

        private CliApplicationBuilder Exec(Action<ICliApplication> action)
        {
            action(_application);
            return this;
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Async counterpart to CliApplicationBuilder.")]
    public class CliAsyncApplicatioBuilder
    {
        private readonly ICliAsyncApplication _application = new CliAsyncApplication();

        public CliAsyncApplicatioBuilder WithCommand(Type commandType)
            => Exec(x => x.RegisterCommand(commandType));

        public CliAsyncApplicatioBuilder WithCommand(Type commandType, object? optionsInstance)
            => Exec(x => ((ICliApplicationBase)x).RegisterCommand(commandType, optionsInstance));

        public CliAsyncApplicatioBuilder WithCommand(Type commandType, Type? executorType)
            => Exec(x => ((ICliApplicationBase)x).RegisterCommand(commandType, executorType));

        public CliAsyncApplicatioBuilder WithCommand(Type commandType, object? optionsInstance, Type? executorType)
            => Exec(x => x.RegisterCommand(commandType, optionsInstance, executorType));

        public CliAsyncApplicatioBuilder WithCommand(Type commandType, Type? executorType, object? executorInstance)
            => Exec(x => x.RegisterCommand(commandType, executorType, executorInstance));

        public CliAsyncApplicatioBuilder WithCommand(Type commandType, object? optionsInstance, Type? executorType, object? executorInstance)
            => Exec(x => x.RegisterCommand(commandType, optionsInstance, executorType, executorInstance));

        public CliAsyncApplicatioBuilder WithCommand(Type commandType, Func<object, Task<int>> executorFunction)
            => Exec(x => x.RegisterCommand(commandType, executorFunction));

        public CliAsyncApplicatioBuilder WithCommand(Type commandType, object? optionsInstance, Func<object, Task<int>> executorFunction)
            => Exec(x => x.RegisterCommand(commandType, optionsInstance, executorFunction));

        public CliAsyncApplicatioBuilder WithCommand<TCommand>(Func<TCommand, Task<int>> executorFunction)
            => Exec(x => x.RegisterCommand(executorFunction));

        public CliAsyncApplicatioBuilder WithCommand<TCommand>(TCommand optionsInstance, Func<TCommand, Task<int>> executorFunction)
            => Exec(x => x.RegisterCommand(optionsInstance, executorFunction));

        public CliAsyncApplicatioBuilder WithCommand<TCommand>()
            where TCommand : ICliAsyncCommandExecutor
            => Exec(x => x.RegisterCommand<TCommand>());

        public CliAsyncApplicatioBuilder WithCommand<TCommand>(TCommand optionsInstance)
            where TCommand : ICliAsyncCommandExecutor
            => Exec(x => x.RegisterCommand(optionsInstance));

        public CliAsyncApplicatioBuilder WithCommand<TCommand, TExecutor>()
            where TExecutor : ICliAsyncCommandExecutor<TCommand>
            => Exec(x => x.RegisterCommand<TCommand, TExecutor>());

        public CliAsyncApplicatioBuilder WithCommand<TCommand, TExecutor>(TExecutor executorInstance)
            where TExecutor : ICliAsyncCommandExecutor<TCommand>
            => Exec(x => x.RegisterCommand<TCommand, TExecutor>(executorInstance));

        public CliAsyncApplicatioBuilder WithCommand<TCommand, TExecutor>(TCommand optionsInstance)
            where TExecutor : ICliAsyncCommandExecutor<TCommand>
            => Exec(x => x.RegisterCommand<TCommand, TExecutor>(optionsInstance));

        public CliAsyncApplicatioBuilder WithCommand<TCommand, TExecutor>(TCommand optionsInstance, TExecutor executorInstance)
            where TExecutor : ICliAsyncCommandExecutor<TCommand>
            => Exec(x => x.RegisterCommand(optionsInstance, executorInstance));

        public CliAsyncApplicatioBuilder Configure(Action<CliApplicationOptions> action)
            => Exec(x => action(x.Options));

        public ICliAsyncApplication Build()
            => _application;

        private CliAsyncApplicatioBuilder Exec(Action<ICliAsyncApplication> action)
        {
            action(_application);
            return this;
        }
    }
}
