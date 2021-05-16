using MaSch.Console.Cli.Configuration;
using MaSch.Console.Cli.Runtime;
using MaSch.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Reflection;
using System.Text;

namespace MaSch.Console.Cli.Test.Help
{
    [TestClass]
    public class CliHelpPageTests : TestClassBase
    {
        private static string NL => Environment.NewLine;

        private Mock<ICliApplicationBase> AppMock => Cache.GetValue(() => new Mock<ICliApplicationBase>(MockBehavior.Strict))!;
        private Mock<IConsoleService> ConsoleServiceMock => Cache.GetValue(() => new Mock<IConsoleService>(MockBehavior.Strict))!;
        private PrivateCliHelpPage HelpPage => Cache.GetValue(() => new PrivateCliHelpPage(ConsoleServiceMock.Object))!;

        [TestMethod]
        public void GetOptionName_NoShortAlias_OneAlias()
        {
            var optionInfo = CreateOptionInfo(new CliCommandOptionAttribute("blub"));

            var result = PrivateCliHelpPage.GetOptionName(optionInfo);

            Assert.AreEqual("--blub", result);
        }

        [TestMethod]
        public void GetOptionName_OneShortAlias_OneAlias()
        {
            var optionInfo = CreateOptionInfo(new CliCommandOptionAttribute('b', "blub"));

            var result = PrivateCliHelpPage.GetOptionName(optionInfo);

            Assert.AreEqual("-b, --blub", result);
        }

        [TestMethod]
        public void GetOptionName_MultipleAliases()
        {
            var optionInfo = CreateOptionInfo(new CliCommandOptionAttribute('b', "blub", 'B', "blub2"));

            var result = PrivateCliHelpPage.GetOptionName(optionInfo);

            Assert.AreEqual("-b, -B, --blub, --blub2", result);
        }

        [TestMethod]
        public void WriteCommandNameAndVersion()
        {
            AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { Name = "MyName", Version = "MyVersion" });
            var sb = AttachStringBuilder(ConsoleServiceMock);

            HelpPage.WriteCommandNameAndVersion(AppMock.Object, null!);

