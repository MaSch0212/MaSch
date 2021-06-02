using MaSch.Console.Cli.Runtime;
using MaSch.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MaSch.Console.Cli.Test
{
    [TestClass]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Base class tests")]
    public class CliApplicationBuilderBaseTests : TestClassBase
    {
        [TestMethod]
        public void Ctor_Null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new DummyBuilder(null!));
        }

        [TestMethod]
        public void Ctor()
        {
            var app = Mocks.Create<ICliApplicationBase>();
            var builder = new DummyBuilder(app.Object);

            Assert.IsNotNull(builder);
            Assert.AreSame(app.Object, builder.Application);
        }

        [TestMethod]
        public void WithCommand_Command()
        {
            var app = Mocks.Create<ICliApplicationBase>();
            var command = Mocks.Create<ICliCommandInfo>();
            var builder = new DummyBuilder(app.Object);
            app.Setup(x => x.RegisterCommand(command.Object)).Verifiable(Verifiables, Times.Once());

            var b = builder.WithCommand(command.Object);

            Assert.AreSame(builder, b);
        }

        [TestMethod]
        public void WithCommand_CommandType()
        {
            var app = Mocks.Create<ICliApplicationBase>();
            var commandType = Mocks.Create<Type>();
            var builder = new DummyBuilder(app.Object);
            app.Setup(x => x.RegisterCommand(commandType.Object)).Verifiable(Verifiables, Times.Once());

            var b = builder.WithCommand(commandType.Object);

            Assert.AreSame(builder, b);
        }

        [TestMethod]
        public void WithCommand_CommandType_OptionsInstance()
        {
            var app = Mocks.Create<ICliApplicationBase>();
            var commandType = Mocks.Create<Type>();
            var optionsInstance = Mocks.Create<object>();
            var builder = new DummyBuilder(app.Object);
            app.Setup(x => x.RegisterCommand(commandType.Object, optionsInstance.Object)).Verifiable(Verifiables, Times.Once());

            var b = builder.WithCommand(commandType.Object, optionsInstance.Object);

            Assert.AreSame(builder, b);
        }

        [TestMethod]
        public void WithCommand_CommandType_ExecutorType()
        {
            var app = Mocks.Create<ICliApplicationBase>();
            var commandType = Mocks.Create<Type>();
            var executorType = Mocks.Create<Type>();
            var builder = new DummyBuilder(app.Object);
            app.Setup(x => x.RegisterCommand(commandType.Object, executorType.Object)).Verifiable(Verifiables, Times.Once());

            var b = builder.WithCommand(commandType.Object, executorType.Object);

            Assert.AreSame(builder, b);
        }

        [TestMethod]
        public void WithCommand_CommandType_OptionsInstance_ExecutorType()
        {
            var app = Mocks.Create<ICliApplicationBase>();
            var commandType = Mocks.Create<Type>();
            var optionsInstance = Mocks.Create<object>();
            var executorType = Mocks.Create<Type>();
            var builder = new DummyBuilder(app.Object);
            app.Setup(x => x.RegisterCommand(commandType.Object, optionsInstance.Object, executorType.Object)).Verifiable(Verifiables, Times.Once());

            var b = builder.WithCommand(commandType.Object, optionsInstance.Object, executorType.Object);

            Assert.AreSame(builder, b);
        }

        [TestMethod]
        public void WithCommand_CommandType_ExecutorType_ExecutorInstance()
        {
            var app = Mocks.Create<ICliApplicationBase>();
            var commandType = Mocks.Create<Type>();
            var executorType = Mocks.Create<Type>();
            var executorInstance = Mocks.Create<object>();
            var builder = new DummyBuilder(app.Object);
            app.Setup(x => x.RegisterCommand(commandType.Object, executorType.Object, executorInstance.Object)).Verifiable(Verifiables, Times.Once());

            var b = builder.WithCommand(commandType.Object, executorType.Object, executorInstance.Object);

            Assert.AreSame(builder, b);
        }

        [TestMethod]
        public void WithCommand_CommandType_OptionsInstance_ExecutorType_ExecutorInstance()
        {
            var app = Mocks.Create<ICliApplicationBase>();
            var command = Mocks.Create<ICliCommandInfo>();
            var commandType = Mocks.Create<Type>();
            var optionsInstance = Mocks.Create<object>();
            var executorType = Mocks.Create<Type>();
            var executorInstance = Mocks.Create<object>();
            var builder = new DummyBuilder(app.Object);
            app.Setup(x => x.RegisterCommand(commandType.Object, optionsInstance.Object, executorType.Object, executorInstance.Object)).Verifiable(Verifiables, Times.Once());

            var b = builder.WithCommand(commandType.Object, optionsInstance.Object, executorType.Object, executorInstance.Object);

            Assert.AreSame(builder, b);
        }

        [TestMethod]
        public void Build()
        {
            var app = Mocks.Create<ICliApplicationBase>();
            var builder = new DummyBuilder(app.Object);

            var a = builder.Build();

            Assert.AreSame(app.Object, a);
        }

        private class DummyBuilder : CliApplicationBuilderBase<ICliApplicationBase, DummyBuilder>
        {
            public new ICliApplicationBase Application => base.Application;

            public DummyBuilder(ICliApplicationBase application)
                : base(application)
            {
            }
        }
    }

    [TestClass]
    public class CliApplicationBuilderTests : TestClassBase
    {
        [TestMethod]
        public void Ctor()
        {
            var builder = new CliApplicationBuilder();
            var app = builder.Build();

            Assert.IsNotNull(app);
            Assert.IsInstanceOfType<CliApplication>(app);
        }

        [TestMethod]
        public void WithCommand_CommandType_ExecutorFunction()
        {
            var builder = CreateCliApplicationBuilder(out var app);
            var executorFunction = Mocks.Create<Func<object, int>>();
            app.Setup(x => x.RegisterCommand(typeof(object), executorFunction.Object)).Verifiable(Verifiables, Times.Once());

            var b = builder.WithCommand(typeof(object), executorFunction.Object);

            Assert.AreSame(builder, b);
        }

        [TestMethod]
        public void WithCommand_CommandType_OptionsInstance_ExecutorFunction()
        {
            var builder = CreateCliApplicationBuilder(out var app);
            var optionsInstance = Mocks.Create<IDisposable>();
            var executorFunction = Mocks.Create<Func<object, int>>();
            app.Setup(x => x.RegisterCommand(typeof(object), optionsInstance.Object, executorFunction.Object)).Verifiable(Verifiables, Times.Once());

            var b = builder.WithCommand(typeof(object), optionsInstance.Object, executorFunction.Object);

            Assert.AreSame(builder, b);
        }

        [TestMethod]
        public void WithCommand_TCommand_ExecutorFunction()
        {
            var builder = CreateCliApplicationBuilder(out var app);
            var executorFunction = Mocks.Create<Func<IDisposable, int>>();
            app.Setup(x => x.RegisterCommand(executorFunction.Object)).Verifiable(Verifiables, Times.Once());

            var b = builder.WithCommand(executorFunction.Object);

            Assert.AreSame(builder, b);
        }

        [TestMethod]
        public void WithCommand_TCommand_OptionsInstance_ExecutorFunction()
        {
            var builder = CreateCliApplicationBuilder(out var app);
            var optionsInstance = Mocks.Create<IDisposable>();
            var executorFunction = Mocks.Create<Func<IDisposable, int>>();
            app.Setup(x => x.RegisterCommand(optionsInstance.Object, executorFunction.Object)).Verifiable(Verifiables, Times.Once());

            var b = builder.WithCommand(optionsInstance.Object, executorFunction.Object);

            Assert.AreSame(builder, b);
        }

        [TestMethod]
        public void WithCommand_TCommand()
        {
            var builder = CreateCliApplicationBuilder(out var app);
            app.Setup(x => x.RegisterCommand<DummyClass>()).Verifiable(Verifiables, Times.Once());

            var b = builder.WithCommand<DummyClass>();

            Assert.AreSame(builder, b);
        }

        [TestMethod]
        public void WithCommand_TCommand_OptionsInstance()
        {
            var builder = CreateCliApplicationBuilder(out var app);
            var optionsInstance = Mocks.Create<DummyClass>();
            app.Setup(x => x.RegisterCommand(optionsInstance.Object)).Verifiable(Verifiables, Times.Once());

            var b = builder.WithCommand(optionsInstance.Object);

            Assert.AreSame(builder, b);
        }

        [TestMethod]
        public void WithCommand_TCommand_TExecutor()
        {
            var builder = CreateCliApplicationBuilder(out var app);
            app.Setup(x => x.RegisterCommand<IDisposable, DummyClass>()).Verifiable(Verifiables, Times.Once());

            var b = builder.WithCommand<IDisposable, DummyClass>();

            Assert.AreSame(builder, b);
        }

        [TestMethod]
        public void WithCommand_TCommand_TExecutor_ExecutorInstance()
        {
            var builder = CreateCliApplicationBuilder(out var app);
            var executorInstance = Mocks.Create<DummyClass>();
            app.Setup(x => x.RegisterCommand<IDisposable, DummyClass>(executorInstance.Object)).Verifiable(Verifiables, Times.Once());

            var b = builder.WithCommand<IDisposable, DummyClass>(executorInstance.Object);

            Assert.AreSame(builder, b);
        }

        [TestMethod]
        public void WithCommand_TCommand_TExecutor_OptionsInstance()
        {
            var builder = CreateCliApplicationBuilder(out var app);
            var optionsInstance = Mocks.Create<IDisposable>();
            app.Setup(x => x.RegisterCommand<IDisposable, DummyClass>(optionsInstance.Object)).Verifiable(Verifiables, Times.Once());

            var b = builder.WithCommand<IDisposable, DummyClass>(optionsInstance.Object);

            Assert.AreSame(builder, b);
        }

        [TestMethod]
        public void WithCommand_TCommand_TExecutor_OptionsInstance_ExecutorInstance()
        {
            var builder = CreateCliApplicationBuilder(out var app);
            var optionsInstance = Mocks.Create<IDisposable>();
            var executorInstance = Mocks.Create<DummyClass>();
            app.Setup(x => x.RegisterCommand(optionsInstance.Object, executorInstance.Object)).Verifiable(Verifiables, Times.Once());

            var b = builder.WithCommand(optionsInstance.Object, executorInstance.Object);

            Assert.AreSame(builder, b);
        }

        [TestMethod]
        public void Configure()
        {
            var options = new CliApplicationOptions();
            var builder = CreateCliApplicationBuilder(out var app);
            var action = Mocks.Create<Action<CliApplicationOptions>>();
            app.Setup(x => x.Options).Returns(options);
            action.Setup(x => x(options)).Verifiable(Verifiables, Times.Once());

            var b = builder.Configure(action.Object);

            Assert.AreSame(builder, b);
        }

        private CliApplicationBuilder CreateCliApplicationBuilder(out Mock<ICliApplication> appMock)
        {
            appMock = Mocks.Create<ICliApplication>();
            var result = Mocks.Create<CliApplicationBuilder>(appMock.Object);
            return result.Object;
        }

        public abstract class DummyClass : ICliCommandExecutor, ICliCommandExecutor<IDisposable>
        {
            public abstract int ExecuteCommand();
            public abstract int ExecuteCommand(IDisposable parameters);
        }
    }

    [TestClass]
    public class CliAsyncApplicationBuilderTests : TestClassBase
    {
        [TestMethod]
        public void Ctor()
        {
            var builder = new CliAsyncApplicationBuilder();
            var app = builder.Build();

            Assert.IsNotNull(app);
            Assert.IsInstanceOfType<CliAsyncApplication>(app);
        }

        [TestMethod]
        public void WithCommand_CommandType_ExecutorFunction()
        {
            var builder = CreateCliAsyncApplicationBuilder(out var app);
            var executorFunction = Mocks.Create<Func<object, Task<int>>>();
            app.Setup(x => x.RegisterCommand(typeof(object), executorFunction.Object)).Verifiable(Verifiables, Times.Once());

            var b = builder.WithCommand(typeof(object), executorFunction.Object);

            Assert.AreSame(builder, b);
        }

        [TestMethod]
        public void WithCommand_CommandType_OptionsInstance_ExecutorFunction()
        {
            var builder = CreateCliAsyncApplicationBuilder(out var app);
            var optionsInstance = Mocks.Create<IDisposable>();
            var executorFunction = Mocks.Create<Func<object, Task<int>>>();
            app.Setup(x => x.RegisterCommand(typeof(object), optionsInstance.Object, executorFunction.Object)).Verifiable(Verifiables, Times.Once());

            var b = builder.WithCommand(typeof(object), optionsInstance.Object, executorFunction.Object);

            Assert.AreSame(builder, b);
        }

        [TestMethod]
        public void WithCommand_TCommand_ExecutorFunction()
        {
            var builder = CreateCliAsyncApplicationBuilder(out var app);
            var executorFunction = Mocks.Create<Func<IDisposable, Task<int>>>();
            app.Setup(x => x.RegisterCommand(executorFunction.Object)).Verifiable(Verifiables, Times.Once());

            var b = builder.WithCommand(executorFunction.Object);

            Assert.AreSame(builder, b);
        }

        [TestMethod]
        public void WithCommand_TCommand_OptionsInstance_ExecutorFunction()
        {
            var builder = CreateCliAsyncApplicationBuilder(out var app);
            var optionsInstance = Mocks.Create<IDisposable>();
            var executorFunction = Mocks.Create<Func<IDisposable, Task<int>>>();
            app.Setup(x => x.RegisterCommand(optionsInstance.Object, executorFunction.Object)).Verifiable(Verifiables, Times.Once());

            var b = builder.WithCommand(optionsInstance.Object, executorFunction.Object);

            Assert.AreSame(builder, b);
        }

        [TestMethod]
        public void WithCommand_TCommand()
        {
            var builder = CreateCliAsyncApplicationBuilder(out var app);
            app.Setup(x => x.RegisterCommand<DummyClass>()).Verifiable(Verifiables, Times.Once());

            var b = builder.WithCommand<DummyClass>();

            Assert.AreSame(builder, b);
        }

        [TestMethod]
        public void WithCommand_TCommand_OptionsInstance()
        {
            var builder = CreateCliAsyncApplicationBuilder(out var app);
            var optionsInstance = Mocks.Create<DummyClass>();
            app.Setup(x => x.RegisterCommand(optionsInstance.Object)).Verifiable(Verifiables, Times.Once());

            var b = builder.WithCommand(optionsInstance.Object);

            Assert.AreSame(builder, b);
        }

        [TestMethod]
        public void WithCommand_TCommand_TExecutor()
        {
            var builder = CreateCliAsyncApplicationBuilder(out var app);
            app.Setup(x => x.RegisterCommand<IDisposable, DummyClass>()).Verifiable(Verifiables, Times.Once());

            var b = builder.WithCommand<IDisposable, DummyClass>();

            Assert.AreSame(builder, b);
        }

        [TestMethod]
        public void WithCommand_TCommand_TExecutor_ExecutorInstance()
        {
            var builder = CreateCliAsyncApplicationBuilder(out var app);
            var executorInstance = Mocks.Create<DummyClass>();
            app.Setup(x => x.RegisterCommand<IDisposable, DummyClass>(executorInstance.Object)).Verifiable(Verifiables, Times.Once());

            var b = builder.WithCommand<IDisposable, DummyClass>(executorInstance.Object);

            Assert.AreSame(builder, b);
        }

        [TestMethod]
        public void WithCommand_TCommand_TExecutor_OptionsInstance()
        {
            var builder = CreateCliAsyncApplicationBuilder(out var app);
            var optionsInstance = Mocks.Create<IDisposable>();
            app.Setup(x => x.RegisterCommand<IDisposable, DummyClass>(optionsInstance.Object)).Verifiable(Verifiables, Times.Once());

            var b = builder.WithCommand<IDisposable, DummyClass>(optionsInstance.Object);

            Assert.AreSame(builder, b);
        }

        [TestMethod]
        public void WithCommand_TCommand_TExecutor_OptionsInstance_ExecutorInstance()
        {
            var builder = CreateCliAsyncApplicationBuilder(out var app);
            var optionsInstance = Mocks.Create<IDisposable>();
            var executorInstance = Mocks.Create<DummyClass>();
            app.Setup(x => x.RegisterCommand(optionsInstance.Object, executorInstance.Object)).Verifiable(Verifiables, Times.Once());

            var b = builder.WithCommand(optionsInstance.Object, executorInstance.Object);

            Assert.AreSame(builder, b);
        }

        [TestMethod]
        public void Configure()
        {
            var options = new CliApplicationOptions();
            var builder = CreateCliAsyncApplicationBuilder(out var app);
            var action = Mocks.Create<Action<CliApplicationOptions>>();
            app.Setup(x => x.Options).Returns(options);
            action.Setup(x => x(options)).Verifiable(Verifiables, Times.Once());

            var b = builder.Configure(action.Object);

            Assert.AreSame(builder, b);
        }

        private CliAsyncApplicationBuilder CreateCliAsyncApplicationBuilder(out Mock<ICliAsyncApplication> appMock)
        {
            appMock = Mocks.Create<ICliAsyncApplication>();
            var result = Mocks.Create<CliAsyncApplicationBuilder>(appMock.Object);
            return result.Object;
        }

        public abstract class DummyClass : ICliAsyncCommandExecutor, ICliAsyncCommandExecutor<IDisposable>
        {
            public abstract Task<int> ExecuteCommandAsync(IDisposable parameters);
            public abstract Task<int> ExecuteCommandAsync();
        }
    }
}
