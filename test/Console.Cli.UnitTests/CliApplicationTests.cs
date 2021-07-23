using MaSch.Console.Cli.Runtime;
using MaSch.Test;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;

namespace MaSch.Console.Cli.UnitTests
{
    [TestClass]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Base class tests")]
    public class CliApplicationBaseTests : TestClassBase
    {
        public delegate bool TryParseArgumentsDelegate(IServiceProvider serviceProvider, string[] args, [NotNullWhen(true)] out CliExecutionContext? context, [NotNullWhen(true)] out object? options, out int errorCode);

        [TestMethod]
        public void Ctor_Success()
        {
            var appMock = CreateAppMock(out _, out _, out _);

            var app = appMock.Object;

            Assert.IsNotNull(app);
            Assert.IsNotNull(app.Options);
        }

        [TestMethod]
        public void Ctor_NullChecks()
        {
            var appMock = CreateAppMock(out _, out _, out _);
            appMock.Protected().SetupGet<Type>("ExecutorType").Returns((Type?)null!);
            appMock.Protected().SetupGet<Type>("GenericExecutorType").Returns(typeof(ICliExecutor<>));

            var ex = Assert.ThrowsException<TargetInvocationException>(() => appMock.Object);
            Assert.IsInstanceOfType<ArgumentNullException>(ex.InnerException);

            appMock.Protected().SetupGet<Type>("ExecutorType").Returns(typeof(ICliExecutable));
            appMock.Protected().SetupGet<Type>("GenericExecutorType").Returns((Type?)null!);
            ex = Assert.ThrowsException<TargetInvocationException>(() => appMock.Object);
            Assert.IsInstanceOfType<ArgumentNullException>(ex.InnerException);
        }

        [TestMethod]
        public void Ctor_WrongGenericExecutorType()
        {
            var appMock = CreateAppMock(out _, out _, out _);
            appMock.Protected().SetupGet<Type>("ExecutorType").Returns(typeof(ICliExecutable));
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
            var appMock = CreateAppMock(out _, out var oMock, out _);
            appMock.Protected().SetupGet<Type>("ExecutorType").Returns(typeof(ICliExecutable));
            appMock.Protected().SetupGet<Type>("GenericExecutorType").Returns(typeof(ICliExecutor<>));

            var app = appMock.Object;

            Assert.IsNotNull(app);
            Assert.AreSame(oMock.Object, app.Options);
        }

        [TestMethod]
        public void Commands()
        {
            var appMock = CreateAppMock(out _, out _, out var commandsMock);
            var rcMock = Mocks.Create<IReadOnlyCliCommandInfoCollection>();
            commandsMock.Setup(x => x.AsReadOnly()).Returns(rcMock.Object).Verifiable(Verifiables, Times.Once());

            var commands1 = appMock.Object.Commands;
            Assert.AreSame(rcMock.Object, commands1);

            var commands2 = appMock.Object.Commands;
            Assert.AreSame(rcMock.Object, commands2);
        }

        [TestMethod]
        public void TryParseArguments_Success()
        {
            var appMock = CreateAppMockForParse(out var spMock, out _, out _, out var parserMock, out _);
            var args = new[] { "blub" };
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var execCtx = new CliExecutionContext(Mocks.Create<IServiceProvider>().Object, commandMock.Object);
            var optionsInstance = new object();
            parserMock
                .Setup(x => x.Parse(args))
                .Returns(new CliArgumentParserResult(execCtx, optionsInstance))
                .Verifiable(Verifiables, Times.Once());

            var result = appMock.Object.TryParseArguments(spMock.Object, args, out var context, out var options, out var errorCode);

            Assert.IsTrue(result);
            Assert.AreSame(execCtx, context);
            Assert.AreSame(optionsInstance, options);
            Assert.AreEqual(0, errorCode);
        }

