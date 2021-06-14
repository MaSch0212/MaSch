using MaSch.Console.Cli.Runtime;
using MaSch.Console.Cli.Runtime.Executors;
using MaSch.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace MaSch.Console.Cli.Test.Runtime.Executors
{
    [TestClass]
    public class FunctionExecutorTests : TestClassBase
    {
        [TestMethod]
        public void GetExecutor_Null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => FunctionExecutor.GetExecutor(null!));
        }

        [TestMethod]
        public void GetExecutor_NotAFunction()
        {
            var m1 = Mocks.Create<Action<object>>();
            var m2 = Mocks.Create<IDisposable>();

            Assert.ThrowsException<ArgumentException>(() => FunctionExecutor.GetExecutor(m1.Object));
            Assert.ThrowsException<ArgumentException>(() => FunctionExecutor.GetExecutor(m2.Object));
        }

        [TestMethod]
        public void GetExecutor_WrongFunctionType()
        {
            var m = Mocks.Create<Func<object, bool, bool, bool>>();
            var m2 = Mocks.Create<Func<CliExecutionContext, bool, bool>>();
            var m3 = Mocks.Create<Func<object, bool, int>>();

            Assert.ThrowsException<ArgumentException>(() => FunctionExecutor.GetExecutor(m.Object));
            Assert.ThrowsException<ArgumentException>(() => FunctionExecutor.GetExecutor(m2.Object));
            Assert.ThrowsException<ArgumentException>(() => FunctionExecutor.GetExecutor(m3.Object));
        }

        [TestMethod]
        public void GetExecutor_WrongReturnValue()
        {
            var m = Mocks.Create<Func<CliExecutionContext, object, bool>>();

            var ex = Assert.ThrowsException<ArgumentException>(() => FunctionExecutor.GetExecutor(m.Object));
            Assert.ContainsAll(new[] { "int", "Task<int>" }, ex.Message);
        }

        [TestMethod]
        public void GetExecutor_Sync()
        {
            var m = Mocks.Create<Func<CliExecutionContext, IDisposable, int>>();

            var e = FunctionExecutor.GetExecutor(m.Object);

            Assert.IsNotNull(e);
            Assert.IsInstanceOfType<FunctionExecutor<IDisposable>>(e);
            Assert.AreSame(m.Object, new PrivateObject(e).GetField("_executorFunc"));
        }

        [TestMethod]
        public void GetExecutor_Async()
        {
            var m = Mocks.Create<Func<CliExecutionContext, IDisposable, Task<int>>>();

            var e = FunctionExecutor.GetExecutor(m.Object);

            Assert.IsNotNull(e);
            Assert.IsInstanceOfType<FunctionExecutor<IDisposable>>(e);
            Assert.AreSame(m.Object, new PrivateObject(e).GetField("_asyncExecutorFunc"));
        }

        [TestMethod]
        public void Ctor_BothNull()
        {
            Assert.ThrowsException<ArgumentException>(() => new FunctionExecutor<object>(null, null));
        }

        [TestMethod]
        public void Execute_NullObj()
        {
            var m = Mocks.Create<Func<CliExecutionContext, IDisposable, int>>();
            var e = new FunctionExecutor<IDisposable>(m.Object, null);
            var cMock = Mocks.Create<ICliCommandInfo>();
            var appMock = Mocks.Create<ICliApplicationBase>();
            var execCtx = new CliExecutionContext(appMock.Object, cMock.Object);

            Assert.ThrowsException<ArgumentNullException>(() => e.Execute(execCtx, null!));
        }

        [TestMethod]
        public void Execute_WrongType()
        {
            var m = Mocks.Create<Func<CliExecutionContext, IDisposable, int>>();
            var e = new FunctionExecutor<IDisposable>(m.Object, null);
            var cMock = Mocks.Create<ICliCommandInfo>();
            var appMock = Mocks.Create<ICliApplicationBase>();
            var execCtx = new CliExecutionContext(appMock.Object, cMock.Object);

            Assert.ThrowsException<ArgumentException>(() => e.Execute(execCtx, new object()));
        }

        [TestMethod]
        public void Execute_NoExecutorFunc()
        {
            var o = Mocks.Create<IDisposable>();
            var m = Mocks.Create<Func<CliExecutionContext, IDisposable, int>>();
            var e = new FunctionExecutor<IDisposable>(m.Object, null);
            new PrivateObject(e).SetField("_executorFunc", null);
            var cMock = Mocks.Create<ICliCommandInfo>();
            var appMock = Mocks.Create<ICliApplicationBase>();
            var execCtx = new CliExecutionContext(appMock.Object, cMock.Object);

            Assert.ThrowsException<InvalidOperationException>(() => e.Execute(execCtx, o.Object));
        }

        [TestMethod]
        public void Execute_Sync()
        {
            var o = Mocks.Create<IDisposable>();
            var m = Mocks.Create<Func<CliExecutionContext, IDisposable, int>>();
            var e = new FunctionExecutor<IDisposable>(m.Object, null);
            var cMock = Mocks.Create<ICliCommandInfo>();
            var appMock = Mocks.Create<ICliApplicationBase>();
            var execCtx = new CliExecutionContext(appMock.Object, cMock.Object);
            m.Setup(x => x(execCtx, o.Object)).Returns(4711).Verifiable(Verifiables, Times.Once());

            var result = e.Execute(execCtx, o.Object);

            Assert.AreEqual(4711, result);
        }

        [TestMethod]
        public void Execute_Async()
        {
            var o = Mocks.Create<IDisposable>();
            var m = Mocks.Create<Func<CliExecutionContext, IDisposable, Task<int>>>();
            var e = new FunctionExecutor<IDisposable>(null, m.Object);
            var cMock = Mocks.Create<ICliCommandInfo>();
            var appMock = Mocks.Create<ICliApplicationBase>();
            var execCtx = new CliExecutionContext(appMock.Object, cMock.Object);
            m.Setup(x => x(execCtx, o.Object)).Returns(Task.Delay(10).ContinueWith(x => 4711)).Verifiable(Verifiables, Times.Once());

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
            var cMock = Mocks.Create<ICliCommandInfo>();
            var appMock = Mocks.Create<ICliApplicationBase>();
            var execCtx = new CliExecutionContext(appMock.Object, cMock.Object);
            sm.Setup(x => x(execCtx, o.Object)).Returns(4711).Verifiable(Verifiables, Times.Once());
            am.Setup(x => x(execCtx, o.Object)).Returns(Task.Delay(10).ContinueWith(x => 4711)).Verifiable(Verifiables, Times.Never());

            var result = e.Execute(execCtx, o.Object);

            Assert.AreEqual(4711, result);
        }

        [TestMethod]
        public async Task ExecuteAsync_NullObj()
        {
            var m = Mocks.Create<Func<CliExecutionContext, IDisposable, Task<int>>>();
            var e = new FunctionExecutor<IDisposable>(null, m.Object);
            var cMock = Mocks.Create<ICliCommandInfo>();
            var appMock = Mocks.Create<ICliApplicationBase>();
            var execCtx = new CliExecutionContext(appMock.Object, cMock.Object);

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => e.ExecuteAsync(execCtx, null!));
        }

        [TestMethod]
        public async Task ExecuteAsync_WrongType()
        {
            var m = Mocks.Create<Func<CliExecutionContext, IDisposable, Task<int>>>();
            var e = new FunctionExecutor<IDisposable>(null, m.Object);
            var cMock = Mocks.Create<ICliCommandInfo>();
            var appMock = Mocks.Create<ICliApplicationBase>();
            var execCtx = new CliExecutionContext(appMock.Object, cMock.Object);

            await Assert.ThrowsExceptionAsync<ArgumentException>(() => e.ExecuteAsync(execCtx, new object()));
        }

        [TestMethod]
        public async Task ExecuteAsync_NoExecutorFunc()
        {
            var o = Mocks.Create<IDisposable>();
            var m = Mocks.Create<Func<CliExecutionContext, IDisposable, Task<int>>>();
            var e = new FunctionExecutor<IDisposable>(null, m.Object);
            new PrivateObject(e).SetField("_asyncExecutorFunc", null);
            var cMock = Mocks.Create<ICliCommandInfo>();
            var appMock = Mocks.Create<ICliApplicationBase>();
            var execCtx = new CliExecutionContext(appMock.Object, cMock.Object);

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => e.ExecuteAsync(execCtx, o.Object));
        }

        [TestMethod]
        public async Task ExecuteAsync_Sync()
        {
            var o = Mocks.Create<IDisposable>();
            var m = Mocks.Create<Func<CliExecutionContext, IDisposable, int>>();
            var e = new FunctionExecutor<IDisposable>(m.Object, null);
            var cMock = Mocks.Create<ICliCommandInfo>();
            var appMock = Mocks.Create<ICliApplicationBase>();
            var execCtx = new CliExecutionContext(appMock.Object, cMock.Object);
            m.Setup(x => x(execCtx, o.Object)).Returns(4711).Verifiable(Verifiables, Times.Once());

            var result = await e.ExecuteAsync(execCtx, o.Object);

            Assert.AreEqual(4711, result);
        }

        [TestMethod]
        public async Task ExecuteAsync_Async()
        {
            var o = Mocks.Create<IDisposable>();
            var m = Mocks.Create<Func<CliExecutionContext, IDisposable, Task<int>>>();
            var e = new FunctionExecutor<IDisposable>(null, m.Object);
            var cMock = Mocks.Create<ICliCommandInfo>();
            var appMock = Mocks.Create<ICliApplicationBase>();
            var execCtx = new CliExecutionContext(appMock.Object, cMock.Object);
            m.Setup(x => x(execCtx, o.Object)).Returns(Task.Delay(10).ContinueWith(x => 4711)).Verifiable(Verifiables, Times.Once());

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
            var cMock = Mocks.Create<ICliCommandInfo>();
            var appMock = Mocks.Create<ICliApplicationBase>();
            var execCtx = new CliExecutionContext(appMock.Object, cMock.Object);
            sm.Setup(x => x(execCtx, o.Object)).Returns(4711).Verifiable(Verifiables, Times.Never());
            am.Setup(x => x(execCtx, o.Object)).Returns(Task.Delay(10).ContinueWith(x => 4711)).Verifiable(Verifiables, Times.Once());

            var result = await e.ExecuteAsync(execCtx, o.Object);

            Assert.AreEqual(4711, result);
        }
    }
}
