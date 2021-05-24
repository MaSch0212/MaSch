using MaSch.Console.Cli.Configuration;
using MaSch.Console.Cli.Runtime;
using MaSch.Console.Cli.Runtime.Executors;
using MaSch.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace MaSch.Console.Cli.Test.Runtime
{
    [TestClass]
    public class CliCommandInfoTests : TestClassBase
    {
        [TestMethod]
        public void From_TCommand()
        {
            var info = CliCommandInfo.From<DummyClass1>();

            Assert.AreEqual(typeof(DummyClass1), info.CommandType);
            Assert.IsNull(info.OptionsInstance);
            Assert.AreEqual("Command1", info.Name);
            Assert.AreCollectionsEquivalent(new[] { "Command1" }, info.Aliases);
            Assert.IsTrue(info.IsDefault);
            Assert.AreEqual("My Help Text", info.HelpText);
            Assert.AreEqual(4711, info.Order);
            Assert.IsFalse(info.IsExecutable);
        }

        [TestMethod]
        public void From_TCommand_WithInstance()
        {
            var instance = new DummyClass1();
            var info = CliCommandInfo.From(instance);

            Assert.AreSame(instance, info.OptionsInstance);
        }

        [TestMethod]
        public void From_CommandType()
        {
            var info = CliCommandInfo.From(typeof(DummyClass1));

            Assert.AreEqual(typeof(DummyClass1), info.CommandType);
            Assert.IsNull(info.OptionsInstance);
            Assert.AreEqual("Command1", info.Name);
            Assert.AreCollectionsEquivalent(new[] { "Command1" }, info.Aliases);
            Assert.IsTrue(info.IsDefault);
            Assert.AreEqual("My Help Text", info.HelpText);
            Assert.AreEqual(4711, info.Order);
            Assert.IsFalse(info.IsExecutable);
        }

        [TestMethod]
        public void From_CommandType_Null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => CliCommandInfo.From(null!));
        }

        [TestMethod]
        public void From_CommandType_MissingAttribute()
        {
            Assert.ThrowsException<ArgumentException>(() => CliCommandInfo.From(typeof(DummyClass3)));
        }

        [TestMethod]
        public void From_CommandType_EmptyCommandName()
        {
            Assert.ThrowsException<ArgumentException>(() => CliCommandInfo.From(typeof(DummyClass4)));
        }

        [TestMethod]
        public void From_CommandType_WhitespaceCommandName()
        {
            Assert.ThrowsException<ArgumentException>(() => CliCommandInfo.From(typeof(DummyClass5)));
        }

        [TestMethod]
        public void From_CommandType_NullCommandName()
        {
            Assert.ThrowsException<ArgumentException>(() => CliCommandInfo.From(typeof(DummyClass6)));
        }

        [TestMethod]
        public void From_CommandType_CommandNameContainsSpace()
        {
            Assert.ThrowsException<ArgumentException>(() => CliCommandInfo.From(typeof(DummyClass7)));
        }

        [TestMethod]
        public void From_CommandType_CommandNameContainsControlCharacter()
        {
            Assert.ThrowsException<ArgumentException>(() => CliCommandInfo.From(typeof(DummyClass8)));
        }

        [TestMethod]
        public void From_CommandType_WithInstance()
        {
            var instance = new DummyClass1();
            var info = CliCommandInfo.From(typeof(DummyClass1), (object)instance);

            Assert.AreSame(instance, info.OptionsInstance);
        }

        [TestMethod]
        public void From_CommandType_Null_WithInstance()
        {
            Assert.ThrowsException<ArgumentNullException>(() => CliCommandInfo.From((Type)null!, (object)new DummyClass1()));
        }

        [TestMethod]
        public void From_CommandType_WithInstance_WrongType()
        {
            Assert.ThrowsException<ArgumentException>(() => CliCommandInfo.From(typeof(DummyClass1), (object)new DummyClass2()));
        }

        [TestMethod]
        public void From_CommandType_WithInstance_Null()
        {
            var info = CliCommandInfo.From(typeof(DummyClass1), (object?)null);

            Assert.IsNull(info.OptionsInstance);
        }

        [TestMethod]
        public void From_CommandType_Executable()
        {
            Assert.ThrowsException<ArgumentException>(() => CliCommandInfo.From(typeof(DummyClass2)));
        }

        [TestMethod]
        public void From_TCommand_TExecutor()
        {
            var info = CliCommandInfo.From<DummyClass2, Executor1>();

            var po = new PrivateObject(info);
            var executor = po.GetField("_executor");

            Assert.IsNotNull(executor);
            Assert.IsInstanceOfType<ExternalExecutor<DummyClass2>>(executor);
        }

        [TestMethod]
        public void From_TCommand_TExecutor_WithExecutorInstance()
        {
            var expectedExecutor = Mocks.Create<Executor1>();
            var info = CliCommandInfo.From<DummyClass2, Executor1>(expectedExecutor.Object);

            var po = new PrivateObject(info);
            po = new PrivateObject(po.GetField("_executor"));
            var executor = po.GetField("_executorInstance");

            Assert.AreSame(expectedExecutor.Object, executor);
        }

        [TestMethod]
        public void From_TCommand_TExecutor_WithCommandInstance()
        {
            var command = new DummyClass2();
            var info = CliCommandInfo.From<DummyClass2, Executor1>(command);

            var po = new PrivateObject(info);
            var executor = po.GetField("_executor");

            Assert.AreSame(command, info.OptionsInstance);
            Assert.IsNotNull(executor);
            Assert.IsInstanceOfType<ExternalExecutor<DummyClass2>>(executor);
        }

        [TestMethod]
        public void From_TCommand_TExecutor_WithCommandInstanceAndExecutorInstance()
        {
            var command = new DummyClass2();
            var expectedExecutor = Mocks.Create<Executor1>();
            var info = CliCommandInfo.From(command, expectedExecutor.Object);

            var po = new PrivateObject(info);
            po = new PrivateObject(po.GetField("_executor"));
            var executor = po.GetField("_executorInstance");

            Assert.AreSame(command, info.OptionsInstance);
            Assert.AreSame(expectedExecutor.Object, executor);
        }

        [TestMethod]
        public void From_CommandType_ExecutorType()
        {
            var info = CliCommandInfo.From(typeof(DummyClass2), typeof(Executor1));

            var po = new PrivateObject(info);
            var executor = po.GetField("_executor");

            Assert.IsNotNull(executor);
            Assert.IsInstanceOfType<ExternalExecutor<DummyClass2>>(executor);
        }

        [TestMethod]
        public void From_CommandType_ExecutorType_WithExecutorInstance()
        {
            var expectedExecutor = Mocks.Create<Executor1>();
            var info = CliCommandInfo.From(typeof(DummyClass2), typeof(Executor1), expectedExecutor.Object);

            var po = new PrivateObject(info);
            po = new PrivateObject(po.GetField("_executor"));
            var executor = po.GetField("_executorInstance");

            Assert.AreSame(expectedExecutor.Object, executor);
        }

        [TestMethod]
        public void From_CommandType_ExecutorType_WithCommandInstance()
        {
            var command = new DummyClass2();
            var info = CliCommandInfo.From(typeof(DummyClass2), command, typeof(Executor1));

            var po = new PrivateObject(info);
            var executor = po.GetField("_executor");

            Assert.AreSame(command, info.OptionsInstance);
            Assert.IsNotNull(executor);
            Assert.IsInstanceOfType<ExternalExecutor<DummyClass2>>(executor);
        }

        [TestMethod]
        public void From_CommandType_ExecutorType_WithCommandInstanceAndExecutorInstance()
        {
            var command = new DummyClass2();
            var expectedExecutor = Mocks.Create<Executor1>();
            var info = CliCommandInfo.From(typeof(DummyClass2), command, typeof(Executor1), expectedExecutor.Object);

            var po = new PrivateObject(info);
            po = new PrivateObject(po.GetField("_executor"));
            var executor = po.GetField("_executorInstance");

            Assert.AreSame(command, info.OptionsInstance);
            Assert.AreSame(expectedExecutor.Object, executor);
        }

        [TestMethod]
        public void From_TCommand_ExecutorFunc()
        {
            var func = Mocks.Create<Func<DummyClass2, int>>();
            var info = CliCommandInfo.From(func.Object);

            var po = new PrivateObject(info);
            var cliExecutor = po.GetField("_executor");

            Assert.IsNotNull(cliExecutor);
            Assert.IsInstanceOfType<FunctionExecutor<DummyClass2>>(cliExecutor);

            po = new PrivateObject(cliExecutor);
            var actualFunc = po.GetField("_executorFunc");

            Assert.AreSame(func.Object, actualFunc);
        }

        [TestMethod]
        public void From_TCommand_ExecutorFunc_WithCommandInstance()
        {
            var command = new DummyClass2();
            var func = Mocks.Create<Func<DummyClass2, int>>();
            var info = CliCommandInfo.From(command, func.Object);

            var po = new PrivateObject(info);
            po = new PrivateObject(po.GetField("_executor"));
            var actualFunc = po.GetField("_executorFunc");

            Assert.AreSame(func.Object, actualFunc);
            Assert.AreSame(command, info.OptionsInstance);
        }

        [TestMethod]
        public void From_TCommand_AsyncExecutorFunc()
        {
            var func = Mocks.Create<Func<DummyClass2, Task<int>>>();
            var info = CliCommandInfo.From(func.Object);

            var po = new PrivateObject(info);
            var cliExecutor = po.GetField("_executor");

            Assert.IsNotNull(cliExecutor);
            Assert.IsInstanceOfType<FunctionExecutor<DummyClass2>>(cliExecutor);

            po = new PrivateObject(cliExecutor);
            var actualFunc = po.GetField("_asyncExecutorFunc");

            Assert.AreSame(func.Object, actualFunc);
        }

        [TestMethod]
        public void From_TCommand_AsyncExecutorFunc_WithCommandInstance()
        {
            var command = new DummyClass2();
            var func = Mocks.Create<Func<DummyClass2, Task<int>>>();
            var info = CliCommandInfo.From(command, func.Object);

            var po = new PrivateObject(info);
            po = new PrivateObject(po.GetField("_executor"));
            var actualFunc = po.GetField("_asyncExecutorFunc");

            Assert.AreSame(func.Object, actualFunc);
            Assert.AreSame(command, info.OptionsInstance);
        }

        [TestMethod]
        public void From_CommandType_ExecutorFunc()
        {
            var func = Mocks.Create<Func<object, int>>();
            var info = CliCommandInfo.From(typeof(DummyClass2), func.Object);

            var po = new PrivateObject(info);
            var cliExecutor = po.GetField("_executor");

            Assert.IsNotNull(cliExecutor);
            Assert.IsInstanceOfType<FunctionExecutor<object>>(cliExecutor);

            po = new PrivateObject(cliExecutor);
            var actualFunc = po.GetField("_executorFunc");

            Assert.AreSame(func.Object, actualFunc);
        }

        [TestMethod]
        public void From_CommandType_ExecutorFunc_WithCommandInstance()
        {
            var command = new DummyClass2();
            var func = Mocks.Create<Func<object, int>>();
            var info = CliCommandInfo.From(typeof(DummyClass2), command, func.Object);

            var po = new PrivateObject(info);
            po = new PrivateObject(po.GetField("_executor"));
            var actualFunc = po.GetField("_executorFunc");

            Assert.AreSame(func.Object, actualFunc);
            Assert.AreSame(command, info.OptionsInstance);
        }

        [TestMethod]
        public void From_CommandType_AsyncExecutorFunc()
        {
            var func = Mocks.Create<Func<object, Task<int>>>();
            var info = CliCommandInfo.From(typeof(DummyClass2), func.Object);

            var po = new PrivateObject(info);
            var cliExecutor = po.GetField("_executor");

            Assert.IsNotNull(cliExecutor);
            Assert.IsInstanceOfType<FunctionExecutor<object>>(cliExecutor);

            po = new PrivateObject(cliExecutor);
            var actualFunc = po.GetField("_asyncExecutorFunc");

            Assert.AreSame(func.Object, actualFunc);
        }

        [TestMethod]
        public void From_CommandType_AsyncExecutorFunc_WithCommandInstance()
        {
            var command = new DummyClass2();
            var func = Mocks.Create<Func<object, Task<int>>>();
            var info = CliCommandInfo.From(typeof(DummyClass2), command, func.Object);

            var po = new PrivateObject(info);
            po = new PrivateObject(po.GetField("_executor"));
            var actualFunc = po.GetField("_asyncExecutorFunc");

            Assert.AreSame(func.Object, actualFunc);
            Assert.AreSame(command, info.OptionsInstance);
        }

        [TestMethod]
        public void ValidateOptions_NotExecutable()
        {
            var options = new DummyClass1();
            var info = CliCommandInfo.From(options);

            var result = info.ValidateOptions(info, options, out var errors);

            Assert.IsTrue(result);
            Assert.IsNull(errors);
        }

        [TestMethod]
        public void ValidateOptions_Executable()
        {
            var executor = Mocks.Create<Executor1>();
            IEnumerable<CliError>? errors = null;
            var options = new DummyClass2();
            var info = CliCommandInfo.From(options, executor.Object);
            executor
                .Setup(x => x.ValidateOptions(info, options, out errors))
                .Returns(new Executor1.ValidateDelegate((ICliCommandInfo command, DummyClass2 parameters, out IEnumerable<CliError>? errors) =>
                {
                    errors = new[] { new CliError("My Error") };
                    return false;
                }))
                .Verifiable(Verifiables, Times.Once());

            var result = info.ValidateOptions(info, options, out var errors2);

            Assert.IsFalse(result);
            Assert.IsNotNull(errors2);
            Assert.AreCollectionsEquivalent(
                new[] { (CliErrorType.Custom, (string?)"My Error") },
                errors2.Select(x => (x.Type, x.CustomErrorMessage)));
        }

        [TestMethod]
        public void Execute_NotExecutable()
        {
            var executor = Mocks.Create<Executor1>();
            var options = new DummyClass2();
            var info = CliCommandInfo.From(options, executor.Object);
            executor.Setup(x => x.ExecuteCommand(options)).Returns(4711).Verifiable(Verifiables, Times.Once());

            var result = info.Execute(options);

            Assert.AreEqual(4711, result);
        }

        [TestMethod]
        public void Execute_Executable()
        {
            var info = CliCommandInfo.From<DummyClass1>();

            Assert.ThrowsException<InvalidOperationException>(() => info.Execute(new DummyClass1()));
        }

        [TestMethod]
        public async Task ExecuteAsync_NotExecutable()
        {
            var executor = Mocks.Create<Executor1>();
            var options = new DummyClass2();
            var info = CliCommandInfo.From(options, executor.Object);
            executor.Setup(x => x.ExecuteCommandAsync(options)).Returns(Task.FromResult(4711)).Verifiable(Verifiables, Times.Once());

            var result = await info.ExecuteAsync(options);

            Assert.AreEqual(4711, result);
        }

        [TestMethod]
        public async Task ExecuteAsync_Executable()
        {
            var info = CliCommandInfo.From<DummyClass1>();

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => await info.ExecuteAsync(new DummyClass1()));
        }

        [TestMethod]
        public void AddChildCommand()
        {
            var executorFunc = Mocks.Create<Func<DummyClass2, int>>();
            var parentInfo = CliCommandInfo.From<DummyClass1>();
            var childInfo = CliCommandInfo.From(executorFunc.Object);

            parentInfo.AddChildCommand(childInfo);

            Assert.AreSame(parentInfo, childInfo.ParentCommand);
            Assert.AreCollectionsEqual(new[] { childInfo }, parentInfo.ChildCommands);
        }

        [TestMethod]
        public void AddChildCommand_NotCliCommandInfo()
        {
            var parentInfo = CliCommandInfo.From<DummyClass1>();
            var childInfo = Mocks.Create<ICliCommandInfo>();

            parentInfo.AddChildCommand(childInfo.Object);

            Assert.AreCollectionsEqual(new[] { childInfo.Object }, parentInfo.ChildCommands);
        }

        [TestMethod]
        public void RemoveChildCommand()
        {
            var executorFunc = Mocks.Create<Func<DummyClass2, int>>();
            var parentInfo = CliCommandInfo.From<DummyClass1>();
            var childInfo = CliCommandInfo.From(executorFunc.Object);

            parentInfo.AddChildCommand(childInfo);
            parentInfo.RemoveChildCommand(childInfo);

            Assert.IsNull(childInfo.ParentCommand);
            Assert.AreCollectionsEqual(Array.Empty<ICliCommandInfo>(), parentInfo.ChildCommands);
        }

        [TestMethod]
        public void RemoveChildCommand_NotCorrectParentCommand()
        {
            var executorFunc = Mocks.Create<Func<DummyClass2, int>>();
            var parentInfo = CliCommandInfo.From<DummyClass1>();
            var childInfo = CliCommandInfo.From(executorFunc.Object);
            var otherCommand = Mocks.Create<ICliCommandInfo>();

            parentInfo.AddChildCommand(childInfo);
            new PrivateObject(childInfo).SetProperty(nameof(CliCommandInfo.ParentCommand), otherCommand.Object);
            parentInfo.RemoveChildCommand(childInfo);

            Assert.AreSame(otherCommand.Object, childInfo.ParentCommand);
            Assert.AreCollectionsEqual(Array.Empty<ICliCommandInfo>(), parentInfo.ChildCommands);
        }

        [TestMethod]
        public void RemoveChildCommand_NotCliCommandInfo()
        {
            var parentInfo = CliCommandInfo.From<DummyClass1>();
            var childInfo = Mocks.Create<ICliCommandInfo>();
            childInfo.Setup(x => x.ParentCommand).Returns(parentInfo);

            parentInfo.AddChildCommand(childInfo.Object);
            parentInfo.RemoveChildCommand(childInfo.Object);

            Assert.AreCollectionsEqual(Array.Empty<ICliCommandInfo>(), parentInfo.ChildCommands);
        }

        [CliCommand("Command1", IsDefault = true, HelpOrder = 4711, HelpText = "My Help Text", Executable = false)]
        public class DummyClass1
        {
        }

        [CliCommand("Command2", IsDefault = true, HelpOrder = 4711, HelpText = "My Help Text")]
        public class DummyClass2
        {
        }

        private class DummyClass3
        {
        }

        [CliCommand("")]
        private class DummyClass4
        {
        }

        [CliCommand("   \t")]
        private class DummyClass5
        {
        }

        [CliCommand(null!)]
        private class DummyClass6
        {
        }

        [CliCommand("blub blub")]
        private class DummyClass7
        {
        }

        [CliCommand("blub\u0001")]
        private class DummyClass8
        {
        }

        [CliCommand("Command9", IsDefault = true, HelpOrder = 4711, HelpText = "My Help Text")]
        private abstract class DummyClass9 : ICliCommandExecutor
        {
            public abstract int ExecuteCommand();
        }

        public abstract class Executor1 : ICliCommandExecutor<DummyClass2>, ICliAsyncCommandExecutor<DummyClass2>, ICliValidator<DummyClass2>
        {
            public delegate bool ValidateDelegate(ICliCommandInfo command, DummyClass2 parameters, [MaybeNullWhen(true)] out IEnumerable<CliError>? errors);

            public abstract int ExecuteCommand(DummyClass2 parameters);
            public abstract Task<int> ExecuteCommandAsync(DummyClass2 parameters);
            public abstract bool ValidateOptions(ICliCommandInfo command, DummyClass2 parameters, [MaybeNullWhen(true)] out IEnumerable<CliError> errors);
        }
    }
}
