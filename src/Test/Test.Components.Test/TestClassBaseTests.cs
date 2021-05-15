using MaSch.Core;
using MaSch.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;

namespace MaSch.Test.Components.Test
{
    [TestClass]
    public class TestClassBaseTests
    {
        [TestMethod]
        public void InitializeTest_()
        {
            var mock = new Mock<TestClassBase>(MockBehavior.Strict);
            using var v1 = mock.Protected().Setup("OnInitializeTest").Verifiable(Times.Once());

            mock.Object.InitializeTest();
        }

        [TestMethod]
        public void CleanupTest_()
        {
            var mock = new Mock<TestClassBase>(MockBehavior.Strict);
            mock.Protected().SetupGet<bool>("CleanupCacheAfterTest").Returns(false);
            mock.SetupGet(x => x.Verifiables).Returns(new MockVerifiableCollection());
            using var v1 = mock.Protected().Setup("OnCleanupTest").Verifiable(Times.Once());

            mock.Object.CleanupTest();
        }

        [TestMethod]
        public void CleanupTest_Cache_Clear()
        {
            var cache = new Cache();
            cache.SetValue("blub", "test");
            var mock = new Mock<TestClassBase>(MockBehavior.Strict);
            mock.Protected().SetupGet<bool>("CleanupCacheAfterTest").Returns(true);
            mock.Protected().SetupGet<Cache>("Cache").Returns(cache);
            mock.SetupGet(x => x.Verifiables).Returns(new MockVerifiableCollection());
            using var v1 = mock.Protected()
                .Setup("OnCleanupTest")
                .Callback(() =>
                {
                    Assert.IsTrue(cache.HasValue("test"));
                    Assert.AreEqual("blub", cache.GetValue<string>("test"));
                })
                .Verifiable(Times.Once());

            mock.Object.CleanupTest();

            Assert.IsFalse(cache.HasValue("test"));
        }

        [TestMethod]
        public void CleanupTest_Cache_NoClear()
        {
            var cache = new Cache();
            cache.SetValue("blub", "test");
            var mock = new Mock<TestClassBase>(MockBehavior.Strict);
            mock.Protected().SetupGet<bool>("CleanupCacheAfterTest").Returns(false);
            mock.Protected().SetupGet<Cache>("Cache").Returns(cache);
            mock.SetupGet(x => x.Verifiables).Returns(new MockVerifiableCollection());
            using var v1 = mock.Protected()
                .Setup("OnCleanupTest")
                .Callback(() =>
                {
                    Assert.IsTrue(cache.HasValue("test"));
                    Assert.AreEqual("blub", cache.GetValue<string>("test"));
                })
                .Verifiable(Times.Once());

            mock.Object.CleanupTest();

            Assert.IsTrue(cache.HasValue("test"));
            Assert.AreEqual("blub", cache.GetValue<string>("test"));
        }

        [TestMethod]
        public void CleanupTest_VerifyVerifiables()
        {
            var verifiableMock = new Mock<IMockVerifiable>(MockBehavior.Strict);
            using var v2 = verifiableMock.Setup(x => x.Verify(null, null)).Verifiable(Times.Once());
            var verifiables = new MockVerifiableCollection { verifiableMock.Object };
            var mock = new Mock<TestClassBase>(MockBehavior.Strict);
            mock.Protected().SetupGet<bool>("CleanupCacheAfterTest").Returns(false);
            mock.SetupGet(x => x.Verifiables).Returns(verifiables);
            using var v1 = mock.Protected().Setup("OnCleanupTest").Callback(() => Assert.AreEqual(1, verifiables.Count)).Verifiable(Times.Once());

            mock.Object.CleanupTest();

            Assert.AreEqual(0, verifiables.Count);
        }

        [TestMethod]
        public void DefaultMockBehavior()
        {
            var prevBehavior = TestClassBase.DefaultMockBehavior;
            try
            {
                var mock = new Mock<TestClassBase>(MockBehavior.Loose) { CallBase = true };
                var po = new PrivateObject(mock.Object);

                TestClassBase.DefaultMockBehavior = MockBehavior.Strict;
                var m = po.GetProperty<MockRepository>("Mocks").Create<IDisposable>();
                Assert.AreEqual(MockBehavior.Strict, m.Behavior);

                TestClassBase.DefaultMockBehavior = MockBehavior.Loose;
                m = po.GetProperty<MockRepository>("Mocks").Create<IDisposable>();
                Assert.AreEqual(MockBehavior.Strict, m.Behavior);

                mock.Object.CleanupTest();
                m = po.GetProperty<MockRepository>("Mocks").Create<IDisposable>();
                Assert.AreEqual(MockBehavior.Loose, m.Behavior);
            }
            finally
            {
                TestClassBase.DefaultMockBehavior = prevBehavior;
            }
        }

        [TestMethod]
        public void MockBehavior_()
        {
            var mock = new Mock<TestClassBase>(MockBehavior.Loose) { CallBase = true };
            var po = new PrivateObject(mock.Object);

            mock.Protected().SetupGet<MockBehavior>("MockBehavior").Returns(MockBehavior.Strict);
            var m = po.GetProperty<MockRepository>("Mocks").Create<IDisposable>();
            Assert.AreEqual(MockBehavior.Strict, m.Behavior);

            mock.Protected().SetupGet<MockBehavior>("MockBehavior").Returns(MockBehavior.Loose);
            m = po.GetProperty<MockRepository>("Mocks").Create<IDisposable>();
            Assert.AreEqual(MockBehavior.Strict, m.Behavior);

            mock.Object.CleanupTest();
            m = po.GetProperty<MockRepository>("Mocks").Create<IDisposable>();
            Assert.AreEqual(MockBehavior.Loose, m.Behavior);
        }
    }
}
