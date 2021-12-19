using MaSch.Console.Cli.Runtime;
using Microsoft.Extensions.DependencyInjection;

#pragma warning disable IDISP012 // Property should not return created disposable.

namespace MaSch.Console.Cli.UnitTests;

[TestClass]
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Base class tests")]
public class CliApplicationBuilderBaseTests : TestClassBase
{
    private List<ServiceDescriptor> Services => Cache.GetValue(() => new List<ServiceDescriptor> { ServiceDescriptor.Singleton(typeof(ICliCommandFactory), CommandFactoryMock.Object) })!;
    private Mock<IServiceCollection> ServiceCollectionMock => Cache.GetValue(() =>
    {
        var mock = Mocks.Create<IServiceCollection>();
        _ = mock.Setup(x => x.GetEnumerator()).Returns(() => Services.GetEnumerator());
        _ = mock.Setup(x => x.Count).Returns(() => Services.Count);
        _ = mock.Setup(x => x.CopyTo(It.IsAny<ServiceDescriptor[]>(), It.IsAny<int>())).Callback<ServiceDescriptor[], int>((a, b) => Services.CopyTo(a, b));
        return mock;
    })!;
    private Mock<ICliCommandInfoCollection> CommandsMock => Cache.GetValue(() => Mocks.Create<ICliCommandInfoCollection>())!;
    private Mock<CliApplicationOptions> OptionsMock => Cache.GetValue(() =>
    {
        var mock = Mocks.Create<CliApplicationOptions>(MockBehavior.Loose);
        mock.CallBase = true;
        return mock;
    })!;
    private Mock<ICliCommandFactory> CommandFactoryMock => Cache.GetValue(() => Mocks.Create<ICliCommandFactory>())!;
    private Mock<DummyBuilder> BuilderMock => Cache.GetValue(() => Mocks.Create<DummyBuilder>(ServiceCollectionMock.Object, CommandsMock.Object, OptionsMock.Object))!;
    private DummyBuilder Builder => BuilderMock.Object;

    [TestMethod]
    public void Ctor()
    {
        var readOnlyCommandsMock = Mocks.Create<IReadOnlyCliCommandInfoCollection>();
        _ = CommandsMock.Setup(x => x.AsReadOnly()).Returns(readOnlyCommandsMock.Object);

        Assert.AreSame(OptionsMock.Object, Builder.Options);
        Assert.AreSame(readOnlyCommandsMock.Object, Builder.Commands);
        Assert.IsNotNull(Builder.GetServiceProvider());

        Assert.IsGreaterThan(0, ServiceCollectionMock.Invocations.Count);
    }

    [TestMethod]
    public void WithCommand_Command_WithoutInstance()
    {
        var command = CreateCommand(typeof(DummyClass), null);
        _ = BuilderMock.Setup(x => x.WithCommand(command)).CallBase();
        _ = CommandsMock.Setup(x => x.Add(command)).Verifiable(Verifiables, Times.Once());
        AddScopedSetup(typeof(DummyClass), typeof(DummyClass), Times.Once());

        var b = Builder.WithCommand(command);

        Assert.AreSame(Builder, b);
    }

    [TestMethod]
    public void WithCommand_Command_WithInstance()
    {
        var instance = new DummyClass();
        var command = CreateCommand(typeof(DummyClass), instance);
        _ = BuilderMock.Setup(x => x.WithCommand(command)).CallBase();
        _ = CommandsMock.Setup(x => x.Add(command)).Verifiable(Verifiables, Times.Once());
        AddSingletonSetup(typeof(DummyClass), instance, Times.Once());

        var b = Builder.WithCommand(command);

        Assert.AreSame(Builder, b);
    }

