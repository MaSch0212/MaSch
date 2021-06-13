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

namespace MaSch.Console.Cli.Test.Runtime.Executors
{
    [TestClass]
    public class ExternalExecutorTests : TestClassBase
    {
        private delegate bool CliValidatorDelegate(ICliCommandInfo command, IDisposable optionsObj, [MaybeNullWhen(true)] out IEnumerable<CliError>? errors);

        [TestMethod]
        public void GetExecutor_NullChecks()
        {
            Assert.ThrowsException<ArgumentNullException>(() => ExternalExecutor.GetExecutor(null!, typeof(object), null));
            Assert.ThrowsException<ArgumentNullException>(() => ExternalExecutor.GetExecutor(typeof(object), null!, null));
        }

        [TestMethod]
        public void GetExecutor_WrongExecutorType_NoExecutor()
        {
            var mock = Mocks.Create<object>();
            var executorType = mock.Object.GetType();

            var ex = Assert.ThrowsException<ArgumentException>(() => ExternalExecutor.GetExecutor(executorType, typeof(object), null));
            Assert.ContainsAll(new[] { executorType.Name, typeof(ICliCommandExecutor<>).Name, typeof(ICliAsyncCommandExecutor<>).Name, nameof(Object) }, ex.Message);
        }

        [TestMethod]
        [DataRow(true, DisplayName = nameof(ICliCommandExecutor))]
        [DataRow(false, DisplayName = nameof(ICliAsyncCommandExecutor))]
        public void GetExecutor_WrongExecutorType_WrongExecutor(bool sync)
        {
            var executorType = sync
                ? Mocks.Create<ICliCommandExecutor<IFormatProvider>>().Object.GetType()
                : Mocks.Create<ICliAsyncCommandExecutor<IFormatProvider>>().Object.GetType();

            var ex = Assert.ThrowsException<ArgumentException>(() => ExternalExecutor.GetExecutor(executorType, typeof(IDisposable), null));
            Assert.ContainsAll(new[] { executorType.Name, typeof(ICliCommandExecutor<>).Name, typeof(ICliAsyncCommandExecutor<>).Name, nameof(IDisposable) }, ex.Message);
        }

        [TestMethod]
        [DataRow(true, DisplayName = nameof(ICliCommandExecutor))]
        [DataRow(false, DisplayName = nameof(ICliAsyncCommandExecutor))]
        public void GetExecutor_Success_ExactType(bool sync)
        {
            var executorType = sync
                ? Mocks.Create<ICliCommandExecutor<IDisposable>>().Object.GetType()
                : Mocks.Create<ICliAsyncCommandExecutor<IDisposable>>().Object.GetType();

            var executor = ExternalExecutor.GetExecutor(executorType, typeof(IDisposable), null);
            Assert.IsNotNull(executor);
            Assert.IsInstanceOfType<ExternalExecutor<IDisposable>>(executor);
        }

        [TestMethod]
        [DataRow(true, DisplayName = nameof(ICliCommandExecutor))]
        [DataRow(false, DisplayName = nameof(ICliAsyncCommandExecutor))]
        public void GetExecutor_Success_DerivedType(bool sync)
        {
            var executorType = sync
                ? Mocks.Create<ICliCommandExecutor<IDisposable>>().Object.GetType()
                : Mocks.Create<ICliAsyncCommandExecutor<IDisposable>>().Object.GetType();

            var executor = ExternalExecutor.GetExecutor(executorType, typeof(IEnumerator<object>), null);
            Assert.IsNotNull(executor);
            Assert.IsInstanceOfType<ExternalExecutor<IDisposable>>(executor);
        }

