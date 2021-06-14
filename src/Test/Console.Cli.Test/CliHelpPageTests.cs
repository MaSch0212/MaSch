using MaSch.Console.Cli.Configuration;
using MaSch.Console.Cli.Runtime;
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

namespace MaSch.Console.Cli.Test.Help
{
    [TestClass]
    public class CliHelpPageTests : TestClassBase
    {
        private static string NL => Environment.NewLine;

        private Mock<ICliApplicationBase> AppMock => Cache.GetValue(() => new Mock<ICliApplicationBase>(MockBehavior.Strict))!;
        private Mock<IConsoleService> ConsoleServiceMock => Cache.GetValue(() => new Mock<IConsoleService>(MockBehavior.Strict))!;
        private PrivateCliHelpPage HelpPage => Cache.GetValue(() => new PrivateCliHelpPage(ConsoleServiceMock.Object))!;
        private ICliCommandInfoFactory Factory => Cache.GetValue(() => new CliCommandInfoFactory())!;

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

            HelpPage.WriteCommandNameAndVersion(AppMock.Object, new CliError("blub"));

            Assert.AreEqual($"MyName MyVersion{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCommandNameAndVersion_CommandSpecific()
        {
            var commandMock = Mocks.Create<ICliCommandInfo>();
            commandMock.Setup(x => x.DisplayName).Returns("YourName");
            commandMock.Setup(x => x.Version).Returns("YourVersion");
            AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { Name = "MyName", Version = "MyVersion" });
            var sb = AttachStringBuilder(ConsoleServiceMock);

            HelpPage.WriteCommandNameAndVersion(AppMock.Object, new CliError("blub", commandMock.Object));

            Assert.AreEqual($"YourName YourVersion{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCopyright_FancyConsole()
        {
            AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { Year = "1337", Author = "Me" });
            ConsoleServiceMock.Setup(x => x.IsFancyConsole).Returns(true);
            var sb = AttachStringBuilder(ConsoleServiceMock);

            HelpPage.WriteCopyright(AppMock.Object, new CliError("blub"));

            Assert.AreEqual($"Copyright © 1337 Me{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCopyright_NonFancyConsole()
        {
            AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { Year = "1337", Author = "Me" });
            ConsoleServiceMock.Setup(x => x.IsFancyConsole).Returns(false);
            var sb = AttachStringBuilder(ConsoleServiceMock);

            HelpPage.WriteCopyright(AppMock.Object, new CliError("blub"));

            Assert.AreEqual($"Copyright (C) 1337 Me{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCopyright_CommandSpecific()
        {
            var commandMock = Mocks.Create<ICliCommandInfo>();
            commandMock.Setup(x => x.Year).Returns("4711");
            commandMock.Setup(x => x.Author).Returns("You");
            AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { Year = "1337", Author = "Me" });
            ConsoleServiceMock.Setup(x => x.IsFancyConsole).Returns(true);
            var sb = AttachStringBuilder(ConsoleServiceMock);

            HelpPage.WriteCopyright(AppMock.Object, new CliError("blub", commandMock.Object));

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
            var helpPageMock = Mocks.Create<CliHelpPage>(ConsoleServiceMock.Object);
            var app = Mocks.Create<ICliApplicationBase>().Object;
            var error = new CliError("Blub");
            var callOrder = new List<string>();
            helpPageMock.Protected().Setup("WriteVersionPage", app, error).CallBase();
            helpPageMock.Protected().Setup("WriteCommandNameAndVersion", app, error).Callback(() => callOrder.Add("CommandNameAndVersion"));
            helpPageMock.Protected().Setup("WriteCopyright", app, error).Callback(() => callOrder.Add("Copyright"));
            helpPageMock.Protected().Setup("WriteCommandVersions", app, error).Callback(() => callOrder.Add("CommandVersions"));

            new PrivateCliHelpPage(helpPageMock.Object).WriteVersionPage(app, error);

            Assert.AreCollectionsEqual(new[] { "CommandNameAndVersion", "Copyright", "CommandVersions" }, callOrder);
        }

        [TestMethod]
        public void WriteHelpPage()
        {
            var helpPageMock = Mocks.Create<CliHelpPage>(ConsoleServiceMock.Object);
            var app = Mocks.Create<ICliApplicationBase>().Object;
            var error = new CliError("Blub");
            var callOrder = new List<string>();
            helpPageMock.Protected().Setup("WriteHelpPage", app, error).CallBase();
            helpPageMock.Protected().Setup("WriteCommandNameAndVersion", app, error).Callback(() => callOrder.Add("CommandNameAndVersion"));
            helpPageMock.Protected().Setup("WriteCopyright", app, error).Callback(() => callOrder.Add("Copyright"));
            helpPageMock.Protected().Setup("WriteCommandUsage", app, error).Callback(() => callOrder.Add("CommandUsage"));
            helpPageMock.Protected().Setup("WriteCommandParameters", app, error).Callback(() => callOrder.Add("CommandParameters"));
            helpPageMock.Protected().Setup("WriteCommands", app, error).Callback(() => callOrder.Add("Commands"));
            ConsoleServiceMock.Setup(x => x.WriteLine(string.Empty)).Callback(() => callOrder.Add(string.Empty));

            new PrivateCliHelpPage(helpPageMock.Object).WriteHelpPage(app, error);

            Assert.AreCollectionsEqual(
                new[]
                {
                    "CommandNameAndVersion",
                    "Copyright",
                    string.Empty,
                    "CommandUsage",
                    "CommandParameters",
                    "Commands",
                },
                callOrder);
        }

        [TestMethod]
        public void WriteErrorPage()
        {
            var helpPageMock = Mocks.Create<CliHelpPage>(ConsoleServiceMock.Object);
            var app = Mocks.Create<ICliApplicationBase>().Object;
            var errors = new[] { new CliError("Blub1"), new CliError("Blub2"), new CliError("Blub3") };
            var callOrder = new List<string>();
            helpPageMock.Protected().Setup("WriteErrorPage", app, errors).CallBase();
            helpPageMock.Protected().Setup("WriteCommandNameAndVersion", app, errors[0]).Callback(() => callOrder.Add("CommandNameAndVersion"));
            helpPageMock.Protected().Setup("WriteCopyright", app, errors[0]).Callback(() => callOrder.Add("Copyright"));
            helpPageMock.Protected().Setup("WriteCommandUsage", app, errors[0]).Callback(() => callOrder.Add("CommandUsage"));
            helpPageMock.Protected().Setup("WriteCommandParameters", app, errors[0]).Callback(() => callOrder.Add("CommandParameters"));
            helpPageMock.Protected().Setup("WriteCommands", app, errors[0]).Callback(() => callOrder.Add("Commands"));
            helpPageMock.Protected().Setup("WriteErrorMessage", app, ItExpr.IsAny<CliError>()).Callback<ICliApplicationBase, CliError>((a, e) => callOrder.Add($"ErrorMessage_{e.CustomErrorMessage}"));
            ConsoleServiceMock.Setup(x => x.WriteLine(string.Empty)).Callback(() => callOrder.Add(string.Empty));

            new PrivateCliHelpPage(helpPageMock.Object).WriteErrorPage(app, errors);

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
                },
                callOrder);
        }

        [TestMethod]
        public void Write_NullApp()
        {
            var helpPageMock = Mocks.Create<CliHelpPage>(ConsoleServiceMock.Object);
            var errors = new[] { new CliError("My Error") };
            helpPageMock.Setup(x => x.Write(It.IsAny<ICliApplicationBase>(), It.IsAny<IEnumerable<CliError>>())).CallBase();

            Assert.ThrowsException<ArgumentNullException>(() => helpPageMock.Object.Write(null!, errors));
        }

        [TestMethod]
        public void Write_NullErrors()
        {
            var helpPageMock = Mocks.Create<CliHelpPage>(ConsoleServiceMock.Object);
            var app = Mocks.Create<ICliApplicationBase>().Object;
            IList<CliError>? errors = null;
            helpPageMock.Setup(x => x.Write(It.IsAny<ICliApplicationBase>(), It.IsAny<IEnumerable<CliError>>())).CallBase();
            helpPageMock.Protected().Setup("WriteErrorPage", app, ItExpr.IsAny<IList<CliError>>()).Callback<ICliApplicationBase, IList<CliError>>((a, e) => errors = e);

            var result = helpPageMock.Object.Write(app, null);

            Assert.IsFalse(result);
            Assert.IsNotNull(errors);
            Assert.AreCollectionsEqual(new[] { CliErrorType.Unknown }, errors.Select(x => x.Type));
        }

        [TestMethod]
        public void Write_NoErrors()
        {
            var helpPageMock = Mocks.Create<CliHelpPage>(ConsoleServiceMock.Object);
            var app = Mocks.Create<ICliApplicationBase>().Object;
            IList<CliError>? errors = null;
            helpPageMock.Setup(x => x.Write(It.IsAny<ICliApplicationBase>(), It.IsAny<IEnumerable<CliError>>())).CallBase();
            helpPageMock.Protected().Setup("WriteErrorPage", app, ItExpr.IsAny<IList<CliError>>()).Callback<ICliApplicationBase, IList<CliError>>((a, e) => errors = e);

            var result = helpPageMock.Object.Write(app, Array.Empty<CliError>());

            Assert.IsFalse(result);
            Assert.IsNotNull(errors);
            Assert.AreCollectionsEqual(new[] { CliErrorType.Unknown }, errors.Select(x => x.Type));
        }

        [TestMethod]
        public void Write_WithNullError()
        {
            var helpPageMock = Mocks.Create<CliHelpPage>(ConsoleServiceMock.Object);
            var app = Mocks.Create<ICliApplicationBase>().Object;
            IList<CliError>? errors = null;
            helpPageMock.Setup(x => x.Write(It.IsAny<ICliApplicationBase>(), It.IsAny<IEnumerable<CliError>>())).CallBase();
            helpPageMock.Protected().Setup("WriteErrorPage", app, ItExpr.IsAny<IList<CliError>>()).Callback<ICliApplicationBase, IList<CliError>>((a, e) => errors = e);

            var result = helpPageMock.Object.Write(app, new[] { (CliError)null! });

            Assert.IsFalse(result);
            Assert.IsNotNull(errors);
            Assert.AreCollectionsEqual(new[] { CliErrorType.Unknown }, errors.Select(x => x.Type));
        }

        [TestMethod]
        public void Write_HasVersionRequested()
        {
            var helpPageMock = Mocks.Create<CliHelpPage>(ConsoleServiceMock.Object);
            var app = Mocks.Create<ICliApplicationBase>().Object;
            var errors = new[] { new CliError("gjklhf"), new CliError(CliErrorType.VersionRequested) };
            helpPageMock.Setup(x => x.Write(app, errors)).CallBase();
            helpPageMock.Protected().Setup("WriteVersionPage", app, errors[1]).Verifiable(Verifiables, Times.Once());

            var result = helpPageMock.Object.Write(app, errors);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Write_HelpRequested()
        {
            var helpPageMock = Mocks.Create<CliHelpPage>(ConsoleServiceMock.Object);
            var app = Mocks.Create<ICliApplicationBase>().Object;
            var errors = new[] { new CliError("gjklhf"), new CliError(CliErrorType.VersionRequested), new CliError(CliErrorType.HelpRequested) };
            helpPageMock.Setup(x => x.Write(app, errors)).CallBase();
            helpPageMock.Protected().Setup("WriteHelpPage", app, errors[2]).Verifiable(Verifiables, Times.Once());

            var result = helpPageMock.Object.Write(app, errors);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Write_OtherErrors()
        {
            var helpPageMock = Mocks.Create<CliHelpPage>(ConsoleServiceMock.Object);
            var app = Mocks.Create<ICliApplicationBase>().Object;
            var errors = new[] { new CliError("gjklhf") };
            helpPageMock.Setup(x => x.Write(app, errors)).CallBase();
            helpPageMock.Protected().Setup("WriteErrorPage", app, errors).Verifiable(Verifiables, Times.Once());

            var result = helpPageMock.Object.Write(app, errors);

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

            HelpPage.WriteCommandUsage(AppMock.Object, new CliError("MyError"));

            Assert.AreEqual($"Usage: MyProgram{new string(' ', 50 - 16 - 1)}{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCommandUsage_NoAffectedCommand_WithCommands()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var collectionMock = Mocks.Create<IReadOnlyCliCommandInfoCollection>();
            var command1Mock = Mocks.Create<ICliCommandInfo>();
            var command2Mock = Mocks.Create<ICliCommandInfo>();
            collectionMock.Setup(x => x.GetRootCommands()).Returns(new[] { command1Mock.Object, command2Mock.Object });
            AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { CliName = "MyProgram" });
            AppMock.Setup(x => x.Commands).Returns(collectionMock.Object);
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));

            HelpPage.WriteCommandUsage(AppMock.Object, new CliError("MyError"));

            Assert.AreEqual($"Usage: MyProgram <command>{new string(' ', 50 - 26 - 1)}{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCommandUsage_WithAffectedCommand_OneAlias_NoParentCommand_NoChildCommands_NoOptions_NoValues()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var aCommandMock = Mocks.Create<ICliCommandInfo>();
            aCommandMock.Setup(x => x.Name).Returns("blub");
            aCommandMock.Setup(x => x.Aliases).Returns(new[] { "blub" });
            aCommandMock.Setup(x => x.ParentCommand).Returns((ICliCommandInfo?)null);
            aCommandMock.Setup(x => x.ChildCommands).Returns(Array.Empty<ICliCommandInfo>());
            aCommandMock.Setup(x => x.Options).Returns(Array.Empty<ICliCommandOptionInfo>());
            aCommandMock.Setup(x => x.Values).Returns(Array.Empty<ICliCommandValueInfo>());
            AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { CliName = "MyProgram" });
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));