            Assert.AreEqual($"MyName MyVersion{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCopyright_FancyConsole()
        {
            AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { Year = "1337", Author = "Me" });
            ConsoleServiceMock.Setup(x => x.IsFancyConsole).Returns(true);
            var sb = AttachStringBuilder(ConsoleServiceMock);

            HelpPage.WriteCopyright(AppMock.Object, null!);

            Assert.AreEqual($"Copyright © 1337 Me{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCopyright_NonFancyConsole()
        {
            AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { Year = "1337", Author = "Me" });
            ConsoleServiceMock.Setup(x => x.IsFancyConsole).Returns(false);
            var sb = AttachStringBuilder(ConsoleServiceMock);

            HelpPage.WriteCopyright(AppMock.Object, null!);

            Assert.AreEqual($"Copyright (C) 1337 Me{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteErrorMessage_UnknownError()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);

            HelpPage.WriteErrorMessage(AppMock.Object, new CliError((CliErrorType)4711));

            Assert.AreEqual($"An unknown error occured.{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteErrorMessage_UnknownCommand()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);

            HelpPage.WriteErrorMessage(AppMock.Object, new CliError(CliErrorType.UnknownCommand) { CommandName = "MyCommandName" });

            Assert.AreEqual($"The command \"MyCommandName\" is unknown.{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteErrorMessage_UnknownOption()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);

            HelpPage.WriteErrorMessage(AppMock.Object, new CliError(CliErrorType.UnknownOption) { OptionName = "MyOptionName" });

            Assert.AreEqual($"The option \"MyOptionName\" is unknown.{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteErrorMessage_UnknownValue()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);

            HelpPage.WriteErrorMessage(AppMock.Object, new CliError(CliErrorType.UnknownValue));

            Assert.AreEqual($"Too many values given.{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteErrorMessage_MissingCommand()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);

            HelpPage.WriteErrorMessage(AppMock.Object, new CliError(CliErrorType.MissingCommand));

            Assert.AreEqual($"No command has been provided.{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteErrorMessage_MissingOption()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var cmd = CliCommandInfo.From<TestCommandOptions>();

            HelpPage.WriteErrorMessage(AppMock.Object, new CliError(CliErrorType.MissingOption, cmd, cmd.Options[0]));

            Assert.AreEqual($"The option --my-option is required.{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteErrorMessage_MissingValue()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);

            HelpPage.WriteErrorMessage(AppMock.Object, new CliError(CliErrorType.MissingValue));

            Assert.AreEqual($"One or more values for this command are missing.{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteErrorMessage_WrongOptionFormat()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var cmd = CliCommandInfo.From<TestCommandOptions>();

            HelpPage.WriteErrorMessage(AppMock.Object, new CliError(CliErrorType.WrongOptionFormat, cmd, cmd.Options[0]));

            Assert.AreEqual($"The value for option --my-option has the wrong format.{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteErrorMessage_WrongValueFormat()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var cmd = CliCommandInfo.From<TestCommandOptions>();

            HelpPage.WriteErrorMessage(AppMock.Object, new CliError(CliErrorType.WrongValueFormat, cmd, cmd.Values[0]));

            Assert.AreEqual($"The value MyValue has the wrong format.{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteErrorMessage_Custom()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);

            HelpPage.WriteErrorMessage(AppMock.Object, new CliError("My custom error message"));

            Assert.AreEqual($"My custom error message{NL}", sb.ToString());
        }

        private static StringBuilder AttachStringBuilder(Mock<IConsoleService> consoleServiceMock)
        {
            var result = new StringBuilder();
            consoleServiceMock.Setup(x => x.Write(It.IsAny<string?>())).Callback<string?>(s => result.Append(s));
            consoleServiceMock.Setup(x => x.WriteLine(It.IsAny<string?>())).Callback<string?>(s => result.AppendLine(s));
            consoleServiceMock.SetupGet(x => x.ForegroundColor).Returns(ConsoleColor.Gray);
            consoleServiceMock.SetupSet(x => x.ForegroundColor = It.IsAny<ConsoleColor>());
            consoleServiceMock.SetupGet(x => x.BackgroundColor).Returns(ConsoleColor.Black);
            consoleServiceMock.SetupSet(x => x.BackgroundColor = It.IsAny<ConsoleColor>());
            return result;
        }

        private static CliCommandOptionInfo CreateOptionInfo(CliCommandOptionAttribute option)
        {
            return new CliCommandOptionInfo(
                CliCommandInfo.From<TestCommandOptions>(),
                new Mock<PropertyInfo>(MockBehavior.Strict).Object,
                option);
        }

        [CliCommand("blub", Executable = false)]
        private class TestCommandOptions
        {
            [CliCommandOption("my-option")]
            public string? MyOption { get; set; }

            [CliCommandValue(0, "MyValue")]
            public string? MyValue { get; set; }
        }

        private class PrivateCliHelpPage
        {
            private static readonly PrivateType _pt = new(typeof(CliHelpPage));

            private readonly PrivateObject _po;

            public PrivateCliHelpPage(IConsoleService consoleService)
            {
                _po = new PrivateObject(new CliHelpPage(consoleService));
            }

            public void WriteVersionPage(ICliApplicationBase application, CliError error)
                => _po.Invoke(nameof(WriteVersionPage), application, error);

            public void WriteHelpPage(ICliApplicationBase application, CliError error)
                => _po.Invoke(nameof(WriteHelpPage), application, error);

            public void WriteErrorPage(ICliApplicationBase application, CliError error)
                => _po.Invoke(nameof(WriteErrorPage), application, error);

            public void WriteCommandNameAndVersion(ICliApplicationBase application, CliError error)
                => _po.Invoke(nameof(WriteCommandNameAndVersion), application, error);

            public void WriteCopyright(ICliApplicationBase application, CliError error)
                => _po.Invoke(nameof(WriteCopyright), application, error);

            public void WriteErrorMessage(ICliApplicationBase application, CliError error)
                => _po.Invoke(nameof(WriteErrorMessage), application, error);

            public void WriteCommandUsage(ICliApplicationBase application, CliError error)
                => _po.Invoke(nameof(WriteCommandUsage), application, error);

            public void WriteCommandParameters(ICliApplicationBase application, CliError error)
                => _po.Invoke(nameof(WriteCommandParameters), application, error);

            public void WriteCommands(ICliApplicationBase application, CliError error)
                => _po.Invoke(nameof(WriteCommands), application, error);

            public static string GetOptionName(CliCommandOptionInfo option)
                => (string)_pt.InvokeStatic(nameof(GetOptionName), option);
        }
    }
}
