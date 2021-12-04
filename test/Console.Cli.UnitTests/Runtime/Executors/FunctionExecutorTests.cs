using MaSch.Console.Cli.Runtime;
using MaSch.Console.Cli.Runtime.Executors;

namespace MaSch.Console.Cli.UnitTests.Runtime.Executors;

[TestClass]
public class FunctionExecutorTests : TestClassBase
{
    [TestMethod]
    public void GetExecutor_Null()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() => FunctionExecutor.GetExecutor(null!));
    }

    [TestMethod]
    public void GetExecutor_NotAFunction()
    {
        var functionMock1 = Mocks.Create<Action<object>>();
        var functionMock2 = Mocks.Create<IDisposable>();

        _ = Assert.ThrowsException<ArgumentException>(() => FunctionExecutor.GetExecutor(functionMock1.Object));
        _ = Assert.ThrowsException<ArgumentException>(() => FunctionExecutor.GetExecutor(functionMock2.Object));
    }

    [TestMethod]
    public void GetExecutor_WrongFunctionType()
    {
        var functionMock1 = Mocks.Create<Func<object, bool, bool, bool>>();
        var functionMock2 = Mocks.Create<Func<CliExecutionContext, bool, bool>>();
        var functionMock3 = Mocks.Create<Func<object, bool, int>>();

        _ = Assert.ThrowsException<ArgumentException>(() => FunctionExecutor.GetExecutor(functionMock1.Object));
        _ = Assert.ThrowsException<ArgumentException>(() => FunctionExecutor.GetExecutor(functionMock2.Object));
        _ = Assert.ThrowsException<ArgumentException>(() => FunctionExecutor.GetExecutor(functionMock3.Object));
    }

    [TestMethod]
    public void GetExecutor_WrongReturnValue()
    {
        var functionMock = Mocks.Create<Func<CliExecutionContext, object, bool>>();

        var ex = Assert.ThrowsException<ArgumentException>(() => FunctionExecutor.GetExecutor(functionMock.Object));
        Assert.ContainsAll(new[] { "int", "Task<int>" }, ex.Message);
    }

    [TestMethod]
    public void GetExecutor_Sync()
    {
        var functionMock = Mocks.Create<Func<CliExecutionContext, IDisposable, int>>();

        var e = FunctionExecutor.GetExecutor(functionMock.Object);

        Assert.IsNotNull(e);
        _ = Assert.IsInstanceOfType<FunctionExecutor<IDisposable>>(e);
        Assert.AreSame(functionMock.Object, new PrivateObject(e).GetField("_executorFunc"));
    }

    [TestMethod]
    public void GetExecutor_Async()
    {
        var functionMock = Mocks.Create<Func<CliExecutionContext, IDisposable, Task<int>>>();

        var e = FunctionExecutor.GetExecutor(functionMock.Object);

        Assert.IsNotNull(e);
        _ = Assert.IsInstanceOfType<FunctionExecutor<IDisposable>>(e);
        Assert.AreSame(functionMock.Object, new PrivateObject(e).GetField("_asyncExecutorFunc"));
    }

    [TestMethod]
    public void Ctor_BothNull()
    {
        _ = Assert.ThrowsException<ArgumentException>(() => new FunctionExecutor<object>(null, null));
    }

    [TestMethod]
    public void Execute_NullObj()
    {
        var functionMock = Mocks.Create<Func<CliExecutionContext, IDisposable, int>>();
        var e = new FunctionExecutor<IDisposable>(functionMock.Object, null);
        var commandMock = Mocks.Create<ICliCommandInfo>();
        var serviceProviderMock = Mocks.Create<IServiceProvider>();
        var execCtx = new CliExecutionContext(serviceProviderMock.Object, commandMock.Object);

        _ = Assert.ThrowsException<ArgumentNullException>(() => e.Execute(execCtx, null!));
    }

    [TestMethod]
    public void Execute_WrongType()
    {
        var functionMock = Mocks.Create<Func<CliExecutionContext, IDisposable, int>>();
        var e = new FunctionExecutor<IDisposable>(functionMock.Object, null);
        var commandMock = Mocks.Create<ICliCommandInfo>();
        var serviceProviderMock = Mocks.Create<IServiceProvider>();
        var execCtx = new CliExecutionContext(serviceProviderMock.Object, commandMock.Object);

        _ = Assert.ThrowsException<ArgumentException>(() => e.Execute(execCtx, new object()));
    }

    [TestMethod]
    public void Execute_NoExecutorFunc()
    {
        var o = Mocks.Create<IDisposable>();
        var functionMock = Mocks.Create<Func<CliExecutionContext, IDisposable, int>>();
        var e = new FunctionExecutor<IDisposable>(functionMock.Object, null);
        new PrivateObject(e).SetField("_executorFunc", null);
        var commandMock = Mocks.Create<ICliCommandInfo>();
        var serviceProviderMock = Mocks.Create<IServiceProvider>();
        var execCtx = new CliExecutionContext(serviceProviderMock.Object, commandMock.Object);

        _ = Assert.ThrowsException<InvalidOperationException>(() => e.Execute(execCtx, o.Object));
    }

    [TestMethod]
    public void Execute_Sync()
    {
        var o = Mocks.Create<IDisposable>();
        var functionMock = Mocks.Create<Func<CliExecutionContext, IDisposable, int>>();
        var e = new FunctionExecutor<IDisposable>(functionMock.Object, null);
        var commandMock = Mocks.Create<ICliCommandInfo>();
        var serviceProviderMock = Mocks.Create<IServiceProvider>();
        var execCtx = new CliExecutionContext(serviceProviderMock.Object, commandMock.Object);
        _ = functionMock.Setup(x => x(execCtx, o.Object)).Returns(4711).Verifiable(Verifiables, Times.Once());

        var result = e.Execute(execCtx, o.Object);

        Assert.AreEqual(4711, result);
    }

    [TestMethod]
    public void Execute_Async()
    {
        var o = Mocks.Create<IDisposable>();
        var functionMock = Mocks.Create<Func<CliExecutionContext, IDisposable, Task<int>>>();
        var e = new FunctionExecutor<IDisposable>(null, functionMock.Object);
        var commandMock = Mocks.Create<ICliCommandInfo>();
        var serviceProviderMock = Mocks.Create<IServiceProvider>();
        var execCtx = new CliExecutionContext(serviceProviderMock.Object, commandMock.Object);
        _ = functionMock.Setup(x => x(execCtx, o.Object)).Returns(Task.Delay(10).ContinueWith(x => 4711)).Verifiable(Verifiables, Times.Once());

        var result = e.Execute(execCtx, o.Object);

        Assert.AreEqual(4711, result);
    }

    [TestMethod]
    public void Execute_Both()
    {
        var o = Mocks.Create<IDisposable>();
        var sm = Mocks.Create<Func<CliExecutionContext, IDisposable, int>>();
        var am = Mocks.Create<Func<CliExecutionContext, IDisposable, Task<int>>>();
        var e = new FunctionExecutor<IDisposable>(sm.Object, am.Object);
        var commandMock = Mocks.Create<ICliCommandInfo>();
        var serviceProviderMock = Mocks.Create<IServiceProvider>();
        var execCtx = new CliExecutionContext(serviceProviderMock.Object, commandMock.Object);
        _ = sm.Setup(x => x(execCtx, o.Object)).Returns(4711).Verifiable(Verifiables, Times.Once());
        _ = am.Setup(x => x(execCtx, o.Object)).Returns(Task.Delay(10).ContinueWith(x => 4711)).Verifiable(Verifiables, Times.Never());

        var result = e.Execute(execCtx, o.Object);

        Assert.AreEqual(4711, result);
    }

    [TestMethod]
    public async Task ExecuteAsync_NullObj()
    {
        var functionMock = Mocks.Create<Func<CliExecutionContext, IDisposable, Task<int>>>();
        var e = new FunctionExecutor<IDisposable>(null, functionMock.Object);
        var commandMock = Mocks.Create<ICliCommandInfo>();
        var serviceProviderMock = Mocks.Create<IServiceProvider>();
        var execCtx = new CliExecutionContext(serviceProviderMock.Object, commandMock.Object);

        _ = await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => e.ExecuteAsync(execCtx, null!));
    }

    [TestMethod]
    public async Task ExecuteAsync_WrongType()
    {
        var functionMock = Mocks.Create<Func<CliExecutionContext, IDisposable, Task<int>>>();
        var e = new FunctionExecutor<IDisposable>(null, functionMock.Object);
        var commandMock = Mocks.Create<ICliCommandInfo>();
        var serviceProviderMock = Mocks.Create<IServiceProvider>();
        var execCtx = new CliExecutionContext(serviceProviderMock.Object, commandMock.Object);

        _ = await Assert.ThrowsExceptionAsync<ArgumentException>(() => e.ExecuteAsync(execCtx, new object()));
    }

    [TestMethod]
    public async Task ExecuteAsync_NoExecutorFunc()
    {
        var o = Mocks.Create<IDisposable>();
        var functionMock = Mocks.Create<Func<CliExecutionContext, IDisposable, Task<int>>>();
        var e = new FunctionExecutor<IDisposable>(null, functionMock.Object);
        new PrivateObject(e).SetField("_asyncExecutorFunc", null);
        var commandMock = Mocks.Create<ICliCommandInfo>();
        var serviceProviderMock = Mocks.Create<IServiceProvider>();
        var execCtx = new CliExecutionContext(serviceProviderMock.Object, commandMock.Object);

        _ = await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => e.ExecuteAsync(execCtx, o.Object));
    }

    [TestMethod]
    public async Task ExecuteAsync_Sync()
    {
        var o = Mocks.Create<IDisposable>();
        var functionMock = Mocks.Create<Func<CliExecutionContext, IDisposable, int>>();
        var e = new FunctionExecutor<IDisposable>(functionMock.Object, null);
        var commandMock = Mocks.Create<ICliCommandInfo>();
        var serviceProviderMock = Mocks.Create<IServiceProvider>();
        var execCtx = new CliExecutionContext(serviceProviderMock.Object, commandMock.Object);
        _ = functionMock.Setup(x => x(execCtx, o.Object)).Returns(4711).Verifiable(Verifiables, Times.Once());

        var result = await e.ExecuteAsync(execCtx, o.Object);

        Assert.AreEqual(4711, result);
    }

    [TestMethod]
    public async Task ExecuteAsync_Async()
    {
        var o = Mocks.Create<IDisposable>();
        var functionMock = Mocks.Create<Func<CliExecutionContext, IDisposable, Task<int>>>();
        var e = new FunctionExecutor<IDisposable>(null, functionMock.Object);
        var commandMock = Mocks.Create<ICliCommandInfo>();
        var serviceProviderMock = Mocks.Create<IServiceProvider>();
        var execCtx = new CliExecutionContext(serviceProviderMock.Object, commandMock.Object);
        _ = functionMock.Setup(x => x(execCtx, o.Object)).Returns(Task.Delay(10).ContinueWith(x => 4711)).Verifiable(Verifiables, Times.Once());

        var result = await e.ExecuteAsync(execCtx, o.Object);

        Assert.AreEqual(4711, result);
    }

    [TestMethod]
    public async Task ExecuteAsync_Both()
    {
        var o = Mocks.Create<IDisposable>();
        var sm = Mocks.Create<Func<CliExecutionContext, IDisposable, int>>();
        var am = Mocks.Create<Func<CliExecutionContext, IDisposable, Task<int>>>();
        var e = new FunctionExecutor<IDisposable>(sm.Object, am.Object);
        var commandMock = Mocks.Create<ICliCommandInfo>();
        var serviceProviderMock = Mocks.Create<IServiceProvider>();
        var execCtx = new CliExecutionContext(serviceProviderMock.Object, commandMock.Object);
        _ = sm.Setup(x => x(execCtx, o.Object)).Returns(4711).Verifiable(Verifiables, Times.Never());
        _ = am.Setup(x => x(execCtx, o.Object)).Returns(Task.Delay(10).ContinueWith(x => 4711)).Verifiable(Verifiables, Times.Once());

        var result = await e.ExecuteAsync(execCtx, o.Object);

        Assert.AreEqual(4711, result);
    }
}
