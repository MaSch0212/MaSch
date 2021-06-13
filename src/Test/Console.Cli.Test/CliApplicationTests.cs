using MaSch.Console.Cli.Runtime;
using MaSch.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;

namespace MaSch.Console.Cli.Test
{
    [TestClass]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Base class tests")]
    public class CliApplicationBaseTests : TestClassBase
    {
        public delegate bool TryParseArgumentsDelegate(string[] args, [NotNullWhen(true)] out ICliCommandInfo? command, [NotNullWhen(true)] out object? options, out int errorCode);

        [TestMethod]
        public void Ctor_Success()
        {
            var appMock = CreateAppMock();

            var app = appMock.Object;

            Assert.IsNotNull(app);
            Assert.IsNotNull(app.Options);
        }

        [TestMethod]
        public void Ctor_NullChecks()
        {
            var appMock = Mocks.Create<CliApplicationBase>();
            appMock.Protected().SetupGet<Type>("ExecutorType").Returns((Type?)null!);
            appMock.Protected().SetupGet<Type>("GenericExecutorType").Returns(typeof(ICliCommandExecutor<>));

            var ex = Assert.ThrowsException<TargetInvocationException>(() => appMock.Object);
            Assert.IsInstanceOfType<ArgumentNullException>(ex.InnerException);

            appMock.Protected().SetupGet<Type>("ExecutorType").Returns(typeof(ICliCommandExecutor));
            appMock.Protected().SetupGet<Type>("GenericExecutorType").Returns((Type?)null!);
            ex = Assert.ThrowsException<TargetInvocationException>(() => appMock.Object);
            Assert.IsInstanceOfType<ArgumentNullException>(ex.InnerException);
        }

        [TestMethod]
        public void Ctor_WrongGenericExecutorType()
        {
            var appMock = Mocks.Create<CliApplicationBase>();
            appMock.Protected().SetupGet<Type>("ExecutorType").Returns(typeof(ICliCommandExecutor));
            appMock.Protected().SetupGet<Type>("GenericExecutorType").Returns(typeof(Action));

            var ex = Assert.ThrowsException<TargetInvocationException>(() => appMock.Object);
            Assert.IsInstanceOfType<ArgumentException>(ex.InnerException);

            appMock.Protected().SetupGet<Type>("GenericExecutorType").Returns(typeof(Action<int, int>));
            ex = Assert.ThrowsException<TargetInvocationException>(() => appMock.Object);
            Assert.IsInstanceOfType<ArgumentException>(ex.InnerException);
        }

        [TestMethod]
        public void Ctor_WithOptions()
        {
            var options = new CliApplicationOptions();
            var appMock = Mocks.Create<CliApplicationBase>(options);
            appMock.Protected().SetupGet<Type>("ExecutorType").Returns(typeof(ICliCommandExecutor));
            appMock.Protected().SetupGet<Type>("GenericExecutorType").Returns(typeof(ICliCommandExecutor<>));

            var app = appMock.Object;

            Assert.IsNotNull(app);
            Assert.AreSame(options, app.Options);
        }

        [TestMethod]
        public void CommandFactory()
        {
            var appMock = CreateAppMock();
            var factoryMock = Mocks.Create<ICliCommandInfoFactory>();

            appMock.Object.CommandFactory = factoryMock.Object;

            Assert.AreSame(factoryMock.Object, appMock.Object.CommandFactory);

            appMock.Object.CommandFactory = null;

            Assert.IsNotNull(appMock.Object.CommandFactory);
            Assert.IsInstanceOfType<CliCommandInfoFactory>(appMock.Object.CommandFactory);
        }

        [TestMethod]
        public void Parser()
        {
            var appMock = CreateAppMock();
            var parserMock = Mocks.Create<ICliArgumentParser>();

            appMock.Object.Parser = parserMock.Object;

            Assert.AreSame(parserMock.Object, appMock.Object.Parser);

            appMock.Object.Parser = null;

            Assert.IsNotNull(appMock.Object.Parser);
            Assert.IsInstanceOfType<CliArgumentParser>(appMock.Object.Parser);
        }

        [TestMethod]
        public void HelpPage()
        {
            var appMock = CreateAppMock();
            var helpPageMock = Mocks.Create<ICliHelpPage>();

            appMock.Object.HelpPage = helpPageMock.Object;

            Assert.AreSame(helpPageMock.Object, appMock.Object.HelpPage);

            appMock.Object.HelpPage = null;

            Assert.IsNotNull(appMock.Object.HelpPage);
            Assert.IsInstanceOfType<CliHelpPage>(appMock.Object.HelpPage);
        }

