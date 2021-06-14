using MaSch.Console.Cli.Runtime;
using MaSch.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MaSch.Console.Cli.Test.Runtime
{
    [TestClass]
    public class CliExecutionContextTests : TestClassBase
    {
        [TestMethod]
        public void Ctor_NullChecks()
        {
            var appMock = Mocks.Create<ICliApplicationBase>();
            var commandMock = Mocks.Create<ICliCommandInfo>();

            Assert.ThrowsException<ArgumentNullException>(() => new CliExecutionContext(null!, commandMock.Object));
            Assert.ThrowsException<ArgumentNullException>(() => new CliExecutionContext(appMock.Object, null!));
        }

        [TestMethod]
        public void Ctor()
        {
            var appMock = Mocks.Create<ICliApplicationBase>();
            var consoleMock = Mocks.Create<IConsoleService>();
            var commandMock = Mocks.Create<ICliCommandInfo>();
            appMock.Setup(x => x.Options).Returns(new CliApplicationOptions { ConsoleService = consoleMock.Object });

            var ctx = new CliExecutionContext(appMock.Object, commandMock.Object);

            Assert.AreSame(appMock.Object, ctx.Application);
            Assert.AreSame(commandMock.Object, ctx.Command);
            Assert.AreSame(consoleMock.Object, ctx.Console);
        }
    }
}
