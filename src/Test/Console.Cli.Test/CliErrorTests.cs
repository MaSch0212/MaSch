using MaSch.Console.Cli.Configuration;
using MaSch.Console.Cli.Runtime;
using MaSch.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MaSch.Console.Cli.Test.ErrorHandling
{
    [TestClass]
    public class CliErrorTests : TestClassBase
    {
        private ICliCommandInfoFactory Factory => Cache.GetValue(() => new CliCommandInfoFactory())!;

        [TestMethod]
        public void Ctor_Type()
        {
            var attr = new CliError((CliErrorType)4711);

            Assert.AreEqual((CliErrorType)4711, attr.Type);
        }

        [TestMethod]
        public void Ctor_Type_Command()
        {
            var command = Factory.Create<TestCommandOptions>();
            var attr = new CliError((CliErrorType)4711, command);

            Assert.AreEqual((CliErrorType)4711, attr.Type);
            Assert.AreSame(command, attr.AffectedCommand);
        }

        [TestMethod]
        public void Ctor_Type_Command_Option()
        {
            var command = Factory.Create<TestCommandOptions>();
            var attr = new CliError((CliErrorType)4711, command, command.Options[0]);

            Assert.AreEqual((CliErrorType)4711, attr.Type);
            Assert.AreSame(command, attr.AffectedCommand);
            Assert.AreSame(command.Options[0], attr.AffectedOption);
        }

        [TestMethod]
        public void Ctor_Type_Command_Value()
        {
            var command = Factory.Create<TestCommandOptions>();
            var attr = new CliError((CliErrorType)4711, command, command.Values[0]);

            Assert.AreEqual((CliErrorType)4711, attr.Type);
            Assert.AreSame(command, attr.AffectedCommand);
            Assert.AreSame(command.Values[0], attr.AffectedValue);
        }

        [TestMethod]
        public void Ctor_Message()
        {
            var attr = new CliError("MyError");

            Assert.AreEqual(CliErrorType.Custom, attr.Type);
            Assert.AreEqual("MyError", attr.CustomErrorMessage);
        }

        [TestMethod]
        public void Ctor_Message_Command()
        {
            var command = Factory.Create<TestCommandOptions>();
            var attr = new CliError("MyError", command);

            Assert.AreEqual(CliErrorType.Custom, attr.Type);
            Assert.AreEqual("MyError", attr.CustomErrorMessage);
            Assert.AreSame(command, attr.AffectedCommand);
        }

        [TestMethod]
        public void Ctor_MessageCommand_Option()
        {
            var command = Factory.Create<TestCommandOptions>();
            var attr = new CliError("MyError", command, command.Options[0]);

            Assert.AreEqual(CliErrorType.Custom, attr.Type);
            Assert.AreEqual("MyError", attr.CustomErrorMessage);
            Assert.AreSame(command, attr.AffectedCommand);
            Assert.AreSame(command.Options[0], attr.AffectedOption);
        }

        [TestMethod]
        public void Ctor_Message_Command_Value()
        {
            var command = Factory.Create<TestCommandOptions>();
            var attr = new CliError("MyError", command, command.Values[0]);

            Assert.AreEqual(CliErrorType.Custom, attr.Type);
            Assert.AreEqual("MyError", attr.CustomErrorMessage);
            Assert.AreSame(command, attr.AffectedCommand);
            Assert.AreSame(command.Values[0], attr.AffectedValue);
        }

        [TestMethod]
        public void InitProperties()
        {
            var testEx = new Exception();
            var attr = new CliError((CliErrorType)4711)
            {
                CommandName = "MyCommandName",
                OptionName = "MyOptionName",
                Exception = testEx,
            };

            Assert.AreEqual("MyCommandName", attr.CommandName);
            Assert.AreEqual("MyOptionName", attr.OptionName);
            Assert.AreSame(testEx, attr.Exception);
        }

        [CliCommand("blub", Executable = false)]
        private class TestCommandOptions
        {
            [CliCommandOption("my-option")]
            public string? MyOptions { get; set; }

            [CliCommandValue(0, "MyValue")]
            public string? MyValue { get; set; }
        }
    }
}
