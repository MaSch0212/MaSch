using MaSch.Console.Cli.Runtime;
using MaSch.Core.Extensions;
using MaSch.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Language;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MaSch.Console.Cli.Test.Runtime
{
    [TestClass]
    public class CliArgumentParserTests : TestClassBase
    {
        private delegate bool CliValidatorDelegate(CliExecutionContext context, object optionsObj, [MaybeNullWhen(true)] out IEnumerable<CliError>? errors);
        private delegate bool CliValidatableDelegate(CliExecutionContext context, [MaybeNullWhen(true)] out IEnumerable<CliError>? errors);

        private ICliCommandInfo? DefaultCommand
        {
            get => Cache.GetValue<ICliCommandInfo?>(() => null);
            set => Cache.SetValue(value);
        }

        private CliArgumentParser Parser => Cache.GetValue(() => new CliArgumentParser())!;
        private List<ICliCommandInfo> Commands => Cache.GetValue(() => new List<ICliCommandInfo>())!;
        private Mock<ICliCommandInfoCollection> CommandsMock => Cache.GetValue(() => CreateCommandsMock(Commands))!;
        private CliApplicationOptions AppOptions => Cache.GetValue(() => new CliApplicationOptions())!;
        private Mock<ICliApplicationBase> ApplicationMock => Cache.GetValue(() => CreateApplicationMock())!;

        [TestMethod]
        [DataRow(true, DisplayName = "Args: null")]
        [DataRow(false, DisplayName = "Args: empty array")]
        public void Parse_NoCommand_NoDefaultCommand(bool nullArgs)
        {
            var result = CallParse(nullArgs ? null! : Array.Empty<string>());

            AssertFailedParserResult(result, x => x.Type, CliErrorType.MissingCommand);
        }

        [TestMethod]
        [DataRow(true, DisplayName = "Args: null")]
        [DataRow(false, DisplayName = "Args: empty array")]
        public void Parse_NoCommand_WithDefaultCommand(bool nullArgs)
        {
            var command = Mocks.Create<ICliCommandInfo>();
            command.Setup(x => x.CommandType).Returns(typeof(DummyClass1)).Verifiable(Verifiables, Times.Once());
            DefaultCommand = command.Object;

            var result = CallParse(nullArgs ? null! : Array.Empty<string>());

            AssertSuccessfulParserResult<DummyClass1>(result, command.Object);
        }

        [TestMethod]
        public void Parse_NoCommand_WithDefaultCommand_WithValue()
        {
            var optionsInstance = new DummyClass1();
            var value = CreateCliCommandValueMock<string>(0);
            var command = CreateCliCommandMock<DummyClass1>(
                new[] { "blub" },
                values: new[] { value.Object },
                optionsInstance: optionsInstance);
            value.Setup(x => x.SetValue(optionsInstance, "blubbi")).Verifiable(Verifiables, Times.Once());
            DefaultCommand = command.Object;

            var result = CallParse("blubbi");

            AssertSuccessfulParserResult(result, command.Object, optionsInstance);
        }

        [TestMethod]
        public void Parse_UnknownCommand()
        {
            var result = CallParse("blub");

            AssertFailedParserResult(
                result,
                x => (x.Type, x.CommandName),
                (CliErrorType.UnknownCommand, "blub"));
        }

        [TestMethod]
        [DataRow("HELP", null, false, false, DisplayName = "Different casing")]
        [DataRow("help", null, false, false, DisplayName = "No command")]
        [DataRow("help", "blubbi", false, true, DisplayName = "Unknown command")]
        [DataRow("help", "blub", true, false, DisplayName = "Known command")]
        public void Parse_HelpCommand(string helpCommand, string commandArg, bool commandExpected, bool hasUnknown)
        {
            var command = CreateCliCommandMock<DummyClass1>(new[] { "blub" });
            Commands.Add(command.Object);
            AppOptions.ProvideHelpCommand = true;

            var result = commandArg == null ? CallParse(helpCommand) : CallParse(helpCommand, commandArg);

            var expectedErrors = !hasUnknown
                ? new[] { (CliErrorType.HelpRequested, commandExpected ? command.Object : null) }
                : new[]
                {
                    (CliErrorType.HelpRequested, commandExpected ? command.Object : null),
                    (CliErrorType.UnknownCommand, commandExpected ? command.Object : null),
                };
            AssertFailedParserResult(
                result,
                x => (x.Type, x.AffectedCommand),
                expectedErrors);
        }

        [TestMethod]
        [DataRow("VERSION", null, false, false, DisplayName = "Different casing")]
        [DataRow("version", null, false, false, DisplayName = "No command")]
        [DataRow("version", "blubbi", false, true, DisplayName = "Unknown command")]
        [DataRow("version", "blub", true, false, DisplayName = "Known command")]
        public void Parse_VersionCommand(string versionCommand, string commandArg, bool commandExpected, bool hasUnknown)
        {
            var command = CreateCliCommandMock<DummyClass1>(new[] { "blub" });
            Commands.Add(command.Object);
            AppOptions.ProvideVersionCommand = true;

            var result = commandArg == null ? CallParse(versionCommand) : CallParse(versionCommand, commandArg);

            var expectedErrors = !hasUnknown
                ? new[] { (CliErrorType.VersionRequested, commandExpected ? command.Object : null) }
                : new[]
                {
                    (CliErrorType.VersionRequested, commandExpected ? command.Object : null),
                    (CliErrorType.UnknownCommand, commandExpected ? command.Object : null),
                };
            AssertFailedParserResult(
                result,
                x => (x.Type, x.AffectedCommand),
                expectedErrors);
        }

        [TestMethod]
        [DataRow("help")]
        [DataRow("version")]
        public void Parse_SpecialCommand_Disabled(string commandName)
        {
            AppOptions.ProvideHelpCommand = false;
            AppOptions.ProvideVersionCommand = false;

            var result = CallParse(commandName);

            AssertFailedParserResult(
                result,
                x => (x.Type, x.CommandName),
                (CliErrorType.UnknownCommand, commandName));
        }

        [TestMethod]
        [DataRow("help")]
        [DataRow("version")]
        public void Parse_SpecialCommand_Existing(string commandName)
        {
            var command = CreateCliCommandMock<DummyClass1>(new[] { commandName });
            Commands.Add(command.Object);
            AppOptions.ProvideHelpCommand = true;
            AppOptions.ProvideVersionCommand = true;

            var result = CallParse(commandName);

            AssertSuccessfulParserResult<DummyClass1>(result, command.Object);
        }

        [TestMethod]
        [DataRow("HELP")]
        [DataRow("help")]
        public void Parse_HelpOption_First(string helpCommand)
        {
            var command = CreateCliCommandMock<DummyClass1>(new[] { "blub" });
            Commands.Add(command.Object);
            AppOptions.ProvideHelpCommand = true;

            var result = CallParse("--" + helpCommand);

            AssertFailedParserResult(
                result,
                x => (x.Type, x.AffectedCommand),
                (CliErrorType.HelpRequested, null));
        }

        [TestMethod]
        [DataRow("VERSION")]
        [DataRow("version")]
        public void Parse_VersionOption_First(string versionCommand)
        {
            var command = CreateCliCommandMock<DummyClass1>(new[] { "blub" });
            Commands.Add(command.Object);
            AppOptions.ProvideVersionCommand = true;

            var result = CallParse("--" + versionCommand);

            AssertFailedParserResult(
                result,
                x => (x.Type, x.AffectedCommand),
                (CliErrorType.VersionRequested, null));
        }

        [TestMethod]
        [DataRow("help")]
        [DataRow("version")]
        public void Parse_SpecialOption_First_Disabled(string optionName)
        {
            AppOptions.ProvideHelpOptions = false;
            AppOptions.ProvideVersionOptions = false;

            var result = CallParse("--" + optionName);

            AssertFailedParserResult(
                result,
                x => (x.Type, x.CommandName),
                (CliErrorType.UnknownCommand, "--" + optionName));
        }

        [TestMethod]
        [DataRow("help")]
        [DataRow("version")]
        public void Parse_SpecialOption_First_ExistingCommand(string commandName)
        {
            var command = CreateCliCommandMock<DummyClass1>(new[] { "--" + commandName });
            Commands.Add(command.Object);
            AppOptions.ProvideHelpCommand = true;
            AppOptions.ProvideVersionCommand = true;

            var result = CallParse("--" + commandName);

            AssertSuccessfulParserResult<DummyClass1>(result, command.Object);
        }

        [TestMethod]
        [DataRow("HELP")]
        [DataRow("help")]
        public void Parse_HelpOption_OnRootCommand(string helpCommand)
        {
            var command = CreateCliCommandMock<DummyClass1>(new[] { "blub" });
            Commands.Add(command.Object);
            AppOptions.ProvideHelpOptions = true;

            var result = CallParse("blub", "--" + helpCommand);

            AssertFailedParserResult(
                result,
                x => (x.Type, x.AffectedCommand),
                (CliErrorType.HelpRequested, command.Object));
        }

        [TestMethod]
        [DataRow("VERSION")]
        [DataRow("version")]
        public void Parse_VersionOption_OnRootCommand(string versionCommand)
        {
            var command = CreateCliCommandMock<DummyClass1>(new[] { "blub" });
            Commands.Add(command.Object);
            AppOptions.ProvideVersionOptions = true;

            var result = CallParse("blub", "--" + versionCommand);

            AssertFailedParserResult(
                result,
                x => (x.Type, x.AffectedCommand),
                (CliErrorType.VersionRequested, command.Object));
        }

        [TestMethod]
        [DataRow("help")]
        [DataRow("version")]
        public void Parse_SpecialOption_OnRootCommand_ExistingOption(string optionName)
        {
            var option = CreateCliCommandOptionMock<bool>(new[] { optionName });
            var command = CreateCliCommandMock<DummyClass1>(new[] { "blub" }, options: new[] { option.Object });
            option.Setup(x => x.SetValue(It.IsAny<DummyClass1>(), true)).Verifiable(Verifiables, Times.Once());
            Commands.Add(command.Object);
            AppOptions.ProvideHelpOptions = true;
            AppOptions.ProvideVersionOptions = true;

            var result = CallParse("blub", "--" + optionName);

            AssertSuccessfulParserResult<DummyClass1>(result, command.Object);
        }

        [TestMethod]
        [DataRow("help")]
        [DataRow("version")]
        public void Parse_SpecialOption_OnRootCommand_Disabled(string optionName)
        {
            var command = CreateCliCommandMock<DummyClass1>(new[] { "blub" });
            Commands.Add(command.Object);
            AppOptions.ProvideHelpOptions = false;
            AppOptions.ProvideVersionOptions = false;

            var result = CallParse("blub", "--" + optionName);

            AssertFailedParserResult(
                result,
                x => (x.Type, x.AffectedCommand, x.OptionName),
                (CliErrorType.UnknownOption, command.Object, "--" + optionName));
        }

        [TestMethod]
        [DataRow("HELP")]
        [DataRow("help")]
        public void Parse_HelpOption_OnRootCommand_WithChildCommand(string helpCommand)
        {
            var childCommand = CreateCliCommandMock<DummyClass2>(new[] { "blib" });
            var parentCommand = CreateCliCommandMock<DummyClass1>(new[] { "blub" }, childCommands: new[] { childCommand.Object });
            Commands.Add(parentCommand.Object);
            AppOptions.ProvideHelpOptions = true;

            var result = CallParse("blub", "--" + helpCommand);

            AssertFailedParserResult(
                result,
                x => (x.Type, x.AffectedCommand),
                (CliErrorType.HelpRequested, parentCommand.Object));
        }

        [TestMethod]
        [DataRow("VERSION")]
        [DataRow("version")]
        public void Parse_VersionOption_OnRootCommand_WithChildCommand(string versionCommand)
        {
            var childCommand = CreateCliCommandMock<DummyClass2>(new[] { "blib" });
            var parentCommand = CreateCliCommandMock<DummyClass1>(new[] { "blub" }, childCommands: new[] { childCommand.Object });
            Commands.Add(parentCommand.Object);
            AppOptions.ProvideVersionOptions = true;

            var result = CallParse("blub", "--" + versionCommand);

            AssertFailedParserResult(
                result,
                x => (x.Type, x.AffectedCommand),
                (CliErrorType.VersionRequested, parentCommand.Object));
        }

        [TestMethod]
        [DataRow("help")]
        [DataRow("version")]
        public void Parse_SpecialOption_OnRootCommand_WithChildCommand_ExistingOption(string optionName)
        {
            var option = CreateCliCommandOptionMock<bool>(new[] { optionName });
            var childCommand = CreateCliCommandMock<DummyClass2>(new[] { "blib" });
            var parentCommand = CreateCliCommandMock<DummyClass1>(new[] { "blub" }, options: new[] { option.Object }, childCommands: new[] { childCommand.Object });
            option.Setup(x => x.SetValue(It.IsAny<DummyClass1>(), true)).Verifiable(Verifiables, Times.Once());
            Commands.Add(parentCommand.Object);
            AppOptions.ProvideHelpOptions = true;
            AppOptions.ProvideVersionOptions = true;

            var result = CallParse("blub", "--" + optionName);

            AssertSuccessfulParserResult<DummyClass1>(result, parentCommand.Object);
        }

        [TestMethod]
        [DataRow("help")]
        [DataRow("version")]
        public void Parse_SpecialOption_OnRootCommand_WithChildCommand_Disabled(string optionName)
        {
            var childCommand = CreateCliCommandMock<DummyClass2>(new[] { "blib" });
            var parentCommand = CreateCliCommandMock<DummyClass1>(new[] { "blub" }, childCommands: new[] { childCommand.Object });
            Commands.Add(parentCommand.Object);
            AppOptions.ProvideHelpOptions = false;
            AppOptions.ProvideVersionOptions = false;

            var result = CallParse("blub", "--" + optionName);

            AssertFailedParserResult(
                result,
                x => (x.Type, x.AffectedCommand, x.OptionName),
                (CliErrorType.UnknownOption, parentCommand.Object, "--" + optionName));
        }

        [TestMethod]
        [DataRow("HELP")]
        [DataRow("help")]
        public void Parse_HelpOption_OnChildCommand(string helpCommand)
        {
            var childCommand = CreateCliCommandMock<DummyClass2>(new[] { "blib" });
            var parentCommand = CreateCliCommandMock<DummyClass1>(new[] { "blub" }, childCommands: new[] { childCommand.Object });
            Commands.Add(parentCommand.Object);
            AppOptions.ProvideHelpOptions = true;

            var result = CallParse("blub", "blib", "--" + helpCommand);

            AssertFailedParserResult(
                result,
                x => (x.Type, x.AffectedCommand),
                (CliErrorType.HelpRequested, childCommand.Object));
        }

        [TestMethod]
        [DataRow("VERSION")]
        [DataRow("version")]
        public void Parse_VersionOption_OnChildCommand(string versionCommand)
        {
            var childCommand = CreateCliCommandMock<DummyClass2>(new[] { "blib" });
            var parentCommand = CreateCliCommandMock<DummyClass1>(new[] { "blub" }, childCommands: new[] { childCommand.Object });
            Commands.Add(parentCommand.Object);
            AppOptions.ProvideVersionOptions = true;

            var result = CallParse("blub", "blib", "--" + versionCommand);

            AssertFailedParserResult(
                result,
                x => (x.Type, x.AffectedCommand),
                (CliErrorType.VersionRequested, childCommand.Object));
        }

        [TestMethod]
        [DataRow("help")]
        [DataRow("version")]
        public void Parse_SpecialOption_OnChildCommand_ExistingOption(string optionName)
        {
            var option = CreateCliCommandOptionMock<bool>(new[] { optionName });
            var childCommand = CreateCliCommandMock<DummyClass2>(new[] { "blib" }, options: new[] { option.Object });
            var parentCommand = CreateCliCommandMock<DummyClass1>(new[] { "blub" }, childCommands: new[] { childCommand.Object });
            option.Setup(x => x.SetValue(It.IsAny<DummyClass2>(), true)).Verifiable(Verifiables, Times.Once());
            Commands.Add(parentCommand.Object);
            AppOptions.ProvideHelpOptions = true;
            AppOptions.ProvideVersionOptions = true;

            var result = CallParse("blub", "blib", "--" + optionName);

            AssertSuccessfulParserResult<DummyClass2>(result, childCommand.Object);
        }

        [TestMethod]
        [DataRow("help")]
        [DataRow("version")]
        public void Parse_SpecialOption_OnChildCommand_Disabled(string optionName)
        {
            var childCommand = CreateCliCommandMock<DummyClass2>(new[] { "blib" });
            var parentCommand = CreateCliCommandMock<DummyClass1>(new[] { "blub" }, childCommands: new[] { childCommand.Object });
            Commands.Add(parentCommand.Object);
            AppOptions.ProvideHelpOptions = false;
            AppOptions.ProvideVersionOptions = false;

            var result = CallParse("blub", "blib", "--" + optionName);

            AssertFailedParserResult(
                result,
                x => (x.Type, x.AffectedCommand, x.OptionName),
                (CliErrorType.UnknownOption, childCommand.Object, "--" + optionName));
        }

        [TestMethod]
        public void Parse_KnownCommand_NoOptionsOrValues()
        {
            var command = CreateCliCommandMock<DummyClass1>(new[] { "blub" });
            Commands.Add(command.Object);

            var result = CallParse("blub");

            AssertSuccessfulParserResult<DummyClass1>(result, command.Object);
        }

        [TestMethod]
        public void Parse_KnownChildCommand_OneLevel_NoOptionsOrValues()
        {
            var childCommand = CreateCliCommandMock<DummyClass2>(new[] { "blib" });
            var parentCommand = CreateCliCommandMock<DummyClass1>(new[] { "blub" }, childCommands: new[] { childCommand.Object });
            Commands.Add(parentCommand.Object);

            var result = CallParse("blub", "blib");

            AssertSuccessfulParserResult<DummyClass2>(result, childCommand.Object);
        }

        [TestMethod]
        public void Parse_KnownChildCommand_MultipleLevels_NoOptionsOrValues()
        {
            var childCommand3 = CreateCliCommandMock<DummyClass4>(new[] { "blob" });
            var childCommand2 = CreateCliCommandMock<DummyClass3>(new[] { "blab" }, childCommands: new[] { childCommand3.Object });
            var childCommand1 = CreateCliCommandMock<DummyClass2>(new[] { "blib" }, childCommands: new[] { childCommand2.Object });
            var parentCommand = CreateCliCommandMock<DummyClass1>(new[] { "blub" }, childCommands: new[] { childCommand1.Object });
            Commands.Add(parentCommand.Object);

            var result = CallParse("blub", "blib", "blab", "blob");

            AssertSuccessfulParserResult<DummyClass4>(result, childCommand3.Object);
        }

        [TestMethod]
        public void Parse_KnownCommand_WithValues()
        {
            var val1 = CreateCliCommandValueMock<string>(0);
            var command = CreateCliCommandMock<DummyClass1>(new[] { "blub" }, values: new[] { val1.Object });
            val1.Setup(x => x.SetValue(It.IsAny<DummyClass1>(), "blubbi")).Verifiable(Verifiables, Times.Once());
            Commands.Add(command.Object);

            var result = CallParse("blub", "blubbi");

            AssertSuccessfulParserResult<DummyClass1>(result, command.Object);
        }

        [TestMethod]
        public void Parse_KnownChildCommand_OneLevel_WithValues()
        {
            var val1 = CreateCliCommandValueMock<string>(0);
            var childCommand = CreateCliCommandMock<DummyClass2>(new[] { "blib" }, values: new[] { val1.Object });
            var parentCommand = CreateCliCommandMock<DummyClass1>(new[] { "blub" }, childCommands: new[] { childCommand.Object });
            val1.Setup(x => x.SetValue(It.IsAny<DummyClass2>(), "blubbi")).Verifiable(Verifiables, Times.Once());
            Commands.Add(parentCommand.Object);

            var result = CallParse("blub", "blib", "blubbi");

            AssertSuccessfulParserResult<DummyClass2>(result, childCommand.Object);
        }

        [TestMethod]
        public void Parse_KnownChildCommand_MultipleLevels_WithValues()
        {
            var val1 = CreateCliCommandValueMock<string>(0);
            var childCommand3 = CreateCliCommandMock<DummyClass4>(new[] { "blob" }, values: new[] { val1.Object });
            var childCommand2 = CreateCliCommandMock<DummyClass3>(new[] { "blab" }, childCommands: new[] { childCommand3.Object });
            var childCommand1 = CreateCliCommandMock<DummyClass2>(new[] { "blib" }, childCommands: new[] { childCommand2.Object });
            var parentCommand = CreateCliCommandMock<DummyClass1>(new[] { "blub" }, childCommands: new[] { childCommand1.Object });
            val1.Setup(x => x.SetValue(It.IsAny<DummyClass4>(), "blubbi")).Verifiable(Verifiables, Times.Once());
            Commands.Add(parentCommand.Object);

            var result = CallParse("blub", "blib", "blab", "blob", "blubbi");

            AssertSuccessfulParserResult<DummyClass4>(result, childCommand3.Object);
        }

        [TestMethod]
        public void Parse_KnownChildCommand_ValueBefore()
        {
            var val1 = CreateCliCommandValueMock<string>(0);
            var val2 = CreateCliCommandValueMock<string>(1);
            var childCommand = CreateCliCommandMock<DummyClass2>(new[] { "blib" });
            var parentCommand = CreateCliCommandMock<DummyClass1>(new[] { "blub" }, values: new[] { val1.Object, val2.Object }, childCommands: new[] { childCommand.Object });
            val1.Setup(x => x.SetValue(It.IsAny<DummyClass1>(), "blubbi")).Verifiable(Verifiables, Times.Once());
            val2.Setup(x => x.SetValue(It.IsAny<DummyClass1>(), "blib")).Verifiable(Verifiables, Times.Once());
            Commands.Add(parentCommand.Object);

            var result = CallParse("blub", "blubbi", "blib");

            AssertSuccessfulParserResult<DummyClass1>(result, parentCommand.Object);
        }

        [TestMethod]
        public void Parse_KnownChildCommand_OptionBefore()
        {
            var val1 = CreateCliCommandValueMock<string>(0);
            var opt1 = CreateCliCommandOptionMock<string>(new[] { "blub" });
            var childCommand = CreateCliCommandMock<DummyClass2>(new[] { "blib" });
            var parentCommand = CreateCliCommandMock<DummyClass1>(
                new[] { "blub" },
                options: new[] { opt1.Object },
                values: new[] { val1.Object },
                childCommands: new[] { childCommand.Object });
            opt1.Setup(x => x.SetValue(It.IsAny<DummyClass1>(), "blubbi")).Verifiable(Verifiables, Times.Once());
            val1.Setup(x => x.SetValue(It.IsAny<DummyClass1>(), "blib")).Verifiable(Verifiables, Times.Once());
            Commands.Add(parentCommand.Object);

            var result = CallParse("blub", "--blub", "blubbi", "blib");

            AssertSuccessfulParserResult<DummyClass1>(result, parentCommand.Object);
        }

        [TestMethod]
        public void Parse_BoolOption_LongAlias_NoOptionValue()
        {
            var obj = new DummyClass1();
            var option = CreateCliCommandOptionMock<bool>(new[] { "my-bool" });
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, options: new[] { option.Object }, optionsInstance: obj);
            option.Setup(x => x.SetValue(obj, true)).Verifiable(Verifiables, Times.Once());
            Commands.Add(command.Object);

            var result = CallParse("my-command", "--my-bool");

            AssertSuccessfulParserResult(result, command.Object, obj);
        }

        [TestMethod]
        public void Parse_BoolOption_LongAlias_NoOptionValue_Nullable()
        {
            var obj = new DummyClass1();
            var option = CreateCliCommandOptionMock<bool?>(new[] { "my-bool" });
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, options: new[] { option.Object }, optionsInstance: obj);
            option.Setup(x => x.SetValue(obj, true)).Verifiable(Verifiables, Times.Once());
            Commands.Add(command.Object);

            var result = CallParse("my-command", "--my-bool");

            AssertSuccessfulParserResult(result, command.Object, obj);
        }

        [TestMethod]
        public void Parse_BoolOption_LongAlias_WithOptionValue()
        {
            var obj = new DummyClass1();
            var option = CreateCliCommandOptionMock<bool>(new[] { "my-bool" });
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, options: new[] { option.Object }, optionsInstance: obj);
            option.Setup(x => x.SetValue(obj, "false")).Verifiable(Verifiables, Times.Once());
            Commands.Add(command.Object);

            var result = CallParse("my-command", "--my-bool", "false");

            AssertSuccessfulParserResult(result, command.Object, obj);
        }

        [TestMethod]
        public void Parse_BoolOption_ShortAlias_NoOptionValue()
        {
            var obj = new DummyClass1();
            var option = CreateCliCommandOptionMock<bool>(new[] { "my-bool" }, new char[] { 'b' });
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, options: new[] { option.Object }, optionsInstance: obj);
            option.Setup(x => x.SetValue(obj, true)).Verifiable(Verifiables, Times.Once());
            Commands.Add(command.Object);

            var result = CallParse("my-command", "-b");

            AssertSuccessfulParserResult(result, command.Object, obj);
        }

        [TestMethod]
        public void Parse_BoolOption_ShortAlias_NoOptionValue_Nullable()
        {
            var obj = new DummyClass1();
            var option = CreateCliCommandOptionMock<bool?>(new[] { "my-bool" }, new char[] { 'b' });
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, options: new[] { option.Object }, optionsInstance: obj);
            option.Setup(x => x.SetValue(obj, true)).Verifiable(Verifiables, Times.Once());
            Commands.Add(command.Object);

            var result = CallParse("my-command", "-b");

            AssertSuccessfulParserResult(result, command.Object, obj);
        }

        [TestMethod]
        public void Parse_BoolOption_ShortAlias_WithOptionValue()
        {
            var obj = new DummyClass1();
            var option = CreateCliCommandOptionMock<bool>(new[] { "my-bool" }, new char[] { 'b' });
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, options: new[] { option.Object }, optionsInstance: obj);
            option.Setup(x => x.SetValue(obj, "false")).Verifiable(Verifiables, Times.Once());
            Commands.Add(command.Object);

            var result = CallParse("my-command", "-b", "false");

            AssertSuccessfulParserResult(result, command.Object, obj);
        }

        [TestMethod]
        public void Parse_BoolOption_ShortAlias_Multiple_NoOptionValue()
        {
            var obj = new DummyClass1();
            var option1 = CreateCliCommandOptionMock<bool>(new[] { "my-bool1" }, new char[] { 'b' });
            var option2 = CreateCliCommandOptionMock<bool>(new[] { "my-bool2" }, new char[] { 'B' });
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, options: new[] { option1.Object, option2.Object }, optionsInstance: obj);
            option1.Setup(x => x.SetValue(obj, true)).Verifiable(Verifiables, Times.Once());
            option2.Setup(x => x.SetValue(obj, true)).Verifiable(Verifiables, Times.Once());
            Commands.Add(command.Object);

            var result = CallParse("my-command", "-bB");

            AssertSuccessfulParserResult(result, command.Object, obj);
        }

        [TestMethod]
        public void Parse_BoolOption_ShortAlias_Multiple_NoOptionValue_Nullable()
        {
            var obj = new DummyClass1();
            var option1 = CreateCliCommandOptionMock<bool?>(new[] { "my-bool1" }, new char[] { 'b' });
            var option2 = CreateCliCommandOptionMock<bool?>(new[] { "my-bool2" }, new char[] { 'B' });
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, options: new[] { option1.Object, option2.Object }, optionsInstance: obj);
            option1.Setup(x => x.SetValue(obj, true)).Verifiable(Verifiables, Times.Once());
            option2.Setup(x => x.SetValue(obj, true)).Verifiable(Verifiables, Times.Once());
            Commands.Add(command.Object);

            var result = CallParse("my-command", "-bB");

            AssertSuccessfulParserResult(result, command.Object, obj);
        }

        [TestMethod]
        public void Parse_Option_WrongOptionFormat()
        {
            var ex = new Exception();
            var option = CreateCliCommandOptionMock<object>(new[] { "my-option" });
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, options: new[] { option.Object });
            option.Setup(x => x.SetValue(It.IsAny<DummyClass1>(), "my-option-value")).Throws(ex).Verifiable(Verifiables, Times.Once());
            Commands.Add(command.Object);

            var result = CallParse("my-command", "--my-option", "my-option-value");

            AssertFailedParserResult(
                result,
                x => (x.Type, x.AffectedCommand, x.AffectedOption, x.Exception),
                (CliErrorType.WrongOptionFormat, command.Object, option.Object, ex));
        }

        [TestMethod]
        public void Parse_Value_WrongOptionFormat()
        {
            var ex = new Exception();
            var value = CreateCliCommandValueMock<object>(0);
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, values: new[] { value.Object });
            value.Setup(x => x.SetValue(It.IsAny<DummyClass1>(), "my-value")).Throws(ex).Verifiable(Verifiables, Times.Once());
            Commands.Add(command.Object);

            var result = CallParse("my-command", "my-value");

            AssertFailedParserResult(
                result,
                x => (x.Type, x.AffectedCommand, x.AffectedValue, x.Exception),
                (CliErrorType.WrongValueFormat, command.Object, value.Object, ex));
        }

        [TestMethod]
        public void Parse_Value_TooManyValues_OneToMany()
        {
            var value = CreateCliCommandValueMock<object>(0);
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, values: new[] { value.Object });
            value.Setup(x => x.SetValue(It.IsAny<DummyClass1>(), "my-value")).Verifiable(Verifiables, Times.Once());
            Commands.Add(command.Object);

            var result = CallParse("my-command", "my-value", "my-second-value");

            AssertFailedParserResult(
                result,
                x => (x.Type, x.AffectedCommand),
                (CliErrorType.UnknownValue, command.Object));
        }

        [TestMethod]
        public void Parse_Value_TooManyValues_TwoToMany()
        {
            var value = CreateCliCommandValueMock<object>(0);
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, values: new[] { value.Object });
            value.Setup(x => x.SetValue(It.IsAny<DummyClass1>(), "my-value")).Verifiable(Verifiables, Times.Once());
            Commands.Add(command.Object);

            var result = CallParse("my-command", "my-value", "my-second-value", "my-third-value");

            AssertFailedParserResult(
                result,
                x => (x.Type, x.AffectedCommand),
                (CliErrorType.UnknownValue, command.Object));
        }

        [TestMethod]
        public void Parse_Value_TooManyValues_Ignored()
        {
            var obj = new DummyClass1();
            var value = CreateCliCommandValueMock<object>(0);
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, values: new[] { value.Object }, optionsInstance: obj);
            value.Setup(x => x.SetValue(obj, "my-value")).Verifiable(Verifiables, Times.Once());
            Commands.Add(command.Object);
            AppOptions.IgnoreAdditionalValues = true;

            var result = CallParse("my-command", "my-value", "my-second-value");

            AssertSuccessfulParserResult(result, command.Object, obj);
        }

        [TestMethod]
        public void Parse_Value_Escaped()
        {
            var obj = new DummyClass1();
            var value = CreateCliCommandValueMock<object>(0);
            var option = CreateCliCommandOptionMock<object>(new[] { "my-option" });
            var command = CreateCliCommandMock<DummyClass1>(
                new[] { "my-command" },
                options: new[] { option.Object },
                values: new[] { value.Object },
                optionsInstance: obj);
            value.Setup(x => x.SetValue(obj, "--my-option")).Verifiable(Verifiables, Times.Once());
            Commands.Add(command.Object);

            var result = CallParse("my-command", "--", "--my-option");

            AssertSuccessfulParserResult(result, command.Object, obj);
        }

        [TestMethod]
        [DataRow("--my-option")]
        [DataRow("-O")]
        public void Parse_Option_Unknown(string optionName)
        {
            var obj = new DummyClass1();
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, optionsInstance: obj);
            Commands.Add(command.Object);

            var result = CallParse("my-command", optionName);

            AssertFailedParserResult(
                result,
                x => (x.Type, x.AffectedCommand, x.OptionName),
                (CliErrorType.UnknownOption, command.Object, optionName));
        }

        [TestMethod]
        [DataRow("--my-option")]
        [DataRow("-O")]
        public void Parse_Option_Unknown_WithValue(string optionName)
        {
            var obj = new DummyClass1();
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, optionsInstance: obj);
            Commands.Add(command.Object);

            var result = CallParse("my-command", optionName, "my-value");

            AssertFailedParserResult(
                result,
                x => (x.Type, x.AffectedCommand, x.OptionName),
                (CliErrorType.UnknownOption, command.Object, optionName));
        }

        [TestMethod]
        [DataRow("--my-option")]
        [DataRow("-O")]
        public void Parse_Option_Unknown_Ignore(string optionName)
        {
            var obj = new DummyClass1();
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, optionsInstance: obj);
            Commands.Add(command.Object);
            AppOptions.IgnoreUnknownOptions = true;

            var result = CallParse("my-command", optionName);

            AssertSuccessfulParserResult(result, command.Object, obj);
        }

        [TestMethod]
        [DataRow("--my-option")]
        [DataRow("-O")]
        public void Parse_Option_Unknown_WithValue_Ignore(string optionName)
        {
            var obj = new DummyClass1();
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, optionsInstance: obj);
            Commands.Add(command.Object);
            AppOptions.IgnoreUnknownOptions = true;

            var result = CallParse("my-command", optionName, "my-value");

            AssertSuccessfulParserResult(result, command.Object, obj);
        }

        [TestMethod]
        [DataRow("--my-option")]
        [DataRow("-O")]
        public void Parse_Option_Unknown_WithKnown(string optionName)
        {
            var obj = new DummyClass1();
            var option = CreateCliCommandOptionMock<string>(new[] { "my-string" });
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, options: new[] { option.Object }, optionsInstance: obj);
            option.Setup(x => x.SetValue(obj, "blub")).Verifiable(Verifiables, Times.Once());
            Commands.Add(command.Object);

            var result = CallParse("my-command", optionName, "--my-string", "blub");

            AssertFailedParserResult(
                result,
                x => (x.Type, x.AffectedCommand, x.OptionName),
                (CliErrorType.UnknownOption, command.Object, optionName));
        }

        [TestMethod]
        [DataRow("--my-option")]
        [DataRow("-O")]
        public void Parse_Option_Unknown_WithKnown_WithValue(string optionName)
        {
            var obj = new DummyClass1();
            var option = CreateCliCommandOptionMock<string>(new[] { "my-string" });
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, options: new[] { option.Object }, optionsInstance: obj);
            option.Setup(x => x.SetValue(obj, "blub")).Verifiable(Verifiables, Times.Once());
            Commands.Add(command.Object);

            var result = CallParse("my-command", optionName, "my-value", "--my-string", "blub");

            AssertFailedParserResult(
                result,
                x => (x.Type, x.AffectedCommand, x.OptionName),
                (CliErrorType.UnknownOption, command.Object, optionName));
        }

        [TestMethod]
        [DataRow("--my-option")]
        [DataRow("-O")]
        public void Parse_Option_Unknown_WithKnown_Ignore(string optionName)
        {
            var obj = new DummyClass1();
            var option = CreateCliCommandOptionMock<string>(new[] { "my-string" });
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, options: new[] { option.Object }, optionsInstance: obj);
            option.Setup(x => x.SetValue(obj, "blub")).Verifiable(Verifiables, Times.Once());
            Commands.Add(command.Object);
            AppOptions.IgnoreUnknownOptions = true;

            var result = CallParse("my-command", optionName, "--my-string", "blub");

            AssertSuccessfulParserResult(result, command.Object, obj);
        }

        [TestMethod]
        [DataRow("--my-option")]
        [DataRow("-O")]
        public void Parse_Option_Unknown_WithKnown_WithValue_Ignore(string optionName)
        {
            var obj = new DummyClass1();
            var option = CreateCliCommandOptionMock<string>(new[] { "my-string" });
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, options: new[] { option.Object }, optionsInstance: obj);
            option.Setup(x => x.SetValue(obj, "blub")).Verifiable(Verifiables, Times.Once());
            Commands.Add(command.Object);
            AppOptions.IgnoreUnknownOptions = true;

            var result = CallParse("my-command", optionName, "my-value", "--my-string", "blub");

            AssertSuccessfulParserResult(result, command.Object, obj);
        }

        [TestMethod]
        public void Parse_Option_Unknown_Multiple_LongAlias()
        {
            var obj = new DummyClass1();
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, optionsInstance: obj);
            Commands.Add(command.Object);

            var result = CallParse("my-command", "--my-option1", "--my-option2");

            AssertFailedParserResult(
                result,
                x => (x.Type, x.AffectedCommand, x.OptionName),
                (CliErrorType.UnknownOption, command.Object, "--my-option1"),
                (CliErrorType.UnknownOption, command.Object, "--my-option2"));
        }

        [TestMethod]
        public void Parse_Option_Unknown_Multiple_ShortAlias()
        {
            var obj = new DummyClass1();
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, optionsInstance: obj);
            Commands.Add(command.Object);

            var result = CallParse("my-command", "-Oo");

            AssertFailedParserResult(
                result,
                x => (x.Type, x.AffectedCommand, x.OptionName),
                (CliErrorType.UnknownOption, command.Object, "-o"),
                (CliErrorType.UnknownOption, command.Object, "-O"));
        }

        [TestMethod]
        public void Parse_Option_Unknown_Multiple_WithValue()
        {
            var obj = new DummyClass1();
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, optionsInstance: obj);
            Commands.Add(command.Object);

            var result = CallParse("my-command", "--my-option1", "my-value", "--my-option2", "my-value");

            AssertFailedParserResult(
                result,
                x => (x.Type, x.AffectedCommand, x.OptionName),
                (CliErrorType.UnknownOption, command.Object, "--my-option1"),
                (CliErrorType.UnknownOption, command.Object, "--my-option2"));
        }

        [TestMethod]
        public void Parse_Option_Unknown_Multiple_LongAlias_Ignore()
        {
            var obj = new DummyClass1();
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, optionsInstance: obj);
            Commands.Add(command.Object);
            AppOptions.IgnoreUnknownOptions = true;

            var result = CallParse("my-command", "--my-option1", "--my-option2");

            AssertSuccessfulParserResult(result, command.Object, obj);
        }

        [TestMethod]
        public void Parse_Option_Unknown_Multiple_ShortAlias_Ignore()
        {
            var obj = new DummyClass1();
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, optionsInstance: obj);
            Commands.Add(command.Object);
            AppOptions.IgnoreUnknownOptions = true;

            var result = CallParse("my-command", "-Oo");

            AssertSuccessfulParserResult(result, command.Object, obj);
        }

        [TestMethod]
        public void Parse_Option_Unknown_Multiple_WithValue_Ignore()
        {
            var obj = new DummyClass1();
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, optionsInstance: obj);
            Commands.Add(command.Object);
            AppOptions.IgnoreUnknownOptions = true;

            var result = CallParse("my-command", "--my-option1", "my-value", "--my-option2", "my-value");

            AssertSuccessfulParserResult(result, command.Object, obj);
        }

        [TestMethod]
        public void Parse_Option_Unknown_Multiple_LongAlias_WithKnown()
        {
            var obj = new DummyClass1();
            var option = CreateCliCommandOptionMock<string>(new[] { "my-string" });
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, options: new[] { option.Object }, optionsInstance: obj);
            option.Setup(x => x.SetValue(obj, "blub")).Verifiable(Verifiables, Times.Once());
            Commands.Add(command.Object);

            var result = CallParse("my-command", "--my-option1", "--my-option2", "--my-string", "blub");

            AssertFailedParserResult(
                result,
                x => (x.Type, x.AffectedCommand, x.OptionName),
                (CliErrorType.UnknownOption, command.Object, "--my-option1"),
                (CliErrorType.UnknownOption, command.Object, "--my-option2"));
        }

        [TestMethod]
        public void Parse_Option_Unknown_Multiple_ShortAlias_WithKnown()
        {
            var obj = new DummyClass1();
            var option = CreateCliCommandOptionMock<string>(new[] { "my-string" });
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, options: new[] { option.Object }, optionsInstance: obj);
            option.Setup(x => x.SetValue(obj, "blub")).Verifiable(Verifiables, Times.Once());
            Commands.Add(command.Object);

            var result = CallParse("my-command", "-Oo", "--my-string", "blub");

            AssertFailedParserResult(
                result,
                x => (x.Type, x.AffectedCommand, x.OptionName),
                (CliErrorType.UnknownOption, command.Object, "-o"),
                (CliErrorType.UnknownOption, command.Object, "-O"));
        }

        [TestMethod]
        public void Parse_Option_Unknown_Multiple_WithKnown_WithValue()
        {
            var obj = new DummyClass1();
            var option = CreateCliCommandOptionMock<string>(new[] { "my-string" });
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, options: new[] { option.Object }, optionsInstance: obj);
            option.Setup(x => x.SetValue(obj, "blub")).Verifiable(Verifiables, Times.Once());
            Commands.Add(command.Object);

            var result = CallParse("my-command", "--my-option1", "my-value", "--my-option2", "my-value", "--my-string", "blub");

            AssertFailedParserResult(
                result,
                x => (x.Type, x.AffectedCommand, x.OptionName),
                (CliErrorType.UnknownOption, command.Object, "--my-option1"),
                (CliErrorType.UnknownOption, command.Object, "--my-option2"));
        }

        [TestMethod]
        public void Parse_Option_Unknown_Multiple_LongAlias_WithKnown_Ignore()
        {
            var obj = new DummyClass1();
            var option = CreateCliCommandOptionMock<string>(new[] { "my-string" });
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, options: new[] { option.Object }, optionsInstance: obj);
            option.Setup(x => x.SetValue(obj, "blub")).Verifiable(Verifiables, Times.Once());
            Commands.Add(command.Object);
            AppOptions.IgnoreUnknownOptions = true;

            var result = CallParse("my-command", "--my-option1", "--my-option2", "--my-string", "blub");

            AssertSuccessfulParserResult(result, command.Object, obj);
        }

        [TestMethod]
        public void Parse_Option_Unknown_Multiple_ShortAlias_WithKnown_Ignore()
        {
            var obj = new DummyClass1();
            var option = CreateCliCommandOptionMock<string>(new[] { "my-string" });
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, options: new[] { option.Object }, optionsInstance: obj);
            option.Setup(x => x.SetValue(obj, "blub")).Verifiable(Verifiables, Times.Once());
            Commands.Add(command.Object);
            AppOptions.IgnoreUnknownOptions = true;

            var result = CallParse("my-command", "-Oo", "--my-string", "blub");

            AssertSuccessfulParserResult(result, command.Object, obj);
        }

        [TestMethod]
        public void Parse_Option_Unknown_Multiple_WithKnown_WithValue_Ignore()
        {
            var obj = new DummyClass1();
            var option = CreateCliCommandOptionMock<string>(new[] { "my-string" });
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, options: new[] { option.Object }, optionsInstance: obj);
            option.Setup(x => x.SetValue(obj, "blub")).Verifiable(Verifiables, Times.Once());
            Commands.Add(command.Object);
            AppOptions.IgnoreUnknownOptions = true;

            var result = CallParse("my-command", "--my-option1", "my-value", "--my-option2", "my-value", "--my-string", "blub");

            AssertSuccessfulParserResult(result, command.Object, obj);
        }

        [TestMethod]
        public void Parse_Option_MissingValue()
        {
            var obj = new DummyClass1();
            var option = CreateCliCommandOptionMock<string>(new[] { "my-string" });
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, options: new[] { option.Object }, optionsInstance: obj);
            Commands.Add(command.Object);

            var result = CallParse("my-command", "--my-string");

            AssertFailedParserResult(
                result,
                x => (x.Type, x.AffectedCommand, x.AffectedOption),
                (CliErrorType.MissingOptionValue, command.Object, option.Object));
        }

        [TestMethod]
        [DataRow(typeof(IEnumerable))]
        [DataRow(typeof(IEnumerable<string>))]
        [DataRow(typeof(List<string>))]
        [DataRow(typeof(string[]))]
        [DataRow(typeof(Array))]
        public void Parse_Option_List(Type listType)
        {
            var obj = new DummyClass1();
            var option = CreateCliCommandOptionMock(listType, new[] { "my-list" }, defaultValue: Array.Empty<object>());
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, options: new[] { option.Object }, optionsInstance: obj);
            option.Setup(x => x.SetValue(obj, new[] { "item1", "item2", "item3", "-" })).Verifiable(Verifiables, Times.Once());
            Commands.Add(command.Object);

            var result = CallParse("my-command", "--my-list", "item1", "item2", "item3", "-");

            AssertSuccessfulParserResult(result, command.Object, obj);
        }

        [TestMethod]
        [DataRow(typeof(IEnumerable))]
        [DataRow(typeof(IEnumerable<string>))]
        [DataRow(typeof(List<string>))]
        [DataRow(typeof(string[]))]
        [DataRow(typeof(Array))]
        public void Parse_Value_List(Type listType)
        {
            var obj = new DummyClass1();
            var value = CreateCliCommandValueMock(listType, 0, defaultValue: Array.Empty<object>());
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, values: new[] { value.Object }, optionsInstance: obj);
            value.Setup(x => x.SetValue(obj, new[] { "item1", "item2", "item3", "-" })).Verifiable(Verifiables, Times.Once());
            Commands.Add(command.Object);

            var result = CallParse("my-command", "item1", "item2", "item3", "-");

            AssertSuccessfulParserResult(result, command.Object, obj);
        }

        [TestMethod]
        public void Parse_Value_List_InterruptedByOption()
        {
            var obj = new DummyClass1();
            var value = CreateCliCommandValueMock<IEnumerable>(0);
            var option = CreateCliCommandOptionMock<string>(new[] { "my-string" });
            var command = CreateCliCommandMock<DummyClass1>(
                new[] { "my-command" },
                options: new[] { option.Object },
                values: new[] { value.Object },
                optionsInstance: obj);
            object? currentValue = null;
            value.Setup(x => x.SetValue(obj, It.IsAny<object?>())).Callback<object?, object?>((_, x) => currentValue = x);
            value.Setup(x => x.GetValue(obj)).Returns<object?>(_ => currentValue);
            value.Setup(x => x.SetValue(obj, new[] { "item1", "item2", "item3", "-" })).Verifiable(Verifiables, Times.Once());
            option.Setup(x => x.SetValue(obj, "blub")).Verifiable(Verifiables, Times.Once());
            Commands.Add(command.Object);

            var result = CallParse("my-command", "item1", "item2", "--my-string", "blub", "item3", "-");

            AssertSuccessfulParserResult(result, command.Object, obj);
        }

        [TestMethod]
        public void Parse_RequiredOption_Missing()
        {
            var obj = new DummyClass1();
            var option = CreateCliCommandOptionMock<object>(new[] { "my-option" }, isRequired: true, hasValue: false);
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, options: new[] { option.Object }, optionsInstance: obj);
            Commands.Add(command.Object);

            var result = CallParse("my-command");

            AssertFailedParserResult(
                result,
                x => (x.Type, x.AffectedCommand, x.AffectedOption),
                (CliErrorType.MissingOption, command.Object, option.Object));
        }

        [TestMethod]
        public void Parse_RequiredValueMissing()
        {
            var obj = new DummyClass1();
            var value = CreateCliCommandValueMock<object>(0, isRequired: true, hasValue: false);
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, values: new[] { value.Object }, optionsInstance: obj);
            Commands.Add(command.Object);

            var result = CallParse("my-command");

            AssertFailedParserResult(
                result,
                x => (x.Type, x.AffectedCommand, x.AffectedValue),
                (CliErrorType.MissingValue, command.Object, value.Object));
        }

        [TestMethod]
        public void Parse_CustomCommonValidatorFails()
        {
            var obj = new DummyClass1();
            var validator = Mocks.Create<ICliValidator<object>>();
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, optionsInstance: obj);
            SetupValidation(validator, command.Object, obj, false, new[] { new CliError("My Error") }).Verifiable(Verifiables, Times.Once());
            Commands.Add(command.Object);
            Parser.AddValidator(validator.Object);

            var result = CallParse("my-command");

            AssertFailedParserResult(
                result,
                x => (x.Type, x.CustomErrorMessage),
                (CliErrorType.Custom, "My Error"));
        }

        [TestMethod]
        public void Parse_OptionsInstanceValidatorFails()
        {
            var obj = Mocks.Create<ICliValidatable>();
            var command = CreateCliCommandMock<object>(new[] { "my-command" }, optionsInstance: obj.Object);
            SetupValidation(obj, command.Object, false, new[] { new CliError("My Error") }).Verifiable(Verifiables, Times.Once());
            Commands.Add(command.Object);

            var result = CallParse("my-command");

            AssertFailedParserResult(
                result,
                x => (x.Type, x.CustomErrorMessage),
                (CliErrorType.Custom, "My Error"));
        }

        [TestMethod]
        public void Parse_OptionsInstanceValidatorSucceeds()
        {
            var obj = Mocks.Create<ICliValidatable>();
            var command = CreateCliCommandMock<object>(new[] { "my-command" }, optionsInstance: obj.Object);
            SetupValidation(obj, command.Object, true, null).Verifiable(Verifiables, Times.Once());
            Commands.Add(command.Object);

            var result = CallParse("my-command");

            AssertSuccessfulParserResult(result, command.Object, obj.Object);
        }

        [TestMethod]
        public void Parse_CommandValidationFails()
        {
            var obj = new DummyClass1();
            var command = CreateCliCommandMock<DummyClass1>(new[] { "my-command" }, optionsInstance: obj);
            SetupValidation(command.As<ICliValidator<object>>(), command.Object, obj, false, new[] { new CliError("My Error") }).Verifiable(Verifiables, Times.Once());
            Commands.Add(command.Object);

            var result = CallParse("my-command");

            AssertFailedParserResult(
                result,
                x => (x.Type, x.CustomErrorMessage),
                (CliErrorType.Custom, "My Error"));
        }

        private CliArgumentParserResult CallParse(params string[] args)
            => Parser.Parse(ApplicationMock.Object, args);

        private Mock<ICliCommandInfoCollection> CreateCommandsMock(ICollection<ICliCommandInfo> commands)
        {
            var result = Mocks.Create<ICliCommandInfoCollection>();
            result.Setup(x => x.GetEnumerator()).Returns(() => commands.GetEnumerator());
            result.Setup(x => x.GetRootCommands()).Returns(() => commands.Where(x =>
            {
                try
                {
                    return x.ParentCommand == null;
                }
                catch (MockException)
                {
                    return true;
                }
            }));
            result.Setup(x => x.DefaultCommand).Returns(() => DefaultCommand);
            return result;
        }

        private Mock<ICliApplicationBase> CreateApplicationMock()
        {
            var result = Mocks.Create<ICliApplicationBase>();
            result.Setup(x => x.Commands).Returns(new CliCommandInfoCollection.ReadOnly(CommandsMock.Object));
            result.Setup(x => x.Options).Returns(AppOptions);
            return result;
        }

        private Mock<ICliCommandInfo> CreateCliCommandMock<TCommandType>(
            string[] aliases,
            ICliCommandOptionInfo[]? options = null,
            ICliCommandValueInfo[]? values = null,
            object? optionsInstance = null,
            ICliCommandInfo[]? childCommands = null)
        {
            var command = Mocks.Create<ICliCommandInfo>();
            command.Setup(x => x.CommandType).Returns(typeof(TCommandType));
            command.Setup(x => x.Aliases).Returns(aliases);
            command.Setup(x => x.Options).Returns(options ?? Array.Empty<ICliCommandOptionInfo>());
            command.Setup(x => x.Values).Returns(values ?? Array.Empty<ICliCommandValueInfo>());
            command.Setup(x => x.ChildCommands).Returns(childCommands ?? Array.Empty<ICliCommandInfo>());
            command.Setup(x => x.OptionsInstance).Returns(optionsInstance);
            command.Setup(x => x.IsExecutable).Returns(true);
            command.Setup(x => x.ParserOptions).Returns(new CliParserOptions());
            command.Setup(x => x.ParentCommand).Returns((ICliCommandInfo?)null);
            SetupValidation(command.As<ICliValidator<object>>(), true, null);
            return command;
        }

        private Mock<ICliCommandValueInfo> CreateCliCommandValueMock<TProperty>(
            int order,
            bool isRequired = false,
            TProperty? currentValue = default,
            TProperty? defaultValue = default,
            bool hasValue = false)
            => CreateCliCommandValueMock(typeof(TProperty), order, isRequired, currentValue, defaultValue, hasValue);

        private Mock<ICliCommandValueInfo> CreateCliCommandValueMock(
            Type propertyType,
            int order,
            bool isRequired = false,
            object? currentValue = default,
            object? defaultValue = default,
            bool hasValue = false)
        {
            var value = Mocks.Create<ICliCommandValueInfo>();
            value.Setup(x => x.Order).Returns(order);
            value.Setup(x => x.PropertyType).Returns(propertyType);
            value.Setup(x => x.IsRequired).Returns(isRequired);
            value.Setup(x => x.DefaultValue).Returns(defaultValue);
            value.Setup(x => x.Command).Returns(Mocks.Create<ICliCommandInfo>().Object);
            value.Setup(x => x.SetDefaultValue(It.IsAny<object>()));
            value.Setup(x => x.SetValue(It.IsAny<object>(), defaultValue));
            value.Setup(x => x.GetValue(It.IsAny<object>())).Returns(currentValue);
            value.Setup(x => x.HasValue(It.IsAny<object>())).Returns(hasValue);
            return value;
        }

        private Mock<ICliCommandOptionInfo> CreateCliCommandOptionMock<TProperty>(
            string[] aliases,
            char[]? shortAliases = null,
            bool isRequired = false,
            TProperty? currentValue = default,
            TProperty? defaultValue = default,
            bool hasValue = false)
            => CreateCliCommandOptionMock(typeof(TProperty), aliases, shortAliases, isRequired, currentValue, defaultValue, hasValue);

        private Mock<ICliCommandOptionInfo> CreateCliCommandOptionMock(
            Type propertyType,
            string[] aliases,
            char[]? shortAliases = null,
            bool isRequired = false,
            object? currentValue = default,
            object? defaultValue = default,
            bool hasValue = false)
        {
            var option = Mocks.Create<ICliCommandOptionInfo>();
            option.Setup(x => x.Aliases).Returns(aliases);
            option.Setup(x => x.ShortAliases).Returns(shortAliases ?? Array.Empty<char>());
            option.Setup(x => x.PropertyType).Returns(propertyType);
            option.Setup(x => x.IsRequired).Returns(isRequired);
            option.Setup(x => x.DefaultValue).Returns(defaultValue);
            option.Setup(x => x.Command).Returns(Mocks.Create<ICliCommandInfo>().Object);
            option.Setup(x => x.SetDefaultValue(It.IsAny<object>()));
            option.Setup(x => x.SetValue(It.IsAny<object>(), defaultValue));
            option.Setup(x => x.GetValue(It.IsAny<object>())).Returns(currentValue);
            option.Setup(x => x.HasValue(It.IsAny<object>())).Returns(hasValue);
            return option;
        }

        private IVerifies SetupValidation<T>(Mock<ICliValidator<T>> mock, bool result, IEnumerable<CliError>? errors)
        {
            IEnumerable<CliError>? validationErrors;
            return mock
                .Setup(x => x.ValidateOptions(It.IsAny<CliExecutionContext>(), It.IsAny<T>(), out validationErrors))
                .Returns(new CliValidatorDelegate((CliExecutionContext c, object o, out IEnumerable<CliError>? e) =>
                {
                    e = errors;
                    return result;
                }));
        }

        private IVerifies SetupValidation<T>(Mock<ICliValidator<T>> mock, ICliCommandInfo command, T obj, bool result, IEnumerable<CliError>? errors)
        {
            IEnumerable<CliError>? validationErrors;
            return mock
                .Setup(x => x.ValidateOptions(It.Is<CliExecutionContext>(x => x.Command == command), obj, out validationErrors))
                .Returns(new CliValidatorDelegate((CliExecutionContext c, object o, out IEnumerable<CliError>? e) =>
                {
                    e = errors;
                    return result;
                }));
        }

        private IVerifies SetupValidation(Mock<ICliValidatable> mock, ICliCommandInfo command, bool result, IEnumerable<CliError>? errors)
        {
            IEnumerable<CliError>? validationErrors;
            return mock
                .Setup(x => x.ValidateOptions(It.Is<CliExecutionContext>(x => x.Command == command), out validationErrors))
                .Returns(new CliValidatableDelegate((CliExecutionContext c, out IEnumerable<CliError>? e) =>
                {
                    e = errors;
                    return result;
                }));
        }

        private void AssertSuccessfulParserResult<TExpectedOptionsType>(
            CliArgumentParserResult result,
            ICliCommandInfo expectedCommand,
            TExpectedOptionsType? expectedOptions = null)
            where TExpectedOptionsType : class
        {
            Assert.IsTrue(result.Success, "Parse failed unexpectedly.");
            Assert.AreSame(expectedCommand, result.ExecutionContext!.Command, "Parse resulted in wrong command.");
            Assert.IsInstanceOfType<TExpectedOptionsType>(result.Options, "Options returned from parse have unexpected type.");

            if (expectedOptions is not null)
            {
                Assert.AreSame(expectedOptions, result.Options, "Options are not the expected ones.");
            }
        }

        private void AssertFailedParserResult<T>(
            CliArgumentParserResult result,
            Func<CliError, T> actualErrorTransformation,
            params T[] expectedErrorValues)
        {
            Assert.IsFalse(result.Success, "Parse succeeded unexpectedly.");
            Assert.AreCollectionsEquivalent(
                expectedErrorValues,
                result.Errors.Select(actualErrorTransformation),
                "Parse errors are not as expected.");
        }

        private class DummyClass1
        {
        }

        private class DummyClass2
        {
        }

        private class DummyClass3
        {
        }

        private class DummyClass4
        {
        }
    }
}