        [TestMethod]
        public void TryParseArguments_Fail_HelpPage()
        {
            var appMock = CreateAppMockForParse(out var spMock, out _, out _, out var parserMock, out var helpPageMock);
            var errors = new[] { new CliError(CliErrorType.VersionRequested) };
            var args = new[] { "blub" };
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var optionsInstance = new object();
            parserMock
                .Setup(x => x.Parse(args))
                .Returns(new CliArgumentParserResult(errors))
                .Verifiable(Verifiables, Times.Once());
            helpPageMock
                .Setup(x => x.Write(errors))
                .Returns(true)
                .Verifiable(Verifiables, Times.Once());

            var result = appMock.Object.TryParseArguments(spMock.Object, args, out var command, out var options, out var errorCode);

            Assert.IsFalse(result);
            Assert.IsNull(command);
            Assert.IsNull(options);
            Assert.AreEqual(0, errorCode);
        }

        [TestMethod]
        public void TryParseArguments_Fail_ParseError()
        {
            var appMock = CreateAppMockForParse(out var spMock, out var oMock, out _, out var parserMock, out var helpPageMock);
            var errors = new[] { new CliError("My custom error") };
            var args = new[] { "blub" };
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var optionsInstance = new object();
            parserMock
                .Setup(x => x.Parse(args))
                .Returns(new CliArgumentParserResult(errors))
                .Verifiable(Verifiables, Times.Once());
            helpPageMock
                .Setup(x => x.Write(errors))
                .Returns(false)
                .Verifiable(Verifiables, Times.Once());
            oMock.Setup(x => x.ParseErrorExitCode).Returns(4711);

            var result = appMock.Object.TryParseArguments(spMock.Object, args, out var command, out var options, out var errorCode);

            Assert.IsFalse(result);
            Assert.IsNull(command);
            Assert.IsNull(options);
            Assert.AreEqual(4711, errorCode);
        }

        private Mock<CliApplicationBase> CreateAppMock(out Mock<IServiceProvider> serviceProviderMock, out Mock<ICliApplicationOptions> optionsMock, out Mock<ICliCommandInfoCollection> commandsMock)
            => CreateAppMock<CliApplicationBase>(out serviceProviderMock, out optionsMock, out commandsMock);

        private Mock<TestCliApplicationBase> CreateAppMockForParse(out Mock<IServiceProvider> serviceProviderMock, out Mock<ICliApplicationOptions> optionsMock, out Mock<ICliCommandInfoCollection> commandsMock, out Mock<ICliArgumentParser> parserMock, out Mock<ICliHelpPage> helpPageMock)
        {
            var appMock = CreateAppMock<TestCliApplicationBase>(out serviceProviderMock, out optionsMock, out commandsMock);
            parserMock = Mocks.Create<ICliArgumentParser>();
            helpPageMock = Mocks.Create<ICliHelpPage>();
            serviceProviderMock.Setup(x => x.GetService(typeof(ICliArgumentParser))).Returns(parserMock.Object);
            serviceProviderMock.Setup(x => x.GetService(typeof(ICliHelpPage))).Returns(helpPageMock.Object);
            return appMock;
        }

        private Mock<T> CreateAppMock<T>(out Mock<IServiceProvider> serviceProviderMock, out Mock<ICliApplicationOptions> optionsMock, out Mock<ICliCommandInfoCollection> commandsMock)
            where T : CliApplicationBase
        {
            serviceProviderMock = Mocks.Create<IServiceProvider>();
            optionsMock = Mocks.Create<ICliApplicationOptions>();
            commandsMock = Mocks.Create<ICliCommandInfoCollection>();
            var appMock = Mocks.Create<T>(serviceProviderMock.Object, optionsMock.Object, commandsMock.Object);
            appMock.Protected().SetupGet<Type>("ExecutorType").Returns(typeof(ICliExecutable));
            appMock.Protected().SetupGet<Type>("GenericExecutorType").Returns(typeof(ICliExecutor<>));
            return appMock;
        }

