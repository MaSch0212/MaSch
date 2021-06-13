using MaSch.Console.Cli.Configuration;
using MaSch.Console.Cli.Runtime;
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
        public void ValidateOptions_NotExecutable()
        {
            var options = new DummyClass1();
            var info = new CliCommandInfo(typeof(DummyClass1), null, options, null, null);

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
            var info = new CliCommandInfo(typeof(DummyClass2), typeof(Executor1), options, null, executor.Object);
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
            var info = new CliCommandInfo(typeof(DummyClass2), typeof(Executor1), options, null, executor.Object);
            executor.Setup(x => x.ExecuteCommand(info, options)).Returns(4711).Verifiable(Verifiables, Times.Once());

            var result = info.Execute(options);

            Assert.AreEqual(4711, result);
        }

        [TestMethod]
        public void Execute_Executable()
        {
            var info = new CliCommandInfo(typeof(DummyClass1), null, null, null, null);

            Assert.ThrowsException<InvalidOperationException>(() => info.Execute(new DummyClass1()));
        }

        [TestMethod]
        public async Task ExecuteAsync_NotExecutable()
        {
            var executor = Mocks.Create<Executor1>();
            var options = new DummyClass2();
            var info = new CliCommandInfo(typeof(DummyClass2), typeof(Executor1), options, null, executor.Object);
            executor.Setup(x => x.ExecuteCommandAsync(info, options)).Returns(Task.FromResult(4711)).Verifiable(Verifiables, Times.Once());

            var result = await info.ExecuteAsync(options);

            Assert.AreEqual(4711, result);
        }

        [TestMethod]
        public async Task ExecuteAsync_Executable()
        {
            var info = new CliCommandInfo(typeof(DummyClass1), null, null, null, null);

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => await info.ExecuteAsync(new DummyClass1()));
        }

        [TestMethod]
        public void AddChildCommand()
        {
            var executorFunc = Mocks.Create<Func<DummyClass2, int>>();
            var parentInfo = new CliCommandInfo(typeof(DummyClass1), null, null, null, null);
            var childInfo = new CliCommandInfo(typeof(DummyClass2), null, null, executorFunc.Object, null);

            parentInfo.AddChildCommand(childInfo);

            Assert.AreSame(parentInfo, childInfo.ParentCommand);
            Assert.AreCollectionsEqual(new[] { childInfo }, parentInfo.ChildCommands);
        }

        [TestMethod]
        public void AddChildCommand_NotCliCommandInfo()
        {
            var parentInfo = new CliCommandInfo(typeof(DummyClass1), null, null, null, null);
            var childInfo = Mocks.Create<ICliCommandInfo>();

            parentInfo.AddChildCommand(childInfo.Object);

            Assert.AreCollectionsEqual(new[] { childInfo.Object }, parentInfo.ChildCommands);
        }

        [TestMethod]
        public void RemoveChildCommand()
        {
            var executorFunc = Mocks.Create<Func<DummyClass2, int>>();
            var parentInfo = new CliCommandInfo(typeof(DummyClass1), null, null, null, null);
            var childInfo = new CliCommandInfo(typeof(DummyClass2), null, null, executorFunc.Object, null);

            parentInfo.AddChildCommand(childInfo);
            parentInfo.RemoveChildCommand(childInfo);

            Assert.IsNull(childInfo.ParentCommand);
            Assert.AreCollectionsEqual(Array.Empty<ICliCommandInfo>(), parentInfo.ChildCommands);
        }

        [TestMethod]
        public void RemoveChildCommand_NotCorrectParentCommand()
        {
            var executorFunc = Mocks.Create<Func<DummyClass2, int>>();
            var parentInfo = new CliCommandInfo(typeof(DummyClass1), null, null, null, null);
            var childInfo = new CliCommandInfo(typeof(DummyClass2), null, null, executorFunc.Object, null);
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
            var parentInfo = new CliCommandInfo(typeof(DummyClass1), null, null, null, null);
            var childInfo = Mocks.Create<ICliCommandInfo>();
            childInfo.Setup(x => x.ParentCommand).Returns(parentInfo);

            parentInfo.AddChildCommand(childInfo.Object);
            parentInfo.RemoveChildCommand(childInfo.Object);

            Assert.AreCollectionsEqual(Array.Empty<ICliCommandInfo>(), parentInfo.ChildCommands);
        }

        [TestMethod]
        public void Metadata()
        {
            var info = new CliCommandInfo(typeof(DummyClass3), null, null, null, null);

            Assert.AreEqual("MyDisplayName", info.DisplayName);
            Assert.AreEqual("MyVersion", info.Version);
            Assert.AreEqual("MyAuthor", info.Author);
            Assert.AreEqual("MyYear", info.Year);
            Assert.IsTrue(info.Hidden);
        }

        [CliCommand("Command1", IsDefault = true, HelpOrder = 4711, HelpText = "My Help Text", Executable = false)]
        public class DummyClass1
        {
        }

        [CliCommand("Command2", IsDefault = true, HelpOrder = 4711, HelpText = "My Help Text")]
        public class DummyClass2
        {
        }

        [CliCommand("Command3", DisplayName = "MyDisplayName", Version = "MyVersion", Author = "MyAuthor", Year = "MyYear", Hidden = true, Executable = false)]
        public class DummyClass3
        {
        }

        public abstract class Executor1 : ICliCommandExecutor<DummyClass2>, ICliAsyncCommandExecutor<DummyClass2>, ICliValidator<DummyClass2>
        {
            public delegate bool ValidateDelegate(ICliCommandInfo command, DummyClass2 parameters, [MaybeNullWhen(true)] out IEnumerable<CliError>? errors);

            public abstract int ExecuteCommand(ICliCommandInfo command, DummyClass2 parameters);
            public abstract Task<int> ExecuteCommandAsync(ICliCommandInfo command, DummyClass2 parameters);
            public abstract bool ValidateOptions(ICliCommandInfo command, DummyClass2 parameters, [MaybeNullWhen(true)] out IEnumerable<CliError> errors);
        }
    }
}
