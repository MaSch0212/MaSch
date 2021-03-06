﻿using System;
using System.Threading.Tasks;

namespace MaSch.Console.Cli.Runtime
{
    /// <inheritdoc/>
    public class CliCommandFactory : ICliCommandFactory
    {
        /// <inheritdoc/>
        public ICliCommandInfo Create<TCommand>()
            => new CliCommandInfo(typeof(TCommand), null, null, null, null);

        /// <inheritdoc/>
        public ICliCommandInfo Create<TCommand>(TCommand optionsInstance)
            => new CliCommandInfo(typeof(TCommand), null, optionsInstance, null, null);

        /// <inheritdoc/>
        public ICliCommandInfo Create(Type commandType)
            => new CliCommandInfo(commandType, null, null, null, null);

        /// <inheritdoc/>
        public ICliCommandInfo Create(Type commandType, object? optionsInstance)
            => new CliCommandInfo(commandType, null, optionsInstance, null, null);

        /// <inheritdoc/>
        public ICliCommandInfo Create<TCommand, TExecutor>()
            where TExecutor : ICliExecutorBase<TCommand>
            => new CliCommandInfo(typeof(TCommand), typeof(TExecutor), null, null, null);

        /// <inheritdoc/>
        public ICliCommandInfo Create<TCommand, TExecutor>(TExecutor executorInstance)
            where TExecutor : ICliExecutorBase<TCommand>
            => new CliCommandInfo(typeof(TCommand), typeof(TExecutor), null, null, executorInstance);

        /// <inheritdoc/>
        public ICliCommandInfo Create<TCommand, TExecutor>(TCommand optionsInstance)
            where TExecutor : ICliExecutorBase<TCommand>
            => new CliCommandInfo(typeof(TCommand), typeof(TExecutor), optionsInstance, null, null);

        /// <inheritdoc/>
        public ICliCommandInfo Create<TCommand, TExecutor>(TCommand optionsInstance, TExecutor executorInstance)
            where TExecutor : ICliExecutorBase<TCommand>
            => new CliCommandInfo(typeof(TCommand), typeof(TExecutor), optionsInstance, null, executorInstance);

        /// <inheritdoc/>
        public ICliCommandInfo Create(Type commandType, Type? executorType)
            => new CliCommandInfo(commandType, executorType, null, null, null);

        /// <inheritdoc/>
        public ICliCommandInfo Create(Type commandType, Type? executorType, object? executorInstance)
            => new CliCommandInfo(commandType, executorType, null, null, executorInstance);

        /// <inheritdoc/>
        public ICliCommandInfo Create(Type commandType, object? optionsInstance, Type? executorType)
            => new CliCommandInfo(commandType, executorType, optionsInstance, null, null);

        /// <inheritdoc/>
        public ICliCommandInfo Create(Type commandType, object? optionsInstance, Type? executorType, object? executorInstance)
            => new CliCommandInfo(commandType, executorType, optionsInstance, null, executorInstance);

        /// <inheritdoc/>
        public ICliCommandInfo Create<TCommand>(Func<CliExecutionContext, TCommand, int> executorFunction)
            => new CliCommandInfo(typeof(TCommand), null, null, executorFunction, null);

        /// <inheritdoc/>
        public ICliCommandInfo Create<TCommand>(TCommand optionsInstance, Func<CliExecutionContext, TCommand, int> executorFunction)
            => new CliCommandInfo(typeof(TCommand), null, optionsInstance, executorFunction, null);

        /// <inheritdoc/>
        public ICliCommandInfo Create<TCommand>(Func<CliExecutionContext, TCommand, Task<int>> executorFunction)
            => new CliCommandInfo(typeof(TCommand), null, null, executorFunction, null);

        /// <inheritdoc/>
        public ICliCommandInfo Create<TCommand>(TCommand optionsInstance, Func<CliExecutionContext, TCommand, Task<int>> executorFunction)
            => new CliCommandInfo(typeof(TCommand), null, optionsInstance, executorFunction, null);

        /// <inheritdoc/>
        public ICliCommandInfo Create(Type commandType, Func<CliExecutionContext, object, int> executorFunction)
            => new CliCommandInfo(commandType, null, null, executorFunction, null);

        /// <inheritdoc/>
        public ICliCommandInfo Create(Type commandType, object? optionsInstance, Func<CliExecutionContext, object, int> executorFunction)
            => new CliCommandInfo(commandType, null, optionsInstance, executorFunction, null);

        /// <inheritdoc/>
        public ICliCommandInfo Create(Type commandType, Func<CliExecutionContext, object, Task<int>> executorFunction)
            => new CliCommandInfo(commandType, null, null, executorFunction, null);

        /// <inheritdoc/>
        public ICliCommandInfo Create(Type commandType, object? optionsInstance, Func<CliExecutionContext, object, Task<int>> executorFunction)
            => new CliCommandInfo(commandType, null, optionsInstance, executorFunction, null);
    }
}