        public abstract class TestCliApplicationBase : CliApplicationBase
        {
            protected TestCliApplicationBase(IServiceProvider serviceProvider, ICliApplicationOptions options, ICliCommandInfoCollection commandsCollection)
                : base(serviceProvider, options, commandsCollection)
            {
            }

            public new bool TryParseArguments(IServiceProvider serviceProvider, string[] args, [NotNullWhen(true)] out CliExecutionContext? context, [NotNullWhen(true)] out object? options, out int errorCode)
                => base.TryParseArguments(serviceProvider, args, out context, out options, out errorCode);
        }
    }

    [TestClass]
    public class CliApplicationTests : TestClassBase
    {
        [TestMethod]
        public void Run_Success()
        {
            var appMock = CreateAppMock(out var spMock, out _, out _, out var scopeMock, out var sspMock);
            scopeMock.Setup(x => x.Dispose()).Verifiable(Verifiables, Times.Once());
            var args = new[] { "blub" };
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var execCtx = new CliExecutionContext(spMock.Object, commandMock.Object);
            var options = new object();
            appMock.Protected()
                .Setup<bool>("TryParseArguments", sspMock.Object, args, ItExpr.Ref<CliExecutionContext?>.IsAny, ItExpr.Ref<object?>.IsAny, ItExpr.Ref<int>.IsAny)
                .Returns(new CliApplicationBaseTests.TryParseArgumentsDelegate((IServiceProvider serviceProvider, string[] a, out CliExecutionContext? c, out object? o, out int e) =>
                {
                    c = execCtx;
                    o = options;
                    e = 0;
                    return true;
                }))
                .Verifiable(Verifiables, Times.Once());
            commandMock.Setup(x => x.Execute(execCtx, options)).Returns(4711).Verifiable(Verifiables, Times.Once());

            var result = appMock.Object.Run(args);

            Assert.AreEqual(4711, result);
        }

