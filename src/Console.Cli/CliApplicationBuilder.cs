using MaSch.Console.Cli.Configuration;
using MaSch.Console.Cli.Runtime;
using MaSch.Console.Cli.Runtime.Validators;
using MaSch.Core;
using MaSch.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
        where TApplication : class, ICliApplicationBase
        where TBuilder : CliApplicationBuilderBase<TApplication, TBuilder>
    {
        private ICliCommandFactory? _commandFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CliApplicationBuilderBase{TApplication, TBuilder}"/> class.
        /// </summary>
        protected CliApplicationBuilderBase()
            : this(new ServiceCollection(), new CliCommandInfoCollection(), new CliApplicationOptions())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliApplicationBuilderBase{TApplication, TBuilder}"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="commands">The commands.</param>
        /// <param name="options">The options.</param>
        protected CliApplicationBuilderBase(IServiceCollection services, ICliCommandInfoCollection commands, CliApplicationOptions options)
        {
            Services = services;
            Commands = commands;
            Options = options;
        }

        /// <summary>
        /// Gets the command factory to use when creating commands.
        /// </summary>
        public ICliCommandFactory CommandFactory
        {
            get
            {
                var factory = _commandFactory;
                if (factory is null && Services.Any(x => x.ServiceType == typeof(ICliCommandFactory)))
                {
                    using var serviceProvider = Services.BuildServiceProvider();
                    factory = serviceProvider.GetService<ICliCommandFactory>();
                }

                if (factory is null)
                {
                    factory = _commandFactory = new CliCommandFactory();
                }

                return factory;
            }
        }

        /// <summary>
        /// Gets the services.
        /// </summary>
        protected IServiceCollection Services { get; }

        /// <summary>
        /// Gets the commands.
        /// </summary>
        protected ICliCommandInfoCollection Commands { get; }

        /// <summary>
        /// Gets the options.
        /// </summary>
        protected CliApplicationOptions Options { get; }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> to the final application.
        /// </summary>
        /// <param name="command">The command to register.</param>
        /// <returns>Self reference to this builder.</returns>
        public virtual TBuilder WithCommand(ICliCommandInfo command)
        {
            Commands.Add(command);
            AddTypeToServices(command.CommandType, command.OptionsInstance);
            return (TBuilder)this;
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from an executable command type to the final application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/> and implements either the <see cref="ICliExecutable"/> or the <see cref="ICliAsyncExecutable"/> interface.</param>
        /// <returns>Self reference to this builder.</returns>
        public virtual TBuilder WithCommand(Type commandType)
        {
            Commands.Add(CommandFactory.Create(commandType));
            AddTypeToServices(commandType, null);
            return (TBuilder)this;
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from an executable command type to the final application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/> and implements either the <see cref="ICliExecutable"/> or the <see cref="ICliAsyncExecutable"/> interface.</param>
        /// <param name="optionsInstance">An instance of <paramref name="commandType"/> that should be used when the command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public virtual TBuilder WithCommand(Type commandType, object? optionsInstance)
        {
            Commands.Add(CommandFactory.Create(commandType, optionsInstance));
            AddTypeToServices(commandType, optionsInstance);
            return (TBuilder)this;
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command and executor type to the final application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="executorType">The executor type that implements either the <see cref="ICliExecutor{TCommand}"/> or the <see cref="ICliAsyncExecutor{TCommand}"/> interface for the <paramref name="commandType"/>.</param>
        /// <returns>Self reference to this builder.</returns>
        public virtual TBuilder WithCommand(Type commandType, Type? executorType)
        {
            Commands.Add(CommandFactory.Create(commandType, executorType));
            AddTypeToServices(commandType, null);
            AddTypeToServices(executorType, null);
            return (TBuilder)this;
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command and executor type to the final application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="optionsInstance">An instance of <paramref name="commandType"/> that should be used when the command is executed.</param>
        /// <param name="executorType">The executor type that implements either the <see cref="ICliExecutor{TCommand}"/> or the <see cref="ICliAsyncExecutor{TCommand}"/> interface for the <paramref name="commandType"/>.</param>
        /// <returns>Self reference to this builder.</returns>
        public virtual TBuilder WithCommand(Type commandType, object? optionsInstance, Type? executorType)
        {
            Commands.Add(CommandFactory.Create(commandType, optionsInstance, executorType));
            AddTypeToServices(commandType, optionsInstance);
            AddTypeToServices(executorType, null);
            return (TBuilder)this;
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command and executor type to the final application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="executorType">The executor type that implements either the <see cref="ICliExecutor{TCommand}"/> or the <see cref="ICliAsyncExecutor{TCommand}"/> interface for the <paramref name="commandType"/>.</param>
        /// <param name="executorInstance">An instance of <paramref name="executorType"/> that should be used when the command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public virtual TBuilder WithCommand(Type commandType, Type? executorType, object? executorInstance)
        {
            Commands.Add(CommandFactory.Create(commandType, executorType, executorInstance));
            AddTypeToServices(commandType, null);
            AddTypeToServices(executorType, executorInstance);
            return (TBuilder)this;
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command and executor type to the final application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="optionsInstance">An instance of <paramref name="commandType"/> that should be used when the command is executed.</param>
        /// <param name="executorType">The executor type that implements either the <see cref="ICliExecutor{TCommand}"/> or the <see cref="ICliAsyncExecutor{TCommand}"/> interface for the <paramref name="commandType"/>.</param>
        /// <param name="executorInstance">An instance of <paramref name="executorType"/> that should be used when the command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public virtual TBuilder WithCommand(Type commandType, object? optionsInstance, Type? executorType, object? executorInstance)
        {
            Commands.Add(CommandFactory.Create(commandType, optionsInstance, executorType, executorInstance));
            AddTypeToServices(commandType, optionsInstance);
            AddTypeToServices(executorType, executorInstance);
            return (TBuilder)this;
        }

        /// <summary>
        /// Configures the options of the final application.
        /// </summary>
        /// <param name="configureDelegate">The configuration delegate.</param>
        /// <returns>Self reference to this builder.</returns>
        public virtual TBuilder ConfigureOptions(Action<CliApplicationOptions> configureDelegate)
        {
            configureDelegate(Options);
            return (TBuilder)this;
        }

        /// <summary>
        /// Adds services to the container. This can be called multiple times and the results will be additive.
        /// </summary>
        /// <param name="configureDelegate">The configuration delegate.</param>
        /// <returns>Self reference to this builder.</returns>
        public virtual TBuilder ConfigureServices(Action<IServiceCollection> configureDelegate)
        {
            var preFactoryDescriptor = Services.FirstOrDefault(x => x.ServiceType == typeof(ICliCommandFactory));

            configureDelegate(Services);

            var postFactoryDescriptor = Services.FirstOrDefault(x => x.ServiceType == typeof(ICliCommandFactory));
            if (!Equals(preFactoryDescriptor, postFactoryDescriptor))
                _commandFactory = null;

            return (TBuilder)this;
        }

        /// <summary>
        /// Builds the application.
        /// </summary>
        /// <returns>The built application instance.</returns>
        public TApplication Build()
        {
            var result = new Lazy<TApplication>(() => OnBuild());

            // Add common services
            Services.TryAddSingleton<IConsoleService, ConsoleService>();
            Services.TryAddSingleton<ICliArgumentParser, CliArgumentParser>();
            Services.TryAddSingleton<ICliHelpPage, CliHelpPage>();
            if (_commandFactory != null)
                Services.TryAddSingleton(_commandFactory);
            else
                Services.TryAddSingleton<ICliCommandFactory, CliCommandFactory>();
            _ = Services.AddSingleton<ICliApplicationBase>(x => result.Value);

            // Add common validators
            _ = Services.AddSingleton<ICliValidator<object>, RequiredValidator>();

            return result.Value;
        }

        /// <summary>
        /// Called when the application is built.
        /// </summary>
        /// <returns>The built application.</returns>
        protected abstract TApplication OnBuild();

        /// <summary>
        /// Adds a type with its instance to the services.
        /// </summary>
        /// <param name="type">The type to add.</param>
        /// <param name="instance">The instance to use.</param>
        protected void AddTypeToServices(Type? type, object? instance)
        {
            if (type is null)
                return;

            if (instance is null)
                Services.TryAddScoped(type);
            else
                Services.TryAdd(ServiceDescriptor.Singleton(type, instance));
        }

        /// <summary>
        /// Adds a type with its instance to the services.
        /// </summary>
        /// <typeparam name="T">The type to add.</typeparam>
        /// <param name="instance">The instance to use.</param>
        protected void AddTypeToServices<T>(T? instance)
        {
            AddTypeToServices(typeof(T), instance);
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
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliApplicationBuilder"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="commands">The commands.</param>
        /// <param name="options">The options.</param>
        protected CliApplicationBuilder(IServiceCollection services, ICliCommandInfoCollection commands, CliApplicationOptions options)
            : base(services, commands, options)
        {
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> to the final application.
        /// </summary>
        /// <param name="command">The command to register.</param>
        /// <returns>Self reference to this builder.</returns>
        [ExcludeFromCodeCoverage]
        public new CliApplicationBuilder WithCommand(ICliCommandInfo command)
        {
            return base.WithCommand(command);
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from an executable command type to the final application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/> and implements the <see cref="ICliExecutable"/> interface.</param>
        /// <returns>Self reference to this builder.</returns>
        [ExcludeFromCodeCoverage]
        public new CliApplicationBuilder WithCommand(Type commandType)
        {
            return base.WithCommand(commandType);
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from an executable command type to the final application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/> and implements the <see cref="ICliExecutable"/> interface.</param>
        /// <param name="optionsInstance">An instance of <paramref name="commandType"/> that should be used when the command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        [ExcludeFromCodeCoverage]
        public new CliApplicationBuilder WithCommand(Type commandType, object? optionsInstance)
        {
            return base.WithCommand(commandType, optionsInstance);
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command and executor type to the final application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="executorType">The executor type that implements the <see cref="ICliExecutor{TCommand}"/> interface for the <paramref name="commandType"/>.</param>
        /// <returns>Self reference to this builder.</returns>
        [ExcludeFromCodeCoverage]
        public new CliApplicationBuilder WithCommand(Type commandType, Type? executorType)
        {
            return base.WithCommand(commandType, executorType);
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command and executor type to the final application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="optionsInstance">An instance of <paramref name="commandType"/> that should be used when the command is executed.</param>
        /// <param name="executorType">The executor type that implements the <see cref="ICliExecutor{TCommand}"/> interface for the <paramref name="commandType"/>.</param>
        /// <returns>Self reference to this builder.</returns>
        [ExcludeFromCodeCoverage]
        public new CliApplicationBuilder WithCommand(Type commandType, object? optionsInstance, Type? executorType)
        {
            return base.WithCommand(commandType, optionsInstance, executorType);
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command and executor type to the final application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="executorType">The executor type that implements the <see cref="ICliExecutor{TCommand}"/> interface for the <paramref name="commandType"/>.</param>
        /// <param name="executorInstance">An instance of <paramref name="executorType"/> that should be used when the command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        [ExcludeFromCodeCoverage]
        public new CliApplicationBuilder WithCommand(Type commandType, Type? executorType, object? executorInstance)
        {
            return base.WithCommand(commandType, executorType, executorInstance);
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command and executor type to the final application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="optionsInstance">An instance of <paramref name="commandType"/> that should be used when the command is executed.</param>
        /// <param name="executorType">The executor type that implements the <see cref="ICliExecutor{TCommand}"/> interface for the <paramref name="commandType"/>.</param>
        /// <param name="executorInstance">An instance of <paramref name="executorType"/> that should be used when the command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        [ExcludeFromCodeCoverage]
        public new CliApplicationBuilder WithCommand(Type commandType, object? optionsInstance, Type? executorType, object? executorInstance)
        {
            return base.WithCommand(commandType, optionsInstance, executorType, executorInstance);
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command type and an executor function to the final application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="executorFunction">The executor function that is called when the created command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public CliApplicationBuilder WithCommand(Type commandType, Func<CliExecutionContext, object, int> executorFunction)
        {
            Commands.Add(CommandFactory.Create(commandType, executorFunction));
            AddTypeToServices(commandType, null);
            return this;
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command type and an executor function to the final application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="optionsInstance">An instance of <paramref name="commandType"/> that should be used when the command is executed.</param>
        /// <param name="executorFunction">The executor function that is called when the created command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public CliApplicationBuilder WithCommand(Type commandType, object? optionsInstance, Func<CliExecutionContext, object, int> executorFunction)
        {
            Commands.Add(CommandFactory.Create(commandType, optionsInstance, executorFunction));
            AddTypeToServices(commandType, optionsInstance);
            return this;
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command type and an executor function to the final application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <param name="executorFunction">The executor function that is called when the created command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public CliApplicationBuilder WithCommand<TCommand>(Func<CliExecutionContext, TCommand, int> executorFunction)
        {
            Commands.Add(CommandFactory.Create(executorFunction));
            AddTypeToServices<TCommand>(default);
            return this;
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command type and an executor function to the final application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <param name="optionsInstance">An instance of <typeparamref name="TCommand"/> that should be used when the command is executed.</param>
        /// <param name="executorFunction">The executor function that is called when the created command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public CliApplicationBuilder WithCommand<TCommand>(TCommand optionsInstance, Func<CliExecutionContext, TCommand, int> executorFunction)
        {
            Commands.Add(CommandFactory.Create(optionsInstance, executorFunction));
            AddTypeToServices(optionsInstance);
            return this;
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from an executable command type to the final application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <returns>Self reference to this builder.</returns>
        public CliApplicationBuilder WithCommand<TCommand>()
        {
            Commands.Add(CommandFactory.Create<TCommand>());
            AddTypeToServices<TCommand>(default);
            return this;
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from an executable command type to the final application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <param name="optionsInstance">An instance of <typeparamref name="TCommand"/> that should be used when the command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public CliApplicationBuilder WithCommand<TCommand>(TCommand optionsInstance)
        {
            Commands.Add(CommandFactory.Create(optionsInstance));
            AddTypeToServices(optionsInstance);
            return this;
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command and executor type to the final application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <typeparam name="TExecutor">The executor type.</typeparam>
        /// <returns>Self reference to this builder.</returns>
        public CliApplicationBuilder WithCommand<TCommand, TExecutor>()
            where TExecutor : ICliExecutor<TCommand>
        {
            Commands.Add(CommandFactory.Create<TCommand, TExecutor>());
            AddTypeToServices<TCommand>(default);
            AddTypeToServices<TExecutor>(default);
            return this;
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command and executor type to the final application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <typeparam name="TExecutor">The executor type.</typeparam>
        /// <param name="executorInstance">An instance of <typeparamref name="TExecutor"/> that should be used when the command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public CliApplicationBuilder WithCommand<TCommand, TExecutor>(TExecutor executorInstance)
            where TExecutor : ICliExecutor<TCommand>
        {
            Commands.Add(CommandFactory.Create<TCommand, TExecutor>(executorInstance));
            AddTypeToServices<TCommand>(default);
            AddTypeToServices(executorInstance);
            return this;
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command and executor type to the final application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <typeparam name="TExecutor">The executor type.</typeparam>
        /// <param name="optionsInstance">An instance of <typeparamref name="TCommand"/> that should be used when the command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public CliApplicationBuilder WithCommand<TCommand, TExecutor>(TCommand optionsInstance)
            where TExecutor : ICliExecutor<TCommand>
        {
            Commands.Add(CommandFactory.Create<TCommand, TExecutor>(optionsInstance));
            AddTypeToServices(optionsInstance);
            AddTypeToServices<TExecutor>(default);
            return this;
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command and executor type to the final application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <typeparam name="TExecutor">The executor type.</typeparam>
        /// <param name="optionsInstance">An instance of <typeparamref name="TCommand"/> that should be used when the command is executed.</param>
        /// <param name="executorInstance">An instance of <typeparamref name="TExecutor"/> that should be used when the command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public CliApplicationBuilder WithCommand<TCommand, TExecutor>(TCommand optionsInstance, TExecutor executorInstance)
            where TExecutor : ICliExecutor<TCommand>
        {
            Commands.Add(CommandFactory.Create(optionsInstance, executorInstance));
            AddTypeToServices(optionsInstance);
            AddTypeToServices(executorInstance);
            return this;
        }

        /// <inheritdoc/>
        protected override ICliApplication OnBuild()
        {
            var result = new Lazy<CliApplication>(() => new CliApplication(Services.BuildServiceProvider(), Options, Commands));

            _ = Services.AddSingleton<ICliApplication>(x => result.Value);
            _ = Services.AddSingleton(x => result.Value);

            return result.Value;
        }
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
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliAsyncApplicationBuilder"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="commands">The commands.</param>
        /// <param name="options">The options.</param>
        protected CliAsyncApplicationBuilder(IServiceCollection services, ICliCommandInfoCollection commands, CliApplicationOptions options)
            : base(services, commands, options)
        {
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> to the final application.
        /// </summary>
        /// <param name="command">The command to register.</param>
        /// <returns>Self reference to this builder.</returns>
        [ExcludeFromCodeCoverage]
        public new CliAsyncApplicationBuilder WithCommand(ICliCommandInfo command)
        {
            return base.WithCommand(command);
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from an executable command type to the final application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/> and implements the <see cref="ICliAsyncExecutable"/> interface.</param>
        /// <returns>Self reference to this builder.</returns>
        [ExcludeFromCodeCoverage]
        public new CliAsyncApplicationBuilder WithCommand(Type commandType)
        {
            return base.WithCommand(commandType);
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from an executable command type to the final application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/> and implements the <see cref="ICliAsyncExecutable"/> interface.</param>
        /// <param name="optionsInstance">An instance of <paramref name="commandType"/> that should be used when the command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        [ExcludeFromCodeCoverage]
        public new CliAsyncApplicationBuilder WithCommand(Type commandType, object? optionsInstance)
        {
            return base.WithCommand(commandType, optionsInstance);
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command and executor type to the final application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="executorType">The executor type that implements the <see cref="ICliAsyncExecutor{TCommand}"/> interface for the <paramref name="commandType"/>.</param>
        /// <returns>Self reference to this builder.</returns>
        [ExcludeFromCodeCoverage]
        public new CliAsyncApplicationBuilder WithCommand(Type commandType, Type? executorType)
        {
            return base.WithCommand(commandType, executorType);
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command and executor type to the final application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="optionsInstance">An instance of <paramref name="commandType"/> that should be used when the command is executed.</param>
        /// <param name="executorType">The executor type that implements the <see cref="ICliAsyncExecutor{TCommand}"/> interface for the <paramref name="commandType"/>.</param>
        /// <returns>Self reference to this builder.</returns>
        [ExcludeFromCodeCoverage]
        public new CliAsyncApplicationBuilder WithCommand(Type commandType, object? optionsInstance, Type? executorType)
        {
            return base.WithCommand(commandType, optionsInstance, executorType);
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command and executor type to the final application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="executorType">The executor type that implements the <see cref="ICliAsyncExecutor{TCommand}"/> interface for the <paramref name="commandType"/>.</param>
        /// <param name="executorInstance">An instance of <paramref name="executorType"/> that should be used when the command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        [ExcludeFromCodeCoverage]
        public new CliAsyncApplicationBuilder WithCommand(Type commandType, Type? executorType, object? executorInstance)
        {
            return base.WithCommand(commandType, executorType, executorInstance);
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command and executor type to the final application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="optionsInstance">An instance of <paramref name="commandType"/> that should be used when the command is executed.</param>
        /// <param name="executorType">The executor type that implements the <see cref="ICliAsyncExecutor{TCommand}"/> interface for the <paramref name="commandType"/>.</param>
        /// <param name="executorInstance">An instance of <paramref name="executorType"/> that should be used when the command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        [ExcludeFromCodeCoverage]
        public new CliAsyncApplicationBuilder WithCommand(Type commandType, object? optionsInstance, Type? executorType, object? executorInstance)
        {
            return base.WithCommand(commandType, optionsInstance, executorType, executorInstance);
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command type and an executor function to the final application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="executorFunction">The executor function that is called when the created command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public CliAsyncApplicationBuilder WithCommand(Type commandType, Func<CliExecutionContext, object, Task<int>> executorFunction)
        {
            Commands.Add(CommandFactory.Create(commandType, executorFunction));
            AddTypeToServices(commandType, null);
            return this;
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command type and an executor function to the final application.
        /// </summary>
        /// <param name="commandType">The command type that has a <see cref="CliCommandAttribute"/>.</param>
        /// <param name="optionsInstance">An instance of <paramref name="commandType"/> that should be used when the command is executed.</param>
        /// <param name="executorFunction">The executor function that is called when the created command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public CliAsyncApplicationBuilder WithCommand(Type commandType, object? optionsInstance, Func<CliExecutionContext, object, Task<int>> executorFunction)
        {
            Commands.Add(CommandFactory.Create(commandType, optionsInstance, executorFunction));
            AddTypeToServices(commandType, optionsInstance);
            return this;
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command type and an executor function to the final application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <param name="executorFunction">The executor function that is called when the created command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public CliAsyncApplicationBuilder WithCommand<TCommand>(Func<CliExecutionContext, TCommand, Task<int>> executorFunction)
        {
            Commands.Add(CommandFactory.Create(executorFunction));
            AddTypeToServices<TCommand>(default);
            return this;
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command type and an executor function to the final application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <param name="optionsInstance">An instance of <typeparamref name="TCommand"/> that should be used when the command is executed.</param>
        /// <param name="executorFunction">The executor function that is called when the created command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public CliAsyncApplicationBuilder WithCommand<TCommand>(TCommand optionsInstance, Func<CliExecutionContext, TCommand, Task<int>> executorFunction)
        {
            Commands.Add(CommandFactory.Create(optionsInstance, executorFunction));
            AddTypeToServices(optionsInstance);
            return this;
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from an executable command type to the final application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <returns>Self reference to this builder.</returns>
        public CliAsyncApplicationBuilder WithCommand<TCommand>()
        {
            Commands.Add(CommandFactory.Create<TCommand>());
            AddTypeToServices<TCommand>(default);
            return this;
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from an executable command type to the final application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <param name="optionsInstance">An instance of <typeparamref name="TCommand"/> that should be used when the command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public CliAsyncApplicationBuilder WithCommand<TCommand>(TCommand optionsInstance)
        {
            Commands.Add(CommandFactory.Create(optionsInstance));
            AddTypeToServices(optionsInstance);
            return this;
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command and executor type to the final application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <typeparam name="TExecutor">The executor type.</typeparam>
        /// <returns>Self reference to this builder.</returns>
        public CliAsyncApplicationBuilder WithCommand<TCommand, TExecutor>()
            where TExecutor : ICliAsyncExecutor<TCommand>
        {
            Commands.Add(CommandFactory.Create<TCommand, TExecutor>());
            AddTypeToServices<TCommand>(default);
            AddTypeToServices<TExecutor>(default);
            return this;
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command and executor type to the final application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <typeparam name="TExecutor">The executor type.</typeparam>
        /// <param name="executorInstance">An instance of <typeparamref name="TExecutor"/> that should be used when the command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public CliAsyncApplicationBuilder WithCommand<TCommand, TExecutor>(TExecutor executorInstance)
            where TExecutor : ICliAsyncExecutor<TCommand>
        {
            Commands.Add(CommandFactory.Create<TCommand, TExecutor>(executorInstance));
            AddTypeToServices<TCommand>(default);
            AddTypeToServices(executorInstance);
            return this;
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command and executor type to the final application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <typeparam name="TExecutor">The executor type.</typeparam>
        /// <param name="optionsInstance">An instance of <typeparamref name="TCommand"/> that should be used when the command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public CliAsyncApplicationBuilder WithCommand<TCommand, TExecutor>(TCommand optionsInstance)
            where TExecutor : ICliAsyncExecutor<TCommand>
        {
            Commands.Add(CommandFactory.Create<TCommand, TExecutor>(optionsInstance));
            AddTypeToServices(optionsInstance);
            AddTypeToServices<TExecutor>(default);
            return this;
        }

        /// <summary>
        /// Adds a <see cref="ICliCommandInfo"/> created from a command and executor type to the final application.
        /// </summary>
        /// <typeparam name="TCommand">The command type that has a <see cref="CliCommandAttribute"/>.</typeparam>
        /// <typeparam name="TExecutor">The executor type.</typeparam>
        /// <param name="optionsInstance">An instance of <typeparamref name="TCommand"/> that should be used when the command is executed.</param>
        /// <param name="executorInstance">An instance of <typeparamref name="TExecutor"/> that should be used when the command is executed.</param>
        /// <returns>Self reference to this builder.</returns>
        public CliAsyncApplicationBuilder WithCommand<TCommand, TExecutor>(TCommand optionsInstance, TExecutor executorInstance)
            where TExecutor : ICliAsyncExecutor<TCommand>
        {
            Commands.Add(CommandFactory.Create(optionsInstance, executorInstance));
            AddTypeToServices(optionsInstance);
            AddTypeToServices(executorInstance);
            return this;
        }

        /// <inheritdoc/>
        protected override ICliAsyncApplication OnBuild()
        {
            var result = new Lazy<CliAsyncApplication>(() => new CliAsyncApplication(Services.BuildServiceProvider(), Options, Commands));

            _ = Services.AddSingleton<ICliAsyncApplication>(x => result.Value);
            _ = Services.AddSingleton(x => result.Value);

            return result.Value;
        }
    }
}
