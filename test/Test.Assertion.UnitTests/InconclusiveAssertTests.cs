using MSAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace MaSch.Test.Assertion.UnitTests;

[TestClass]
public class InconclusiveAssertTests
{
    private static InconclusiveAssert AssertUnderTest => MaSch.Test.Assertion.InconclusiveAssert.Instance;

    [TestMethod]
    public void Instance()
    {
        MSAssert.IsNotNull(MaSch.Test.Assertion.InconclusiveAssert.Instance);
    }

#if MSTEST
    [TestMethod]
    public void That_Action()
    {
        var actionMock = new Mock<Action<MSAssert>>(MockBehavior.Strict);
        _ = actionMock.Setup(x => x(MSAssert.That)).Throws(new Exception(nameof(That_Action)));
        var mock = new Mock<InconclusiveAssert>(MockBehavior.Strict);
        _ = mock.Setup(x => x.That(It.IsAny<Action<MSAssert>>())).CallBase();
        _ = mock.Setup(x => x.CatchAssertException(It.IsAny<Action>()));

        mock.Object.That(actionMock.Object);

        mock.Verify(x => x.CatchAssertException(It.Is<Action>(y => ValidateThatException(y, nameof(That_Action)))), Times.Once());
    }

    [TestMethod]
    public void That_FuncT()
    {
        var actionMock = new Mock<Func<MSAssert, string>>(MockBehavior.Strict);
        _ = actionMock.Setup(x => x(MSAssert.That)).Throws(new Exception(nameof(That_FuncT)));
        var mock = new Mock<InconclusiveAssert>(MockBehavior.Strict);
        _ = mock.Setup(x => x.That(It.IsAny<Func<MSAssert, string>>())).CallBase();
        _ = mock.Setup(x => x.CatchAssertException(It.IsAny<Func<string>>())).Returns("Str");

        var result = mock.Object.That(actionMock.Object);

        MSAssert.AreEqual("Str", result);
        mock.Verify(x => x.CatchAssertException(It.Is<Func<string>>(y => ValidateThatException(() => y(), nameof(That_FuncT)))), Times.Once());
    }

    [TestMethod]
    public async Task That_AsyncAction()
    {
        var actionMock = new Mock<Func<MSAssert, Task>>(MockBehavior.Strict);
        _ = actionMock.Setup(x => x(MSAssert.That)).Throws(new Exception(nameof(That_AsyncAction)));
        var mock = new Mock<InconclusiveAssert>(MockBehavior.Strict);
        _ = mock.Setup(x => x.That(It.IsAny<Func<MSAssert, Task>>())).CallBase();
        _ = mock.Setup(x => x.CatchAssertException(It.IsAny<Func<Task>>())).Returns(Task.CompletedTask);

        await mock.Object.That(actionMock.Object);

        mock.Verify(x => x.CatchAssertException(It.Is<Func<Task>>(y => ValidateThatException(() => y(), nameof(That_AsyncAction)))), Times.Once());
    }

    [TestMethod]
    public async Task That_FuncTaskT()
    {
        var actionMock = new Mock<Func<MSAssert, Task<string>>>(MockBehavior.Strict);
        _ = actionMock.Setup(x => x(MSAssert.That)).Throws(new Exception(nameof(That_FuncTaskT)));
        var mock = new Mock<InconclusiveAssert>(MockBehavior.Strict);
        _ = mock.Setup(x => x.That(It.IsAny<Func<MSAssert, Task<string>>>())).CallBase();
        _ = mock.Setup(x => x.CatchAssertException(It.IsAny<Func<Task<string>>>())).Returns(Task.FromResult("Str"));

        var result = await mock.Object.That(actionMock.Object);

        MSAssert.AreEqual("Str", result);
        mock.Verify(x => x.CatchAssertException(It.Is<Func<Task<string>>>(y => ValidateThatException(() => y(), nameof(That_FuncTaskT)))), Times.Once());
    }
#endif

    [TestMethod]
    public void AssertNamePrefix()
    {
        var prop = typeof(InconclusiveAssert).GetProperty("AssertNamePrefix", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        MSAssert.IsNotNull(prop);
        MSAssert.AreEqual("Assert.Inc", prop.GetValue(AssertUnderTest));
    }

    [TestMethod]
    public void HandleFailedAssertion()
    {
        var method = typeof(InconclusiveAssert).GetMethod("HandleFailedAssertion", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly, null, new[] { typeof(string) }, null);
        MSAssert.IsNotNull(method);
        var ex = MSAssert.ThrowsException<TargetInvocationException>(() => method.Invoke(AssertUnderTest, new object[] { "My test error message" }));
        MSAssert.IsInstanceOfType(ex.InnerException, typeof(AssertInconclusiveException));
        MSAssert.AreEqual("[Inconclusive] My test error message", ex.InnerException!.Message);
    }

    [ExcludeFromCodeCoverage]
    private static bool ValidateThatException(Action action, string methodName)
    {
        try
        {
            action();
            return false;
        }
        catch (Exception ex)
        {
            return ex.Message == methodName;
        }
    }
}
