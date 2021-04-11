using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;
using System.Threading.Tasks;
using AssertBase = MaSch.Test.Assertion.AssertBase;

namespace MaSch.Test.Components.Test.Assertion
{
    [TestClass]
    public class AssertBaseTests
    {
        [TestMethod]
        public void ThrowAssertError_NullNamePrefix()
        {
            var mock = new Mock<AssertBase>(MockBehavior.Strict);
            mock.Setup(x => x.ThrowAssertError(It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<(string, object?)[]?>())).CallBase();
            mock.Setup(x => x.ThrowAssertError(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<(string, object?)[]?>())).CallBase();
            mock.Protected().SetupGet<string?>("AssertNamePrefix").Returns((string?)null);
            mock.Protected().Setup("HandleFailedAssertion", ItExpr.IsAny<string>());

            mock.Object.ThrowAssertError(12, null);

            mock.Protected().Verify("HandleFailedAssertion", Times.Once(), $"{nameof(ThrowAssertError_NullNamePrefix)} failed.");
        }

        [TestMethod]
        public void ThrowAssertError_NullMessage()
        {
            var mock = new Mock<AssertBase>(MockBehavior.Strict);
            mock.Setup(x => x.ThrowAssertError(It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<(string, object?)[]?>())).CallBase();
            mock.Setup(x => x.ThrowAssertError(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<(string, object?)[]?>())).CallBase();
            mock.Protected().SetupGet<string?>("AssertNamePrefix").Returns("AssertBaseTests");
            mock.Protected().Setup("HandleFailedAssertion", ItExpr.IsAny<string>());

            mock.Object.ThrowAssertError(12, null);

            mock.Protected().Verify("HandleFailedAssertion", Times.Once(), $"AssertBaseTests.{nameof(ThrowAssertError_NullMessage)} failed.");
        }

        [TestMethod]
        public void ThrowAssertError_WithMessage()
        {
            var mock = new Mock<AssertBase>(MockBehavior.Strict);
            mock.Setup(x => x.ThrowAssertError(It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<(string, object?)[]?>())).CallBase();
            mock.Setup(x => x.ThrowAssertError(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<(string, object?)[]?>())).CallBase();
            mock.Protected().SetupGet<string?>("AssertNamePrefix").Returns("AssertBaseTests");
            mock.Protected().Setup("HandleFailedAssertion", ItExpr.IsAny<string>());

            mock.Object.ThrowAssertError(12, "My error message.");

            mock.Protected().Verify("HandleFailedAssertion", Times.Once(), $"AssertBaseTests.{nameof(ThrowAssertError_WithMessage)} failed. My error message.");
        }

        [TestMethod]
        public void ThrowAssertError_GreaterStackFrames()
        {
            var mock = new Mock<AssertBase>(MockBehavior.Strict);
            mock.Setup(x => x.ThrowAssertError(It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<(string, object?)[]?>())).CallBase();
            mock.Setup(x => x.ThrowAssertError(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<(string, object?)[]?>())).CallBase();
            mock.Protected().SetupGet<string?>("AssertNamePrefix").Returns("AssertBaseTests");
            mock.Protected().Setup("HandleFailedAssertion", ItExpr.IsAny<string>());

            TestMethod();

            mock.Protected().Verify("HandleFailedAssertion", Times.Once(), $"AssertBaseTests.{nameof(ThrowAssertError_GreaterStackFrames)} failed. My error message.");

            void TestMethod()
            {
                mock.Object.ThrowAssertError(13, "My error message.");
            }
        }

        [TestMethod]
        public void ThrowAssertError_NegativeStackFrames()
        {
            var mock = new Mock<AssertBase>(MockBehavior.Strict);
            mock.Setup(x => x.ThrowAssertError(It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<(string, object?)[]?>())).CallBase();

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => mock.Object.ThrowAssertError(-1, null));
        }

