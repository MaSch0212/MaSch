using MaSch.Console.Cli.Configuration;
using MaSch.Console.Cli.Runtime;
using MaSch.Core;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

#pragma warning disable SA1402 // File may only contain a single type

namespace MaSch.Console.Cli
{
    /// <summary>
    /// Base class for comand line application builders.
    /// </summary>
    /// <typeparam name="TApplication">The type of application that is built.</typeparam>
    /// <typeparam name="TBuilder">The type of the builder (the class that derived from this class).</typeparam>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Base class")]
    public abstract class CliApplicationBuilderBase<TApplication, TBuilder>
        where TApplication : ICliApplicationBase
        where TBuilder : CliApplicationBuilderBase<TApplication, TBuilder>
    {
        /// <summary>
        /// Gets the application instance that is built.
        /// </summary>
        protected TApplication Application { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliApplicationBuilderBase{TApplication, TBuilder}"/> class.
        /// </summary>
        /// <param name="application">A base instance of the application to built.</param>
        protected CliApplicationBuilderBase(TApplication application)
        {
            Guard.NotNull(application, nameof(application));
            Application = application;
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> to the final application.
        /// </summary>
        /// <param name="command">The command to register.</param>
        /// <returns>Self reference to this builder.</returns>
        public virtual TBuilder WithCommand(ICliCommandInfo command)
            => Exec(x => x.RegisterCommand(command));

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from an executable command type to the final application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/> and implements either the <see cref="ICliCommandExecutor"/> or the <see cref="ICliAsyncCommandExecutor"/> interface.</param>
        /// <returns>Self reference to this builder.</returns>
        public virtual TBuilder WithCommand(Type commandType)
            => Exec(x => x.RegisterCommand(commandType));

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from an executable command type to the final application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/> and implements either the <see cref="ICliCommandExecutor"/> or the <see cref="ICliAsyncCommandExecutor"/> interface.</param>
        /// <param name="optionsInstance">An instance of <paramref name="commandType"/> that should be used when the command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public virtual TBuilder WithCommand(Type commandType, object? optionsInstance)
            => Exec(x => ((ICliApplicationBase)x).RegisterCommand(commandType, optionsInstance));

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command and executor type to the final application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="executorType">The executor type that implements either the <see cref="ICliCommandExecutor{TCommand}"/> or the <see cref="ICliAsyncCommandExecutor{TCommand}"/> interface for the <paramref name="commandType"/>.</param>
        /// <returns>Self reference to this builder.</returns>
        public virtual TBuilder WithCommand(Type commandType, Type? executorType)
            => Exec(x => ((ICliApplicationBase)x).RegisterCommand(commandType, executorType));

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command and executor type to the final application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="optionsInstance">An instance of <paramref name="commandType"/> that should be used when the command is executed.</param>
        /// <param name="executorType">The executor type that implements either the <see cref="ICliCommandExecutor{TCommand}"/> or the <see cref="ICliAsyncCommandExecutor{TCommand}"/> interface for the <paramref name="commandType"/>.</param>
        /// <returns>Self reference to this builder.</returns>
        public virtual TBuilder WithCommand(Type commandType, object? optionsInstance, Type? executorType)
            => Exec(x => x.RegisterCommand(commandType, optionsInstance, executorType));

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command and executor type to the final application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="executorType">The executor type that implements either the <see cref="ICliCommandExecutor{TCommand}"/> or the <see cref="ICliAsyncCommandExecutor{TCommand}"/> interface for the <paramref name="commandType"/>.</param>
        /// <param name="executorInstance">An instance of <paramref name="executorType"/> that should be used when the command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public virtual TBuilder WithCommand(Type commandType, Type? executorType, object? executorInstance)
            => Exec(x => x.RegisterCommand(commandType, executorType, executorInstance));

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command and executor type to the final application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="optionsInstance">An instance of <paramref name="commandType"/> that should be used when the command is executed.</param>
        /// <param name="executorType">The executor type that implements either the <see cref="ICliCommandExecutor{TCommand}"/> or the <see cref="ICliAsyncCommandExecutor{TCommand}"/> interface for the <paramref name="commandType"/>.</param>
        /// <param name="executorInstance">An instance of <paramref name="executorType"/> that should be used when the command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public virtual TBuilder WithCommand(Type commandType, object? optionsInstance, Type? executorType, object? executorInstance)
            => Exec(x => x.RegisterCommand(commandType, optionsInstance, executorType, executorInstance));

        /// <summary>
        /// Builds the application.
        /// </summary>
        /// <returns>The built application instance.</returns>
        public virtual TApplication Build()
            => Application;

        /// <summary>
        /// Executes a builder action.
        /// </summary>
        /// <param name="action">The builder action to exeucte.</param>
        /// <returns>Self reference to this builder.</returns>
        protected TBuilder Exec(Action<TApplication> action)
        {
            action(Application);
            return (TBuilder)this;
        }
    }

    /// <summary>
    /// Builder that is used to build a <see cref="ICliApplication"/> instance.
    /// </summary>
    public class CliApplicationBuilder : CliApplicationBuilderBase<ICliApplication, CliApplicationBuilder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CliApplicationBuilder"/> class.
        /// </summary>
        public CliApplicationBuilder()
            : this(new CliApplication())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliApplicationBuilder"/> class.
        /// </summary>
        /// <param name="application">A base instance of the application to built.</param>
        protected CliApplicationBuilder(ICliApplication application)
            : base(application)
        {
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command type and an executor function to the final application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="executorFunction">The executor function that is called when the created command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public CliApplicationBuilder WithCommand(Type commandType, Func<object, int> executorFunction)
            => Exec(x => x.RegisterCommand(commandType, executorFunction));

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command type and an executor function to the final application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="optionsInstance">An instance of <paramref name="commandType"/> that should be used when the command is executed.</param>
        /// <param name="executorFunction">The executor function that is called when the created command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public CliApplicationBuilder WithCommand(Type commandType, object? optionsInstance, Func<object, int> executorFunction)
            => Exec(x => x.RegisterCommand(commandType, optionsInstance, executorFunction));

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command type and an executor function to the final application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <param name="executorFunction">The executor function that is called when the created command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public CliApplicationBuilder WithCommand<TCommand>(Func<TCommand, int> executorFunction)
            => Exec(x => x.RegisterCommand(executorFunction));

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command type and an executor function to the final application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <param name="optionsInstance">An instance of <typeparamref name="TCommand"/> that should be used when the command is executed.</param>
        /// <param name="executorFunction">The executor function that is called when the created command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public CliApplicationBuilder WithCommand<TCommand>(TCommand optionsInstance, Func<TCommand, int> executorFunction)
            => Exec(x => x.RegisterCommand(optionsInstance, executorFunction));

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from an executable command type to the final application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <returns>Self reference to this builder.</returns>
        public CliApplicationBuilder WithCommand<TCommand>()
            where TCommand : ICliCommandExecutor
            => Exec(x => x.RegisterCommand<TCommand>());

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from an executable command type to the final application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <param name="optionsInstance">An instance of <typeparamref name="TCommand"/> that should be used when the command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public CliApplicationBuilder WithCommand<TCommand>(TCommand optionsInstance)
            where TCommand : ICliCommandExecutor
            => Exec(x => x.RegisterCommand(optionsInstance));

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command and executor type to the final application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <typeparam name="TExecutor">The executor type.</typeparam>
        /// <returns>Self reference to this builder.</returns>
        public CliApplicationBuilder WithCommand<TCommand, TExecutor>()
            where TExecutor : ICliCommandExecutor<TCommand>
            => Exec(x => x.RegisterCommand<TCommand, TExecutor>());

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command and executor type to the final application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <typeparam name="TExecutor">The executor type.</typeparam>
        /// <param name="executorInstance">An instance of <typeparamref name="TExecutor"/> that should be used when the command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public CliApplicationBuilder WithCommand<TCommand, TExecutor>(TExecutor executorInstance)
            where TExecutor : ICliCommandExecutor<TCommand>
            => Exec(x => x.RegisterCommand<TCommand, TExecutor>(executorInstance));

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command and executor type to the final application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <typeparam name="TExecutor">The executor type.</typeparam>
        /// <param name="optionsInstance">An instance of <typeparamref name="TCommand"/> that should be used when the command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public CliApplicationBuilder WithCommand<TCommand, TExecutor>(TCommand optionsInstance)
            where TExecutor : ICliCommandExecutor<TCommand>
            => Exec(x => x.RegisterCommand<TCommand, TExecutor>(optionsInstance));

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command and executor type to the final application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <typeparam name="TExecutor">The executor type.</typeparam>
        /// <param name="optionsInstance">An instance of <typeparamref name="TCommand"/> that should be used when the command is executed.</param>
        /// <param name="executorInstance">An instance of <typeparamref name="TExecutor"/> that should be used when the command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public CliApplicationBuilder WithCommand<TCommand, TExecutor>(TCommand optionsInstance, TExecutor executorInstance)
            where TExecutor : ICliCommandExecutor<TCommand>
            => Exec(x => x.RegisterCommand(optionsInstance, executorInstance));

        /// <summary>
        /// Configures the options of the final application.
        /// </summary>
        /// <param name="action">The configuration action.</param>
        /// <returns>Self reference to this builder.</returns>
        public CliApplicationBuilder Configure(Action<CliApplicationOptions> action)
            => Exec(x => action(x.Options));
    }

    /// <summary>
    /// Builder that is used to build a <see cref="ICliAsyncApplication"/> instance.
    /// </summary>
    public class CliAsyncApplicationBuilder : CliApplicationBuilderBase<ICliAsyncApplication, CliAsyncApplicationBuilder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CliAsyncApplicationBuilder"/> class.
        /// </summary>
        public CliAsyncApplicationBuilder()
            : base(new CliAsyncApplication())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliAsyncApplicationBuilder"/> class.
        /// </summary>
        /// <param name="application">A base instance of the application to built.</param>
        protected CliAsyncApplicationBuilder(ICliAsyncApplication application)
            : base(application)
        {
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command type and an executor function to the final application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="executorFunction">The executor function that is called when the created command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public CliAsyncApplicationBuilder WithCommand(Type commandType, Func<object, Task<int>> executorFunction)
            => Exec(x => x.RegisterCommand(commandType, executorFunction));

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command type and an executor function to the final application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="optionsInstance">An instance of <paramref name="commandType"/> that should be used when the command is executed.</param>
        /// <param name="executorFunction">The executor function that is called when the created command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public CliAsyncApplicationBuilder WithCommand(Type commandType, object? optionsInstance, Func<object, Task<int>> executorFunction)
            => Exec(x => x.RegisterCommand(commandType, optionsInstance, executorFunction));

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command type and an executor function to the final application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <param name="executorFunction">The executor function that is called when the created command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public CliAsyncApplicationBuilder WithCommand<TCommand>(Func<TCommand, Task<int>> executorFunction)
            => Exec(x => x.RegisterCommand(executorFunction));

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command type and an executor function to the final application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <param name="optionsInstance">An instance of <typeparamref name="TCommand"/> that should be used when the command is executed.</param>
        /// <param name="executorFunction">The executor function that is called when the created command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public CliAsyncApplicationBuilder WithCommand<TCommand>(TCommand optionsInstance, Func<TCommand, Task<int>> executorFunction)
            => Exec(x => x.RegisterCommand(optionsInstance, executorFunction));

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from an executable command type to the final application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <returns>Self reference to this builder.</returns>
        public CliAsyncApplicationBuilder WithCommand<TCommand>()
            where TCommand : ICliAsyncCommandExecutor
            => Exec(x => x.RegisterCommand<TCommand>());

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from an executable command type to the final application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <param name="optionsInstance">An instance of <typeparamref name="TCommand"/> that should be used when the command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public CliAsyncApplicationBuilder WithCommand<TCommand>(TCommand optionsInstance)
            where TCommand : ICliAsyncCommandExecutor
            => Exec(x => x.RegisterCommand(optionsInstance));

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command and executor type to the final application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <typeparam name="TExecutor">The executor type.</typeparam>
        /// <returns>Self reference to this builder.</returns>
        public CliAsyncApplicationBuilder WithCommand<TCommand, TExecutor>()
            where TExecutor : ICliAsyncCommandExecutor<TCommand>
            => Exec(x => x.RegisterCommand<TCommand, TExecutor>());

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command and executor type to the final application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <typeparam name="TExecutor">The executor type.</typeparam>
        /// <param name="executorInstance">An instance of <typeparamref name="TExecutor"/> that should be used when the command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public CliAsyncApplicationBuilder WithCommand<TCommand, TExecutor>(TExecutor executorInstance)
            where TExecutor : ICliAsyncCommandExecutor<TCommand>
            => Exec(x => x.RegisterCommand<TCommand, TExecutor>(executorInstance));

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command and executor type to the final application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <typeparam name="TExecutor">The executor type.</typeparam>
        /// <param name="optionsInstance">An instance of <typeparamref name="TCommand"/> that should be used when the command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public CliAsyncApplicationBuilder WithCommand<TCommand, TExecutor>(TCommand optionsInstance)
            where TExecutor : ICliAsyncCommandExecutor<TCommand>
            => Exec(x => x.RegisterCommand<TCommand, TExecutor>(optionsInstance));

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command and executor type to the final application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <typeparam name="TExecutor">The executor type.</typeparam>
        /// <param name="optionsInstance">An instance of <typeparamref name="TCommand"/> that should be used when the command is executed.</param>
        /// <param name="executorInstance">An instance of <typeparamref name="TExecutor"/> that should be used when the command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public CliAsyncApplicationBuilder WithCommand<TCommand, TExecutor>(TCommand optionsInstance, TExecutor executorInstance)
            where TExecutor : ICliAsyncCommandExecutor<TCommand>
            => Exec(x => x.RegisterCommand(optionsInstance, executorInstance));

        /// <summary>
        /// Configures the options of the final application.
        /// </summary>
        /// <param name="action">The configuration action.</param>
        /// <returns>Self reference to this builder.</returns>
        public CliAsyncApplicationBuilder Configure(Action<CliApplicationOptions> action)
            => Exec(x => action(x.Options));
    }
}