        [TestMethod]
        public void Ctor_NullExecutor()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new ExternalExecutor<IDisposable>(null!, null));
        }

        [TestMethod]
        public void Ctor_ExecutorInstanceWrongType()
        {
            Assert.ThrowsException<ArgumentException>(() => new ExternalExecutor<IDisposable>(typeof(ICliCommandExecutor<IDisposable>), new object()));
        }

        [TestMethod]
        public void Ctor_NoExecutor()
        {
            var ex = Assert.ThrowsException<ArgumentException>(() => new ExternalExecutor<object>(typeof(object), null));
            Assert.ContainsAll(new[] { nameof(Object), typeof(ICliCommandExecutor<object>).Name, typeof(ICliAsyncCommandExecutor<object>).Name }, ex.Message);
        }

        [TestMethod]
        [DataRow(true, DisplayName = nameof(ICliCommandExecutor))]
        [DataRow(false, DisplayName = nameof(ICliAsyncCommandExecutor))]
        public void Ctor_WrongExecutor(bool sync)
        {
            var executorType = sync
                ? Mocks.Create<ICliCommandExecutor<IFormatProvider>>().Object.GetType()
                : Mocks.Create<ICliAsyncCommandExecutor<IFormatProvider>>().Object.GetType();

            var ex = Assert.ThrowsException<ArgumentException>(() => new ExternalExecutor<IDisposable>(executorType, null));
            Assert.ContainsAll(new[] { executorType.Name, typeof(ICliCommandExecutor<IDisposable>).Name, typeof(ICliAsyncCommandExecutor<IDisposable>).Name, nameof(IDisposable) }, ex.Message);
        }

        [TestMethod]
        [DataRow(true, DisplayName = nameof(ICliCommandExecutor))]
        [DataRow(false, DisplayName = nameof(ICliAsyncCommandExecutor))]
        public void Ctor_Success(bool sync)
        {
            object exec = sync
                ? Mocks.Create<ICliCommandExecutor<IDisposable>>().Object
                : Mocks.Create<ICliAsyncCommandExecutor<IDisposable>>().Object;

            _ = new ExternalExecutor<IDisposable>(exec.GetType(), exec);
        }

        [TestMethod]
        public void Execute_Null()
        {
            var eMock = Mocks.Create<ICliCommandExecutor<IDisposable>>();
            var cMock = Mocks.Create<ICliCommandInfo>();
            var executor = new ExternalExecutor<IDisposable>(eMock.Object.GetType(), eMock.Object);

            Assert.ThrowsException<ArgumentNullException>(() => executor.Execute(null!, new object()));
            Assert.ThrowsException<ArgumentNullException>(() => executor.Execute(cMock.Object, null!));
        }

        [TestMethod]
        public void Execute_WrongObjType()
        {
            var eMock = Mocks.Create<ICliCommandExecutor<IDisposable>>();
            var cMock = Mocks.Create<ICliCommandInfo>();
            var executor = new ExternalExecutor<IDisposable>(eMock.Object.GetType(), eMock.Object);

            var ex = Assert.ThrowsException<ArgumentException>(() => executor.Execute(cMock.Object, new object()));
            Assert.ContainsAll(new[] { nameof(IDisposable), nameof(Object) }, ex.Message);
        }

        [TestMethod]
        public void Execute_NoExecutor()
        {
            var eMock = Mocks.Create<ICliCommandExecutor<IDisposable>>();
            var cMock = Mocks.Create<ICliCommandInfo>();
            var objMock = Mocks.Create<IDisposable>();
            var executor = new ExternalExecutor<IDisposable>(eMock.Object.GetType(), null);
            new PrivateObject(executor).SetField("_executorInstance", new object());

            Assert.ThrowsException<InvalidOperationException>(() => executor.Execute(cMock.Object, objMock.Object));
        }

        [TestMethod]
        public void Execute_CreateAndCacheExecutor()
        {
            var obj1Mock = Mocks.Create<IDisposable>();
            var obj2Mock = Mocks.Create<IDisposable>();
            var executor = new ExternalExecutor<IDisposable>(typeof(DummySyncExecutor), null);
            var cMock = Mocks.Create<ICliCommandInfo>();

            var result = executor.Execute(cMock.Object, obj1Mock.Object);

            Assert.AreEqual(1234, result);
            var ei = Assert.IsInstanceOfType<DummySyncExecutor>(executor.LastExecutorInstance);
            Assert.AreCollectionsEqual(new[] { (cMock.Object, obj1Mock.Object) }, ei.Executions);

            _ = executor.Execute(cMock.Object, obj2Mock.Object);
            Assert.AreSame(ei, executor.LastExecutorInstance);
            Assert.AreCollectionsEqual(new[] { (cMock.Object, obj1Mock.Object), (cMock.Object, obj2Mock.Object) }, ei.Executions);
        }

        [TestMethod]
        public void Execute_AsyncExecutor()
        {
            var obj1Mock = Mocks.Create<IDisposable>();
            var executor = new ExternalExecutor<IDisposable>(typeof(DummyAsyncExecutor), null);
            var cMock = Mocks.Create<ICliCommandInfo>();

            var result = executor.Execute(cMock.Object, obj1Mock.Object);

            Assert.AreEqual(1234, result);
            var ei = Assert.IsInstanceOfType<DummyAsyncExecutor>(executor.LastExecutorInstance);
            Assert.AreCollectionsEqual(new[] { (cMock.Object, obj1Mock.Object) }, ei.Executions);
        }

        [TestMethod]
        public void Execute_CombinedExecutor()
        {
            var obj1Mock = Mocks.Create<IDisposable>();
            var executor = new ExternalExecutor<IDisposable>(typeof(DummyCombinedExecutor), null);
            var cMock = Mocks.Create<ICliCommandInfo>();

            var result = executor.Execute(cMock.Object, obj1Mock.Object);

            Assert.AreEqual(5678, result);
            var ei = Assert.IsInstanceOfType<DummyCombinedExecutor>(executor.LastExecutorInstance);
            Assert.AreCollectionsEqual(new[] { (cMock.Object, obj1Mock.Object) }, ei.SyncExecutions);
            Assert.AreCollectionsEqual(Array.Empty<(ICliCommandInfo, IDisposable)>(), ei.AsyncExecutions);
        }

        [TestMethod]
        public async Task ExecuteAsync_Null()
        {
            var eMock = Mocks.Create<ICliCommandExecutor<IDisposable>>();
            var cMock = Mocks.Create<ICliCommandInfo>();
            var executor = new ExternalExecutor<IDisposable>(eMock.Object.GetType(), eMock.Object);

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => executor.ExecuteAsync(null!, new object()));
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => executor.ExecuteAsync(cMock.Object, null!));
        }

        [TestMethod]
        public async Task ExecuteAsync_WrongObjType()
        {
            var eMock = Mocks.Create<ICliCommandExecutor<IDisposable>>();
            var cMock = Mocks.Create<ICliCommandInfo>();
            var executor = new ExternalExecutor<IDisposable>(eMock.Object.GetType(), eMock.Object);

            var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => executor.ExecuteAsync(cMock.Object, new object()));
            Assert.ContainsAll(new[] { nameof(IDisposable), nameof(Object) }, ex.Message);
        }

        [TestMethod]
        public async Task ExecuteAsync_NoExecutor()
        {
            var eMock = Mocks.Create<ICliCommandExecutor<IDisposable>>();
            var cMock = Mocks.Create<ICliCommandInfo>();
            var objMock = Mocks.Create<IDisposable>();
            var executor = new ExternalExecutor<IDisposable>(eMock.Object.GetType(), null);
            new PrivateObject(executor).SetField("_executorInstance", new object());

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => executor.ExecuteAsync(cMock.Object, objMock.Object));
        }

        [TestMethod]
        public async Task ExecuteAsync_CreateAndCacheExecutor()
        {
            var obj1Mock = Mocks.Create<IDisposable>();
            var obj2Mock = Mocks.Create<IDisposable>();
            var executor = new ExternalExecutor<IDisposable>(typeof(DummyAsyncExecutor), null);
            var cMock = Mocks.Create<ICliCommandInfo>();

            var result = await executor.ExecuteAsync(cMock.Object, obj1Mock.Object);

            Assert.AreEqual(1234, result);
            var ei = Assert.IsInstanceOfType<DummyAsyncExecutor>(executor.LastExecutorInstance);
            Assert.AreCollectionsEqual(new[] { (cMock.Object, obj1Mock.Object) }, ei.Executions);

            _ = await executor.ExecuteAsync(cMock.Object, obj2Mock.Object);
            Assert.AreSame(ei, executor.LastExecutorInstance);
            Assert.AreCollectionsEqual(new[] { (cMock.Object, obj1Mock.Object), (cMock.Object, obj2Mock.Object) }, ei.Executions);
        }

        [TestMethod]
        public async Task ExecuteAsync_SyncExecutor()
        {
            var obj1Mock = Mocks.Create<IDisposable>();
            var cMock = Mocks.Create<ICliCommandInfo>();
            var executor = new ExternalExecutor<IDisposable>(typeof(DummySyncExecutor), null);

            var result = await executor.ExecuteAsync(cMock.Object, obj1Mock.Object);

            Assert.AreEqual(1234, result);
            var ei = Assert.IsInstanceOfType<DummySyncExecutor>(executor.LastExecutorInstance);
            Assert.AreCollectionsEqual(new[] { (cMock.Object, obj1Mock.Object) }, ei.Executions);
        }

        [TestMethod]
        public async Task ExecuteAsync_CombinedExecutor()
        {
            var obj1Mock = Mocks.Create<IDisposable>();
            var cMock = Mocks.Create<ICliCommandInfo>();
            var executor = new ExternalExecutor<IDisposable>(typeof(DummyCombinedExecutor), null);

            var result = await executor.ExecuteAsync(cMock.Object, obj1Mock.Object);

            Assert.AreEqual(1234, result);
            var ei = Assert.IsInstanceOfType<DummyCombinedExecutor>(executor.LastExecutorInstance);
            Assert.AreCollectionsEqual(new[] { (cMock.Object, obj1Mock.Object) }, ei.AsyncExecutions);
            Assert.AreCollectionsEqual(Array.Empty<(ICliCommandInfo, IDisposable)>(), ei.SyncExecutions);
        }

        [TestMethod]
        public void ValidateOptions_NullChecks()
        {
            var objMock = Mocks.Create<IDisposable>();
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var executor = new ExternalExecutor<IDisposable>(typeof(DummySyncExecutor), null);

            Assert.ThrowsException<ArgumentNullException>(() => executor.ValidateOptions(null!, objMock.Object, out _));
            Assert.ThrowsException<ArgumentNullException>(() => executor.ValidateOptions(commandMock.Object, null!, out _));
        }

        [TestMethod]
        public void ValidateOptions_WrongParamType()
        {
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var executor = new ExternalExecutor<IDisposable>(typeof(DummySyncExecutor), null);

            Assert.ThrowsException<ArgumentException>(() => executor.ValidateOptions(commandMock.Object, new object(), out _));
        }

        [TestMethod]
        public void ValidateOptions_ExecutorIsNotValidator()
        {
            var objMock = Mocks.Create<IDisposable>();
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var executor = new ExternalExecutor<IDisposable>(typeof(DummySyncExecutor), null);

            var result = executor.ValidateOptions(commandMock.Object, objMock.Object, out var errors);

            Assert.IsTrue(result);
            Assert.IsNull(errors);
        }

        [TestMethod]
        public void ValidateOptions_ValidatorExactType()
        {
            IEnumerable<CliError>? err;
            var objMock = Mocks.Create<IDisposable>();
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var eMock = Mocks.Create<DummySyncExecutor>();
            var vMock = eMock.As<ICliValidator<IDisposable>>();
            var executor = new ExternalExecutor<IDisposable>(vMock.Object.GetType(), vMock.Object);
            vMock.Setup(x => x.ValidateOptions(commandMock.Object, objMock.Object, out err))
                .Returns(new CliValidatorDelegate((ICliCommandInfo c, IDisposable o, out IEnumerable<CliError>? e) =>
                {
                    e = new[] { new CliError("My Test Error") };
                    return false;
                }))
                .Verifiable(Verifiables, Times.Once());

            var result = executor.ValidateOptions(commandMock.Object, objMock.Object, out var errors);

            Assert.IsFalse(result);
            Assert.IsNotNull(errors);
            Assert.AreCollectionsEqual(
                new[] { (CliErrorType.Custom, (string?)"My Test Error") },
                errors.Select(x => (x.Type, x.CustomErrorMessage)));
        }

        [TestMethod]
        public void ValidateOptions_ValidatorDerivedType()
        {
            IEnumerable<CliError>? err;
            var objMock = Mocks.Create<IEnumerator<object>>();
            var commandMock = Mocks.Create<ICliCommandInfo>();
            var eMock = Mocks.Create<DummySyncExecutor>();
            var vMock = eMock.As<ICliValidator<IDisposable>>();
            var executor = new ExternalExecutor<IDisposable>(vMock.Object.GetType(), vMock.Object);
            vMock.Setup(x => x.ValidateOptions(commandMock.Object, objMock.Object, out err))
                .Returns(new CliValidatorDelegate((ICliCommandInfo c, IDisposable o, out IEnumerable<CliError>? e) =>
                {
                    e = new[] { new CliError("My Test Error") };
                    return false;
                }))
                .Verifiable(Verifiables, Times.Once());

            var result = executor.ValidateOptions(commandMock.Object, objMock.Object, out var errors);

            Assert.IsFalse(result);
            Assert.IsNotNull(errors);
            Assert.AreCollectionsEqual(
                new[] { (CliErrorType.Custom, (string?)"My Test Error") },
                errors.Select(x => (x.Type, x.CustomErrorMessage)));
        }

        public class DummySyncExecutor : ICliCommandExecutor<IDisposable>
        {
            public List<(ICliCommandInfo, IDisposable)> Executions { get; } = new();
            public int ReturnValue { get; set; } = 1234;

            public int ExecuteCommand(ICliCommandInfo command, IDisposable parameters)
            {
                Executions.Add((command, parameters));
                return ReturnValue;
            }
        }

        private class DummyAsyncExecutor : ICliAsyncCommandExecutor<IDisposable>
        {
            public List<(ICliCommandInfo, IDisposable)> Executions { get; } = new();
            public int ReturnValue { get; set; } = 1234;

            public Task<int> ExecuteCommandAsync(ICliCommandInfo command, IDisposable parameters)
            {
                Executions.Add((command, parameters));
                return Task.Delay(10).ContinueWith(x => ReturnValue);
            }
        }

        private class DummyCombinedExecutor : ICliCommandExecutor<IDisposable>, ICliAsyncCommandExecutor<IDisposable>
        {
            public List<(ICliCommandInfo, IDisposable)> AsyncExecutions { get; } = new();
            public List<(ICliCommandInfo, IDisposable)> SyncExecutions { get; } = new();
            public int AsyncReturnValue { get; set; } = 1234;
            public int SyncReturnValue { get; set; } = 5678;

            public int ExecuteCommand(ICliCommandInfo command, IDisposable parameters)
            {
                SyncExecutions.Add((command, parameters));
                return SyncReturnValue;
            }

            public Task<int> ExecuteCommandAsync(ICliCommandInfo command, IDisposable parameters)
            {
                AsyncExecutions.Add((command, parameters));
                return Task.Delay(10).ContinueWith(x => AsyncReturnValue);
            }
        }
    }
}
