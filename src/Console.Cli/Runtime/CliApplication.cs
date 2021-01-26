using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MaSch.Console.Cli.Configuration;
using MaSch.Core;

namespace MaSch.Console.Cli.Runtime
{
    public class CliApplication : ICliApplication
    {
        private readonly CliCommandInfoCollection _commands;
        private IReadOnlyCollection<CliCommandInfo>? _readOnlyCommands;

        public IReadOnlyCollection<CliCommandInfo> Commands => _readOnlyCommands ??= _commands.AsReadOnly();

        public CliApplicationOptions Options { get; }

        public CliApplication()
            : this(null)
        {
        }

        public CliApplication(CliApplicationOptions? options)
        {
            Options = options ?? new CliApplicationOptions();
            _commands = new CliCommandInfoCollection();
        }

        public void RegisterCommand(Type commandType)
        {
            Guard.NotNull(commandType, nameof(commandType));
            if (!typeof(ICliCommandExecutor).IsAssignableFrom(commandType))
                throw new ArgumentException($"The commandType needs to implement the {typeof(ICliCommandExecutor).FullName} interface.", nameof(commandType));
            _commands.Add(CliCommandInfo.From(commandType));
        }

        public void RegisterCommand(Type commandType, Type? executorType)
        {
            if (executorType == null)
            {
                RegisterCommand(commandType);
                return;
            }

            Guard.NotNull(commandType, nameof(commandType));

            if (!typeof(ICliCommandExecutor<>).IsAssignableFrom(executorType))
                throw new ArgumentException($"The executorType needs to implement the {typeof(ICliCommandExecutor<>).FullName} interface.", nameof(executorType));
            if (!executorType.GenericTypeArguments[0].IsAssignableFrom(commandType))
                throw new ArgumentException($"The first generic argument type of the {typeof(ICliCommandExecutor<>).Name} interface of class {executorType.FullName} needs to be assignable from type {commandType.FullName}.", nameof(executorType));

            _commands.Add(CliCommandInfo.From(commandType, executorType));
        }

        public void RegisterCommand(Type commandType, Func<object, int> executorFunction)
            => _commands.Add(CliCommandInfo.From(commandType, executorFunction));

        public void RegisterCommand<TCommand>(Func<TCommand, int> executorFunction)
            => _commands.Add(CliCommandInfo.From(executorFunction));

        public void RegisterCommand<TCommand>()
            where TCommand : ICliCommandExecutor
            => _commands.Add(CliCommandInfo.From<TCommand>());

        public void RegisterCommand<TCommand, TExecutor>()
            where TExecutor : ICliCommandExecutor<TCommand>
            => _commands.Add(CliCommandInfo.From<TCommand, TExecutor>());

        public int Run(string[] args)
        {
            // TODO
            return 0;
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Async counterpart to CliApplication.")]
    public class CliAsyncApplication : ICliAsyncApplication
    {
        private readonly CliCommandInfoCollection _commands;
        private IReadOnlyCollection<CliCommandInfo>? _readOnlyCommands;

        public IReadOnlyCollection<CliCommandInfo> Commands => _readOnlyCommands ??= _commands.AsReadOnly();

        public CliApplicationOptions Options { get; }

        public CliAsyncApplication()
            : this(null)
        {
        }

        public CliAsyncApplication(CliApplicationOptions? options)
        {
            Options = options ?? new CliApplicationOptions();
            _commands = new CliCommandInfoCollection();
        }

        public void RegisterCommand(Type commandType)
        {
            Guard.NotNull(commandType, nameof(commandType));
            if (!typeof(ICliAsyncCommandExecutor).IsAssignableFrom(commandType))
                throw new ArgumentException($"The commandType needs to implement the {typeof(ICliAsyncCommandExecutor).FullName} interface.", nameof(commandType));
            _commands.Add(CliCommandInfo.From(commandType));
        }

        public void RegisterCommand(Type commandType, Type? executorType)
        {
            if (executorType == null)
            {
                RegisterCommand(commandType);
                return;
            }

            Guard.NotNull(commandType, nameof(commandType));

            if (!typeof(ICliAsyncCommandExecutor<>).IsAssignableFrom(executorType))
                throw new ArgumentException($"The executorType needs to implement the {typeof(ICliAsyncCommandExecutor<>).FullName} interface.", nameof(executorType));
            if (!executorType.GenericTypeArguments[0].IsAssignableFrom(commandType))
                throw new ArgumentException($"The first generic argument type of the {typeof(ICliAsyncCommandExecutor<>).Name} interface of class {executorType.FullName} needs to be assignable from type {commandType.FullName}.", nameof(executorType));

            _commands.Add(CliCommandInfo.From(commandType, executorType));
        }

        public void RegisterCommand(Type commandType, Func<object, Task<int>> executorFunction)
            => _commands.Add(CliCommandInfo.From(commandType, executorFunction));

        public void RegisterCommand<TCommand>(Func<TCommand, Task<int>> executorFunction)
            => _commands.Add(CliCommandInfo.From(executorFunction));

        public void RegisterCommand<TCommand>()
            where TCommand : ICliAsyncCommandExecutor
            => _commands.Add(CliCommandInfo.From<TCommand>());

        public void RegisterCommand<TCommand, TExecutor>()
            where TExecutor : ICliAsyncCommandExecutor<TCommand>
            => _commands.Add(CliCommandInfo.From<TCommand, TExecutor>());

        public Task<int> RunAsync(string[] args)
        {
            // TODO
            return Task.FromResult(0);
        }
    }
}