            HelpPage.WriteCommandUsage(AppMock.Object, new CliError("MyError", aCommandMock.Object));

            Assert.AreEqual($"Usage: MyProgram blub{new string(' ', 50 - 21 - 1)}{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCommandUsage_WithAffectedCommand_MultipleAliases_NoParentCommand_NoChildCommands_NoOptions_NoValues()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var aCommandMock = Mocks.Create<ICliCommandInfo>();
            aCommandMock.Setup(x => x.Aliases).Returns(new[] { "blub", "blib", "blab" });
            aCommandMock.Setup(x => x.ParentCommand).Returns((ICliCommandInfo?)null);
            aCommandMock.Setup(x => x.ChildCommands).Returns(Array.Empty<ICliCommandInfo>());
            aCommandMock.Setup(x => x.Options).Returns(Array.Empty<ICliCommandOptionInfo>());
            aCommandMock.Setup(x => x.Values).Returns(Array.Empty<ICliCommandValueInfo>());
            AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { CliName = "MyProgram" });
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));

            HelpPage.WriteCommandUsage(AppMock.Object, new CliError("MyError", aCommandMock.Object));

            Assert.AreEqual($"Usage: MyProgram (blub|blib|blab){new string(' ', 50 - 33 - 1)}{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCommandUsage_WithAffectedCommand_OneAlias_WithParentCommand_NoChildCommands_NoOptions_NoValues()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var ppCommandMock = Mocks.Create<ICliCommandInfo>();
            var pCommandMock = Mocks.Create<ICliCommandInfo>();
            var aCommandMock = Mocks.Create<ICliCommandInfo>();
            ppCommandMock.Setup(x => x.Aliases).Returns(new[] { "pp", "pi" });
            ppCommandMock.Setup(x => x.ParentCommand).Returns((ICliCommandInfo?)null);
            pCommandMock.Setup(x => x.Name).Returns("p");
            pCommandMock.Setup(x => x.Aliases).Returns(new[] { "p" });
            pCommandMock.Setup(x => x.ParentCommand).Returns(ppCommandMock.Object);
            aCommandMock.Setup(x => x.Name).Returns("blub");
            aCommandMock.Setup(x => x.Aliases).Returns(new[] { "blub" });
            aCommandMock.Setup(x => x.ParentCommand).Returns(pCommandMock.Object);
            aCommandMock.Setup(x => x.ChildCommands).Returns(Array.Empty<ICliCommandInfo>());
            aCommandMock.Setup(x => x.Options).Returns(Array.Empty<ICliCommandOptionInfo>());
            aCommandMock.Setup(x => x.Values).Returns(Array.Empty<ICliCommandValueInfo>());
            AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { CliName = "MyProgram" });
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));

            HelpPage.WriteCommandUsage(AppMock.Object, new CliError("MyError", aCommandMock.Object));

            Assert.AreEqual($"Usage: MyProgram (pp|pi) p blub{new string(' ', 50 - 31 - 1)}{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCommandUsage_WithAffectedCommand_OneAlias_NoParentCommand_Executable_WithChildCommands_NoOptions_NoValues()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var aCommandMock = Mocks.Create<ICliCommandInfo>();
            var command1Mock = Mocks.Create<ICliCommandInfo>();
            var command2Mock = Mocks.Create<ICliCommandInfo>();
            aCommandMock.Setup(x => x.Name).Returns("blub");
            aCommandMock.Setup(x => x.Aliases).Returns(new[] { "blub" });
            aCommandMock.Setup(x => x.ParentCommand).Returns((ICliCommandInfo?)null);
            aCommandMock.Setup(x => x.ChildCommands).Returns(new[] { command1Mock.Object, command2Mock.Object });
            aCommandMock.Setup(x => x.Options).Returns(Array.Empty<ICliCommandOptionInfo>());
            aCommandMock.Setup(x => x.Values).Returns(Array.Empty<ICliCommandValueInfo>());
            aCommandMock.Setup(x => x.IsExecutable).Returns(true);
            AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { CliName = "MyProgram" });
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));

            HelpPage.WriteCommandUsage(AppMock.Object, new CliError("MyError", aCommandMock.Object));

            Assert.AreEqual($"Usage: MyProgram blub [command]{new string(' ', 50 - 31 - 1)}{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCommandUsage_WithAffectedCommand_OneAlias_NoParentCommand_NonExecutable_WithChildCommands_NoOptions_NoValues()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var aCommandMock = Mocks.Create<ICliCommandInfo>();
            var command1Mock = Mocks.Create<ICliCommandInfo>();
            var command2Mock = Mocks.Create<ICliCommandInfo>();
            aCommandMock.Setup(x => x.Name).Returns("blub");
            aCommandMock.Setup(x => x.Aliases).Returns(new[] { "blub" });
            aCommandMock.Setup(x => x.ParentCommand).Returns((ICliCommandInfo?)null);
            aCommandMock.Setup(x => x.ChildCommands).Returns(new[] { command1Mock.Object, command2Mock.Object });
            aCommandMock.Setup(x => x.Options).Returns(Array.Empty<ICliCommandOptionInfo>());
            aCommandMock.Setup(x => x.Values).Returns(Array.Empty<ICliCommandValueInfo>());
            aCommandMock.Setup(x => x.IsExecutable).Returns(false);
            AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { CliName = "MyProgram" });
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));

            HelpPage.WriteCommandUsage(AppMock.Object, new CliError("MyError", aCommandMock.Object));

            Assert.AreEqual($"Usage: MyProgram blub <command>{new string(' ', 50 - 31 - 1)}{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCommandUsage_WithAffectedCommand_OneAlias_NoParentCommand_NoChildCommands_WithOptions_NoValues()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var aCommandMock = Mocks.Create<ICliCommandInfo>();
            var option1Mock = Mocks.Create<ICliCommandOptionInfo>();
            var option2Mock = Mocks.Create<ICliCommandOptionInfo>();
            aCommandMock.Setup(x => x.Name).Returns("blub");
            aCommandMock.Setup(x => x.Aliases).Returns(new[] { "blub" });
            aCommandMock.Setup(x => x.ParentCommand).Returns((ICliCommandInfo?)null);
            aCommandMock.Setup(x => x.ChildCommands).Returns(Array.Empty<ICliCommandInfo>());
            aCommandMock.Setup(x => x.Options).Returns(new[] { option1Mock.Object, option2Mock.Object });
            aCommandMock.Setup(x => x.Values).Returns(Array.Empty<ICliCommandValueInfo>());
            AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { CliName = "MyProgram" });
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));

            HelpPage.WriteCommandUsage(AppMock.Object, new CliError("MyError", aCommandMock.Object));

            Assert.AreEqual($"Usage: MyProgram blub [options]{new string(' ', 50 - 31 - 1)}{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCommandUsage_WithAffectedCommand_OneAlias_NoParentCommand_NoChildCommands_NoOptions_WithValues()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var aCommandMock = Mocks.Create<ICliCommandInfo>();
            var value1Mock = Mocks.Create<ICliCommandValueInfo>();
            var value2Mock = Mocks.Create<ICliCommandValueInfo>();
            aCommandMock.Setup(x => x.Name).Returns("blub");
            aCommandMock.Setup(x => x.Aliases).Returns(new[] { "blub" });
            aCommandMock.Setup(x => x.ParentCommand).Returns((ICliCommandInfo?)null);
            aCommandMock.Setup(x => x.ChildCommands).Returns(Array.Empty<ICliCommandInfo>());
            aCommandMock.Setup(x => x.Options).Returns(Array.Empty<ICliCommandOptionInfo>());
            aCommandMock.Setup(x => x.Values).Returns(new[] { value1Mock.Object, value2Mock.Object });
            value1Mock.Setup(x => x.Order).Returns(2);
            value1Mock.Setup(x => x.DisplayName).Returns("Val1");
            value1Mock.Setup(x => x.IsRequired).Returns(false);
            value2Mock.Setup(x => x.Order).Returns(1);
            value2Mock.Setup(x => x.DisplayName).Returns("Val2");
            value2Mock.Setup(x => x.IsRequired).Returns(true);
            AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { CliName = "MyProgram" });
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));

            HelpPage.WriteCommandUsage(AppMock.Object, new CliError("MyError", aCommandMock.Object));

            Assert.AreEqual($"Usage: MyProgram blub <Val2> [Val1]{new string(' ', 50 - 35 - 1)}{NL}", sb.ToString());
        }

        [TestMethod]
        public void WriteCommandUsage_Everything()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var ppCommandMock = Mocks.Create<ICliCommandInfo>();
            var pCommandMock = Mocks.Create<ICliCommandInfo>();
            var aCommandMock = Mocks.Create<ICliCommandInfo>();
            var command1Mock = Mocks.Create<ICliCommandInfo>();
            var command2Mock = Mocks.Create<ICliCommandInfo>();
            var value1Mock = Mocks.Create<ICliCommandValueInfo>();
            var value2Mock = Mocks.Create<ICliCommandValueInfo>();
            var option1Mock = Mocks.Create<ICliCommandOptionInfo>();
            var option2Mock = Mocks.Create<ICliCommandOptionInfo>();
            ppCommandMock.Setup(x => x.Aliases).Returns(new[] { "grandparent", "supervisor" });
            ppCommandMock.Setup(x => x.ParentCommand).Returns((ICliCommandInfo?)null);
            pCommandMock.Setup(x => x.Name).Returns("parent");
            pCommandMock.Setup(x => x.Aliases).Returns(new[] { "parent" });
            pCommandMock.Setup(x => x.ParentCommand).Returns(ppCommandMock.Object);
            aCommandMock.Setup(x => x.Aliases).Returns(new[] { "blub", "blib", "blab" });
            aCommandMock.Setup(x => x.ParentCommand).Returns(pCommandMock.Object);
            aCommandMock.Setup(x => x.ChildCommands).Returns(new[] { command1Mock.Object, command2Mock.Object });
            aCommandMock.Setup(x => x.Options).Returns(new[] { option1Mock.Object, option2Mock.Object });
            aCommandMock.Setup(x => x.Values).Returns(new[] { value1Mock.Object, value2Mock.Object });
            aCommandMock.Setup(x => x.IsExecutable).Returns(true);
            value1Mock.Setup(x => x.Order).Returns(2);
            value1Mock.Setup(x => x.DisplayName).Returns("Val1");
            value1Mock.Setup(x => x.IsRequired).Returns(false);
            value2Mock.Setup(x => x.Order).Returns(1);
            value2Mock.Setup(x => x.DisplayName).Returns("Val2");
            value2Mock.Setup(x => x.IsRequired).Returns(true);
            AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { CliName = "MyProgram" });
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));

            HelpPage.WriteCommandUsage(AppMock.Object, new CliError("MyError", aCommandMock.Object));

            Assert.AreEqual(
                $"Usage: MyProgram (grandparent|supervisor) parent {NL}" +
                $"       (blub|blib|blab) [command] <Val2> [Val1]  {NL}" +
                $"       [options]                                 {NL}",
                sb.ToString());
        }

        [TestMethod]
        public void WriteCommandParameters_NoAffectedCommand()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);

            HelpPage.WriteCommandParameters(AppMock.Object, new CliError("MyError"));

            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [TestMethod]
        public void WriteCommandParameters_NoValues_NoOptions()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var aCommandMock = Mocks.Create<ICliCommandInfo>();
            aCommandMock.Setup(x => x.Options).Returns(Array.Empty<ICliCommandOptionInfo>());
            aCommandMock.Setup(x => x.Values).Returns(Array.Empty<ICliCommandValueInfo>());

            HelpPage.WriteCommandParameters(AppMock.Object, new CliError("MyError", aCommandMock.Object));

            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [TestMethod]
        public void WriteCommandParameters_NoVisibleValues_NoVisibleOptions()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var aCommandMock = Mocks.Create<ICliCommandInfo>();
            var value1Mock = Mocks.Create<ICliCommandValueInfo>();
            var option1Mock = Mocks.Create<ICliCommandOptionInfo>();
            value1Mock.Setup(x => x.Hidden).Returns(true);
            option1Mock.Setup(x => x.Hidden).Returns(true);
            aCommandMock.Setup(x => x.Options).Returns(new[] { option1Mock.Object });
            aCommandMock.Setup(x => x.Values).Returns(new[] { value1Mock.Object });
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));
            ConsoleServiceMock.Setup(x => x.IsOutputRedirected).Returns(false);

            HelpPage.WriteCommandParameters(AppMock.Object, new CliError("MyError", aCommandMock.Object));

            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [TestMethod]
        public void WriteCommandParameters_WithValues_NoOptions()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var aCommandMock = Mocks.Create<ICliCommandInfo>();
            var value1Mock = Mocks.Create<ICliCommandValueInfo>();
            var value2Mock = Mocks.Create<ICliCommandValueInfo>();
            value1Mock.Setup(x => x.Order).Returns(2);
            value1Mock.Setup(x => x.DisplayName).Returns("Val1");
            value1Mock.Setup(x => x.HelpText).Returns("This is my help text for value 1.");
            value1Mock.Setup(x => x.IsRequired).Returns(false);
            value1Mock.Setup(x => x.Hidden).Returns(false);
            value2Mock.Setup(x => x.Order).Returns(1);
            value2Mock.Setup(x => x.DisplayName).Returns("Val2");
            value2Mock.Setup(x => x.HelpText).Returns("This is my very longer help text for value 2 to test the wrapping.");
            value2Mock.Setup(x => x.IsRequired).Returns(true);
            value2Mock.Setup(x => x.Hidden).Returns(false);
            aCommandMock.Setup(x => x.Options).Returns(Array.Empty<ICliCommandOptionInfo>());
            aCommandMock.Setup(x => x.Values).Returns(new[] { value1Mock.Object, value2Mock.Object });
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));
            ConsoleServiceMock.Setup(x => x.IsOutputRedirected).Returns(false);

            HelpPage.WriteCommandParameters(AppMock.Object, new CliError("MyError", aCommandMock.Object));

            Assert.AreEqual(
                $"{NL}Values:{NL}" +
                $"   Val2   This is my very longer help text for   {NL}" +
                $"          value 2 to test the wrapping.          {NL}" +
                $"   Val1   This is my help text for value 1.      {NL}",
                sb.ToString());
        }

        [TestMethod]
        public void WriteCommandParameters_NoValues_WithRequiredOptions()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var aCommandMock = Mocks.Create<ICliCommandInfo>();
            var option1Mock = Mocks.Create<ICliCommandOptionInfo>();
            var option2Mock = Mocks.Create<ICliCommandOptionInfo>();
            var option3Mock = Mocks.Create<ICliCommandOptionInfo>();
            option1Mock.Setup(x => x.ShortAliases).Returns(new[] { 'a', 'b' });
            option1Mock.Setup(x => x.Aliases).Returns(new[] { "blub" });
            option1Mock.Setup(x => x.HelpOrder).Returns(-1);
            option1Mock.Setup(x => x.HelpText).Returns("Help text for option1.");
            option1Mock.Setup(x => x.IsRequired).Returns(true);
            option1Mock.Setup(x => x.Hidden).Returns(false);
            option2Mock.Setup(x => x.ShortAliases).Returns(Array.Empty<char>());
            option2Mock.Setup(x => x.Aliases).Returns(new[] { "blib" });
            option2Mock.Setup(x => x.HelpOrder).Returns(0);
            option2Mock.Setup(x => x.HelpText).Returns("Help text for option2.");
            option2Mock.Setup(x => x.IsRequired).Returns(true);
            option2Mock.Setup(x => x.Hidden).Returns(false);
            option3Mock.Setup(x => x.ShortAliases).Returns(new[] { 'A' });
            option3Mock.Setup(x => x.Aliases).Returns(new[] { "blab", "blob" });
            option3Mock.Setup(x => x.HelpOrder).Returns(0);
            option3Mock.Setup(x => x.HelpText).Returns("Help text for option3. This has longer text to test wrapping.");
            option3Mock.Setup(x => x.IsRequired).Returns(true);
            option3Mock.Setup(x => x.Hidden).Returns(false);
            aCommandMock.Setup(x => x.Options).Returns(new[] { option1Mock.Object, option2Mock.Object, option3Mock.Object });
            aCommandMock.Setup(x => x.Values).Returns(Array.Empty<ICliCommandValueInfo>());
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));
            ConsoleServiceMock.Setup(x => x.IsOutputRedirected).Returns(false);

            HelpPage.WriteCommandParameters(AppMock.Object, new CliError("MyError", aCommandMock.Object));

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
            var aCommandMock = Mocks.Create<ICliCommandInfo>();
            var option1Mock = Mocks.Create<ICliCommandOptionInfo>();
            var option2Mock = Mocks.Create<ICliCommandOptionInfo>();
            var option3Mock = Mocks.Create<ICliCommandOptionInfo>();
            option1Mock.Setup(x => x.ShortAliases).Returns(new[] { 'a', 'b' });
            option1Mock.Setup(x => x.Aliases).Returns(new[] { "blub" });
            option1Mock.Setup(x => x.HelpOrder).Returns(-1);
            option1Mock.Setup(x => x.HelpText).Returns("Help text for option1.");
            option1Mock.Setup(x => x.IsRequired).Returns(false);
            option1Mock.Setup(x => x.Hidden).Returns(false);
            option2Mock.Setup(x => x.ShortAliases).Returns(Array.Empty<char>());
            option2Mock.Setup(x => x.Aliases).Returns(new[] { "blib" });
            option2Mock.Setup(x => x.HelpOrder).Returns(0);
            option2Mock.Setup(x => x.HelpText).Returns("Help text for option2.");
            option2Mock.Setup(x => x.IsRequired).Returns(false);
            option2Mock.Setup(x => x.Hidden).Returns(false);
            option3Mock.Setup(x => x.ShortAliases).Returns(new[] { 'A' });
            option3Mock.Setup(x => x.Aliases).Returns(new[] { "blab", "blob" });
            option3Mock.Setup(x => x.HelpOrder).Returns(0);
            option3Mock.Setup(x => x.HelpText).Returns("Help text for option3. This has longer text to test wrapping.");
            option3Mock.Setup(x => x.IsRequired).Returns(false);
            option3Mock.Setup(x => x.Hidden).Returns(false);
            aCommandMock.Setup(x => x.Options).Returns(new[] { option1Mock.Object, option2Mock.Object, option3Mock.Object });
            aCommandMock.Setup(x => x.Values).Returns(Array.Empty<ICliCommandValueInfo>());
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));
            ConsoleServiceMock.Setup(x => x.IsOutputRedirected).Returns(false);

            HelpPage.WriteCommandParameters(AppMock.Object, new CliError("MyError", aCommandMock.Object));

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
            var aCommandMock = Mocks.Create<ICliCommandInfo>();
            var value1Mock = Mocks.Create<ICliCommandValueInfo>();
            var option1Mock = Mocks.Create<ICliCommandOptionInfo>();
            var option2Mock = Mocks.Create<ICliCommandOptionInfo>();
            value1Mock.Setup(x => x.Order).Returns(1);
            value1Mock.Setup(x => x.DisplayName).Returns("Val1");
            value1Mock.Setup(x => x.HelpText).Returns("This is my very longer help text for value 1 to test the wrapping.");
            value1Mock.Setup(x => x.IsRequired).Returns(true);
            value1Mock.Setup(x => x.Hidden).Returns(false);
            option1Mock.Setup(x => x.ShortAliases).Returns(new[] { 'A' });
            option1Mock.Setup(x => x.Aliases).Returns(new[] { "blab", "blob" });
            option1Mock.Setup(x => x.HelpOrder).Returns(0);
            option1Mock.Setup(x => x.HelpText).Returns("Help text for optional. This has longer text to test wrapping.");
            option1Mock.Setup(x => x.IsRequired).Returns(false);
            option1Mock.Setup(x => x.Hidden).Returns(false);
            option2Mock.Setup(x => x.ShortAliases).Returns(new[] { 'A' });
            option2Mock.Setup(x => x.Aliases).Returns(new[] { "blab", "blob" });
            option2Mock.Setup(x => x.HelpOrder).Returns(0);
            option2Mock.Setup(x => x.HelpText).Returns("Help text for required. This has longer text to test wrapping.");
            option2Mock.Setup(x => x.IsRequired).Returns(true);
            option2Mock.Setup(x => x.Hidden).Returns(false);
            aCommandMock.Setup(x => x.Options).Returns(new[] { option1Mock.Object, option2Mock.Object });
            aCommandMock.Setup(x => x.Values).Returns(new[] { value1Mock.Object });
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(50, int.MaxValue));
            ConsoleServiceMock.Setup(x => x.IsOutputRedirected).Returns(false);

            HelpPage.WriteCommandParameters(AppMock.Object, new CliError("MyError", aCommandMock.Object));

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

            HelpPage.WriteCommands(AppMock.Object, new CliError("MyError"));

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

            HelpPage.WriteCommands(AppMock.Object, new CliError("MyError"));

            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [TestMethod]
        public void WriteCommands_NoAffectedCommand_WithCommands()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var collectionMock = Mocks.Create<IReadOnlyCliCommandInfoCollection>();
            var command1Mock = Mocks.Create<ICliCommandInfo>();
            var command2Mock = Mocks.Create<ICliCommandInfo>();
            var command3Mock = Mocks.Create<ICliCommandInfo>();
            var command4Mock = Mocks.Create<ICliCommandInfo>();
            command1Mock.Setup(x => x.IsDefault).Returns(false);
            command1Mock.Setup(x => x.Order).Returns(0);
            command1Mock.Setup(x => x.Name).Returns("zorro");
            command1Mock.Setup(x => x.Aliases).Returns(new[] { "zorro", "lorro" });
            command1Mock.Setup(x => x.HelpText).Returns("This should be the last command in the list.");
            command1Mock.Setup(x => x.Hidden).Returns(false);
            command2Mock.Setup(x => x.IsDefault).Returns(true);
            command2Mock.Setup(x => x.Order).Returns(5);
            command2Mock.Setup(x => x.Name).Returns("list");
            command2Mock.Setup(x => x.Aliases).Returns(new[] { "list" });
            command2Mock.Setup(x => x.HelpText).Returns("The is the default command and should be at the top of the list.");
            command2Mock.Setup(x => x.Hidden).Returns(false);
            command3Mock.Setup(x => x.IsDefault).Returns(false);
            command3Mock.Setup(x => x.Order).Returns(-2);
            command3Mock.Setup(x => x.Name).Returns("zzzzz");
            command3Mock.Setup(x => x.Aliases).Returns(new[] { "zzzzz" });
            command3Mock.Setup(x => x.HelpText).Returns("Should be second command in list.");
            command3Mock.Setup(x => x.Hidden).Returns(false);
            command4Mock.Setup(x => x.IsDefault).Returns(false);
            command4Mock.Setup(x => x.Order).Returns(0);
            command4Mock.Setup(x => x.Name).Returns("ape");
            command4Mock.Setup(x => x.Aliases).Returns(new[] { "ape" });
            command4Mock.Setup(x => x.HelpText).Returns("Should be third command in list.");
            command4Mock.Setup(x => x.Hidden).Returns(false);
            collectionMock.Setup(x => x.GetRootCommands()).Returns(new[] { command1Mock.Object, command2Mock.Object, command3Mock.Object, command4Mock.Object });
            AppMock.Setup(x => x.Commands).Returns(collectionMock.Object);
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(55, int.MaxValue));
            ConsoleServiceMock.Setup(x => x.IsOutputRedirected).Returns(false);

            HelpPage.WriteCommands(AppMock.Object, new CliError("MyError"));

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
            var aCommandMock = Mocks.Create<ICliCommandInfo>();
            aCommandMock.Setup(x => x.ChildCommands).Returns(Array.Empty<ICliCommandInfo>());

            HelpPage.WriteCommands(AppMock.Object, new CliError("MyError", aCommandMock.Object));

            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [TestMethod]
        public void WriteCommands_WithAffectedCommand_NoVisibleCommands()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var aCommandMock = Mocks.Create<ICliCommandInfo>();
            var command1Mock = Mocks.Create<ICliCommandInfo>();
            command1Mock.Setup(x => x.Hidden).Returns(true);
            aCommandMock.Setup(x => x.ChildCommands).Returns(new[] { command1Mock.Object });

            HelpPage.WriteCommands(AppMock.Object, new CliError("MyError", aCommandMock.Object));

            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [TestMethod]
        public void WriteCommands_WithAffectedCommand_WithCommands()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var aCommandMock = Mocks.Create<ICliCommandInfo>();
            var command1Mock = Mocks.Create<ICliCommandInfo>();
            var command2Mock = Mocks.Create<ICliCommandInfo>();
            var command3Mock = Mocks.Create<ICliCommandInfo>();
            var command4Mock = Mocks.Create<ICliCommandInfo>();
            command1Mock.Setup(x => x.IsDefault).Returns(false);
            command1Mock.Setup(x => x.Order).Returns(0);
            command1Mock.Setup(x => x.Name).Returns("zorro");
            command1Mock.Setup(x => x.Aliases).Returns(new[] { "zorro", "lorro" });
            command1Mock.Setup(x => x.HelpText).Returns("This should be the last command in the list.");
            command1Mock.Setup(x => x.Hidden).Returns(false);
            command2Mock.Setup(x => x.IsDefault).Returns(true);
            command2Mock.Setup(x => x.Order).Returns(5);
            command2Mock.Setup(x => x.Name).Returns("list");
            command2Mock.Setup(x => x.Aliases).Returns(new[] { "list" });
            command2Mock.Setup(x => x.HelpText).Returns("The is the default command and should be at the top of the list.");
            command2Mock.Setup(x => x.Hidden).Returns(false);
            command3Mock.Setup(x => x.IsDefault).Returns(false);
            command3Mock.Setup(x => x.Order).Returns(-2);
            command3Mock.Setup(x => x.Name).Returns("zzzzz");
            command3Mock.Setup(x => x.Aliases).Returns(new[] { "zzzzz" });
            command3Mock.Setup(x => x.HelpText).Returns("Should be second command in list.");
            command3Mock.Setup(x => x.Hidden).Returns(false);
            command4Mock.Setup(x => x.IsDefault).Returns(false);
            command4Mock.Setup(x => x.Order).Returns(0);
            command4Mock.Setup(x => x.Name).Returns("ape");
            command4Mock.Setup(x => x.Aliases).Returns(new[] { "ape" });
            command4Mock.Setup(x => x.HelpText).Returns("Should be third command in list.");
            command4Mock.Setup(x => x.Hidden).Returns(false);
            aCommandMock.Setup(x => x.ChildCommands).Returns(new[] { command1Mock.Object, command2Mock.Object, command3Mock.Object, command4Mock.Object });
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(55, int.MaxValue));
            ConsoleServiceMock.Setup(x => x.IsOutputRedirected).Returns(false);

            HelpPage.WriteCommands(AppMock.Object, new CliError("MyError", aCommandMock.Object));

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

            HelpPage.WriteCommandVersions(AppMock.Object, new CliError("MyError"));

            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [TestMethod]
        public void WriteCommandVersions_NoAffectedCommand_NoOverridingCommands()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var collectionMock = Mocks.Create<IReadOnlyCliCommandInfoCollection>();
            commandMock.Setup(x => x.Author).Returns((string?)null);
            commandMock.Setup(x => x.DisplayName).Returns((string?)null);
            commandMock.Setup(x => x.Year).Returns((string?)null);
            commandMock.Setup(x => x.Version).Returns((string?)null);
            commandMock.Setup(x => x.Hidden).Returns(false);
            collectionMock.Setup(x => x.GetRootCommands()).Returns(new[] { commandMock.Object });
            AppMock.Setup(x => x.Commands).Returns(collectionMock.Object);

            HelpPage.WriteCommandVersions(AppMock.Object, new CliError("MyError"));

            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [TestMethod]
        public void WriteCommandVersions_NoAffectedCommand_NoVisibleCommands()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var collectionMock = Mocks.Create<IReadOnlyCliCommandInfoCollection>();
            commandMock.Setup(x => x.Hidden).Returns(true);
            collectionMock.Setup(x => x.GetRootCommands()).Returns(new[] { commandMock.Object });
            AppMock.Setup(x => x.Commands).Returns(collectionMock.Object);

            HelpPage.WriteCommandVersions(AppMock.Object, new CliError("MyError"));

            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [TestMethod]
        [DataRow(false, DisplayName = "Without Fancy Console")]
        [DataRow(true, DisplayName = "With Fancy Console")]
        public void WriteCommandVersions_NoAffectedCommand_WithCommands(bool fancyConsole)
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var command1Mock = Mocks.Create<ICliCommandInfo>();
            var command2Mock = Mocks.Create<ICliCommandInfo>();
            var command3Mock = Mocks.Create<ICliCommandInfo>();
            var collectionMock = Mocks.Create<IReadOnlyCliCommandInfoCollection>();
            command1Mock.Setup(x => x.Author).Returns((string?)null);
            command1Mock.Setup(x => x.DisplayName).Returns((string?)null);
            command1Mock.Setup(x => x.Year).Returns((string?)null);
            command1Mock.Setup(x => x.Version).Returns((string?)null);
            command1Mock.Setup(x => x.Order).Returns(0);
            command1Mock.Setup(x => x.ParentCommand).Returns((ICliCommandInfo?)null);
            command1Mock.Setup(x => x.Hidden).Returns(false);
            command2Mock.Setup(x => x.Name).Returns("cmd2");
            command2Mock.Setup(x => x.Aliases).Returns(new[] { "cmd2" });
            command2Mock.Setup(x => x.Author).Returns("You");
            command2Mock.Setup(x => x.DisplayName).Returns((string?)null);
            command2Mock.Setup(x => x.Year).Returns("4711");
            command2Mock.Setup(x => x.Version).Returns((string?)null);
            command2Mock.Setup(x => x.Order).Returns(0);
            command2Mock.Setup(x => x.ParentCommand).Returns((ICliCommandInfo?)null);
            command2Mock.Setup(x => x.Hidden).Returns(false);
            command3Mock.Setup(x => x.Name).Returns("cmd3");
            command3Mock.Setup(x => x.Aliases).Returns(new[] { "cmd3", "command3", "my-command-3" });
            command3Mock.Setup(x => x.Author).Returns((string?)null);
            command3Mock.Setup(x => x.DisplayName).Returns("Yours");
            command3Mock.Setup(x => x.Year).Returns((string?)null);
            command3Mock.Setup(x => x.Version).Returns("99");
            command3Mock.Setup(x => x.Order).Returns(0);
            command3Mock.Setup(x => x.ParentCommand).Returns((ICliCommandInfo?)null);
            command3Mock.Setup(x => x.Hidden).Returns(false);
            collectionMock.Setup(x => x.GetRootCommands()).Returns(new[] { command1Mock.Object, command2Mock.Object, command3Mock.Object });
            AppMock.Setup(x => x.Commands).Returns(collectionMock.Object);
            AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { Name = "My", Year = "1337", Author = "Me", Version = "15" });
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(55, int.MaxValue));
            ConsoleServiceMock.Setup(x => x.IsOutputRedirected).Returns(false);
            ConsoleServiceMock.Setup(x => x.IsFancyConsole).Returns(fancyConsole);

            HelpPage.WriteCommandVersions(AppMock.Object, new CliError("MyError"));

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
            var aCommandMock = Mocks.Create<ICliCommandInfo>();
            aCommandMock.Setup(x => x.ChildCommands).Returns(Array.Empty<ICliCommandInfo>());

            HelpPage.WriteCommandVersions(AppMock.Object, new CliError("MyError", aCommandMock.Object));

            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [TestMethod]
        public void WriteCommandVersions_WithAffectedCommand_NoOverridingCommands()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var aCommandMock = Mocks.Create<ICliCommandInfo>();
            var commandMock = Mocks.Create<ICliCommandInfo>();
            commandMock.Setup(x => x.Author).Returns((string?)null);
            commandMock.Setup(x => x.DisplayName).Returns((string?)null);
            commandMock.Setup(x => x.Year).Returns((string?)null);
            commandMock.Setup(x => x.Version).Returns((string?)null);
            commandMock.Setup(x => x.Hidden).Returns(false);
            aCommandMock.Setup(x => x.ChildCommands).Returns(new[] { commandMock.Object });

            HelpPage.WriteCommandVersions(AppMock.Object, new CliError("MyError", aCommandMock.Object));

            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [TestMethod]
        public void WriteCommandVersions_WithAffectedCommand_NoVisibleCommands()
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var aCommandMock = Mocks.Create<ICliCommandInfo>();
            var commandMock = Mocks.Create<ICliCommandInfo>();
            commandMock.Setup(x => x.Hidden).Returns(true);
            aCommandMock.Setup(x => x.ChildCommands).Returns(new[] { commandMock.Object });

            HelpPage.WriteCommandVersions(AppMock.Object, new CliError("MyError", aCommandMock.Object));

            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [TestMethod]
        [DataRow(false, DisplayName = "Without Fancy Console")]
        [DataRow(true, DisplayName = "With Fancy Console")]
        public void WriteCommandVersions_WithAffectedCommand_WithCommands_FallbackToParentCommand(bool fancyConsole)
        {
            var sb = AttachStringBuilder(ConsoleServiceMock);
            var command1Mock = Mocks.Create<ICliCommandInfo>();
            var command2Mock = Mocks.Create<ICliCommandInfo>();
            var command3Mock = Mocks.Create<ICliCommandInfo>();
            var aCommandMock = Mocks.Create<ICliCommandInfo>();
            aCommandMock.Setup(x => x.Author).Returns("Other");
            aCommandMock.Setup(x => x.Year).Returns("0815");
            aCommandMock.Setup(x => x.Version).Returns("77");
            command1Mock.Setup(x => x.Author).Returns((string?)null);
            command1Mock.Setup(x => x.DisplayName).Returns((string?)null);
            command1Mock.Setup(x => x.Year).Returns((string?)null);
            command1Mock.Setup(x => x.Version).Returns((string?)null);
            command1Mock.Setup(x => x.Order).Returns(0);
            command1Mock.Setup(x => x.ParentCommand).Returns(aCommandMock.Object);
            command1Mock.Setup(x => x.Hidden).Returns(false);
            command2Mock.Setup(x => x.Name).Returns("cmd2");
            command2Mock.Setup(x => x.Aliases).Returns(new[] { "cmd2" });
            command2Mock.Setup(x => x.Author).Returns("You");
            command2Mock.Setup(x => x.DisplayName).Returns((string?)null);
            command2Mock.Setup(x => x.Year).Returns("4711");
            command2Mock.Setup(x => x.Version).Returns((string?)null);
            command2Mock.Setup(x => x.Order).Returns(0);
            command2Mock.Setup(x => x.ParentCommand).Returns(aCommandMock.Object);
            command2Mock.Setup(x => x.Hidden).Returns(false);
            command3Mock.Setup(x => x.Name).Returns("cmd3");
            command3Mock.Setup(x => x.Aliases).Returns(new[] { "cmd3", "command3", "my-command-3" });
            command3Mock.Setup(x => x.Author).Returns((string?)null);
            command3Mock.Setup(x => x.DisplayName).Returns("Yours");
            command3Mock.Setup(x => x.Year).Returns((string?)null);
            command3Mock.Setup(x => x.Version).Returns("99");
            command3Mock.Setup(x => x.Order).Returns(0);
            command3Mock.Setup(x => x.ParentCommand).Returns(aCommandMock.Object);
            command3Mock.Setup(x => x.Hidden).Returns(false);
            aCommandMock.Setup(x => x.ChildCommands).Returns(new[] { command1Mock.Object, command2Mock.Object, command3Mock.Object });
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(55, int.MaxValue));
            ConsoleServiceMock.Setup(x => x.IsOutputRedirected).Returns(false);
            ConsoleServiceMock.Setup(x => x.IsFancyConsole).Returns(fancyConsole);

            HelpPage.WriteCommandVersions(AppMock.Object, new CliError("MyError", aCommandMock.Object));

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
            var command1Mock = Mocks.Create<ICliCommandInfo>();
            var command2Mock = Mocks.Create<ICliCommandInfo>();
            var command3Mock = Mocks.Create<ICliCommandInfo>();
            var aCommandMock = Mocks.Create<ICliCommandInfo>();
            aCommandMock.Setup(x => x.Author).Returns((string?)null);
            aCommandMock.Setup(x => x.Year).Returns((string?)null);
            aCommandMock.Setup(x => x.Version).Returns((string?)null);
            aCommandMock.Setup(x => x.ParentCommand).Returns((ICliCommandInfo?)null);
            command1Mock.Setup(x => x.Author).Returns((string?)null);
            command1Mock.Setup(x => x.DisplayName).Returns((string?)null);
            command1Mock.Setup(x => x.Year).Returns((string?)null);
            command1Mock.Setup(x => x.Version).Returns((string?)null);
            command1Mock.Setup(x => x.Order).Returns(0);
            command1Mock.Setup(x => x.ParentCommand).Returns(aCommandMock.Object);
            command1Mock.Setup(x => x.Hidden).Returns(false);
            command2Mock.Setup(x => x.Name).Returns("cmd2");
            command2Mock.Setup(x => x.Aliases).Returns(new[] { "cmd2" });
            command2Mock.Setup(x => x.Author).Returns("You");
            command2Mock.Setup(x => x.DisplayName).Returns((string?)null);
            command2Mock.Setup(x => x.Year).Returns("4711");
            command2Mock.Setup(x => x.Version).Returns((string?)null);
            command2Mock.Setup(x => x.Order).Returns(0);
            command2Mock.Setup(x => x.ParentCommand).Returns(aCommandMock.Object);
            command2Mock.Setup(x => x.Hidden).Returns(false);
            command3Mock.Setup(x => x.Name).Returns("cmd3");
            command3Mock.Setup(x => x.Aliases).Returns(new[] { "cmd3", "command3", "my-command-3" });
            command3Mock.Setup(x => x.Author).Returns((string?)null);
            command3Mock.Setup(x => x.DisplayName).Returns("Yours");
            command3Mock.Setup(x => x.Year).Returns((string?)null);
            command3Mock.Setup(x => x.Version).Returns("99");
            command3Mock.Setup(x => x.Order).Returns(0);
            command3Mock.Setup(x => x.ParentCommand).Returns(aCommandMock.Object);
            command3Mock.Setup(x => x.Hidden).Returns(false);
            aCommandMock.Setup(x => x.ChildCommands).Returns(new[] { command1Mock.Object, command2Mock.Object, command3Mock.Object });
            AppMock.Setup(x => x.Options).Returns(new CliApplicationOptions { Name = "My", Year = "1337", Author = "Me", Version = "15" });
            ConsoleServiceMock.Setup(x => x.BufferSize).Returns(CreateConsoleSize(55, int.MaxValue));
            ConsoleServiceMock.Setup(x => x.IsOutputRedirected).Returns(false);
            ConsoleServiceMock.Setup(x => x.IsFancyConsole).Returns(fancyConsole);

            HelpPage.WriteCommandVersions(AppMock.Object, new CliError("MyError", aCommandMock.Object));

            var cchar = fancyConsole ? "©" : "(C)";
            Assert.AreEqual(
                $"{NL}Commands:{NL}" +
                $"   cmd2                         15   {cchar} 4711 You{NL}" +
                $"   cmd3, command3,      Yours   99   {cchar} 1337 Me {NL}" +
                $"   my-command-3                                {(fancyConsole ? string.Empty : "  ")}{NL}",
                sb.ToString());
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

            HelpPage.WriteErrorMessage(AppMock.Object, errorExpr.Compile().Invoke());

            if (addException)
                Assert.AreEqual($"{expectedErrorMessage}{NL}    {exMsg1}{NL}    {exMsg2}{NL}", sb.ToString());
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

            return new CliCommandOptionInfo(
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

        [ExcludeFromCodeCoverage]
        [CliCommand("blub", Executable = false)]
        private class TestCommandOptions : ICliCommandExecutor
        {
            [CliCommandOption("my-option")]
            public string? MyOption { get; set; }

            [CliCommandValue(0, "MyValue")]
            public string? MyValue { get; set; }

            public int ExecuteCommand(CliExecutionContext context)
            {
                throw new NotImplementedException();
            }
        }

        private class PrivateCliHelpPage
        {
            private static readonly PrivateType _pt = new(typeof(CliHelpPage));

            private readonly PrivateObject _po;

            public PrivateCliHelpPage(IConsoleService consoleService)
                : this(new CliHelpPage(consoleService))
            {
            }

            public PrivateCliHelpPage(CliHelpPage helpPage)
            {
                _po = new PrivateObject(helpPage);
            }

            public void WriteVersionPage(ICliApplicationBase application, CliError error)
                => _po.Invoke(nameof(WriteVersionPage), application, error);

            public void WriteHelpPage(ICliApplicationBase application, CliError error)
                => _po.Invoke(nameof(WriteHelpPage), application, error);

            public void WriteErrorPage(ICliApplicationBase application, IList<CliError> errors)
                => _po.Invoke(nameof(WriteErrorPage), application, errors);

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

            public void WriteCommandVersions(ICliApplicationBase application, CliError error)
                => _po.Invoke(nameof(WriteCommandVersions), application, error);

            public static string GetOptionName(CliCommandOptionInfo option)
                => (string)_pt.InvokeStatic(nameof(GetOptionName), option);
        }
    }
}
