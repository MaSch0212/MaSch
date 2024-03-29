﻿using MaSch.Console.Cli.Configuration;
using MaSch.Console.Cli.Runtime;
using MaSch.Console.Cli.Runtime.Executors;

namespace MaSch.Console.Cli.UnitTests.Runtime;

[TestClass]
public class CliCommandInfoFactoryTests : TestClassBase
{
    private CliCommandFactory Factory => Cache.GetValue(() => new CliCommandFactory())!;

    [TestMethod]
    public void From_TCommand()
    {
        var info = Factory.Create<DummyClass1>();

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
        var info = Factory.Create(instance);

        Assert.AreSame(instance, info.OptionsInstance);
    }

    [TestMethod]
    public void From_CommandType()
    {
        var info = Factory.Create(typeof(DummyClass1));

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
        _ = Assert.ThrowsException<ArgumentNullException>(() => Factory.Create(null!));
    }

    [TestMethod]
    public void From_CommandType_MissingAttribute()
    {
        _ = Assert.ThrowsException<ArgumentException>(() => Factory.Create(typeof(DummyClass3)));
    }

    [TestMethod]
    public void From_CommandType_EmptyCommandName()
    {
        _ = Assert.ThrowsException<ArgumentException>(() => Factory.Create(typeof(DummyClass4)));
    }

    [TestMethod]
    public void From_CommandType_WhitespaceCommandName()
    {
        _ = Assert.ThrowsException<ArgumentException>(() => Factory.Create(typeof(DummyClass5)));
    }

    [TestMethod]
    public void From_CommandType_NullCommandName()
    {
        _ = Assert.ThrowsException<ArgumentException>(() => Factory.Create(typeof(DummyClass6)));
    }

    [TestMethod]
    public void From_CommandType_CommandNameContainsSpace()
    {
        _ = Assert.ThrowsException<ArgumentException>(() => Factory.Create(typeof(DummyClass7)));
    }

    [TestMethod]
    public void From_CommandType_CommandNameContainsControlCharacter()
    {
        _ = Assert.ThrowsException<ArgumentException>(() => Factory.Create(typeof(DummyClass8)));
    }

    [TestMethod]
    public void From_CommandType_WithInstance()
    {
        var instance = new DummyClass1();
        var info = Factory.Create(typeof(DummyClass1), (object)instance);

        Assert.AreSame(instance, info.OptionsInstance);
    }

