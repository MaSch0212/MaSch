﻿namespace MaSch.Core.UnitTests;

[TestClass]
public class DelegateDisposableEnumerableTests : TestClassBase
{
    [TestMethod]
    public void Constructor_ParameterChecks()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() => new DelegateDisposableEnumerable<object>(null!, () => { }).Dispose());
        _ = Assert.ThrowsException<ArgumentNullException>(() => new DelegateDisposableEnumerable<object>(Array.Empty<object>(), null!).Dispose());
    }

    [TestMethod]
    public void GetEnumerator_Generic()
    {
        var actionMock = new Mock<Action>();
        var enumeratorMock = new Mock<IEnumerator<string>>(MockBehavior.Strict);
        var enumerableMock = new Mock<IEnumerable<string>>(MockBehavior.Strict);
        _ = enumerableMock.Setup(x => x.GetEnumerator()).Returns(enumeratorMock.Object);

        var dde = new DelegateDisposableEnumerable<string>(enumerableMock.Object, actionMock.Object);

        var result = dde.GetEnumerator();

        Assert.AreSame(enumeratorMock.Object, result);
        enumerableMock.Verify(x => x.GetEnumerator(), Times.Once());
        actionMock.Verify(x => x(), Times.Never());
    }

    [TestMethod]
    public void GetEnumerator_NonGeneric()
    {
        var actionMock = new Mock<Action>();
        var enumeratorMock = new Mock<IEnumerator>(MockBehavior.Strict);
        var enumerableMock = new Mock<IEnumerable<string>>(MockBehavior.Strict);
        _ = enumerableMock.As<IEnumerable>().Setup(x => x.GetEnumerator()).Returns(enumeratorMock.Object);

        using var dde = new DelegateDisposableEnumerable<string>(enumerableMock.Object, actionMock.Object);

        var result = ((IEnumerable)dde).GetEnumerator();

        Assert.AreSame(enumeratorMock.Object, result);
        enumerableMock.As<IEnumerable>().Verify(x => x.GetEnumerator(), Times.Once());
        actionMock.Verify(x => x(), Times.Never());
    }

    [TestMethod]
    [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP017:Prefer using.", Justification = "Makes sense here")]
    [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP016:Don't use disposed instance.", Justification = "False positive.")]
    public void Dispose_()
    {
        var disposingMock = new Mock<EventHandler<DisposeEventArgs>>();
        var disposedMock = new Mock<EventHandler<DisposeEventArgs>>();
        var actionMock = new Mock<Action>();
        var enumerableMock = new Mock<IEnumerable<string>>(MockBehavior.Strict);
        _ = disposingMock.Setup(x => x(It.IsAny<object?>(), It.IsAny<DisposeEventArgs>()))
            .Callback(() =>
            {
                actionMock.Verify(x => x(), Times.Never());
                disposedMock.Verify(x => x(It.IsAny<object?>(), It.IsAny<DisposeEventArgs>()), Times.Never());
            });
        _ = actionMock.Setup(x => x())
            .Callback(() =>
            {
                disposingMock.Verify(x => x(It.IsAny<object?>(), It.IsAny<DisposeEventArgs>()), Times.Once());
                disposedMock.Verify(x => x(It.IsAny<object?>(), It.IsAny<DisposeEventArgs>()), Times.Never());
            });
        _ = disposedMock.Setup(x => x(It.IsAny<object?>(), It.IsAny<DisposeEventArgs>()))
            .Callback(() =>
            {
                actionMock.Verify(x => x(), Times.Once());
                disposedMock.Verify(x => x(It.IsAny<object?>(), It.IsAny<DisposeEventArgs>()), Times.Once());
            });

        void Act()
        {
            var dde = new DelegateDisposableEnumerable<string>(enumerableMock.Object, actionMock.Object);
            dde.Disposing += disposingMock.Object;
            dde.Disposed += disposedMock.Object;

            dde.Dispose();

            actionMock.Verify(x => x(), Times.Once());
            disposingMock.Verify(x => x(dde, It.Is<DisposeEventArgs>(a => a != null && a.IsDisposing)), Times.Once());
            disposedMock.Verify(x => x(dde, It.Is<DisposeEventArgs>(a => a != null && a.IsDisposing)), Times.Once());
        }

        Act();
        GC.Collect(0, GCCollectionMode.Forced);
        GC.WaitForPendingFinalizers();

        // Verify that after dispose no other events are executed.
        actionMock.Verify(x => x(), Times.Once());
        disposingMock.Verify(x => x(It.IsAny<DelegateDisposableEnumerable<string>>(), It.IsAny<DisposeEventArgs>()), Times.Once());
        disposedMock.Verify(x => x(It.IsAny<DelegateDisposableEnumerable<string>>(), It.IsAny<DisposeEventArgs>()), Times.Once());
    }

    [TestMethod]
    public void Finalizer()
    {
        var disposingMock = new Mock<EventHandler<DisposeEventArgs>>();
        var disposedMock = new Mock<EventHandler<DisposeEventArgs>>();
        var actionMock = new Mock<Action>();
        var enumerableMock = new Mock<IEnumerable<string>>(MockBehavior.Strict);

        int ddeHash = 0;
        void Act()
        {
            var dde = new DelegateDisposableEnumerable<string>(enumerableMock.Object, actionMock.Object);
            dde.Disposing += disposingMock.Object;
            dde.Disposed += disposedMock.Object;
            ddeHash = dde.GetHashCode();
        }

        Act();
        GC.Collect(0, GCCollectionMode.Forced);
        GC.WaitForPendingFinalizers();

        actionMock.Verify(x => x(), Times.Never());
    }
}

[TestClass]
public class DelegateOrderedDisposableEnumerableTests
{
    [TestMethod]
    public void Constructor_ParameterChecks()
    {
        var enumerableMock = new Mock<IOrderedEnumerable<object>>(MockBehavior.Strict);

        _ = Assert.ThrowsException<ArgumentNullException>(() => new DelegateOrderedDisposableEnumerable<object>(null!, () => { }).Dispose());
        _ = Assert.ThrowsException<ArgumentNullException>(() => new DelegateOrderedDisposableEnumerable<object>(enumerableMock.Object, null!).Dispose());
    }

    [TestMethod]
    public void CreateOrderedEnumerable()
    {
        var actionMock = new Mock<Action>();
        var keySelectorMock = new Mock<Func<string, int>>();
        var comparerMock = new Mock<IComparer<int>>(MockBehavior.Strict);
        var resultEnumerableMock = new Mock<IOrderedEnumerable<string>>(MockBehavior.Strict);
        var enumerableMock = new Mock<IOrderedEnumerable<string>>(MockBehavior.Strict);
        _ = enumerableMock.Setup(x => x.CreateOrderedEnumerable(It.IsAny<Func<string, int>>(), It.IsAny<IComparer<int>>(), It.IsAny<bool>())).Returns(resultEnumerableMock.Object);

        using var dde = new DelegateOrderedDisposableEnumerable<string>(enumerableMock.Object, actionMock.Object);

        var result = dde.CreateOrderedEnumerable(keySelectorMock.Object, comparerMock.Object, true);

        Assert.AreSame(resultEnumerableMock.Object, result);
        enumerableMock.Verify(x => x.CreateOrderedEnumerable(keySelectorMock.Object, comparerMock.Object, true), Times.Once());
        actionMock.Verify(x => x(), Times.Never());
    }

    [TestMethod]
    public void GetEnumerator_Generic()
    {
        var actionMock = new Mock<Action>();
        var enumeratorMock = new Mock<IEnumerator<string>>(MockBehavior.Strict);
        var enumerableMock = new Mock<IOrderedEnumerable<string>>(MockBehavior.Strict);
        _ = enumerableMock.Setup(x => x.GetEnumerator()).Returns(enumeratorMock.Object);

        var dde = new DelegateOrderedDisposableEnumerable<string>(enumerableMock.Object, actionMock.Object);

        var result = dde.GetEnumerator();

        Assert.AreSame(enumeratorMock.Object, result);
        enumerableMock.Verify(x => x.GetEnumerator(), Times.Once());
        actionMock.Verify(x => x(), Times.Never());
    }

    [TestMethod]
    public void GetEnumerator_NonGeneric()
    {
        var actionMock = new Mock<Action>();
        var enumeratorMock = new Mock<IEnumerator>(MockBehavior.Strict);
        var enumerableMock = new Mock<IOrderedEnumerable<string>>(MockBehavior.Strict);
        _ = enumerableMock.As<IEnumerable>().Setup(x => x.GetEnumerator()).Returns(enumeratorMock.Object);

        using var dde = new DelegateOrderedDisposableEnumerable<string>(enumerableMock.Object, actionMock.Object);

        var result = ((IEnumerable)dde).GetEnumerator();

        Assert.AreSame(enumeratorMock.Object, result);
        enumerableMock.As<IEnumerable>().Verify(x => x.GetEnumerator(), Times.Once());
        actionMock.Verify(x => x(), Times.Never());
    }

    [TestMethod]
    [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP017:Prefer using.", Justification = "Makes sense here")]
    [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP016:Don't use disposed instance.", Justification = "False positive.")]
    public void Dispose_()
    {
        var disposingMock = new Mock<EventHandler<DisposeEventArgs>>();
        var disposedMock = new Mock<EventHandler<DisposeEventArgs>>();
        var actionMock = new Mock<Action>();
        var enumerableMock = new Mock<IOrderedEnumerable<string>>(MockBehavior.Strict);
        _ = disposingMock.Setup(x => x(It.IsAny<object?>(), It.IsAny<DisposeEventArgs>()))
            .Callback(() =>
            {
                actionMock.Verify(x => x(), Times.Never());
                disposedMock.Verify(x => x(It.IsAny<object?>(), It.IsAny<DisposeEventArgs>()), Times.Never());
            });
        _ = actionMock.Setup(x => x())
            .Callback(() =>
            {
                disposingMock.Verify(x => x(It.IsAny<object?>(), It.IsAny<DisposeEventArgs>()), Times.Once());
                disposedMock.Verify(x => x(It.IsAny<object?>(), It.IsAny<DisposeEventArgs>()), Times.Never());
            });
        _ = disposedMock.Setup(x => x(It.IsAny<object?>(), It.IsAny<DisposeEventArgs>()))
            .Callback(() =>
            {
                actionMock.Verify(x => x(), Times.Once());
                disposedMock.Verify(x => x(It.IsAny<object?>(), It.IsAny<DisposeEventArgs>()), Times.Once());
            });

        void Act()
        {
            var dde = new DelegateOrderedDisposableEnumerable<string>(enumerableMock.Object, actionMock.Object);
            dde.Disposing += disposingMock.Object;
            dde.Disposed += disposedMock.Object;

            dde.Dispose();

            actionMock.Verify(x => x(), Times.Once());
            disposingMock.Verify(x => x(dde, It.Is<DisposeEventArgs>(a => a != null && a.IsDisposing)), Times.Once());
            disposedMock.Verify(x => x(dde, It.Is<DisposeEventArgs>(a => a != null && a.IsDisposing)), Times.Once());
        }

        Act();
        GC.Collect(0, GCCollectionMode.Forced);
        GC.WaitForPendingFinalizers();

        // Verify that after dispose no other events are executed.
        actionMock.Verify(x => x(), Times.Once());
        disposingMock.Verify(x => x(It.IsAny<DelegateOrderedDisposableEnumerable<string>>(), It.IsAny<DisposeEventArgs>()), Times.Once());
        disposedMock.Verify(x => x(It.IsAny<DelegateOrderedDisposableEnumerable<string>>(), It.IsAny<DisposeEventArgs>()), Times.Once());
    }

    [TestMethod]
    public void Finalizer()
    {
        var disposingMock = new Mock<EventHandler<DisposeEventArgs>>();
        var disposedMock = new Mock<EventHandler<DisposeEventArgs>>();
        var actionMock = new Mock<Action>();
        var enumerableMock = new Mock<IOrderedEnumerable<string>>(MockBehavior.Strict);

        int ddeHash = 0;
        void Act()
        {
            var dde = new DelegateOrderedDisposableEnumerable<string>(enumerableMock.Object, actionMock.Object);
            dde.Disposing += disposingMock.Object;
            dde.Disposed += disposedMock.Object;
            ddeHash = dde.GetHashCode();
        }

        Act();
        GC.Collect(0, GCCollectionMode.Forced);
        GC.WaitForPendingFinalizers();

        actionMock.Verify(x => x(), Times.Never());
    }
}
