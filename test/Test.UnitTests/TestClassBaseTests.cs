using MaSch.Core;
using MaSch.Test.Models;

namespace MaSch.Test.UnitTests;

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
    public void CleanupTest_()
    {
        var mock = CreateTestClassMock(false);
        _ = mock.Protected().SetupGet<bool>("CleanupCacheAfterTest").Returns(false);
        _ = mock.SetupGet(x => x.Verifiables).Returns(new MockVerifiableCollection());
        using var v1 = mock.Protected().Setup("OnCleanupTest").Verifiable(Times.Once());

        mock.Object.CleanupTest();
    }

    [TestMethod]
    public void CleanupTest_Cache_Clear()
    {
        using var cache = new Cache();
        cache.SetValue("blub", "test");
        var mock = CreateTestClassMock(false);
        _ = mock.Protected().SetupGet<bool>("CleanupCacheAfterTest").Returns(true);
        _ = mock.Protected().SetupGet<Cache>("Cache").Returns(cache);
        _ = mock.SetupGet(x => x.Verifiables).Returns(new MockVerifiableCollection());
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
        using var cache = new Cache();
        cache.SetValue("blub", "test");
        var mock = CreateTestClassMock(false);
        _ = mock.Protected().SetupGet<bool>("CleanupCacheAfterTest").Returns(false);
        _ = mock.Protected().SetupGet<Cache>("Cache").Returns(cache);
        _ = mock.SetupGet(x => x.Verifiables).Returns(new MockVerifiableCollection());
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
        _ = mock.Protected().SetupGet<bool>("CleanupCacheAfterTest").Returns(false);
        _ = mock.SetupGet(x => x.Verifiables).Returns(verifiables);
        using var v1 = mock.Protected().Setup("OnCleanupTest").Verifiable(Times.Once());

        mock.Object.CleanupTest();

        Assert.AreEqual(0, verifiables.Count);
    }

#if MSTEST
    [TestMethod]
    public void CleanupTest_VerifyVerifiables_TestFailed()
    {
        var verifiableMock = new Mock<IMockVerifiable>(MockBehavior.Strict);
        using var v2 = verifiableMock.Setup(x => x.Verify(null, null)).Verifiable(Times.Never());
        using var verifiables = new MockVerifiableCollection { verifiableMock.Object };
        var mock = CreateTestClassMock(false, out var testContextMock);
        _ = testContextMock.Setup(x => x.CurrentTestOutcome).Returns(UnitTestOutcome.Failed);
        _ = mock.Protected().SetupGet<bool>("CleanupCacheAfterTest").Returns(false);
        _ = mock.SetupGet(x => x.Verifiables).Returns(verifiables);
        using var v1 = mock.Protected().Setup("OnCleanupTest").Verifiable(Times.Once());

        mock.Object.CleanupTest();

        Assert.AreEqual(0, verifiables.Count);
    }
#endif

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

        _ = mock.Protected().SetupGet<MockBehavior>("MockBehavior").Returns(MockBehavior.Strict);
        var m = po.GetProperty<MockRepository>("Mocks").Create<IDisposable>();
        Assert.AreEqual(MockBehavior.Strict, m.Behavior);

        _ = mock.Protected().SetupGet<MockBehavior>("MockBehavior").Returns(MockBehavior.Loose);
        m = po.GetProperty<MockRepository>("Mocks").Create<IDisposable>();
        Assert.AreEqual(MockBehavior.Strict, m.Behavior);

        mock.Object.CleanupTest();
        m = po.GetProperty<MockRepository>("Mocks").Create<IDisposable>();
        Assert.AreEqual(MockBehavior.Loose, m.Behavior);
    }

#if MSTEST
    private Mock<TestClassBase> CreateTestClassMock(bool callBase)
    {
        return CreateTestClassMock(callBase, out _);
    }

    private Mock<TestClassBase> CreateTestClassMock(bool callBase, out Mock<TestContext> testContextMock)
    {
        var mock = new Mock<TestClassBase>(callBase ? MockBehavior.Loose : MockBehavior.Strict) { CallBase = callBase };
        testContextMock = new Mock<TestContext>(MockBehavior.Strict);
        _ = testContextMock.Setup(x => x.CurrentTestOutcome).Returns(UnitTestOutcome.Passed);
        mock.Object.TestContext = testContextMock.Object;
        return mock;
    }
#else
    private Mock<TestClassBase> CreateTestClassMock(bool callBase)
    {
        var mock = new Mock<TestClassBase>(callBase ? MockBehavior.Loose : MockBehavior.Strict) { CallBase = callBase };
        return mock;
    }
#endif
}
