using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Assert = MaSch.Test.Assertion.Assert;

namespace MaSch.Core.Test
{
    [TestClass]
    public class CacheTests
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private Cache _cache;
        private Mock<Cache> _cacheMock;

        public TestContext TestContext { get; set; }
        public Assert Assert => Assert.Instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [TestInitialize]
        public void InitializeTest()
        {
            if (TestContext.TestName.StartsWith("Static_"))
            {
                _cache = null!;
                _cacheMock = new Mock<Cache>(MockBehavior.Strict);
                Cache<string>.Instance = _cacheMock.Object;
            }
            else
            {
                _cacheMock = null!;
                _cache = new Cache();
            }
        }

        [TestMethod]
        public void GetValue_ParameterValidation()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _cache.GetValue<object>(null!));
        }

        [TestMethod]
        public void GetValue_NoKey()
        {
            GetCacheDict().Add(nameof(GetValue_NoKey), "Test123");

            var result = _cache.GetValue<string>();

            Assert.AreEqual("Test123", result);
        }

        [TestMethod]
        public void GetValue_WithKey()
        {
            GetCacheDict().Add("MyKey", "Test123");

            var result = _cache.GetValue<string>("MyKey");

            Assert.AreEqual("Test123", result);
        }

        [TestMethod]
        public void GetValue_MissingValue()
        {
            Assert.ThrowsException<KeyNotFoundException>(() => _cache.GetValue<string>("MyKey"));
        }

        [TestMethod]
        public void GetValue_WrongType()
        {
            GetCacheDict().Add("MyKey", "Test123");
            Assert.ThrowsException<InvalidCastException>(() => _cache.GetValue<int>("MyKey"));
        }

        [TestMethod]
        public void GetValue_WithFactory_ParameterValidation()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _cache.GetValue<object>(null!, "MyTest"));
            Assert.ThrowsException<ArgumentNullException>(() => _cache.GetValue(() => new object(), null!));
        }

        [TestMethod]
        public void GetValue_WithFactory_NoKey()
        {
            var result = _cache.GetValue(() => "Test123");

            Assert.AreEqual("Test123", result);
        }

        [TestMethod]
        public void GetValue_WithFactory_WithKey_MissingValue()
        {
            var factoryMock = new Mock<Func<string>>();
            factoryMock.Setup(x => x()).Returns("Test123");

            var result = _cache.GetValue(factoryMock.Object, "MyKey");

            Assert.AreEqual("Test123", result);
            factoryMock.Verify(x => x(), Times.Once());

            Assert.AreEqual("Test123", _cache.GetValue<string>("MyKey"));
        }

        [TestMethod]
        public void GetValue_WithFactory_WithKey_ExistingValue()
        {
            var factoryMock = new Mock<Func<string>>();
            factoryMock.Setup(x => x()).Returns("blub");

            GetCacheDict().Add("MyKey", "Test123");

            var result = _cache.GetValue(factoryMock.Object, "MyKey");

            Assert.AreEqual("Test123", result);
            factoryMock.Verify(x => x(), Times.Never());
        }

        [TestMethod]
        public void GetValue_WithFactory_WithKey_ExistingValue_WrongType()
        {
            GetCacheDict().Add("MyKey", "Test123");
            Assert.ThrowsException<InvalidCastException>(() => _cache.GetValue(() => 123, "MyKey"));
        }

        [TestMethod]
        public async Task GetValueAsync_WithFactory_ParameterValidation()
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _cache.GetValueAsync<object>(null!, "MyTest"));
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _cache.GetValueAsync(() => Task.FromResult(new object()), null!));
        }

        [TestMethod]
        public async Task GetValueAsync_WithFactory_NoKey()
        {
            var result = await _cache.GetValueAsync(() => Task.FromResult("Test123"));

            Assert.AreEqual("Test123", result);
        }

        [TestMethod]
        public async Task GetValueAsync_WithFactory_WithKey_MissingValue()
        {
            var factoryMock = new Mock<Func<Task<string>>>();
            factoryMock.Setup(x => x()).Returns(Task.FromResult("Test123"));

            var result = await _cache.GetValueAsync(factoryMock.Object, "MyKey");

            Assert.AreEqual("Test123", result);
            factoryMock.Verify(x => x(), Times.Once());

            Assert.AreEqual("Test123", _cache.GetValue<string>("MyKey"));
        }

        [TestMethod]
        public async Task GetValueAsync_WithFactory_WithKey_ExistingValue()
        {
            var factoryMock = new Mock<Func<Task<string>>>();
            factoryMock.Setup(x => x()).Returns(Task.FromResult("blub"));

            GetCacheDict().Add("MyKey", "Test123");

            var result = await _cache.GetValueAsync(factoryMock.Object, "MyKey");

            Assert.AreEqual("Test123", result);
            factoryMock.Verify(x => x(), Times.Never());
        }

        [TestMethod]
        public async Task GetValueAsync_WithFactory_WithKey_ExistingValue_WrongType()
        {
            GetCacheDict().Add("MyKey", "Test123");
            await Assert.ThrowsExceptionAsync<InvalidCastException>(() => _cache.GetValueAsync(() => Task.FromResult(123), "MyKey"));
        }

        [TestMethod]
        public void TryGetValue_ParameterChecks()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _cache.TryGetValue<object>(out _, null!));
        }

        [TestMethod]
        public void TryGetValue_NoKey()
        {
            GetCacheDict().Add(nameof(TryGetValue_NoKey), "Test123");

            var found = _cache.TryGetValue<string>(out var result);

            Assert.IsTrue(found);
            Assert.AreEqual("Test123", result);
        }

        [TestMethod]
        public void TryGetValue_WithKey_ExistingValue()
        {
            GetCacheDict().Add("MyKey", "Test123");

            var found = _cache.TryGetValue<string>(out var result, "MyKey");

            Assert.IsTrue(found);
            Assert.AreEqual("Test123", result);
        }

        [TestMethod]
        public void TryGetValue_WithKey_MissingValue()
        {
            var found = _cache.TryGetValue<string>(out var result, "MyKey");

            Assert.IsFalse(found);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TryGetValue_WithKey_WrongType()
        {
            GetCacheDict().Add("MyKey", 123);

            var found = _cache.TryGetValue<string>(out var result, "MyKey");

            Assert.IsFalse(found);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void HasValue_ParameterChecks()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _cache.HasValue(null!));
        }

        [TestMethod]
        public void HasValue_WithKey_False()
        {
            var result = _cache.HasValue("MyKey");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HasValue_NoKey_True()
        {
            GetCacheDict().Add(nameof(HasValue_NoKey_True), "Test123");

            var result = _cache.HasValue();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HasValue_WithKey_True()
        {
            GetCacheDict().Add("MyKey", "Test123");

            var result = _cache.HasValue("MyKey");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void RemoveValue_ParameterChecks()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _cache.RemoveValue(null!));
        }

        [TestMethod]
        public void RemoveValue_NoKey_ExistingValue()
        {
            var dict = GetCacheDict();
            dict.Add(nameof(RemoveValue_NoKey_ExistingValue), "Test123");

            _cache.RemoveValue();

            Assert.IsFalse(dict.ContainsKey(nameof(RemoveValue_NoKey_ExistingValue)));
        }

        [TestMethod]
        public void RemoveValue_WithKey_ExistingValue()
        {
            var dict = GetCacheDict();
            dict.Add("MyKey", "Test123");

            _cache.RemoveValue("MyKey");

            Assert.IsFalse(dict.ContainsKey("MyKey"));
        }

        [TestMethod]
        public void RemoveValue_WithKey_MissingValue()
        {
            // Expected to do nothing if value does not exist.
            _cache.RemoveValue("MyKey");
        }

        [TestMethod]
        public void RemoveAndDisposeValue_ParameterChecks()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _cache.RemoveAndDisposeValue(null!));
        }

        [TestMethod]
        public void RemoveAndDisposeValue_NoKey_ExistingValue_Disposable()
        {
            var disposableMock = new Mock<IDisposable>();
            var dict = GetCacheDict();
            dict.Add(nameof(RemoveAndDisposeValue_NoKey_ExistingValue_Disposable), disposableMock.Object);

            _cache.RemoveAndDisposeValue();

            Assert.IsFalse(dict.ContainsKey(nameof(RemoveAndDisposeValue_NoKey_ExistingValue_Disposable)));
            disposableMock.Verify(x => x.Dispose(), Times.Once());
        }

        [TestMethod]
        public void RemoveAndDisposeValue_WithKey_ExistingValue_Disposable()
        {
            var disposableMock = new Mock<IDisposable>();
            var dict = GetCacheDict();
            dict.Add("MyKey", disposableMock.Object);

            _cache.RemoveAndDisposeValue("MyKey");

            Assert.IsFalse(dict.ContainsKey("MyKey"));
            disposableMock.Verify(x => x.Dispose(), Times.Once());
        }

        [TestMethod]
        public void RemoveAndDisposeValue_WithKey_ExistingValue_Null()
        {
            var dict = GetCacheDict();
            dict.Add("MyKey", null);

            // Expected to not do anything with the null value.
            _cache.RemoveAndDisposeValue("MyKey");

            Assert.IsFalse(dict.ContainsKey("MyKey"));
        }

        [TestMethod]
        public void RemoveAndDisposeValue_WithKey_ExistingValue_NonDisposable()
        {
            var dict = GetCacheDict();
            dict.Add("MyKey", DateTime.Now);

            // Expected to not do anything with the non-disposable value.
            _cache.RemoveAndDisposeValue("MyKey");

            Assert.IsFalse(dict.ContainsKey("MyKey"));
        }

        [TestMethod]
        public void RemoveAndDisposeValue_WithKey_MissingValue()
        {
            // Expected to do nothing if value does not exist.
            _cache.RemoveAndDisposeValue("MyKey");
        }

        [TestMethod]
        public void SetValue_ParameterChecks()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _cache.SetValue(new object(), null!));
        }

        [TestMethod]
        public void SetValue_NoKey()
        {
            _cache.SetValue("Test123");

            Assert.AreEqual("Test123", GetCacheDict()[nameof(SetValue_NoKey)]);
        }

        [TestMethod]
        public void SetValue_WithKey()
        {
            _cache.SetValue("Test123", "MyKey");

            Assert.AreEqual("Test123", GetCacheDict()["MyKey"]);
        }

        [TestMethod]
        public void Clear()
        {
            var dict = GetCacheDict();
            dict.Add("MyKey1", "blub");
            dict.Add("MyKey2", "blub");

            _cache.Clear();

            Assert.AreEqual(0, dict.Count);
        }

        [TestMethod]
        public void ClearAndDispose()
        {
            var disposableMock1 = new Mock<IDisposable>();
            var disposableMock2 = new Mock<IDisposable>();

            var dict = GetCacheDict();
            dict.Add("MyKey1", disposableMock1.Object);
            dict.Add("MyKey2", "blub");
            dict.Add("MyKey3", null);
            dict.Add("MyKey4", disposableMock2.Object);

            _cache.ClearAndDispose();

            Assert.AreEqual(0, dict.Count);
            disposableMock1.Verify(x => x.Dispose(), Times.Once());
            disposableMock2.Verify(x => x.Dispose(), Times.Once());
        }

        [TestMethod]
        public void Dispose()
        {
            var disposableMock1 = new Mock<IDisposable>();
            var disposableMock2 = new Mock<IDisposable>();

            var dict = GetCacheDict();
            dict.Add("MyKey1", disposableMock1.Object);
            dict.Add("MyKey2", "blub");
            dict.Add("MyKey3", null);
            dict.Add("MyKey4", disposableMock2.Object);

            _cache.Dispose();

            Assert.AreEqual(0, dict.Count);
            disposableMock1.Verify(x => x.Dispose(), Times.Once());
            disposableMock2.Verify(x => x.Dispose(), Times.Once());
        }

        [TestMethod]
        public void Static_GetValue_NoKey()
        {
            _cacheMock.Setup(x => x.GetValue<string>(It.IsAny<string>())).Returns("Test123");

            var result = Cache<string>.GetValue();

            Assert.AreEqual("Test123", result);
            _cacheMock.Verify(x => x.GetValue<string>(nameof(Static_GetValue_NoKey)), Times.Once());
        }

        [TestMethod]
        public void Static_GetValue_WithKey()
        {
            _cacheMock.Setup(x => x.GetValue<string>(It.IsAny<string>())).Returns("Test123");

            var result = Cache<string>.GetValue("MyKey");

            Assert.AreEqual("Test123", result);
            _cacheMock.Verify(x => x.GetValue<string>("MyKey"), Times.Once());
        }

        [TestMethod]
        public void Static_GetValue_WithFactory_NoKey()
        {
            _cacheMock.Setup(x => x.GetValue(It.IsAny<Func<string>>(), It.IsAny<string>())).Returns("Test123");
            var factory = new Func<string>(() => "Test123");

            var result = Cache<string>.GetValue(factory);

            Assert.AreEqual("Test123", result);
            _cacheMock.Verify(x => x.GetValue(factory, nameof(Static_GetValue_WithFactory_NoKey)), Times.Once());
        }

        [TestMethod]
        public void Static_GetValue_WithFactory_WithKey()
        {
            _cacheMock.Setup(x => x.GetValue(It.IsAny<Func<string>>(), It.IsAny<string>())).Returns("Test123");
            var factory = new Func<string>(() => "Test123");

            var result = Cache<string>.GetValue(factory, "MyKey");

            Assert.AreEqual("Test123", result);
            _cacheMock.Verify(x => x.GetValue(factory, "MyKey"), Times.Once());
        }

        [TestMethod]
        public async Task Static_GetValueAsync_WithFactory_NoKey()
        {
            _cacheMock.Setup(x => x.GetValueAsync(It.IsAny<Func<Task<string>>>(), It.IsAny<string>())).Returns(Task.FromResult((string?)"Test123"));
            var factory = new Func<Task<string>>(() => Task.FromResult("Test123"));

            var result = await Cache<string>.GetValueAsync(factory);

            Assert.AreEqual("Test123", result);
            _cacheMock.Verify(x => x.GetValueAsync(factory, nameof(Static_GetValueAsync_WithFactory_NoKey)), Times.Once());
        }

        [TestMethod]
        public async Task Static_GetValueAsync_WithFactory_WithKey()
        {
            _cacheMock.Setup(x => x.GetValueAsync(It.IsAny<Func<Task<string>>>(), It.IsAny<string>())).Returns(Task.FromResult((string?)"Test123"));
            var factory = new Func<Task<string>>(() => Task.FromResult("Test123"));

            var result = await Cache<string>.GetValueAsync(factory, "MyKey");

            Assert.AreEqual("Test123", result);
            _cacheMock.Verify(x => x.GetValueAsync(factory, "MyKey"), Times.Once());
        }

        [TestMethod]
        public void Static_TryGetValue_NoKey()
        {
            string? expectedValue = "Test123";
            _cacheMock.Setup(x => x.TryGetValue(out expectedValue, It.IsAny<string>())).Returns(true);

            var result = Cache<string>.TryGetValue(out var value);

            Assert.IsTrue(result);
            Assert.AreEqual("Test123", value);
            _cacheMock.Verify(x => x.TryGetValue(out expectedValue, nameof(Static_TryGetValue_NoKey)), Times.Once());
        }

        [TestMethod]
        public void Static_TryGetValue_WithKey()
        {
            string? expectedValue = "Test123";
            _cacheMock.Setup(x => x.TryGetValue(out expectedValue, It.IsAny<string>())).Returns(true);

            var result = Cache<string>.TryGetValue(out var value, "MyKey");

            Assert.IsTrue(result);
            Assert.AreEqual("Test123", value);
            _cacheMock.Verify(x => x.TryGetValue(out expectedValue, "MyKey"), Times.Once());
        }

        [TestMethod]
        public void Static_HasValue_NoKey()
        {
            _cacheMock.Setup(x => x.HasValue(It.IsAny<string>())).Returns(true);

            var result = Cache<string>.HasValue();

            Assert.IsTrue(result);
            _cacheMock.Verify(x => x.HasValue(nameof(Static_HasValue_NoKey)), Times.Once());
        }

        [TestMethod]
        public void Static_HasValue_WithKey()
        {
            _cacheMock.Setup(x => x.HasValue(It.IsAny<string>())).Returns(true);

            var result = Cache<string>.HasValue("MyKey");

            Assert.IsTrue(result);
            _cacheMock.Verify(x => x.HasValue("MyKey"), Times.Once());
        }

        [TestMethod]
        public void Static_RemoveValue_NoKey()
        {
            _cacheMock.Setup(x => x.RemoveValue(It.IsAny<string>()));

            Cache<string>.RemoveValue();

            _cacheMock.Verify(x => x.RemoveValue(nameof(Static_RemoveValue_NoKey)), Times.Once());
        }

        [TestMethod]
        public void Static_RemoveValue_WithKey()
        {
            _cacheMock.Setup(x => x.RemoveValue(It.IsAny<string>()));

            Cache<string>.RemoveValue("MyKey");

            _cacheMock.Verify(x => x.RemoveValue("MyKey"), Times.Once());
        }

        [TestMethod]
        public void Static_RemoveAndDisposeValue_NoKey()
        {
            _cacheMock.Setup(x => x.RemoveAndDisposeValue(It.IsAny<string>()));

            Cache<string>.RemoveAndDisposeValue();

            _cacheMock.Verify(x => x.RemoveAndDisposeValue(nameof(Static_RemoveAndDisposeValue_NoKey)), Times.Once());
        }

        [TestMethod]
        public void Static_RemoveAndDisposeValue_WithKey()
        {
            _cacheMock.Setup(x => x.RemoveAndDisposeValue(It.IsAny<string>()));

            Cache<string>.RemoveAndDisposeValue("MyKey");

            _cacheMock.Verify(x => x.RemoveAndDisposeValue("MyKey"), Times.Once());
        }

        [TestMethod]
        public void Static_SetValue_NoKey()
        {
            _cacheMock.Setup(x => x.SetValue(It.IsAny<string>(), It.IsAny<string>()));

            Cache<string>.SetValue("Test123");

            _cacheMock.Verify(x => x.SetValue("Test123", nameof(Static_SetValue_NoKey)), Times.Once());
        }

        [TestMethod]
        public void Static_SetValue_WithKey()
        {
            _cacheMock.Setup(x => x.SetValue(It.IsAny<string>(), It.IsAny<string>()));

            Cache<string>.SetValue("Test123", "MyKey");

            _cacheMock.Verify(x => x.SetValue("Test123", "MyKey"), Times.Once());
        }

        [TestMethod]
        public void Static_Clear()
        {
            _cacheMock.Setup(x => x.Clear());

            Cache<string>.Clear();

            _cacheMock.Verify(x => x.Clear(), Times.Once());
        }

        [TestMethod]
        public void Static_ClearAndDispose()
        {
            _cacheMock.Setup(x => x.ClearAndDispose());

            Cache<string>.ClearAndDispose();

            _cacheMock.Verify(x => x.ClearAndDispose(), Times.Once());
        }

        [TestMethod]
        public void Static_Dispose()
        {
            _cacheMock.Protected().Setup("Dispose", true, true);

            Cache<string>.Dispose();

            Assert.AreNotSame(_cacheMock.Object, Cache<string>.Instance);
            _cacheMock.Protected().Verify("Dispose", Times.Once(), true, true);
        }

        private IDictionary<string, object?> GetCacheDict()
        {
            var prop = typeof(Cache).GetProperty("Objects", BindingFlags.Instance | BindingFlags.NonPublic);
            return (IDictionary<string, object?>)prop!.GetValue(_cache)!;
        }
    }
}
