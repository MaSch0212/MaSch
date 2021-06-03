using MaSch.Console.Cli.Runtime;
using MaSch.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace MaSch.Console.Cli.Test
{
    [TestClass]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Base class tests")]
    public class CliApplicationBaseTests : TestClassBase
    {
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

        private Mock<CliApplicationBase> CreateAppMock()
        {
            var appMock = Mocks.Create<CliApplicationBase>();
            appMock.Protected().SetupGet<Type>("ExecutorType").Returns(typeof(ICliCommandExecutor));
            appMock.Protected().SetupGet<Type>("GenericExecutorType").Returns(typeof(ICliCommandExecutor<>));
            return appMock;
        }
    }

    [TestClass]
    public class CliApplicationTests : TestClassBase
    {
    }

    [TestClass]
    public class CliAsyncApplicationTests : TestClassBase
    {
    }
}
