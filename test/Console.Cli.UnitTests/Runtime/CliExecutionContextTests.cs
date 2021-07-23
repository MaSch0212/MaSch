using MaSch.Console.Cli.Runtime;
using MaSch.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MaSch.Console.Cli.UnitTests.Runtime
{
    [TestClass]
    public class CliExecutionContextTests : TestClassBase
    {
        [TestMethod]
        public void Ctor_NullChecks()
        {
            var spMock = Mocks.Create<IServiceProvider>();
            var commandMock = Mocks.Create<ICliCommandInfo>();

            Assert.ThrowsException<ArgumentNullException>(() => new CliExecutionContext(null!, commandMock.Object));
            Assert.ThrowsException<ArgumentNullException>(() => new CliExecutionContext(spMock.Object, null!));
        }

        [TestMethod]
        public void Ctor()
        {
            var spMock = Mocks.Create<IServiceProvider>();
            var commandMock = Mocks.Create<ICliCommandInfo>();

            var ctx = new CliExecutionContext(spMock.Object, commandMock.Object);

            Assert.AreSame(spMock.Object, ctx.ServiceProvider);
            Assert.AreSame(commandMock.Object, ctx.Command);
        }
    }
}
