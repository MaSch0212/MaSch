using MaSch.Console.Cli.Runtime;
using MaSch.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Language;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MaSch.Console.Cli.Test.Runtime
{
    [TestClass]
    public class CliArgumentParserTests : TestClassBase
    {
        private delegate bool CliValidatorDelegate(ICliCommandInfo command, object optionsObj, [MaybeNullWhen(true)] out IEnumerable<CliError>? errors);

        private ICliCommandInfo? DefaultCommand
        {
            get => Cache.GetValue<ICliCommandInfo?>(() => null);
            set => Cache.SetValue(value);
        }

        private CliApplicationOptions AppOptions => Cache.GetValue(() => new CliApplicationOptions())!;
        private List<ICliCommandInfo> Commands => Cache.GetValue(() => new List<ICliCommandInfo>())!;
        private Mock<ICliCommandInfoCollection> CommandsMock => Cache.GetValue(() => CreateCommandsMock(Commands))!;

        [TestMethod]
        [DataRow(true, DisplayName = "Args: null")]
        [DataRow(false, DisplayName = "Args: empty array")]
        public void Parse_NoCommand_NoDefaultCommand(bool nullArgs)
        {
            var result = CallParse(nullArgs ? null! : Array.Empty<string>());

            Assert.IsFalse(result.Success);
            Assert.AreCollectionsEquivalent(new[] { CliErrorType.MissingCommand }, result.Errors.Select(x => x.Type));
        }

        [TestMethod]
        [DataRow(true, DisplayName = "Args: null")]
        [DataRow(false, DisplayName = "Args: empty array")]
        public void Parse_NoCommand_WithDefaultCommand(bool nullArgs)
        {
            var command = Mocks.Create<ICliCommandInfo>();
            command.Setup(x => x.CommandType).Returns(typeof(DummyClass)).Verifiable(Verifiables, Times.Once());
            DefaultCommand = command.Object;

            var result = CallParse(nullArgs ? null! : Array.Empty<string>());

            Assert.IsTrue(result.Success);
            Assert.AreSame(command.Object, result.Command);
            Assert.IsInstanceOfType<DummyClass>(result.Options);
        }

        [TestMethod]
        public void Parse_UnknownCommand()
        {
            var result = CallParse("blub");

            Assert.IsFalse(result.Success);
            Assert.AreCollectionsEquivalent(
                new[] { (CliErrorType.UnknownCommand, (string?)"blub") },
                result.Errors.Select(x => (x.Type, x.CommandName)));
        }

        [TestMethod]
        [DataRow("HELP", null, false, DisplayName = "Different casing")]
        [DataRow("help", null, false, DisplayName = "No command")]
        [DataRow("help", "blubbi", false, DisplayName = "Unknown command")]
        [DataRow("help", "blub", true, DisplayName = "Known command")]
        public void Parse_HelpCommand(string helpCommand, string commandArg, bool commandExpected)
        {
            var command = CreateCliCommandMock(new[] { "blub" });
            Commands.Add(command.Object);
            AppOptions.ProvideHelpCommand = true;

            var result = commandArg == null ? CallParse(helpCommand) : CallParse(helpCommand, commandArg);

            Assert.IsFalse(result.Success);
            Assert.AreCollectionsEquivalent(
                new[] { (CliErrorType.HelpRequested, commandExpected ? command.Object : null) },
                result.Errors.Select(x => (x.Type, x.AffectedCommand)));
        }

        [TestMethod]
        [DataRow("VERSION", null, false, DisplayName = "Different casing")]
        [DataRow("version", null, false, DisplayName = "No command")]
        [DataRow("version", "blubbi", false, DisplayName = "Unknown command")]
        [DataRow("version", "blub", true, DisplayName = "Known command")]
        public void Parse_VersionCommand(string versionCommand, string commandArg, bool commandExpected)
        {
            var command = CreateCliCommandMock(new[] { "blub" });
            Commands.Add(command.Object);
            AppOptions.ProvideVersionCommand = true;

            var result = commandArg == null ? CallParse(versionCommand) : CallParse(versionCommand, commandArg);

            Assert.IsFalse(result.Success);
            Assert.AreCollectionsEquivalent(
                new[] { (CliErrorType.VersionRequested, commandExpected ? command.Object : null) },
                result.Errors.Select(x => (x.Type, x.AffectedCommand)));
        }

        [TestMethod]
        [DataRow("help")]
        [DataRow("version")]
        public void Parse_SpecialCommand_Disabled(string commandName)
        {
            AppOptions.ProvideHelpCommand = false;
            AppOptions.ProvideVersionCommand = false;

            var result = CallParse(commandName);

            Assert.IsFalse(result.Success);
            Assert.AreCollectionsEquivalent(
                new[] { (CliErrorType.UnknownCommand, (string?)commandName) },
                result.Errors.Select(x => (x.Type, x.CommandName)));
        }

        [TestMethod]
        [DataRow("help")]
        [DataRow("version")]
        public void Parse_SpecialCommand_Existing(string commandName)
        {
            var command = CreateCliCommandMock(new[] { commandName });
            Commands.Add(command.Object);
            AppOptions.ProvideHelpCommand = true;
            AppOptions.ProvideVersionCommand = true;

            var result = CallParse(commandName);

            Assert.IsTrue(result.Success);
            Assert.AreSame(command.Object, result.Command);
            Assert.IsInstanceOfType<DummyClass>(result.Options);
        }

        [TestMethod]
        [DataRow("HELP")]
        [DataRow("help")]
        public void Parse_HelpOption_First(string helpCommand)
        {
            var command = CreateCliCommandMock(new[] { "blub" });
            Commands.Add(command.Object);
            AppOptions.ProvideHelpCommand = true;

            var result = CallParse("--" + helpCommand);

            Assert.IsFalse(result.Success);
            Assert.AreCollectionsEquivalent(
                new[] { (CliErrorType.HelpRequested, (ICliCommandInfo?)null) },
                result.Errors.Select(x => (x.Type, x.AffectedCommand)));
        }

        [TestMethod]
        [DataRow("VERSION")]
        [DataRow("version")]
        public void Parse_VersionOption_First(string versionCommand)
        {
            var command = CreateCliCommandMock(new[] { "blub" });
            Commands.Add(command.Object);
            AppOptions.ProvideVersionCommand = true;

            var result = CallParse("--" + versionCommand);

            Assert.IsFalse(result.Success);
            Assert.AreCollectionsEquivalent(
                new[] { (CliErrorType.VersionRequested, (ICliCommandInfo?)null) },
                result.Errors.Select(x => (x.Type, x.AffectedCommand)));
        }

        [TestMethod]
        [DataRow("help")]
        [DataRow("version")]
        public void Parse_SpecialOption_First_Disabled(string optionName)
        {
            AppOptions.ProvideHelpCommand = false;
            AppOptions.ProvideVersionCommand = false;

            var result = CallParse("--" + optionName);

            Assert.IsFalse(result.Success);
            Assert.AreCollectionsEquivalent(
                new[] { (CliErrorType.UnknownCommand, (string?)("--" + optionName)) },
                result.Errors.Select(x => (x.Type, x.CommandName)));
        }

        [TestMethod]
        [DataRow("help")]
        [DataRow("version")]
        public void Parse_SpecialOption_First_ExistingCommand(string commandName)
        {
            var command = CreateCliCommandMock(new[] { "--" + commandName });
            Commands.Add(command.Object);
            AppOptions.ProvideHelpCommand = true;
            AppOptions.ProvideVersionCommand = true;

            var result = CallParse("--" + commandName);

            Assert.IsTrue(result.Success);
            Assert.AreSame(command.Object, result.Command);
            Assert.IsInstanceOfType<DummyClass>(result.Options);
        }

        private CliArgumentParserResult CallParse(params string[] args)
            => CliArgumentParser.Parse(args, AppOptions, CommandsMock.Object);

        private Mock<ICliCommandInfoCollection> CreateCommandsMock(ICollection<ICliCommandInfo> commands)
        {
            var result = Mocks.Create<ICliCommandInfoCollection>();
            result.Setup(x => x.GetEnumerator()).Returns(() => commands.GetEnumerator());
            result.Setup(x => x.DefaultCommand).Returns(() => DefaultCommand);
            return result;
        }

        private Mock<ICliCommandInfo> CreateCliCommandMock(
            string[] aliases,
            ICliCommandOptionInfo[]? options = null,
            ICliCommandValueInfo[]? values = null,
            object? optionsInstance = null)
        {
            var command = Mocks.Create<ICliCommandInfo>();
            command.Setup(x => x.CommandType).Returns(typeof(DummyClass));
            command.Setup(x => x.Aliases).Returns(aliases);
            command.Setup(x => x.Options).Returns(options ?? Array.Empty<ICliCommandOptionInfo>());
            command.Setup(x => x.Values).Returns(values ?? Array.Empty<ICliCommandValueInfo>());
            command.Setup(x => x.OptionsInstance).Returns(optionsInstance);
            SetupValidation(command.As<ICliValidator<object>>(), true, null);
            return command;
        }

        private IVerifies SetupValidation<T>(Mock<ICliValidator<T>> mock, bool result, IEnumerable<CliError>? errors)
        {
            IEnumerable<CliError>? validationErrors;
            return mock
                .Setup(x => x.ValidateOptions(It.IsAny<ICliCommandInfo>(), It.IsAny<T>(), out validationErrors))
                .Returns(new CliValidatorDelegate((ICliCommandInfo c, object o, out IEnumerable<CliError>? e) =>
                {
                    e = errors;
                    return result;
                }));
        }

        private class DummyClass
        {
        }
    }
}