        [TestMethod]
        public void ThrowAssertError_TooLargeStackFrame()
        {
            var mock = new Mock<AssertBase>(MockBehavior.Strict);
            mock.Setup(x => x.ThrowAssertError(It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<(string, object?)[]?>())).CallBase();
            mock.Setup(x => x.ThrowAssertError(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<(string, object?)[]?>())).CallBase();
            mock.Protected().SetupGet<string?>("AssertNamePrefix").Returns("AssertBaseTests");
            mock.Protected().Setup("HandleFailedAssertion", ItExpr.IsAny<string>());

            mock.Object.ThrowAssertError(1000, "My error message.");

            mock.Protected().Verify("HandleFailedAssertion", Times.Once(), $"AssertBaseTests.<unknown> failed. My error message.");
        }

        [TestMethod]
        public void ThrowAssertError_NullValues()
        {
            var mock = new Mock<AssertBase>(MockBehavior.Strict);
            mock.Setup(x => x.ThrowAssertError(It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<(string, object?)[]?>())).CallBase();
            mock.Setup(x => x.ThrowAssertError(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<(string, object?)[]?>())).CallBase();
            mock.Protected().SetupGet<string?>("AssertNamePrefix").Returns("AssertBaseTests");
            mock.Protected().Setup("HandleFailedAssertion", ItExpr.IsAny<string>());

            mock.Object.ThrowAssertError(12, "My error message.", null);

            mock.Protected().Verify("HandleFailedAssertion", Times.Once(), $"AssertBaseTests.{nameof(ThrowAssertError_NullValues)} failed. My error message.");
        }

        [TestMethod]
        public void ThrowAssertError_WithValues()
        {
            var mock = new Mock<AssertBase>(MockBehavior.Strict);
            mock.Setup(x => x.ThrowAssertError(It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<(string, object?)[]?>())).CallBase();
            mock.Setup(x => x.ThrowAssertError(It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<(string, object?)[]?>())).CallBase();
            mock.Protected().SetupGet<string?>("AssertNamePrefix").Returns("AssertBaseTests");
            mock.Protected().Setup("HandleFailedAssertion", ItExpr.IsAny<string>());

            mock.Object.ThrowAssertError(12, "My error message.", ("blub", 456.123), ("test", "My test value"));

            mock.Protected().Verify("HandleFailedAssertion", Times.Once(), $"AssertBaseTests.{nameof(ThrowAssertError_WithValues)} failed. blub:<456.123>. test:<My test value>. My error message.");
        }

        [TestMethod]
        public void RunAssertion_NoValues_Success()
        {
            var funcMock = new Mock<Func<bool>>(MockBehavior.Strict);
            funcMock.Setup(x => x()).Returns(true);
            var mock = new Mock<AssertBase>(MockBehavior.Strict);
            mock.Setup(x => x.RunAssertion(It.IsAny<string?>(), It.IsAny<Func<bool>>())).CallBase();
            mock.Setup(x => x.ThrowAssertError(1, "My error message", Array.Empty<(string, object?)>()));

            mock.Object.RunAssertion("My error message", funcMock.Object);

            funcMock.Verify(x => x(), Times.Once());
            mock.Verify(x => x.ThrowAssertError(1, "My error message", Array.Empty<(string, object?)>()), Times.Never());
        }

        [TestMethod]
        public void RunAssertion_NoValues_Fail()
        {
            var funcMock = new Mock<Func<bool>>(MockBehavior.Strict);
            funcMock.Setup(x => x()).Returns(false);
            var mock = new Mock<AssertBase>(MockBehavior.Strict);
            mock.Setup(x => x.RunAssertion(It.IsAny<string?>(), It.IsAny<Func<bool>>())).CallBase();
            mock.Setup(x => x.ThrowAssertError(1, "My error message", Array.Empty<(string, object?)>()));

            mock.Object.RunAssertion("My error message", funcMock.Object);

            funcMock.Verify(x => x(), Times.Once());
            mock.Verify(x => x.ThrowAssertError(1, "My error message", Array.Empty<(string, object?)>()), Times.Once());
        }

        [TestMethod]
        public void RunAssertion_WithValues_Success()
        {
            var funcMock = new Mock<Func<string, int, bool>>(MockBehavior.Strict);
            funcMock.Setup(x => x("Str", 4711)).Returns(true);
            var mock = new Mock<AssertBase>(MockBehavior.Strict);
            mock.Setup(x => x.RunAssertion(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<Func<string, int, bool>>())).CallBase();
            mock.Setup(x => x.ThrowAssertError(1, "My error message", It.IsAny<(string, object?)[]?>()));

            mock.Object.RunAssertion("Str", 4711, "My error message", funcMock.Object);

            funcMock.Verify(x => x("Str", 4711), Times.Once());
            mock.Verify(x => x.ThrowAssertError(1, "My error message", It.IsAny<(string, object?)[]?>()), Times.Never());
        }