        [TestMethod]
        public void Commands()
        {
            var appMock = CreateAppMock();

            var commands1 = appMock.Object.Commands;

            Assert.IsNotInstanceOfType<ICliCommandInfoCollection>(commands1);
            Assert.IsNotInstanceOfType<ICollection<ICliCommandInfo>>(commands1);

            var commands2 = appMock.Object.Commands;

            Assert.AreSame(commands1, commands2);
        }

        [TestMethod]
        public void RegisterCommand_Command()
        {
            var appMock = CreateAppMock(out var collectionMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            appMock.Object.RegisterCommand(commandMock.Object);
        }

        [TestMethod]
        public void RegisterCommand_CommandType()
        {
            var appMock = CreateAppMock(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var commandTypeMock = Mocks.Create<Type>(MockBehavior.Loose);
            commandFactoryMock.Setup(x => x.Create(commandTypeMock.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            appMock.Object.RegisterCommand(commandTypeMock.Object);
        }

        [TestMethod]
        public void RegisterCommand_CommandType_OptionsInstance()
        {
            var appMock = CreateAppMock(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var commandTypeMock = Mocks.Create<Type>(MockBehavior.Loose);
            var optionsInstance = new object();
            commandFactoryMock.Setup(x => x.Create(commandTypeMock.Object, optionsInstance)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            appMock.Object.RegisterCommand(commandTypeMock.Object, optionsInstance);
        }

        [TestMethod]
        public void RegisterCommand_CommandType_ExecutorType()
        {
            var appMock = CreateAppMock(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var commandTypeMock = Mocks.Create<Type>(MockBehavior.Loose);
            var executorTypeMock = Mocks.Create<Type>(MockBehavior.Loose);
            commandFactoryMock.Setup(x => x.Create(commandTypeMock.Object, executorTypeMock.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            appMock.Object.RegisterCommand(commandTypeMock.Object, executorTypeMock.Object);
        }

        [TestMethod]
        public void RegisterCommand_CommandType_OptionsInstance_ExecutorType()
        {
            var appMock = CreateAppMock(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var commandTypeMock = Mocks.Create<Type>(MockBehavior.Loose);
            var optionsInstance = new object();
            var executorTypeMock = Mocks.Create<Type>(MockBehavior.Loose);
            commandFactoryMock.Setup(x => x.Create(commandTypeMock.Object, optionsInstance, executorTypeMock.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            appMock.Object.RegisterCommand(commandTypeMock.Object, optionsInstance, executorTypeMock.Object);
        }

        [TestMethod]
        public void RegisterCommand_CommandType_ExecutorType_ExecutorInstance()
        {
            var appMock = CreateAppMock(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var commandTypeMock = Mocks.Create<Type>(MockBehavior.Loose);
            var executorTypeMock = Mocks.Create<Type>(MockBehavior.Loose);
            var executorInstance = new object();
            commandFactoryMock.Setup(x => x.Create(commandTypeMock.Object, executorTypeMock.Object, executorInstance)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            appMock.Object.RegisterCommand(commandTypeMock.Object, executorTypeMock.Object, executorInstance);
        }

        [TestMethod]
        public void RegisterCommand_CommandType_OptionsInstance_ExecutorType_ExecutorInstance()
        {
            var appMock = CreateAppMock(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var commandTypeMock = Mocks.Create<Type>(MockBehavior.Loose);
            var executorTypeMock = Mocks.Create<Type>(MockBehavior.Loose);
            var optionsInstance = new object();
            var executorInstance = new object();
            commandFactoryMock.Setup(x => x.Create(commandTypeMock.Object, optionsInstance, executorTypeMock.Object, executorInstance)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            appMock.Object.RegisterCommand(commandTypeMock.Object, optionsInstance, executorTypeMock.Object, executorInstance);
        }

        [TestMethod]
        public void TryParseArguments_Success()
        {
            var appMock = CreateAppMockForParse(out var parserMock, out _);
            var args = new[] { "blub" };
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var optionsInstance = new object();
            parserMock
                .Setup(x => x.Parse(appMock.Object, args))
                .Returns(new CliArgumentParserResult(commandMock.Object, optionsInstance))
                .Verifiable(Verifiables, Times.Once());

            var result = appMock.Object.TryParseArguments(args, out var command, out var options, out var errorCode);

            Assert.IsTrue(result);
            Assert.AreSame(commandMock.Object, command);
            Assert.AreSame(optionsInstance, options);
            Assert.AreEqual(0, errorCode);
        }

        [TestMethod]
        public void TryParseArguments_Fail_HelpPage()
        {
            var appMock = CreateAppMockForParse(out var parserMock, out var helpPageMock);
            var errors = new[] { new CliError(CliErrorType.VersionRequested) };
            var args = new[] { "blub" };
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var optionsInstance = new object();
            parserMock
                .Setup(x => x.Parse(appMock.Object, args))
                .Returns(new CliArgumentParserResult(errors))
                .Verifiable(Verifiables, Times.Once());
            helpPageMock
                .Setup(x => x.Write(appMock.Object, errors))
                .Returns(true)
                .Verifiable(Verifiables, Times.Once());

            var result = appMock.Object.TryParseArguments(args, out var command, out var options, out var errorCode);

            Assert.IsFalse(result);
            Assert.IsNull(command);
            Assert.IsNull(options);
            Assert.AreEqual(0, errorCode);
        }

        [TestMethod]
        public void TryParseArguments_Fail_ParseError()
        {
            var appMock = CreateAppMockForParse(out var parserMock, out var helpPageMock);
            var errors = new[] { new CliError("My custom error") };
            var args = new[] { "blub" };
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var optionsInstance = new object();
            parserMock
                .Setup(x => x.Parse(appMock.Object, args))
                .Returns(new CliArgumentParserResult(errors))
                .Verifiable(Verifiables, Times.Once());
            helpPageMock
                .Setup(x => x.Write(appMock.Object, errors))
                .Returns(false)
                .Verifiable(Verifiables, Times.Once());
            appMock.Object.Options.ParseErrorExitCode = 4711;

            var result = appMock.Object.TryParseArguments(args, out var command, out var options, out var errorCode);

            Assert.IsFalse(result);
            Assert.IsNull(command);
            Assert.IsNull(options);
            Assert.AreEqual(4711, errorCode);
        }

        private Mock<CliApplicationBase> CreateAppMock()
        {
            var appMock = Mocks.Create<CliApplicationBase>();
            SetupAppMock(appMock);
            return appMock;
        }

        private Mock<CliApplicationBase> CreateAppMock(out Mock<ICliCommandInfoCollection> collectionMock)
        {
            collectionMock = Mocks.Create<ICliCommandInfoCollection>();
            var appMock = Mocks.Create<CliApplicationBase>(new CliApplicationOptions(), collectionMock.Object);
            SetupAppMock(appMock);
            return appMock;
        }

        private Mock<CliApplicationBase> CreateAppMock(out Mock<ICliCommandInfoCollection> collectionMock, out Mock<ICliCommandInfoFactory> commandFactoryMock)
        {
            var appMock = CreateAppMock(out collectionMock);
            commandFactoryMock = Mocks.Create<ICliCommandInfoFactory>();
            appMock.Object.CommandFactory = commandFactoryMock.Object;
            return appMock;
        }

        private Mock<TestCliApplicationBase> CreateAppMockForParse(out Mock<ICliArgumentParser> parserMock, out Mock<ICliHelpPage> helpPageMock)
        {
            var appMock = Mocks.Create<TestCliApplicationBase>();
            SetupAppMock(appMock);
            parserMock = Mocks.Create<ICliArgumentParser>();
            helpPageMock = Mocks.Create<ICliHelpPage>();
            appMock.Object.Parser = parserMock.Object;
            appMock.Object.HelpPage = helpPageMock.Object;
            return appMock;
        }

        private static void SetupAppMock<T>(Mock<T> appMock)
            where T : CliApplicationBase
        {
            appMock.Protected().SetupGet<Type>("ExecutorType").Returns(typeof(ICliCommandExecutor));
            appMock.Protected().SetupGet<Type>("GenericExecutorType").Returns(typeof(ICliCommandExecutor<>));
        }

        public abstract class TestCliApplicationBase : CliApplicationBase
        {
            public new bool TryParseArguments(string[] args, [NotNullWhen(true)] out ICliCommandInfo? command, [NotNullWhen(true)] out object? options, out int errorCode)
                => base.TryParseArguments(args, out command, out options, out errorCode);
        }
    }

    [TestClass]
    public class CliApplicationTests : TestClassBase
    {
        [TestMethod]
        public void Ctor()
        {
            var app = new CliApplication();

            Assert.IsNotNull(app.Options);
        }

        [TestMethod]
        public void Ctor_Options_Null()
        {
            var app = new CliApplication(null);

            Assert.IsNotNull(app.Options);
        }

        [TestMethod]
        public void Ctor_Options()
        {
            var options = new Mock<CliApplicationOptions>();

            var app = new CliApplication(options.Object);

            Assert.AreSame(options.Object, app.Options);
        }

        [TestMethod]
        public void RegisterCommand_Command()
        {
            var app = CreateApp(out var collectionMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            app.RegisterCommand(commandMock.Object);
        }

        [TestMethod]
        public void RegisterCommand_CommandType()
        {
            var app = CreateApp(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var commandTypeMock = Mocks.Create<Type>(MockBehavior.Loose);
            commandFactoryMock.Setup(x => x.Create(commandTypeMock.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            app.RegisterCommand(commandTypeMock.Object);
        }

        [TestMethod]
        public void RegisterCommand_CommandType_OptionsInstance()
        {
            var app = CreateApp(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var commandTypeMock = Mocks.Create<Type>(MockBehavior.Loose);
            var optionsInstance = new object();
            commandFactoryMock.Setup(x => x.Create(commandTypeMock.Object, optionsInstance)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            app.RegisterCommand(commandTypeMock.Object, optionsInstance);
        }

        [TestMethod]
        public void RegisterCommand_CommandType_ExecutorType()
        {
            var app = CreateApp(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var commandTypeMock = Mocks.Create<Type>(MockBehavior.Loose);
            var executorTypeMock = Mocks.Create<Type>(MockBehavior.Loose);
            commandFactoryMock.Setup(x => x.Create(commandTypeMock.Object, executorTypeMock.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            app.RegisterCommand(commandTypeMock.Object, executorTypeMock.Object);
        }

        [TestMethod]
        public void RegisterCommand_CommandType_OptionsInstance_ExecutorType()
        {
            var app = CreateApp(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var commandTypeMock = Mocks.Create<Type>(MockBehavior.Loose);
            var optionsInstance = new object();
            var executorTypeMock = Mocks.Create<Type>(MockBehavior.Loose);
            commandFactoryMock.Setup(x => x.Create(commandTypeMock.Object, optionsInstance, executorTypeMock.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            app.RegisterCommand(commandTypeMock.Object, optionsInstance, executorTypeMock.Object);
        }

        [TestMethod]
        public void RegisterCommand_CommandType_ExecutorType_ExecutorInstance()
        {
            var app = CreateApp(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var commandTypeMock = Mocks.Create<Type>(MockBehavior.Loose);
            var executorTypeMock = Mocks.Create<Type>(MockBehavior.Loose);
            var executorInstance = new object();
            commandFactoryMock.Setup(x => x.Create(commandTypeMock.Object, executorTypeMock.Object, executorInstance)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            app.RegisterCommand(commandTypeMock.Object, executorTypeMock.Object, executorInstance);
        }

        [TestMethod]
        public void RegisterCommand_CommandType_OptionsInstance_ExecutorType_ExecutorInstance()
        {
            var app = CreateApp(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var commandTypeMock = Mocks.Create<Type>(MockBehavior.Loose);
            var executorTypeMock = Mocks.Create<Type>(MockBehavior.Loose);
            var optionsInstance = new object();
            var executorInstance = new object();
            commandFactoryMock.Setup(x => x.Create(commandTypeMock.Object, optionsInstance, executorTypeMock.Object, executorInstance)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            app.RegisterCommand(commandTypeMock.Object, optionsInstance, executorTypeMock.Object, executorInstance);
        }

        [TestMethod]
        public void RegisterCommand_CommandType_ExecutorFunction()
        {
            var app = CreateApp(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var commandTypeMock = Mocks.Create<Type>(MockBehavior.Loose);
            var executorFunctionMock = Mocks.Create<Func<object, int>>();
            commandFactoryMock.Setup(x => x.Create(commandTypeMock.Object, executorFunctionMock.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            app.RegisterCommand(commandTypeMock.Object, executorFunctionMock.Object);
        }

        [TestMethod]
        public void RegisterCommand_CommandType_OptionsInstance_ExecutorFunction()
        {
            var app = CreateApp(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var commandTypeMock = Mocks.Create<Type>(MockBehavior.Loose);
            var optionsInstance = new object();
            var executorFunctionMock = Mocks.Create<Func<object, int>>();
            commandFactoryMock.Setup(x => x.Create(commandTypeMock.Object, optionsInstance, executorFunctionMock.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            app.RegisterCommand(commandTypeMock.Object, optionsInstance, executorFunctionMock.Object);
        }

        [TestMethod]
        public void RegisterCommand_TCommand_ExecutorFunction()
        {
            var app = CreateApp(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var executorFunctionMock = Mocks.Create<Func<DummyClass1, int>>();
            commandFactoryMock.Setup(x => x.Create(executorFunctionMock.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            app.RegisterCommand(executorFunctionMock.Object);
        }

        [TestMethod]
        public void RegisterCommand_TCommand_OptionsInstance_ExecutorFunction()
        {
            var app = CreateApp(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var optionsInstance = new DummyClass1();
            var executorFunctionMock = Mocks.Create<Func<DummyClass1, int>>();
            commandFactoryMock.Setup(x => x.Create(optionsInstance, executorFunctionMock.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            app.RegisterCommand(optionsInstance, executorFunctionMock.Object);
        }

        [TestMethod]
        public void RegisterCommand_TCommand()
        {
            var app = CreateApp(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            commandFactoryMock.Setup(x => x.Create<DummyClass2>()).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            app.RegisterCommand<DummyClass2>();
        }

        [TestMethod]
        public void RegisterCommand_TCommand_OptionsInstance()
        {
            var app = CreateApp(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var optionsInstance = new DummyClass2();
            commandFactoryMock.Setup(x => x.Create(optionsInstance)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            app.RegisterCommand(optionsInstance);
        }

        [TestMethod]
        public void RegisterCommand_TCommand_TExecutor()
        {
            var app = CreateApp(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            commandFactoryMock.Setup(x => x.Create<DummyClass1, DummyClass3>()).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            app.RegisterCommand<DummyClass1, DummyClass3>();
        }

        [TestMethod]
        public void RegisterCommand_TCommand_TExecutor_ExecutorInstance()
        {
            var app = CreateApp(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var executorInstance = new DummyClass3();
            commandFactoryMock.Setup(x => x.Create<DummyClass1, DummyClass3>(executorInstance)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            app.RegisterCommand<DummyClass1, DummyClass3>(executorInstance);
        }

        [TestMethod]
        public void RegisterCommand_TCommand_TExecutor_OptionsInstance()
        {
            var app = CreateApp(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var optionsInstance = new DummyClass1();
            commandFactoryMock.Setup(x => x.Create<DummyClass1, DummyClass3>(optionsInstance)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            app.RegisterCommand<DummyClass1, DummyClass3>(optionsInstance);
        }

        [TestMethod]
        public void RegisterCommand_TCommand_TExecutor_OptionsInstance_ExecutorInstance()
        {
            var app = CreateApp(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var optionsInstance = new DummyClass1();
            var executorInstance = new DummyClass3();
            commandFactoryMock.Setup(x => x.Create(optionsInstance, executorInstance)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            app.RegisterCommand(optionsInstance, executorInstance);
        }

        [TestMethod]
        public void Run_Success()
        {
            var appMock = Mocks.Create<CliApplication>();
            SetupAppMock(appMock);
            var args = new[] { "blub" };
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var options = new object();
            appMock.Protected()
                .Setup<bool>("TryParseArguments", args, ItExpr.Ref<ICliCommandInfo?>.IsAny, ItExpr.Ref<object?>.IsAny, ItExpr.Ref<int>.IsAny)
                .Returns(new CliApplicationBaseTests.TryParseArgumentsDelegate((string[] a, out ICliCommandInfo? c, out object? o, out int e) =>
                {
                    c = commandMock.Object;
                    o = options;
                    e = 0;
                    return true;
                }))
                .Verifiable(Verifiables, Times.Once());
            commandMock.Setup(x => x.Execute(options)).Returns(4711).Verifiable(Verifiables, Times.Once());

            var result = appMock.Object.Run(args);

            Assert.AreEqual(4711, result);
        }

        [TestMethod]
        public void Run_Fail()
        {
            var appMock = Mocks.Create<CliApplication>();
            SetupAppMock(appMock);
            var args = new[] { "blub" };
            appMock.Protected()
                .Setup<bool>("TryParseArguments", args, ItExpr.Ref<ICliCommandInfo?>.IsAny, ItExpr.Ref<object?>.IsAny, ItExpr.Ref<int>.IsAny)
                .Returns(new CliApplicationBaseTests.TryParseArgumentsDelegate((string[] a, out ICliCommandInfo? c, out object? o, out int e) =>
                {
                    c = null;
                    o = null;
                    e = 4711;
                    return false;
                }))
                .Verifiable(Verifiables, Times.Once());

            var result = appMock.Object.Run(args);

            Assert.AreEqual(4711, result);
        }

        private CliApplication CreateApp(out Mock<ICliCommandInfoCollection> collectionMock)
        {
            collectionMock = Mocks.Create<ICliCommandInfoCollection>();
            var po = new PrivateObject(typeof(CliApplication), null, collectionMock.Object);
            return (CliApplication)po.Target;
        }

        private CliApplication CreateApp(out Mock<ICliCommandInfoCollection> collectionMock, out Mock<ICliCommandInfoFactory> commandFactoryMock)
        {
            commandFactoryMock = Mocks.Create<ICliCommandInfoFactory>();
            var result = CreateApp(out collectionMock);
            result.CommandFactory = commandFactoryMock.Object;
            return result;
        }

        private static void SetupAppMock(Mock<CliApplication> appMock)
        {
            appMock.Protected().SetupGet<Type>("ExecutorType").CallBase();
            appMock.Protected().SetupGet<Type>("GenericExecutorType").CallBase();
        }

        public class DummyClass1
        {
        }

        public class DummyClass2 : ICliCommandExecutor
        {
            [ExcludeFromCodeCoverage]
            public int ExecuteCommand(ICliCommandInfo command)
            {
                throw new NotImplementedException();
            }
        }

        public class DummyClass3 : ICliCommandExecutor<DummyClass1>
        {
            [ExcludeFromCodeCoverage]
            public int ExecuteCommand(ICliCommandInfo command, DummyClass1 parameters)
            {
                throw new NotImplementedException();
            }
        }
    }

    [TestClass]
    public class CliAsyncApplicationTests : TestClassBase
    {
        [TestMethod]
        public void Ctor()
        {
            var app = new CliAsyncApplication();

            Assert.IsNotNull(app.Options);
        }

        [TestMethod]
        public void Ctor_Options_Null()
        {
            var app = new CliAsyncApplication(null);

            Assert.IsNotNull(app.Options);
        }

        [TestMethod]
        public void Ctor_Options()
        {
            var options = new Mock<CliApplicationOptions>();

            var app = new CliAsyncApplication(options.Object);

            Assert.AreSame(options.Object, app.Options);
        }

        [TestMethod]
        public void RegisterCommand_Command()
        {
            var app = CreateApp(out var collectionMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            app.RegisterCommand(commandMock.Object);
        }

        [TestMethod]
        public void RegisterCommand_CommandType()
        {
            var app = CreateApp(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var commandTypeMock = Mocks.Create<Type>(MockBehavior.Loose);
            commandFactoryMock.Setup(x => x.Create(commandTypeMock.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            app.RegisterCommand(commandTypeMock.Object);
        }

        [TestMethod]
        public void RegisterCommand_CommandType_OptionsInstance()
        {
            var app = CreateApp(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var commandTypeMock = Mocks.Create<Type>(MockBehavior.Loose);
            var optionsInstance = new object();
            commandFactoryMock.Setup(x => x.Create(commandTypeMock.Object, optionsInstance)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            app.RegisterCommand(commandTypeMock.Object, optionsInstance);
        }

        [TestMethod]
        public void RegisterCommand_CommandType_ExecutorType()
        {
            var app = CreateApp(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var commandTypeMock = Mocks.Create<Type>(MockBehavior.Loose);
            var executorTypeMock = Mocks.Create<Type>(MockBehavior.Loose);
            commandFactoryMock.Setup(x => x.Create(commandTypeMock.Object, executorTypeMock.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            app.RegisterCommand(commandTypeMock.Object, executorTypeMock.Object);
        }

        [TestMethod]
        public void RegisterCommand_CommandType_OptionsInstance_ExecutorType()
        {
            var app = CreateApp(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var commandTypeMock = Mocks.Create<Type>(MockBehavior.Loose);
            var optionsInstance = new object();
            var executorTypeMock = Mocks.Create<Type>(MockBehavior.Loose);
            commandFactoryMock.Setup(x => x.Create(commandTypeMock.Object, optionsInstance, executorTypeMock.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            app.RegisterCommand(commandTypeMock.Object, optionsInstance, executorTypeMock.Object);
        }

        [TestMethod]
        public void RegisterCommand_CommandType_ExecutorType_ExecutorInstance()
        {
            var app = CreateApp(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var commandTypeMock = Mocks.Create<Type>(MockBehavior.Loose);
            var executorTypeMock = Mocks.Create<Type>(MockBehavior.Loose);
            var executorInstance = new object();
            commandFactoryMock.Setup(x => x.Create(commandTypeMock.Object, executorTypeMock.Object, executorInstance)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            app.RegisterCommand(commandTypeMock.Object, executorTypeMock.Object, executorInstance);
        }

        [TestMethod]
        public void RegisterCommand_CommandType_OptionsInstance_ExecutorType_ExecutorInstance()
        {
            var app = CreateApp(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var commandTypeMock = Mocks.Create<Type>(MockBehavior.Loose);
            var executorTypeMock = Mocks.Create<Type>(MockBehavior.Loose);
            var optionsInstance = new object();
            var executorInstance = new object();
            commandFactoryMock.Setup(x => x.Create(commandTypeMock.Object, optionsInstance, executorTypeMock.Object, executorInstance)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            app.RegisterCommand(commandTypeMock.Object, optionsInstance, executorTypeMock.Object, executorInstance);
        }

        [TestMethod]
        public void RegisterCommand_CommandType_ExecutorFunction()
        {
            var app = CreateApp(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var commandTypeMock = Mocks.Create<Type>(MockBehavior.Loose);
            var executorFunctionMock = Mocks.Create<Func<object, Task<int>>>();
            commandFactoryMock.Setup(x => x.Create(commandTypeMock.Object, executorFunctionMock.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            app.RegisterCommand(commandTypeMock.Object, executorFunctionMock.Object);
        }

        [TestMethod]
        public void RegisterCommand_CommandType_OptionsInstance_ExecutorFunction()
        {
            var app = CreateApp(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var commandTypeMock = Mocks.Create<Type>(MockBehavior.Loose);
            var optionsInstance = new object();
            var executorFunctionMock = Mocks.Create<Func<object, Task<int>>>();
            commandFactoryMock.Setup(x => x.Create(commandTypeMock.Object, optionsInstance, executorFunctionMock.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            app.RegisterCommand(commandTypeMock.Object, optionsInstance, executorFunctionMock.Object);
        }

        [TestMethod]
        public void RegisterCommand_TCommand_ExecutorFunction()
        {
            var app = CreateApp(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var executorFunctionMock = Mocks.Create<Func<DummyClass1, Task<int>>>();
            commandFactoryMock.Setup(x => x.Create(executorFunctionMock.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            app.RegisterCommand(executorFunctionMock.Object);
        }

        [TestMethod]
        public void RegisterCommand_TCommand_OptionsInstance_ExecutorFunction()
        {
            var app = CreateApp(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var optionsInstance = new DummyClass1();
            var executorFunctionMock = Mocks.Create<Func<DummyClass1, Task<int>>>();
            commandFactoryMock.Setup(x => x.Create(optionsInstance, executorFunctionMock.Object)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            app.RegisterCommand(optionsInstance, executorFunctionMock.Object);
        }

        [TestMethod]
        public void RegisterCommand_TCommand()
        {
            var app = CreateApp(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            commandFactoryMock.Setup(x => x.Create<DummyClass2>()).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            app.RegisterCommand<DummyClass2>();
        }

        [TestMethod]
        public void RegisterCommand_TCommand_OptionsInstance()
        {
            var app = CreateApp(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var optionsInstance = new DummyClass2();
            commandFactoryMock.Setup(x => x.Create(optionsInstance)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            app.RegisterCommand(optionsInstance);
        }

        [TestMethod]
        public void RegisterCommand_TCommand_TExecutor()
        {
            var app = CreateApp(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            commandFactoryMock.Setup(x => x.Create<DummyClass1, DummyClass3>()).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            app.RegisterCommand<DummyClass1, DummyClass3>();
        }

        [TestMethod]
        public void RegisterCommand_TCommand_TExecutor_ExecutorInstance()
        {
            var app = CreateApp(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var executorInstance = new DummyClass3();
            commandFactoryMock.Setup(x => x.Create<DummyClass1, DummyClass3>(executorInstance)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            app.RegisterCommand<DummyClass1, DummyClass3>(executorInstance);
        }

        [TestMethod]
        public void RegisterCommand_TCommand_TExecutor_OptionsInstance()
        {
            var app = CreateApp(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var optionsInstance = new DummyClass1();
            commandFactoryMock.Setup(x => x.Create<DummyClass1, DummyClass3>(optionsInstance)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            app.RegisterCommand<DummyClass1, DummyClass3>(optionsInstance);
        }

        [TestMethod]
        public void RegisterCommand_TCommand_TExecutor_OptionsInstance_ExecutorInstance()
        {
            var app = CreateApp(out var collectionMock, out var commandFactoryMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var optionsInstance = new DummyClass1();
            var executorInstance = new DummyClass3();
            commandFactoryMock.Setup(x => x.Create(optionsInstance, executorInstance)).Returns(commandMock.Object).Verifiable(Verifiables, Times.Once());
            collectionMock.Setup(x => x.Add(commandMock.Object)).Verifiable(Verifiables, Times.Once());

            app.RegisterCommand(optionsInstance, executorInstance);
        }

        [TestMethod]
        public async Task RunAsync_Success()
        {
            var appMock = Mocks.Create<CliAsyncApplication>();
            SetupAppMock(appMock);
            var args = new[] { "blub" };
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var options = new object();
            appMock.Protected()
                .Setup<bool>("TryParseArguments", args, ItExpr.Ref<ICliCommandInfo?>.IsAny, ItExpr.Ref<object?>.IsAny, ItExpr.Ref<int>.IsAny)
                .Returns(new CliApplicationBaseTests.TryParseArgumentsDelegate((string[] a, out ICliCommandInfo? c, out object? o, out int e) =>
                {
                    c = commandMock.Object;
                    o = options;
                    e = 0;
                    return true;
                }))
                .Verifiable(Verifiables, Times.Once());
            commandMock.Setup(x => x.ExecuteAsync(options)).Returns(Task.Delay(10).ContinueWith(t => 4711)).Verifiable(Verifiables, Times.Once());

            var result = await appMock.Object.RunAsync(args);

            Assert.AreEqual(4711, result);
        }

        [TestMethod]
        public async Task RunAsync_Fail()
        {
            var appMock = Mocks.Create<CliAsyncApplication>();
            SetupAppMock(appMock);
            var args = new[] { "blub" };
            appMock.Protected()
                .Setup<bool>("TryParseArguments", args, ItExpr.Ref<ICliCommandInfo?>.IsAny, ItExpr.Ref<object?>.IsAny, ItExpr.Ref<int>.IsAny)
                .Returns(new CliApplicationBaseTests.TryParseArgumentsDelegate((string[] a, out ICliCommandInfo? c, out object? o, out int e) =>
                {
                    c = null;
                    o = null;
                    e = 4711;
                    return false;
                }))
                .Verifiable(Verifiables, Times.Once());

            var result = await appMock.Object.RunAsync(args);

            Assert.AreEqual(4711, result);
        }

        private CliAsyncApplication CreateApp(out Mock<ICliCommandInfoCollection> collectionMock)
        {
            collectionMock = Mocks.Create<ICliCommandInfoCollection>();
            var po = new PrivateObject(typeof(CliAsyncApplication), null, collectionMock.Object);
            return (CliAsyncApplication)po.Target;
        }

        private CliAsyncApplication CreateApp(out Mock<ICliCommandInfoCollection> collectionMock, out Mock<ICliCommandInfoFactory> commandFactoryMock)
        {
            commandFactoryMock = Mocks.Create<ICliCommandInfoFactory>();
            var result = CreateApp(out collectionMock);
            result.CommandFactory = commandFactoryMock.Object;
            return result;
        }

        private static void SetupAppMock(Mock<CliAsyncApplication> appMock)
        {
            appMock.Protected().SetupGet<Type>("ExecutorType").CallBase();
            appMock.Protected().SetupGet<Type>("GenericExecutorType").CallBase();
        }

        public class DummyClass1
        {
        }

        public class DummyClass2 : ICliAsyncCommandExecutor
        {
            [ExcludeFromCodeCoverage]
            public Task<int> ExecuteCommandAsync(ICliCommandInfo command)
            {
                throw new NotImplementedException();
            }
        }

        public class DummyClass3 : ICliAsyncCommandExecutor<DummyClass1>
        {
            [ExcludeFromCodeCoverage]
            public Task<int> ExecuteCommandAsync(ICliCommandInfo command, DummyClass1 parameters)
            {
                throw new NotImplementedException();
            }
        }
    }
}
