using MaSch.Console.Cli.Configuration;
using MaSch.Console.Cli.Runtime;
using MaSch.Core;
using MaSch.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace MaSch.Console.Cli.UnitTests
{
    [TestClass]
    public class CliHelpPageTests : TestClassBase
    {
        private static string NL => Environment.NewLine;

        private Mock<ICliApplicationBase> AppMock => Cache.GetValue(CreateAppMock)!;
        private Mock<IConsoleService> ConsoleServiceMock => Cache.GetValue(() => new Mock<IConsoleService>(MockBehavior.Strict))!;
        private PrivateCliHelpPage HelpPage => Cache.GetValue(() => new PrivateCliHelpPage(AppMock.Object, ConsoleServiceMock.Object))!;
        private ICliCommandFactory Factory => Cache.GetValue(() => new CliCommandFactory())!;

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
            _ = AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { Name = "MyName", Version = "MyVersion" });
            var sb = AttachStringBuilder(ConsoleServiceMock);

            HelpPage.WriteCommandNameAndVersion(new CliError("blub"));

            Assert.AreEqual($"MyName MyVersion{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCommandNameAndVersion_CommandSpecific()
        {
            var commandMock = Mocks.Create<ICliCommandInfo>();
            _ = commandMock.Setup(x => x.ParserOptions).Returns(new CliParserOptions { Name = "YourName", Version = "YourVersion" });
            _ = AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { Name = "MyName", Version = "MyVersion" });
            var sb = AttachStringBuilder(ConsoleServiceMock);

            HelpPage.WriteCommandNameAndVersion(new CliError("blub", commandMock.Object));

            Assert.AreEqual($"YourName YourVersion{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCopyright_FancyConsole()
        {
            _ = AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { Year = "1337", Author = "Me" });
            _ = ConsoleServiceMock.Setup(x => x.IsFancyConsole).Returns(true);
            var sb = AttachStringBuilder(ConsoleServiceMock);

            HelpPage.WriteCopyright(new CliError("blub"));

            Assert.AreEqual($"Copyright © 1337 Me{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCopyright_NonFancyConsole()
        {
            _ = AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { Year = "1337", Author = "Me" });
            _ = ConsoleServiceMock.Setup(x => x.IsFancyConsole).Returns(false);
            var sb = AttachStringBuilder(ConsoleServiceMock);

            HelpPage.WriteCopyright(new CliError("blub"));

            Assert.AreEqual($"Copyright (C) 1337 Me{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCopyright_CommandSpecific()
        {
            var commandMock = CreateCommand(null, "blub");
            _ = commandMock.Setup(x => x.ParserOptions).Returns(new CliParserOptions { Year = "4711", Author = "You" });
            _ = AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { Year = "1337", Author = "Me" });
            _ = ConsoleServiceMock.Setup(x => x.IsFancyConsole).Returns(true);
            var sb = AttachStringBuilder(ConsoleServiceMock);

            HelpPage.WriteCopyright(new CliError("blub", commandMock.Object));

            Assert.AreEqual($"Copyright © 4711 You{NL}", sb.ToString());
        }

        [TestMethod]
        [DataRow(false, DisplayName = "Without Exception")]
        [DataRow(true, DisplayName = "With Exception")]
        public void WriteErrorMessage_UnknownError(bool addException)
        {
            TestWriteErrorMessgae(
                () => new CliError((CliErrorType)4711),
                "An unknown error occured.",
                addException);
        }

        [TestMethod]
        [DataRow(false, DisplayName = "Without Exception")]
        [DataRow(true, DisplayName = "With Exception")]
        public void WriteErrorMessage_UnknownCommand(bool addException)
        {
            TestWriteErrorMessgae(
                () => new CliError(CliErrorType.UnknownCommand) { CommandName = "MyCommandName" },
                "The command \"MyCommandName\" is unknown.",
                addException);
        }

        [TestMethod]
        [DataRow(false, DisplayName = "Without Exception")]
        [DataRow(true, DisplayName = "With Exception")]
        public void WriteErrorMessage_UnknownOption(bool addException)
        {
            TestWriteErrorMessgae(
                () => new CliError(CliErrorType.UnknownOption) { OptionName = "MyOptionName" },
                "The option \"MyOptionName\" is unknown.",
                addException);
        }

        [TestMethod]
        [DataRow(false, DisplayName = "Without Exception")]
        [DataRow(true, DisplayName = "With Exception")]
        public void WriteErrorMessage_UnknownValue(bool addException)
        {
            TestWriteErrorMessgae(
                () => new CliError(CliErrorType.UnknownValue),
                "Too many values given.",
                addException);
        }

        [TestMethod]
        [DataRow(false, DisplayName = "Without Exception")]
        [DataRow(true, DisplayName = "With Exception")]
        public void WriteErrorMessage_MissingCommand(bool addException)
        {
            TestWriteErrorMessgae(
                () => new CliError(CliErrorType.MissingCommand),
                "No command has been provided.",
                addException);
        }

        [TestMethod]
        [DataRow(false, DisplayName = "Without Exception")]
        [DataRow(true, DisplayName = "With Exception")]
        public void WriteErrorMessage_MissingOption(bool addException)
        {
            var cmd = Factory.Create<TestCommandOptions>();
            TestWriteErrorMessgae(
                () => new CliError(CliErrorType.MissingOption, cmd, cmd.Options[0]),
                "The option --my-option is required.",
                addException);
        }

        [TestMethod]
        [DataRow(false, DisplayName = "Without Exception")]
        [DataRow(true, DisplayName = "With Exception")]
        public void WriteErrorMessage_MissingOptionValue(bool addException)
        {
            var cmd = Factory.Create<TestCommandOptions>();
            TestWriteErrorMessgae(
                () => new CliError(CliErrorType.MissingOptionValue, cmd, cmd.Options[0]),
                "A value needs to be provided for option --my-option.",
                addException);
        }

        [TestMethod]
        [DataRow(false, DisplayName = "Without Exception")]
        [DataRow(true, DisplayName = "With Exception")]
        public void WriteErrorMessage_MissingValue(bool addException)
        {
            TestWriteErrorMessgae(
                () => new CliError(CliErrorType.MissingValue),
                "One or more values for this command are missing.",
                addException);
        }

        [TestMethod]
        [DataRow(false, DisplayName = "Without Exception")]
        [DataRow(true, DisplayName = "With Exception")]
        public void WriteErrorMessage_WrongOptionFormat(bool addException)
        {
            var cmd = Factory.Create<TestCommandOptions>();
            TestWriteErrorMessgae(
                () => new CliError(CliErrorType.WrongOptionFormat, cmd, cmd.Options[0]),
                "The value for option --my-option has the wrong format.",
                addException);
        }

        [TestMethod]
        [DataRow(false, DisplayName = "Without Exception")]
        [DataRow(true, DisplayName = "With Exception")]
        public void WriteErrorMessage_WrongValueFormat(bool addException)
        {
            var cmd = Factory.Create<TestCommandOptions>();
            TestWriteErrorMessgae(
                () => new CliError(CliErrorType.WrongValueFormat, cmd, cmd.Values[0]),
                "The value MyValue has the wrong format.",
                addException);
        }

        [TestMethod]
        [DataRow(false, DisplayName = "Without Exception")]
        [DataRow(true, DisplayName = "With Exception")]
        public void WriteErrorMessage_Custom(bool addException)
        {
            TestWriteErrorMessgae(() => new CliError("My custom error message"), "My custom error message", addException);
        }

        [TestMethod]
        public void WriteVersionPage()
        {
            var helpPageMock = Mocks.Create<CliHelpPage>(AppMock.Object, ConsoleServiceMock.Object);
            var errors = new[] { new CliError(CliErrorType.VersionRequested) };
            var callOrder = new List<string>();
            _ = helpPageMock.Protected().Setup("WriteVersionPage", (IList<CliError>)errors).CallBase();
            _ = helpPageMock.Protected().Setup("WriteCommandNameAndVersion", errors[0]).Callback(() => callOrder.Add("CommandNameAndVersion"));
            _ = helpPageMock.Protected().Setup("WriteCopyright", errors[0]).Callback(() => callOrder.Add("Copyright"));
            _ = helpPageMock.Protected().Setup("WriteCommandVersions", errors[0]).Callback(() => callOrder.Add("CommandVersions"));
            _ = ConsoleServiceMock.Setup(x => x.WriteLine(string.Empty)).Callback(() => callOrder.Add(string.Empty));

            new PrivateCliHelpPage(helpPageMock.Object).WriteVersionPage(errors);

            Assert.AreCollectionsEqual(new[] { "CommandNameAndVersion", "Copyright", "CommandVersions", string.Empty }, callOrder);
        }

        [TestMethod]
        public void WriteHelpPage()
        {
            var helpPageMock = Mocks.Create<CliHelpPage>(AppMock.Object, ConsoleServiceMock.Object);
            var errors = new[] { new CliError(CliErrorType.HelpRequested) };
            var callOrder = new List<string>();
            _ = helpPageMock.Protected().Setup("WriteHelpPage", (IList<CliError>)errors).CallBase();
            _ = helpPageMock.Protected().Setup("WriteCommandNameAndVersion", errors[0]).Callback(() => callOrder.Add("CommandNameAndVersion"));
            _ = helpPageMock.Protected().Setup("WriteCopyright", errors[0]).Callback(() => callOrder.Add("Copyright"));
            _ = helpPageMock.Protected().Setup("WriteCommandUsage", errors[0]).Callback(() => callOrder.Add("CommandUsage"));
            _ = helpPageMock.Protected().Setup("WriteCommandParameters", errors[0]).Callback(() => callOrder.Add("CommandParameters"));
            _ = helpPageMock.Protected().Setup("WriteCommands", errors[0]).Callback(() => callOrder.Add("Commands"));
            _ = ConsoleServiceMock.Setup(x => x.WriteLine(string.Empty)).Callback(() => callOrder.Add(string.Empty));

            new PrivateCliHelpPage(helpPageMock.Object).WriteHelpPage(errors);

            Assert.AreCollectionsEqual(
                new[]
                {
                    "CommandNameAndVersion",
                    "Copyright",
                    string.Empty,
                    "CommandUsage",
                    "CommandParameters",
                    "Commands",
                    string.Empty,
                },
                callOrder);
        }

        [TestMethod]
        public void WriteHelpPage_WithErrors()
        {
            var helpPageMock = Mocks.Create<CliHelpPage>(AppMock.Object, ConsoleServiceMock.Object);
            var errors = new[] { new CliError("Blub1"), new CliError("Blub2"), new CliError("Blub3") };
            var callOrder = new List<string>();
            _ = helpPageMock.Protected().Setup("WriteHelpPage", (IList<CliError>)errors).CallBase();
            _ = helpPageMock.Protected().Setup("WriteCommandNameAndVersion", errors[0]).Callback(() => callOrder.Add("CommandNameAndVersion"));
            _ = helpPageMock.Protected().Setup("WriteCopyright", errors[0]).Callback(() => callOrder.Add("Copyright"));
            _ = helpPageMock.Protected().Setup("WriteCommandUsage", errors[0]).Callback(() => callOrder.Add("CommandUsage"));
            _ = helpPageMock.Protected().Setup("WriteCommandParameters", errors[0]).Callback(() => callOrder.Add("CommandParameters"));
            _ = helpPageMock.Protected().Setup("WriteCommands", errors[0]).Callback(() => callOrder.Add("Commands"));
            _ = helpPageMock.Protected().Setup("WriteErrorMessage", ItExpr.IsAny<CliError>()).Callback<CliError>(e => callOrder.Add($"ErrorMessage_{e.CustomErrorMessage}"));
            _ = ConsoleServiceMock.Setup(x => x.WriteLine(string.Empty)).Callback(() => callOrder.Add(string.Empty));

            new PrivateCliHelpPage(helpPageMock.Object).WriteHelpPage(errors);

            Assert.AreCollectionsEqual(
                new[]
                {
                    "CommandNameAndVersion",
                    "Copyright",
                    string.Empty,
                    "ErrorMessage_Blub1",
                    "ErrorMessage_Blub2",
                    "ErrorMessage_Blub3",
                    string.Empty,
                    "CommandUsage",
                    "CommandParameters",
                    "Commands",
                    string.Empty,
                },
                callOrder);
        }

        [TestMethod]
        public void Write_NullErrors()
        {
            var helpPageMock = Mocks.Create<CliHelpPage>(AppMock.Object, ConsoleServiceMock.Object);
            IList<CliError>? errors = null;
            _ = helpPageMock.Setup(x => x.Write(It.IsAny<IEnumerable<CliError>>())).CallBase();
            _ = helpPageMock.Protected().Setup("WriteHelpPage", ItExpr.IsAny<IList<CliError>>()).Callback<IList<CliError>>(e => errors = e);

            var result = helpPageMock.Object.Write(null);

            Assert.IsFalse(result);
            Assert.IsNotNull(errors);
            Assert.AreCollectionsEqual(new[] { CliErrorType.Unknown }, errors.Select(x => x.Type));
        }

        [TestMethod]
        public void Write_NoErrors()
        {
            var helpPageMock = Mocks.Create<CliHelpPage>(AppMock.Object, ConsoleServiceMock.Object);
            IList<CliError>? errors = null;
            _ = helpPageMock.Setup(x => x.Write(It.IsAny<IEnumerable<CliError>>())).CallBase();
            _ = helpPageMock.Protected().Setup("WriteHelpPage", ItExpr.IsAny<IList<CliError>>()).Callback<IList<CliError>>(e => errors = e);

            var result = helpPageMock.Object.Write(Array.Empty<CliError>());

            Assert.IsFalse(result);
            Assert.IsNotNull(errors);
            Assert.AreCollectionsEqual(new[] { CliErrorType.Unknown }, errors.Select(x => x.Type));
        }

        [TestMethod]
        public void Write_WithNullError()
        {
            var helpPageMock = Mocks.Create<CliHelpPage>(AppMock.Object, ConsoleServiceMock.Object);
            IList<CliError>? errors = null;
            _ = helpPageMock.Setup(x => x.Write(It.IsAny<IEnumerable<CliError>>())).CallBase();
            _ = helpPageMock.Protected().Setup("WriteHelpPage", ItExpr.IsAny<IList<CliError>>()).Callback<IList<CliError>>(e => errors = e);

            var result = helpPageMock.Object.Write(new[] { (CliError)null! });

            Assert.IsFalse(result);
            Assert.IsNotNull(errors);
            Assert.AreCollectionsEqual(new[] { CliErrorType.Unknown }, errors.Select(x => x.Type));
        }

        [TestMethod]
        public void Write_HasVersionRequested()
        {
            var helpPageMock = Mocks.Create<CliHelpPage>(AppMock.Object, ConsoleServiceMock.Object);
            var errors = new[] { new CliError("gjklhf"), new CliError(CliErrorType.VersionRequested) };
            _ = helpPageMock.Setup(x => x.Write(errors)).CallBase();
            _ = helpPageMock.Protected().Setup("WriteVersionPage", (IList<CliError>)new[] { errors[1], errors[0] }).Verifiable(Verifiables, Times.Once());

            var result = helpPageMock.Object.Write(errors);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Write_HelpRequested()
        {
            var helpPageMock = Mocks.Create<CliHelpPage>(AppMock.Object, ConsoleServiceMock.Object);
            var errors = new[] { new CliError("gjklhf"), new CliError(CliErrorType.VersionRequested), new CliError(CliErrorType.HelpRequested) };
            _ = helpPageMock.Setup(x => x.Write(errors)).CallBase();
            _ = helpPageMock.Protected().Setup("WriteHelpPage", (IList<CliError>)new[] { errors[2], errors[0] }).Verifiable(Verifiables, Times.Once());

            var result = helpPageMock.Object.Write(errors);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Write_OtherErrors()
        {
            var helpPageMock = Mocks.Create<CliHelpPage>(AppMock.Object, ConsoleServiceMock.Object);
            var errors = new[] { new CliError("gjklhf") };
            _ = helpPageMock.Setup(x => x.Write(errors)).CallBase();
            _ = helpPageMock.Protected().Setup("WriteHelpPage", (IList<CliError>)errors).Verifiable(Verifiables, Times.Once());

            var result = helpPageMock.Object.Write(errors);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void WriteCommandUsage_NoAffectedCommand_NoCommands()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var collectionMock = Mocks.Create<IReadOnlyCliCommandInfoCollection>();
            _ = collectionMock.Setup(x => x.GetRootCommands()).Returns(Array.Empty<ICliCommandInfo>());
            _ = AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { CliName = "MyProgram" });
            _ = AppMock.Setup(x => x.Commands).Returns(collectionMock.Object);
            _ = ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));

            HelpPage.WriteCommandUsage(new CliError("MyError"));

            Assert.AreEqual($"Usage: MyProgram{new string(' ', 50 - 16 - 1)}{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCommandUsage_NoAffectedCommand_WithCommands()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var collectionMock = Mocks.Create<IReadOnlyCliCommandInfoCollection>();
            var command1Mock = CreateCommand(null, "command1");
            var command2Mock = CreateCommand(null, "command2");
            _ = collectionMock.Setup(x => x.GetRootCommands()).Returns(new[] { command1Mock.Object, command2Mock.Object });
            _ = AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { CliName = "MyProgram" });
            _ = AppMock.Setup(x => x.Commands).Returns(collectionMock.Object);
            _ = ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));

            HelpPage.WriteCommandUsage(new CliError("MyError"));

            Assert.AreEqual($"Usage: MyProgram <command>{new string(' ', 50 - 26 - 1)}{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCommandUsage_WithAffectedCommand_OneAlias_NoParentCommand_NoChildCommands_NoOptions_NoValues()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var commandMock = CreateCommand(null, "blub");
            _ = AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { CliName = "MyProgram" });
            _ = ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));

            HelpPage.WriteCommandUsage(new CliError("MyError", commandMock.Object));

            Assert.AreEqual($"Usage: MyProgram blub{new string(' ', 50 - 21 - 1)}{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCommandUsage_WithAffectedCommand_MultipleAliases_NoParentCommand_NoChildCommands_NoOptions_NoValues()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var commandMock = CreateCommand(null, "blub", "blib", "blab");
            _ = AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { CliName = "MyProgram" });
            _ = ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));

            HelpPage.WriteCommandUsage(new CliError("MyError", commandMock.Object));

            Assert.AreEqual($"Usage: MyProgram (blub|blib|blab){new string(' ', 50 - 33 - 1)}{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCommandUsage_WithAffectedCommand_OneAlias_WithParentCommand_NoChildCommands_NoOptions_NoValues()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var greatParentCommandMock = CreateCommand(null, "pp", "pi");
            var parentCommandMock = CreateCommand(greatParentCommandMock, "p");
            var commandMock = CreateCommand(parentCommandMock, "blub");
            _ = AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { CliName = "MyProgram" });
            _ = ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));

            HelpPage.WriteCommandUsage(new CliError("MyError", commandMock.Object));

            Assert.AreEqual($"Usage: MyProgram (pp|pi) p blub{new string(' ', 50 - 31 - 1)}{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCommandUsage_WithAffectedCommand_OneAlias_NoParentCommand_Executable_WithChildCommands_NoOptions_NoValues()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var commandMock = CreateCommand(null, "blub");
            var childCommand1Mock = CreateCommand(commandMock, "command1");
            var childCommand2Mock = CreateCommand(commandMock, "command2");
            _ = AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { CliName = "MyProgram" });
            _ = ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));

            HelpPage.WriteCommandUsage(new CliError("MyError", commandMock.Object));

            Assert.AreEqual($"Usage: MyProgram blub [command]{new string(' ', 50 - 31 - 1)}{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCommandUsage_WithAffectedCommand_OneAlias_NoParentCommand_NonExecutable_WithChildCommands_NoOptions_NoValues()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var commandMock = CreateCommand(null, "blub");
            var childCommand1Mock = CreateCommand(commandMock, "command1");
            var childCommand2Mock = CreateCommand(commandMock, "command2");
            _ = commandMock.Setup(x => x.IsExecutable).Returns(false);
            _ = AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { CliName = "MyProgram" });
            _ = ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));

            HelpPage.WriteCommandUsage(new CliError("MyError", commandMock.Object));

            Assert.AreEqual($"Usage: MyProgram blub <command>{new string(' ', 50 - 31 - 1)}{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCommandUsage_WithAffectedCommand_OneAlias_NoParentCommand_NoChildCommands_WithOptions_NoValues()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var commandMock = CreateCommand(null, "blub");
            var option1Mock = CreateOption(commandMock, null, null);
            var option2Mock = CreateOption(commandMock, null, null);
            _ = AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { CliName = "MyProgram" });
            _ = ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));

            HelpPage.WriteCommandUsage(new CliError("MyError", commandMock.Object));

            Assert.AreEqual($"Usage: MyProgram blub [options]{new string(' ', 50 - 31 - 1)}{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCommandUsage_WithAffectedCommand_OneAlias_NoParentCommand_NoChildCommands_NoOptions_WithValues()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var commandMock = CreateCommand(null, "blub");
            var value1Mock = CreateValue(commandMock, 2, "Val1");
            var value2Mock = CreateValue(commandMock, 1, "Val2");
            _ = value2Mock.Setup(x => x.IsRequired).Returns(true);
            _ = AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { CliName = "MyProgram" });
            _ = ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));

            HelpPage.WriteCommandUsage(new CliError("MyError", commandMock.Object));

            Assert.AreEqual($"Usage: MyProgram blub <Val2> [Val1]{new string(' ', 50 - 35 - 1)}{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCommandUsage_Everything()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var greatParentCommandMock = CreateCommand(null, "grandparent", "supervisor");
            var parentCommandMock = CreateCommand(greatParentCommandMock, "parent");
            var commandMock = CreateCommand(parentCommandMock, "blub", "blib", "blab");
            var childCommand1Mock = CreateCommand(commandMock, "command1");
            var childCommand2Mock = CreateCommand(commandMock, "command2");
            var value1Mock = CreateValue(commandMock, 2, "Val1");
            var value2Mock = CreateValue(commandMock, 1, "Val2");
            var option1Mock = CreateOption(commandMock, null, null);
            var option2Mock = CreateOption(commandMock, null, null);
            _ = value2Mock.Setup(x => x.IsRequired).Returns(true);
            _ = value2Mock.Setup(x => x.PropertyType).Returns(typeof(List<object>));
            _ = option2Mock.Setup(x => x.Hidden).Returns(true);
            _ = AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { CliName = "MyProgram" });
            _ = ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));

            HelpPage.WriteCommandUsage(new CliError("MyError", commandMock.Object));

            Assert.AreEqual(
                $"Usage: MyProgram (grandparent|supervisor) parent {NL}" +
                $"       (blub|blib|blab) [command] <Val2          {NL}" +
                $"       [Val2]...> [Val1] [options]               {NL}",
                sb.ToString());
        }

        [TestMethod]
        public void WriteCommandParameters_NoAffectedCommand()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);

            HelpPage.WriteCommandParameters(new CliError("MyError"));

            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [TestMethod]
        public void WriteCommandParameters_NoValues_NoOptions()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var commandMock = CreateCommand(null, "blub");

            HelpPage.WriteCommandParameters(new CliError("MyError", commandMock.Object));

            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [TestMethod]
        public void WriteCommandParameters_NoVisibleValues_NoVisibleOptions()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var commandMock = CreateCommand(null, "blub");
            var value1Mock = CreateValue(commandMock, 0, "Val1");
            var option1Mock = CreateOption(commandMock, null, null);
            _ = value1Mock.Setup(x => x.Hidden).Returns(true);
            _ = option1Mock.Setup(x => x.Hidden).Returns(true);
            _ = ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));
            _ = ConsoleServiceMock.Setup(x => x.IsOutputRedirected).Returns(false);

            HelpPage.WriteCommandParameters(new CliError("MyError", commandMock.Object));

            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [TestMethod]
        public void WriteCommandParameters_WithValues_NoOptions()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var commandMock = CreateCommand(null, "blub");
            var value1Mock = CreateValue(commandMock, 2, "Val1");
            var value2Mock = CreateValue(commandMock, 1, "Val2");
            _ = value1Mock.Setup(x => x.HelpText).Returns("This is my help text for value 1.");
            _ = value2Mock.Setup(x => x.HelpText).Returns("This is my very longer help text for value 2 to test the wrapping.");
            _ = value2Mock.Setup(x => x.IsRequired).Returns(true);
            _ = ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));
            _ = ConsoleServiceMock.Setup(x => x.IsOutputRedirected).Returns(false);

            HelpPage.WriteCommandParameters(new CliError("MyError", commandMock.Object));

            Assert.AreEqual(
                $"{NL}Values:{NL}" +
                $"   Val2   This is my very longer help text for   {NL}" +
                $"          value 2 to test the wrapping.          {NL}" +
                $"   Val1   (Optional) This is my help text for    {NL}" +
                $"          value 1.                               {NL}",
                sb.ToString());
        }

        [TestMethod]
        public void WriteCommandParameters_NoValues_WithRequiredOptions()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var commandMock = CreateCommand(null, "blub");
            var option1Mock = CreateOption(commandMock, 'a', "blub", 'b');
            var option2Mock = CreateOption(commandMock, null, "blib");
            var option3Mock = CreateOption(commandMock, 'A', "blab", "blob");
            _ = option1Mock.Setup(x => x.HelpOrder).Returns(-1);
            _ = option1Mock.Setup(x => x.HelpText).Returns("Help text for option1.");
            _ = option1Mock.Setup(x => x.IsRequired).Returns(true);
            _ = option2Mock.Setup(x => x.HelpText).Returns("Help text for option2.");
            _ = option2Mock.Setup(x => x.IsRequired).Returns(true);
            _ = option3Mock.Setup(x => x.HelpText).Returns("Help text for option3. This has longer text to test wrapping.");
            _ = option3Mock.Setup(x => x.IsRequired).Returns(true);
            _ = ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));
            _ = ConsoleServiceMock.Setup(x => x.IsOutputRedirected).Returns(false);

            HelpPage.WriteCommandParameters(new CliError("MyError", commandMock.Object));

            Assert.AreEqual(
                $"{NL}Required options:{NL}" +
                $"   -a, -b, --blub     Help text for option1.     {NL}" +
                $"   -A, --blab,        Help text for option3. This{NL}" +
                $"   --blob             has longer text to test    {NL}" +
                $"                      wrapping.                  {NL}" +
                $"   --blib             Help text for option2.     {NL}",
                sb.ToString());
        }

        [TestMethod]
        public void WriteCommandParameters_NoValues_WithOptionalOptions()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var commandMock = CreateCommand(null, "blub");
            var option1Mock = CreateOption(commandMock, 'a', "blub", 'b');
            var option2Mock = CreateOption(commandMock, null, "blib");
            var option3Mock = CreateOption(commandMock, 'A', "blab", "blob");
            _ = option1Mock.Setup(x => x.HelpOrder).Returns(-1);
            _ = option1Mock.Setup(x => x.HelpText).Returns("Help text for option1.");
            _ = option2Mock.Setup(x => x.HelpText).Returns("Help text for option2.");
            _ = option3Mock.Setup(x => x.HelpText).Returns("Help text for option3. This has longer text to test wrapping.");
            _ = commandMock.Setup(x => x.Options).Returns(new[] { option1Mock.Object, option2Mock.Object, option3Mock.Object });
            _ = commandMock.Setup(x => x.Values).Returns(Array.Empty<ICliCommandValueInfo>());
            _ = ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));
            _ = ConsoleServiceMock.Setup(x => x.IsOutputRedirected).Returns(false);

            HelpPage.WriteCommandParameters(new CliError("MyError", commandMock.Object));

            Assert.AreEqual(
                $"{NL}Optional options:{NL}" +
                $"   -a, -b, --blub     Help text for option1.     {NL}" +
                $"   -A, --blab,        Help text for option3. This{NL}" +
                $"   --blob             has longer text to test    {NL}" +
                $"                      wrapping.                  {NL}" +
                $"   --blib             Help text for option2.     {NL}",
                sb.ToString());
        }

        [TestMethod]
        public void WriteCommandParameters_Everything()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var commandMock = CreateCommand(null, "blub");
            var value1Mock = CreateValue(commandMock, 1, "Val1");
            var option1Mock = CreateOption(commandMock, 'A', "blab", "blob");
            var option2Mock = CreateOption(commandMock, 'A', "blab", "blob");
            _ = value1Mock.Setup(x => x.HelpText).Returns("This is my very longer help text for value 1 to test the wrapping.");
            _ = value1Mock.Setup(x => x.IsRequired).Returns(true);
            _ = option1Mock.Setup(x => x.HelpText).Returns("Help text for optional. This has longer text to test wrapping.");
            _ = option2Mock.Setup(x => x.HelpText).Returns("Help text for required. This has longer text to test wrapping.");
            _ = option2Mock.Setup(x => x.IsRequired).Returns(true);
            _ = ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));
            _ = ConsoleServiceMock.Setup(x => x.IsOutputRedirected).Returns(false);

            HelpPage.WriteCommandParameters(new CliError("MyError", commandMock.Object));

            Assert.AreEqual(
                $"{NL}Values:{NL}" +
                $"   Val1   This is my very longer help text for   {NL}" +
                $"          value 1 to test the wrapping.          {NL}" +
                $"{NL}Required options:{NL}" +
                $"   -A, --blab,        Help text for required.    {NL}" +
                $"   --blob             This has longer text to    {NL}" +
                $"                      test wrapping.             {NL}" +
                $"{NL}Optional options:{NL}" +
                $"   -A, --blab,        Help text for optional.    {NL}" +
                $"   --blob             This has longer text to    {NL}" +
                $"                      test wrapping.             {NL}",
                sb.ToString());
        }

        [TestMethod]
        public void WriteCommands_NoAffectedCommand_NoCommands()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var collectionMock = Mocks.Create<IReadOnlyCliCommandInfoCollection>();
            _ = collectionMock.Setup(x => x.GetRootCommands()).Returns(Array.Empty<ICliCommandInfo>());
            _ = AppMock.Setup(x => x.Commands).Returns(collectionMock.Object);

            HelpPage.WriteCommands(new CliError("MyError"));

            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [TestMethod]
        public void WriteCommands_NoAffectedCommand_NoVisibleCommands()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var collectionMock = Mocks.Create<IReadOnlyCliCommandInfoCollection>();
            var command1Mock = Mocks.Create<ICliCommandInfo>();
            _ = command1Mock.Setup(x => x.Hidden).Returns(true);
            _ = collectionMock.Setup(x => x.GetRootCommands()).Returns(new[] { command1Mock.Object });
            _ = AppMock.Setup(x => x.Commands).Returns(collectionMock.Object);

            HelpPage.WriteCommands(new CliError("MyError"));

            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [TestMethod]
        public void WriteCommands_NoAffectedCommand_WithCommands()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var collectionMock = Mocks.Create<IReadOnlyCliCommandInfoCollection>();
            var command1Mock = CreateCommand(null, "zorro", "lorro");
            var command2Mock = CreateCommand(null, "list");
            var command3Mock = CreateCommand(null, "zzzzz");
            var command4Mock = CreateCommand(null, "ape");
            _ = command1Mock.Setup(x => x.HelpText).Returns("This should be the last command in the list.");
            _ = command2Mock.Setup(x => x.IsDefault).Returns(true);
            _ = command2Mock.Setup(x => x.Order).Returns(5);
            _ = command2Mock.Setup(x => x.HelpText).Returns("The is the default command and should be at the top of the list.");
            _ = command3Mock.Setup(x => x.Order).Returns(-2);
            _ = command3Mock.Setup(x => x.HelpText).Returns("Should be second command in list.");
            _ = command4Mock.Setup(x => x.HelpText).Returns("Should be third command in list.");
            _ = collectionMock.Setup(x => x.GetRootCommands()).Returns(new[] { command1Mock.Object, command2Mock.Object, command3Mock.Object, command4Mock.Object });
            _ = AppMock.Setup(x => x.Commands).Returns(collectionMock.Object);
            _ = ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(55, int.MaxValue));
            _ = ConsoleServiceMock.Setup(x => x.IsOutputRedirected).Returns(false);

            HelpPage.WriteCommands(new CliError("MyError"));

            Assert.AreEqual(
                $"{NL}Commands:{NL}" +
                $"   list           (Default) The is the default command{NL}" +
                $"                  and should be at the top of the     {NL}" +
                $"                  list.                               {NL}" +
                $"   zzzzz          Should be second command in list.   {NL}" +
                $"   ape            Should be third command in list.    {NL}" +
                $"   zorro, lorro   This should be the last command in  {NL}" +
                $"                  the list.                           {NL}",
                sb.ToString());
        }

        [TestMethod]
        public void WriteCommands_WithAffectedCommand_NoCommands()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var commandMock = CreateCommand(null, "blub");
            _ = commandMock.Setup(x => x.ChildCommands).Returns(Array.Empty<ICliCommandInfo>());

            HelpPage.WriteCommands(new CliError("MyError", commandMock.Object));

            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [TestMethod]
        public void WriteCommands_WithAffectedCommand_NoVisibleCommands()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var commandMock = CreateCommand(null, "blub");
            var childCommand1Mock = CreateCommand(commandMock, "blib");
            _ = childCommand1Mock.Setup(x => x.Hidden).Returns(true);

            HelpPage.WriteCommands(new CliError("MyError", commandMock.Object));

            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [TestMethod]
        public void WriteCommands_WithAffectedCommand_WithCommands()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var commandMock = CreateCommand(null, "blub");
            var childCommand1Mock = CreateCommand(commandMock, "zorro", "lorro");
            var childCommand2Mock = CreateCommand(commandMock, "list");
            var childCommand3Mock = CreateCommand(commandMock, "zzzzz");
            var childCommand4Mock = CreateCommand(commandMock, "ape");
            _ = childCommand1Mock.Setup(x => x.HelpText).Returns("This should be the last command in the list.");
            _ = childCommand2Mock.Setup(x => x.IsDefault).Returns(true);
            _ = childCommand2Mock.Setup(x => x.Order).Returns(5);
            _ = childCommand2Mock.Setup(x => x.HelpText).Returns("The is the default command and should be at the top of the list.");
            _ = childCommand3Mock.Setup(x => x.Order).Returns(-2);
            _ = childCommand3Mock.Setup(x => x.HelpText).Returns("Should be second command in list.");
            _ = childCommand4Mock.Setup(x => x.HelpText).Returns("Should be third command in list.");
            _ = ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(55, int.MaxValue));
            _ = ConsoleServiceMock.Setup(x => x.IsOutputRedirected).Returns(false);

            HelpPage.WriteCommands(new CliError("MyError", commandMock.Object));

            Assert.AreEqual(
                $"{NL}Commands:{NL}" +
                $"   list           (Default) The is the default command{NL}" +
                $"                  and should be at the top of the     {NL}" +
                $"                  list.                               {NL}" +
                $"   zzzzz          Should be second command in list.   {NL}" +
                $"   ape            Should be third command in list.    {NL}" +
                $"   zorro, lorro   This should be the last command in  {NL}" +
                $"                  the list.                           {NL}",
                sb.ToString());
        }

        [TestMethod]
        public void WriteCommandVersions_NoAffectedCommand_NoCommands()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var collectionMock = Mocks.Create<IReadOnlyCliCommandInfoCollection>();
            _ = collectionMock.Setup(x => x.GetRootCommands()).Returns(Array.Empty<ICliCommandInfo>());
            _ = AppMock.Setup(x => x.Commands).Returns(collectionMock.Object);

            HelpPage.WriteCommandVersions(new CliError("MyError"));

            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [TestMethod]
        public void WriteCommandVersions_NoAffectedCommand_NoOverridingCommands()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var commandMock = CreateCommand(null, "blub");
            var collectionMock = Mocks.Create<IReadOnlyCliCommandInfoCollection>();
            _ = collectionMock.Setup(x => x.GetRootCommands()).Returns(new[] { commandMock.Object });
            _ = AppMock.Setup(x => x.Commands).Returns(collectionMock.Object);

            HelpPage.WriteCommandVersions(new CliError("MyError"));

            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [TestMethod]
        public void WriteCommandVersions_NoAffectedCommand_NoVisibleCommands()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var commandMock = CreateCommand(null, "blub");
            var collectionMock = Mocks.Create<IReadOnlyCliCommandInfoCollection>();
            _ = collectionMock.Setup(x => x.GetRootCommands()).Returns(new[] { commandMock.Object });
            _ = AppMock.Setup(x => x.Commands).Returns(collectionMock.Object);

            HelpPage.WriteCommandVersions(new CliError("MyError"));

            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [TestMethod]
        [DataRow(false, DisplayName = "Without Fancy Console")]
        [DataRow(true, DisplayName = "With Fancy Console")]
        public void WriteCommandVersions_NoAffectedCommand_WithCommands(bool fancyConsole)
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var command1Mock = CreateCommand(null, "cmd1");
            var command2Mock = CreateCommand(null, "cmd2");
            var command3Mock = CreateCommand(null, "cmd3", "command3", "my-command-3");
            var collectionMock = Mocks.Create<IReadOnlyCliCommandInfoCollection>();
            _ = command2Mock.Setup(x => x.ParserOptions).Returns(new CliParserOptions { Author = "You", Year = "4711" });
            _ = command3Mock.Setup(x => x.ParserOptions).Returns(new CliParserOptions { Name = "Yours", Version = "99" });
            _ = collectionMock.Setup(x => x.GetRootCommands()).Returns(new[] { command1Mock.Object, command2Mock.Object, command3Mock.Object });
            _ = AppMock.Setup(x => x.Commands).Returns(collectionMock.Object);
            _ = AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { Name = "My", Year = "1337", Author = "Me", Version = "15" });
            _ = ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(55, int.MaxValue));
            _ = ConsoleServiceMock.Setup(x => x.IsOutputRedirected).Returns(false);
            _ = ConsoleServiceMock.Setup(x => x.IsFancyConsole).Returns(fancyConsole);

            HelpPage.WriteCommandVersions(new CliError("MyError"));

            var cchar = fancyConsole ? "©" : "(C)";
            Assert.AreEqual(
                $"{NL}Commands:{NL}" +
                $"   cmd2                         15   {cchar} 4711 You{NL}" +
                $"   cmd3, command3,      Yours   99   {cchar} 1337 Me {NL}" +
                $"   my-command-3                                {(fancyConsole ? string.Empty : "  ")}{NL}",
                sb.ToString());
        }

        [TestMethod]
        public void WriteCommandVersions_WithAffectedCommand_NoCommands()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var commandMock = CreateCommand(null, "blub");
            _ = commandMock.Setup(x => x.ChildCommands).Returns(Array.Empty<ICliCommandInfo>());

            HelpPage.WriteCommandVersions(new CliError("MyError", commandMock.Object));

            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [TestMethod]
        public void WriteCommandVersions_WithAffectedCommand_NoOverridingCommands()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var commandMock = CreateCommand(null, "blub");
            _ = CreateCommand(commandMock, "cmd1");

            HelpPage.WriteCommandVersions(new CliError("MyError", commandMock.Object));

            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [TestMethod]
        public void WriteCommandVersions_WithAffectedCommand_NoVisibleCommands()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var commandMock = CreateCommand(null, "blub");
            var childCommandMock = CreateCommand(commandMock, "cmd1");
            _ = childCommandMock.Setup(x => x.Hidden).Returns(true);

            HelpPage.WriteCommandVersions(new CliError("MyError", commandMock.Object));

            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [TestMethod]
        [DataRow(false, DisplayName = "Without Fancy Console")]
        [DataRow(true, DisplayName = "With Fancy Console")]
        public void WriteCommandVersions_WithAffectedCommand_WithCommands_FallbackToParentCommand(bool fancyConsole)
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var commandMock = CreateCommand(null, "blub");
            var childCommand1Mock = CreateCommand(commandMock, "cmd1");
            var childCommand2Mock = CreateCommand(commandMock, "cmd2");
            var childCommand3Mock = CreateCommand(commandMock, "cmd3", "command3", "my-command-3");
            _ = commandMock.Setup(x => x.ParserOptions).Returns(new CliParserOptions { Author = "Other", Year = "0815", Version = "77" });
            _ = childCommand2Mock.Setup(x => x.ParserOptions).Returns(new CliParserOptions { Author = "You", Year = "4711" });
            _ = childCommand3Mock.Setup(x => x.ParserOptions).Returns(new CliParserOptions { Name = "Yours", Version = "99" });
            _ = ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(55, int.MaxValue));
            _ = ConsoleServiceMock.Setup(x => x.IsOutputRedirected).Returns(false);
            _ = ConsoleServiceMock.Setup(x => x.IsFancyConsole).Returns(fancyConsole);

            HelpPage.WriteCommandVersions(new CliError("MyError", commandMock.Object));

            var cchar = fancyConsole ? "©" : "(C)";
            Assert.AreEqual(
                $"{NL}Commands:{NL}" +
                $"   cmd2                         77   {cchar} 4711 You  {NL}" +
                $"   cmd3, command3,      Yours   99   {cchar} 0815 Other{NL}" +
                $"   my-command-3                                  {(fancyConsole ? string.Empty : "  ")}{NL}",
                sb.ToString());
        }

        [TestMethod]
        [DataRow(false, DisplayName = "Without Fancy Console")]
        [DataRow(true, DisplayName = "With Fancy Console")]
        public void WriteCommandVersions_WithAffectedCommand_WithCommands_FallbackToAppp(bool fancyConsole)
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var commandMock = CreateCommand(null, "blub");
            var childCommand1Mock = CreateCommand(commandMock, "cmd1");
            var childCommand2Mock = CreateCommand(commandMock, "cmd2");
            var childCommand3Mock = CreateCommand(commandMock, "cmd3", "command3", "my-command-3");
            _ = childCommand2Mock.Setup(x => x.ParserOptions).Returns(new CliParserOptions { Author = "You", Year = "4711" });
            _ = childCommand3Mock.Setup(x => x.ParserOptions).Returns(new CliParserOptions { Name = "Yours", Version = "99" });
            _ = AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { Name = "My", Year = "1337", Author = "Me", Version = "15" });
            _ = ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(55, int.MaxValue));
            _ = ConsoleServiceMock.Setup(x => x.IsOutputRedirected).Returns(false);
            _ = ConsoleServiceMock.Setup(x => x.IsFancyConsole).Returns(fancyConsole);

            HelpPage.WriteCommandVersions(new CliError("MyError", commandMock.Object));

            var cchar = fancyConsole ? "©" : "(C)";
            Assert.AreEqual(
                $"{NL}Commands:{NL}" +
                $"   cmd2                         15   {cchar} 4711 You{NL}" +
                $"   cmd3, command3,      Yours   99   {cchar} 1337 Me {NL}" +
                $"   my-command-3                                {(fancyConsole ? string.Empty : "  ")}{NL}",
                sb.ToString());
        }

        private static StringBuilder AttachStringBuilder(Mock<IConsoleService> consoleServiceMock)
        {
            var result = new StringBuilder();
            _ = consoleServiceMock.Setup(x => x.Write(It.IsAny<string?>())).Callback<string?>(s => result.Append(s));
            _ = consoleServiceMock.Setup(x => x.WriteLine(It.IsAny<string?>())).Callback<string?>(s => result.AppendLine(s));
            _ = consoleServiceMock.SetupGet(x => x.ForegroundColor).Returns(ConsoleColor.Gray);
            _ = consoleServiceMock.SetupSet(x => x.ForegroundColor = It.IsAny<ConsoleColor>());
            _ = consoleServiceMock.SetupGet(x => x.BackgroundColor).Returns(ConsoleColor.Black);
            _ = consoleServiceMock.SetupSet(x => x.BackgroundColor = It.IsAny<ConsoleColor>());
            return result;
        }

        private Mock<ICliCommandInfo> CreateCommand(Mock<ICliCommandInfo>? parentCommand, string name, params string[] additionalAliases)
        {
            var command = Mocks.Create<ICliCommandInfo>();
            _ = command.Setup(x => x.Aliases).Returns(additionalAliases.Prepend(name).ToArray());
            _ = command.Setup(x => x.Attribute).Returns(new CliCommandAttribute(string.Empty));
            _ = command.Setup(x => x.ChildCommands).Returns(new List<ICliCommandInfo>());
            _ = command.Setup(x => x.CommandType).Returns(typeof(object));
            _ = command.Setup(x => x.HelpText).Returns(string.Empty);
            _ = command.Setup(x => x.Hidden).Returns(false);
            _ = command.Setup(x => x.IsDefault).Returns(false);
            _ = command.Setup(x => x.IsExecutable).Returns(true);
            _ = command.Setup(x => x.Name).Returns(name);
            _ = command.Setup(x => x.Options).Returns(new List<ICliCommandOptionInfo>());
            _ = command.Setup(x => x.OptionsInstance).Returns((object?)null);
            _ = command.Setup(x => x.Order).Returns(0);
            _ = command.Setup(x => x.ParentCommand).Returns(parentCommand?.Object ?? null);
            _ = command.Setup(x => x.ParserOptions).Returns(new CliParserOptions());
            _ = command.Setup(x => x.Values).Returns(new List<ICliCommandValueInfo>());

            if (parentCommand != null)
                ((List<ICliCommandInfo>)parentCommand.Object.ChildCommands).Add(command.Object);

            return command;
        }

        private Mock<ICliCommandOptionInfo> CreateOption(Mock<ICliCommandInfo> command, char? shortName, string? name, params object[] additionalNames)
        {
            var shortNames = additionalNames.OfType<char>();
            if (shortName.HasValue)
                shortNames = shortNames.Prepend(shortName.Value);
            var names = additionalNames.OfType<string>();
            if (name != null)
                names = names.Prepend(name);

            var option = Mocks.Create<ICliCommandOptionInfo>();
            _ = option.Setup(x => x.Aliases).Returns(names.ToArray());
            _ = option.Setup(x => x.Attribute).Returns((CliCommandOptionAttribute?)null!);
            _ = option.Setup(x => x.Command).Returns(command.Object);
            _ = option.Setup(x => x.DefaultValue).Returns((object?)null);
            _ = option.Setup(x => x.HelpOrder).Returns(0);
            _ = option.Setup(x => x.HelpText).Returns(string.Empty);
            _ = option.Setup(x => x.Hidden).Returns(false);
            _ = option.Setup(x => x.IsRequired).Returns(false);
            _ = option.Setup(x => x.PropertyName).Returns(string.Empty);
            _ = option.Setup(x => x.PropertyType).Returns(typeof(object));
            _ = option.Setup(x => x.ShortAliases).Returns(shortNames.ToArray());

            ((List<ICliCommandOptionInfo>)command.Object.Options).Add(option.Object);

            return option;
        }

        private Mock<ICliCommandValueInfo> CreateValue(Mock<ICliCommandInfo> command, int order, string displayName)
        {
            var value = Mocks.Create<ICliCommandValueInfo>();
            _ = value.Setup(x => x.Attribute).Returns((CliCommandValueAttribute)null!);
            _ = value.Setup(x => x.Command).Returns(command.Object);
            _ = value.Setup(x => x.DefaultValue).Returns((object?)null);
            _ = value.Setup(x => x.DisplayName).Returns(displayName);
            _ = value.Setup(x => x.HelpText).Returns(string.Empty);
            _ = value.Setup(x => x.Hidden).Returns(false);
            _ = value.Setup(x => x.IsRequired).Returns(false);
            _ = value.Setup(x => x.Order).Returns(order);
            _ = value.Setup(x => x.PropertyName).Returns(string.Empty);
            _ = value.Setup(x => x.PropertyType).Returns(typeof(object));

            ((List<ICliCommandValueInfo>)command.Object.Values).Add(value.Object);

            return value;
        }

        private ConsoleSize CreateConsoleSize(int width, int height)
        {
            var widthCallback = Mocks.Create<Action<int>>();
            var heightCallback = Mocks.Create<Action<int>>();
            return new ConsoleSize(() => width, widthCallback.Object, () => height, heightCallback.Object);
        }

        private void TestWriteErrorMessgae(Expression<Func<CliError>> errorFunc, string expectedErrorMessage, bool addException)
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var exceptionMessage1 = Guid.NewGuid().ToString();
            var exceptionMessage2 = Guid.NewGuid().ToString();
            var errorExpr = errorFunc;
            _ = ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));
            if (addException)
            {
                var prop = typeof(CliError).GetProperty(nameof(CliError.Exception), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)!;
                var bindExpr = Expression.Bind(prop, Expression.Constant(new Exception(exceptionMessage1 + NL + exceptionMessage2)));
                MemberInitExpression? initExpr = null;
                if (errorFunc.Body is NewExpression newExpr)
                    initExpr = Expression.MemberInit(newExpr, bindExpr);
                else if (errorFunc.Body is MemberInitExpression initExpr2)
                    initExpr = initExpr2.Update(initExpr2.NewExpression, initExpr2.Bindings.Append(bindExpr));

                if (initExpr != null)
                    errorExpr = Expression.Lambda<Func<CliError>>(initExpr);
            }

            HelpPage.WriteErrorMessage(errorExpr.Compile().Invoke());

            if (addException)
                Assert.AreEqual($"{expectedErrorMessage}{NL}   {exceptionMessage1,-46}{NL}   {exceptionMessage2,-46}{NL}", sb.ToString());
            else
                Assert.AreEqual($"{expectedErrorMessage}{NL}", sb.ToString());
        }

        private CliCommandOptionInfo CreateOptionInfo(CliCommandOptionAttribute option)
        {
            var p = Mocks.Create<PropertyInfo>();
            _ = p.Setup(x => x.GetIndexParameters()).Returns(Array.Empty<ParameterInfo>());
            _ = p.Setup(x => x.GetAccessors(true)).Returns(Array.Empty<MethodInfo>());
            _ = p.Setup(x => x.CanRead).Returns(true);
            _ = p.Setup(x => x.CanWrite).Returns(true);
            var extensionStorage = new ObjectExtensionDataStorage();

            return new CliCommandOptionInfo(
                extensionStorage,
                Factory.Create<TestCommandOptions>(),
                p.Object,
                option);
        }

        private Mock<ICliApplicationBase> CreateAppMock()
        {
            var result = Mocks.Create<ICliApplicationBase>();
            _ = result.Setup(x => x.Options).Returns(new CliApplicationOptions());
            return result;
        }

        [ExcludeFromCodeCoverage]
        [CliCommand("blub")]
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

            public PrivateCliHelpPage(ICliApplicationBase app, IConsoleService consoleService)
                : this(new CliHelpPage(app, consoleService))
            {
            }

            public PrivateCliHelpPage(CliHelpPage helpPage)
            {
                _po = new PrivateObject(helpPage);
            }

            public static string GetOptionName(CliCommandOptionInfo option)
            {
                return (string)_pt.InvokeStatic(nameof(GetOptionName), option);
            }

            public void WriteVersionPage(IList<CliError> errors)
            {
                _ = _po.Invoke(nameof(WriteVersionPage), errors);
            }

            public void WriteHelpPage(IList<CliError> errors)
            {
                _ = _po.Invoke(nameof(WriteHelpPage), errors);
            }

            public void WriteCommandNameAndVersion(CliError error)
            {
                _ = _po.Invoke(nameof(WriteCommandNameAndVersion), error);
            }

            public void WriteCopyright(CliError error)
            {
                _ = _po.Invoke(nameof(WriteCopyright), error);
            }

            public void WriteErrorMessage(CliError error)
            {
                _ = _po.Invoke(nameof(WriteErrorMessage), error);
            }

            public void WriteCommandUsage(CliError error)
            {
                _ = _po.Invoke(nameof(WriteCommandUsage), error);
            }

            public void WriteCommandParameters(CliError error)
            {
                _ = _po.Invoke(nameof(WriteCommandParameters), error);
            }

            public void WriteCommands(CliError error)
            {
                _ = _po.Invoke(nameof(WriteCommands), error);
            }

            public void WriteCommandVersions(CliError error)
            {
                _ = _po.Invoke(nameof(WriteCommandVersions), error);
            }
        }
    }
}
