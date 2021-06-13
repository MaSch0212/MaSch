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
            var m = Mocks.Create<Func<object, bool, bool>>();

            Assert.ThrowsException<ArgumentException>(() => FunctionExecutor.GetExecutor(m.Object));
        }

        [TestMethod]
        public void GetExecutor_WrongReturnValue()
        {
            var m = Mocks.Create<Func<object, bool>>();

            var ex = Assert.ThrowsException<ArgumentException>(() => FunctionExecutor.GetExecutor(m.Object));
            Assert.ContainsAll(new[] { "int", "Task<int>" }, ex.Message);
        }

        [TestMethod]
        public void GetExecutor_Sync()
        {
            var m = Mocks.Create<Func<IDisposable, int>>();

            var e = FunctionExecutor.GetExecutor(m.Object);

            Assert.IsNotNull(e);
            Assert.IsInstanceOfType<FunctionExecutor<IDisposable>>(e);
            Assert.AreSame(m.Object, new PrivateObject(e).GetField("_executorFunc"));
        }

        [TestMethod]
        public void GetExecutor_Async()
        {
            var m = Mocks.Create<Func<IDisposable, Task<int>>>();

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
            var m = Mocks.Create<Func<IDisposable, int>>();
            var e = new FunctionExecutor<IDisposable>(m.Object, null);
            var cMock = Mocks.Create<ICliCommandInfo>();

            Assert.ThrowsException<ArgumentNullException>(() => e.Execute(cMock.Object, null!));
        }

        [TestMethod]
        public void Execute_WrongType()
        {
            var m = Mocks.Create<Func<IDisposable, int>>();
            var e = new FunctionExecutor<IDisposable>(m.Object, null);
            var cMock = Mocks.Create<ICliCommandInfo>();

            Assert.ThrowsException<ArgumentException>(() => e.Execute(cMock.Object, new object()));
        }

        [TestMethod]
        public void Execute_NoExecutorFunc()
        {
            var o = Mocks.Create<IDisposable>();
            var m = Mocks.Create<Func<IDisposable, int>>();
            var e = new FunctionExecutor<IDisposable>(m.Object, null);
            new PrivateObject(e).SetField("_executorFunc", null);
            var cMock = Mocks.Create<ICliCommandInfo>();

            Assert.ThrowsException<InvalidOperationException>(() => e.Execute(cMock.Object, o.Object));
        }

        [TestMethod]
        public void Execute_Sync()
        {
            var o = Mocks.Create<IDisposable>();
            var m = Mocks.Create<Func<IDisposable, int>>();
            var e = new FunctionExecutor<IDisposable>(m.Object, null);
            var cMock = Mocks.Create<ICliCommandInfo>();
            m.Setup(x => x(o.Object)).Returns(4711).Verifiable(Verifiables, Times.Once());

            var result = e.Execute(cMock.Object, o.Object);

            Assert.AreEqual(4711, result);
        }

        [TestMethod]
        public void Execute_Async()
        {
            var o = Mocks.Create<IDisposable>();
            var m = Mocks.Create<Func<IDisposable, Task<int>>>();
            var e = new FunctionExecutor<IDisposable>(null, m.Object);
            var cMock = Mocks.Create<ICliCommandInfo>();
            m.Setup(x => x(o.Object)).Returns(Task.Delay(10).ContinueWith(x => 4711)).Verifiable(Verifiables, Times.Once());

            var result = e.Execute(cMock.Object, o.Object);

            Assert.AreEqual(4711, result);
        }

        [TestMethod]
        public void Execute_Both()
        {
            var o = Mocks.Create<IDisposable>();
            var sm = Mocks.Create<Func<IDisposable, int>>();
            var am = Mocks.Create<Func<IDisposable, Task<int>>>();
            var e = new FunctionExecutor<IDisposable>(sm.Object, am.Object);
            var cMock = Mocks.Create<ICliCommandInfo>();
            sm.Setup(x => x(o.Object)).Returns(4711).Verifiable(Verifiables, Times.Once());
            am.Setup(x => x(o.Object)).Returns(Task.Delay(10).ContinueWith(x => 4711)).Verifiable(Verifiables, Times.Never());

            var result = e.Execute(cMock.Object, o.Object);

            Assert.AreEqual(4711, result);
        }

        [TestMethod]
        public async Task ExecuteAsync_NullObj()
        {
            var m = Mocks.Create<Func<IDisposable, Task<int>>>();
            var e = new FunctionExecutor<IDisposable>(null, m.Object);
            var cMock = Mocks.Create<ICliCommandInfo>();

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => e.ExecuteAsync(cMock.Object, null!));
        }

        [TestMethod]
        public async Task ExecuteAsync_WrongType()
        {
            var m = Mocks.Create<Func<IDisposable, Task<int>>>();
            var e = new FunctionExecutor<IDisposable>(null, m.Object);
            var cMock = Mocks.Create<ICliCommandInfo>();

            await Assert.ThrowsExceptionAsync<ArgumentException>(() => e.ExecuteAsync(cMock.Object, new object()));
        }

        [TestMethod]
        public async Task ExecuteAsync_NoExecutorFunc()
        {
            var o = Mocks.Create<IDisposable>();
            var m = Mocks.Create<Func<IDisposable, Task<int>>>();
            var e = new FunctionExecutor<IDisposable>(null, m.Object);
            new PrivateObject(e).SetField("_asyncExecutorFunc", null);
            var cMock = Mocks.Create<ICliCommandInfo>();

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => e.ExecuteAsync(cMock.Object, o.Object));
        }

        [TestMethod]
        public async Task ExecuteAsync_Sync()
        {
            var o = Mocks.Create<IDisposable>();
            var m = Mocks.Create<Func<IDisposable, int>>();
            var e = new FunctionExecutor<IDisposable>(m.Object, null);
            var cMock = Mocks.Create<ICliCommandInfo>();
            m.Setup(x => x(o.Object)).Returns(4711).Verifiable(Verifiables, Times.Once());

            var result = await e.ExecuteAsync(cMock.Object, o.Object);

            Assert.AreEqual(4711, result);
        }

        [TestMethod]
        public async Task ExecuteAsync_Async()
        {
            var o = Mocks.Create<IDisposable>();
            var m = Mocks.Create<Func<IDisposable, Task<int>>>();
            var e = new FunctionExecutor<IDisposable>(null, m.Object);
            var cMock = Mocks.Create<ICliCommandInfo>();
            m.Setup(x => x(o.Object)).Returns(Task.Delay(10).ContinueWith(x => 4711)).Verifiable(Verifiables, Times.Once());

            var result = await e.ExecuteAsync(cMock.Object, o.Object);

            Assert.AreEqual(4711, result);
        }

        [TestMethod]
        public async Task ExecuteAsync_Both()
        {
            var o = Mocks.Create<IDisposable>();
            var sm = Mocks.Create<Func<IDisposable, int>>();
            var am = Mocks.Create<Func<IDisposable, Task<int>>>();
            var e = new FunctionExecutor<IDisposable>(sm.Object, am.Object);
            var cMock = Mocks.Create<ICliCommandInfo>();
            sm.Setup(x => x(o.Object)).Returns(4711).Verifiable(Verifiables, Times.Never());
            am.Setup(x => x(o.Object)).Returns(Task.Delay(10).ContinueWith(x => 4711)).Verifiable(Verifiables, Times.Once());

            var result = await e.ExecuteAsync(cMock.Object, o.Object);

            Assert.AreEqual(4711, result);
        }
    }
}
