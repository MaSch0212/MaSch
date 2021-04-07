using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MaSch.Core.Test
{
    [TestClass]
    public class ServiceContextTests
    {

        internal static IDictionary<(Type Type, string? Name), object> GetServicesDict(object? instance)
        {
            var servicesField = typeof(ServiceContext).GetField("_services", BindingFlags.Instance | BindingFlags.NonPublic);
            return (IDictionary<(Type, string?), object>)servicesField!.GetValue(instance)!;
        }

        internal static IDictionary<Type, IDisposable> GetViewsDict(object? instance)
        {
            var servicesField = typeof(ServiceContext).GetField("_views", BindingFlags.Instance | BindingFlags.NonPublic);
            return (IDictionary<Type, IDisposable>)servicesField!.GetValue(instance)!;
        }
    }

    [TestClass]
    public class ServiceContextStaticTests
    {
        private static readonly FieldInfo InstanceField = typeof(ServiceContext).GetField("_instance", BindingFlags.Static | BindingFlags.NonPublic)!;

        [TestInitialize]
        public void InitializeTest()
        {
            InstanceField.SetValue(null, null);
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
            var context = ServiceContext.CreateContext();

            Assert.IsNotNull(context);
            Assert.AreEqual(0, ServiceContextTests.GetServicesDict(context).Count);
            Assert.IsNull(InstanceField.GetValue(null));
        }

        [TestMethod]
        public void GetAllServices()
        {
            var mock = SetMockInstance();
            var dict = new Mock<IReadOnlyDictionary<(Type, string?), object>>();
            mock.Setup(x => x.GetAllServices()).Returns(dict.Object);

            var result = ServiceContext.GetAllServices();

            Assert.AreSame(dict.Object, result);
            mock.Verify(x => x.GetAllServices(), Times.Once());
        }

        [TestMethod]
        public void GetAllServicesT()
        {
            var mock = SetMockInstance();
            mock.Setup(x => x.GetAllServices(typeof(string))).Returns(new (string?, object)[] { ("blub", "Test123"), ("zzz", "bar") });

            var result = ServiceContext.GetAllServices<string>();

            Assert.IsTrue(result.SequenceEqual(new (string?, string)[] { ("blub", "Test123"), ("zzz", "bar") }));
            mock.Verify(x => x.GetAllServices(typeof(string)), Times.Once());
        }

        [TestMethod]
        public void GetAllServices_WithServiceType()
        {
            var mock = SetMockInstance();
            var list = new Mock<IEnumerable<(string?, object)>>();
            mock.Setup(x => x.GetAllServices(typeof(string))).Returns(list.Object);

            var result = ServiceContext.GetAllServices(typeof(string));

            Assert.AreSame(list.Object, result);
            mock.Verify(x => x.GetAllServices(typeof(string)), Times.Once());
        }

        [TestMethod]
        public void AddServiceT()
        {
            var mock = SetMockInstance();
            mock.Setup(x => x.AddService(It.IsAny<Type>(), It.IsAny<object>(), It.IsAny<string?>()));

            ServiceContext.AddService("MyTest");

            mock.Verify(x => x.AddService(typeof(string), "MyTest", null), Times.Once());
        }

        [TestMethod]
        public void AddServiceT_WithName()
        {
            var mock = SetMockInstance();
            mock.Setup(x => x.AddService(It.IsAny<Type>(), It.IsAny<object>(), It.IsAny<string?>()));

            ServiceContext.AddService("MyTest", "blub");

            mock.Verify(x => x.AddService(typeof(string), "MyTest", "blub"), Times.Once());
        }

        [TestMethod]
        public void AddService()
        {
            var mock = SetMockInstance();
            mock.Setup(x => x.AddService(It.IsAny<Type>(), It.IsAny<object>(), It.IsAny<string?>()));

            ServiceContext.AddService(typeof(string), (object)"MyTest");

            mock.Verify(x => x.AddService(typeof(string), "MyTest", null), Times.Once());
        }

        [TestMethod]
        public void AddService_WithName()
        {
            var mock = SetMockInstance();
            mock.Setup(x => x.AddService(It.IsAny<Type>(), It.IsAny<object>(), It.IsAny<string?>()));

            ServiceContext.AddService(typeof(string), "MyTest", "blub");

            mock.Verify(x => x.AddService(typeof(string), "MyTest", "blub"), Times.Once());
        }

        private static Mock<IServiceContext> SetMockInstance()
        {
            var mock = new Mock<IServiceContext>(MockBehavior.Strict);
            InstanceField.SetValue(null, mock.Object);
            return mock;
        }
    }
}
