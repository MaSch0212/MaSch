using MaSch.Console.Cli.Configuration;
using System;
using System.Threading.Tasks;

namespace MaSch.Console.Cli.Runtime
{
    public class CliApplicationBuilder
    {
        private CliApplication _application = new CliApplication();

        public CliApplicationBuilder WithCommand(Type commandType)
            => Exec(x => x.RegisterCommand(commandType));
        public CliApplicationBuilder WithCommand(Type commandType, Type? executorType)
            => Exec(x => x.RegisterCommand(commandType, executorType));
        public CliApplicationBuilder WithCommand(Type commandType, Func<object, int> executorFunction)
            => Exec(x => x.RegisterCommand(commandType, executorFunction));
        public CliApplicationBuilder WithCommand<TCommand>(Func<TCommand, int> executorFunction)
            => Exec(x => x.RegisterCommand(executorFunction));
        public CliApplicationBuilder WithCommand<TCommand>() where TCommand : ICliCommandExecutor
            => Exec(x => x.RegisterCommand<TCommand>());
        public CliApplicationBuilder WithCommand<TCommand, TExecutor>() where TExecutor : ICliCommandExecutor<TCommand>
            => Exec(x => x.RegisterCommand<TCommand, TExecutor>());

        public CliApplicationBuilder Configure(Action<CliApplicationOptions> action)
            => Exec(x => action(x.Options));

        public CliApplication Build()
            => _application;

        private CliApplicationBuilder Exec(Action<CliApplication> action)
        {
            action(_application);
            return this;
        }
    }

    public class CliAsyncApplicatioBuilder
    {
        private CliAsyncApplication _application = new CliAsyncApplication();

        public CliAsyncApplicatioBuilder WithCommand(Type commandType)
            => Exec(x => x.RegisterCommand(commandType));
        public CliAsyncApplicatioBuilder WithCommand(Type commandType, Type? executorType)
            => Exec(x => x.RegisterCommand(commandType, executorType));
        public CliAsyncApplicatioBuilder WithCommand(Type commandType, Func<object, Task<int>> executorFunction)
            => Exec(x => x.RegisterCommand(commandType, executorFunction));
        public CliAsyncApplicatioBuilder WithCommand<TCommand>(Func<TCommand, Task<int>> executorFunction)
            => Exec(x => x.RegisterCommand(executorFunction));
        public CliAsyncApplicatioBuilder WithCommand<TCommand>() where TCommand : ICliAsyncCommandExecutor
            => Exec(x => x.RegisterCommand<TCommand>());
        public CliAsyncApplicatioBuilder WithCommand<TCommand, TExecutor>() where TExecutor : ICliAsyncCommandExecutor<TCommand>
            => Exec(x => x.RegisterCommand<TCommand, TExecutor>());

        public CliAsyncApplicatioBuilder Configure(Action<CliApplicationOptions> action)
            => Exec(x => action(x.Options));

        public CliAsyncApplication Build()
            => _application;

        private CliAsyncApplicatioBuilder Exec(Action<CliAsyncApplication> action)
        {
            action(_application);
            return this;
        }
    }
}