        [TestMethod]
        public void RunAssertion_WithValues_Fail()
        {
            var funcMock = new Mock<Func<string, int, bool>>(MockBehavior.Strict);
            funcMock.Setup(x => x("Str", 4711)).Returns(false);
            var mock = new Mock<AssertBase>(MockBehavior.Strict);
            mock.Setup(x => x.RunAssertion(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<Func<string, int, bool>>())).CallBase();
            mock.Setup(x => x.ThrowAssertError(1, "My error message", It.IsAny<(string, object?)[]?>()));

            mock.Object.RunAssertion("Str", 4711, "My error message", funcMock.Object);

            funcMock.Verify(x => x("Str", 4711), Times.Once());
            mock.Verify(x => x.ThrowAssertError(1, "My error message", It.Is<(string, object?)[]?>(y => y != null && y.Length == 2 && y[0].Item1 == "Expected" && Equals(y[0].Item2, "Str") && y[1].Item1 == "Actual" && Equals(y[1].Item2, 4711))), Times.Once());
        }

        [TestMethod]
        public void RunNegatedAssertion_WithValues_Success()
        {
            var funcMock = new Mock<Func<string, int, bool>>(MockBehavior.Strict);
            funcMock.Setup(x => x("Str", 4711)).Returns(false);
            var mock = new Mock<AssertBase>(MockBehavior.Strict);
            mock.Setup(x => x.RunNegatedAssertion(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<Func<string, int, bool>>())).CallBase();
            mock.Setup(x => x.ThrowAssertError(1, "My error message", It.IsAny<(string, object?)[]?>()));

            mock.Object.RunNegatedAssertion("Str", 4711, "My error message", funcMock.Object);

            funcMock.Verify(x => x("Str", 4711), Times.Once());
            mock.Verify(x => x.ThrowAssertError(1, "My error message", It.IsAny<(string, object?)[]?>()), Times.Never());
        }

        [TestMethod]
        public void RunNegatedAssertion_WithValues_Fail()
        {
            var funcMock = new Mock<Func<string, int, bool>>(MockBehavior.Strict);
            funcMock.Setup(x => x("Str", 4711)).Returns(true);
            var mock = new Mock<AssertBase>(MockBehavior.Strict);
            mock.Setup(x => x.RunNegatedAssertion(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<Func<string, int, bool>>())).CallBase();
            mock.Setup(x => x.ThrowAssertError(1, "My error message", It.IsAny<(string, object?)[]?>()));

            mock.Object.RunNegatedAssertion("Str", 4711, "My error message", funcMock.Object);

            funcMock.Verify(x => x("Str", 4711), Times.Once());
            mock.Verify(x => x.ThrowAssertError(1, "My error message", It.Is<(string, object?)[]?>(y => y != null && y.Length == 2 && y[0].Item1 == "NotExpected" && Equals(y[0].Item2, "Str") && y[1].Item1 == "Actual" && Equals(y[1].Item2, 4711))), Times.Once());
        }

        [TestMethod]
        public void CatchAssertException_FuncT_Success()
        {
            var funcMock = new Mock<Func<string>>(MockBehavior.Strict);
            funcMock.Setup(x => x()).Returns("MyTest");
            var mock = new Mock<AssertBase>(MockBehavior.Strict);
            mock.Setup(x => x.CatchAssertException(It.IsAny<Func<string>>())).CallBase();
            mock.Protected().Setup("HandleFailedAssertion", ItExpr.IsAny<string>());

            var result = mock.Object.CatchAssertException(funcMock.Object);

            Assert.AreEqual("MyTest", result);
            funcMock.Verify(x => x(), Times.Once());
            mock.Protected().Verify("HandleFailedAssertion", Times.Never(), ItExpr.IsAny<string>());
        }

