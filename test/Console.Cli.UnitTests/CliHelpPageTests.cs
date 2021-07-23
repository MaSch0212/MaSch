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
            AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { Name = "MyName", Version = "MyVersion" });
            var sb = AttachStringBuilder(ConsoleServiceMock);

            HelpPage.WriteCommandNameAndVersion(new CliError("blub"));

            Assert.AreEqual($"MyName MyVersion{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCommandNameAndVersion_CommandSpecific()
        {
            var commandMock = Mocks.Create<ICliCommandInfo>();
            commandMock.Setup(x => x.ParserOptions).Returns(new CliParserOptions { Name = "YourName", Version = "YourVersion" });
            AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { Name = "MyName", Version = "MyVersion" });
            var sb = AttachStringBuilder(ConsoleServiceMock);

            HelpPage.WriteCommandNameAndVersion(new CliError("blub", commandMock.Object));

            Assert.AreEqual($"YourName YourVersion{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCopyright_FancyConsole()
        {
            AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { Year = "1337", Author = "Me" });
            ConsoleServiceMock.Setup(x => x.IsFancyConsole).Returns(true);
            var sb = AttachStringBuilder(ConsoleServiceMock);

            HelpPage.WriteCopyright(new CliError("blub"));

            Assert.AreEqual($"Copyright © 1337 Me{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCopyright_NonFancyConsole()
        {
            AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { Year = "1337", Author = "Me" });
            ConsoleServiceMock.Setup(x => x.IsFancyConsole).Returns(false);
            var sb = AttachStringBuilder(ConsoleServiceMock);

            HelpPage.WriteCopyright(new CliError("blub"));

            Assert.AreEqual($"Copyright (C) 1337 Me{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCopyright_CommandSpecific()
        {
            var commandMock = CreateCommand(null, "blub");
            commandMock.Setup(x => x.ParserOptions).Returns(new CliParserOptions { Year = "4711", Author = "You" });
            AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { Year = "1337", Author = "Me" });
            ConsoleServiceMock.Setup(x => x.IsFancyConsole).Returns(true);
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
            => TestWriteErrorMessgae(() => new CliError("My custom error message"), "My custom error message", addException);

        [TestMethod]
        public void WriteVersionPage()
        {
            var helpPageMock = Mocks.Create<CliHelpPage>(AppMock.Object, ConsoleServiceMock.Object);
            var errors = new[] { new CliError(CliErrorType.VersionRequested) };
            var callOrder = new List<string>();
            helpPageMock.Protected().Setup("WriteVersionPage", (IList<CliError>)errors).CallBase();
            helpPageMock.Protected().Setup("WriteCommandNameAndVersion", errors[0]).Callback(() => callOrder.Add("CommandNameAndVersion"));
            helpPageMock.Protected().Setup("WriteCopyright", errors[0]).Callback(() => callOrder.Add("Copyright"));
            helpPageMock.Protected().Setup("WriteCommandVersions", errors[0]).Callback(() => callOrder.Add("CommandVersions"));
            ConsoleServiceMock.Setup(x => x.WriteLine(string.Empty)).Callback(() => callOrder.Add(string.Empty));

            new PrivateCliHelpPage(helpPageMock.Object).WriteVersionPage(errors);

            Assert.AreCollectionsEqual(new[] { "CommandNameAndVersion", "Copyright", "CommandVersions", string.Empty }, callOrder);
        }

        [TestMethod]
        public void WriteHelpPage()
        {
            var helpPageMock = Mocks.Create<CliHelpPage>(AppMock.Object, ConsoleServiceMock.Object);
            var errors = new[] { new CliError(CliErrorType.HelpRequested) };
            var callOrder = new List<string>();
            helpPageMock.Protected().Setup("WriteHelpPage", (IList<CliError>)errors).CallBase();
            helpPageMock.Protected().Setup("WriteCommandNameAndVersion", errors[0]).Callback(() => callOrder.Add("CommandNameAndVersion"));
            helpPageMock.Protected().Setup("WriteCopyright", errors[0]).Callback(() => callOrder.Add("Copyright"));
            helpPageMock.Protected().Setup("WriteCommandUsage", errors[0]).Callback(() => callOrder.Add("CommandUsage"));
            helpPageMock.Protected().Setup("WriteCommandParameters", errors[0]).Callback(() => callOrder.Add("CommandParameters"));
            helpPageMock.Protected().Setup("WriteCommands", errors[0]).Callback(() => callOrder.Add("Commands"));
            ConsoleServiceMock.Setup(x => x.WriteLine(string.Empty)).Callback(() => callOrder.Add(string.Empty));

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
            helpPageMock.Protected().Setup("WriteHelpPage", (IList<CliError>)errors).CallBase();
            helpPageMock.Protected().Setup("WriteCommandNameAndVersion", errors[0]).Callback(() => callOrder.Add("CommandNameAndVersion"));
            helpPageMock.Protected().Setup("WriteCopyright", errors[0]).Callback(() => callOrder.Add("Copyright"));
            helpPageMock.Protected().Setup("WriteCommandUsage", errors[0]).Callback(() => callOrder.Add("CommandUsage"));
            helpPageMock.Protected().Setup("WriteCommandParameters", errors[0]).Callback(() => callOrder.Add("CommandParameters"));
            helpPageMock.Protected().Setup("WriteCommands", errors[0]).Callback(() => callOrder.Add("Commands"));
            helpPageMock.Protected().Setup("WriteErrorMessage", ItExpr.IsAny<CliError>()).Callback<CliError>(e => callOrder.Add($"ErrorMessage_{e.CustomErrorMessage}"));
            ConsoleServiceMock.Setup(x => x.WriteLine(string.Empty)).Callback(() => callOrder.Add(string.Empty));

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
            helpPageMock.Setup(x => x.Write(It.IsAny<IEnumerable<CliError>>())).CallBase();
            helpPageMock.Protected().Setup("WriteHelpPage", ItExpr.IsAny<IList<CliError>>()).Callback<IList<CliError>>(e => errors = e);

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
            helpPageMock.Setup(x => x.Write(It.IsAny<IEnumerable<CliError>>())).CallBase();
            helpPageMock.Protected().Setup("WriteHelpPage", ItExpr.IsAny<IList<CliError>>()).Callback<IList<CliError>>(e => errors = e);

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
            helpPageMock.Setup(x => x.Write(It.IsAny<IEnumerable<CliError>>())).CallBase();
            helpPageMock.Protected().Setup("WriteHelpPage", ItExpr.IsAny<IList<CliError>>()).Callback<IList<CliError>>(e => errors = e);

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
            helpPageMock.Setup(x => x.Write(errors)).CallBase();
            helpPageMock.Protected().Setup("WriteVersionPage", (IList<CliError>)new[] { errors[1], errors[0] }).Verifiable(Verifiables, Times.Once());

            var result = helpPageMock.Object.Write(errors);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Write_HelpRequested()
        {
            var helpPageMock = Mocks.Create<CliHelpPage>(AppMock.Object, ConsoleServiceMock.Object);
            var errors = new[] { new CliError("gjklhf"), new CliError(CliErrorType.VersionRequested), new CliError(CliErrorType.HelpRequested) };
            helpPageMock.Setup(x => x.Write(errors)).CallBase();
            helpPageMock.Protected().Setup("WriteHelpPage", (IList<CliError>)new[] { errors[2], errors[0] }).Verifiable(Verifiables, Times.Once());

            var result = helpPageMock.Object.Write(errors);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Write_OtherErrors()
        {
            var helpPageMock = Mocks.Create<CliHelpPage>(AppMock.Object, ConsoleServiceMock.Object);
            var errors = new[] { new CliError("gjklhf") };
            helpPageMock.Setup(x => x.Write(errors)).CallBase();
            helpPageMock.Protected().Setup("WriteHelpPage", (IList<CliError>)errors).Verifiable(Verifiables, Times.Once());

            var result = helpPageMock.Object.Write(errors);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void WriteCommandUsage_NoAffectedCommand_NoCommands()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var collectionMock = Mocks.Create<IReadOnlyCliCommandInfoCollection>();
            collectionMock.Setup(x => x.GetRootCommands()).Returns(Array.Empty<ICliCommandInfo>());
            AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { CliName = "MyProgram" });
            AppMock.Setup(x => x.Commands).Returns(collectionMock.Object);
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));

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
            collectionMock.Setup(x => x.GetRootCommands()).Returns(new[] { command1Mock.Object, command2Mock.Object });
            AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { CliName = "MyProgram" });
            AppMock.Setup(x => x.Commands).Returns(collectionMock.Object);
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));

            HelpPage.WriteCommandUsage(new CliError("MyError"));

            Assert.AreEqual($"Usage: MyProgram <command>{new string(' ', 50 - 26 - 1)}{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCommandUsage_WithAffectedCommand_OneAlias_NoParentCommand_NoChildCommands_NoOptions_NoValues()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var aCommandMock = CreateCommand(null, "blub");
            AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { CliName = "MyProgram" });
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));

            HelpPage.WriteCommandUsage(new CliError("MyError", aCommandMock.Object));

            Assert.AreEqual($"Usage: MyProgram blub{new string(' ', 50 - 21 - 1)}{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCommandUsage_WithAffectedCommand_MultipleAliases_NoParentCommand_NoChildCommands_NoOptions_NoValues()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var aCommandMock = CreateCommand(null, "blub", "blib", "blab");
            AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { CliName = "MyProgram" });
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));

            HelpPage.WriteCommandUsage(new CliError("MyError", aCommandMock.Object));

            Assert.AreEqual($"Usage: MyProgram (blub|blib|blab){new string(' ', 50 - 33 - 1)}{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCommandUsage_WithAffectedCommand_OneAlias_WithParentCommand_NoChildCommands_NoOptions_NoValues()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var ppCommandMock = CreateCommand(null, "pp", "pi");
            var pCommandMock = CreateCommand(ppCommandMock, "p");
            var aCommandMock = CreateCommand(pCommandMock, "blub");
            AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { CliName = "MyProgram" });
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));

            HelpPage.WriteCommandUsage(new CliError("MyError", aCommandMock.Object));

            Assert.AreEqual($"Usage: MyProgram (pp|pi) p blub{new string(' ', 50 - 31 - 1)}{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCommandUsage_WithAffectedCommand_OneAlias_NoParentCommand_Executable_WithChildCommands_NoOptions_NoValues()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var aCommandMock = CreateCommand(null, "blub");
            var command1Mock = CreateCommand(aCommandMock, "command1");
            var command2Mock = CreateCommand(aCommandMock, "command2");
            AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { CliName = "MyProgram" });
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));

            HelpPage.WriteCommandUsage(new CliError("MyError", aCommandMock.Object));

            Assert.AreEqual($"Usage: MyProgram blub [command]{new string(' ', 50 - 31 - 1)}{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCommandUsage_WithAffectedCommand_OneAlias_NoParentCommand_NonExecutable_WithChildCommands_NoOptions_NoValues()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var aCommandMock = CreateCommand(null, "blub");
            var command1Mock = CreateCommand(aCommandMock, "command1");
            var command2Mock = CreateCommand(aCommandMock, "command2");
            aCommandMock.Setup(x => x.IsExecutable).Returns(false);
            AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { CliName = "MyProgram" });
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));

            HelpPage.WriteCommandUsage(new CliError("MyError", aCommandMock.Object));

            Assert.AreEqual($"Usage: MyProgram blub <command>{new string(' ', 50 - 31 - 1)}{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCommandUsage_WithAffectedCommand_OneAlias_NoParentCommand_NoChildCommands_WithOptions_NoValues()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var aCommandMock = CreateCommand(null, "blub");
            var option1Mock = CreateOption(aCommandMock, null, null);
            var option2Mock = CreateOption(aCommandMock, null, null);
            AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { CliName = "MyProgram" });
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));

            HelpPage.WriteCommandUsage(new CliError("MyError", aCommandMock.Object));

            Assert.AreEqual($"Usage: MyProgram blub [options]{new string(' ', 50 - 31 - 1)}{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCommandUsage_WithAffectedCommand_OneAlias_NoParentCommand_NoChildCommands_NoOptions_WithValues()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var aCommandMock = CreateCommand(null, "blub");
            var value1Mock = CreateValue(aCommandMock, 2, "Val1");
            var value2Mock = CreateValue(aCommandMock, 1, "Val2");
            value2Mock.Setup(x => x.IsRequired).Returns(true);
            AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { CliName = "MyProgram" });
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));

            HelpPage.WriteCommandUsage(new CliError("MyError", aCommandMock.Object));

            Assert.AreEqual($"Usage: MyProgram blub <Val2> [Val1]{new string(' ', 50 - 35 - 1)}{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCommandUsage_Everything()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var ppCommandMock = CreateCommand(null, "grandparent", "supervisor");
            var pCommandMock = CreateCommand(ppCommandMock, "parent");
            var aCommandMock = CreateCommand(pCommandMock, "blub", "blib", "blab");
            var command1Mock = CreateCommand(aCommandMock, "command1");
            var command2Mock = CreateCommand(aCommandMock, "command2");
            var value1Mock = CreateValue(aCommandMock, 2, "Val1");
            var value2Mock = CreateValue(aCommandMock, 1, "Val2");
            var option1Mock = CreateOption(aCommandMock, null, null);
            var option2Mock = CreateOption(aCommandMock, null, null);
            value2Mock.Setup(x => x.IsRequired).Returns(true);
            value2Mock.Setup(x => x.PropertyType).Returns(typeof(List<object>));
            option2Mock.Setup(x => x.Hidden).Returns(true);
            AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { CliName = "MyProgram" });
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));

            HelpPage.WriteCommandUsage(new CliError("MyError", aCommandMock.Object));

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
            var aCommandMock = CreateCommand(null, "blub");

            HelpPage.WriteCommandParameters(new CliError("MyError", aCommandMock.Object));

            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [TestMethod]
        public void WriteCommandParameters_NoVisibleValues_NoVisibleOptions()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var aCommandMock = CreateCommand(null, "blub");
            var value1Mock = CreateValue(aCommandMock, 0, "Val1");
            var option1Mock = CreateOption(aCommandMock, null, null);
            value1Mock.Setup(x => x.Hidden).Returns(true);
            option1Mock.Setup(x => x.Hidden).Returns(true);
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));
            ConsoleServiceMock.Setup(x => x.IsOutputRedirected).Returns(false);

            HelpPage.WriteCommandParameters(new CliError("MyError", aCommandMock.Object));

            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [TestMethod]
        public void WriteCommandParameters_WithValues_NoOptions()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var aCommandMock = CreateCommand(null, "blub");
            var value1Mock = CreateValue(aCommandMock, 2, "Val1");
            var value2Mock = CreateValue(aCommandMock, 1, "Val2");
            value1Mock.Setup(x => x.HelpText).Returns("This is my help text for value 1.");
            value2Mock.Setup(x => x.HelpText).Returns("This is my very longer help text for value 2 to test the wrapping.");
            value2Mock.Setup(x => x.IsRequired).Returns(true);
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));
            ConsoleServiceMock.Setup(x => x.IsOutputRedirected).Returns(false);

            HelpPage.WriteCommandParameters(new CliError("MyError", aCommandMock.Object));

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
            var aCommandMock = CreateCommand(null, "blub");
            var option1Mock = CreateOption(aCommandMock, 'a', "blub", 'b');
            var option2Mock = CreateOption(aCommandMock, null, "blib");
            var option3Mock = CreateOption(aCommandMock, 'A', "blab", "blob");
            option1Mock.Setup(x => x.HelpOrder).Returns(-1);
            option1Mock.Setup(x => x.HelpText).Returns("Help text for option1.");
            option1Mock.Setup(x => x.IsRequired).Returns(true);
            option2Mock.Setup(x => x.HelpText).Returns("Help text for option2.");
            option2Mock.Setup(x => x.IsRequired).Returns(true);
            option3Mock.Setup(x => x.HelpText).Returns("Help text for option3. This has longer text to test wrapping.");
            option3Mock.Setup(x => x.IsRequired).Returns(true);
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));
            ConsoleServiceMock.Setup(x => x.IsOutputRedirected).Returns(false);

            HelpPage.WriteCommandParameters(new CliError("MyError", aCommandMock.Object));

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
            var aCommandMock = CreateCommand(null, "blub");
            var option1Mock = CreateOption(aCommandMock, 'a', "blub", 'b');
            var option2Mock = CreateOption(aCommandMock, null, "blib");
            var option3Mock = CreateOption(aCommandMock, 'A', "blab", "blob");
            option1Mock.Setup(x => x.HelpOrder).Returns(-1);
            option1Mock.Setup(x => x.HelpText).Returns("Help text for option1.");
            option2Mock.Setup(x => x.HelpText).Returns("Help text for option2.");
            option3Mock.Setup(x => x.HelpText).Returns("Help text for option3. This has longer text to test wrapping.");
            aCommandMock.Setup(x => x.Options).Returns(new[] { option1Mock.Object, option2Mock.Object, option3Mock.Object });
            aCommandMock.Setup(x => x.Values).Returns(Array.Empty<ICliCommandValueInfo>());
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));
            ConsoleServiceMock.Setup(x => x.IsOutputRedirected).Returns(false);

            HelpPage.WriteCommandParameters(new CliError("MyError", aCommandMock.Object));

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
            var aCommandMock = CreateCommand(null, "blub");
            var value1Mock = CreateValue(aCommandMock, 1, "Val1");
            var option1Mock = CreateOption(aCommandMock, 'A', "blab", "blob");
            var option2Mock = CreateOption(aCommandMock, 'A', "blab", "blob");
            value1Mock.Setup(x => x.HelpText).Returns("This is my very longer help text for value 1 to test the wrapping.");
            value1Mock.Setup(x => x.IsRequired).Returns(true);
            option1Mock.Setup(x => x.HelpText).Returns("Help text for optional. This has longer text to test wrapping.");
            option2Mock.Setup(x => x.HelpText).Returns("Help text for required. This has longer text to test wrapping.");
            option2Mock.Setup(x => x.IsRequired).Returns(true);
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));
            ConsoleServiceMock.Setup(x => x.IsOutputRedirected).Returns(false);

            HelpPage.WriteCommandParameters(new CliError("MyError", aCommandMock.Object));

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
            collectionMock.Setup(x => x.GetRootCommands()).Returns(Array.Empty<ICliCommandInfo>());
            AppMock.Setup(x => x.Commands).Returns(collectionMock.Object);

            HelpPage.WriteCommands(new CliError("MyError"));

            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [TestMethod]
        public void WriteCommands_NoAffectedCommand_NoVisibleCommands()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var collectionMock = Mocks.Create<IReadOnlyCliCommandInfoCollection>();
            var command1Mock = Mocks.Create<ICliCommandInfo>();
            command1Mock.Setup(x => x.Hidden).Returns(true);
            collectionMock.Setup(x => x.GetRootCommands()).Returns(new[] { command1Mock.Object });
            AppMock.Setup(x => x.Commands).Returns(collectionMock.Object);

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
            command1Mock.Setup(x => x.HelpText).Returns("This should be the last command in the list.");
            command2Mock.Setup(x => x.IsDefault).Returns(true);
            command2Mock.Setup(x => x.Order).Returns(5);
            command2Mock.Setup(x => x.HelpText).Returns("The is the default command and should be at the top of the list.");
            command3Mock.Setup(x => x.Order).Returns(-2);
            command3Mock.Setup(x => x.HelpText).Returns("Should be second command in list.");
            command4Mock.Setup(x => x.HelpText).Returns("Should be third command in list.");
            collectionMock.Setup(x => x.GetRootCommands()).Returns(new[] { command1Mock.Object, command2Mock.Object, command3Mock.Object, command4Mock.Object });
            AppMock.Setup(x => x.Commands).Returns(collectionMock.Object);
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(55, int.MaxValue));
            ConsoleServiceMock.Setup(x => x.IsOutputRedirected).Returns(false);

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
            var aCommandMock = CreateCommand(null, "blub");
            aCommandMock.Setup(x => x.ChildCommands).Returns(Array.Empty<ICliCommandInfo>());

            HelpPage.WriteCommands(new CliError("MyError", aCommandMock.Object));

            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [TestMethod]
        public void WriteCommands_WithAffectedCommand_NoVisibleCommands()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var aCommandMock = CreateCommand(null, "blub");
            var command1Mock = CreateCommand(aCommandMock, "blib");
            command1Mock.Setup(x => x.Hidden).Returns(true);

            HelpPage.WriteCommands(new CliError("MyError", aCommandMock.Object));

            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [TestMethod]
        public void WriteCommands_WithAffectedCommand_WithCommands()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var aCommandMock = CreateCommand(null, "blub");
            var command1Mock = CreateCommand(aCommandMock, "zorro", "lorro");
            var command2Mock = CreateCommand(aCommandMock, "list");
            var command3Mock = CreateCommand(aCommandMock, "zzzzz");
            var command4Mock = CreateCommand(aCommandMock, "ape");
            command1Mock.Setup(x => x.HelpText).Returns("This should be the last command in the list.");
            command2Mock.Setup(x => x.IsDefault).Returns(true);
            command2Mock.Setup(x => x.Order).Returns(5);
            command2Mock.Setup(x => x.HelpText).Returns("The is the default command and should be at the top of the list.");
            command3Mock.Setup(x => x.Order).Returns(-2);
            command3Mock.Setup(x => x.HelpText).Returns("Should be second command in list.");
            command4Mock.Setup(x => x.HelpText).Returns("Should be third command in list.");
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(55, int.MaxValue));
            ConsoleServiceMock.Setup(x => x.IsOutputRedirected).Returns(false);

            HelpPage.WriteCommands(new CliError("MyError", aCommandMock.Object));

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
            collectionMock.Setup(x => x.GetRootCommands()).Returns(Array.Empty<ICliCommandInfo>());
            AppMock.Setup(x => x.Commands).Returns(collectionMock.Object);

            HelpPage.WriteCommandVersions(new CliError("MyError"));

            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [TestMethod]
        public void WriteCommandVersions_NoAffectedCommand_NoOverridingCommands()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var commandMock = CreateCommand(null, "blub");
            var collectionMock = Mocks.Create<IReadOnlyCliCommandInfoCollection>();
            collectionMock.Setup(x => x.GetRootCommands()).Returns(new[] { commandMock.Object });
            AppMock.Setup(x => x.Commands).Returns(collectionMock.Object);

            HelpPage.WriteCommandVersions(new CliError("MyError"));

            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [TestMethod]
        public void WriteCommandVersions_NoAffectedCommand_NoVisibleCommands()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var commandMock = CreateCommand(null, "blub");
            var collectionMock = Mocks.Create<IReadOnlyCliCommandInfoCollection>();
            collectionMock.Setup(x => x.GetRootCommands()).Returns(new[] { commandMock.Object });
            AppMock.Setup(x => x.Commands).Returns(collectionMock.Object);

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
            command2Mock.Setup(x => x.ParserOptions).Returns(new CliParserOptions { Author = "You", Year = "4711" });
            command3Mock.Setup(x => x.ParserOptions).Returns(new CliParserOptions { Name = "Yours", Version = "99" });
            collectionMock.Setup(x => x.GetRootCommands()).Returns(new[] { command1Mock.Object, command2Mock.Object, command3Mock.Object });
            AppMock.Setup(x => x.Commands).Returns(collectionMock.Object);
            AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { Name = "My", Year = "1337", Author = "Me", Version = "15" });
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(55, int.MaxValue));
            ConsoleServiceMock.Setup(x => x.IsOutputRedirected).Returns(false);
            ConsoleServiceMock.Setup(x => x.IsFancyConsole).Returns(fancyConsole);

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
            var aCommandMock = CreateCommand(null, "blub");
            aCommandMock.Setup(x => x.ChildCommands).Returns(Array.Empty<ICliCommandInfo>());

            HelpPage.WriteCommandVersions(new CliError("MyError", aCommandMock.Object));

            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [TestMethod]
        public void WriteCommandVersions_WithAffectedCommand_NoOverridingCommands()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var aCommandMock = CreateCommand(null, "blub");
            var commandMock = CreateCommand(aCommandMock, "cmd1");

            HelpPage.WriteCommandVersions(new CliError("MyError", aCommandMock.Object));

            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [TestMethod]
        public void WriteCommandVersions_WithAffectedCommand_NoVisibleCommands()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var aCommandMock = CreateCommand(null, "blub");
            var commandMock = CreateCommand(aCommandMock, "cmd1");
            commandMock.Setup(x => x.Hidden).Returns(true);

            HelpPage.WriteCommandVersions(new CliError("MyError", aCommandMock.Object));

            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [TestMethod]
        [DataRow(false, DisplayName = "Without Fancy Console")]
        [DataRow(true, DisplayName = "With Fancy Console")]
        public void WriteCommandVersions_WithAffectedCommand_WithCommands_FallbackToParentCommand(bool fancyConsole)
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var aCommandMock = CreateCommand(null, "blub");
            var command1Mock = CreateCommand(aCommandMock, "cmd1");
            var command2Mock = CreateCommand(aCommandMock, "cmd2");
            var command3Mock = CreateCommand(aCommandMock, "cmd3", "command3", "my-command-3");
            aCommandMock.Setup(x => x.ParserOptions).Returns(new CliParserOptions { Author = "Other", Year = "0815", Version = "77" });
            command2Mock.Setup(x => x.ParserOptions).Returns(new CliParserOptions { Author = "You", Year = "4711" });
            command3Mock.Setup(x => x.ParserOptions).Returns(new CliParserOptions { Name = "Yours", Version = "99" });
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(55, int.MaxValue));
            ConsoleServiceMock.Setup(x => x.IsOutputRedirected).Returns(false);
            ConsoleServiceMock.Setup(x => x.IsFancyConsole).Returns(fancyConsole);

            HelpPage.WriteCommandVersions(new CliError("MyError", aCommandMock.Object));

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
            var aCommandMock = CreateCommand(null, "blub");
            var command1Mock = CreateCommand(aCommandMock, "cmd1");
            var command2Mock = CreateCommand(aCommandMock, "cmd2");
            var command3Mock = CreateCommand(aCommandMock, "cmd3", "command3", "my-command-3");
            command2Mock.Setup(x => x.ParserOptions).Returns(new CliParserOptions { Author = "You", Year = "4711" });
            command3Mock.Setup(x => x.ParserOptions).Returns(new CliParserOptions { Name = "Yours", Version = "99" });
            AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { Name = "My", Year = "1337", Author = "Me", Version = "15" });
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(55, int.MaxValue));
            ConsoleServiceMock.Setup(x => x.IsOutputRedirected).Returns(false);
            ConsoleServiceMock.Setup(x => x.IsFancyConsole).Returns(fancyConsole);

            HelpPage.WriteCommandVersions(new CliError("MyError", aCommandMock.Object));

            var cchar = fancyConsole ? "©" : "(C)";
            Assert.AreEqual(
                $"{NL}Commands:{NL}" +
                $"   cmd2                         15   {cchar} 4711 You{NL}" +
                $"   cmd3, command3,      Yours   99   {cchar} 1337 Me {NL}" +
                $"   my-command-3                                {(fancyConsole ? string.Empty : "  ")}{NL}",
                sb.ToString());
        }

        private Mock<ICliCommandInfo> CreateCommand(Mock<ICliCommandInfo>? parentCommand, string name, params string[] additionalAliases)
        {
            var command = Mocks.Create<ICliCommandInfo>();
            command.Setup(x => x.Aliases).Returns(additionalAliases.Prepend(name).ToArray());
            command.Setup(x => x.Attribute).Returns(new CliCommandAttribute(string.Empty));
            command.Setup(x => x.ChildCommands).Returns(new List<ICliCommandInfo>());
            command.Setup(x => x.CommandType).Returns(typeof(object));
            command.Setup(x => x.HelpText).Returns(string.Empty);
            command.Setup(x => x.Hidden).Returns(false);
            command.Setup(x => x.IsDefault).Returns(false);
            command.Setup(x => x.IsExecutable).Returns(true);
            command.Setup(x => x.Name).Returns(name);
            command.Setup(x => x.Options).Returns(new List<ICliCommandOptionInfo>());
            command.Setup(x => x.OptionsInstance).Returns((object?)null);
            command.Setup(x => x.Order).Returns(0);
            command.Setup(x => x.ParentCommand).Returns(parentCommand?.Object ?? null);
            command.Setup(x => x.ParserOptions).Returns(new CliParserOptions());
            command.Setup(x => x.Values).Returns(new List<ICliCommandValueInfo>());

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
            option.Setup(x => x.Aliases).Returns(names.ToArray());
            option.Setup(x => x.Attribute).Returns((CliCommandOptionAttribute?)null!);
            option.Setup(x => x.Command).Returns(command.Object);
            option.Setup(x => x.DefaultValue).Returns((object?)null);
            option.Setup(x => x.HelpOrder).Returns(0);
            option.Setup(x => x.HelpText).Returns(string.Empty);
            option.Setup(x => x.Hidden).Returns(false);
            option.Setup(x => x.IsRequired).Returns(false);
            option.Setup(x => x.PropertyName).Returns(string.Empty);
            option.Setup(x => x.PropertyType).Returns(typeof(object));
            option.Setup(x => x.ShortAliases).Returns(shortNames.ToArray());

            ((List<ICliCommandOptionInfo>)command.Object.Options).Add(option.Object);

            return option;
        }

        private Mock<ICliCommandValueInfo> CreateValue(Mock<ICliCommandInfo> command, int order, string displayName)
        {
            var value = Mocks.Create<ICliCommandValueInfo>();
            value.Setup(x => x.Attribute).Returns((CliCommandValueAttribute)null!);
            value.Setup(x => x.Command).Returns(command.Object);
            value.Setup(x => x.DefaultValue).Returns((object?)null);
            value.Setup(x => x.DisplayName).Returns(displayName);
            value.Setup(x => x.HelpText).Returns(string.Empty);
            value.Setup(x => x.Hidden).Returns(false);
            value.Setup(x => x.IsRequired).Returns(false);
            value.Setup(x => x.Order).Returns(order);
            value.Setup(x => x.PropertyName).Returns(string.Empty);
            value.Setup(x => x.PropertyType).Returns(typeof(object));

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
            var exMsg1 = Guid.NewGuid().ToString();
            var exMsg2 = Guid.NewGuid().ToString();
            var errorExpr = errorFunc;
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));
            if (addException)
            {
                var prop = typeof(CliError).GetProperty(nameof(CliError.Exception), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)!;
                var bindExpr = Expression.Bind(prop, Expression.Constant(new Exception(exMsg1 + NL + exMsg2)));
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
                Assert.AreEqual($"{expectedErrorMessage}{NL}   {exMsg1.PadRight(46)}{NL}   {exMsg2.PadRight(46)}{NL}", sb.ToString());
            else
                Assert.AreEqual($"{expectedErrorMessage}{NL}", sb.ToString());
        }

        private CliCommandOptionInfo CreateOptionInfo(CliCommandOptionAttribute option)
        {
            var p = Mocks.Create<PropertyInfo>();
            p.Setup(x => x.GetIndexParameters()).Returns(Array.Empty<ParameterInfo>());
            p.Setup(x => x.GetAccessors(true)).Returns(Array.Empty<MethodInfo>());
            p.Setup(x => x.CanRead).Returns(true);
            p.Setup(x => x.CanWrite).Returns(true);
            var extensionStorage = new ObjectExtensionDataStorage();

            return new CliCommandOptionInfo(
                extensionStorage,
                Factory.Create<TestCommandOptions>(),
                p.Object,
                option);
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

        private Mock<ICliApplicationBase> CreateAppMock()
        {
            var result = Mocks.Create<ICliApplicationBase>();
            result.Setup(x => x.Options).Returns(new CliApplicationOptions());
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

            public void WriteVersionPage(IList<CliError> errors)
                => _po.Invoke(nameof(WriteVersionPage), errors);

            public void WriteHelpPage(IList<CliError> errors)
                => _po.Invoke(nameof(WriteHelpPage), errors);

            public void WriteCommandNameAndVersion(CliError error)
                => _po.Invoke(nameof(WriteCommandNameAndVersion), error);

            public void WriteCopyright(CliError error)
                => _po.Invoke(nameof(WriteCopyright), error);

            public void WriteErrorMessage(CliError error)
                => _po.Invoke(nameof(WriteErrorMessage), error);

            public void WriteCommandUsage(CliError error)
                => _po.Invoke(nameof(WriteCommandUsage), error);

            public void WriteCommandParameters(CliError error)
                => _po.Invoke(nameof(WriteCommandParameters), error);

            public void WriteCommands(CliError error)
                => _po.Invoke(nameof(WriteCommands), error);

            public void WriteCommandVersions(CliError error)
                => _po.Invoke(nameof(WriteCommandVersions), error);

            public static string GetOptionName(CliCommandOptionInfo option)
                => (string)_pt.InvokeStatic(nameof(GetOptionName), option);
        }
    }
}
