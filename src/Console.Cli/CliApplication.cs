﻿using MaSch.Console.Cli.Runtime;
using MaSch.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MaSch.Console.Cli
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Base class")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Base class")]
    public abstract class CliApplicationBase : ICliApplicationBase
    {
        private IReadOnlyCollection<ICliCommandInfo>? _readOnlyCommands;

        protected CliCommandInfoCollection CommandsCollection { get; }
        protected abstract Type ExecutorType { get; }
        protected abstract Type GenericExecutorType { get; }

        public IReadOnlyCollection<ICliCommandInfo> Commands => _readOnlyCommands ??= CommandsCollection.AsReadOnly();
        public CliApplicationOptions Options { get; }

        protected CliApplicationBase()
            : this(null)
        {
        }

        protected CliApplicationBase(CliApplicationOptions? options)
        {
            VerifyTypes();
            Options = options ?? new CliApplicationOptions();
            CommandsCollection = new CliCommandInfoCollection();
        }

        public void RegisterCommand(Type commandType)
            => RegisterCommand(commandType, null);

        public void RegisterCommand(Type commandType, object? optionsInstance)
        {
            Guard.NotNull(commandType, nameof(commandType));
            if (!ExecutorType.IsAssignableFrom(commandType))
            {
                throw new ArgumentException(
                    ExecutorType.IsInterface
                        ? $"The commandType needs to implement the {ExecutorType.FullName} interface."
                        : $"The commandType needs to derive from the {ExecutorType.FullName} class.",
                    nameof(commandType));
            }

            CommandsCollection.Add(CliCommandInfo.From(commandType, optionsInstance));
        }

        public void RegisterCommand(Type commandType, Type? executorType)
            => RegisterCommand(commandType, null, executorType, null);

        public void RegisterCommand(Type commandType, object? optionsInstance, Type? executorType)
            => RegisterCommand(commandType, optionsInstance, executorType, null);

        public void RegisterCommand(Type commandType, Type? executorType, object? executorInstance)
            => RegisterCommand(commandType, null, executorType, executorInstance);

        public void RegisterCommand(Type commandType, object? optionsInstance, Type? executorType, object? executorInstance)
        {
            if (executorType == null)
            {
                RegisterCommand(commandType, optionsInstance);
                return;
            }

            Guard.NotNull(commandType, nameof(commandType));

            if (!GenericExecutorType.IsAssignableFrom(executorType))
            {
                throw new ArgumentException(
                    GenericExecutorType.IsInterface
                        ? $"The executorType needs to implement the {GenericExecutorType.FullName} interface."
                        : $"The executorType needs to derive from the {GenericExecutorType.FullName} class.",
                    nameof(executorType));
            }

            if (!executorType.GenericTypeArguments[0].IsAssignableFrom(commandType))
            {
                throw new ArgumentException(
                    GenericExecutorType.IsInterface
                        ? $"The first generic argument type of the {GenericExecutorType.Name} interface of class {executorType.FullName} needs to be assignable from type {commandType.FullName}."
                        : $"The first generic argument type of the base class {GenericExecutorType.Name} of class {executorType.FullName} needs to be assignable from type {commandType.FullName}.",
                    nameof(executorType));
            }

            CommandsCollection.Add(CliCommandInfo.From(commandType, optionsInstance, executorType, executorInstance));
        }

        protected bool TryParseArguments(string[] args, [NotNullWhen(true)] out ICliCommandInfo? command, [NotNullWhen(true)] out object? options)
        {
            var result = CliArgumentParser.Parse(args, Options, CommandsCollection);
            if (result.Success)
            {
                command = result.Command!;
                options = result.Options!;
                return true;
            }
            else
            {
                options = command = null;
                Options.HelpPage.Write(this, result.Errors);
                return false;
            }
        }

        private void VerifyTypes()
        {
            Guard.NotNull(ExecutorType, nameof(ExecutorType));
            Guard.NotNull(GenericExecutorType, nameof(GenericExecutorType));

            if (!GenericExecutorType.IsGenericType || GenericExecutorType.GetGenericArguments().Length != 1)
                throw new ArgumentException($"The generic executor type needs to be a generic type with exactly one gneric argument.");
        }
    }

    public class CliApplication : CliApplicationBase, ICliApplication
    {
        protected override Type ExecutorType { get; } = typeof(ICliCommandExecutor);
        protected override Type GenericExecutorType { get; } = typeof(ICliCommandExecutor<>);

        public CliApplication()
            : base()
        {
        }

        public CliApplication(CliApplicationOptions? options)
            : base(options)
        {
        }

        public new void RegisterCommand(Type commandType)
            => base.RegisterCommand(commandType);

        public new void RegisterCommand(Type commandType, object? optionsInstance)
            => base.RegisterCommand(commandType, optionsInstance);

        public new void RegisterCommand(Type commandType, Type? executorType)
            => base.RegisterCommand(commandType, executorType);

        public new void RegisterCommand(Type commandType, object? optionsInstance, Type? executorType)
            => base.RegisterCommand(commandType, optionsInstance, executorType);

        public new void RegisterCommand(Type commandType, Type? executorType, object? executorInstance)
            => base.RegisterCommand(commandType, executorType, executorInstance);

        public new void RegisterCommand(Type commandType, object? optionsInstance, Type? executorType, object? executorInstance)
            => base.RegisterCommand(commandType, optionsInstance, executorType, executorInstance);

        public void RegisterCommand(Type commandType, Func<object, int> executorFunction)
            => CommandsCollection.Add(CliCommandInfo.From(commandType, executorFunction));

        public void RegisterCommand(Type commandType, Func<object, int> executorFunction, object? optionsInstance)
            => CommandsCollection.Add(CliCommandInfo.From(commandType, executorFunction, optionsInstance));

        public void RegisterCommand<TCommand>(Func<TCommand, int> executorFunction)
            => CommandsCollection.Add(CliCommandInfo.From(executorFunction));

        public void RegisterCommand<TCommand>(Func<TCommand, int> executorFunction, TCommand optionsInstance)
            => CommandsCollection.Add(CliCommandInfo.From(executorFunction, optionsInstance));

        public void RegisterCommand<TCommand>()
            where TCommand : ICliCommandExecutor
            => CommandsCollection.Add(CliCommandInfo.From<TCommand>());

        public void RegisterCommand<TCommand>(TCommand commandInstance)
            where TCommand : ICliCommandExecutor
            => CommandsCollection.Add(CliCommandInfo.From(commandInstance));

        public void RegisterCommand<TCommand, TExecutor>()
            where TExecutor : ICliCommandExecutor<TCommand>
            => CommandsCollection.Add(CliCommandInfo.From<TCommand, TExecutor>());

        public void RegisterCommand<TCommand, TExecutor>(TExecutor executorInstance)
            where TExecutor : ICliCommandExecutor<TCommand>
            => CommandsCollection.Add(CliCommandInfo.From<TCommand, TExecutor>(executorInstance));

        public void RegisterCommand<TCommand, TExecutor>(TCommand commandInstance)
            where TExecutor : ICliCommandExecutor<TCommand>
            => CommandsCollection.Add(CliCommandInfo.From<TCommand, TExecutor>(commandInstance));

        public void RegisterCommand<TCommand, TExecutor>(TCommand commandInstance, TExecutor executorInstance)
            where TExecutor : ICliCommandExecutor<TCommand>
            => CommandsCollection.Add(CliCommandInfo.From(commandInstance, executorInstance));

        public int Run(string[] args)
        {
            if (TryParseArguments(args, out var command, out var options))
                return command.Execute(options);
            return 0;
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Async counterpart to CliApplication.")]
    public class CliAsyncApplication : CliApplicationBase, ICliAsyncApplication
    {
        protected override Type ExecutorType { get; } = typeof(ICliAsyncCommandExecutor);
        protected override Type GenericExecutorType { get; } = typeof(ICliAsyncCommandExecutor<>);

        public CliAsyncApplication()
            : base()
        {
        }

        public CliAsyncApplication(CliApplicationOptions? options)
            : base(options)
        {
        }

        public new void RegisterCommand(Type commandType)
            => base.RegisterCommand(commandType);

        public new void RegisterCommand(Type commandType, object? optionsInstance)
            => base.RegisterCommand(commandType, optionsInstance);

        public new void RegisterCommand(Type commandType, Type? executorType)
            => base.RegisterCommand(commandType, executorType);

        public new void RegisterCommand(Type commandType, object? optionsInstance, Type? executorType)
            => base.RegisterCommand(commandType, optionsInstance, executorType);

        public new void RegisterCommand(Type commandType, Type? executorType, object? executorInstance)
            => base.RegisterCommand(commandType, executorType, executorInstance);

        public new void RegisterCommand(Type commandType, object? optionsInstance, Type? executorType, object? executorInstance)
            => base.RegisterCommand(commandType, optionsInstance, executorType, executorInstance);

        public void RegisterCommand(Type commandType, Func<object, Task<int>> executorFunction)
            => CommandsCollection.Add(CliCommandInfo.From(commandType, executorFunction));

        public void RegisterCommand(Type commandType, Func<object, Task<int>> executorFunction, object? optionsInstance)
            => CommandsCollection.Add(CliCommandInfo.From(commandType, executorFunction, optionsInstance));

        public void RegisterCommand<TCommand>(Func<TCommand, Task<int>> executorFunction)
            => CommandsCollection.Add(CliCommandInfo.From(executorFunction));

        public void RegisterCommand<TCommand>(Func<TCommand, Task<int>> executorFunction, TCommand optionsInstance)
            => CommandsCollection.Add(CliCommandInfo.From(executorFunction, optionsInstance));

        public void RegisterCommand<TCommand>()
            where TCommand : ICliAsyncCommandExecutor
            => CommandsCollection.Add(CliCommandInfo.From<TCommand>());

        public void RegisterCommand<TCommand>(TCommand commandInstance)
            where TCommand : ICliAsyncCommandExecutor
            => CommandsCollection.Add(CliCommandInfo.From(commandInstance));

        public void RegisterCommand<TCommand, TExecutor>()
            where TExecutor : ICliAsyncCommandExecutor<TCommand>
            => CommandsCollection.Add(CliCommandInfo.From<TCommand, TExecutor>());

        public void RegisterCommand<TCommand, TExecutor>(TExecutor executorInstance)
            where TExecutor : ICliAsyncCommandExecutor<TCommand>
            => CommandsCollection.Add(CliCommandInfo.From<TCommand, TExecutor>(executorInstance));

        public void RegisterCommand<TCommand, TExecutor>(TCommand commandInstance)
            where TExecutor : ICliAsyncCommandExecutor<TCommand>
            => CommandsCollection.Add(CliCommandInfo.From<TCommand, TExecutor>(commandInstance));

        public void RegisterCommand<TCommand, TExecutor>(TCommand commandInstance, TExecutor executorInstance)
            where TExecutor : ICliAsyncCommandExecutor<TCommand>
            => CommandsCollection.Add(CliCommandInfo.From(commandInstance, executorInstance));

        public async Task<int> RunAsync(string[] args)
        {
            if (TryParseArguments(args, out var command, out var options))
                return await command.ExecuteAsync(options);
            return 0;
        }
    }
}
