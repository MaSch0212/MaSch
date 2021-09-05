using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Assert = MaSch.Test.Assertion.Assert;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace MaSch.Core.UnitTests
{
    [TestClass]
    public sealed class CacheTests
    {
        private Cache _cache;

        public TestContext TestContext { get; set; }
        public Assert Assert => Assert.Instance;

        [TestInitialize]
        public void InitializeTest()
        {
            _cache = new Cache();
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _cache?.Dispose();
        }

        [TestMethod]
        public void GetValue_ParameterValidation()
        {
            _ = Assert.ThrowsException<ArgumentNullException>(() => _cache.GetValue<object>(null!));
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
            _ = Assert.ThrowsException<KeyNotFoundException>(() => _cache.GetValue<string>("MyKey"));
        }

        [TestMethod]
        public void GetValue_WrongType()
        {
            GetCacheDict().Add("MyKey", "Test123");
            _ = Assert.ThrowsException<InvalidCastException>(() => _cache.GetValue<int>("MyKey"));
        }

        [TestMethod]
        public void GetValue_WithFactory_ParameterValidation()
        {
            _ = Assert.ThrowsException<ArgumentNullException>(() => _cache.GetValue<object>(null!, "MyTest"));
            _ = Assert.ThrowsException<ArgumentNullException>(() => _cache.GetValue(() => new object(), null!));
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
            _ = factoryMock.Setup(x => x()).Returns("Test123");

            var result = _cache.GetValue(factoryMock.Object, "MyKey");

            Assert.AreEqual("Test123", result);
            factoryMock.Verify(x => x(), Times.Once());

            Assert.AreEqual("Test123", _cache.GetValue<string>("MyKey"));
        }

        [TestMethod]
        public void GetValue_WithFactory_WithKey_ExistingValue()
        {
            var factoryMock = new Mock<Func<string>>();
            _ = factoryMock.Setup(x => x()).Returns("blub");

            GetCacheDict().Add("MyKey", "Test123");

            var result = _cache.GetValue(factoryMock.Object, "MyKey");

            Assert.AreEqual("Test123", result);
            factoryMock.Verify(x => x(), Times.Never());
        }

        [TestMethod]
        public void GetValue_WithFactory_WithKey_ExistingValue_WrongType()
        {
            GetCacheDict().Add("MyKey", "Test123");
            _ = Assert.ThrowsException<InvalidCastException>(() => _cache.GetValue(() => 123, "MyKey"));
        }

        [TestMethod]
        public async Task GetValueAsync_WithFactory_ParameterValidation()
        {
            _ = await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _cache.GetValueAsync<object>(null!, "MyTest"));
            _ = await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _cache.GetValueAsync(() => Task.FromResult(new object()), null!));
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
            _ = factoryMock.Setup(x => x()).Returns(Task.FromResult("Test123"));

            var result = await _cache.GetValueAsync(factoryMock.Object, "MyKey");

            Assert.AreEqual("Test123", result);
            factoryMock.Verify(x => x(), Times.Once());

            Assert.AreEqual("Test123", _cache.GetValue<string>("MyKey"));
        }

        [TestMethod]
        public async Task GetValueAsync_WithFactory_WithKey_ExistingValue()
        {
            var factoryMock = new Mock<Func<Task<string>>>();
            _ = factoryMock.Setup(x => x()).Returns(Task.FromResult("blub"));

            GetCacheDict().Add("MyKey", "Test123");

            var result = await _cache.GetValueAsync(factoryMock.Object, "MyKey");

            Assert.AreEqual("Test123", result);
            factoryMock.Verify(x => x(), Times.Never());
        }

        [TestMethod]
        public async Task GetValueAsync_WithFactory_WithKey_ExistingValue_WrongType()
        {
            GetCacheDict().Add("MyKey", "Test123");
            _ = await Assert.ThrowsExceptionAsync<InvalidCastException>(() => _cache.GetValueAsync(() => Task.FromResult(123), "MyKey"));
        }

        [TestMethod]
        public void TryGetValue_ParameterChecks()
        {
            _ = Assert.ThrowsException<ArgumentNullException>(() => _cache.TryGetValue<object>(out _, null!));
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
            _ = Assert.ThrowsException<ArgumentNullException>(() => _cache.HasValue(null!));
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
            _ = Assert.ThrowsException<ArgumentNullException>(() => _cache.RemoveValue(null!));
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
            _ = Assert.ThrowsException<ArgumentNullException>(() => _cache.RemoveAndDisposeValue(null!));
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
            _ = Assert.ThrowsException<ArgumentNullException>(() => _cache.SetValue(new object(), null!));
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
        public void Dispose_()
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

        private IDictionary<string, object?> GetCacheDict()
        {
            var prop = typeof(Cache).GetProperty("Objects", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            return (IDictionary<string, object?>)prop!.GetValue(_cache)!;
        }
    }
}
