using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.RegularExpressions;

namespace MaSch.Test.Components.Test.Assertion
{
    [TestClass]
    public class StringAssertionsTests
    {
        private static MaSch.Test.Assertion.Assert AssertUnderTest => MaSch.Test.Assertion.Assert.Instance;

        #region Contains

        [TestMethod]
        public void Contains_Success()
        {
            AssertUnderTest.Contains("blub", "jhfkjhfdgblubkjfh");
        }

        [TestMethod]
        public void Contains_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("blub", (string?)null));
            Assert.AreEqual("Assert.Contains failed. Expected:<blub>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void Contains_Fail_DifferentContent()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("blub", "bbb"));
            Assert.AreEqual("Assert.Contains failed. Expected:<blub>. Actual:<bbb>.", ex.Message);
        }

        [TestMethod]
        public void Contains_Fail_DifferentCasing()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("blub", "BLUB"));
            Assert.AreEqual("Assert.Contains failed. Expected:<blub>. Actual:<BLUB>.", ex.Message);
        }

        [TestMethod]
        public void Contains_WithMessage_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("blub", (string?)null, "This is my test"));
            Assert.AreEqual("Assert.Contains failed. Expected:<blub>. Actual:<(null)>. This is my test", ex.Message);
        }

        [TestMethod]
        public void Contains_WithMessage_Fail_DifferentContent()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("blub", "bbb", "This is my test"));
            Assert.AreEqual("Assert.Contains failed. Expected:<blub>. Actual:<bbb>. This is my test", ex.Message);
        }

        [TestMethod]
        public void Contains_WithMessage_Fail_DifferentCasing()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("blub", "BLUB", "This is my test"));
            Assert.AreEqual("Assert.Contains failed. Expected:<blub>. Actual:<BLUB>. This is my test", ex.Message);
        }

        [TestMethod]
        public void Contains_WithComparison_Success_SameCasing()
        {
            AssertUnderTest.Contains("blub", "jhfkjhfdgblubkjfh", StringComparison.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void Contains_WithComparison_Success_DifferentCasing()
        {
            AssertUnderTest.Contains("blub", "jhfkjhfdgBLUBkjfh", StringComparison.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void Contains_WithComparison_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("blub", null, StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual("Assert.Contains failed. Expected:<blub>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void Contains_WithComparison_Fail_DifferentContent()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("blub", "bbb", StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual("Assert.Contains failed. Expected:<blub>. Actual:<bbb>.", ex.Message);
        }

        [TestMethod]
        public void Contains_WithComparison_WithMessage_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("blub", null, StringComparison.OrdinalIgnoreCase, "This is my test"));
            Assert.AreEqual("Assert.Contains failed. Expected:<blub>. Actual:<(null)>. This is my test", ex.Message);
        }

        [TestMethod]
        public void Contains_WithComparison_WithMessage_Fail_DifferentContent()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("blub", "bbb", StringComparison.OrdinalIgnoreCase, "This is my test"));
            Assert.AreEqual("Assert.Contains failed. Expected:<blub>. Actual:<bbb>. This is my test", ex.Message);
        }

        #endregion

        #region ContainsAny

        [TestMethod]
        public void ContainsAny_Success()
        {
            AssertUnderTest.ContainsAny(new[] { "blub", "blib" }, "jhfkjhfdgblubkjfh");
        }

        [TestMethod]
        public void ContainsAny_Null()
        {
            var nl = Environment.NewLine;
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ContainsAny(new[] { "blub", "blib" }, (string?)null));
            Assert.AreEqual($"Assert.ContainsAny failed. Expected:<[{nl}\tblub,{nl}\tblib{nl}]>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void ContainsAny_NoneMatch()
        {
            var nl = Environment.NewLine;
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ContainsAny(new[] { "blub", "blib" }, "kjhgfdkghggfd"));
            Assert.AreEqual($"Assert.ContainsAny failed. Expected:<[{nl}\tblub,{nl}\tblib{nl}]>. Actual:<kjhgfdkghggfd>.", ex.Message);
        }

        [TestMethod]
        public void ContainsAny_Match_DifferentCasing()
        {
            var nl = Environment.NewLine;
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ContainsAny(new[] { "blub", "blib" }, "kjhgfdBLUBkghggfd"));
            Assert.AreEqual($"Assert.ContainsAny failed. Expected:<[{nl}\tblub,{nl}\tblib{nl}]>. Actual:<kjhgfdBLUBkghggfd>.", ex.Message);
        }

        [TestMethod]
        public void ContainsAny_WithMessage_Null()
        {
            var nl = Environment.NewLine;
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ContainsAny(new[] { "blub", "blib" }, (string?)null, "This is my test"));
            Assert.AreEqual($"Assert.ContainsAny failed. Expected:<[{nl}\tblub,{nl}\tblib{nl}]>. Actual:<(null)>. This is my test", ex.Message);
        }

        [TestMethod]
        public void ContainsAny_WithMessage_NoneMatch()
        {
            var nl = Environment.NewLine;
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ContainsAny(new[] { "blub", "blib" }, "kjhgfdkghggfd", "This is my test"));
            Assert.AreEqual($"Assert.ContainsAny failed. Expected:<[{nl}\tblub,{nl}\tblib{nl}]>. Actual:<kjhgfdkghggfd>. This is my test", ex.Message);
        }

        [TestMethod]
        public void ContainsAny_WithMessage_Match_DifferentCasing()
        {
            var nl = Environment.NewLine;
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ContainsAny(new[] { "blub", "blib" }, "kjhgfdBLUBkghggfd", "This is my test"));
            Assert.AreEqual($"Assert.ContainsAny failed. Expected:<[{nl}\tblub,{nl}\tblib{nl}]>. Actual:<kjhgfdBLUBkghggfd>. This is my test", ex.Message);
        }

        [TestMethod]
        public void ContainsAny_WithComparison_Success_SameCasing()
        {
            AssertUnderTest.ContainsAny(new[] { "blub", "blib" }, "jhfkjhfdgblubkjfh", StringComparison.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void ContainsAny_WithComparison_Success_DifferentCasing()
        {
            AssertUnderTest.ContainsAny(new[] { "blub", "blib" }, "jhfkjhfdgBLUBkjfh", StringComparison.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void ContainsAny_WithComparison_Null()
        {
            var nl = Environment.NewLine;
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ContainsAny(new[] { "blub", "blib" }, (string?)null, StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual($"Assert.ContainsAny failed. Expected:<[{nl}\tblub,{nl}\tblib{nl}]>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void ContainsAny_WithComparison_NoneMatch()
        {
            var nl = Environment.NewLine;
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ContainsAny(new[] { "blub", "blib" }, "kjhgfdkghggfd", StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual($"Assert.ContainsAny failed. Expected:<[{nl}\tblub,{nl}\tblib{nl}]>. Actual:<kjhgfdkghggfd>.", ex.Message);
        }

        [TestMethod]
        public void ContainsAny_WithComparison_WithMessage_Null()
        {
            var nl = Environment.NewLine;
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ContainsAny(new[] { "blub", "blib" }, (string?)null, StringComparison.OrdinalIgnoreCase, "This is my test"));
            Assert.AreEqual($"Assert.ContainsAny failed. Expected:<[{nl}\tblub,{nl}\tblib{nl}]>. Actual:<(null)>. This is my test", ex.Message);
        }

        [TestMethod]
        public void ContainsAny_WithComparison_WithMessage_NoneMatch()
        {
            var nl = Environment.NewLine;
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ContainsAny(new[] { "blub", "blib" }, "kjhgfdkghggfd", StringComparison.OrdinalIgnoreCase, "This is my test"));
            Assert.AreEqual($"Assert.ContainsAny failed. Expected:<[{nl}\tblub,{nl}\tblib{nl}]>. Actual:<kjhgfdkghggfd>. This is my test", ex.Message);
        }

        #endregion

        #region ContainsAll

        [TestMethod]
        public void ContainsAll_Success()
        {
            AssertUnderTest.ContainsAll(new[] { "blub", "blib" }, "jhfkblibjhfdgblubkjfh");
        }

        [TestMethod]
        public void ContainsAll_Null()
        {
            var nl = Environment.NewLine;
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ContainsAll(new[] { "blub", "blib" }, (string?)null));
            Assert.AreEqual($"Assert.ContainsAll failed. Expected:<[{nl}\tblub,{nl}\tblib{nl}]>. Actual:<(null)>. MissingStrings:<[{nl}\tblub,{nl}\tblib{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void ContainsAll_NoneMatch()
        {
            var nl = Environment.NewLine;
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ContainsAll(new[] { "blub", "blib" }, "kjhgfdkghggfd"));
            Assert.AreEqual($"Assert.ContainsAll failed. Expected:<[{nl}\tblub,{nl}\tblib{nl}]>. Actual:<kjhgfdkghggfd>. MissingStrings:<[{nl}\tblub,{nl}\tblib{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void ContainsAll_OneMatch()
        {
            var nl = Environment.NewLine;
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ContainsAll(new[] { "blub", "blib" }, "kjhgfdkblubghggfd"));
            Assert.AreEqual($"Assert.ContainsAll failed. Expected:<[{nl}\tblub,{nl}\tblib{nl}]>. Actual:<kjhgfdkblubghggfd>. MissingStrings:<[{nl}\tblib{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void ContainsAll_Match_DifferentCasing()
        {
            var nl = Environment.NewLine;
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ContainsAll(new[] { "blub", "blib" }, "kjhgfdBLUBkghgBLIBgfd"));
            Assert.AreEqual($"Assert.ContainsAll failed. Expected:<[{nl}\tblub,{nl}\tblib{nl}]>. Actual:<kjhgfdBLUBkghgBLIBgfd>. MissingStrings:<[{nl}\tblub,{nl}\tblib{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void ContainsAll_WithMessage_Null()
        {
            var nl = Environment.NewLine;
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ContainsAll(new[] { "blub", "blib" }, (string?)null, "This is my test"));
            Assert.AreEqual($"Assert.ContainsAll failed. Expected:<[{nl}\tblub,{nl}\tblib{nl}]>. Actual:<(null)>. MissingStrings:<[{nl}\tblub,{nl}\tblib{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void ContainsAll_WithMessage_NoneMatch()
        {
            var nl = Environment.NewLine;
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ContainsAll(new[] { "blub", "blib" }, "kjhgfdkghggfd", "This is my test"));
            Assert.AreEqual($"Assert.ContainsAll failed. Expected:<[{nl}\tblub,{nl}\tblib{nl}]>. Actual:<kjhgfdkghggfd>. MissingStrings:<[{nl}\tblub,{nl}\tblib{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void ContainsAll_WithMessage_OneMatch()
        {
            var nl = Environment.NewLine;
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ContainsAll(new[] { "blub", "blib" }, "kjhgfdkblubghggfd", "This is my test"));
            Assert.AreEqual($"Assert.ContainsAll failed. Expected:<[{nl}\tblub,{nl}\tblib{nl}]>. Actual:<kjhgfdkblubghggfd>. MissingStrings:<[{nl}\tblib{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void ContainsAll_WithMessage_Match_DifferentCasing()
        {
            var nl = Environment.NewLine;
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ContainsAll(new[] { "blub", "blib" }, "kjhgfdBLUBkghggBLIBfd", "This is my test"));
            Assert.AreEqual($"Assert.ContainsAll failed. Expected:<[{nl}\tblub,{nl}\tblib{nl}]>. Actual:<kjhgfdBLUBkghggBLIBfd>. MissingStrings:<[{nl}\tblub,{nl}\tblib{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void ContainsAll_WithComparison_Success_SameCasing()
        {
            AssertUnderTest.ContainsAll(new[] { "blub", "blib" }, "jhfblibkjhfdgblubkjfh", StringComparison.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void ContainsAll_WithComparison_Success_DifferentCasing()
        {
            AssertUnderTest.ContainsAll(new[] { "blub", "blib" }, "jhfkBLIBjhfdgBLUBkjfh", StringComparison.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void ContainsAll_WithComparison_Null()
        {
            var nl = Environment.NewLine;
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ContainsAll(new[] { "blub", "blib" }, (string?)null, StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual($"Assert.ContainsAll failed. Expected:<[{nl}\tblub,{nl}\tblib{nl}]>. Actual:<(null)>. MissingStrings:<[{nl}\tblub,{nl}\tblib{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void ContainsAll_WithComparison_NoneMatch()
        {
            var nl = Environment.NewLine;
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ContainsAll(new[] { "blub", "blib" }, "kjhgfdkghggfd", StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual($"Assert.ContainsAll failed. Expected:<[{nl}\tblub,{nl}\tblib{nl}]>. Actual:<kjhgfdkghggfd>. MissingStrings:<[{nl}\tblub,{nl}\tblib{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void ContainsAll_WithComparison_OneMatch()
        {
            var nl = Environment.NewLine;
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ContainsAll(new[] { "blub", "blib" }, "kjhgfdkblubghggfd", StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual($"Assert.ContainsAll failed. Expected:<[{nl}\tblub,{nl}\tblib{nl}]>. Actual:<kjhgfdkblubghggfd>. MissingStrings:<[{nl}\tblib{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void ContainsAll_WithComparison_WithMessage_Null()
        {
            var nl = Environment.NewLine;
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ContainsAll(new[] { "blub", "blib" }, (string?)null, StringComparison.OrdinalIgnoreCase, "This is my test"));
            Assert.AreEqual($"Assert.ContainsAll failed. Expected:<[{nl}\tblub,{nl}\tblib{nl}]>. Actual:<(null)>. MissingStrings:<[{nl}\tblub,{nl}\tblib{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void ContainsAll_WithComparison_WithMessage_NoneMatch()
        {
            var nl = Environment.NewLine;
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ContainsAll(new[] { "blub", "blib" }, "kjhgfdkghggfd", StringComparison.OrdinalIgnoreCase, "This is my test"));
            Assert.AreEqual($"Assert.ContainsAll failed. Expected:<[{nl}\tblub,{nl}\tblib{nl}]>. Actual:<kjhgfdkghggfd>. MissingStrings:<[{nl}\tblub,{nl}\tblib{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void ContainsAll_WithComparison_WithMessage_OneMatch()
        {
            var nl = Environment.NewLine;
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ContainsAll(new[] { "blub", "blib" }, "kjhgfdkblubghggfd", StringComparison.OrdinalIgnoreCase, "This is my test"));
            Assert.AreEqual($"Assert.ContainsAll failed. Expected:<[{nl}\tblub,{nl}\tblib{nl}]>. Actual:<kjhgfdkblubghggfd>. MissingStrings:<[{nl}\tblib{nl}]>. This is my test", ex.Message);
        }

        #endregion

        #region DoesNotContain

        [TestMethod]
        public void DoesNotContain_Success_DifferentContent()
        {
            AssertUnderTest.DoesNotContain("blub", "bbb");
        }

        [TestMethod]
        public void DoesNotContain_Success_DifferentCasing()
        {
            AssertUnderTest.DoesNotContain("blub", "BLUB");
        }

        [TestMethod]
        public void DoesNotContain_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("blub", (string?)null));
            Assert.AreEqual("Assert.DoesNotContain failed. NotExpected:<blub>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void DoesNotContain_Fail()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("blub", "jhfkjhfdgblubkjfh"));
            Assert.AreEqual("Assert.DoesNotContain failed. NotExpected:<blub>. Actual:<jhfkjhfdgblubkjfh>.", ex.Message);
        }

        [TestMethod]
        public void DoesNotContain_WithMessage_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("blub", (string?)null, "This is my test"));
            Assert.AreEqual("Assert.DoesNotContain failed. NotExpected:<blub>. Actual:<(null)>. This is my test", ex.Message);
        }

        [TestMethod]
        public void DoesNotContain_WithMessage_Fail()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("blub", "jhfkjhfdgblubkjfh", "This is my test"));
            Assert.AreEqual("Assert.DoesNotContain failed. NotExpected:<blub>. Actual:<jhfkjhfdgblubkjfh>. This is my test", ex.Message);
        }

        [TestMethod]
        public void DoesNotContain_WithComparison_Success_DifferentContent()
        {
            AssertUnderTest.DoesNotContain("blub", "bbb", StringComparison.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void DoesNotContain_WithComparison_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("blub", null, StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual("Assert.DoesNotContain failed. NotExpected:<blub>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void DoesNotContain_WithComparison_Fail_SameCasing()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("blub", "jhfkjhfdgblubkjfh", StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual("Assert.DoesNotContain failed. NotExpected:<blub>. Actual:<jhfkjhfdgblubkjfh>.", ex.Message);
        }

        [TestMethod]
        public void DoesNotContain_WithComparison_Fail_DifferentCasing()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("blub", "jhfkjhfdgBLUBkjfh", StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual("Assert.DoesNotContain failed. NotExpected:<blub>. Actual:<jhfkjhfdgBLUBkjfh>.", ex.Message);
        }

        [TestMethod]
        public void DoesNotContain_WithMessage_WithComparison_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("blub", null, StringComparison.OrdinalIgnoreCase, "This is my test"));
            Assert.AreEqual("Assert.DoesNotContain failed. NotExpected:<blub>. Actual:<(null)>. This is my test", ex.Message);
        }

        [TestMethod]
        public void DoesNotContain_WithMessage_WithComparison_Fail_SameCasing()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("blub", "jhfkjhfdgblubkjfh", StringComparison.OrdinalIgnoreCase, "This is my test"));
            Assert.AreEqual("Assert.DoesNotContain failed. NotExpected:<blub>. Actual:<jhfkjhfdgblubkjfh>. This is my test", ex.Message);
        }

        [TestMethod]
        public void DoesNotContain_WithMessage_WithComparison_Fail_DifferentCasing()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("blub", "jhfkjhfdgBLUBkjfh", StringComparison.OrdinalIgnoreCase, "This is my test"));
            Assert.AreEqual("Assert.DoesNotContain failed. NotExpected:<blub>. Actual:<jhfkjhfdgBLUBkjfh>. This is my test", ex.Message);
        }

        #endregion

        #region StartsWith

        [TestMethod]
        public void StartsWith_Success()
        {
            AssertUnderTest.StartsWith("blub", "blubjhfkjhfdgkjfh");
        }

        [TestMethod]
        public void StartsWith_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.StartsWith("blub", null));
            Assert.AreEqual("Assert.StartsWith failed. Expected:<blub>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void StartsWith_Fail_DifferentContent()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.StartsWith("blub", "bbb"));
            Assert.AreEqual("Assert.StartsWith failed. Expected:<blub>. Actual:<bbb>.", ex.Message);
        }

        [TestMethod]
        public void StartsWith_Fail_DifferentCasing()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.StartsWith("blub", "BLUB"));
            Assert.AreEqual("Assert.StartsWith failed. Expected:<blub>. Actual:<BLUB>.", ex.Message);
        }

        [TestMethod]
        public void StartsWith_WithMessage_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.StartsWith("blub", null, "This is my test"));
            Assert.AreEqual("Assert.StartsWith failed. Expected:<blub>. Actual:<(null)>. This is my test", ex.Message);
        }

        [TestMethod]
        public void StartsWith_WithMessage_Fail_DifferentContent()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.StartsWith("blub", "bbb", "This is my test"));
            Assert.AreEqual("Assert.StartsWith failed. Expected:<blub>. Actual:<bbb>. This is my test", ex.Message);
        }

        [TestMethod]
        public void StartsWith_WithMessage_Fail_DifferentCasing()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.StartsWith("blub", "BLUB", "This is my test"));
            Assert.AreEqual("Assert.StartsWith failed. Expected:<blub>. Actual:<BLUB>. This is my test", ex.Message);
        }

        [TestMethod]
        public void StartsWith_WithComparison_Success_SameCasing()
        {
            AssertUnderTest.StartsWith("blub", "blubjhfkjhfdgkjfh", StringComparison.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void StartsWith_WithComparison_Success_DifferentCasing()
        {
            AssertUnderTest.StartsWith("blub", "BLUBjhfkjhfdgkjfh", StringComparison.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void StartsWith_WithComparison_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.StartsWith("blub", null, StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual("Assert.StartsWith failed. Expected:<blub>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void StartsWith_WithComparison_Fail_DifferentContent()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.StartsWith("blub", "bbb", StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual("Assert.StartsWith failed. Expected:<blub>. Actual:<bbb>.", ex.Message);
        }

        [TestMethod]
        public void StartsWith_WithComparison_WithMessage_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.StartsWith("blub", null, StringComparison.OrdinalIgnoreCase, "This is my test"));
            Assert.AreEqual("Assert.StartsWith failed. Expected:<blub>. Actual:<(null)>. This is my test", ex.Message);
        }

        [TestMethod]
        public void StartsWith_WithComparison_WithMessage_Fail_DifferentContent()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.StartsWith("blub", "bbb", StringComparison.OrdinalIgnoreCase, "This is my test"));
            Assert.AreEqual("Assert.StartsWith failed. Expected:<blub>. Actual:<bbb>. This is my test", ex.Message);
        }

        #endregion

        #region DoesNotStartWith

        [TestMethod]
        public void DoesNotStartWith_Success_DifferentContent()
        {
            AssertUnderTest.DoesNotStartWith("blub", "bbb");
        }

        [TestMethod]
        public void DoesNotStartWith_Success_DifferentCasing()
        {
            AssertUnderTest.DoesNotStartWith("blub", "BLUB");
        }

        [TestMethod]
        public void DoesNotStartWith_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotStartWith("blub", null));
            Assert.AreEqual("Assert.DoesNotStartWith failed. NotExpected:<blub>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void DoesNotStartWith_Fail()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotStartWith("blub", "blubjhfkjhfdgkjfh"));
            Assert.AreEqual("Assert.DoesNotStartWith failed. NotExpected:<blub>. Actual:<blubjhfkjhfdgkjfh>.", ex.Message);
        }

        [TestMethod]
        public void DoesNotStartWith_WithMessage_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotStartWith("blub", null, "This is my test"));
            Assert.AreEqual("Assert.DoesNotStartWith failed. NotExpected:<blub>. Actual:<(null)>. This is my test", ex.Message);
        }

        [TestMethod]
        public void DoesNotStartWith_WithMessage_Fail()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotStartWith("blub", "blubjhfkjhfdgkjfh", "This is my test"));
            Assert.AreEqual("Assert.DoesNotStartWith failed. NotExpected:<blub>. Actual:<blubjhfkjhfdgkjfh>. This is my test", ex.Message);
        }

        [TestMethod]
        public void DoesNotStartWith_WithComparison_Success_DifferentContent()
        {
            AssertUnderTest.DoesNotStartWith("blub", "bbb", StringComparison.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void DoesNotStartWith_WithComparison_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotStartWith("blub", null, StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual("Assert.DoesNotStartWith failed. NotExpected:<blub>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void DoesNotStartWith_WithComparison_Fail_SameCasing()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotStartWith("blub", "blubjhfkjhfdgkjfh", StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual("Assert.DoesNotStartWith failed. NotExpected:<blub>. Actual:<blubjhfkjhfdgkjfh>.", ex.Message);
        }

        [TestMethod]
        public void DoesNotStartWith_WithComparison_Fail_DifferentCasing()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotStartWith("blub", "BLUBjhfkjhfdgkjfh", StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual("Assert.DoesNotStartWith failed. NotExpected:<blub>. Actual:<BLUBjhfkjhfdgkjfh>.", ex.Message);
        }

        [TestMethod]
        public void DoesNotStartWith_WithMessage_WithComparison_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotStartWith("blub", null, StringComparison.OrdinalIgnoreCase, "This is my test"));
            Assert.AreEqual("Assert.DoesNotStartWith failed. NotExpected:<blub>. Actual:<(null)>. This is my test", ex.Message);
        }

        [TestMethod]
        public void DoesNotStartWith_WithMessage_WithComparison_Fail_SameCasing()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotStartWith("blub", "blubjhfkjhfdgkjfh", StringComparison.OrdinalIgnoreCase, "This is my test"));
            Assert.AreEqual("Assert.DoesNotStartWith failed. NotExpected:<blub>. Actual:<blubjhfkjhfdgkjfh>. This is my test", ex.Message);
        }

        [TestMethod]
        public void DoesNotStartWith_WithMessage_WithComparison_Fail_DifferentCasing()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotStartWith("blub", "BLUBjhfkjhfdgkjfh", StringComparison.OrdinalIgnoreCase, "This is my test"));
            Assert.AreEqual("Assert.DoesNotStartWith failed. NotExpected:<blub>. Actual:<BLUBjhfkjhfdgkjfh>. This is my test", ex.Message);
        }

        #endregion

        #region EndsWith

        [TestMethod]
        public void EndsWith_Success()
        {
            AssertUnderTest.EndsWith("blub", "jhfkjhfdgkjfhblub");
        }

        [TestMethod]
        public void EndsWith_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.EndsWith("blub", null));
            Assert.AreEqual("Assert.EndsWith failed. Expected:<blub>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void EndsWith_Fail_DifferentContent()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.EndsWith("blub", "bbb"));
            Assert.AreEqual("Assert.EndsWith failed. Expected:<blub>. Actual:<bbb>.", ex.Message);
        }

        [TestMethod]
        public void EndsWith_Fail_DifferentCasing()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.EndsWith("blub", "BLUB"));
            Assert.AreEqual("Assert.EndsWith failed. Expected:<blub>. Actual:<BLUB>.", ex.Message);
        }

        [TestMethod]
        public void EndsWith_WithMessage_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.EndsWith("blub", null, "This is my test"));
            Assert.AreEqual("Assert.EndsWith failed. Expected:<blub>. Actual:<(null)>. This is my test", ex.Message);
        }

        [TestMethod]
        public void EndsWith_WithMessage_Fail_DifferentContent()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.EndsWith("blub", "bbb", "This is my test"));
            Assert.AreEqual("Assert.EndsWith failed. Expected:<blub>. Actual:<bbb>. This is my test", ex.Message);
        }

        [TestMethod]
        public void EndsWith_WithMessage_Fail_DifferentCasing()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.EndsWith("blub", "BLUB", "This is my test"));
            Assert.AreEqual("Assert.EndsWith failed. Expected:<blub>. Actual:<BLUB>. This is my test", ex.Message);
        }

        [TestMethod]
        public void EndsWith_WithComparison_Success_SameCasing()
        {
            AssertUnderTest.EndsWith("blub", "jhfkjhfdgkjfhblub", StringComparison.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void EndsWith_WithComparison_Success_DifferentCasing()
        {
            AssertUnderTest.EndsWith("blub", "jhfkjhfdgkjfhBLUB", StringComparison.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void EndsWith_WithComparison_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.EndsWith("blub", null, StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual("Assert.EndsWith failed. Expected:<blub>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void EndsWith_WithComparison_Fail_DifferentContent()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.EndsWith("blub", "bbb", StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual("Assert.EndsWith failed. Expected:<blub>. Actual:<bbb>.", ex.Message);
        }

        [TestMethod]
        public void EndsWith_WithComparison_WithMessage_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.EndsWith("blub", null, StringComparison.OrdinalIgnoreCase, "This is my test"));
            Assert.AreEqual("Assert.EndsWith failed. Expected:<blub>. Actual:<(null)>. This is my test", ex.Message);
        }

        [TestMethod]
        public void EndsWith_WithComparison_WithMessage_Fail_DifferentContent()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.EndsWith("blub", "bbb", StringComparison.OrdinalIgnoreCase, "This is my test"));
            Assert.AreEqual("Assert.EndsWith failed. Expected:<blub>. Actual:<bbb>. This is my test", ex.Message);
        }

        #endregion

        #region DoesNotEndWith

        [TestMethod]
        public void DoesNotEndWith_Success_DifferentContent()
        {
            AssertUnderTest.DoesNotEndWith("blub", "bbb");
        }

        [TestMethod]
        public void DoesNotEndWith_Success_DifferentCasing()
        {
            AssertUnderTest.DoesNotEndWith("blub", "BLUB");
        }

        [TestMethod]
        public void DoesNotEndWith_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotEndWith("blub", null));
            Assert.AreEqual("Assert.DoesNotEndWith failed. NotExpected:<blub>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void DoesNotEndWith_Fail()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotEndWith("blub", "jhfkjhfdgkjfhblub"));
            Assert.AreEqual("Assert.DoesNotEndWith failed. NotExpected:<blub>. Actual:<jhfkjhfdgkjfhblub>.", ex.Message);
        }

        [TestMethod]
        public void DoesNotEndWith_WithMessage_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotEndWith("blub", null, "This is my test"));
            Assert.AreEqual("Assert.DoesNotEndWith failed. NotExpected:<blub>. Actual:<(null)>. This is my test", ex.Message);
        }

        [TestMethod]
        public void DoesNotEndWith_WithMessage_Fail()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotEndWith("blub", "jhfkjhfdgkjfhblub", "This is my test"));
            Assert.AreEqual("Assert.DoesNotEndWith failed. NotExpected:<blub>. Actual:<jhfkjhfdgkjfhblub>. This is my test", ex.Message);
        }

        [TestMethod]
        public void DoesNotEndWith_WithComparison_Success_DifferentContent()
        {
            AssertUnderTest.DoesNotEndWith("blub", "bbb", StringComparison.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void DoesNotEndWith_WithComparison_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotEndWith("blub", null, StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual("Assert.DoesNotEndWith failed. NotExpected:<blub>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void DoesNotEndWith_WithComparison_Fail_SameCasing()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotEndWith("blub", "jhfkjhfdgkjfhblub", StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual("Assert.DoesNotEndWith failed. NotExpected:<blub>. Actual:<jhfkjhfdgkjfhblub>.", ex.Message);
        }

        [TestMethod]
        public void DoesNotEndWith_WithComparison_Fail_DifferentCasing()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotEndWith("blub", "jhfkjhfdgkjfhBLUB", StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual("Assert.DoesNotEndWith failed. NotExpected:<blub>. Actual:<jhfkjhfdgkjfhBLUB>.", ex.Message);
        }

        [TestMethod]
        public void DoesNotEndWith_WithMessage_WithComparison_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotEndWith("blub", null, StringComparison.OrdinalIgnoreCase, "This is my test"));
            Assert.AreEqual("Assert.DoesNotEndWith failed. NotExpected:<blub>. Actual:<(null)>. This is my test", ex.Message);
        }

        [TestMethod]
        public void DoesNotEndWith_WithMessage_WithComparison_Fail_SameCasing()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotEndWith("blub", "jhfkjhfdgkjfhblub", StringComparison.OrdinalIgnoreCase, "This is my test"));
            Assert.AreEqual("Assert.DoesNotEndWith failed. NotExpected:<blub>. Actual:<jhfkjhfdgkjfhblub>. This is my test", ex.Message);
        }

        [TestMethod]
        public void DoesNotEndWith_WithMessage_WithComparison_Fail_DifferentCasing()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotEndWith("blub", "jhfkjhfdgkjfhBLUB", StringComparison.OrdinalIgnoreCase, "This is my test"));
            Assert.AreEqual("Assert.DoesNotEndWith failed. NotExpected:<blub>. Actual:<jhfkjhfdgkjfhBLUB>. This is my test", ex.Message);
        }

        #endregion

        #region Matches

        [TestMethod]
        public void Matches_Success()
        {
            AssertUnderTest.Matches(new Regex("^[a-z]+$"), "test");
        }

        [TestMethod]
        public void Matches_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Matches(new Regex("^[a-z]+$"), null));
            Assert.AreEqual("Assert.Matches failed. Expected:<^[a-z]+$>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void Matches_Fail()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Matches(new Regex("^[a-z]+$"), "Test"));
            Assert.AreEqual("Assert.Matches failed. Expected:<^[a-z]+$>. Actual:<Test>.", ex.Message);
        }

        [TestMethod]
        public void Matches_WithMessage_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Matches(new Regex("^[a-z]+$"), null, "This is my test"));
            Assert.AreEqual("Assert.Matches failed. Expected:<^[a-z]+$>. Actual:<(null)>. This is my test", ex.Message);
        }

        [TestMethod]
        public void Matches_WithMessage_Fail()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Matches(new Regex("^[a-z]+$"), "Test", "This is my test"));
            Assert.AreEqual("Assert.Matches failed. Expected:<^[a-z]+$>. Actual:<Test>. This is my test", ex.Message);
        }

        #endregion

        #region DoesNotMatch

        [TestMethod]
        public void DoesNotMatch_Success()
        {
            AssertUnderTest.DoesNotMatch(new Regex("^[a-z]+$"), "TEST");
        }

        [TestMethod]
        public void DoesNotMatch_Null()
        {
            AssertUnderTest.DoesNotMatch(new Regex("^[a-z]+$"), null);
        }

        [TestMethod]
        public void DoesNotMatch_Fail()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotMatch(new Regex("^[a-z]+$"), "test"));
            Assert.AreEqual("Assert.DoesNotMatch failed. NotExpected:<^[a-z]+$>. Actual:<test>.", ex.Message);
        }

        [TestMethod]
        public void DoesNotMatch_WithMessage_Fail()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotMatch(new Regex("^[a-z]+$"), "test", "This is my test"));
            Assert.AreEqual("Assert.DoesNotMatch failed. NotExpected:<^[a-z]+$>. Actual:<test>. This is my test", ex.Message);
        }

        #endregion
    }
}
