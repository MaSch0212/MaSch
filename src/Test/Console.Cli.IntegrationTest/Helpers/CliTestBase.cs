using MaSch.Test;
using Moq;
using System;
using System.Text;

namespace MaSch.Console.Cli.IntegrationTest.Helpers
{
    public class CliTestBase : TestClassBase
    {
        protected StringBuilder ConsoleOutputBuilder => Cache.GetValue(() => new StringBuilder())!;
        protected Mock<IConsoleService> ConsoleServiceMock => Cache.GetValue(() => CreateConsoleServiceMock(ConsoleOutputBuilder))!;

        protected IConsoleService ConsoleService => ConsoleServiceMock.Object;
        protected string ConsoleOutput => ConsoleOutputBuilder.ToString();

        private Mock<IConsoleService> CreateConsoleServiceMock(StringBuilder consoleBuilder)
        {
            var consoleServiceMock = Mocks.Create<IConsoleService>();
            consoleServiceMock.Setup(x => x.Write(It.IsAny<string?>())).Callback<string?>(s => consoleBuilder.Append(s));
            consoleServiceMock.Setup(x => x.WriteLine(It.IsAny<string?>())).Callback<string?>(s => consoleBuilder.AppendLine(s));
            consoleServiceMock.SetupGet(x => x.ForegroundColor).Returns(ConsoleColor.Gray);
            consoleServiceMock.SetupSet(x => x.ForegroundColor = It.IsAny<ConsoleColor>());
            consoleServiceMock.SetupGet(x => x.BackgroundColor).Returns(ConsoleColor.Black);
            consoleServiceMock.SetupSet(x => x.BackgroundColor = It.IsAny<ConsoleColor>());
            return consoleServiceMock;
        }
    }
}
