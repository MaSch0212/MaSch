using MaSch.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MaSch.Core.Test
{
    [TestClass]
    public class DelegateDisposableEnumerableTests : UnitTestBase
    {
        [TestMethod]
        public void Constructor_ParameterChecks()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new DelegateDisposableEnumerable<object>(null!, () => { }));
            Assert.ThrowsException<ArgumentNullException>(() => new DelegateDisposableEnumerable<object>(Array.Empty<object>(), null!));
        }

        [TestMethod]
        public void GetEnumerator_Generic()
        {
            var actionMock = new Mock<Action>();
            var enumeratorMock = new Mock<IEnumerator<string>>(MockBehavior.Strict);
            var enumerableMock = new Mock<IEnumerable<string>>(MockBehavior.Strict);
            enumerableMock.Setup(x => x.GetEnumerator()).Returns(enumeratorMock.Object);

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
            enumerableMock.As<IEnumerable>().Setup(x => x.GetEnumerator()).Returns(enumeratorMock.Object);

            var dde = new DelegateDisposableEnumerable<string>(enumerableMock.Object, actionMock.Object);

            var result = ((IEnumerable)dde).GetEnumerator();

            Assert.AreSame(enumeratorMock.Object, result);
            enumerableMock.As<IEnumerable>().Verify(x => x.GetEnumerator(), Times.Once());
            actionMock.Verify(x => x(), Times.Never());
        }

        [TestMethod]
        public void Dispose_()
        {
            var disposingMock = new Mock<EventHandler<DisposeEventArgs>>();
            var disposedMock = new Mock<EventHandler<DisposeEventArgs>>();
            var actionMock = new Mock<Action>();
            var enumerableMock = new Mock<IEnumerable<string>>(MockBehavior.Strict);
            disposingMock.Setup(x => x(It.IsAny<object?>(), It.IsAny<DisposeEventArgs>()))
                .Callback(() =>
                {
                    actionMock.Verify(x => x(), Times.Never());
                    disposedMock.Verify(x => x(It.IsAny<object?>(), It.IsAny<DisposeEventArgs>()), Times.Never());
                });
            actionMock.Setup(x => x())
                .Callback(() =>
                {
                    disposingMock.Verify(x => x(It.IsAny<object?>(), It.IsAny<DisposeEventArgs>()), Times.Once());
                    disposedMock.Verify(x => x(It.IsAny<object?>(), It.IsAny<DisposeEventArgs>()), Times.Never());
                });
            disposedMock.Setup(x => x(It.IsAny<object?>(), It.IsAny<DisposeEventArgs>()))
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
            disposingMock.Setup(x => x(It.IsAny<object?>(), It.IsAny<DisposeEventArgs>()))
                .Callback(() =>
                {
                    disposedMock.Verify(x => x(It.IsAny<object?>(), It.IsAny<DisposeEventArgs>()), Times.Never());
                });
            disposedMock.Setup(x => x(It.IsAny<object?>(), It.IsAny<DisposeEventArgs>()))
                .Callback(() =>
                {
                    disposedMock.Verify(x => x(It.IsAny<object?>(), It.IsAny<DisposeEventArgs>()), Times.Once());
                });

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
            disposingMock.Verify(x => x(It.Is<DelegateDisposableEnumerable<string>>(a => a.GetHashCode() == ddeHash), It.Is<DisposeEventArgs>(a => a != null && !a.IsDisposing)), Times.Once());
            disposedMock.Verify(x => x(It.Is<DelegateDisposableEnumerable<string>>(a => a.GetHashCode() == ddeHash), It.Is<DisposeEventArgs>(a => a != null && !a.IsDisposing)), Times.Once());
        }
    }

    [TestClass]
    public class DelegateOrderedDisposableEnumerableTests
    {
        [TestMethod]
        public void Constructor_ParameterChecks()
        {
            var enumerableMock = new Mock<IOrderedEnumerable<object>>(MockBehavior.Strict);

            Assert.ThrowsException<ArgumentNullException>(() => new DelegateOrderedDisposableEnumerable<object>(null!, () => { }));
            Assert.ThrowsException<ArgumentNullException>(() => new DelegateOrderedDisposableEnumerable<object>(enumerableMock.Object, null!));
        }

        [TestMethod]
        public void CreateOrderedEnumerable()
        {
            var actionMock = new Mock<Action>();
            var keySelectorMock = new Mock<Func<string, int>>();
            var comparerMock = new Mock<IComparer<int>>(MockBehavior.Strict);
            var resultEnumerableMock = new Mock<IOrderedEnumerable<string>>(MockBehavior.Strict);
            var enumerableMock = new Mock<IOrderedEnumerable<string>>(MockBehavior.Strict);
            enumerableMock.Setup(x => x.CreateOrderedEnumerable(It.IsAny<Func<string, int>>(), It.IsAny<IComparer<int>>(), It.IsAny<bool>())).Returns(resultEnumerableMock.Object);

            var dde = new DelegateOrderedDisposableEnumerable<string>(enumerableMock.Object, actionMock.Object);

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
            enumerableMock.Setup(x => x.GetEnumerator()).Returns(enumeratorMock.Object);

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
            enumerableMock.As<IEnumerable>().Setup(x => x.GetEnumerator()).Returns(enumeratorMock.Object);

            var dde = new DelegateOrderedDisposableEnumerable<string>(enumerableMock.Object, actionMock.Object);

            var result = ((IEnumerable)dde).GetEnumerator();

            Assert.AreSame(enumeratorMock.Object, result);
            enumerableMock.As<IEnumerable>().Verify(x => x.GetEnumerator(), Times.Once());
            actionMock.Verify(x => x(), Times.Never());
        }

        [TestMethod]
        public void Dispose()
        {
            var disposingMock = new Mock<EventHandler<DisposeEventArgs>>();
            var disposedMock = new Mock<EventHandler<DisposeEventArgs>>();
            var actionMock = new Mock<Action>();
            var enumerableMock = new Mock<IOrderedEnumerable<string>>(MockBehavior.Strict);
            disposingMock.Setup(x => x(It.IsAny<object?>(), It.IsAny<DisposeEventArgs>()))
                .Callback(() =>
                {
                    actionMock.Verify(x => x(), Times.Never());
                    disposedMock.Verify(x => x(It.IsAny<object?>(), It.IsAny<DisposeEventArgs>()), Times.Never());
                });
            actionMock.Setup(x => x())
                .Callback(() =>
                {
                    disposingMock.Verify(x => x(It.IsAny<object?>(), It.IsAny<DisposeEventArgs>()), Times.Once());
                    disposedMock.Verify(x => x(It.IsAny<object?>(), It.IsAny<DisposeEventArgs>()), Times.Never());
                });
            disposedMock.Setup(x => x(It.IsAny<object?>(), It.IsAny<DisposeEventArgs>()))
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
            disposingMock.Setup(x => x(It.IsAny<object?>(), It.IsAny<DisposeEventArgs>()))
                .Callback(() =>
                {
                    disposedMock.Verify(x => x(It.IsAny<object?>(), It.IsAny<DisposeEventArgs>()), Times.Never());
                });
            disposedMock.Setup(x => x(It.IsAny<object?>(), It.IsAny<DisposeEventArgs>()))
                .Callback(() =>
                {
                    disposedMock.Verify(x => x(It.IsAny<object?>(), It.IsAny<DisposeEventArgs>()), Times.Once());
                });

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
            disposingMock.Verify(x => x(It.Is<DelegateOrderedDisposableEnumerable<string>>(a => a.GetHashCode() == ddeHash), It.Is<DisposeEventArgs>(a => a != null && !a.IsDisposing)), Times.Once());
            disposedMock.Verify(x => x(It.Is<DelegateOrderedDisposableEnumerable<string>>(a => a.GetHashCode() == ddeHash), It.Is<DisposeEventArgs>(a => a != null && !a.IsDisposing)), Times.Once());
        }
    }
}