        [TestMethod]
        public void CatchAssertException_FuncT_Fail()
        {
            var funcMock = new Mock<Func<string>>(MockBehavior.Strict);
            funcMock.Setup(x => x()).Throws(new AssertFailedException("This is a test"));
            var mock = new Mock<AssertBase>(MockBehavior.Strict);
            mock.Setup(x => x.CatchAssertException(It.IsAny<Func<string>>())).CallBase();
            mock.Protected().Setup("HandleFailedAssertion", ItExpr.IsAny<string>());

            var result = mock.Object.CatchAssertException(funcMock.Object);

            Assert.IsNull(result);
            funcMock.Verify(x => x(), Times.Once());
            mock.Protected().Verify("HandleFailedAssertion", Times.Once(), "This is a test");
        }

        [TestMethod]
        public void CatchAssertException_FuncT_Fail_OtherException()
        {
            var funcMock = new Mock<Func<string>>(MockBehavior.Strict);
            funcMock.Setup(x => x()).Throws(new InvalidOperationException("This is a test"));
            var mock = new Mock<AssertBase>(MockBehavior.Strict);
            mock.Setup(x => x.CatchAssertException(It.IsAny<Func<string>>())).CallBase();
            mock.Protected().Setup("HandleFailedAssertion", ItExpr.IsAny<string>());

            Assert.ThrowsException<InvalidOperationException>(() => mock.Object.CatchAssertException(funcMock.Object));

            funcMock.Verify(x => x(), Times.Once());
            mock.Protected().Verify("HandleFailedAssertion", Times.Never(), ItExpr.IsAny<string>());
        }

        [TestMethod]
        public async Task CatchAssertException_FuncTaskT_Success()
        {
            var funcMock = new Mock<Func<Task<string>>>(MockBehavior.Strict);
            funcMock.Setup(x => x()).Returns(Task.FromResult("MyTest"));
            var mock = new Mock<AssertBase>(MockBehavior.Strict);
            mock.Setup(x => x.CatchAssertException(It.IsAny<Func<Task<string>>>())).CallBase();
            mock.Protected().Setup("HandleFailedAssertion", ItExpr.IsAny<string>());

            var result = await mock.Object.CatchAssertException(funcMock.Object);

            Assert.AreEqual("MyTest", result);
            funcMock.Verify(x => x(), Times.Once());
            mock.Protected().Verify("HandleFailedAssertion", Times.Never(), ItExpr.IsAny<string>());
        }

        [TestMethod]
        public async Task CatchAssertException_FuncTaskT_Fail()
        {
            var funcMock = new Mock<Func<Task<string>>>(MockBehavior.Strict);
            funcMock.Setup(x => x()).Throws(new AssertFailedException("This is a test"));
            var mock = new Mock<AssertBase>(MockBehavior.Strict);
            mock.Setup(x => x.CatchAssertException(It.IsAny<Func<Task<string>>>())).CallBase();
            mock.Protected().Setup("HandleFailedAssertion", ItExpr.IsAny<string>());

            var result = await mock.Object.CatchAssertException(funcMock.Object);

            Assert.IsNull(result);
            funcMock.Verify(x => x(), Times.Once());
            mock.Protected().Verify("HandleFailedAssertion", Times.Once(), "This is a test");
        }

        [TestMethod]
        public async Task CatchAssertException_FuncTaskT_Fail_OtherException()
        {
            var funcMock = new Mock<Func<Task<string>>>(MockBehavior.Strict);
            funcMock.Setup(x => x()).Throws(new InvalidOperationException("This is a test"));
            var mock = new Mock<AssertBase>(MockBehavior.Strict);
            mock.Setup(x => x.CatchAssertException(It.IsAny<Func<Task<string>>>())).CallBase();
            mock.Protected().Setup("HandleFailedAssertion", ItExpr.IsAny<string>());

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => await mock.Object.CatchAssertException(funcMock.Object));

