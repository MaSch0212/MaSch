using MaSch.Console.Cli.Runtime;
using MaSch.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;

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

            Assert.AreSame(b, builder);
        }

        [TestMethod]
        public void WithCommand_CommandType()
        {
            var app = Mocks.Create<ICliApplicationBase>();
            var commandType = Mocks.Create<Type>();
            var builder = new DummyBuilder(app.Object);
            app.Setup(x => x.RegisterCommand(commandType.Object)).Verifiable(Verifiables, Times.Once());

            var b = builder.WithCommand(commandType.Object);

            Assert.AreSame(b, builder);
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

            Assert.AreSame(b, builder);
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

            Assert.AreSame(b, builder);
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

            Assert.AreSame(b, builder);
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

            Assert.AreSame(b, builder);
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

            Assert.AreSame(b, builder);
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
    }

    [TestClass]
    public class CliAsyncApplicationBuilderTests : TestClassBase
    {
    }
}