    [TestMethod]
    public void WithCommand_CommandType()
    {
        var commandType = Mocks.Create<Type>();
        var commandMock = Mocks.Create<ICliCommandInfo>();
        _ = BuilderMock.Setup(x => x.WithCommand(commandType.Object)).CallBase();
        _ = CommandFactoryMock.Setup(x => x.Create(commandType.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
        _ = CommandsMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());
        AddScopedSetup(commandType.Object, commandType.Object, Times.Once());

        var b = Builder.WithCommand(commandType.Object);

        Assert.AreSame(Builder, b);
    }

    [TestMethod]
    public void WithCommand_CommandType_OptionsInstance()
    {
        var commandType = Mocks.Create<Type>();
        var optionsInstance = Mocks.Create<object>();
        var commandMock = Mocks.Create<ICliCommandInfo>();
        _ = BuilderMock.Setup(x => x.WithCommand(commandType.Object, optionsInstance.Object)).CallBase();
        _ = CommandFactoryMock.Setup(x => x.Create(commandType.Object, optionsInstance.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
        _ = CommandsMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());
        AddSingletonSetup(commandType.Object, optionsInstance.Object, Times.Once());

        var b = Builder.WithCommand(commandType.Object, optionsInstance.Object);

        Assert.AreSame(Builder, b);
    }

    [TestMethod]
    public void WithCommand_CommandType_ExecutorType()
    {
        var commandType = typeof(IDisposable);
        var executorType = typeof(IEnumerable);
        var commandMock = Mocks.Create<ICliCommandInfo>();
        _ = BuilderMock.Setup(x => x.WithCommand(commandType, executorType)).CallBase();
        _ = CommandFactoryMock.Setup(x => x.Create(commandType, executorType)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
        _ = CommandsMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());
        AddScopedSetup(commandType, commandType, Times.Once());
        AddScopedSetup(executorType, executorType, Times.Once());

        var b = Builder.WithCommand(commandType, executorType);

        Assert.AreSame(Builder, b);
    }

    [TestMethod]
    public void WithCommand_CommandType_OptionsInstance_ExecutorType()
    {
        var commandType = Mocks.Create<Type>();
        var optionsInstance = Mocks.Create<object>();
        var executorType = Mocks.Create<Type>();
        var commandMock = Mocks.Create<ICliCommandInfo>();
        _ = BuilderMock.Setup(x => x.WithCommand(commandType.Object, optionsInstance.Object, executorType.Object)).CallBase();
        _ = CommandFactoryMock.Setup(x => x.Create(commandType.Object, optionsInstance.Object, executorType.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
        _ = CommandsMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());
        AddSingletonSetup(commandType.Object, optionsInstance.Object, Times.Once());
        AddScopedSetup(executorType.Object, executorType.Object, Times.Once());

        var b = Builder.WithCommand(commandType.Object, optionsInstance.Object, executorType.Object);

        Assert.AreSame(Builder, b);
    }

    [TestMethod]
    public void WithCommand_CommandType_ExecutorType_ExecutorInstance()
    {
        var commandType = Mocks.Create<Type>();
        var executorType = Mocks.Create<Type>();
        var executorInstance = Mocks.Create<object>();
        var commandMock = Mocks.Create<ICliCommandInfo>();
        _ = BuilderMock.Setup(x => x.WithCommand(commandType.Object, executorType.Object, executorInstance.Object)).CallBase();
        _ = CommandFactoryMock.Setup(x => x.Create(commandType.Object, executorType.Object, executorInstance.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
        _ = CommandsMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());
        AddScopedSetup(commandType.Object, commandType.Object, Times.Once());
        AddSingletonSetup(executorType.Object, executorInstance.Object, Times.Once());

        var b = Builder.WithCommand(commandType.Object, executorType.Object, executorInstance.Object);

        Assert.AreSame(Builder, b);
    }

    [TestMethod]
    public void WithCommand_CommandType_OptionsInstance_ExecutorType_ExecutorInstance()
    {
        var commandType = typeof(IDisposable);
        var optionsInstance = Mocks.Create<object>();
        var executorType = typeof(IEnumerable);
        var executorInstance = Mocks.Create<object>();
        var commandMock = Mocks.Create<ICliCommandInfo>();
        _ = BuilderMock.Setup(x => x.WithCommand(commandType, optionsInstance.Object, executorType, executorInstance.Object)).CallBase();
        _ = CommandFactoryMock.Setup(x => x.Create(commandType, optionsInstance.Object, executorType, executorInstance.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
        _ = CommandsMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());
        AddSingletonSetup(commandType, optionsInstance.Object, Times.Once());
        AddSingletonSetup(executorType, executorInstance.Object, Times.Once());

        var b = Builder.WithCommand(commandType, optionsInstance.Object, executorType, executorInstance.Object);

        Assert.AreSame(Builder, b);
    }

    [TestMethod]
    public void ConfigureOptions()
    {
        var action = Mocks.Create<Action<ICliApplicationOptions>>();
        var options = Builder.Options;
        _ = BuilderMock.Setup(x => x.ConfigureOptions(action.Object)).CallBase();
        _ = action.Setup(x => x(options)).Verifiable(Verifiables, Times.Once());

        var b = Builder.ConfigureOptions(action.Object);

        Assert.AreSame(Builder, b);
    }

    [TestMethod]
    public void Build()
    {
        var app = Mocks.Create<ICliApplicationBase>();
        Services.Clear();
        _ = ServiceCollectionMock.Setup(x => x.Add(It.Is<ServiceDescriptor>(y => y.ServiceType == typeof(IConsoleService)))).Verifiable(Verifiables, Times.Once());
        _ = ServiceCollectionMock.Setup(x => x.Add(It.Is<ServiceDescriptor>(y => y.ServiceType == typeof(ICliArgumentParser)))).Verifiable(Verifiables, Times.Once());
        _ = ServiceCollectionMock.Setup(x => x.Add(It.Is<ServiceDescriptor>(y => y.ServiceType == typeof(ICliHelpPage)))).Verifiable(Verifiables, Times.Once());
        _ = ServiceCollectionMock.Setup(x => x.Add(It.Is<ServiceDescriptor>(y => y.ServiceType == typeof(ICliCommandFactory)))).Verifiable(Verifiables, Times.Once());
        _ = ServiceCollectionMock.Setup(x => x.Add(It.Is<ServiceDescriptor>(y => y.ServiceType == typeof(ICliApplicationBase)))).Verifiable(Verifiables, Times.Once());
        _ = ServiceCollectionMock.Setup(x => x.Add(It.Is<ServiceDescriptor>(y => y.ServiceType == typeof(ICliValidator<object>)))).Verifiable(Verifiables, Times.Once());
        _ = ServiceCollectionMock.Setup(x => x.Add(It.Is<ServiceDescriptor>(y => y.ServiceType == typeof(ICliOptionsProvider)))).Verifiable(Verifiables, Times.Once());
        _ = BuilderMock.Protected().Setup<ICliApplicationBase>("OnBuild").Returns(app.Object);

        var a = Builder.Build();

        Assert.AreSame(app.Object, a);
    }

    private ICliCommandInfo CreateCommand(Type commandType, object? optionsInstance)
    {
        var commandMock = Mocks.Create<ICliCommandInfo>();
        _ = commandMock.Setup(x => x.CommandType).Returns(commandType);
        _ = commandMock.Setup(x => x.OptionsInstance).Returns(optionsInstance);
        return commandMock.Object;
    }

    private void AddSingletonSetup(Type serviceType, object instance, Times times)
    {
        _ = ServiceCollectionMock
            .Setup(x => x.Add(It.Is<ServiceDescriptor>(d =>
                d.Lifetime == ServiceLifetime.Singleton &&
                d.ServiceType == serviceType &&
                d.ImplementationInstance == instance)))
            .Verifiable(Verifiables, times);
    }

    private void AddScopedSetup(Type serviceType, Type implementationType, Times times)
    {
        _ = ServiceCollectionMock
            .Setup(x => x.Add(It.Is<ServiceDescriptor>(d =>
                d.Lifetime == ServiceLifetime.Scoped &&
                d.ServiceType == serviceType &&
                d.ImplementationType == implementationType)))
            .Verifiable(Verifiables, times);
    }

    public class DummyClass
    {
    }

    public abstract class DummyBuilder : CliApplicationBuilderBase<ICliApplicationBase, DummyBuilder>
    {
        protected DummyBuilder(IServiceCollection services, ICliCommandInfoCollection commands, CliApplicationOptions options)
            : base(services, commands, options)
        {
        }

        public new IReadOnlyCliCommandInfoCollection Commands => base.Commands.AsReadOnly();
        public new ICliApplicationOptions Options => base.Options;

        public ServiceProvider GetServiceProvider()
        {
            return Services.BuildServiceProvider();
        }
    }
}

[TestClass]
public class CliApplicationBuilderTests : TestClassBase
{
    private List<ServiceDescriptor> Services => Cache.GetValue(() => new List<ServiceDescriptor> { ServiceDescriptor.Singleton(typeof(ICliCommandFactory), CommandFactoryMock.Object) })!;
    private Mock<IServiceCollection> ServiceCollectionMock => Cache.GetValue(() =>
    {
        var mock = Mocks.Create<IServiceCollection>();
        _ = mock.Setup(x => x.GetEnumerator()).Returns(() => Services.GetEnumerator());
        _ = mock.Setup(x => x.Count).Returns(() => Services.Count);
        _ = mock.Setup(x => x.CopyTo(It.IsAny<ServiceDescriptor[]>(), It.IsAny<int>())).Callback<ServiceDescriptor[], int>((a, b) => Services.CopyTo(a, b));
        return mock;
    })!;
    private Mock<ICliCommandInfoCollection> CommandsMock => Cache.GetValue(() => Mocks.Create<ICliCommandInfoCollection>())!;
    private Mock<CliApplicationOptions> OptionsMock => Cache.GetValue(() =>
    {
        var mock = Mocks.Create<CliApplicationOptions>(MockBehavior.Loose);
        mock.CallBase = true;
        return mock;
    })!;
    private Mock<ICliCommandFactory> CommandFactoryMock => Cache.GetValue(() => Mocks.Create<ICliCommandFactory>())!;
    private Mock<CliApplicationBuilder> BuilderMock => Cache.GetValue(() => Mocks.Create<CliApplicationBuilder>(ServiceCollectionMock.Object, CommandsMock.Object, OptionsMock.Object))!;
    private CliApplicationBuilder Builder => BuilderMock.Object;

    [TestMethod]
    public void WithCommand_CommandType_ExecutorFunction()
    {
        var executorFunction = Mocks.Create<Func<CliExecutionContext, object, int>>();
        var commandMock = Mocks.Create<ICliCommandInfo>();
        _ = CommandFactoryMock.Setup(x => x.Create(typeof(object), executorFunction.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
        _ = CommandsMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());
        AddScopedSetup(typeof(object), typeof(object), Times.Once());

        var b = Builder.WithCommand(typeof(object), executorFunction.Object);

        Assert.AreSame(Builder, b);
    }

    [TestMethod]
    public void WithCommand_CommandType_OptionsInstance_ExecutorFunction()
    {
        var optionsInstance = Mocks.Create<IDisposable>();
        var executorFunction = Mocks.Create<Func<CliExecutionContext, object, int>>();
        var commandMock = Mocks.Create<ICliCommandInfo>();
        _ = CommandFactoryMock.Setup(x => x.Create(typeof(object), optionsInstance.Object, executorFunction.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
        _ = CommandsMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());
        AddSingletonSetup(typeof(object), optionsInstance.Object, Times.Once());

        var b = Builder.WithCommand(typeof(object), optionsInstance.Object, executorFunction.Object);

        Assert.AreSame(Builder, b);
    }

    [TestMethod]
    public void WithCommand_TCommand_ExecutorFunction()
    {
        var executorFunction = Mocks.Create<Func<CliExecutionContext, IDisposable, int>>();
        var commandMock = Mocks.Create<ICliCommandInfo>();
        _ = CommandFactoryMock.Setup(x => x.Create(executorFunction.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
        _ = CommandsMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());
        AddScopedSetup(typeof(IDisposable), typeof(IDisposable), Times.Once());

        var b = Builder.WithCommand(executorFunction.Object);

        Assert.AreSame(Builder, b);
    }

    [TestMethod]
    public void WithCommand_TCommand_OptionsInstance_ExecutorFunction()
    {
        var optionsInstance = Mocks.Create<IDisposable>();
        var executorFunction = Mocks.Create<Func<CliExecutionContext, IDisposable, int>>();
        var commandMock = Mocks.Create<ICliCommandInfo>();
        _ = CommandFactoryMock.Setup(x => x.Create(optionsInstance.Object, executorFunction.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
        _ = CommandsMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());
        AddSingletonSetup(typeof(IDisposable), optionsInstance.Object, Times.Once());

        var b = Builder.WithCommand(optionsInstance.Object, executorFunction.Object);

        Assert.AreSame(Builder, b);
    }

    [TestMethod]
    public void WithCommand_TCommand()
    {
        var commandMock = Mocks.Create<ICliCommandInfo>();
        _ = CommandFactoryMock.Setup(x => x.Create<DummyClass>()).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
        _ = CommandsMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());
        AddScopedSetup(typeof(DummyClass), typeof(DummyClass), Times.Once());

        var b = Builder.WithCommand<DummyClass>();

        Assert.AreSame(Builder, b);
    }

    [TestMethod]
    public void WithCommand_TCommand_OptionsInstance()
    {
        var optionsInstance = Mocks.Create<DummyClass>();
        var commandMock = Mocks.Create<ICliCommandInfo>();
        _ = CommandFactoryMock.Setup(x => x.Create(optionsInstance.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
        _ = CommandsMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());
        AddSingletonSetup(typeof(DummyClass), optionsInstance.Object, Times.Once());

        var b = Builder.WithCommand(optionsInstance.Object);

        Assert.AreSame(Builder, b);
    }

    [TestMethod]
    public void WithCommand_TCommand_TExecutor()
    {
        var commandMock = Mocks.Create<ICliCommandInfo>();
        _ = CommandFactoryMock.Setup(x => x.Create<IDisposable, DummyClass>()).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
        _ = CommandsMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());
        AddScopedSetup(typeof(IDisposable), typeof(IDisposable), Times.Once());
        AddScopedSetup(typeof(DummyClass), typeof(DummyClass), Times.Once());

        var b = Builder.WithCommand<IDisposable, DummyClass>();

        Assert.AreSame(Builder, b);
    }

    [TestMethod]
    public void WithCommand_TCommand_TExecutor_ExecutorInstance()
    {
        var executorInstance = Mocks.Create<DummyClass>();
        var commandMock = Mocks.Create<ICliCommandInfo>();
        _ = CommandFactoryMock.Setup(x => x.Create<IDisposable, DummyClass>(executorInstance.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
        _ = CommandsMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());
        AddScopedSetup(typeof(IDisposable), typeof(IDisposable), Times.Once());
        AddSingletonSetup(typeof(DummyClass), executorInstance.Object, Times.Once());

        var b = Builder.WithCommand<IDisposable, DummyClass>(executorInstance.Object);

        Assert.AreSame(Builder, b);
    }

    [TestMethod]
    public void WithCommand_TCommand_TExecutor_OptionsInstance()
    {
        var optionsInstance = Mocks.Create<IDisposable>();
        var commandMock = Mocks.Create<ICliCommandInfo>();
        _ = CommandFactoryMock.Setup(x => x.Create<IDisposable, DummyClass>(optionsInstance.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
        _ = CommandsMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());
        AddSingletonSetup(typeof(IDisposable), optionsInstance.Object, Times.Once());
        AddScopedSetup(typeof(DummyClass), typeof(DummyClass), Times.Once());

        var b = Builder.WithCommand<IDisposable, DummyClass>(optionsInstance.Object);

        Assert.AreSame(Builder, b);
    }

    [TestMethod]
    public void WithCommand_TCommand_TExecutor_OptionsInstance_ExecutorInstance()
    {
        var optionsInstance = Mocks.Create<IDisposable>();
        var executorInstance = Mocks.Create<DummyClass>();
        var commandMock = Mocks.Create<ICliCommandInfo>();
        _ = CommandFactoryMock.Setup(x => x.Create(optionsInstance.Object, executorInstance.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
        _ = CommandsMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());
        AddSingletonSetup(typeof(IDisposable), optionsInstance.Object, Times.Once());
        AddSingletonSetup(typeof(DummyClass), executorInstance.Object, Times.Once());

        var b = Builder.WithCommand(optionsInstance.Object, executorInstance.Object);

        Assert.AreSame(Builder, b);
    }

    private void AddSingletonSetup(Type serviceType, object instance, Times times)
    {
        _ = ServiceCollectionMock
            .Setup(x => x.Add(It.Is<ServiceDescriptor>(d =>
                d.Lifetime == ServiceLifetime.Singleton &&
                d.ServiceType == serviceType &&
                d.ImplementationInstance == instance)))
            .Verifiable(Verifiables, times);
    }

    private void AddScopedSetup(Type serviceType, Type implementationType, Times times)
    {
        _ = ServiceCollectionMock
            .Setup(x => x.Add(It.Is<ServiceDescriptor>(d =>
                d.Lifetime == ServiceLifetime.Scoped &&
                d.ServiceType == serviceType &&
                d.ImplementationType == implementationType)))
            .Verifiable(Verifiables, times);
    }

    public abstract class DummyClass : ICliExecutable, ICliExecutor<IDisposable>
    {
        public abstract int ExecuteCommand(CliExecutionContext context);
        public abstract int ExecuteCommand(CliExecutionContext context, IDisposable parameters);
    }
}

[TestClass]
public class CliAsyncApplicationBuilderTests : TestClassBase
{
    private List<ServiceDescriptor> Services => Cache.GetValue(() => new List<ServiceDescriptor> { ServiceDescriptor.Singleton(typeof(ICliCommandFactory), CommandFactoryMock.Object) })!;
    private Mock<IServiceCollection> ServiceCollectionMock => Cache.GetValue(() =>
    {
        var mock = Mocks.Create<IServiceCollection>();
        _ = mock.Setup(x => x.GetEnumerator()).Returns(() => Services.GetEnumerator());
        _ = mock.Setup(x => x.Count).Returns(() => Services.Count);
        _ = mock.Setup(x => x.CopyTo(It.IsAny<ServiceDescriptor[]>(), It.IsAny<int>())).Callback<ServiceDescriptor[], int>((a, b) => Services.CopyTo(a, b));
        return mock;
    })!;
    private Mock<ICliCommandInfoCollection> CommandsMock => Cache.GetValue(() => Mocks.Create<ICliCommandInfoCollection>())!;
    private Mock<CliApplicationOptions> OptionsMock => Cache.GetValue(() =>
    {
        var mock = Mocks.Create<CliApplicationOptions>(MockBehavior.Loose);
        mock.CallBase = true;
        return mock;
    })!;
    private Mock<ICliCommandFactory> CommandFactoryMock => Cache.GetValue(() => Mocks.Create<ICliCommandFactory>())!;
    private Mock<CliAsyncApplicationBuilder> BuilderMock => Cache.GetValue(() => Mocks.Create<CliAsyncApplicationBuilder>(ServiceCollectionMock.Object, CommandsMock.Object, OptionsMock.Object))!;
    private CliAsyncApplicationBuilder Builder => BuilderMock.Object;

    [TestMethod]
    public void WithCommand_CommandType_ExecutorFunction()
    {
        var executorFunction = Mocks.Create<Func<CliExecutionContext, object, Task<int>>>();
        var commandMock = Mocks.Create<ICliCommandInfo>();
        _ = CommandFactoryMock.Setup(x => x.Create(typeof(object), executorFunction.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
        _ = CommandsMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());
        AddScopedSetup(typeof(object), typeof(object), Times.Once());

        var b = Builder.WithCommand(typeof(object), executorFunction.Object);

        Assert.AreSame(Builder, b);
    }

    [TestMethod]
    public void WithCommand_CommandType_OptionsInstance_ExecutorFunction()
    {
        var optionsInstance = Mocks.Create<IDisposable>();
        var executorFunction = Mocks.Create<Func<CliExecutionContext, object, Task<int>>>();
        var commandMock = Mocks.Create<ICliCommandInfo>();
        _ = CommandFactoryMock.Setup(x => x.Create(typeof(object), optionsInstance.Object, executorFunction.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
        _ = CommandsMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());
        AddSingletonSetup(typeof(object), optionsInstance.Object, Times.Once());

        var b = Builder.WithCommand(typeof(object), optionsInstance.Object, executorFunction.Object);

        Assert.AreSame(Builder, b);
    }

    [TestMethod]
    public void WithCommand_TCommand_ExecutorFunction()
    {
        var executorFunction = Mocks.Create<Func<CliExecutionContext, IDisposable, Task<int>>>();
        var commandMock = Mocks.Create<ICliCommandInfo>();
        _ = CommandFactoryMock.Setup(x => x.Create(executorFunction.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
        _ = CommandsMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());
        AddScopedSetup(typeof(IDisposable), typeof(IDisposable), Times.Once());

        var b = Builder.WithCommand(executorFunction.Object);

        Assert.AreSame(Builder, b);
    }

    [TestMethod]
    public void WithCommand_TCommand_OptionsInstance_ExecutorFunction()
    {
        var optionsInstance = Mocks.Create<IDisposable>();
        var executorFunction = Mocks.Create<Func<CliExecutionContext, IDisposable, Task<int>>>();
        var commandMock = Mocks.Create<ICliCommandInfo>();
        _ = CommandFactoryMock.Setup(x => x.Create(optionsInstance.Object, executorFunction.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
        _ = CommandsMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());
        AddSingletonSetup(typeof(IDisposable), optionsInstance.Object, Times.Once());

        var b = Builder.WithCommand(optionsInstance.Object, executorFunction.Object);

        Assert.AreSame(Builder, b);
    }

    [TestMethod]
    public void WithCommand_TCommand()
    {
        var commandMock = Mocks.Create<ICliCommandInfo>();
        _ = CommandFactoryMock.Setup(x => x.Create<DummyClass>()).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
        _ = CommandsMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());
        AddScopedSetup(typeof(DummyClass), typeof(DummyClass), Times.Once());

        var b = Builder.WithCommand<DummyClass>();

        Assert.AreSame(Builder, b);
    }

    [TestMethod]
    public void WithCommand_TCommand_OptionsInstance()
    {
        var optionsInstance = Mocks.Create<DummyClass>();
        var commandMock = Mocks.Create<ICliCommandInfo>();
        _ = CommandFactoryMock.Setup(x => x.Create(optionsInstance.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
        _ = CommandsMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());
        AddSingletonSetup(typeof(DummyClass), optionsInstance.Object, Times.Once());

        var b = Builder.WithCommand(optionsInstance.Object);

        Assert.AreSame(Builder, b);
    }

    [TestMethod]
    public void WithCommand_TCommand_TExecutor()
    {
        var commandMock = Mocks.Create<ICliCommandInfo>();
        _ = CommandFactoryMock.Setup(x => x.Create<IDisposable, DummyClass>()).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
        _ = CommandsMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());
        AddScopedSetup(typeof(IDisposable), typeof(IDisposable), Times.Once());
        AddScopedSetup(typeof(DummyClass), typeof(DummyClass), Times.Once());

        var b = Builder.WithCommand<IDisposable, DummyClass>();

        Assert.AreSame(Builder, b);
    }

    [TestMethod]
    public void WithCommand_TCommand_TExecutor_ExecutorInstance()
    {
        var executorInstance = Mocks.Create<DummyClass>();
        var commandMock = Mocks.Create<ICliCommandInfo>();
        _ = CommandFactoryMock.Setup(x => x.Create<IDisposable, DummyClass>(executorInstance.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
        _ = CommandsMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());
        AddScopedSetup(typeof(IDisposable), typeof(IDisposable), Times.Once());
        AddSingletonSetup(typeof(DummyClass), executorInstance.Object, Times.Once());

        var b = Builder.WithCommand<IDisposable, DummyClass>(executorInstance.Object);

        Assert.AreSame(Builder, b);
    }

    [TestMethod]
    public void WithCommand_TCommand_TExecutor_OptionsInstance()
    {
        var optionsInstance = Mocks.Create<IDisposable>();
        var commandMock = Mocks.Create<ICliCommandInfo>();
        _ = CommandFactoryMock.Setup(x => x.Create<IDisposable, DummyClass>(optionsInstance.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
        _ = CommandsMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());
        AddSingletonSetup(typeof(IDisposable), optionsInstance.Object, Times.Once());
        AddScopedSetup(typeof(DummyClass), typeof(DummyClass), Times.Once());

        var b = Builder.WithCommand<IDisposable, DummyClass>(optionsInstance.Object);

        Assert.AreSame(Builder, b);
    }

    [TestMethod]
    public void WithCommand_TCommand_TExecutor_OptionsInstance_ExecutorInstance()
    {
        var optionsInstance = Mocks.Create<IDisposable>();
        var executorInstance = Mocks.Create<DummyClass>();
        var commandMock = Mocks.Create<ICliCommandInfo>();
        _ = CommandFactoryMock.Setup(x => x.Create(optionsInstance.Object, executorInstance.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
        _ = CommandsMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());
        AddSingletonSetup(typeof(IDisposable), optionsInstance.Object, Times.Once());
        AddSingletonSetup(typeof(DummyClass), executorInstance.Object, Times.Once());

        var b = Builder.WithCommand(optionsInstance.Object, executorInstance.Object);

        Assert.AreSame(Builder, b);
    }

    private void AddSingletonSetup(Type serviceType, object instance, Times times)
    {
        _ = ServiceCollectionMock
            .Setup(x => x.Add(It.Is<ServiceDescriptor>(d =>
                d.Lifetime == ServiceLifetime.Singleton &&
                d.ServiceType == serviceType &&
                d.ImplementationInstance == instance)))
            .Verifiable(Verifiables, times);
    }

    private void AddScopedSetup(Type serviceType, Type implementationType, Times times)
    {
        _ = ServiceCollectionMock
            .Setup(x => x.Add(It.Is<ServiceDescriptor>(d =>
                d.Lifetime == ServiceLifetime.Scoped &&
                d.ServiceType == serviceType &&
                d.ImplementationType == implementationType)))
            .Verifiable(Verifiables, times);
    }

    public abstract class DummyClass : ICliAsyncExecutable, ICliAsyncExecutor<IDisposable>
    {
        public abstract Task<int> ExecuteCommandAsync(CliExecutionContext context, IDisposable parameters);
        public abstract Task<int> ExecuteCommandAsync(CliExecutionContext context);
    }
}
