using MaSch.Console.Cli.Runtime;
using MaSch.Test;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

#pragma warning disable IDISP012 // Property should not return created disposable.

namespace MaSch.Console.Cli.Test
{
    [TestClass]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Base class tests")]
    public class CliApplicationBuilderBaseTests : TestClassBase
    {
        private List<ServiceDescriptor> Services => Cache.GetValue(() => new List<ServiceDescriptor> { ServiceDescriptor.Singleton(typeof(ICliCommandFactory), CommandFactoryMock.Object) })!;
        private Mock<IServiceCollection> ServiceCollectionMock => Cache.GetValue(() =>
        {
            var mock = Mocks.Create<IServiceCollection>();
            mock.Setup(x => x.GetEnumerator()).Returns(() => Services.GetEnumerator());
            mock.Setup(x => x.Count).Returns(() => Services.Count);
            mock.Setup(x => x.CopyTo(It.IsAny<ServiceDescriptor[]>(), It.IsAny<int>())).Callback<ServiceDescriptor[], int>((a, b) => Services.CopyTo(a, b));
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
            var rcMock = Mocks.Create<IReadOnlyCliCommandInfoCollection>();
            CommandsMock.Setup(x => x.AsReadOnly()).Returns(rcMock.Object);

            Assert.AreSame(OptionsMock.Object, Builder.Options);
            Assert.AreSame(rcMock.Object, Builder.Commands);
            Assert.IsNotNull(Builder.GetServiceProvider());

            Assert.IsGreaterThan(0, ServiceCollectionMock.Invocations.Count);
        }

        [TestMethod]
        public void WithCommand_Command_WithoutInstance()
        {
            var command = CreateCommand(typeof(DummyClass), null);
            BuilderMock.Setup(x => x.WithCommand(command)).CallBase();
            CommandsMock.Setup(x => x.Add(command)).Verifiable(Verifiables, Times.Once());
            AddScopedSetup(typeof(DummyClass), typeof(DummyClass), Times.Once());

            var b = Builder.WithCommand(command);

            Assert.AreSame(Builder, b);
        }

        [TestMethod]
        public void WithCommand_Command_WithInstance()
        {
            var instance = new DummyClass();
            var command = CreateCommand(typeof(DummyClass), instance);
            BuilderMock.Setup(x => x.WithCommand(command)).CallBase();
            CommandsMock.Setup(x => x.Add(command)).Verifiable(Verifiables, Times.Once());
            AddSingletonSetup(typeof(DummyClass), instance, Times.Once());

            var b = Builder.WithCommand(command);

            Assert.AreSame(Builder, b);
        }

        [TestMethod]
        public void WithCommand_CommandType()
        {
            var commandType = Mocks.Create<Type>();
            var cMock = Mocks.Create<ICliCommandInfo>();
            BuilderMock.Setup(x => x.WithCommand(commandType.Object)).CallBase();
            CommandFactoryMock.Setup(x => x.Create(commandType.Object)).Returns(cMock.Object).Verifiable(Verifiables, Times.Once());
            CommandsMock.Setup(x => x.Add(cMock.Object)).Verifiable(Verifiables, Times.Once());
            AddScopedSetup(commandType.Object, commandType.Object, Times.Once());

            var b = Builder.WithCommand(commandType.Object);

            Assert.AreSame(Builder, b);
        }

        [TestMethod]
        public void WithCommand_CommandType_OptionsInstance()
        {
            var commandType = Mocks.Create<Type>();
            var optionsInstance = Mocks.Create<object>();
            var cMock = Mocks.Create<ICliCommandInfo>();
            BuilderMock.Setup(x => x.WithCommand(commandType.Object, optionsInstance.Object)).CallBase();
            CommandFactoryMock.Setup(x => x.Create(commandType.Object, optionsInstance.Object)).Returns(cMock.Object).Verifiable(Verifiables, Times.Once());
            CommandsMock.Setup(x => x.Add(cMock.Object)).Verifiable(Verifiables, Times.Once());
            AddSingletonSetup(commandType.Object, optionsInstance.Object, Times.Once());

            var b = Builder.WithCommand(commandType.Object, optionsInstance.Object);

            Assert.AreSame(Builder, b);
        }

        [TestMethod]
        public void WithCommand_CommandType_ExecutorType()
        {
            var commandType = typeof(IDisposable);
            var executorType = typeof(IEnumerable);
            var cMock = Mocks.Create<ICliCommandInfo>();
            BuilderMock.Setup(x => x.WithCommand(commandType, executorType)).CallBase();
            CommandFactoryMock.Setup(x => x.Create(commandType, executorType)).Returns(cMock.Object).Verifiable(Verifiables, Times.Once());
            CommandsMock.Setup(x => x.Add(cMock.Object)).Verifiable(Verifiables, Times.Once());
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
            var cMock = Mocks.Create<ICliCommandInfo>();
            BuilderMock.Setup(x => x.WithCommand(commandType.Object, optionsInstance.Object, executorType.Object)).CallBase();
            CommandFactoryMock.Setup(x => x.Create(commandType.Object, optionsInstance.Object, executorType.Object)).Returns(cMock.Object).Verifiable(Verifiables, Times.Once());
            CommandsMock.Setup(x => x.Add(cMock.Object)).Verifiable(Verifiables, Times.Once());
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
            var cMock = Mocks.Create<ICliCommandInfo>();
            BuilderMock.Setup(x => x.WithCommand(commandType.Object, executorType.Object, executorInstance.Object)).CallBase();
            CommandFactoryMock.Setup(x => x.Create(commandType.Object, executorType.Object, executorInstance.Object)).Returns(cMock.Object).Verifiable(Verifiables, Times.Once());
            CommandsMock.Setup(x => x.Add(cMock.Object)).Verifiable(Verifiables, Times.Once());
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
            var cMock = Mocks.Create<ICliCommandInfo>();
            BuilderMock.Setup(x => x.WithCommand(commandType, optionsInstance.Object, executorType, executorInstance.Object)).CallBase();
            CommandFactoryMock.Setup(x => x.Create(commandType, optionsInstance.Object, executorType, executorInstance.Object)).Returns(cMock.Object).Verifiable(Verifiables, Times.Once());
            CommandsMock.Setup(x => x.Add(cMock.Object)).Verifiable(Verifiables, Times.Once());
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
            BuilderMock.Setup(x => x.ConfigureOptions(action.Object)).CallBase();
            action.Setup(x => x(options)).Verifiable(Verifiables, Times.Once());

            var b = Builder.ConfigureOptions(action.Object);

            Assert.AreSame(Builder, b);
        }

        [TestMethod]
        public void Build()
        {
            var app = Mocks.Create<ICliApplicationBase>();
            Services.Clear();
            ServiceCollectionMock.Setup(x => x.Add(It.Is<ServiceDescriptor>(y => y.ServiceType == typeof(IConsoleService)))).Verifiable(Verifiables, Times.Once());
            ServiceCollectionMock.Setup(x => x.Add(It.Is<ServiceDescriptor>(y => y.ServiceType == typeof(ICliArgumentParser)))).Verifiable(Verifiables, Times.Once());
            ServiceCollectionMock.Setup(x => x.Add(It.Is<ServiceDescriptor>(y => y.ServiceType == typeof(ICliHelpPage)))).Verifiable(Verifiables, Times.Once());
            ServiceCollectionMock.Setup(x => x.Add(It.Is<ServiceDescriptor>(y => y.ServiceType == typeof(ICliCommandFactory)))).Verifiable(Verifiables, Times.Once());
            ServiceCollectionMock.Setup(x => x.Add(It.Is<ServiceDescriptor>(y => y.ServiceType == typeof(ICliApplicationBase)))).Verifiable(Verifiables, Times.Once());
            ServiceCollectionMock.Setup(x => x.Add(It.Is<ServiceDescriptor>(y => y.ServiceType == typeof(ICliValidator<object>)))).Verifiable(Verifiables, Times.Once());
            BuilderMock.Protected().Setup<ICliApplicationBase>("OnBuild").Returns(app.Object);

            var a = Builder.Build();

            Assert.AreSame(app.Object, a);
        }

        private ICliCommandInfo CreateCommand(Type commandType, object? optionsInstance)
        {
            var cMock = Mocks.Create<ICliCommandInfo>();
            cMock.Setup(x => x.CommandType).Returns(commandType);
            cMock.Setup(x => x.OptionsInstance).Returns(optionsInstance);
            return cMock.Object;
        }

        private void AddSingletonSetup(Type serviceType, object instance, Times times)
        {
            ServiceCollectionMock
                .Setup(x => x.Add(It.Is<ServiceDescriptor>(d =>
                    d.Lifetime == ServiceLifetime.Singleton &&
                    d.ServiceType == serviceType &&
                    d.ImplementationInstance == instance)))
                .Verifiable(Verifiables, times);
        }

        private void AddScopedSetup(Type serviceType, Type implementationType, Times times)
        {
            ServiceCollectionMock
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

            public ServiceProvider GetServiceProvider() => Services.BuildServiceProvider();
        }
    }

