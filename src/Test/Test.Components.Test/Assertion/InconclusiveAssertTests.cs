using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace MaSch.Test.Components.Test.Assertion
{
    [TestClass]
    public class InconclusiveAssertTests
    {
        private static MaSch.Test.Assertion.InconclusiveAssert AssertUnderTest => MaSch.Test.Assertion.InconclusiveAssert.Instance;

        [TestMethod]
        public void Instance()
        {
            Assert.IsNotNull(MaSch.Test.Assertion.InconclusiveAssert.Instance);
        }

        [TestMethod]
        public void That_Action()
        {
            var actionMock = new Mock<Action<Assert>>(MockBehavior.Strict);
            actionMock.Setup(x => x(Assert.That)).Throws(new Exception(nameof(That_Action)));
            var mock = new Mock<MaSch.Test.Assertion.InconclusiveAssert>(MockBehavior.Strict);
            mock.Setup(x => x.That(It.IsAny<Action<Assert>>())).CallBase();
            mock.Setup(x => x.CatchAssertException(It.IsAny<Action>()));

            mock.Object.That(actionMock.Object);

            mock.Verify(x => x.CatchAssertException(It.Is<Action>(y => ValidateThatException(y, nameof(That_Action)))), Times.Once());
        }

        [TestMethod]
        public void That_FuncT()
        {
            var actionMock = new Mock<Func<Assert, string>>(MockBehavior.Strict);
            actionMock.Setup(x => x(Assert.That)).Throws(new Exception(nameof(That_FuncT)));
            var mock = new Mock<MaSch.Test.Assertion.InconclusiveAssert>(MockBehavior.Strict);
            mock.Setup(x => x.That(It.IsAny<Func<Assert, string>>())).CallBase();
            mock.Setup(x => x.CatchAssertException(It.IsAny<Func<string>>())).Returns("Str");

            var result = mock.Object.That(actionMock.Object);

            Assert.AreEqual("Str", result);
            mock.Verify(x => x.CatchAssertException(It.Is<Func<string>>(y => ValidateThatException(() => y(), nameof(That_FuncT)))), Times.Once());
        }

        [TestMethod]
        public async Task That_AsyncAction()
        {
            var actionMock = new Mock<Func<Assert, Task>>(MockBehavior.Strict);
            actionMock.Setup(x => x(Assert.That)).Throws(new Exception(nameof(That_AsyncAction)));
            var mock = new Mock<MaSch.Test.Assertion.InconclusiveAssert>(MockBehavior.Strict);
            mock.Setup(x => x.That(It.IsAny<Func<Assert, Task>>())).CallBase();
            mock.Setup(x => x.CatchAssertException(It.IsAny<Func<Task>>())).Returns(Task.CompletedTask);

            await mock.Object.That(actionMock.Object);

            mock.Verify(x => x.CatchAssertException(It.Is<Func<Task>>(y => ValidateThatException(() => y(), nameof(That_AsyncAction)))), Times.Once());
        }

        [TestMethod]
        public async Task That_FuncTaskT()
        {
            var actionMock = new Mock<Func<Assert, Task<string>>>(MockBehavior.Strict);
            actionMock.Setup(x => x(Assert.That)).Throws(new Exception(nameof(That_FuncTaskT)));
            var mock = new Mock<MaSch.Test.Assertion.InconclusiveAssert>(MockBehavior.Strict);
            mock.Setup(x => x.That(It.IsAny<Func<Assert, Task<string>>>())).CallBase();
            mock.Setup(x => x.CatchAssertException(It.IsAny<Func<Task<string>>>())).Returns(Task.FromResult("Str"));

            var result = await mock.Object.That(actionMock.Object);

            Assert.AreEqual("Str", result);
            mock.Verify(x => x.CatchAssertException(It.Is<Func<Task<string>>>(y => ValidateThatException(() => y(), nameof(That_FuncTaskT)))), Times.Once());
        }

        [TestMethod]
        public void AssertNamePrefix()
        {
            var prop = typeof(MaSch.Test.Assertion.InconclusiveAssert).GetProperty("AssertNamePrefix", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(prop);
            Assert.AreEqual("Assert.Inc", prop.GetValue(AssertUnderTest));
        }

        [TestMethod]
        public void HandleFailedAssertion()
        {
            var method = typeof(MaSch.Test.Assertion.InconclusiveAssert).GetMethod("HandleFailedAssertion", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(method);
            var ex = Assert.ThrowsException<TargetInvocationException>(() => method.Invoke(AssertUnderTest, new object[] { "My test error message" }));
            Assert.IsInstanceOfType(ex.InnerException, typeof(AssertInconclusiveException));
            Assert.AreEqual("[Inconclusive] My test error message", ex.InnerException!.Message);
        }

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
}
