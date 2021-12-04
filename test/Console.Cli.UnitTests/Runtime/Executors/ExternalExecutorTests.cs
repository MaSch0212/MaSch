using MaSch.Console.Cli.Runtime;
using MaSch.Console.Cli.Runtime.Executors;

namespace MaSch.Console.Cli.UnitTests.Runtime.Executors;

[TestClass]
public class ExternalExecutorTests : TestClassBase
{
    private delegate bool CliValidatorDelegate(CliExecutionContext context, IDisposable optionsObj, [MaybeNullWhen(true)] out IEnumerable<CliError>? errors);

    [TestMethod]
    public void GetExecutor_NullChecks()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() => ExternalExecutor.GetExecutor(null!, typeof(object), null));
        _ = Assert.ThrowsException<ArgumentNullException>(() => ExternalExecutor.GetExecutor(typeof(object), null!, null));
    }

    [TestMethod]
    public void GetExecutor_WrongExecutorType_NoExecutor()
    {
        var mock = Mocks.Create<object>();
        var executorType = mock.Object.GetType();

        var ex = Assert.ThrowsException<ArgumentException>(() => ExternalExecutor.GetExecutor(executorType, typeof(object), null));
        Assert.ContainsAll(new[] { executorType.Name, typeof(ICliExecutor<>).Name, typeof(ICliAsyncExecutor<>).Name, nameof(Object) }, ex.Message);
    }

    [TestMethod]
    [DataRow(true, DisplayName = nameof(ICliExecutor<IFormatProvider>))]
    [DataRow(false, DisplayName = nameof(ICliAsyncExecutor<IFormatProvider>))]
    public void GetExecutor_WrongExecutorType_WrongExecutor(bool sync)
    {
        var executorType = sync
            ? Mocks.Create<ICliExecutor<IFormatProvider>>().Object.GetType()
            : Mocks.Create<ICliAsyncExecutor<IFormatProvider>>().Object.GetType();

        var ex = Assert.ThrowsException<ArgumentException>(() => ExternalExecutor.GetExecutor(executorType, typeof(IDisposable), null));
        Assert.ContainsAll(new[] { executorType.Name, typeof(ICliExecutor<>).Name, typeof(ICliAsyncExecutor<>).Name, nameof(IDisposable) }, ex.Message);
    }

    [TestMethod]
    [DataRow(true, DisplayName = nameof(ICliExecutor<IDisposable>))]
    [DataRow(false, DisplayName = nameof(ICliAsyncExecutor<IDisposable>))]
    public void GetExecutor_Success_ExactType(bool sync)
    {
        var executorType = sync
            ? Mocks.Create<ICliExecutor<IDisposable>>().Object.GetType()
            : Mocks.Create<ICliAsyncExecutor<IDisposable>>().Object.GetType();

        var executor = ExternalExecutor.GetExecutor(executorType, typeof(IDisposable), null);
        Assert.IsNotNull(executor);
        _ = Assert.IsInstanceOfType<ExternalExecutor<IDisposable>>(executor);
    }

    [TestMethod]
    [DataRow(true, DisplayName = nameof(ICliExecutor<IDisposable>))]
    [DataRow(false, DisplayName = nameof(ICliAsyncExecutor<IDisposable>))]
    public void GetExecutor_Success_DerivedType(bool sync)
    {
        var executorType = sync
            ? Mocks.Create<ICliExecutor<IDisposable>>().Object.GetType()
            : Mocks.Create<ICliAsyncExecutor<IDisposable>>().Object.GetType();

        var executor = ExternalExecutor.GetExecutor(executorType, typeof(IEnumerator<object>), null);
        Assert.IsNotNull(executor);
        _ = Assert.IsInstanceOfType<ExternalExecutor<IDisposable>>(executor);
    }

    [TestMethod]
    public void Ctor_NullExecutor()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() => new ExternalExecutor<IDisposable>(null!, null));
    }

    [TestMethod]
    public void Ctor_ExecutorInstanceWrongType()
    {
        _ = Assert.ThrowsException<ArgumentException>(() => new ExternalExecutor<IDisposable>(typeof(ICliExecutor<IDisposable>), new object()));
    }

    [TestMethod]
    public void Ctor_NoExecutor()
    {
        var ex = Assert.ThrowsException<ArgumentException>(() => new ExternalExecutor<object>(typeof(object), null));
        Assert.ContainsAll(new[] { nameof(Object), typeof(ICliExecutor<object>).Name, typeof(ICliAsyncExecutor<object>).Name }, ex.Message);
    }

    [TestMethod]
    [DataRow(true, DisplayName = nameof(ICliExecutor<IFormatProvider>))]
    [DataRow(false, DisplayName = nameof(ICliAsyncExecutor<IFormatProvider>))]
    public void Ctor_WrongExecutor(bool sync)
    {
        var executorType = sync
            ? Mocks.Create<ICliExecutor<IFormatProvider>>().Object.GetType()
            : Mocks.Create<ICliAsyncExecutor<IFormatProvider>>().Object.GetType();

        var ex = Assert.ThrowsException<ArgumentException>(() => new ExternalExecutor<IDisposable>(executorType, null));
        Assert.ContainsAll(new[] { executorType.Name, typeof(ICliExecutor<IDisposable>).Name, typeof(ICliAsyncExecutor<IDisposable>).Name, nameof(IDisposable) }, ex.Message);
    }

    [TestMethod]
    [DataRow(true, DisplayName = nameof(ICliExecutor<IDisposable>))]
    [DataRow(false, DisplayName = nameof(ICliAsyncExecutor<IDisposable>))]
    public void Ctor_Success(bool sync)
    {
        object exec = sync
            ? Mocks.Create<ICliExecutor<IDisposable>>().Object
            : Mocks.Create<ICliAsyncExecutor<IDisposable>>().Object;

        _ = new ExternalExecutor<IDisposable>(exec.GetType(), exec);
    }

    [TestMethod]
    public void Execute_Null()
    {
        var executorMock = Mocks.Create<ICliExecutor<IDisposable>>();
        var commandMock = Mocks.Create<ICliCommandInfo>();
        var serviceProviderMock = Mocks.Create<IServiceProvider>();
        var execCtx = new CliExecutionContext(serviceProviderMock.Object, commandMock.Object);
        var executor = new ExternalExecutor<IDisposable>(executorMock.Object.GetType(), executorMock.Object);

        _ = Assert.ThrowsException<ArgumentNullException>(() => executor.Execute(null!, new object()));
        _ = Assert.ThrowsException<ArgumentNullException>(() => executor.Execute(execCtx, null!));
    }

    [TestMethod]
    public void Execute_WrongObjType()
    {
        var executorMock = Mocks.Create<ICliExecutor<IDisposable>>();
        var commandMock = Mocks.Create<ICliCommandInfo>();
        var serviceProviderMock = Mocks.Create<IServiceProvider>();
        var execCtx = new CliExecutionContext(serviceProviderMock.Object, commandMock.Object);
        var executor = new ExternalExecutor<IDisposable>(executorMock.Object.GetType(), executorMock.Object);

        var ex = Assert.ThrowsException<ArgumentException>(() => executor.Execute(execCtx, new object()));
        Assert.ContainsAll(new[] { nameof(IDisposable), nameof(Object) }, ex.Message);
    }

    [TestMethod]
    public void Execute_NoExecutor()
    {
        var executorMock = Mocks.Create<ICliExecutor<IDisposable>>();
        var commandMock = Mocks.Create<ICliCommandInfo>();
        var serviceProviderMock = Mocks.Create<IServiceProvider>();
        var execCtx = new CliExecutionContext(serviceProviderMock.Object, commandMock.Object);
        var objMock = Mocks.Create<IDisposable>();
        var executor = new ExternalExecutor<IDisposable>(executorMock.Object.GetType(), null);
        new PrivateObject(executor).SetField("_executorInstance", new object());

        _ = Assert.ThrowsException<InvalidOperationException>(() => executor.Execute(execCtx, objMock.Object));
    }

    [TestMethod]
    public void Execute_CreateAndCacheExecutor()
    {
        var obj1Mock = Mocks.Create<IDisposable>();
        var obj2Mock = Mocks.Create<IDisposable>();
        var executorInstance = new DummySyncExecutor();
        var executor = new ExternalExecutor<IDisposable>(typeof(DummySyncExecutor), null);
        var commandMock = Mocks.Create<ICliCommandInfo>();
        var serviceProviderMock = Mocks.Create<IServiceProvider>();
        var execCtx = new CliExecutionContext(serviceProviderMock.Object, commandMock.Object);
        _ = serviceProviderMock.Setup(x => x.GetService(typeof(DummySyncExecutor))).Returns(executorInstance).Verifiable(Verifiables, Times.Exactly(2));

        var result = executor.Execute(execCtx, obj1Mock.Object);

        Assert.AreEqual(1234, result);
        Assert.AreSame(executorInstance, executor.LastExecutorInstance);
        Assert.AreCollectionsEqual(new[] { (execCtx, obj1Mock.Object) }, executorInstance.Executions);

        _ = executor.Execute(execCtx, obj2Mock.Object);
        Assert.AreSame(executorInstance, executor.LastExecutorInstance);
        Assert.AreCollectionsEqual(new[] { (execCtx, obj1Mock.Object), (execCtx, obj2Mock.Object) }, executorInstance.Executions);
    }

    [TestMethod]
    public void Execute_AsyncExecutor()
    {
        var obj1Mock = Mocks.Create<IDisposable>();
        var executorInstance = new DummyAsyncExecutor();
        var executor = new ExternalExecutor<IDisposable>(typeof(DummyAsyncExecutor), null);
        var commandMock = Mocks.Create<ICliCommandInfo>();
        var serviceProviderMock = Mocks.Create<IServiceProvider>();
        var execCtx = new CliExecutionContext(serviceProviderMock.Object, commandMock.Object);
        _ = serviceProviderMock.Setup(x => x.GetService(typeof(DummyAsyncExecutor))).Returns(executorInstance).Verifiable(Verifiables, Times.Once());

        var result = executor.Execute(execCtx, obj1Mock.Object);

        Assert.AreEqual(1234, result);
        Assert.AreSame(executorInstance, executor.LastExecutorInstance);
        Assert.AreCollectionsEqual(new[] { (execCtx, obj1Mock.Object) }, executorInstance.Executions);
    }

    [TestMethod]
    public void Execute_CombinedExecutor()
    {
        var obj1Mock = Mocks.Create<IDisposable>();
        var executorInstance = new DummyCombinedExecutor();
        var executor = new ExternalExecutor<IDisposable>(typeof(DummyCombinedExecutor), null);
        var commandMock = Mocks.Create<ICliCommandInfo>();
        var serviceProviderMock = Mocks.Create<IServiceProvider>();
        var execCtx = new CliExecutionContext(serviceProviderMock.Object, commandMock.Object);
        _ = serviceProviderMock.Setup(x => x.GetService(typeof(DummyCombinedExecutor))).Returns(executorInstance).Verifiable(Verifiables, Times.Once());

        var result = executor.Execute(execCtx, obj1Mock.Object);

        Assert.AreEqual(5678, result);
        Assert.AreSame(executorInstance, executor.LastExecutorInstance);
        Assert.AreCollectionsEqual(new[] { (execCtx, obj1Mock.Object) }, executorInstance.SyncExecutions);
        Assert.AreCollectionsEqual(Array.Empty<(CliExecutionContext, IDisposable)>(), executorInstance.AsyncExecutions);
    }

    [TestMethod]
    public async Task ExecuteAsync_Null()
    {
        var executorMock = Mocks.Create<ICliExecutor<IDisposable>>();
        var commandMock = Mocks.Create<ICliCommandInfo>();
        var serviceProviderMock = Mocks.Create<IServiceProvider>();
        var execCtx = new CliExecutionContext(serviceProviderMock.Object, commandMock.Object);
        var executor = new ExternalExecutor<IDisposable>(executorMock.Object.GetType(), executorMock.Object);

        _ = await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => executor.ExecuteAsync(null!, new object()));
        _ = await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => executor.ExecuteAsync(execCtx, null!));
    }

    [TestMethod]
    public async Task ExecuteAsync_WrongObjType()
    {
        var executorMock = Mocks.Create<ICliExecutor<IDisposable>>();
        var commandMock = Mocks.Create<ICliCommandInfo>();
        var serviceProviderMock = Mocks.Create<IServiceProvider>();
        var execCtx = new CliExecutionContext(serviceProviderMock.Object, commandMock.Object);
        var executor = new ExternalExecutor<IDisposable>(executorMock.Object.GetType(), executorMock.Object);

        var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => executor.ExecuteAsync(execCtx, new object()));
        Assert.ContainsAll(new[] { nameof(IDisposable), nameof(Object) }, ex.Message);
    }

    [TestMethod]
    public async Task ExecuteAsync_NoExecutor()
    {
        var executorMock = Mocks.Create<ICliExecutor<IDisposable>>();
        var commandMock = Mocks.Create<ICliCommandInfo>();
        var objMock = Mocks.Create<IDisposable>();
        var serviceProviderMock = Mocks.Create<IServiceProvider>();
        var execCtx = new CliExecutionContext(serviceProviderMock.Object, commandMock.Object);
        var executor = new ExternalExecutor<IDisposable>(executorMock.Object.GetType(), null);
        new PrivateObject(executor).SetField("_executorInstance", new object());

        _ = await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => executor.ExecuteAsync(execCtx, objMock.Object));
    }

    [TestMethod]
    public async Task ExecuteAsync_CreateAndCacheExecutor()
    {
        var obj1Mock = Mocks.Create<IDisposable>();
        var obj2Mock = Mocks.Create<IDisposable>();
        var executorInstance = new DummyAsyncExecutor();
        var executor = new ExternalExecutor<IDisposable>(typeof(DummyAsyncExecutor), null);
        var commandMock = Mocks.Create<ICliCommandInfo>();
        var serviceProviderMock = Mocks.Create<IServiceProvider>();
        var execCtx = new CliExecutionContext(serviceProviderMock.Object, commandMock.Object);
        _ = serviceProviderMock.Setup(x => x.GetService(typeof(DummyAsyncExecutor))).Returns(executorInstance).Verifiable(Verifiables, Times.Exactly(2));

        var result = await executor.ExecuteAsync(execCtx, obj1Mock.Object);

        Assert.AreEqual(1234, result);
        Assert.AreSame(executorInstance, executor.LastExecutorInstance);
        Assert.AreCollectionsEqual(new[] { (execCtx, obj1Mock.Object) }, executorInstance.Executions);

        _ = await executor.ExecuteAsync(execCtx, obj2Mock.Object);
        Assert.AreSame(executorInstance, executor.LastExecutorInstance);
        Assert.AreCollectionsEqual(new[] { (execCtx, obj1Mock.Object), (execCtx, obj2Mock.Object) }, executorInstance.Executions);
    }

    [TestMethod]
    public async Task ExecuteAsync_SyncExecutor()
    {
        var obj1Mock = Mocks.Create<IDisposable>();
        var commandMock = Mocks.Create<ICliCommandInfo>();
        var serviceProviderMock = Mocks.Create<IServiceProvider>();
        var execCtx = new CliExecutionContext(serviceProviderMock.Object, commandMock.Object);
        var executorInstance = new DummySyncExecutor();
        var executor = new ExternalExecutor<IDisposable>(typeof(DummySyncExecutor), null);
        _ = serviceProviderMock.Setup(x => x.GetService(typeof(DummySyncExecutor))).Returns(executorInstance).Verifiable(Verifiables, Times.Once());

        var result = await executor.ExecuteAsync(execCtx, obj1Mock.Object);

        Assert.AreEqual(1234, result);
        Assert.AreSame(executorInstance, executor.LastExecutorInstance);
        Assert.AreCollectionsEqual(new[] { (execCtx, obj1Mock.Object) }, executorInstance.Executions);
    }

    [TestMethod]
    public async Task ExecuteAsync_CombinedExecutor()
    {
        var obj1Mock = Mocks.Create<IDisposable>();
        var commandMock = Mocks.Create<ICliCommandInfo>();
        var serviceProviderMock = Mocks.Create<IServiceProvider>();
        var execCtx = new CliExecutionContext(serviceProviderMock.Object, commandMock.Object);
        var executorInstance = new DummyCombinedExecutor();
        var executor = new ExternalExecutor<IDisposable>(typeof(DummyCombinedExecutor), null);
        _ = serviceProviderMock.Setup(x => x.GetService(typeof(DummyCombinedExecutor))).Returns(executorInstance).Verifiable(Verifiables, Times.Once());

        var result = await executor.ExecuteAsync(execCtx, obj1Mock.Object);

        Assert.AreEqual(1234, result);
        Assert.AreSame(executorInstance, executor.LastExecutorInstance);
        Assert.AreCollectionsEqual(new[] { (execCtx, obj1Mock.Object) }, executorInstance.AsyncExecutions);
        Assert.AreCollectionsEqual(Array.Empty<(CliExecutionContext, IDisposable)>(), executorInstance.SyncExecutions);
    }

    [TestMethod]
    public void ValidateOptions_NullChecks()
    {
        var objMock = Mocks.Create<IDisposable>();
        var commandMock = Mocks.Create<ICliCommandInfo>();
        var serviceProviderMock = Mocks.Create<IServiceProvider>();
        var execCtx = new CliExecutionContext(serviceProviderMock.Object, commandMock.Object);
        var executor = new ExternalExecutor<IDisposable>(typeof(DummySyncExecutor), null);

        _ = Assert.ThrowsException<ArgumentNullException>(() => executor.ValidateOptions(null!, objMock.Object, out _));
        _ = Assert.ThrowsException<ArgumentNullException>(() => executor.ValidateOptions(execCtx, null!, out _));
    }

    [TestMethod]
    public void ValidateOptions_WrongParamType()
    {
        var commandMock = Mocks.Create<ICliCommandInfo>();
        var executor = new ExternalExecutor<IDisposable>(typeof(DummySyncExecutor), null);
        var serviceProviderMock = Mocks.Create<IServiceProvider>();
        var execCtx = new CliExecutionContext(serviceProviderMock.Object, commandMock.Object);

        _ = Assert.ThrowsException<ArgumentException>(() => executor.ValidateOptions(execCtx, new object(), out _));
    }

    [TestMethod]
    public void ValidateOptions_ExecutorIsNotValidator()
    {
        var objMock = Mocks.Create<IDisposable>();
        var commandMock = Mocks.Create<ICliCommandInfo>();
        var serviceProviderMock = Mocks.Create<IServiceProvider>();
        var execCtx = new CliExecutionContext(serviceProviderMock.Object, commandMock.Object);
        var executorInstance = new DummySyncExecutor();
        var executor = new ExternalExecutor<IDisposable>(typeof(DummySyncExecutor), null);
        _ = serviceProviderMock.Setup(x => x.GetService(typeof(DummySyncExecutor))).Returns(executorInstance).Verifiable(Verifiables, Times.Once());

        var result = executor.ValidateOptions(execCtx, objMock.Object, out var errors);

        Assert.IsTrue(result);
        Assert.IsNull(errors);
    }

    [TestMethod]
    public void ValidateOptions_ValidatorExactType()
    {
        IEnumerable<CliError>? err;
        var objMock = Mocks.Create<IDisposable>();
        var commandMock = Mocks.Create<ICliCommandInfo>();
        var executorMock = Mocks.Create<DummySyncExecutor>();
        var validatorMock = executorMock.As<ICliValidator<IDisposable>>();
        var serviceProviderMock = Mocks.Create<IServiceProvider>();
        var execCtx = new CliExecutionContext(serviceProviderMock.Object, commandMock.Object);
        var executor = new ExternalExecutor<IDisposable>(validatorMock.Object.GetType(), validatorMock.Object);
        _ = validatorMock.Setup(x => x.ValidateOptions(execCtx, objMock.Object, out err))
            .Returns(new CliValidatorDelegate((CliExecutionContext c, IDisposable o, out IEnumerable<CliError>? e) =>
            {
                e = new[] { new CliError("My Test Error") };
                return false;
            }))
            .Verifiable(Verifiables, Times.Once());

        var result = executor.ValidateOptions(execCtx, objMock.Object, out var errors);

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
        var executorMock = Mocks.Create<DummySyncExecutor>();
        var validatorMock = executorMock.As<ICliValidator<IDisposable>>();
        var serviceProviderMock = Mocks.Create<IServiceProvider>();
        var execCtx = new CliExecutionContext(serviceProviderMock.Object, commandMock.Object);
        var executor = new ExternalExecutor<IDisposable>(validatorMock.Object.GetType(), validatorMock.Object);
        _ = validatorMock.Setup(x => x.ValidateOptions(execCtx, objMock.Object, out err))
            .Returns(new CliValidatorDelegate((CliExecutionContext c, IDisposable o, out IEnumerable<CliError>? e) =>
            {
                e = new[] { new CliError("My Test Error") };
                return false;
            }))
            .Verifiable(Verifiables, Times.Once());

        var result = executor.ValidateOptions(execCtx, objMock.Object, out var errors);

        Assert.IsFalse(result);
        Assert.IsNotNull(errors);
        Assert.AreCollectionsEqual(
            new[] { (CliErrorType.Custom, (string?)"My Test Error") },
            errors.Select(x => (x.Type, x.CustomErrorMessage)));
    }

    public class DummySyncExecutor : ICliExecutor<IDisposable>
    {
        public List<(CliExecutionContext, IDisposable)> Executions { get; } = new();
        public int ReturnValue { get; set; } = 1234;

        public int ExecuteCommand(CliExecutionContext context, IDisposable parameters)
        {
            Executions.Add((context, parameters));
            return ReturnValue;
        }
    }

    private class DummyAsyncExecutor : ICliAsyncExecutor<IDisposable>
    {
        public List<(CliExecutionContext, IDisposable)> Executions { get; } = new();
        public int ReturnValue { get; set; } = 1234;

        public Task<int> ExecuteCommandAsync(CliExecutionContext context, IDisposable parameters)
        {
            Executions.Add((context, parameters));
            return Task.Delay(10).ContinueWith(x => ReturnValue);
        }
    }

    private class DummyCombinedExecutor : ICliExecutor<IDisposable>, ICliAsyncExecutor<IDisposable>
    {
        public List<(CliExecutionContext, IDisposable)> AsyncExecutions { get; } = new();
        public List<(CliExecutionContext, IDisposable)> SyncExecutions { get; } = new();
        public int AsyncReturnValue { get; set; } = 1234;
        public int SyncReturnValue { get; set; } = 5678;

        public int ExecuteCommand(CliExecutionContext context, IDisposable parameters)
        {
            SyncExecutions.Add((context, parameters));
            return SyncReturnValue;
        }

        public Task<int> ExecuteCommandAsync(CliExecutionContext context, IDisposable parameters)
        {
            AsyncExecutions.Add((context, parameters));
            return Task.Delay(10).ContinueWith(x => AsyncReturnValue);
        }
    }
}
