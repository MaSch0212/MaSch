﻿using MaSch.Console.Cli.Runtime;
using MaSch.Console.Cli.Runtime.Executors;
using MaSch.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace MaSch.Console.Cli.Test.Runtime.Executors
{
    [TestClass]
    public class DirectExecutorTests : TestClassBase
    {
        [TestMethod]
        public void Ctor_NullChecks()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new DirectExecutor(null!));
        }

        [TestMethod]
        public void Ctor_WrongType()
        {
            Assert.ThrowsException<ArgumentException>(() => new DirectExecutor(typeof(object)));
        }

        [TestMethod]
        [DataRow(true, false, DisplayName = "Sync Executor")]
        [DataRow(false, true, DisplayName = "Async Executor")]
        [DataRow(true, true, DisplayName = "Sync and Async Executor")]
        public void Ctor_Success(bool syncExecutor, bool asyncExecutor)
        {
            var mock = Mocks.Create<object>();
            if (syncExecutor)
                mock.As<ICliCommandExecutor>();
            if (asyncExecutor)
                mock.As<ICliAsyncCommandExecutor>();
            var commandType = mock.Object.GetType();

            var executor = new DirectExecutor(commandType);

            var po = new PrivateObject(executor);
            Assert.AreSame(commandType, po.GetField("_commandType"));
        }

        [TestMethod]
        public void Execute_Null()
        {
            var mock = Mocks.Create<ICliCommandExecutor>();
            var commandType = mock.Object.GetType();

            var executor = new DirectExecutor(commandType);

            Assert.ThrowsException<ArgumentNullException>(() => executor.Execute(null!));
        }

        [TestMethod]
        public void Execute_TypeCheck()
        {
            var mock1 = Mocks.Create<ICliCommandExecutor>();
            var mock2 = Mocks.Create<ICliCommandExecutor>();
            mock2.As<IDisposable>();
            var commandType = mock1.Object.GetType();

            var executor = new DirectExecutor(commandType);

            Assert.ThrowsException<ArgumentException>(() => executor.Execute(mock2.Object));
        }

        [TestMethod]
        public void Execute_NotAnExecutor()
        {
            var executor = new DirectExecutor(typeof(ICliCommandExecutor));
            var po = new PrivateObject(executor);

            po.SetField("_commandType", typeof(object));

            var ex = Assert.ThrowsException<InvalidOperationException>(() => executor.Execute(new object()));
            Assert.ContainsAll(new[] { nameof(Object), nameof(ICliCommandExecutor), nameof(ICliAsyncCommandExecutor) }, ex.Message);
        }

        [TestMethod]
        public void Execute_SyncExecutor()
        {
            var mock = Mocks.Create<ICliCommandExecutor>();
            mock.Setup(x => x.ExecuteCommand()).Returns(4711).Verifiable(Verifiables, Times.Once());

            var executor = new DirectExecutor(mock.Object.GetType());

            var result = executor.Execute(mock.Object);

            Assert.AreEqual(4711, result);
        }

        [TestMethod]
        public void Execute_AsyncExecutor()
        {
            var mock = Mocks.Create<ICliAsyncCommandExecutor>();
            mock.Setup(x => x.ExecuteCommandAsync()).Returns(Task.Delay(10).ContinueWith(x => 4711)).Verifiable(Verifiables, Times.Once());

            var executor = new DirectExecutor(mock.Object.GetType());

            var result = executor.Execute(mock.Object);

            Assert.AreEqual(4711, result);
        }

        [TestMethod]
        public void Execute_SyncAndAsyncExecutor()
        {
            var mock = Mocks.Create<ICliCommandExecutor>();
            mock.Setup(x => x.ExecuteCommand()).Returns(4711).Verifiable(Verifiables, Times.Once());
            mock.As<ICliAsyncCommandExecutor>()
                .Setup(x => x.ExecuteCommandAsync())
                .Returns(Task.Delay(10).ContinueWith(x => 1337))
                .Verifiable(Verifiables, Times.Never());

            var executor = new DirectExecutor(mock.Object.GetType());

            var result = executor.Execute(mock.Object);

            Assert.AreEqual(4711, result);
        }

        [TestMethod]
        public async Task ExecuteAsync_Null()
        {
            var mock = Mocks.Create<ICliAsyncCommandExecutor>();
            var commandType = mock.Object.GetType();

            var executor = new DirectExecutor(commandType);

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => executor.ExecuteAsync(null!));
        }

        [TestMethod]
        public async Task ExecuteAsync_TypeCheck()
        {
            var mock1 = Mocks.Create<ICliAsyncCommandExecutor>();
            var mock2 = Mocks.Create<ICliAsyncCommandExecutor>();
            mock2.As<IDisposable>();
            var commandType = mock1.Object.GetType();

            var executor = new DirectExecutor(commandType);

            await Assert.ThrowsExceptionAsync<ArgumentException>(() => executor.ExecuteAsync(mock2.Object));
        }

        [TestMethod]
        public async Task ExecuteAsync_NotAnExecutor()
        {
            var executor = new DirectExecutor(typeof(ICliAsyncCommandExecutor));
            var po = new PrivateObject(executor);

            po.SetField("_commandType", typeof(object));

            var ex = await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => executor.ExecuteAsync(new object()));
            Assert.ContainsAll(new[] { nameof(Object), nameof(ICliCommandExecutor), nameof(ICliAsyncCommandExecutor) }, ex.Message);
        }

        [TestMethod]
        public async Task ExecuteAsync_SyncExecutor()
        {
            var mock = Mocks.Create<ICliCommandExecutor>();
            mock.Setup(x => x.ExecuteCommand()).Returns(4711).Verifiable(Verifiables, Times.Once());

            var executor = new DirectExecutor(mock.Object.GetType());

            var result = await executor.ExecuteAsync(mock.Object);

            Assert.AreEqual(4711, result);
        }

        [TestMethod]
        public async Task ExecuteAsync_AsyncExecutor()
        {
            var mock = Mocks.Create<ICliAsyncCommandExecutor>();
            mock.Setup(x => x.ExecuteCommandAsync()).Returns(Task.Delay(10).ContinueWith(x => 4711)).Verifiable(Verifiables, Times.Once());

            var executor = new DirectExecutor(mock.Object.GetType());

            var result = await executor.ExecuteAsync(mock.Object);

            Assert.AreEqual(4711, result);
        }

        [TestMethod]
        public async Task ExecuteAsync_SyncAndAsyncExecutor()
        {
            var mock = Mocks.Create<ICliCommandExecutor>();
            mock.Setup(x => x.ExecuteCommand()).Returns(4711).Verifiable(Verifiables, Times.Never());
            mock.As<ICliAsyncCommandExecutor>()
                .Setup(x => x.ExecuteCommandAsync())
                .Returns(Task.Delay(10).ContinueWith(x => 1337))
                .Verifiable(Verifiables, Times.Once());

            var executor = new DirectExecutor(mock.Object.GetType());

            var result = await executor.ExecuteAsync(mock.Object);

            Assert.AreEqual(1337, result);
        }
    }
}