            funcMock.Verify(x => x(), Times.Once());
            mock.Protected().Verify("HandleFailedAssertion", Times.Never(), ItExpr.IsAny<string>());
        }

        [TestMethod]
        public void CatchAssertException_Action_Success()
        {
            var actionMock = new Mock<Action>(MockBehavior.Strict);
            actionMock.Setup(x => x());
            var mock = new Mock<AssertBase>(MockBehavior.Strict);
            mock.Setup(x => x.CatchAssertException(It.IsAny<Action>())).CallBase();
            mock.Protected().Setup("HandleFailedAssertion", ItExpr.IsAny<string>());

            mock.Object.CatchAssertException(actionMock.Object);

            actionMock.Verify(x => x(), Times.Once());
            mock.Protected().Verify("HandleFailedAssertion", Times.Never(), ItExpr.IsAny<string>());
        }

        [TestMethod]
        public void CatchAssertException_Action_Fail()
        {
            var actionMock = new Mock<Action>(MockBehavior.Strict);
            actionMock.Setup(x => x()).Throws(new AssertFailedException("This is a test"));
            var mock = new Mock<AssertBase>(MockBehavior.Strict);
            mock.Setup(x => x.CatchAssertException(It.IsAny<Action>())).CallBase();
            mock.Protected().Setup("HandleFailedAssertion", ItExpr.IsAny<string>());

            mock.Object.CatchAssertException(actionMock.Object);

            actionMock.Verify(x => x(), Times.Once());
            mock.Protected().Verify("HandleFailedAssertion", Times.Once(), "This is a test");
        }

        [TestMethod]
        public void CatchAssertException_Action_Fail_OtherException()
        {
            var actionMock = new Mock<Action>(MockBehavior.Strict);
            actionMock.Setup(x => x()).Throws(new InvalidOperationException("This is a test"));
            var mock = new Mock<AssertBase>(MockBehavior.Strict);
            mock.Setup(x => x.CatchAssertException(It.IsAny<Action>())).CallBase();
            mock.Protected().Setup("HandleFailedAssertion", ItExpr.IsAny<string>());

            Assert.ThrowsException<InvalidOperationException>(() => mock.Object.CatchAssertException(actionMock.Object));

            actionMock.Verify(x => x(), Times.Once());
            mock.Protected().Verify("HandleFailedAssertion", Times.Never(), ItExpr.IsAny<string>());
        }

        [TestMethod]
        public async Task CatchAssertException_AsyncAction_Success()
        {
            var actionMock = new Mock<Func<Task>>(MockBehavior.Strict);
            actionMock.Setup(x => x()).Returns(Task.CompletedTask);
            var mock = new Mock<AssertBase>(MockBehavior.Strict);
            mock.Setup(x => x.CatchAssertException(It.IsAny<Func<Task>>())).CallBase();
            mock.Protected().Setup("HandleFailedAssertion", ItExpr.IsAny<string>());

            await mock.Object.CatchAssertException(actionMock.Object);

            actionMock.Verify(x => x(), Times.Once());
            mock.Protected().Verify("HandleFailedAssertion", Times.Never(), ItExpr.IsAny<string>());
        }

        [TestMethod]
        public async Task CatchAssertException_AsyncAction_Fail()
        {
            var actionMock = new Mock<Func<Task>>(MockBehavior.Strict);
            actionMock.Setup(x => x()).Throws(new AssertFailedException("This is a test"));
            var mock = new Mock<AssertBase>(MockBehavior.Strict);
            mock.Setup(x => x.CatchAssertException(It.IsAny<Func<Task>>())).CallBase();
            mock.Protected().Setup("HandleFailedAssertion", ItExpr.IsAny<string>());

            await mock.Object.CatchAssertException(actionMock.Object);

            actionMock.Verify(x => x(), Times.Once());
            mock.Protected().Verify("HandleFailedAssertion", Times.Once(), "This is a test");
        }

        [TestMethod]
        public async Task CatchAssertException_AsyncAction_Fail_OtherException()
        {
            var actionMock = new Mock<Func<Task>>(MockBehavior.Strict);
            actionMock.Setup(x => x()).Throws(new InvalidOperationException("This is a test"));
            var mock = new Mock<AssertBase>(MockBehavior.Strict);
            mock.Setup(x => x.CatchAssertException(It.IsAny<Func<Task>>())).CallBase();
            mock.Protected().Setup("HandleFailedAssertion", ItExpr.IsAny<string>());

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => await mock.Object.CatchAssertException(actionMock.Object));

            actionMock.Verify(x => x(), Times.Once());
            mock.Protected().Verify("HandleFailedAssertion", Times.Never(), ItExpr.IsAny<string>());
        }
    }
}