    [TestClass]
    public class CliApplicationBuilderTests : TestClassBase
    {
        private List<ServiceDescriptor> Services => Cache.GetValue(() => new List<ServiceDescriptor> { ServiceDescriptor.Singleton(typeof(ICliCommandFactory), CommandFactoryMock.Object) })!;
        private Mock<IServiceCollection> ServiceCollectionMock => Cache.GetValue(() =>
        {
            var mock = Mocks.Create<IServiceCollection>();
            mock.Setup(x => x.GetEnumerator()).Returns(() => Services.GetEnumerator());
            mock.Setup(x => x.Count).Returns(() => Services.Count);
            mock.Setup(x => x.CopyTo(It.IsAny<ServiceDescriptor[]>(), It.IsAny<int>())).Callback<ServiceDescriptor[], int>((a, b) => Services.CopyTo(a, b));
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
            var cMock = Mocks.Create<ICliCommandInfo>();
            CommandFactoryMock.Setup(x => x.Create(typeof(object), executorFunction.Object)).Returns(cMock.Object).Verifiable(Verifiables, Times.Once());
            CommandsMock.Setup(x => x.Add(cMock.Object)).Verifiable(Verifiables, Times.Once());
            AddScopedSetup(typeof(object), typeof(object), Times.Once());

            var b = Builder.WithCommand(typeof(object), executorFunction.Object);

            Assert.AreSame(Builder, b);
        }

        [TestMethod]
        public void WithCommand_CommandType_OptionsInstance_ExecutorFunction()
        {
            var optionsInstance = Mocks.Create<IDisposable>();
            var executorFunction = Mocks.Create<Func<CliExecutionContext, object, int>>();
            var cMock = Mocks.Create<ICliCommandInfo>();
            CommandFactoryMock.Setup(x => x.Create(typeof(object), optionsInstance.Object, executorFunction.Object)).Returns(cMock.Object).Verifiable(Verifiables, Times.Once());
            CommandsMock.Setup(x => x.Add(cMock.Object)).Verifiable(Verifiables, Times.Once());
            AddSingletonSetup(typeof(object), optionsInstance.Object, Times.Once());

            var b = Builder.WithCommand(typeof(object), optionsInstance.Object, executorFunction.Object);

            Assert.AreSame(Builder, b);
        }

        [TestMethod]
        public void WithCommand_TCommand_ExecutorFunction()
        {
            var executorFunction = Mocks.Create<Func<CliExecutionContext, IDisposable, int>>();
            var cMock = Mocks.Create<ICliCommandInfo>();
            CommandFactoryMock.Setup(x => x.Create(executorFunction.Object)).Returns(cMock.Object).Verifiable(Verifiables, Times.Once());
            CommandsMock.Setup(x => x.Add(cMock.Object)).Verifiable(Verifiables, Times.Once());
            AddScopedSetup(typeof(IDisposable), typeof(IDisposable), Times.Once());

            var b = Builder.WithCommand(executorFunction.Object);

            Assert.AreSame(Builder, b);
        }

        [TestMethod]
        public void WithCommand_TCommand_OptionsInstance_ExecutorFunction()
        {
            var optionsInstance = Mocks.Create<IDisposable>();
            var executorFunction = Mocks.Create<Func<CliExecutionContext, IDisposable, int>>();
            var cMock = Mocks.Create<ICliCommandInfo>();
            CommandFactoryMock.Setup(x => x.Create(optionsInstance.Object, executorFunction.Object)).Returns(cMock.Object).Verifiable(Verifiables, Times.Once());
            CommandsMock.Setup(x => x.Add(cMock.Object)).Verifiable(Verifiables, Times.Once());
            AddSingletonSetup(typeof(IDisposable), optionsInstance.Object, Times.Once());

            var b = Builder.WithCommand(optionsInstance.Object, executorFunction.Object);

            Assert.AreSame(Builder, b);
        }

        [TestMethod]
        public void WithCommand_TCommand()
        {
            var cMock = Mocks.Create<ICliCommandInfo>();
            CommandFactoryMock.Setup(x => x.Create<DummyClass>()).Returns(cMock.Object).Verifiable(Verifiables, Times.Once());
            CommandsMock.Setup(x => x.Add(cMock.Object)).Verifiable(Verifiables, Times.Once());
            AddScopedSetup(typeof(DummyClass), typeof(DummyClass), Times.Once());

            var b = Builder.WithCommand<DummyClass>();

            Assert.AreSame(Builder, b);
        }

        [TestMethod]
        public void WithCommand_TCommand_OptionsInstance()
        {
            var optionsInstance = Mocks.Create<DummyClass>();
            var cMock = Mocks.Create<ICliCommandInfo>();
            CommandFactoryMock.Setup(x => x.Create(optionsInstance.Object)).Returns(cMock.Object).Verifiable(Verifiables, Times.Once());
            CommandsMock.Setup(x => x.Add(cMock.Object)).Verifiable(Verifiables, Times.Once());
            AddSingletonSetup(typeof(DummyClass), optionsInstance.Object, Times.Once());

            var b = Builder.WithCommand(optionsInstance.Object);

            Assert.AreSame(Builder, b);
        }

        [TestMethod]
        public void WithCommand_TCommand_TExecutor()
        {
            var cMock = Mocks.Create<ICliCommandInfo>();
            CommandFactoryMock.Setup(x => x.Create<IDisposable, DummyClass>()).Returns(cMock.Object).Verifiable(Verifiables, Times.Once());
            CommandsMock.Setup(x => x.Add(cMock.Object)).Verifiable(Verifiables, Times.Once());
            AddScopedSetup(typeof(IDisposable), typeof(IDisposable), Times.Once());
            AddScopedSetup(typeof(DummyClass), typeof(DummyClass), Times.Once());

            var b = Builder.WithCommand<IDisposable, DummyClass>();

            Assert.AreSame(Builder, b);
        }

        [TestMethod]
        public void WithCommand_TCommand_TExecutor_ExecutorInstance()
        {
            var executorInstance = Mocks.Create<DummyClass>();
            var cMock = Mocks.Create<ICliCommandInfo>();
            CommandFactoryMock.Setup(x => x.Create<IDisposable, DummyClass>(executorInstance.Object)).Returns(cMock.Object).Verifiable(Verifiables, Times.Once());
            CommandsMock.Setup(x => x.Add(cMock.Object)).Verifiable(Verifiables, Times.Once());
            AddScopedSetup(typeof(IDisposable), typeof(IDisposable), Times.Once());
            AddSingletonSetup(typeof(DummyClass), executorInstance.Object, Times.Once());

            var b = Builder.WithCommand<IDisposable, DummyClass>(executorInstance.Object);

            Assert.AreSame(Builder, b);
        }

        [TestMethod]
        public void WithCommand_TCommand_TExecutor_OptionsInstance()
        {
            var optionsInstance = Mocks.Create<IDisposable>();
            var cMock = Mocks.Create<ICliCommandInfo>();
            CommandFactoryMock.Setup(x => x.Create<IDisposable, DummyClass>(optionsInstance.Object)).Returns(cMock.Object).Verifiable(Verifiables, Times.Once());
            CommandsMock.Setup(x => x.Add(cMock.Object)).Verifiable(Verifiables, Times.Once());
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
            var cMock = Mocks.Create<ICliCommandInfo>();
            CommandFactoryMock.Setup(x => x.Create(optionsInstance.Object, executorInstance.Object)).Returns(cMock.Object).Verifiable(Verifiables, Times.Once());
            CommandsMock.Setup(x => x.Add(cMock.Object)).Verifiable(Verifiables, Times.Once());
            AddSingletonSetup(typeof(IDisposable), optionsInstance.Object, Times.Once());
            AddSingletonSetup(typeof(DummyClass), executorInstance.Object, Times.Once());

            var b = Builder.WithCommand(optionsInstance.Object, executorInstance.Object);

            Assert.AreSame(Builder, b);
        }

        private void AddSingletonSetup(Type serviceType, object instance, Times times)
        {
            ServiceCollectionMock
                .Setup(x => x.Add(It.Is<ServiceDescriptor>(d =>
                    d.Lifetime == ServiceLifetime.Singleton &&
                    d.ServiceType == serviceType &&
                    d.ImplementationInstance == instance)))
                .Verifiable(Verifiables, times);
        }

        private void AddScopedSetup(Type serviceType, Type implementationType, Times times)
        {
            ServiceCollectionMock
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
            mock.Setup(x => x.GetEnumerator()).Returns(() => Services.GetEnumerator());
            mock.Setup(x => x.Count).Returns(() => Services.Count);
            mock.Setup(x => x.CopyTo(It.IsAny<ServiceDescriptor[]>(), It.IsAny<int>())).Callback<ServiceDescriptor[], int>((a, b) => Services.CopyTo(a, b));
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
            var cMock = Mocks.Create<ICliCommandInfo>();
            CommandFactoryMock.Setup(x => x.Create(typeof(object), executorFunction.Object)).Returns(cMock.Object).Verifiable(Verifiables, Times.Once());
            CommandsMock.Setup(x => x.Add(cMock.Object)).Verifiable(Verifiables, Times.Once());
            AddScopedSetup(typeof(object), typeof(object), Times.Once());

            var b = Builder.WithCommand(typeof(object), executorFunction.Object);

            Assert.AreSame(Builder, b);
        }

        [TestMethod]
        public void WithCommand_CommandType_OptionsInstance_ExecutorFunction()
        {
            var optionsInstance = Mocks.Create<IDisposable>();
            var executorFunction = Mocks.Create<Func<CliExecutionContext, object, Task<int>>>();
            var cMock = Mocks.Create<ICliCommandInfo>();
            CommandFactoryMock.Setup(x => x.Create(typeof(object), optionsInstance.Object, executorFunction.Object)).Returns(cMock.Object).Verifiable(Verifiables, Times.Once());
            CommandsMock.Setup(x => x.Add(cMock.Object)).Verifiable(Verifiables, Times.Once());
            AddSingletonSetup(typeof(object), optionsInstance.Object, Times.Once());

            var b = Builder.WithCommand(typeof(object), optionsInstance.Object, executorFunction.Object);

            Assert.AreSame(Builder, b);
        }

        [TestMethod]
        public void WithCommand_TCommand_ExecutorFunction()
        {
            var executorFunction = Mocks.Create<Func<CliExecutionContext, IDisposable, Task<int>>>();
            var cMock = Mocks.Create<ICliCommandInfo>();
            CommandFactoryMock.Setup(x => x.Create(executorFunction.Object)).Returns(cMock.Object).Verifiable(Verifiables, Times.Once());
            CommandsMock.Setup(x => x.Add(cMock.Object)).Verifiable(Verifiables, Times.Once());
            AddScopedSetup(typeof(IDisposable), typeof(IDisposable), Times.Once());

            var b = Builder.WithCommand(executorFunction.Object);

            Assert.AreSame(Builder, b);
        }

        [TestMethod]
        public void WithCommand_TCommand_OptionsInstance_ExecutorFunction()
        {
            var optionsInstance = Mocks.Create<IDisposable>();
            var executorFunction = Mocks.Create<Func<CliExecutionContext, IDisposable, Task<int>>>();
            var cMock = Mocks.Create<ICliCommandInfo>();
            CommandFactoryMock.Setup(x => x.Create(optionsInstance.Object, executorFunction.Object)).Returns(cMock.Object).Verifiable(Verifiables, Times.Once());
            CommandsMock.Setup(x => x.Add(cMock.Object)).Verifiable(Verifiables, Times.Once());
            AddSingletonSetup(typeof(IDisposable), optionsInstance.Object, Times.Once());

            var b = Builder.WithCommand(optionsInstance.Object, executorFunction.Object);

            Assert.AreSame(Builder, b);
        }

        [TestMethod]
        public void WithCommand_TCommand()
        {
            var cMock = Mocks.Create<ICliCommandInfo>();
            CommandFactoryMock.Setup(x => x.Create<DummyClass>()).Returns(cMock.Object).Verifiable(Verifiables, Times.Once());
            CommandsMock.Setup(x => x.Add(cMock.Object)).Verifiable(Verifiables, Times.Once());
            AddScopedSetup(typeof(DummyClass), typeof(DummyClass), Times.Once());

            var b = Builder.WithCommand<DummyClass>();

            Assert.AreSame(Builder, b);
        }

        [TestMethod]
        public void WithCommand_TCommand_OptionsInstance()
        {
            var optionsInstance = Mocks.Create<DummyClass>();
            var cMock = Mocks.Create<ICliCommandInfo>();
            CommandFactoryMock.Setup(x => x.Create(optionsInstance.Object)).Returns(cMock.Object).Verifiable(Verifiables, Times.Once());
            CommandsMock.Setup(x => x.Add(cMock.Object)).Verifiable(Verifiables, Times.Once());
            AddSingletonSetup(typeof(DummyClass), optionsInstance.Object, Times.Once());

            var b = Builder.WithCommand(optionsInstance.Object);

            Assert.AreSame(Builder, b);
        }

        [TestMethod]
        public void WithCommand_TCommand_TExecutor()
        {
            var cMock = Mocks.Create<ICliCommandInfo>();
            CommandFactoryMock.Setup(x => x.Create<IDisposable, DummyClass>()).Returns(cMock.Object).Verifiable(Verifiables, Times.Once());
            CommandsMock.Setup(x => x.Add(cMock.Object)).Verifiable(Verifiables, Times.Once());
            AddScopedSetup(typeof(IDisposable), typeof(IDisposable), Times.Once());
            AddScopedSetup(typeof(DummyClass), typeof(DummyClass), Times.Once());

            var b = Builder.WithCommand<IDisposable, DummyClass>();

            Assert.AreSame(Builder, b);
        }

        [TestMethod]
        public void WithCommand_TCommand_TExecutor_ExecutorInstance()
        {
            var executorInstance = Mocks.Create<DummyClass>();
            var cMock = Mocks.Create<ICliCommandInfo>();
            CommandFactoryMock.Setup(x => x.Create<IDisposable, DummyClass>(executorInstance.Object)).Returns(cMock.Object).Verifiable(Verifiables, Times.Once());
            CommandsMock.Setup(x => x.Add(cMock.Object)).Verifiable(Verifiables, Times.Once());
            AddScopedSetup(typeof(IDisposable), typeof(IDisposable), Times.Once());
            AddSingletonSetup(typeof(DummyClass), executorInstance.Object, Times.Once());

            var b = Builder.WithCommand<IDisposable, DummyClass>(executorInstance.Object);

            Assert.AreSame(Builder, b);
        }

        [TestMethod]
        public void WithCommand_TCommand_TExecutor_OptionsInstance()
        {
            var optionsInstance = Mocks.Create<IDisposable>();
            var cMock = Mocks.Create<ICliCommandInfo>();
            CommandFactoryMock.Setup(x => x.Create<IDisposable, DummyClass>(optionsInstance.Object)).Returns(cMock.Object).Verifiable(Verifiables, Times.Once());
            CommandsMock.Setup(x => x.Add(cMock.Object)).Verifiable(Verifiables, Times.Once());
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
            var cMock = Mocks.Create<ICliCommandInfo>();
            CommandFactoryMock.Setup(x => x.Create(optionsInstance.Object, executorInstance.Object)).Returns(cMock.Object).Verifiable(Verifiables, Times.Once());
            CommandsMock.Setup(x => x.Add(cMock.Object)).Verifiable(Verifiables, Times.Once());
            AddSingletonSetup(typeof(IDisposable), optionsInstance.Object, Times.Once());
            AddSingletonSetup(typeof(DummyClass), executorInstance.Object, Times.Once());

            var b = Builder.WithCommand(optionsInstance.Object, executorInstance.Object);

            Assert.AreSame(Builder, b);
        }

        private void AddSingletonSetup(Type serviceType, object instance, Times times)
        {
            ServiceCollectionMock
                .Setup(x => x.Add(It.Is<ServiceDescriptor>(d =>
                    d.Lifetime == ServiceLifetime.Singleton &&
                    d.ServiceType == serviceType &&
                    d.ImplementationInstance == instance)))
                .Verifiable(Verifiables, times);
        }

        private void AddScopedSetup(Type serviceType, Type implementationType, Times times)
        {
            ServiceCollectionMock
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
}