    [TestMethod]
    public void From_CommandType_Null_WithInstance()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() => Factory.Create((Type)null!, (object)new DummyClass1()));
    }

    [TestMethod]
    public void From_CommandType_WithInstance_WrongType()
    {
        _ = Assert.ThrowsException<ArgumentException>(() => Factory.Create(typeof(DummyClass1), (object)new DummyClass2()));
    }

    [TestMethod]
    public void From_CommandType_WithInstance_Null()
    {
        var info = Factory.Create(typeof(DummyClass1), (object?)null);

        Assert.IsNull(info.OptionsInstance);
    }

    [TestMethod]
    public void From_TCommand_TExecutor()
    {
        var info = Factory.Create<DummyClass2, Executor1>();

        var po = new PrivateObject(info);
        var executor = po.GetField("_executor");

        Assert.IsNotNull(executor);
        _ = Assert.IsInstanceOfType<ExternalExecutor<DummyClass2>>(executor);
    }

    [TestMethod]
    public void From_TCommand_TExecutor_WithExecutorInstance()
    {
        var expectedExecutor = Mocks.Create<Executor1>();
        var info = Factory.Create<DummyClass2, Executor1>(expectedExecutor.Object);

        var po = new PrivateObject(info);
        po = new PrivateObject(po.GetField("_executor"));
        var executor = po.GetField("_executorInstance");

        Assert.AreSame(expectedExecutor.Object, executor);
    }

    [TestMethod]
    public void From_TCommand_TExecutor_WithCommandInstance()
    {
        var command = new DummyClass2();
        var info = Factory.Create<DummyClass2, Executor1>(command);

        var po = new PrivateObject(info);
        var executor = po.GetField("_executor");

        Assert.AreSame(command, info.OptionsInstance);
        Assert.IsNotNull(executor);
        _ = Assert.IsInstanceOfType<ExternalExecutor<DummyClass2>>(executor);
    }

    [TestMethod]
    public void From_TCommand_TExecutor_WithCommandInstanceAndExecutorInstance()
    {
        var command = new DummyClass2();
        var expectedExecutor = Mocks.Create<Executor1>();
        var info = Factory.Create(command, expectedExecutor.Object);

        var po = new PrivateObject(info);
        po = new PrivateObject(po.GetField("_executor"));
        var executor = po.GetField("_executorInstance");

        Assert.AreSame(command, info.OptionsInstance);
        Assert.AreSame(expectedExecutor.Object, executor);
    }

    [TestMethod]
    public void From_CommandType_ExecutorType()
    {
        var info = Factory.Create(typeof(DummyClass2), typeof(Executor1));

        var po = new PrivateObject(info);
        var executor = po.GetField("_executor");

        Assert.IsNotNull(executor);
        _ = Assert.IsInstanceOfType<ExternalExecutor<DummyClass2>>(executor);
    }

    [TestMethod]
    public void From_CommandType_ExecutorType_WithExecutorInstance()
    {
        var expectedExecutor = Mocks.Create<Executor1>();
        var info = Factory.Create(typeof(DummyClass2), typeof(Executor1), expectedExecutor.Object);

        var po = new PrivateObject(info);
        po = new PrivateObject(po.GetField("_executor"));
        var executor = po.GetField("_executorInstance");

        Assert.AreSame(expectedExecutor.Object, executor);
    }

    [TestMethod]
    public void From_CommandType_ExecutorType_WithCommandInstance()
    {
        var command = new DummyClass2();
        var info = Factory.Create(typeof(DummyClass2), command, typeof(Executor1));

        var po = new PrivateObject(info);
        var executor = po.GetField("_executor");

        Assert.AreSame(command, info.OptionsInstance);
        Assert.IsNotNull(executor);
        _ = Assert.IsInstanceOfType<ExternalExecutor<DummyClass2>>(executor);
    }

    [TestMethod]
    public void From_CommandType_ExecutorType_WithCommandInstanceAndExecutorInstance()
    {
        var command = new DummyClass2();
        var expectedExecutor = Mocks.Create<Executor1>();
        var info = Factory.Create(typeof(DummyClass2), command, typeof(Executor1), expectedExecutor.Object);

        var po = new PrivateObject(info);
        po = new PrivateObject(po.GetField("_executor"));
        var executor = po.GetField("_executorInstance");

        Assert.AreSame(command, info.OptionsInstance);
        Assert.AreSame(expectedExecutor.Object, executor);
    }

    [TestMethod]
    public void From_TCommand_ExecutorFunc()
    {
        var func = Mocks.Create<Func<CliExecutionContext, DummyClass2, int>>();
        var info = Factory.Create(func.Object);

        var po = new PrivateObject(info);
        var cliExecutor = po.GetField("_executor");

        Assert.IsNotNull(cliExecutor);
        _ = Assert.IsInstanceOfType<FunctionExecutor<DummyClass2>>(cliExecutor);

        po = new PrivateObject(cliExecutor);
        var actualFunc = po.GetField("_executorFunc");

        Assert.AreSame(func.Object, actualFunc);
    }

    [TestMethod]
    public void From_TCommand_ExecutorFunc_WithCommandInstance()
    {
        var command = new DummyClass2();
        var func = Mocks.Create<Func<CliExecutionContext, DummyClass2, int>>();
        var info = Factory.Create(command, func.Object);

        var po = new PrivateObject(info);
        po = new PrivateObject(po.GetField("_executor"));
        var actualFunc = po.GetField("_executorFunc");

        Assert.AreSame(func.Object, actualFunc);
        Assert.AreSame(command, info.OptionsInstance);
    }

    [TestMethod]
    public void From_TCommand_AsyncExecutorFunc()
    {
        var func = Mocks.Create<Func<CliExecutionContext, DummyClass2, Task<int>>>();
        var info = Factory.Create(func.Object);

        var po = new PrivateObject(info);
        var cliExecutor = po.GetField("_executor");

        Assert.IsNotNull(cliExecutor);
        _ = Assert.IsInstanceOfType<FunctionExecutor<DummyClass2>>(cliExecutor);

        po = new PrivateObject(cliExecutor);
        var actualFunc = po.GetField("_asyncExecutorFunc");

        Assert.AreSame(func.Object, actualFunc);
    }

    [TestMethod]
    public void From_TCommand_AsyncExecutorFunc_WithCommandInstance()
    {
        var command = new DummyClass2();
        var func = Mocks.Create<Func<CliExecutionContext, DummyClass2, Task<int>>>();
        var info = Factory.Create(command, func.Object);

        var po = new PrivateObject(info);
        po = new PrivateObject(po.GetField("_executor"));
        var actualFunc = po.GetField("_asyncExecutorFunc");

        Assert.AreSame(func.Object, actualFunc);
        Assert.AreSame(command, info.OptionsInstance);
    }

    [TestMethod]
    public void From_CommandType_ExecutorFunc()
    {
        var func = Mocks.Create<Func<CliExecutionContext, object, int>>();
        var info = Factory.Create(typeof(DummyClass2), func.Object);

        var po = new PrivateObject(info);
        var cliExecutor = po.GetField("_executor");

        Assert.IsNotNull(cliExecutor);
        _ = Assert.IsInstanceOfType<FunctionExecutor<object>>(cliExecutor);

        po = new PrivateObject(cliExecutor);
        var actualFunc = po.GetField("_executorFunc");

        Assert.AreSame(func.Object, actualFunc);
    }

    [TestMethod]
    public void From_CommandType_ExecutorFunc_WithCommandInstance()
    {
        var command = new DummyClass2();
        var func = Mocks.Create<Func<CliExecutionContext, object, int>>();
        var info = Factory.Create(typeof(DummyClass2), command, func.Object);

        var po = new PrivateObject(info);
        po = new PrivateObject(po.GetField("_executor"));
        var actualFunc = po.GetField("_executorFunc");

        Assert.AreSame(func.Object, actualFunc);
        Assert.AreSame(command, info.OptionsInstance);
    }

    [TestMethod]
    public void From_CommandType_AsyncExecutorFunc()
    {
        var func = Mocks.Create<Func<CliExecutionContext, object, Task<int>>>();
        var info = Factory.Create(typeof(DummyClass2), func.Object);

        var po = new PrivateObject(info);
        var cliExecutor = po.GetField("_executor");

        Assert.IsNotNull(cliExecutor);
        _ = Assert.IsInstanceOfType<FunctionExecutor<object>>(cliExecutor);

        po = new PrivateObject(cliExecutor);
        var actualFunc = po.GetField("_asyncExecutorFunc");

        Assert.AreSame(func.Object, actualFunc);
    }

    [TestMethod]
    public void From_CommandType_AsyncExecutorFunc_WithCommandInstance()
    {
        var command = new DummyClass2();
        var func = Mocks.Create<Func<CliExecutionContext, object, Task<int>>>();
        var info = Factory.Create(typeof(DummyClass2), command, func.Object);

        var po = new PrivateObject(info);
        po = new PrivateObject(po.GetField("_executor"));
        var actualFunc = po.GetField("_asyncExecutorFunc");

        Assert.AreSame(func.Object, actualFunc);
        Assert.AreSame(command, info.OptionsInstance);
    }

    [CliCommand("Command1", IsDefault = true, HelpOrder = 4711, HelpText = "My Help Text")]
    public class DummyClass1
    {
    }

    [CliCommand("Command2", IsDefault = true, HelpOrder = 4711, HelpText = "My Help Text")]
    public class DummyClass2 : ICliExecutable
    {
        [ExcludeFromCodeCoverage]
        public int ExecuteCommand(CliExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class Executor1 : ICliExecutor<DummyClass2>, ICliAsyncExecutor<DummyClass2>, ICliValidator<DummyClass2>
    {
        public delegate bool ValidateDelegate(CliExecutionContext context, DummyClass2 parameters, [MaybeNullWhen(true)] out IEnumerable<CliError>? errors);

        public abstract int ExecuteCommand(CliExecutionContext context, DummyClass2 parameters);
        public abstract Task<int> ExecuteCommandAsync(CliExecutionContext context, DummyClass2 parameters);
        public abstract bool ValidateOptions(CliExecutionContext context, DummyClass2 parameters, [MaybeNullWhen(true)] out IEnumerable<CliError> errors);
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
    private abstract class DummyClass9 : ICliExecutable
    {
        public abstract int ExecuteCommand(CliExecutionContext context);
    }
}
