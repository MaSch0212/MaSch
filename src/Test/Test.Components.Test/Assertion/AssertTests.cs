using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace MaSch.Test.Components.Test.Assertion
{
    [TestClass]
    public class AssertTests
    {
        private static MaSch.Test.Assertion.Assert AssertUnderTest => MaSch.Test.Assertion.Assert.Instance;

        [TestMethod]
        public void Instance()
        {
            Assert.IsNotNull(MaSch.Test.Assertion.Assert.Instance);
        }

        [TestMethod]
        public void That()
        {
            Assert.AreSame(Assert.That, AssertUnderTest.That);
        }

        [TestMethod]
        public void Inc()
        {
            Assert.AreSame(MaSch.Test.Assertion.InconclusiveAssert.Instance, AssertUnderTest.Inc);
        }

        [TestMethod]
        public void AssertNamePrefix()
        {
            var prop = typeof(MaSch.Test.Assertion.Assert).GetProperty("AssertNamePrefix", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(prop);
            Assert.AreEqual("Assert", prop.GetValue(AssertUnderTest));
        }

        [TestMethod]
        public void HandleFailedAssertion()
        {
            var method = typeof(MaSch.Test.Assertion.Assert).GetMethod("HandleFailedAssertion", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(method);
            var ex = Assert.ThrowsException<TargetInvocationException>(() => method.Invoke(AssertUnderTest, new object[] { "My test error message" }));
            Assert.IsInstanceOfType(ex.InnerException, typeof(AssertFailedException));
            Assert.AreEqual("My test error message", ex.InnerException!.Message);
        }
    }
}
