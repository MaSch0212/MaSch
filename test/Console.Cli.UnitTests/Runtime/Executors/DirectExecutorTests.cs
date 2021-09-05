using MaSch.Console.Cli.Runtime;
using MaSch.Console.Cli.Runtime.Executors;
using MaSch.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace MaSch.Console.Cli.UnitTests.Runtime.Executors
{
    [TestClass]
    public class DirectExecutorTests : TestClassBase
    {
        [TestMethod]
        public void Ctor_NullChecks()
        {
            _ = Assert.ThrowsException<ArgumentNullException>(() => new DirectExecutor(null!));
        }

        [TestMethod]
        public void Ctor_WrongType()
        {
            _ = Assert.ThrowsException<ArgumentException>(() => new DirectExecutor(typeof(object)));
        }

        [TestMethod]
        [DataRow(true, false, DisplayName = "Sync Executor")]
        [DataRow(false, true, DisplayName = "Async Executor")]
        [DataRow(true, true, DisplayName = "Sync and Async Executor")]
        public void Ctor_Success(bool syncExecutor, bool asyncExecutor)
        {
            var mock = Mocks.Create<object>();
            if (syncExecutor)
                _ = mock.As<ICliExecutable>();
            if (asyncExecutor)
                _ = mock.As<ICliAsyncExecutable>();
            var commandType = mock.Object.GetType();

            var executor = new DirectExecutor(commandType);

            var po = new PrivateObject(executor);
            Assert.AreSame(commandType, po.GetField("_commandType"));
        }

        [TestMethod]
        public void Execute_Null()
        {
            var sut = CreateSut<ICliExecutable>();

            _ = Assert.ThrowsException<ArgumentNullException>(() => sut.Executor.Execute(sut.ExecutionContext, null!));
            _ = Assert.ThrowsException<ArgumentNullException>(() => sut.Executor.Execute(null!, new object()));
        }

        [TestMethod]
        public void Execute_TypeCheck()
        {
            var mock1 = Mocks.Create<ICliExecutable>();
            var mock2 = Mocks.Create<ICliExecutable>();
            _ = mock2.As<IDisposable>();

            var sut = CreateSut(mock1);

            _ = Assert.ThrowsException<ArgumentException>(() => sut.Executor.Execute(sut.ExecutionContext, mock2.Object));
        }

        [TestMethod]
        public void Execute_NotAnExecutor()
        {
            var executor = new DirectExecutor(typeof(ICliExecutable));
            var po = new PrivateObject(executor);
            var sut = CreateSut<ICliExecutable>();

            po.SetField("_commandType", typeof(object));

            var ex = Assert.ThrowsException<InvalidOperationException>(() => executor.Execute(sut.ExecutionContext, new object()));
            Assert.ContainsAll(new[] { nameof(Object), nameof(ICliExecutable), nameof(ICliAsyncExecutable) }, ex.Message);
        }

        [TestMethod]
        public void Execute_SyncExecutor()
        {
            var sut = CreateSut<ICliExecutable>();
            _ = sut.ExecutableMock.Setup(x => x.ExecuteCommand(sut.ExecutionContext)).Returns(4711).Verifiable(Verifiables, Times.Once());

            var result = sut.Executor.Execute(sut.ExecutionContext, sut.ExecutableMock.Object);

            Assert.AreEqual(4711, result);
        }

        [TestMethod]
        public void Execute_AsyncExecutor()
        {
            var sut = CreateSut<ICliAsyncExecutable>();
            _ = sut.ExecutableMock.Setup(x => x.ExecuteCommandAsync(sut.ExecutionContext)).Returns(Task.Delay(10).ContinueWith(x => 4711)).Verifiable(Verifiables, Times.Once());

            var result = sut.Executor.Execute(sut.ExecutionContext, sut.ExecutableMock.Object);

            Assert.AreEqual(4711, result);
        }

        [TestMethod]
        public void Execute_SyncAndAsyncExecutor()
        {
            Mock<ICliAsyncExecutable> asyncMock = null!;
            var sut = CreateSut<ICliExecutable>(action: x => asyncMock = x.As<ICliAsyncExecutable>());
            _ = sut.ExecutableMock.Setup(x => x.ExecuteCommand(sut.ExecutionContext)).Returns(4711).Verifiable(Verifiables, Times.Once());
            _ = asyncMock
                .Setup(x => x.ExecuteCommandAsync(sut.ExecutionContext))
                .Returns(Task.Delay(10).ContinueWith(x => 1337))
                .Verifiable(Verifiables, Times.Never());

            var result = sut.Executor.Execute(sut.ExecutionContext, sut.ExecutableMock.Object);

            Assert.AreEqual(4711, result);
        }

        [TestMethod]
        public async Task ExecuteAsync_Null()
        {
            var sut = CreateSut<ICliAsyncExecutable>();

            _ = await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => sut.Executor.ExecuteAsync(sut.ExecutionContext, null!));
            _ = await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => sut.Executor.ExecuteAsync(null!, new object()));
        }

        [TestMethod]
        public async Task ExecuteAsync_TypeCheck()
        {
            var mock1 = Mocks.Create<ICliAsyncExecutable>();
            var mock2 = Mocks.Create<ICliAsyncExecutable>();
            _ = mock2.As<IDisposable>();

            var sut = CreateSut(mock1);

            _ = await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.Executor.ExecuteAsync(sut.ExecutionContext, mock2.Object));
        }

        [TestMethod]
        public async Task ExecuteAsync_NotAnExecutor()
        {
            var sut = CreateSut<ICliAsyncExecutable>();
            var executor = new DirectExecutor(typeof(ICliAsyncExecutable));
            var po = new PrivateObject(executor);

            po.SetField("_commandType", typeof(object));

            var ex = await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => executor.ExecuteAsync(sut.ExecutionContext, new object()));
            Assert.ContainsAll(new[] { nameof(Object), nameof(ICliExecutable), nameof(ICliAsyncExecutable) }, ex.Message);
        }

        [TestMethod]
        public async Task ExecuteAsync_SyncExecutor()
        {
            var sut = CreateSut<ICliExecutable>();
            _ = sut.ExecutableMock.Setup(x => x.ExecuteCommand(sut.ExecutionContext)).Returns(4711).Verifiable(Verifiables, Times.Once());

            var result = await sut.Executor.ExecuteAsync(sut.ExecutionContext, sut.ExecutableMock.Object);

            Assert.AreEqual(4711, result);
        }

        [TestMethod]
        public async Task ExecuteAsync_AsyncExecutor()
        {
            var sut = CreateSut<ICliAsyncExecutable>();
            _ = sut.ExecutableMock.Setup(x => x.ExecuteCommandAsync(sut.ExecutionContext)).Returns(Task.Delay(10).ContinueWith(x => 4711)).Verifiable(Verifiables, Times.Once());

            var result = await sut.Executor.ExecuteAsync(sut.ExecutionContext, sut.ExecutableMock.Object);

            Assert.AreEqual(4711, result);
        }

        [TestMethod]
        public async Task ExecuteAsync_SyncAndAsyncExecutor()
        {
            Mock<ICliAsyncExecutable> asyncMock = null!;
            var sut = CreateSut<ICliExecutable>(action: x => asyncMock = x.As<ICliAsyncExecutable>());
            _ = sut.ExecutableMock.Setup(x => x.ExecuteCommand(sut.ExecutionContext)).Returns(4711).Verifiable(Verifiables, Times.Never());
            _ = asyncMock
                .Setup(x => x.ExecuteCommandAsync(sut.ExecutionContext))
                .Returns(Task.Delay(10).ContinueWith(x => 1337))
                .Verifiable(Verifiables, Times.Once());

            var result = await sut.Executor.ExecuteAsync(sut.ExecutionContext, sut.ExecutableMock.Object);

            Assert.AreEqual(1337, result);
        }

        private DirectExecutorSut<TExecutable> CreateSut<TExecutable>(Mock<TExecutable>? executableMock = null, Action<Mock<TExecutable>>? action = null)
            where TExecutable : class, ICliExecutableBase
        {
            executableMock ??= Mocks.Create<TExecutable>();
            action?.Invoke(executableMock);
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var serviceProviderMock = Mocks.Create<IServiceProvider>();
            var executionContext = new CliExecutionContext(serviceProviderMock.Object, commandMock.Object);
            var executor = new DirectExecutor(executableMock.Object.GetType());

            return new DirectExecutorSut<TExecutable>(executor, executionContext, serviceProviderMock, commandMock, executableMock);
        }

        private record DirectExecutorSut<TExecutable>(
            DirectExecutor Executor,
            CliExecutionContext ExecutionContext,
            Mock<IServiceProvider> ServiceProviderMock,
            Mock<ICliCommandInfo> CommandMock,
            Mock<TExecutable> ExecutableMock)
            where TExecutable : class, ICliExecutableBase;
    }
}
