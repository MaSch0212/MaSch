using MaSch.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace MaSch.Core.Test
{
    [TestClass]
    public class ServiceContextTests : TestClassBase
    {
        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP012:Property should not return created disposable.", Justification = "Should be fine here")]
        private IServiceContext Context => Cache.GetValue(() => ServiceContext.CreateContext())!;
        private IDictionary<(Type Type, string? Name), object> ServiceDict => Cache.GetValue(() => GetServicesDict(Context))!;
        private IDictionary<Type, IDisposable> ViewsDict => Cache.GetValue(() => GetViewsDict(Context))!;

        [TestMethod]
        public void GetAllServices()
        {
            ServiceDict.Add((typeof(string), null), "blub");

            var result = Context.GetAllServices();

            Assert.AreCollectionsEqual(ServiceDict, result);
            Assert.ThrowsException<NotSupportedException>(() => ((IDictionary<(Type, string?), object>)result)[(typeof(string), null)] = "Hello");
            Assert.ThrowsException<NotSupportedException>(() => ((IDictionary<(Type, string?), object>)result).Add((typeof(int), null), 2));
        }

        [TestMethod]
        public void GetAllServices_WithType()
        {
            ServiceDict.Add((typeof(string), null), "blub");
            ServiceDict.Add((typeof(string), "test"), "blub2");
            ServiceDict.Add((typeof(object), null), "blub3");

            var result = Context.GetAllServices(typeof(string));

            Assert.AreCollectionsEquivalent(
                new (string?, object)[]
                {
                    (null, "blub"),
                    ("test", "blub2"),
                },
                result);
        }

        [TestMethod]
        public void AddService_NullInstance()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Context.AddService(typeof(object), (object?)null!));
        }

        [TestMethod]
        public void AddService_InstanceHasWrongType()
        {
            Assert.ThrowsException<ArgumentException>(() => Context.AddService(typeof(string), 123));
        }

        [TestMethod]
        public void AddService_Success_Add()
        {
            Context.AddService(typeof(string), "MyTest", "MyName");

            Assert.AreCollectionsEquivalent(
                new Dictionary<(Type, string?), object>
                {
                    [(typeof(string), "MyName")] = "MyTest",
                },
                ServiceDict);
        }

        [TestMethod]
        public void AddService_Success_Add_WithEvents()
        {
            var changingHandlerMock = new Mock<ServiceContextEventHandler>(MockBehavior.Strict);
            var changedHandlerMock = new Mock<ServiceContextEventHandler>(MockBehavior.Strict);
            var expectedArgs = new ServiceContextEventArgs("MyName", typeof(string), null, "MyTest", ServiceAction.Added);
            changingHandlerMock.Setup(x => x(Context, expectedArgs)).Verifiable(this, Times.Once(), "Changing event handler.");
            changedHandlerMock.Setup(x => x(Context, expectedArgs)).Verifiable(this, Times.Once(), "Changed event handler.");

            Context.Changing += changingHandlerMock.Object;
            Context.Changed += changedHandlerMock.Object;

            Context.AddService(typeof(string), "MyTest", "MyName");

            Assert.AreCollectionsEquivalent(
                new Dictionary<(Type, string?), object>
                {
                    [(typeof(string), "MyName")] = "MyTest",
                },
                ServiceDict);
        }

        [TestMethod]
        public void AddService_Success_Overwrite()
        {
            ServiceDict.Add((typeof(string), "MyName"), "MyPreviousTest");

            Context.AddService(typeof(string), "MyTest", "MyName");

            Assert.AreCollectionsEquivalent(
                new Dictionary<(Type, string?), object>
                {
                    [(typeof(string), "MyName")] = "MyTest",
                },
                ServiceDict);
        }

        [TestMethod]
        public void AddService_Success_Overwrite_WithEvents()
        {
            var changingHandlerMock = new Mock<ServiceContextEventHandler>(MockBehavior.Strict);
            var changedHandlerMock = new Mock<ServiceContextEventHandler>(MockBehavior.Strict);
            var expectedArgs = new ServiceContextEventArgs("MyName", typeof(string), "MyPreviousTest", "MyTest", ServiceAction.Changed);
            changingHandlerMock.Setup(x => x(Context, expectedArgs)).Verifiable(this, Times.Once(), "Changing event handler.");
            changedHandlerMock.Setup(x => x(Context, expectedArgs)).Verifiable(this, Times.Once(), "Changed event handler.");

            Context.Changing += changingHandlerMock.Object;
            Context.Changed += changedHandlerMock.Object;
            ServiceDict.Add((typeof(string), "MyName"), "MyPreviousTest");

            Context.AddService(typeof(string), "MyTest", "MyName");

            Assert.AreCollectionsEquivalent(
                new Dictionary<(Type, string?), object>
                {
                    [(typeof(string), "MyName")] = "MyTest",
                },
                ServiceDict);
        }

        [TestMethod]
        public void GetService_DoesNotExist()
        {
            Assert.ThrowsException<KeyNotFoundException>(() => Context.GetService(typeof(string), "MyName"));
        }

        [TestMethod]
        public void GetService_Exists()
        {
            var obj = new object();
            ServiceDict.Add((typeof(object), "MyName"), obj);

            var result = Context.GetService(typeof(object), "MyName");

            Assert.AreSame(obj, result);
        }

        [TestMethod]
        public void RemoveService_DoesNotExist()
        {
            Assert.ThrowsException<KeyNotFoundException>(() => Context.RemoveService(typeof(string), "MyName"));
        }

        [TestMethod]
        public void RemoveService_Exists()
        {
            ServiceDict.Add((typeof(string), "MyName"), "MyPreviousTest");
            ServiceDict.Add((typeof(string), "MyName2"), "MyPreviousTest2");

            Context.RemoveService(typeof(string), "MyName");

            Assert.AreCollectionsEquivalent(
                new Dictionary<(Type, string?), object>
                {
                    [(typeof(string), "MyName2")] = "MyPreviousTest2",
                },
                ServiceDict);
        }

        [TestMethod]
        public void RemoveService_Exists_WithEvents()
        {
            var changingHandlerMock = new Mock<ServiceContextEventHandler>(MockBehavior.Strict);
            var changedHandlerMock = new Mock<ServiceContextEventHandler>(MockBehavior.Strict);
            var expectedArgs = new ServiceContextEventArgs("MyName", typeof(string), "MyPreviousTest", null, ServiceAction.Removed);
            changingHandlerMock.Setup(x => x(Context, expectedArgs)).Verifiable(this, Times.Once(), "Changing event handler.");
            changedHandlerMock.Setup(x => x(Context, expectedArgs)).Verifiable(this, Times.Once(), "Changed event handler.");

            Context.Changing += changingHandlerMock.Object;
            Context.Changed += changedHandlerMock.Object;
            ServiceDict.Add((typeof(string), "MyName"), "MyPreviousTest");
            ServiceDict.Add((typeof(string), "MyName2"), "MyPreviousTest2");

            Context.RemoveService(typeof(string), "MyName");

            Assert.AreCollectionsEquivalent(
                new Dictionary<(Type, string?), object>
                {
                    [(typeof(string), "MyName2")] = "MyPreviousTest2",
                },
                ServiceDict);
        }

        [TestMethod]
        public void ContainsService_True()
        {
            ServiceDict.Add((typeof(string), "MyName"), "MyTest");

            Assert.IsTrue(Context.ContainsService(typeof(string), "MyName"));
        }

        [TestMethod]
        public void ContainsService_False()
        {
            Assert.IsFalse(Context.ContainsService(typeof(string), "MyName"));
        }

        [TestMethod]
        public void Clear_NullType()
        {
            var obj = new object();
            ServiceDict.Add((typeof(string), "MyName"), "MyPreviousTest");
            ServiceDict.Add((typeof(object), "MyName"), obj);

            Context.Clear(null);

            Assert.IsEmpty(ServiceDict);
        }

        [TestMethod]
        public void Clear_NullType_WithEvents()
        {
            var obj = new object();
            var changingHandlerMock = new Mock<ServiceContextEventHandler>(MockBehavior.Strict);
            var changedHandlerMock = new Mock<ServiceContextEventHandler>(MockBehavior.Strict);
            var expectedArgs1 = new ServiceContextEventArgs("MyName", typeof(string), "MyPreviousTest", null, ServiceAction.Removed);
            var expectedArgs2 = new ServiceContextEventArgs("MyName", typeof(object), obj, null, ServiceAction.Removed);
            changingHandlerMock.Setup(x => x(Context, expectedArgs1)).Verifiable(this, Times.Once(), "Changing event handler.");
            changingHandlerMock.Setup(x => x(Context, expectedArgs2)).Verifiable(this, Times.Once(), "Changing event handler.");
            changedHandlerMock.Setup(x => x(Context, expectedArgs1)).Verifiable(this, Times.Once(), "Changed event handler.");
            changedHandlerMock.Setup(x => x(Context, expectedArgs2)).Verifiable(this, Times.Once(), "Changed event handler.");

            Context.Changing += changingHandlerMock.Object;
            Context.Changed += changedHandlerMock.Object;
            ServiceDict.Add((typeof(string), "MyName"), "MyPreviousTest");
            ServiceDict.Add((typeof(object), "MyName"), obj);

            Context.Clear(null);

            Assert.IsEmpty(ServiceDict);
        }

        [TestMethod]
        public void Clear_NullType_NothingToRemove()
        {
            Context.Clear(null);

            Assert.IsEmpty(ServiceDict);
        }

        [TestMethod]
        public void Clear_NullType_NothingToRemove_WithEvents()
        {
            var changingHandlerMock = new Mock<ServiceContextEventHandler>(MockBehavior.Strict);
            var changedHandlerMock = new Mock<ServiceContextEventHandler>(MockBehavior.Strict);

            Context.Changing += changingHandlerMock.Object;
            Context.Changed += changedHandlerMock.Object;

            Context.Clear(null);

            Assert.IsEmpty(ServiceDict);
        }

        [TestMethod]
        public void Clear_WithType()
        {
            var obj = new object();
            ServiceDict.Add((typeof(string), "MyName"), "MyPreviousTest");
            ServiceDict.Add((typeof(object), "MyName"), obj);

            Context.Clear(typeof(string));

            Assert.AreCollectionsEquivalent(
                new Dictionary<(Type, string?), object>
                {
                    [(typeof(object), "MyName")] = obj,
                },
                ServiceDict);
        }

        [TestMethod]
        public void Clear_WithType_WithEvents()
        {
            var obj = new object();
            var changingHandlerMock = new Mock<ServiceContextEventHandler>(MockBehavior.Strict);
            var changedHandlerMock = new Mock<ServiceContextEventHandler>(MockBehavior.Strict);
            var expectedArgs1 = new ServiceContextEventArgs("MyName", typeof(string), "MyPreviousTest", null, ServiceAction.Removed);
            changingHandlerMock.Setup(x => x(Context, expectedArgs1)).Verifiable(this, Times.Once(), "Changing event handler.");
            changedHandlerMock.Setup(x => x(Context, expectedArgs1)).Verifiable(this, Times.Once(), "Changed event handler.");

            Context.Changing += changingHandlerMock.Object;
            Context.Changed += changedHandlerMock.Object;
            ServiceDict.Add((typeof(string), "MyName"), "MyPreviousTest");
            ServiceDict.Add((typeof(object), "MyName"), obj);

            Context.Clear(typeof(string));

            Assert.AreCollectionsEquivalent(
                new Dictionary<(Type, string?), object>
                {
                    [(typeof(object), "MyName")] = obj,
                },
                ServiceDict);
        }

        [TestMethod]
        public void Clear_WithType_NothingToRemove()
        {
            var obj = new object();
            ServiceDict.Add((typeof(object), "MyName"), obj);

            Context.Clear(typeof(string));

            Assert.AreCollectionsEquivalent(
                new Dictionary<(Type, string?), object>
                {
                    [(typeof(object), "MyName")] = obj,
                },
                ServiceDict);
        }

        [TestMethod]
        public void Clear_WithType_NothingToRemove_WithEvents()
        {
            var obj = new object();
            var changingHandlerMock = new Mock<ServiceContextEventHandler>(MockBehavior.Strict);
            var changedHandlerMock = new Mock<ServiceContextEventHandler>(MockBehavior.Strict);

            Context.Changing += changingHandlerMock.Object;
            Context.Changed += changedHandlerMock.Object;

            ServiceDict.Add((typeof(object), "MyName"), obj);

            Context.Clear(typeof(string));

            Assert.AreCollectionsEquivalent(
                new Dictionary<(Type, string?), object>
                {
                    [(typeof(object), "MyName")] = obj,
                },
                ServiceDict);
        }

        [TestMethod]
        public void GetView_New()
        {
            var view = Context.GetView<string>();

            var sView = Assert.IsInstanceOfType<ServiceContext<string>>(view);
            Assert.AreSame(Context, sView.Context);
            Assert.Contains(new KeyValuePair<Type, IDisposable>(typeof(string), view), ViewsDict);
        }

        [TestMethod]
        public void GetView_FromCache()
        {
            var viewMock = new Mock<IServiceContext<string>>(MockBehavior.Strict);
            ViewsDict.Add(typeof(string), viewMock.Object);

            var view = Context.GetView<string>();

            Assert.AreSame(viewMock.Object, view);
        }

        [TestMethod]
        public void Dispose_()
        {
            var viewMock1 = new Mock<IServiceContext<string>>(MockBehavior.Strict);
            viewMock1.Setup(x => x.Dispose()).Verifiable(this, Times.Once());
            ViewsDict.Add(typeof(string), viewMock1.Object);
            var viewMock2 = new Mock<IServiceContext<object>>(MockBehavior.Strict);
            viewMock2.Setup(x => x.Dispose()).Verifiable(this, Times.Once());
            ViewsDict.Add(typeof(object), viewMock2.Object);

            ServiceDict.Add((typeof(string), null), "blub");
            ServiceDict.Add((typeof(int), null), 2);

            Context.Dispose();

            Assert.IsEmpty(ViewsDict);
            Assert.IsEmpty(ServiceDict);
        }

        internal static IDictionary<(Type Type, string? Name), object> GetServicesDict(object? instance)
        {
            var servicesField = typeof(ServiceContext).GetField("_services", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            return (IDictionary<(Type, string?), object>)servicesField!.GetValue(instance)!;
        }

        internal static IDictionary<Type, IDisposable> GetViewsDict(object? instance)
        {
            var servicesField = typeof(ServiceContext).GetField("_views", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            return (IDictionary<Type, IDisposable>)servicesField!.GetValue(instance)!;
        }
    }

    [TestClass]
    public class ServicecontextTTests : TestClassBase
    {
        private Mock<IServiceContext> ContextMock => Cache.GetValue(() => CreateContextMock())!;
        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP012:Property should not return created disposable.", Justification = "Should be fine here")]
        private IServiceContext<string> Context => Cache.GetValue(() => new ServiceContext<string>(ContextMock.Object))!;

        [TestMethod]
        public void Constructor()
        {
            using var context = new ServiceContext<string>(ContextMock.Object);

            ContextMock.VerifyAdd(x => x.Changing += It.IsAny<ServiceContextEventHandler>(), Times.Once());
            ContextMock.VerifyAdd(x => x.Changed += It.IsAny<ServiceContextEventHandler>(), Times.Once());
        }

        [TestMethod]
        public void GetAllServices()
        {
            ContextMock
                .Setup(x => x.GetAllServices(typeof(string)))
                .Returns(new (string?, object)[] { ("blub", "Test123"), ("zzz", "bar") })
                .Verifiable(this, Times.Once());

            var result = Context.GetAllServices();

            Assert.AreCollectionsEquivalent(new (string?, string)[] { ("blub", "Test123"), ("zzz", "bar") }, result);
        }

        [TestMethod]
        public void AddService()
        {
            ContextMock.Setup(x => x.AddService(typeof(string), "MyTest", "MyName")).Verifiable(this, Times.Once());

            Context.AddService("MyTest", "MyName");
        }

        [TestMethod]
        public void GetService()
        {
            ContextMock.Setup(x => x.GetService(typeof(string), "MyName")).Returns("MyTest").Verifiable(this, Times.Once());

            var result = Context.GetService("MyName");

            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void ContainsService_True()
        {
            ContextMock.Setup(x => x.ContainsService(typeof(string), "MyName")).Returns(true).Verifiable(this, Times.Once());

            Assert.IsTrue(Context.ContainsService("MyName"));
        }

        [TestMethod]
        public void ContainsService_False()
        {
            ContextMock.Setup(x => x.ContainsService(typeof(string), "MyName")).Returns(false).Verifiable(this, Times.Once());

            Assert.IsFalse(Context.ContainsService("MyName"));
        }

        [TestMethod]
        public void RemoveService()
        {
            ContextMock.Setup(x => x.RemoveService(typeof(string), "MyName")).Verifiable(this, Times.Once());

            Context.RemoveService("MyName");
        }

        [TestMethod]
        public void Clear()
        {
            ContextMock.Setup(x => x.Clear(typeof(string))).Verifiable(this, Times.Once());

            Context.Clear();
        }

        [TestMethod]
        public void Dispose_()
        {
            ContextMock
                .SetupAdd(x => x.Changing += It.IsAny<ServiceContextEventHandler>())
                .Callback<ServiceContextEventHandler>(
                    @event => ContextMock.SetupRemove(x => x.Changing -= @event).Verifiable(this, Times.Once(), "Changing event handler (remove)."))
                .Verifiable(this, Times.Once(), "Changing event handler (add)");
            ContextMock
                .SetupAdd(x => x.Changed += It.IsAny<ServiceContextEventHandler>())
                .Callback<ServiceContextEventHandler>(
                    @event => ContextMock.SetupRemove(x => x.Changed -= @event).Verifiable(this, Times.Once(), "Changed event handler (remove)."))
                .Verifiable(this, Times.Once(), "Changed event handler (add)");

            Context.Dispose();
        }

        [TestMethod]
        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP001:Dispose created.", Justification = "Mock is verified, no need to dispose")]
        public void ChangingEvent_WrongType()
        {
            ServiceContextEventHandler? contextHandler = null;
            var eventVerifiable = ContextMock
                .SetupAdd(x => x.Changing += It.IsAny<ServiceContextEventHandler>())
                .Callback<ServiceContextEventHandler>(@event => contextHandler = @event)
                .Verifiable(Times.Once(), "Changing event handler (add)");
            var eventHandlerMock = new Mock<ServiceContextEventHandler<string>>(MockBehavior.Strict);

            Context.Changing += eventHandlerMock.Object;

            eventVerifiable.Verify();
            contextHandler!.Invoke(ContextMock.Object, new ServiceContextEventArgs("MyName", typeof(object), null, new object(), ServiceAction.Added));
        }

        [TestMethod]
        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP001:Dispose created.", Justification = "Mock is verified, no need to dispose")]
        public void ChangingEvent_CorrectType()
        {
            ServiceContextEventHandler? contextHandler = null;
            var eventVerifiable = ContextMock
                .SetupAdd(x => x.Changing += It.IsAny<ServiceContextEventHandler>())
                .Callback<ServiceContextEventHandler>(@event => contextHandler = @event)
                .Verifiable(Times.Once(), "Changing event handler (add)");
            var eventHandlerMock = new Mock<ServiceContextEventHandler<string>>(MockBehavior.Strict);
            eventHandlerMock
                .Setup(x => x(Context, new ServiceContextEventArgs<string>("MyName", "OldValue", "NewValue", ServiceAction.Changed)))
                .Callback<object?, ServiceContextEventArgs<string>>((e, a) => Assert.AreEqual(typeof(string), a.Type))
                .Verifiable(this, Times.Once());

            Context.Changing += eventHandlerMock.Object;

            eventVerifiable.Verify();
            contextHandler!.Invoke(ContextMock.Object, new ServiceContextEventArgs("MyName", typeof(string), "OldValue", "NewValue", ServiceAction.Changed));
        }

        [TestMethod]
        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP001:Dispose created.", Justification = "Mock is verified, no need to dispose")]
        public void ChangingEvent_CorrectType_NullHandler()
        {
            ServiceContextEventHandler? contextHandler = null;
            var eventVerifiable = ContextMock
                .SetupAdd(x => x.Changing += It.IsAny<ServiceContextEventHandler>())
                .Callback<ServiceContextEventHandler>(@event => contextHandler = @event)
                .Verifiable(Times.Once(), "Changing event handler (add)");

            _ = Context;

            eventVerifiable.Verify();
            contextHandler!.Invoke(ContextMock.Object, new ServiceContextEventArgs("MyName", typeof(string), "OldValue", "NewValue", ServiceAction.Changed));
        }

        [TestMethod]
        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP001:Dispose created.", Justification = "Mock is verified, no need to dispose")]
        public void ChangedEvent_WrongType()
        {
            ServiceContextEventHandler? contextHandler = null;
            var eventVerifiable = ContextMock
                .SetupAdd(x => x.Changed += It.IsAny<ServiceContextEventHandler>())
                .Callback<ServiceContextEventHandler>(@event => contextHandler = @event)
                .Verifiable(Times.Once(), "Changed event handler (add)");
            var eventHandlerMock = new Mock<ServiceContextEventHandler<string>>(MockBehavior.Strict);

            Context.Changed += eventHandlerMock.Object;

            eventVerifiable.Verify();
            contextHandler!.Invoke(ContextMock.Object, new ServiceContextEventArgs("MyName", typeof(object), null, new object(), ServiceAction.Added));
        }

        [TestMethod]
        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP001:Dispose created.", Justification = "Mock is verified, no need to dispose")]
        public void ChangedEvent_CorrectType()
        {
            ServiceContextEventHandler? contextHandler = null;
            var eventVerifiable = ContextMock
                .SetupAdd(x => x.Changed += It.IsAny<ServiceContextEventHandler>())
                .Callback<ServiceContextEventHandler>(@event => contextHandler = @event)
                .Verifiable(Times.Once(), "Changed event handler (add)");
            var eventHandlerMock = new Mock<ServiceContextEventHandler<string>>(MockBehavior.Strict);
            eventHandlerMock
                .Setup(x => x(Context, new ServiceContextEventArgs<string>("MyName", "OldValue", "NewValue", ServiceAction.Changed)))
                .Callback<object?, ServiceContextEventArgs<string>>((e, a) => Assert.AreEqual(typeof(string), a.Type))
                .Verifiable(this, Times.Once());

            Context.Changed += eventHandlerMock.Object;

            eventVerifiable.Verify();
            contextHandler!.Invoke(ContextMock.Object, new ServiceContextEventArgs("MyName", typeof(string), "OldValue", "NewValue", ServiceAction.Changed));
        }

        [TestMethod]
        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP001:Dispose created.", Justification = "Mock is verified, no need to dispose")]
        public void ChangedEvent_CorrectType_NullHandler()
        {
            ServiceContextEventHandler? contextHandler = null;
            var eventVerifiable = ContextMock
                .SetupAdd(x => x.Changed += It.IsAny<ServiceContextEventHandler>())
                .Callback<ServiceContextEventHandler>(@event => contextHandler = @event)
                .Verifiable(Times.Once(), "Changed event handler (add)");

            _ = Context;

            eventVerifiable.Verify();
            contextHandler!.Invoke(ContextMock.Object, new ServiceContextEventArgs("MyName", typeof(string), "OldValue", "NewValue", ServiceAction.Changed));
        }

        private static Mock<IServiceContext> CreateContextMock()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.SetupAdd(x => x.Changing += It.IsAny<ServiceContextEventHandler>());
            mock.SetupAdd(x => x.Changed += It.IsAny<ServiceContextEventHandler>());
            return mock;
        }
    }

    [TestClass]
    public class ServiceContextStaticTests : TestClassBase
    {
        private static readonly FieldInfo InstanceField = typeof(ServiceContext).GetField("_instance", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly)!;

        protected override void OnCleanupTest()
        {
            ClearMockInstance();
        }

        [TestMethod]
        public void Instance_NonExistingInstance()
        {
            Assert.IsNull(InstanceField.GetValue(null));
            var instance = ServiceContext.Instance;
            Assert.IsNotNull(instance);
            Assert.IsNotNull(InstanceField.GetValue(null));
            Assert.AreSame(InstanceField.GetValue(null), instance);
        }

        [TestMethod]
        public void Instance_ExistingInstance()
        {
            var preInstance = SetMockInstance().Object;
            InstanceField.SetValue(null, preInstance);
            var instance = ServiceContext.Instance;
            Assert.AreSame(preInstance, instance);
        }

        [TestMethod]
        public void CreateContext()
        {
            using var context = ServiceContext.CreateContext();

            Assert.IsNotNull(context);
            Assert.AreEqual(0, ServiceContextTests.GetServicesDict(context).Count);
            Assert.IsNull(InstanceField.GetValue(null));
        }

        [TestMethod]
        public void GetAllServices()
        {
            var mock = SetMockInstance();
            var dict = new Mock<IReadOnlyDictionary<(Type, string?), object>>();
            mock.Setup(x => x.GetAllServices()).Returns(dict.Object).Verifiable(this, Times.Once());

            var result = ServiceContext.GetAllServices();

            Assert.AreSame(dict.Object, result);
        }

        [TestMethod]
        public void GetAllServicesT()
        {
            SetMockInstance()
                .Setup(x => x.GetAllServices(typeof(string)))
                .Returns(new (string?, object)[] { ("blub", "Test123"), ("zzz", "bar") })
                .Verifiable(this, Times.Once());

            var result = ServiceContext.GetAllServices<string>();

            Assert.IsTrue(result.SequenceEqual(new (string?, string)[] { ("blub", "Test123"), ("zzz", "bar") }));
        }

        [TestMethod]
        public void GetAllServices_WithServiceType()
        {
            var list = new Mock<IEnumerable<(string?, object)>>();
            SetMockInstance().Setup(x => x.GetAllServices(typeof(string))).Returns(list.Object).Verifiable(this, Times.Once());

            var result = ServiceContext.GetAllServices(typeof(string));

            Assert.AreSame(list.Object, result);
        }

        [TestMethod]
        public void AddServiceT()
        {
            SetMockInstance().Setup(x => x.AddService(typeof(string), "MyTest", null)).Verifiable(this, Times.Once());

            ServiceContext.AddService("MyTest");
        }

        [TestMethod]
        public void AddServiceT_WithName()
        {
            SetMockInstance().Setup(x => x.AddService(typeof(string), "MyTest", "blub")).Verifiable(this, Times.Once());

            ServiceContext.AddService("MyTest", "blub");
        }

        [TestMethod]
        public void AddService()
        {
            SetMockInstance().Setup(x => x.AddService(typeof(string), "MyTest", null)).Verifiable(this, Times.Once());

            ServiceContext.AddService(typeof(string), (object)"MyTest");
        }

        [TestMethod]
        public void AddService_WithName()
        {
            SetMockInstance().Setup(x => x.AddService(typeof(string), "MyTest", "blub")).Verifiable(this, Times.Once());

            ServiceContext.AddService(typeof(string), "MyTest", "blub");
        }

        [TestMethod]
        public void GetServiceTOut()
        {
            SetMockInstance().Setup(x => x.GetService(typeof(string), null)).Returns("MyTest").Verifiable(this, Times.Once());

            ServiceContext.GetService(out string result);

            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void GetServiceTOut_WithName()
        {
            SetMockInstance().Setup(x => x.GetService(typeof(string), "blub")).Returns("MyTest").Verifiable(this, Times.Once());

            ServiceContext.GetService(out string result, "blub");

            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void GetServiceT()
        {
            SetMockInstance().Setup(x => x.GetService(typeof(string), null)).Returns("MyTest").Verifiable(this, Times.Once());

            var result = ServiceContext.GetService<string>();

            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void GetServiceT_WithName()
        {
            SetMockInstance().Setup(x => x.GetService(typeof(string), "blub")).Returns("MyTest").Verifiable(this, Times.Once());

            var result = ServiceContext.GetService<string>("blub");

            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void GetService()
        {
            SetMockInstance().Setup(x => x.GetService(typeof(string), null)).Returns("MyTest").Verifiable(this, Times.Once());

            var result = ServiceContext.GetService(typeof(string));

            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void GetService_WithName()
        {
            SetMockInstance().Setup(x => x.GetService(typeof(string), "blub")).Returns("MyTest").Verifiable(this, Times.Once());

            var result = ServiceContext.GetService(typeof(string), "blub");

            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void TryGetServiceTOut_Found()
        {
            var mock = SetMockInstance();
            mock.Setup(x => x.ContainsService(typeof(string), null)).Returns(true).Verifiable(this, Times.Once());
            mock.Setup(x => x.GetService(typeof(string), null)).Returns("MyTest").Verifiable(this, Times.Once());

            var success = ServiceContext.TryGetService<string>(out var result);

            Assert.IsTrue(success);
            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void TryGetServiceTOut_NotFound()
        {
            SetMockInstance().Setup(x => x.ContainsService(typeof(string), null)).Returns(false).Verifiable(this, Times.Once());

            var success = ServiceContext.TryGetService<string>(out var result);

            Assert.IsFalse(success);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TryGetServiceTOut_Found_WithName()
        {
            var mock = SetMockInstance();
            mock.Setup(x => x.ContainsService(typeof(string), "blub")).Returns(true).Verifiable(this, Times.Once());
            mock.Setup(x => x.GetService(typeof(string), "blub")).Returns("MyTest").Verifiable(this, Times.Once());

            var success = ServiceContext.TryGetService<string>(out var result, "blub");

            Assert.IsTrue(success);
            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void TryGetServiceTOut_NotFound_WithName()
        {
            SetMockInstance().Setup(x => x.ContainsService(typeof(string), "blub")).Returns(false).Verifiable(this, Times.Once());

            var success = ServiceContext.TryGetService<string>(out var result, "blub");

            Assert.IsFalse(success);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TryGetServiceT_Found()
        {
            var mock = SetMockInstance();
            mock.Setup(x => x.ContainsService(typeof(string), null)).Returns(true).Verifiable(this, Times.Once());
            mock.Setup(x => x.GetService(typeof(string), null)).Returns("MyTest").Verifiable(this, Times.Once());

            var result = ServiceContext.TryGetService<string>();

            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void TryGetServiceT_NotFound()
        {
            SetMockInstance().Setup(x => x.ContainsService(typeof(string), null)).Returns(false).Verifiable(this, Times.Once());

            var result = ServiceContext.TryGetService<string>();

            Assert.IsNull(result);
        }

        [TestMethod]
        public void TryGetServiceT_Found_WithName()
        {
            var mock = SetMockInstance();
            mock.Setup(x => x.ContainsService(typeof(string), "blub")).Returns(true).Verifiable(this, Times.Once());
            mock.Setup(x => x.GetService(typeof(string), "blub")).Returns("MyTest").Verifiable(this, Times.Once());

            var result = ServiceContext.TryGetService<string>("blub");

            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void TryGetServiceT_NotFound_WithName()
        {
            SetMockInstance().Setup(x => x.ContainsService(typeof(string), "blub")).Returns(false).Verifiable(this, Times.Once());

            var result = ServiceContext.TryGetService<string>("blub");

            Assert.IsNull(result);
        }

        [TestMethod]
        public void TryGetService_Found()
        {
            var mock = SetMockInstance();
            mock.Setup(x => x.ContainsService(typeof(string), null)).Returns(true).Verifiable(this, Times.Once());
            mock.Setup(x => x.GetService(typeof(string), null)).Returns("MyTest").Verifiable(this, Times.Once());

            var result = ServiceContext.TryGetService(typeof(string));

            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void TryGetService_NotFound()
        {
            SetMockInstance().Setup(x => x.ContainsService(typeof(string), null)).Returns(false).Verifiable(this, Times.Once());

            var result = ServiceContext.TryGetService(typeof(string));

            Assert.IsNull(result);
        }

        [TestMethod]
        public void TryGetService_Found_WithName()
        {
            var mock = SetMockInstance();
            mock.Setup(x => x.ContainsService(typeof(string), "blub")).Returns(true).Verifiable(this, Times.Once());
            mock.Setup(x => x.GetService(typeof(string), "blub")).Returns("MyTest").Verifiable(this, Times.Once());

            var result = ServiceContext.TryGetService(typeof(string), "blub");

            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void TryGetService_NotFound_WithName()
        {
            SetMockInstance().Setup(x => x.ContainsService(typeof(string), "blub")).Returns(false).Verifiable(this, Times.Once());

            var result = ServiceContext.TryGetService(typeof(string), "blub");

            Assert.IsNull(result);
        }

        [TestMethod]
        public void RemoveServiceT()
        {
            SetMockInstance().Setup(x => x.RemoveService(typeof(string), null)).Verifiable(this, Times.Once());

            ServiceContext.RemoveService<string>();
        }

        [TestMethod]
        public void RemoveServiceT_WithName()
        {
            SetMockInstance().Setup(x => x.RemoveService(typeof(string), "blub")).Verifiable(this, Times.Once());

            ServiceContext.RemoveService<string>("blub");
        }

        [TestMethod]
        public void RemoveService()
        {
            SetMockInstance().Setup(x => x.RemoveService(typeof(string), null)).Verifiable(this, Times.Once());

            ServiceContext.RemoveService(typeof(string));
        }

        [TestMethod]
        public void RemoveService_WithName()
        {
            SetMockInstance().Setup(x => x.RemoveService(typeof(string), "blub")).Verifiable(this, Times.Once());

            ServiceContext.RemoveService(typeof(string), "blub");
        }

        [TestMethod]
        public void TryRemoveServiceT_Found()
        {
            var mock = SetMockInstance();
            mock.Setup(x => x.ContainsService(typeof(string), null)).Returns(true).Verifiable(this, Times.Once());
            mock.Setup(x => x.RemoveService(typeof(string), null)).Verifiable(this, Times.Once());

            ServiceContext.TryRemoveService<string>();
        }

        [TestMethod]
        public void TryRemoveServiceT_NotFound()
        {
            SetMockInstance().Setup(x => x.ContainsService(typeof(string), null)).Returns(false).Verifiable(this, Times.Once());

            ServiceContext.TryRemoveService<string>();
        }

        [TestMethod]
        public void TryRemoveServiceT_Found_WithName()
        {
            var mock = SetMockInstance();
            mock.Setup(x => x.ContainsService(typeof(string), "blub")).Returns(true).Verifiable(this, Times.Once());
            mock.Setup(x => x.RemoveService(typeof(string), "blub")).Verifiable(this, Times.Once());

            ServiceContext.TryRemoveService<string>("blub");
        }

        [TestMethod]
        public void TryRemoveServiceT_NotFound_WithName()
        {
            SetMockInstance().Setup(x => x.ContainsService(typeof(string), "blub")).Returns(false).Verifiable(this, Times.Once());

            ServiceContext.TryRemoveService<string>("blub");
        }

        [TestMethod]
        public void TryRemoveService_Found()
        {
            var mock = SetMockInstance();
            mock.Setup(x => x.ContainsService(typeof(string), null)).Returns(true).Verifiable(this, Times.Once());
            mock.Setup(x => x.RemoveService(typeof(string), null)).Verifiable(this, Times.Once());

            ServiceContext.TryRemoveService(typeof(string));
        }

        [TestMethod]
        public void TryRemoveService_NotFound()
        {
            SetMockInstance().Setup(x => x.ContainsService(typeof(string), null)).Returns(false).Verifiable(this, Times.Once());

            ServiceContext.TryRemoveService(typeof(string));
        }

        [TestMethod]
        public void TryRemoveService_Found_WithName()
        {
            var mock = SetMockInstance();
            mock.Setup(x => x.ContainsService(typeof(string), "blub")).Returns(true).Verifiable(this, Times.Once());
            mock.Setup(x => x.RemoveService(typeof(string), "blub")).Verifiable(this, Times.Once());

            ServiceContext.TryRemoveService(typeof(string), "blub");
        }

        [TestMethod]
        public void TryRemoveService_NotFound_WithName()
        {
            SetMockInstance().Setup(x => x.ContainsService(typeof(string), "blub")).Returns(false).Verifiable(this, Times.Once());

            ServiceContext.TryRemoveService(typeof(string), "blub");
        }

        [TestMethod]
        public void ContainsServiceT_True()
        {
            SetMockInstance().Setup(x => x.ContainsService(typeof(string), null)).Returns(true).Verifiable(this, Times.Once());

            Assert.IsTrue(ServiceContext.ContainsService<string>());
        }

        [TestMethod]
        public void ContainsServiceT_False()
        {
            SetMockInstance().Setup(x => x.ContainsService(typeof(string), null)).Returns(false).Verifiable(this, Times.Once());

            Assert.IsFalse(ServiceContext.ContainsService<string>());
        }

        [TestMethod]
        public void ContainsServiceT_WithName_True()
        {
            SetMockInstance().Setup(x => x.ContainsService(typeof(string), "blub")).Returns(true).Verifiable(this, Times.Once());

            Assert.IsTrue(ServiceContext.ContainsService<string>("blub"));
        }

        [TestMethod]
        public void ContainsServiceT_WithName_False()
        {
            SetMockInstance().Setup(x => x.ContainsService(typeof(string), "blub")).Returns(false).Verifiable(this, Times.Once());

            Assert.IsFalse(ServiceContext.ContainsService<string>("blub"));
        }

        [TestMethod]
        public void ContainsService_True()
        {
            SetMockInstance().Setup(x => x.ContainsService(typeof(string), null)).Returns(true).Verifiable(this, Times.Once());

            Assert.IsTrue(ServiceContext.ContainsService(typeof(string)));
        }

        [TestMethod]
        public void ContainsService_false()
        {
            SetMockInstance().Setup(x => x.ContainsService(typeof(string), null)).Returns(false).Verifiable(this, Times.Once());

            Assert.IsFalse(ServiceContext.ContainsService(typeof(string)));
        }

        [TestMethod]
        public void ContainsService_WithName_True()
        {
            SetMockInstance().Setup(x => x.ContainsService(typeof(string), "blub")).Returns(true).Verifiable(this, Times.Once());

            Assert.IsTrue(ServiceContext.ContainsService(typeof(string), "blub"));
        }

        [TestMethod]
        public void ContainsService_WithName_False()
        {
            SetMockInstance().Setup(x => x.ContainsService(typeof(string), "blub")).Returns(false).Verifiable(this, Times.Once());

            Assert.IsFalse(ServiceContext.ContainsService(typeof(string), "blub"));
        }

        [TestMethod]
        public void Clear()
        {
            SetMockInstance().Setup(x => x.Clear(null)).Verifiable(this, Times.Once());

            ServiceContext.Clear();
        }

        [TestMethod]
        public void ClearT()
        {
            SetMockInstance().Setup(x => x.Clear(typeof(string))).Verifiable(this, Times.Once());

            ServiceContext.Clear<string>();
        }

        [TestMethod]
        public void Clear_WithType()
        {
            SetMockInstance().Setup(x => x.Clear(typeof(string))).Verifiable(this, Times.Once());

            ServiceContext.Clear(typeof(string));
        }

        internal static Mock<IServiceContext> SetMockInstance()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            InstanceField.SetValue(null, mock.Object);
            return mock;
        }

        internal static void ClearMockInstance()
        {
            InstanceField.SetValue(null, null);
        }
    }

    [TestClass]
    public class ServiceContextTStaticTests : TestClassBase
    {
        protected override void OnCleanupTest()
        {
            ServiceContextStaticTests.ClearMockInstance();
        }

        [TestMethod]
        public void GetAllServices()
        {
            SetMockInstance<string>()
                .Setup(x => x.GetAllServices())
                .Returns(new (string?, string)[] { ("blub", "Test123"), ("zzz", "bar") })
                .Verifiable(this, Times.Once());

            var result = ServiceContext<string>.GetAllServices();

            Assert.IsTrue(result.SequenceEqual(new (string?, string)[] { ("blub", "Test123"), ("zzz", "bar") }));
        }

        [TestMethod]
        public void AddService()
        {
            SetMockInstance<string>().Setup(x => x.AddService("MyTest", null)).Verifiable(this, Times.Once());

            ServiceContext<string>.AddService("MyTest");
        }

        [TestMethod]
        public void AddService_WithName()
        {
            SetMockInstance<string>().Setup(x => x.AddService("MyTest", "blub")).Verifiable(this, Times.Once());

            ServiceContext<string>.AddService("MyTest", "blub");
        }

        [TestMethod]
        public void GetServiceOut()
        {
            SetMockInstance<string>().Setup(x => x.GetService(null)).Returns("MyTest").Verifiable(this, Times.Once());

            ServiceContext<string>.GetService(out var result);

            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void GetServiceOut_WithName()
        {
            SetMockInstance<string>().Setup(x => x.GetService("blub")).Returns("MyTest").Verifiable(this, Times.Once());

            ServiceContext<string>.GetService(out var result, "blub");

            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void GetService()
        {
            SetMockInstance<string>().Setup(x => x.GetService(null)).Returns("MyTest").Verifiable(this, Times.Once());

            var result = ServiceContext<string>.GetService();

            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void GetService_WithName()
        {
            SetMockInstance<string>().Setup(x => x.GetService("blub")).Returns("MyTest").Verifiable(this, Times.Once());

            var result = ServiceContext<string>.GetService("blub");

            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void TryGetServiceOut_Found()
        {
            var mock = SetMockInstance<string>();
            mock.Setup(x => x.ContainsService(null)).Returns(true).Verifiable(this, Times.Once());
            mock.Setup(x => x.GetService(null)).Returns("MyTest").Verifiable(this, Times.Once());

            var success = ServiceContext<string>.TryGetService(out var result);

            Assert.IsTrue(success);
            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void TryGetServiceOut_NotFound()
        {
            var mock = SetMockInstance<string>();
            mock.Setup(x => x.ContainsService(null)).Returns(false).Verifiable(this, Times.Once());

            var success = ServiceContext<string>.TryGetService(out var result);

            Assert.IsFalse(success);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TryGetServiceOut_Found_WithName()
        {
            var mock = SetMockInstance<string>();
            mock.Setup(x => x.ContainsService("blub")).Returns(true).Verifiable(this, Times.Once());
            mock.Setup(x => x.GetService("blub")).Returns("MyTest").Verifiable(this, Times.Once());

            var success = ServiceContext<string>.TryGetService(out var result, "blub");

            Assert.IsTrue(success);
            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void TryGetServiceOut_NotFound_WithName()
        {
            var mock = SetMockInstance<string>();
            mock.Setup(x => x.ContainsService("blub")).Returns(false).Verifiable(this, Times.Once());

            var success = ServiceContext<string>.TryGetService(out var result, "blub");

            Assert.IsFalse(success);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TryGetService_Found()
        {
            var mock = SetMockInstance<string>();
            mock.Setup(x => x.ContainsService(null)).Returns(true).Verifiable(this, Times.Once());
            mock.Setup(x => x.GetService(null)).Returns("MyTest").Verifiable(this, Times.Once());

            var result = ServiceContext<string>.TryGetService();

            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void TryGetService_NotFound()
        {
            var mock = SetMockInstance<string>();
            mock.Setup(x => x.ContainsService(null)).Returns(false).Verifiable(this, Times.Once());

            var result = ServiceContext<string>.TryGetService();

            Assert.IsNull(result);
        }

        [TestMethod]
        public void TryGetService_Found_WithName()
        {
            var mock = SetMockInstance<string>();
            mock.Setup(x => x.ContainsService("blub")).Returns(true).Verifiable(this, Times.Once());
            mock.Setup(x => x.GetService("blub")).Returns("MyTest").Verifiable(this, Times.Once());

            var result = ServiceContext<string>.TryGetService("blub");

            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void TryGetService_NotFound_WithName()
        {
            var mock = SetMockInstance<string>();
            mock.Setup(x => x.ContainsService("blub")).Returns(false).Verifiable(this, Times.Once());

            var result = ServiceContext<string>.TryGetService("blub");

            Assert.IsNull(result);
        }

        [TestMethod]
        public void RemoveService()
        {
            SetMockInstance<string>().Setup(x => x.RemoveService(null)).Verifiable(this, Times.Once());

            ServiceContext<string>.RemoveService();
        }

        [TestMethod]
        public void RemoveService_WithName()
        {
            SetMockInstance<string>().Setup(x => x.RemoveService("blub")).Verifiable(this, Times.Once());

            ServiceContext<string>.RemoveService("blub");
        }

        [TestMethod]
        public void TryRemoveService_Found()
        {
            var mock = SetMockInstance<string>();
            mock.Setup(x => x.ContainsService(null)).Returns(true).Verifiable(this, Times.Once());
            mock.Setup(x => x.RemoveService(null)).Verifiable(this, Times.Once());

            ServiceContext<string>.TryRemoveService();
        }

        [TestMethod]
        public void TryRemoveService_NotFound()
        {
            var mock = SetMockInstance<string>();
            mock.Setup(x => x.ContainsService(null)).Returns(false).Verifiable(this, Times.Once());

            ServiceContext<string>.TryRemoveService();
        }

        [TestMethod]
        public void TryRemoveService_Found_WithName()
        {
            var mock = SetMockInstance<string>();
            mock.Setup(x => x.ContainsService("blub")).Returns(true).Verifiable(this, Times.Once());
            mock.Setup(x => x.RemoveService("blub")).Verifiable(this, Times.Once());

            ServiceContext<string>.TryRemoveService("blub");
        }

        [TestMethod]
        public void TryRemoveService_NotFound_WithName()
        {
            var mock = SetMockInstance<string>();
            mock.Setup(x => x.ContainsService("blub")).Returns(false).Verifiable(this, Times.Once());

            ServiceContext<string>.TryRemoveService("blub");
        }

        [TestMethod]
        public void ContainsService_True()
        {
            SetMockInstance<string>().Setup(x => x.ContainsService(null)).Returns(true).Verifiable(this, Times.Once());

            Assert.IsTrue(ServiceContext<string>.ContainsService());
        }

        [TestMethod]
        public void ContainsService_False()
        {
            SetMockInstance<string>().Setup(x => x.ContainsService(null)).Returns(false).Verifiable(this, Times.Once());

            Assert.IsFalse(ServiceContext<string>.ContainsService());
        }

        [TestMethod]
        public void ContainsService_True_WithName()
        {
            SetMockInstance<string>().Setup(x => x.ContainsService("blub")).Returns(true).Verifiable(this, Times.Once());

            Assert.IsTrue(ServiceContext<string>.ContainsService("blub"));
        }

        [TestMethod]
        public void ContainsService_False_WithName()
        {
            SetMockInstance<string>().Setup(x => x.ContainsService("blub")).Returns(false).Verifiable(this, Times.Once());

            Assert.IsFalse(ServiceContext<string>.ContainsService("blub"));
        }

        [TestMethod]
        public void Clear()
        {
            SetMockInstance<string>().Setup(x => x.Clear()).Verifiable(this, Times.Once());

            ServiceContext<string>.Clear();
        }

        internal static Mock<IServiceContext<T>> SetMockInstance<T>()
        {
            var mock = new Mock<IServiceContext<T>>(MockBehavior.Strict);

            var nonGenericInstanceMock = ServiceContextStaticTests.SetMockInstance();
            nonGenericInstanceMock.Setup(x => x.GetView<T>()).Returns(mock.Object);

            return mock;
        }
    }

    [TestClass]
    public class SerivceContextExtensionsTests : TestClassBase
    {
        #region IServiceContext

        [TestMethod]
        public void ISC_GetAllServicesT()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.GetAllServices(typeof(string)))
                .Returns(new (string?, object)[] { ("blub", "Test123"), ("zzz", "bar") })
                .Verifiable(this, Times.Once());

            var result = ServiceContextExtensions.GetAllServices<string>(mock.Object);

            Assert.IsTrue(result.SequenceEqual(new (string?, string)[] { ("blub", "Test123"), ("zzz", "bar") }));
        }

        [TestMethod]
        public void ISC_AddService()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.AddService(typeof(string), "MyTest", null)).Verifiable(this, Times.Once());

            ServiceContextExtensions.AddService(mock.Object, typeof(string), (object)"MyTest");
        }

        [TestMethod]
        public void ISC_AddServiceT()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.AddService(typeof(string), "MyTest", null)).Verifiable(this, Times.Once());

            ServiceContextExtensions.AddService(mock.Object, "MyTest");
        }

        [TestMethod]
        public void ISC_AddServiceT_WithName()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.AddService(typeof(string), "MyTest", "blub")).Verifiable(this, Times.Once());

            ServiceContextExtensions.AddService(mock.Object, "MyTest", "blub");
        }

        [TestMethod]
        public void ISC_GetService()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.GetService(typeof(string), null)).Returns("MyTest").Verifiable(this, Times.Once());

            var result = ServiceContextExtensions.GetService(mock.Object, typeof(string));

            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void ISC_GetServiceTOut()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.GetService(typeof(string), null)).Returns("MyTest").Verifiable(this, Times.Once());

            ServiceContextExtensions.GetService<string>(mock.Object, out var result);

            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void ISC_GetServiceTOut_Withname()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.GetService(typeof(string), "blub")).Returns("MyTest").Verifiable(this, Times.Once());

            ServiceContextExtensions.GetService<string>(mock.Object, out var result, "blub");

            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void ISC_GetServiceT()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.GetService(typeof(string), null)).Returns("MyTest").Verifiable(this, Times.Once());

            var result = ServiceContextExtensions.GetService<string>(mock.Object);

            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void ISC_GetServiceT_Withname()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.GetService(typeof(string), "blub")).Returns("MyTest").Verifiable(this, Times.Once());

            var result = ServiceContextExtensions.GetService<string>(mock.Object, "blub");

            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void ISC_TryGetServiceTOut_Found()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService(typeof(string), null)).Returns(true).Verifiable(this, Times.Once());
            mock.Setup(x => x.GetService(typeof(string), null)).Returns("MyTest").Verifiable(this, Times.Once());

            var success = ServiceContextExtensions.TryGetService<string>(mock.Object, out var result);

            Assert.IsTrue(success);
            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void ISC_TryGetServiceTOut_NotFound()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService(typeof(string), null)).Returns(false).Verifiable(this, Times.Once());

            var success = ServiceContextExtensions.TryGetService<string>(mock.Object, out var result);

            Assert.IsFalse(success);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ISC_TryGetServiceTOut_Found_WithName()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService(typeof(string), "blub")).Returns(true).Verifiable(this, Times.Once());
            mock.Setup(x => x.GetService(typeof(string), "blub")).Returns("MyTest").Verifiable(this, Times.Once());

            var success = ServiceContextExtensions.TryGetService<string>(mock.Object, out var result, "blub");

            Assert.IsTrue(success);
            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void ISC_TryGetServiceTOut_NotFound_WithName()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService(typeof(string), "blub")).Returns(false).Verifiable(this, Times.Once());

            var success = ServiceContextExtensions.TryGetService<string>(mock.Object, out var result, "blub");

            Assert.IsFalse(success);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ISC_TryGetServiceT_Found()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService(typeof(string), null)).Returns(true).Verifiable(this, Times.Once());
            mock.Setup(x => x.GetService(typeof(string), null)).Returns("MyTest").Verifiable(this, Times.Once());

            var result = ServiceContextExtensions.TryGetService<string>(mock.Object);

            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void ISC_TryGetServiceT_NotFound()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService(typeof(string), null)).Returns(false).Verifiable(this, Times.Once());

            var result = ServiceContextExtensions.TryGetService<string>(mock.Object);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void ISC_TryGetServiceT_Found_WithName()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService(typeof(string), "blub")).Returns(true).Verifiable(this, Times.Once());
            mock.Setup(x => x.GetService(typeof(string), "blub")).Returns("MyTest").Verifiable(this, Times.Once());

            var result = ServiceContextExtensions.TryGetService<string>(mock.Object, "blub");

            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void ISC_TryGetServiceT_NotFound_WithName()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService(typeof(string), "blub")).Returns(false).Verifiable(this, Times.Once());

            var result = ServiceContextExtensions.TryGetService<string>(mock.Object, "blub");

            Assert.IsNull(result);
        }

        [TestMethod]
        public void ISC_TryGetService_Found()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService(typeof(string), null)).Returns(true).Verifiable(this, Times.Once());
            mock.Setup(x => x.GetService(typeof(string), null)).Returns("MyTest").Verifiable(this, Times.Once());

            var result = ServiceContextExtensions.TryGetService(mock.Object, typeof(string));

            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void ISC_TryGetService_NotFound()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService(typeof(string), null)).Returns(false).Verifiable(this, Times.Once());

            var result = ServiceContextExtensions.TryGetService(mock.Object, typeof(string));

            Assert.IsNull(result);
        }

        [TestMethod]
        public void ISC_TryGetService_Found_WithName()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService(typeof(string), "blub")).Returns(true).Verifiable(this, Times.Once());
            mock.Setup(x => x.GetService(typeof(string), "blub")).Returns("MyTest").Verifiable(this, Times.Once());

            var result = ServiceContextExtensions.TryGetService(mock.Object, typeof(string), "blub");

            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void ISC_TryGetService_NotFound_WithName()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService(typeof(string), "blub")).Returns(false).Verifiable(this, Times.Once());

            var result = ServiceContextExtensions.TryGetService(mock.Object, typeof(string), "blub");

            Assert.IsNull(result);
        }

        [TestMethod]
        public void ISC_ContainsServiceT_True()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService(typeof(string), null)).Returns(true).Verifiable(this, Times.Once());

            Assert.IsTrue(ServiceContextExtensions.ContainsService<string>(mock.Object));
        }

        [TestMethod]
        public void ISC_ContainsServiceT_False()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService(typeof(string), null)).Returns(false).Verifiable(this, Times.Once());

            Assert.IsFalse(ServiceContextExtensions.ContainsService<string>(mock.Object));
        }

        [TestMethod]
        public void ISC_ContainsServiceT_WithName_True()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService(typeof(string), "blub")).Returns(true).Verifiable(this, Times.Once());

            Assert.IsTrue(ServiceContextExtensions.ContainsService<string>(mock.Object, "blub"));
        }

        [TestMethod]
        public void ISC_ContainsServiceT_WithName_False()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService(typeof(string), "blub")).Returns(false).Verifiable(this, Times.Once());

            Assert.IsFalse(ServiceContextExtensions.ContainsService<string>(mock.Object, "blub"));
        }

        [TestMethod]
        public void ISC_ContainsService_True()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService(typeof(string), null)).Returns(true).Verifiable(this, Times.Once());

            Assert.IsTrue(ServiceContextExtensions.ContainsService(mock.Object, typeof(string)));
        }

        [TestMethod]
        public void ISC_ContainsService_false()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService(typeof(string), null)).Returns(false).Verifiable(this, Times.Once());

            Assert.IsFalse(ServiceContextExtensions.ContainsService(mock.Object, typeof(string)));
        }

        [TestMethod]
        public void ISC_RemoveService()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.RemoveService(typeof(string), null)).Verifiable(this, Times.Once());

            ServiceContextExtensions.RemoveService(mock.Object, typeof(string));
        }

        [TestMethod]
        public void ISC_RemoveServiceT()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.RemoveService(typeof(string), null)).Verifiable(this, Times.Once());

            ServiceContextExtensions.RemoveService<string>(mock.Object);
        }

        [TestMethod]
        public void ISC_RemoveServiceT_WithName()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.RemoveService(typeof(string), "blub")).Verifiable(this, Times.Once());

            ServiceContextExtensions.RemoveService<string>(mock.Object, "blub");
        }

        [TestMethod]
        public void TryRemoveServiceT_Found()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService(typeof(string), null)).Returns(true).Verifiable(this, Times.Once());
            mock.Setup(x => x.RemoveService(typeof(string), null)).Verifiable(this, Times.Once());

            ServiceContextExtensions.TryRemoveService<string>(mock.Object);
        }

        [TestMethod]
        public void TryRemoveServiceT_NotFound()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService(typeof(string), null)).Returns(false).Verifiable(this, Times.Once());

            ServiceContextExtensions.TryRemoveService<string>(mock.Object);
        }

        [TestMethod]
        public void TryRemoveServiceT_Found_WithName()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService(typeof(string), "blub")).Returns(true).Verifiable(this, Times.Once());
            mock.Setup(x => x.RemoveService(typeof(string), "blub")).Verifiable(this, Times.Once());

            ServiceContextExtensions.TryRemoveService<string>(mock.Object, "blub");
        }

        [TestMethod]
        public void TryRemoveServiceT_NotFound_WithName()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService(typeof(string), "blub")).Returns(false).Verifiable(this, Times.Once());

            ServiceContextExtensions.TryRemoveService<string>(mock.Object, "blub");
        }

        [TestMethod]
        public void TryRemoveService_Found()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService(typeof(string), null)).Returns(true).Verifiable(this, Times.Once());
            mock.Setup(x => x.RemoveService(typeof(string), null)).Verifiable(this, Times.Once());

            ServiceContextExtensions.TryRemoveService(mock.Object, typeof(string));
        }

        [TestMethod]
        public void TryRemoveService_NotFound()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService(typeof(string), null)).Returns(false).Verifiable(this, Times.Once());

            ServiceContextExtensions.TryRemoveService(mock.Object, typeof(string));
        }

        [TestMethod]
        public void TryRemoveService_Found_WithName()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService(typeof(string), "blub")).Returns(true).Verifiable(this, Times.Once());
            mock.Setup(x => x.RemoveService(typeof(string), "blub")).Verifiable(this, Times.Once());

            ServiceContextExtensions.TryRemoveService(mock.Object, typeof(string), "blub");
        }

        [TestMethod]
        public void TryRemoveService_NotFound_WithName()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService(typeof(string), "blub")).Returns(false).Verifiable(this, Times.Once());

            ServiceContextExtensions.TryRemoveService(mock.Object, typeof(string), "blub");
        }

        [TestMethod]
        public void ISC_Clear()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.Clear(null)).Verifiable(this, Times.Once());

            ServiceContextExtensions.Clear(mock.Object);
        }

        [TestMethod]
        public void ISC_ClearT()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            mock.Setup(x => x.Clear(typeof(string))).Verifiable(this, Times.Once());

            ServiceContextExtensions.Clear<string>(mock.Object);
        }

        #endregion

        #region IServiceContext<T>

        [TestMethod]
        public void ISCT_AddService()
        {
            var mock = new Mock<IServiceContext<string>>(MockBehavior.Strict);
            mock.Setup(x => x.AddService("MyTest", null)).Verifiable(this, Times.Once());

            ServiceContextExtensions.AddService(mock.Object, "MyTest");
        }

        [TestMethod]
        public void ISCT_GetService()
        {
            var mock = new Mock<IServiceContext<string>>(MockBehavior.Strict);
            mock.Setup(x => x.GetService(null)).Returns("MyTest").Verifiable(this, Times.Once());

            var result = ServiceContextExtensions.GetService(mock.Object);

            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void ISCT_GetServiceOut()
        {
            var mock = new Mock<IServiceContext<string>>(MockBehavior.Strict);
            mock.Setup(x => x.GetService(null)).Returns("MyTest").Verifiable(this, Times.Once());

            ServiceContextExtensions.GetService(mock.Object, out var result);

            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void ISCT_GetServiceOut_WithName()
        {
            var mock = new Mock<IServiceContext<string>>(MockBehavior.Strict);
            mock.Setup(x => x.GetService("blub")).Returns("MyTest").Verifiable(this, Times.Once());

            ServiceContextExtensions.GetService(mock.Object, out var result, "blub");

            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void ISCT_TryGetServiceOut_Found()
        {
            var mock = new Mock<IServiceContext<string>>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService(null)).Returns(true).Verifiable(this, Times.Once());
            mock.Setup(x => x.GetService(null)).Returns("MyTest").Verifiable(this, Times.Once());

            var success = ServiceContextExtensions.TryGetService(mock.Object, out var result);

            Assert.IsTrue(success);
            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void ISCT_TryGetServiceOut_NotFound()
        {
            var mock = new Mock<IServiceContext<string>>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService(null)).Returns(false).Verifiable(this, Times.Once());

            var success = ServiceContextExtensions.TryGetService(mock.Object, out var result);

            Assert.IsFalse(success);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ISCT_TryGetServiceOut_Found_WithName()
        {
            var mock = new Mock<IServiceContext<string>>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService("blub")).Returns(true).Verifiable(this, Times.Once());
            mock.Setup(x => x.GetService("blub")).Returns("MyTest").Verifiable(this, Times.Once());

            var success = ServiceContextExtensions.TryGetService(mock.Object, out var result, "blub");

            Assert.IsTrue(success);
            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void ISCT_TryGetServiceOut_NotFound_WithName()
        {
            var mock = new Mock<IServiceContext<string>>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService("blub")).Returns(false).Verifiable(this, Times.Once());

            var success = ServiceContextExtensions.TryGetService(mock.Object, out var result, "blub");

            Assert.IsFalse(success);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ISCT_TryGetService_Found()
        {
            var mock = new Mock<IServiceContext<string>>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService(null)).Returns(true).Verifiable(this, Times.Once());
            mock.Setup(x => x.GetService(null)).Returns("MyTest").Verifiable(this, Times.Once());

            var result = ServiceContextExtensions.TryGetService(mock.Object);

            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void ISCT_TryGetService_NotFound()
        {
            var mock = new Mock<IServiceContext<string>>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService(null)).Returns(false).Verifiable(this, Times.Once());

            var result = ServiceContextExtensions.TryGetService(mock.Object);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void ISCT_TryGetService_Found_WithName()
        {
            var mock = new Mock<IServiceContext<string>>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService("blub")).Returns(true).Verifiable(this, Times.Once());
            mock.Setup(x => x.GetService("blub")).Returns("MyTest").Verifiable(this, Times.Once());

            var result = ServiceContextExtensions.TryGetService(mock.Object, "blub");

            Assert.AreEqual("MyTest", result);
        }

        [TestMethod]
        public void ISCT_TryGetService_NotFound_WithName()
        {
            var mock = new Mock<IServiceContext<string>>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService("blub")).Returns(false).Verifiable(this, Times.Once());

            var result = ServiceContextExtensions.TryGetService(mock.Object, "blub");

            Assert.IsNull(result);
        }

        [TestMethod]
        public void ISCT_RemoveService()
        {
            var mock = new Mock<IServiceContext<string>>(MockBehavior.Strict);
            mock.Setup(x => x.RemoveService(null)).Verifiable(this, Times.Once());

            ServiceContextExtensions.RemoveService(mock.Object);
        }

        [TestMethod]
        public void ISCT_TryRemoveService_Found()
        {
            var mock = new Mock<IServiceContext<string>>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService(null)).Returns(true).Verifiable(this, Times.Once());
            mock.Setup(x => x.RemoveService(null)).Verifiable(this, Times.Once());

            ServiceContextExtensions.TryRemoveService(mock.Object);
        }

        [TestMethod]
        public void ISCT_TryRemoveService_NotFound()
        {
            var mock = new Mock<IServiceContext<string>>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService(null)).Returns(false).Verifiable(this, Times.Once());

            ServiceContextExtensions.TryRemoveService(mock.Object);
        }

        [TestMethod]
        public void ISCT_TryRemoveService_Found_WithName()
        {
            var mock = new Mock<IServiceContext<string>>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService("blub")).Returns(true).Verifiable(this, Times.Once());
            mock.Setup(x => x.RemoveService("blub")).Verifiable(this, Times.Once());

            ServiceContextExtensions.TryRemoveService(mock.Object, "blub");
        }

        [TestMethod]
        public void ISCT_TryRemoveService_NotFound_WithName()
        {
            var mock = new Mock<IServiceContext<string>>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService("blub")).Returns(false).Verifiable(this, Times.Once());

            ServiceContextExtensions.TryRemoveService(mock.Object, "blub");
        }

        [TestMethod]
        public void ISCT_ContainsService_True()
        {
            var mock = new Mock<IServiceContext<string>>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService(null)).Returns(true).Verifiable(this, Times.Once());

            Assert.IsTrue(ServiceContextExtensions.ContainsService(mock.Object));
        }

        [TestMethod]
        public void ISCT_ContainsService_False()
        {
            var mock = new Mock<IServiceContext<string>>(MockBehavior.Strict);
            mock.Setup(x => x.ContainsService(null)).Returns(false).Verifiable(this, Times.Once());

            Assert.IsFalse(ServiceContextExtensions.ContainsService(mock.Object));
        }

        #endregion
    }
}