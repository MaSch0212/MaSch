using MaSch.Core;
using MaSch.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MaSch.Test.Components.Test
{
    [TestClass]
    public class TestClassBaseTests
    {
        [TestMethod]
        public void InitializeTest_()
        {
            var mock = CreateTestClassMock(false);
            using var v1 = mock.Protected().Setup("OnInitializeTest").Verifiable(Times.Once());

            mock.Object.InitializeTest();
        }

        [TestMethod]
        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP004:Don't ignore created IDisposable.", Justification = "Fine here")]
        public void CleanupTest_()
        {
            var mock = CreateTestClassMock(false);
            mock.Protected().SetupGet<bool>("CleanupCacheAfterTest").Returns(false);
            mock.SetupGet(x => x.Verifiables).Returns(new MockVerifiableCollection());
            using var v1 = mock.Protected().Setup("OnCleanupTest").Verifiable(Times.Once());

            mock.Object.CleanupTest();
        }

        [TestMethod]
        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP004:Don't ignore created IDisposable.", Justification = "Fine here")]
        public void CleanupTest_Cache_Clear()
        {
            using var cache = new Cache();
            cache.SetValue("blub", "test");
            var mock = CreateTestClassMock(false);
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
        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP004:Don't ignore created IDisposable.", Justification = "Fine here")]
        public void CleanupTest_Cache_NoClear()
        {
            using var cache = new Cache();
            cache.SetValue("blub", "test");
            var mock = CreateTestClassMock(false);
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
        public void CleanupTest_VerifyVerifiables_TestPassed()
        {
            var verifiableMock = new Mock<IMockVerifiable>(MockBehavior.Strict);
            using var v2 = verifiableMock.Setup(x => x.Verify(null, null)).Verifiable(Times.Once());
            using var verifiables = new MockVerifiableCollection { verifiableMock.Object };
            var mock = CreateTestClassMock(false);
            mock.Protected().SetupGet<bool>("CleanupCacheAfterTest").Returns(false);
            mock.SetupGet(x => x.Verifiables).Returns(verifiables);
            using var v1 = mock.Protected().Setup("OnCleanupTest").Callback(() => Assert.AreEqual(1, verifiables.Count)).Verifiable(Times.Once());

            mock.Object.CleanupTest();

            Assert.AreEqual(0, verifiables.Count);
        }

        [TestMethod]
        public void CleanupTest_VerifyVerifiables_TestFailed()
        {
            var verifiableMock = new Mock<IMockVerifiable>(MockBehavior.Strict);
            using var v2 = verifiableMock.Setup(x => x.Verify(null, null)).Verifiable(Times.Never());
            using var verifiables = new MockVerifiableCollection { verifiableMock.Object };
            var mock = CreateTestClassMock(false, out var testContextMock);
            testContextMock.Setup(x => x.CurrentTestOutcome).Returns(UnitTestOutcome.Failed);
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
                var mock = CreateTestClassMock(true);
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
            var mock = CreateTestClassMock(true);
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

        private Mock<TestClassBase> CreateTestClassMock(bool callBase)
            => CreateTestClassMock(callBase, out _);

        private Mock<TestClassBase> CreateTestClassMock(bool callBase, out Mock<TestContext> testContextMock)
        {
            var mock = new Mock<TestClassBase>(callBase ? MockBehavior.Loose : MockBehavior.Strict) { CallBase = callBase };
            testContextMock = new Mock<TestContext>(MockBehavior.Strict);
            testContextMock.Setup(x => x.CurrentTestOutcome).Returns(UnitTestOutcome.Passed);
            mock.Object.TestContext = testContextMock.Object;
            return mock;
        }
    }
}