        [TestMethod]
        public void Run_Fail()
        {
            var appMock = CreateAppMock(out var spMock, out _, out _, out var scopeMock, out var sspMock);
            scopeMock.Setup(x => x.Dispose()).Verifiable(Verifiables, Times.Once());
            var args = new[] { "blub" };
            appMock.Protected()
                .Setup<bool>("TryParseArguments", sspMock.Object, args, ItExpr.Ref<CliExecutionContext?>.IsAny, ItExpr.Ref<object?>.IsAny, ItExpr.Ref<int>.IsAny)
                .Returns(new CliApplicationBaseTests.TryParseArgumentsDelegate((IServiceProvider serviceProvider, string[] a, out CliExecutionContext? c, out object? o, out int e) =>
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

        private Mock<CliApplication> CreateAppMock(
            out Mock<IServiceProvider> serviceProviderMock,
            out Mock<CliApplicationOptions> optionsMock,
            out Mock<ICliCommandInfoCollection> commandsMock,
            out Mock<IServiceScope> serviceScopeMock,
            out Mock<IServiceProvider> scopedServiceProviderMock)
        {
            serviceProviderMock = Mocks.Create<IServiceProvider>();
            optionsMock = Mocks.Create<CliApplicationOptions>(MockBehavior.Loose);
            optionsMock.CallBase = true;
            commandsMock = Mocks.Create<ICliCommandInfoCollection>();
            scopedServiceProviderMock = Mocks.Create<IServiceProvider>();
            serviceScopeMock = Mocks.Create<IServiceScope>();
            var scopeFactoryMock = Mocks.Create<IServiceScopeFactory>();
            var appMock = Mocks.Create<CliApplication>(serviceProviderMock.Object, optionsMock.Object, commandsMock.Object);
            appMock.Protected().SetupGet<Type>("ExecutorType").CallBase();
            appMock.Protected().SetupGet<Type>("GenericExecutorType").CallBase();
            serviceProviderMock.Setup(x => x.GetService(typeof(IServiceScopeFactory))).Returns(scopeFactoryMock.Object);
            scopeFactoryMock.Setup(x => x.CreateScope()).Returns(serviceScopeMock.Object);
            serviceScopeMock.Setup(x => x.ServiceProvider).Returns(scopedServiceProviderMock.Object);
            return appMock;
        }
    }

    [TestClass]
    public class CliAsyncApplicationTests : TestClassBase
    {
        [TestMethod]
        public async Task RunAsync_Success()
        {
            var appMock = CreateAppMock(out var spMock, out _, out _, out var scopeMock, out var sspMock);
            scopeMock.Setup(x => x.Dispose()).Verifiable(Verifiables, Times.Once());
            var args = new[] { "blub" };
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var execCtx = new CliExecutionContext(spMock.Object, commandMock.Object);
            var options = new object();
            appMock.Protected()
                .Setup<bool>("TryParseArguments", sspMock.Object, args, ItExpr.Ref<CliExecutionContext?>.IsAny, ItExpr.Ref<object?>.IsAny, ItExpr.Ref<int>.IsAny)
                .Returns(new CliApplicationBaseTests.TryParseArgumentsDelegate((IServiceProvider serviceProvider, string[] a, out CliExecutionContext? c, out object? o, out int e) =>
                {
                    c = execCtx;
                    o = options;
                    e = 0;
                    return true;
                }))
                .Verifiable(Verifiables, Times.Once());
            commandMock.Setup(x => x.ExecuteAsync(execCtx, options)).Returns(Task.Delay(10).ContinueWith(t => 4711)).Verifiable(Verifiables, Times.Once());

            var result = await appMock.Object.RunAsync(args);

            Assert.AreEqual(4711, result);
        }

        [TestMethod]
        public async Task RunAsync_Fail()
        {
            var appMock = CreateAppMock(out var spMock, out _, out _, out var scopeMock, out var sspMock);
            scopeMock.Setup(x => x.Dispose()).Verifiable(Verifiables, Times.Once());
            var args = new[] { "blub" };
            appMock.Protected()
                .Setup<bool>("TryParseArguments", sspMock.Object, args, ItExpr.Ref<CliExecutionContext?>.IsAny, ItExpr.Ref<object?>.IsAny, ItExpr.Ref<int>.IsAny)
                .Returns(new CliApplicationBaseTests.TryParseArgumentsDelegate((IServiceProvider serviceProvider, string[] a, out CliExecutionContext? c, out object? o, out int e) =>
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

        private Mock<CliAsyncApplication> CreateAppMock(
            out Mock<IServiceProvider> serviceProviderMock,
            out Mock<CliApplicationOptions> optionsMock,
            out Mock<ICliCommandInfoCollection> commandsMock,
            out Mock<IServiceScope> serviceScopeMock,
            out Mock<IServiceProvider> scopedServiceProviderMock)
        {
            serviceProviderMock = Mocks.Create<IServiceProvider>();
            optionsMock = Mocks.Create<CliApplicationOptions>(MockBehavior.Loose);
            optionsMock.CallBase = true;
            commandsMock = Mocks.Create<ICliCommandInfoCollection>();
            scopedServiceProviderMock = Mocks.Create<IServiceProvider>();
            serviceScopeMock = Mocks.Create<IServiceScope>();
            var scopeFactoryMock = Mocks.Create<IServiceScopeFactory>();
            var appMock = Mocks.Create<CliAsyncApplication>(serviceProviderMock.Object, optionsMock.Object, commandsMock.Object);
            appMock.Protected().SetupGet<Type>("ExecutorType").CallBase();
            appMock.Protected().SetupGet<Type>("GenericExecutorType").CallBase();
            serviceProviderMock.Setup(x => x.GetService(typeof(IServiceScopeFactory))).Returns(scopeFactoryMock.Object);
            scopeFactoryMock.Setup(x => x.CreateScope()).Returns(serviceScopeMock.Object);
            serviceScopeMock.Setup(x => x.ServiceProvider).Returns(scopedServiceProviderMock.Object);
            return appMock;
        }
    }
}
