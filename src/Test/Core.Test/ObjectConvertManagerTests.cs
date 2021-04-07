using MaSch.Core.Converters;
using MaSch.Core.Extensions;
using MaSch.Test.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace MaSch.Core.Test
{
    [TestClass]
    public class ObjectConvertManagerTests
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private ObjectConvertManager _manager;

        public TestContext TestContext { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [TestInitialize]
        public void InitializeTest()
        {
            if (!TestContext.TestName.StartsWith("Constructor"))
                _manager = new ObjectConvertManager();
        }

        [TestMethod]
        public void Constructor()
        {
            var manager = new ObjectConvertManager();

            Assert.IsNotNull(manager.ObjectConverters);
            Assert.AreEqual(0, manager.ObjectConverters.Count);
        }

        [TestMethod]
        public void CanConvert_SingleMatchingConvert()
        {
            var converter1Mock = CreateConverterCanConvertMock<string, int>(true);
            GetManagerConverterList().Add(converter1Mock.Object);

            var result = _manager.CanConvert(typeof(string), typeof(int));

            Assert.IsTrue(result);
            converter1Mock.Verify(x => x.CanConvert(typeof(string), typeof(int), _manager), Times.Once());
        }

        [TestMethod]
        public void CanConvert_MultipleConverters_OneMatching()
        {
            var converter1Mock = CreateConverterCanConvertMock<string, int>(false);
            var converter2Mock = CreateConverterCanConvertMock<string, int>(true);
            var converter3Mock = CreateConverterCanConvertMock<string, int>(false);
            GetManagerConverterList().Add(converter1Mock.Object, converter2Mock.Object, converter3Mock.Object);

            var result = _manager.CanConvert(typeof(string), typeof(int));

            Assert.IsTrue(result);
            converter1Mock.Verify(x => x.CanConvert(typeof(string), typeof(int), _manager), Times.Once());
            converter2Mock.Verify(x => x.CanConvert(typeof(string), typeof(int), _manager), Times.Once());
            converter3Mock.Verify(x => x.CanConvert(typeof(string), typeof(int), _manager), Times.Never());
        }

        [TestMethod]
        public void CanConvert_MultipleMatchingConverters()
        {
            var converter1Mock = CreateConverterCanConvertMock<string, int>(true);
            var converter2Mock = CreateConverterCanConvertMock<string, int>(true);
            GetManagerConverterList().Add(converter1Mock.Object, converter2Mock.Object);

            var result = _manager.CanConvert(typeof(string), typeof(int));

            Assert.IsTrue(result);
            converter1Mock.Verify(x => x.CanConvert(typeof(string), typeof(int), _manager), Times.Once());
            converter2Mock.Verify(x => x.CanConvert(typeof(string), typeof(int), _manager), Times.Never());
        }

        [TestMethod]
        public void CanConvert_SingleNonMatchingConverter()
        {
            var converter1Mock = CreateConverterCanConvertMock<string, int>(false);
            GetManagerConverterList().Add(converter1Mock.Object);

            var result = _manager.CanConvert(typeof(string), typeof(int));

            Assert.IsFalse(result);
            converter1Mock.Verify(x => x.CanConvert(typeof(string), typeof(int), _manager), Times.Once());
        }

        [TestMethod]
        public void CanConvert_MultipleNonMatchingConverter()
        {
            var converter1Mock = CreateConverterCanConvertMock<string, int>(false);
            var converter2Mock = CreateConverterCanConvertMock<string, int>(false);
            GetManagerConverterList().Add(converter1Mock.Object, converter2Mock.Object);

            var result = _manager.CanConvert(typeof(string), typeof(int));

            Assert.IsFalse(result);
            converter1Mock.Verify(x => x.CanConvert(typeof(string), typeof(int), _manager), Times.Once());
            converter2Mock.Verify(x => x.CanConvert(typeof(string), typeof(int), _manager), Times.Once());
        }

        [TestMethod]
        public void Convert_ParameterChecks_Null()
        {
            var formatProvider = new Mock<IFormatProvider>(MockBehavior.Strict);

            Assert.ThrowsException<ArgumentNullException>(() => _manager.Convert(new object(), typeof(object), null!, formatProvider.Object));
            Assert.ThrowsException<ArgumentNullException>(() => _manager.Convert(new object(), typeof(object), typeof(int), null!));
        }

        [TestMethod]
        public void Convert_ParameterChecks_NullObjectForNonNullableType()
        {
            var formatProvider = new Mock<IFormatProvider>(MockBehavior.Strict);

            Assert.ThrowsException<ArgumentException>(() => _manager.Convert(null, typeof(bool), typeof(int), formatProvider.Object));
        }

        [TestMethod]
        public void Convert_ParameterChecks_ObjectNotInstanceOfSourceType()
        {
            var formatProvider = new Mock<IFormatProvider>(MockBehavior.Strict);

            Assert.ThrowsException<ArgumentException>(() => _manager.Convert(new object(), typeof(string), typeof(int), formatProvider.Object));
        }

        [TestMethod]
        public void Convert_NullObject_NullSourceType_SingleConverter_CanNotConvert()
        {
            var formatProvider = new Mock<IFormatProvider>(MockBehavior.Strict);
            var converter = CreateConverterMock(null, typeof(int), false, 0, null);
            GetManagerConverterList().Add(converter.Object);

            Assert.ThrowsException<InvalidCastException>(() => _manager.Convert(null, null, typeof(int), formatProvider.Object));

            converter.Verify(x => x.CanConvert(null, typeof(int), _manager), Times.Once());
        }

        [TestMethod]
        public void Convert_WithObject_NullSourceType_SingleConverter_CanNotConvert()
        {
            var formatProvider = new Mock<IFormatProvider>(MockBehavior.Strict);
            var converter = CreateConverterMock(typeof(string), typeof(int), false, 0, null);
            GetManagerConverterList().Add(converter.Object);

            Assert.ThrowsException<InvalidCastException>(() => _manager.Convert("Test", null, typeof(int), formatProvider.Object));

            converter.Verify(x => x.CanConvert(typeof(string), typeof(int), _manager), Times.Once());
        }

        [TestMethod]
        public void Convert_NullObject_WithSourceType_SingleConvert_CanConvertFromNull()
        {
            var formatProvider = new Mock<IFormatProvider>(MockBehavior.Strict);
            var converter = CreateConverterMock(null, typeof(int), true, 0, 4711);
            converter.Setup(x => x.CanConvert(typeof(string), typeof(int), It.IsAny<IObjectConvertManager>())).Returns(false);
            GetManagerConverterList().Add(converter.Object);

            var result = _manager.Convert(null, typeof(string), typeof(int), formatProvider.Object);

            Assert.AreEqual(4711, result);
            converter.Verify(x => x.CanConvert(typeof(string), typeof(int), _manager), Times.Once());
            converter.Verify(x => x.CanConvert(null, typeof(int), _manager), Times.Once());
            converter.Verify(x => x.Convert(null, null, typeof(int), _manager, formatProvider.Object), Times.Once());
        }

        [TestMethod]
        public void Convert_NullObject_WithSourceType_SingleConverter_CanConvertType()
        {
            var formatProvider = new Mock<IFormatProvider>(MockBehavior.Strict);
            var converter = CreateConverterMock(typeof(string), typeof(int), true, 0, 4711);
            GetManagerConverterList().Add(converter.Object);

            var result = _manager.Convert(null, typeof(string), typeof(int), formatProvider.Object);

            Assert.AreEqual(4711, result);
            converter.Verify(x => x.CanConvert(typeof(string), typeof(int), _manager), Times.Once());
            converter.Verify(x => x.Convert(null, typeof(string), typeof(int), _manager, formatProvider.Object), Times.Once());
        }

        [TestMethod]
        public void Convert_SingleConverter_ExceptionOnConvert()
        {
            var guid = Guid.NewGuid().ToString();
            var formatProvider = new Mock<IFormatProvider>(MockBehavior.Strict);
            var converter = CreateConverterMock(typeof(string), typeof(int), true, 0, 4711);
            converter
                .Setup(x => x.Convert(It.IsAny<object?>(), typeof(string), typeof(int), It.IsAny<IObjectConvertManager>(), It.IsAny<IFormatProvider>()))
                .Returns<object?, Type, Type, IObjectConvertManager, IFormatProvider>((a, b, c, d, e) => throw new Exception(guid));
            GetManagerConverterList().Add(converter.Object);

            var ex = Assert.ThrowsException<InvalidCastException>(() => _manager.Convert("Test", typeof(string), typeof(int), formatProvider.Object));

            Assert.That.Contains("IObjectConverterProxy", ex.Message);
            Assert.That.Contains(guid, ex.Message);
        }

        [TestMethod]
        public void Convert_MultipleConverters_SamePriority()
        {
            var formatProvider = new Mock<IFormatProvider>(MockBehavior.Strict);
            var converter1 = CreateConverterMock(typeof(string), typeof(int), true, 0, 1337);
            var converter2 = CreateConverterMock(typeof(string), typeof(int), true, 0, 4711);
            GetManagerConverterList().Add(converter1.Object, converter2.Object);

            var result = _manager.Convert("Test", typeof(string), typeof(int), formatProvider.Object);

            Assert.AreEqual(1337, result);
            converter1.Verify(x => x.Convert("Test", typeof(string), typeof(int), _manager, formatProvider.Object), Times.Once());
            converter2.Verify(x => x.Convert("Test", typeof(string), typeof(int), _manager, formatProvider.Object), Times.Never());
        }

        [TestMethod]
        public void Convert_MultipleConverters_DifferentPriorities()
        {
            var formatProvider = new Mock<IFormatProvider>(MockBehavior.Strict);
            var converter1 = CreateConverterMock(typeof(string), typeof(int), true, 0, 1337);
            var converter2 = CreateConverterMock(typeof(string), typeof(int), true, 10, 4711);
            GetManagerConverterList().Add(converter1.Object, converter2.Object);

            var result = _manager.Convert("Test", typeof(string), typeof(int), formatProvider.Object);

            Assert.AreEqual(4711, result);
            converter2.Verify(x => x.Convert("Test", typeof(string), typeof(int), _manager, formatProvider.Object), Times.Once());
            converter1.Verify(x => x.Convert("Test", typeof(string), typeof(int), _manager, formatProvider.Object), Times.Never());
        }

        [TestMethod]
        public void Convert_MultipleConverters_OnlyOneCanConvert()
        {
            var formatProvider = new Mock<IFormatProvider>(MockBehavior.Strict);
            var converter1 = CreateConverterMock(typeof(string), typeof(int), false, 0, 1337);
            var converter2 = CreateConverterMock(typeof(string), typeof(int), true, 0, 4711);
            var converter3 = CreateConverterMock(typeof(string), typeof(int), false, 0, 2468);
            GetManagerConverterList().Add(converter1.Object, converter2.Object);

            var result = _manager.Convert("Test", typeof(string), typeof(int), formatProvider.Object);

            Assert.AreEqual(4711, result);
            converter2.Verify(x => x.Convert("Test", typeof(string), typeof(int), _manager, formatProvider.Object), Times.Once());
            converter1.Verify(x => x.Convert("Test", typeof(string), typeof(int), _manager, formatProvider.Object), Times.Never());
            converter3.Verify(x => x.Convert("Test", typeof(string), typeof(int), _manager, formatProvider.Object), Times.Never());
        }

        [TestMethod]
        public void Convert_MultipleConverters_ErrorInCanConvert()
        {
            var formatProvider = new Mock<IFormatProvider>(MockBehavior.Strict);
            var converter1 = CreateConverterMock(typeof(string), typeof(int), true, 0, 1337);
            var converter2 = CreateConverterMock(typeof(string), typeof(int), true, 0, 4711);
            converter1.Setup(x => x.CanConvert(typeof(string), typeof(int), It.IsAny<IObjectConvertManager>())).Throws(new Exception());
            GetManagerConverterList().Add(converter1.Object, converter2.Object);

            var result = _manager.Convert("Test", typeof(string), typeof(int), formatProvider.Object);

            Assert.AreEqual(4711, result);
            converter2.Verify(x => x.Convert("Test", typeof(string), typeof(int), _manager, formatProvider.Object), Times.Once());
            converter1.Verify(x => x.Convert("Test", typeof(string), typeof(int), _manager, formatProvider.Object), Times.Never());
        }

        [TestMethod]
        public void Convert_MultipleConverters_ErrorInGetPriority()
        {
            var formatProvider = new Mock<IFormatProvider>(MockBehavior.Strict);
            var converter1 = CreateConverterMock(typeof(string), typeof(int), true, 0, 1337);
            var converter2 = CreateConverterMock(typeof(string), typeof(int), true, int.MinValue + 1, 4711);
            converter1.Setup(x => x.GetPriority(typeof(string), typeof(int))).Throws(new Exception());
            GetManagerConverterList().Add(converter1.Object, converter2.Object);

            var result = _manager.Convert("Test", typeof(string), typeof(int), formatProvider.Object);

            Assert.AreEqual(4711, result);
            converter2.Verify(x => x.Convert("Test", typeof(string), typeof(int), _manager, formatProvider.Object), Times.Once());
            converter1.Verify(x => x.Convert("Test", typeof(string), typeof(int), _manager, formatProvider.Object), Times.Never());
        }

        [TestMethod]
        public void RegisterConverter_ParameterChecks()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _manager.RegisterConverter(null!));
        }

        [TestMethod]
        public void RegisterConverter()
        {
            var converterMock = new Mock<IObjectConverter>(MockBehavior.Strict);

            _manager.RegisterConverter(converterMock.Object);

            Assert.AreEqual(1, _manager.ObjectConverters.Count);
            Assert.AreSame(converterMock.Object, _manager.ObjectConverters.ElementAt(0));
        }

        private List<IObjectConverter> GetManagerConverterList()
        {
            var field = typeof(ObjectConvertManager).GetField("_objectConverters", BindingFlags.Instance | BindingFlags.NonPublic);
            return (List<IObjectConverter>)field!.GetValue(_manager)!;
        }

        private static Mock<IObjectConverter> CreateConverterCanConvertMock<TSource, TTarget>(bool result)
        {
            var mock = new Mock<IObjectConverter>(MockBehavior.Strict);
            mock.Setup(x => x.CanConvert(typeof(TSource), typeof(TTarget), It.IsAny<IObjectConvertManager>())).Returns(result);
            return mock;
        }

        private static Mock<IObjectConverter> CreateConverterMock(Type? source, Type target, bool canConvert, int priority, object? result)
        {
            var mock = new Mock<IObjectConverter>(MockBehavior.Strict);
            mock.Setup(x => x.CanConvert(source, target, It.IsAny<IObjectConvertManager>())).Returns(canConvert);
            if (canConvert)
            {
                mock.Setup(x => x.GetPriority(source, target)).Returns(priority);
                mock.Setup(x => x.Convert(It.IsAny<object?>(), source, target, It.IsAny<IObjectConvertManager>(), It.IsAny<IFormatProvider>())).Returns(result);
            }

            return mock;
        }
    }

    [TestClass]
    public class DefaultObjectConvertManagerTests
    {
        [TestMethod]
        public void Constructor()
        {
            var manager = new DefaultObjectConvertManager();

            Assert.AreEqual(7, manager.ObjectConverters.Count);
            Assert.IsNotNull(manager.ObjectConverters.OfType<NullableObjectConverter>().SingleOrDefault());
            Assert.IsNotNull(manager.ObjectConverters.OfType<ConvertibleObjectConverter>().SingleOrDefault());
            Assert.IsNotNull(manager.ObjectConverters.OfType<EnumConverter>().SingleOrDefault());
            Assert.IsNotNull(manager.ObjectConverters.OfType<EnumerableConverter>().SingleOrDefault());
            Assert.IsNotNull(manager.ObjectConverters.OfType<ToStringObjectConverter>().SingleOrDefault());
            Assert.IsNotNull(manager.ObjectConverters.OfType<NullObjectConverter>().SingleOrDefault());
            Assert.IsNotNull(manager.ObjectConverters.OfType<IdentityObjectConverter>().SingleOrDefault());
        }
    }

    [TestClass]
    public class ObjectConvertManagerExtensionsTests
    {
        [TestMethod]
        public void CanConvertT2()
        {
            var managerMock = new Mock<IObjectConvertManager>(MockBehavior.Strict);
            managerMock.Setup(x => x.CanConvert(It.IsAny<Type>(), It.IsAny<Type>())).Returns(true);

            var result = ObjectConvertManagerExtensions.CanConvert<string, int>(managerMock.Object);

            Assert.IsTrue(result);
            managerMock.Verify(x => x.CanConvert(typeof(string), typeof(int)), Times.Once());
        }
    }
}
