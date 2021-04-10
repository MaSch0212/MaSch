using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MaSch.Test.Components.Test.Assertion
{
    [TestClass]
    public class StringAssertionsTests
    {
        private static MaSch.Test.Assertion.Assert AssertUnderTest => MaSch.Test.Assertion.Assert.Instance;

        #region Contains (string)

        [TestMethod]
        public void ContainsStr_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("blub", (string?)null));
            Assert.AreEqual("Assert.Contains failed. Expected:<blub>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void ContainsStr_Fail_DifferentContent()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("blub", "bbb"));
            Assert.AreEqual("Assert.Contains failed. Expected:<blub>. Actual:<bbb>.", ex.Message);
        }

        [TestMethod]
        public void ContainsStr_Fail_DifferentCasing()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("blub", "BLUB"));
            Assert.AreEqual("Assert.Contains failed. Expected:<blub>. Actual:<BLUB>.", ex.Message);
        }

        [TestMethod]
        public void ContainsStr_Success()
        {
            AssertUnderTest.Contains("blub", "jhfkjhfdgblubkjfh");
        }

        [TestMethod]
        public void ContainsStr_WithMessage_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("blub", (string?)null, "This is my test"));
            Assert.AreEqual("Assert.Contains failed. Expected:<blub>. Actual:<(null)>. This is my test", ex.Message);
        }

        [TestMethod]
        public void ContainsStr_WithMessage_Fail_DifferentContent()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("blub", "bbb", "This is my test"));
            Assert.AreEqual("Assert.Contains failed. Expected:<blub>. Actual:<bbb>. This is my test", ex.Message);
        }

        [TestMethod]
        public void ContainsStr_WithMessage_Fail_DifferentCasing()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("blub", "BLUB", "This is my test"));
            Assert.AreEqual("Assert.Contains failed. Expected:<blub>. Actual:<BLUB>. This is my test", ex.Message);
        }

        [TestMethod]
        public void ContainsStr_WithMessage_Success()
        {
            AssertUnderTest.Contains("blub", "jhfkjhfdgblubkjfh", "This is my test");
        }

        [TestMethod]
        public void ContainsStr_WithComparison_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("blub", null, StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual("Assert.Contains failed. Expected:<blub>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void ContainsStr_WithComparison_Fail_DifferentContent()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("blub", "bbb", StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual("Assert.Contains failed. Expected:<blub>. Actual:<bbb>.", ex.Message);
        }

        [TestMethod]
        public void ContainsStr_WithComparison_Success_SameCasing()
        {
            AssertUnderTest.Contains("blub", "jhfkjhfdgblubkjfh", StringComparison.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void ContainsStr_WithComparison_Success_DifferentCasing()
        {
            AssertUnderTest.Contains("blub", "jhfkjhfdgBLUBkjfh", StringComparison.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void ContainsStr_WithComparison_WithMessage_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("blub", null, StringComparison.OrdinalIgnoreCase, "This is my test"));
            Assert.AreEqual("Assert.Contains failed. Expected:<blub>. Actual:<(null)>. This is my test", ex.Message);
        }

        [TestMethod]
        public void ContainsStr_WithComparison_WithMessage_Fail_DifferentContent()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("blub", "bbb", StringComparison.OrdinalIgnoreCase, "This is my test"));
            Assert.AreEqual("Assert.Contains failed. Expected:<blub>. Actual:<bbb>. This is my test", ex.Message);
        }

        [TestMethod]
        public void ContainsStr_WithComparison_WithMessage_Success_SameCasing()
        {
            AssertUnderTest.Contains("blub", "jhfkjhfdgblubkjfh", StringComparison.OrdinalIgnoreCase, "This is my test");
        }

        [TestMethod]
        public void ContainsStr_WithComparison_WithMessage_Success_DifferentCasing()
        {
            AssertUnderTest.Contains("blub", "jhfkjhfdgBLUBkjfh", StringComparison.OrdinalIgnoreCase, "This is my test");
        }

        #endregion
    }
}
