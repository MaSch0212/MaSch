using MaSch.Core;
using MaSch.Test.Assertion.UnitTests.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using MSAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace MaSch.Test.Assertion.UnitTests
{
    [TestClass]
    public class ComparableAssertionsTests
    {
        private static MaSch.Test.Assertion.Assert AssertUnderTest => MaSch.Test.Assertion.Assert.Instance;

        #region IsGreaterThan

        [TestMethod]
        public void IsGreaterThan_Success()
        {
            AssertUnderTest.IsGreaterThan(1, 2);
        }

        [TestMethod]
        public void IsGreaterThan_Fail_Smaller()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsGreaterThan(2, 1));
            MSAssert.AreEqual("Assert.IsGreaterThan failed. Expected:<2>. Actual:<1>.", ex.Message);
        }

        [TestMethod]
        public void IsGreaterThan_Fail_Equal()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsGreaterThan(1, 1));
            MSAssert.AreEqual("Assert.IsGreaterThan failed. Expected:<1>. Actual:<1>.", ex.Message);
        }

        [TestMethod]
        public void IsGreaterThan_FailWithMessage_Smaller()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsGreaterThan(2, 1, "This is my test"));
            MSAssert.AreEqual("Assert.IsGreaterThan failed. Expected:<2>. Actual:<1>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsGreaterThan_FailWithMessage_Equal()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsGreaterThan(1, 1, "This is my test"));
            MSAssert.AreEqual("Assert.IsGreaterThan failed. Expected:<1>. Actual:<1>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsGreaterThan_WithComparer_Success()
        {
            using var comparer = CreateComparer(1, 2, 1);
            AssertUnderTest.IsGreaterThan(2, 1, comparer);
        }

        [TestMethod]
        public void IsGreaterThan_WithComparer_Fail_Smaller()
        {
            using var comparer = CreateComparer(2, 1, -1);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsGreaterThan(1, 2, comparer));
            MSAssert.AreEqual("Assert.IsGreaterThan failed. Expected:<1>. Actual:<2>.", ex.Message);
        }

        [TestMethod]
        public void IsGreaterThan_WithComparer_Fail_Equal()
        {
            using var comparer = CreateComparer(1, 1, 0);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsGreaterThan(1, 1, comparer));
            MSAssert.AreEqual("Assert.IsGreaterThan failed. Expected:<1>. Actual:<1>.", ex.Message);
        }

        [TestMethod]
        public void IsGreaterThan_WithComparer_FailWithMessage_Smaller()
        {
            using var comparer = CreateComparer(2, 1, -1);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsGreaterThan(1, 2, comparer, "This is my test"));
            MSAssert.AreEqual("Assert.IsGreaterThan failed. Expected:<1>. Actual:<2>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsGreaterThan_WithComparer_FailWithMessage_Equal()
        {
            using var comparer = CreateComparer(1, 1, 0);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsGreaterThan(1, 1, comparer, "This is my test"));
            MSAssert.AreEqual("Assert.IsGreaterThan failed. Expected:<1>. Actual:<1>. This is my test", ex.Message);
        }

        #endregion

        #region IsGreaterThanOrEqualTo

        [TestMethod]
        public void IsGreaterThanOrEqualTo_Success_Equal()
        {
            AssertUnderTest.IsGreaterThanOrEqualTo(1, 1);
        }

        [TestMethod]
        public void IsGreaterThanOrEqualTo_Success_Greater()
        {
            AssertUnderTest.IsGreaterThanOrEqualTo(1, 2);
        }

        [TestMethod]
        public void IsGreaterThanOrEqualTo_Fail()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsGreaterThanOrEqualTo(2, 1));
            MSAssert.AreEqual("Assert.IsGreaterThanOrEqualTo failed. Expected:<2>. Actual:<1>.", ex.Message);
        }

        [TestMethod]
        public void IsGreaterThanOrEqualTo_FailWithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsGreaterThanOrEqualTo(2, 1, "This is my test"));
            MSAssert.AreEqual("Assert.IsGreaterThanOrEqualTo failed. Expected:<2>. Actual:<1>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsGreaterThanOrEqualTo_WithComparer_Success_Equal()
        {
            using var comparer = CreateComparer(1, 2, 0);
            AssertUnderTest.IsGreaterThanOrEqualTo(2, 1, comparer);
        }

        [TestMethod]
        public void IsGreaterThanOrEqualTo_WithComparer_Success_Greater()
        {
            using var comparer = CreateComparer(1, 2, 1);
            AssertUnderTest.IsGreaterThanOrEqualTo(2, 1, comparer);
        }

        [TestMethod]
        public void IsGreaterThanOrEqualTo_WithComparer_Fail()
        {
            using var comparer = CreateComparer(2, 1, -1);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsGreaterThanOrEqualTo(1, 2, comparer));
            MSAssert.AreEqual("Assert.IsGreaterThanOrEqualTo failed. Expected:<1>. Actual:<2>.", ex.Message);
        }

        [TestMethod]
        public void IsGreaterThanOrEqualTo_WithComparer_FailWithMessage()
        {
            using var comparer = CreateComparer(2, 1, -1);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsGreaterThanOrEqualTo(1, 2, comparer, "This is my test"));
            MSAssert.AreEqual("Assert.IsGreaterThanOrEqualTo failed. Expected:<1>. Actual:<2>. This is my test", ex.Message);
        }

        #endregion

        #region IsSmallerThan

        [TestMethod]
        public void IsSmallerThan_Success()
        {
            AssertUnderTest.IsSmallerThan(2, 1);
        }

        [TestMethod]
        public void IsSmallerThan_Fail_Greater()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsSmallerThan(1, 2));
            MSAssert.AreEqual("Assert.IsSmallerThan failed. Expected:<1>. Actual:<2>.", ex.Message);
        }

        [TestMethod]
        public void IsSmallerThan_Fail_Equal()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsSmallerThan(1, 1));
            MSAssert.AreEqual("Assert.IsSmallerThan failed. Expected:<1>. Actual:<1>.", ex.Message);
        }

        [TestMethod]
        public void IsSmallerThan_FailWithMessage_Greater()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsSmallerThan(1, 2, "This is my test"));
            MSAssert.AreEqual("Assert.IsSmallerThan failed. Expected:<1>. Actual:<2>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsSmallerThan_FailWithMessage_Equal()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsSmallerThan(1, 1, "This is my test"));
            MSAssert.AreEqual("Assert.IsSmallerThan failed. Expected:<1>. Actual:<1>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsSmallerThan_WithComparer_Success()
        {
            using var comparer = CreateComparer(2, 1, -1);
            AssertUnderTest.IsSmallerThan(1, 2, comparer);
        }

        [TestMethod]
        public void IsSmallerThan_WithComparer_Fail_Greater()
        {
            using var comparer = CreateComparer(1, 2, 1);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsSmallerThan(2, 1, comparer));
            MSAssert.AreEqual("Assert.IsSmallerThan failed. Expected:<2>. Actual:<1>.", ex.Message);
        }

        [TestMethod]
        public void IsSmallerThan_WithComparer_Fail_Equal()
        {
            using var comparer = CreateComparer(1, 1, 0);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsSmallerThan(1, 1, comparer));
            MSAssert.AreEqual("Assert.IsSmallerThan failed. Expected:<1>. Actual:<1>.", ex.Message);
        }

        [TestMethod]
        public void IsSmallerThan_WithComparer_FailWithMessage_Greater()
        {
            using var comparer = CreateComparer(1, 2, 1);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsSmallerThan(2, 1, comparer, "This is my test"));
            MSAssert.AreEqual("Assert.IsSmallerThan failed. Expected:<2>. Actual:<1>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsSmallerThan_WithComparer_FailWithMessage_Equal()
        {
            using var comparer = CreateComparer(1, 1, 0);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsSmallerThan(1, 1, comparer, "This is my test"));
            MSAssert.AreEqual("Assert.IsSmallerThan failed. Expected:<1>. Actual:<1>. This is my test", ex.Message);
        }

        #endregion

        #region IsSmallerThanOrEqualTo

        [TestMethod]
        public void IsSmallerThanOrEqualTo_Success_Equal()
        {
            AssertUnderTest.IsSmallerThanOrEqualTo(1, 1);
        }

        [TestMethod]
        public void IsSmallerThanOrEqualTo_Success_Smaller()
        {
            AssertUnderTest.IsSmallerThanOrEqualTo(2, 1);
        }

        [TestMethod]
        public void IsSmallerThanOrEqualTo_Fail()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsSmallerThanOrEqualTo(1, 2));
            MSAssert.AreEqual("Assert.IsSmallerThanOrEqualTo failed. Expected:<1>. Actual:<2>.", ex.Message);
        }

        [TestMethod]
        public void IsSmallerThanOrEqualTo_FailWithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsSmallerThanOrEqualTo(1, 2, "This is my test"));
            MSAssert.AreEqual("Assert.IsSmallerThanOrEqualTo failed. Expected:<1>. Actual:<2>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsSmallerThanOrEqualTo_WithComparer_Success_Equal()
        {
            using var comparer = CreateComparer(2, 1, 0);
            AssertUnderTest.IsSmallerThanOrEqualTo(1, 2, comparer);
        }

        [TestMethod]
        public void IsSmallerThanOrEqualTo_WithComparer_Success_Smaller()
        {
            using var comparer = CreateComparer(2, 1, -1);
            AssertUnderTest.IsSmallerThanOrEqualTo(1, 2, comparer);
        }

        [TestMethod]
        public void IsSmallerThanOrEqualTo_WithComparer_Fail()
        {
            using var comparer = CreateComparer(1, 2, 1);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsSmallerThanOrEqualTo(2, 1, comparer));
            MSAssert.AreEqual("Assert.IsSmallerThanOrEqualTo failed. Expected:<2>. Actual:<1>.", ex.Message);
        }

        [TestMethod]
        public void IsSmallerThanOrEqualTo_WithComparer_FailWithMessage()
        {
            using var comparer = CreateComparer(1, 2, 1);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsSmallerThanOrEqualTo(2, 1, comparer, "This is my test"));
            MSAssert.AreEqual("Assert.IsSmallerThanOrEqualTo failed. Expected:<2>. Actual:<1>. This is my test", ex.Message);
        }

        #endregion

        #region IsBetween

        [TestMethod]
        public void IsBetween_Success()
        {
            AssertUnderTest.IsBetween(1, 3, 2);
        }

        [TestMethod]
        public void IsBetween_Fail_EqualMin()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 1));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<1>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_Fail_SmallerMin()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 0));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<0>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_Fail_EqualMax()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 3));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<3>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_Fail_GreaterMax()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 4));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<4>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_FailWithMessage_EqualMin()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 1, "This is my test"));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<1>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_FailWithMessage_SmallerMin()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 0, "This is my test"));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<0>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_FailWithMessage_EqualMax()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 3, "This is my test"));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<3>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_FailWithMessage_GreaterMax()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 4, "This is my test"));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<4>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_Success()
        {
            using var comparer = CreateBetweenComparer(2);
            AssertUnderTest.IsBetween(1, 3, 2, comparer);
        }

        [TestMethod]
        public void IsBetween_WithComparer_Fail_EqualMin()
        {
            using var comparer = CreateBetweenComparer(1);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 1, comparer));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<1>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_Fail_SmallerMin()
        {
            using var comparer = CreateBetweenComparer(0);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 0, comparer));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<0>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_Fail_EqualMax()
        {
            using var comparer = CreateBetweenComparer(3);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 3, comparer));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<3>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_Fail_GreaterMax()
        {
            using var comparer = CreateBetweenComparer(4);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 4, comparer));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<4>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_FailWithMessage_EqualMin()
        {
            using var comparer = CreateBetweenComparer(1);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 1, comparer, "This is my test"));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<1>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_FailWithMessage_SmallerMin()
        {
            using var comparer = CreateBetweenComparer(0);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 0, comparer, "This is my test"));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<0>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_FailWithMessage_EqualMax()
        {
            using var comparer = CreateBetweenComparer(3);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 3, comparer, "This is my test"));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<3>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_FailWithMessage_GreaterMax()
        {
            using var comparer = CreateBetweenComparer(4);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 4, comparer, "This is my test"));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<4>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_IncludeMin_Success_GreaterMin()
        {
            AssertUnderTest.IsBetween(1, 3, 2, true, false);
        }

        [TestMethod]
        public void IsBetween_IncludeMin_Success_EqualMin()
        {
            AssertUnderTest.IsBetween(1, 3, 1, true, false);
        }

        [TestMethod]
        public void IsBetween_IncludeMin_Fail_SmallerMin()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 0, true, false));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<0>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_IncludeMin_Fail_EqualMax()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 3, true, false));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<3>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_IncludeMin_Fail_GreaterMax()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 4, true, false));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<4>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_IncludeMin_FailWithMessage_SmallerMin()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 0, true, false, "This is my test"));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<0>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_IncludeMin_FailWithMessage_EqualMax()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 3, true, false, "This is my test"));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<3>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_IncludeMin_FailWithMessage_GreaterMax()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 4, true, false, "This is my test"));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<4>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMin_Success_GreaterMin()
        {
            using var comparer = CreateBetweenComparer(2);
            AssertUnderTest.IsBetween(1, 3, 2, true, false, comparer);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMin_Success_EqualMin()
        {
            using var comparer = CreateBetweenComparer(1);
            AssertUnderTest.IsBetween(1, 3, 1, true, false, comparer);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMin_Fail_SmallerMin()
        {
            using var comparer = CreateBetweenComparer(0);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 0, true, false, comparer));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<0>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMin_Fail_EqualMax()
        {
            using var comparer = CreateBetweenComparer(3);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 3, true, false, comparer));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<3>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMin_Fail_GreaterMax()
        {
            using var comparer = CreateBetweenComparer(4);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 4, true, false, comparer));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<4>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMin_FailWithMessage_SmallerMin()
        {
            using var comparer = CreateBetweenComparer(0);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 0, true, false, comparer, "This is my test"));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<0>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMin_FailWithMessage_EqualMax()
        {
            using var comparer = CreateBetweenComparer(3);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 3, true, false, comparer, "This is my test"));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<3>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMin_FailWithMessage_GreaterMax()
        {
            using var comparer = CreateBetweenComparer(4);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 4, true, false, comparer, "This is my test"));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<4>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_IncludeMax_Success_SmallerMax()
        {
            AssertUnderTest.IsBetween(1, 3, 2, false, true);
        }

        [TestMethod]
        public void IsBetween_IncludeMax_Success_EqualMax()
        {
            AssertUnderTest.IsBetween(1, 3, 3, false, true);
        }

        [TestMethod]
        public void IsBetween_IncludeMin_Fail_EqualMin()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 1, false, true));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<1>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_IncludeMax_Fail_SmallerMin()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 0, false, true));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<0>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_IncludeMax_Fail_GreaterMax()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 4, false, true));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<4>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_IncludeMax_FailWithMessage_EqualMin()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 1, false, true, "This is my test"));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<1>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_IncludeMax_FailWithMessage_SmallerMin()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 0, false, true, "This is my test"));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<0>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_IncludeMax_FailWithMessage_GreaterMax()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 4, false, true, "This is my test"));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<4>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMax_Success_SmallerMax()
        {
            using var comparer = CreateBetweenComparer(2);
            AssertUnderTest.IsBetween(1, 3, 2, false, true, comparer);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMax_Success_EqualMax()
        {
            using var comparer = CreateBetweenComparer(3);
            AssertUnderTest.IsBetween(1, 3, 3, false, true, comparer);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMax_Fail_EqualMin()
        {
            using var comparer = CreateBetweenComparer(1);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 1, false, true, comparer));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<1>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMax_Fail_SmallerMin()
        {
            using var comparer = CreateBetweenComparer(0);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 0, false, true, comparer));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<0>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMax_Fail_GreaterMax()
        {
            using var comparer = CreateBetweenComparer(4);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 4, false, true, comparer));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<4>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMax_FailWithMessage_EqualMin()
        {
            using var comparer = CreateBetweenComparer(1);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 1, false, true, comparer, "This is my test"));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<1>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMax_FailWithMessage_SmallerMin()
        {
            using var comparer = CreateBetweenComparer(0);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 0, false, true, comparer, "This is my test"));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<0>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMax_FailWithMessage_GreaterMax()
        {
            using var comparer = CreateBetweenComparer(4);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 4, false, true, comparer, "This is my test"));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<4>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_IncludeMinAndMax_Success_SmallerMax_GreaterMin()
        {
            AssertUnderTest.IsBetween(1, 3, 2, true, true);
        }

        [TestMethod]
        public void IsBetween_IncludeMinAndMax_Success_EqualMin()
        {
            AssertUnderTest.IsBetween(1, 3, 1, true, true);
        }

        [TestMethod]
        public void IsBetween_IncludeMinAndMax_Success_EqualMax()
        {
            AssertUnderTest.IsBetween(1, 3, 3, true, true);
        }

        [TestMethod]
        public void IsBetween_IncludeMaxAndMax_Fail_SmallerMin()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 0, true, true));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<0>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_IncludeMaxAndMax_Fail_GreaterMax()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 4, true, true));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<4>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_IncludeMaxAndMax_FailWithMessage_SmallerMin()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 0, true, true, "This is my test"));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<0>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_IncludeMaxAndMax_FailWithMessage_GreaterMax()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 4, true, true, "This is my test"));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<4>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMinAndMax_Success_SmallerMax_GreaterMin()
        {
            using var comparer = CreateBetweenComparer(2);
            AssertUnderTest.IsBetween(1, 3, 2, true, true, comparer);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMinAndMax_Success_EqualMin()
        {
            using var comparer = CreateBetweenComparer(1);
            AssertUnderTest.IsBetween(1, 3, 1, true, true, comparer);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMinAndMax_Success_EqualMax()
        {
            using var comparer = CreateBetweenComparer(3);
            AssertUnderTest.IsBetween(1, 3, 3, true, true, comparer);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMaxAndMax_Fail_SmallerMin()
        {
            using var comparer = CreateBetweenComparer(0);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 0, true, true, comparer));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<0>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMaxAndMax_Fail_GreaterMax()
        {
            using var comparer = CreateBetweenComparer(4);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 4, true, true, comparer));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<4>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMaxAndMax_FailWithMessage_SmallerMin()
        {
            using var comparer = CreateBetweenComparer(0);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 0, true, true, comparer, "This is my test"));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<0>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMaxAndMax_FailWithMessage_GreaterMax()
        {
            using var comparer = CreateBetweenComparer(4);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsBetween(1, 3, 4, true, true, comparer, "This is my test"));
            MSAssert.AreEqual("Assert.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<4>. This is my test", ex.Message);
        }

        #endregion

        #region IsNotBetween

        [TestMethod]
        public void IsNotBetween_Success_EqualMin()
        {
            AssertUnderTest.IsNotBetween(1, 3, 1);
        }

        [TestMethod]
        public void IsNotBetween_Success_SmallerMin()
        {
            AssertUnderTest.IsNotBetween(1, 3, 0);
        }

        [TestMethod]
        public void IsNotBetween_Success_EqualMax()
        {
            AssertUnderTest.IsNotBetween(1, 3, 3);
        }

        [TestMethod]
        public void IsNotBetween_Success_GreaterMax()
        {
            AssertUnderTest.IsNotBetween(1, 3, 4);
        }

        [TestMethod]
        public void IsNotBetween_Fail()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotBetween(1, 3, 2));
            MSAssert.AreEqual("Assert.IsNotBetween failed. NotExpectedMin:<1>. NotExpectedMax:<3>. Actual:<2>.", ex.Message);
        }

        [TestMethod]
        public void IsNotBetween_FailWithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotBetween(1, 3, 2, "This is my test"));
            MSAssert.AreEqual("Assert.IsNotBetween failed. NotExpectedMin:<1>. NotExpectedMax:<3>. Actual:<2>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsNotBetween_WithComparer_Success_EqualMin()
        {
            using var comparer = CreateBetweenComparer(1);
            AssertUnderTest.IsNotBetween(1, 3, 1, comparer);
        }

        [TestMethod]
        public void IsNotBetween_WithComparer_Success_SmallerMin()
        {
            using var comparer = CreateBetweenComparer(0);
            AssertUnderTest.IsNotBetween(1, 3, 0, comparer);
        }

        [TestMethod]
        public void IsNotBetween_WithComparer_Success_EqualMax()
        {
            using var comparer = CreateBetweenComparer(3);
            AssertUnderTest.IsNotBetween(1, 3, 3, comparer);
        }

        [TestMethod]
        public void IsNotBetween_WithComparer_Success_GreaterMax()
        {
            using var comparer = CreateBetweenComparer(4);
            AssertUnderTest.IsNotBetween(1, 3, 4, comparer);
        }

        [TestMethod]
        public void IsNotBetween_WithComparer_Fail()
        {
            using var comparer = CreateBetweenComparer(2);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotBetween(1, 3, 2, comparer));
            MSAssert.AreEqual("Assert.IsNotBetween failed. NotExpectedMin:<1>. NotExpectedMax:<3>. Actual:<2>.", ex.Message);
        }

        [TestMethod]
        public void IsNotBetween_WithComparer_FailWithMessage()
        {
            using var comparer = CreateBetweenComparer(2);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotBetween(1, 3, 2, comparer, "This is my test"));
            MSAssert.AreEqual("Assert.IsNotBetween failed. NotExpectedMin:<1>. NotExpectedMax:<3>. Actual:<2>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsNotBetween_IncludeMin_Success_SmallerMin()
        {
            AssertUnderTest.IsNotBetween(1, 3, 0, true, false);
        }

        [TestMethod]
        public void IsNotBetween_IncludeMin_Success_EqualMax()
        {
            AssertUnderTest.IsNotBetween(1, 3, 3, true, false);
        }

        [TestMethod]
        public void IsNotBetween_IncludeMin_Success_GreaterMax()
        {
            AssertUnderTest.IsNotBetween(1, 3, 4, true, false);
        }

        [TestMethod]
        public void IsNotBetween_IncludeMin_Fail_GreaterMin()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotBetween(1, 3, 2, true, false));
            MSAssert.AreEqual("Assert.IsNotBetween failed. NotExpectedMin:<1>. NotExpectedMax:<3>. Actual:<2>.", ex.Message);
        }

        [TestMethod]
        public void IsNotBetween_IncludeMin_Fail_EqualMin()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotBetween(1, 3, 1, true, false));
            MSAssert.AreEqual("Assert.IsNotBetween failed. NotExpectedMin:<1>. NotExpectedMax:<3>. Actual:<1>.", ex.Message);
        }

        [TestMethod]
        public void IsNotBetween_IncludeMin_FailWithMessage_GreaterMin()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotBetween(1, 3, 2, true, false, "This is my test"));
            MSAssert.AreEqual("Assert.IsNotBetween failed. NotExpectedMin:<1>. NotExpectedMax:<3>. Actual:<2>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsNotBetween_IncludeMin_FailWithMessage_EqualMin()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotBetween(1, 3, 1, true, false, "This is my test"));
            MSAssert.AreEqual("Assert.IsNotBetween failed. NotExpectedMin:<1>. NotExpectedMax:<3>. Actual:<1>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsNotBetween_WithComparer_IncludeMin_Success_SmallerMin()
        {
            using var comparer = CreateBetweenComparer(0);
            AssertUnderTest.IsNotBetween(1, 3, 0, true, false, comparer);
        }

        [TestMethod]
        public void IsNotBetween_WithComparer_IncludeMin_Success_EqualMax()
        {
            using var comparer = CreateBetweenComparer(3);
            AssertUnderTest.IsNotBetween(1, 3, 3, true, false, comparer);
        }

        [TestMethod]
        public void IsNotBetween_WithComparer_IncludeMin_Success_GreaterMax()
        {
            using var comparer = CreateBetweenComparer(4);
            AssertUnderTest.IsNotBetween(1, 3, 4, true, false, comparer);
        }

        [TestMethod]
        public void IsNotBetween_WithComparer_IncludeMin_Fail_GreaterMin()
        {
            using var comparer = CreateBetweenComparer(2);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotBetween(1, 3, 2, true, false, comparer));
            MSAssert.AreEqual("Assert.IsNotBetween failed. NotExpectedMin:<1>. NotExpectedMax:<3>. Actual:<2>.", ex.Message);
        }

        [TestMethod]
        public void IsNotBetween_WithComparer_IncludeMin_Fail_EqualMin()
        {
            using var comparer = CreateBetweenComparer(1);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotBetween(1, 3, 1, true, false, comparer));
            MSAssert.AreEqual("Assert.IsNotBetween failed. NotExpectedMin:<1>. NotExpectedMax:<3>. Actual:<1>.", ex.Message);
        }

        [TestMethod]
        public void IsNotBetween_WithComparer_IncludeMin_FailWithMessage_GreaterMin()
        {
            using var comparer = CreateBetweenComparer(2);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotBetween(1, 3, 2, true, false, comparer, "This is my test"));
            MSAssert.AreEqual("Assert.IsNotBetween failed. NotExpectedMin:<1>. NotExpectedMax:<3>. Actual:<2>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsNotBetween_WithComparer_IncludeMin_FailWithMessage_EqualMin()
        {
            using var comparer = CreateBetweenComparer(1);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotBetween(1, 3, 1, true, false, comparer, "This is my test"));
            MSAssert.AreEqual("Assert.IsNotBetween failed. NotExpectedMin:<1>. NotExpectedMax:<3>. Actual:<1>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsNotBetween_IncludeMin_Success_EqualMin()
        {
            AssertUnderTest.IsNotBetween(1, 3, 1, false, true);
        }

        [TestMethod]
        public void IsNotBetween_IncludeMax_Success_SmallerMin()
        {
            AssertUnderTest.IsNotBetween(1, 3, 0, false, true);
        }

        [TestMethod]
        public void IsNotBetween_IncludeMax_Success_GreaterMax()
        {
            AssertUnderTest.IsNotBetween(1, 3, 4, false, true);
        }

        [TestMethod]
        public void IsNotBetween_IncludeMax_Fail_SmallerMax()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotBetween(1, 3, 2, false, true));
            MSAssert.AreEqual("Assert.IsNotBetween failed. NotExpectedMin:<1>. NotExpectedMax:<3>. Actual:<2>.", ex.Message);
        }

        [TestMethod]
        public void IsNotBetween_IncludeMax_Fail_EqualMax()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotBetween(1, 3, 3, false, true));
            MSAssert.AreEqual("Assert.IsNotBetween failed. NotExpectedMin:<1>. NotExpectedMax:<3>. Actual:<3>.", ex.Message);
        }

        [TestMethod]
        public void IsNotBetween_IncludeMax_FailWithMessage_SmallerMax()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotBetween(1, 3, 2, false, true, "This is my test"));
            MSAssert.AreEqual("Assert.IsNotBetween failed. NotExpectedMin:<1>. NotExpectedMax:<3>. Actual:<2>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsNotBetween_IncludeMax_FailWithMessage_EqualMax()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotBetween(1, 3, 3, false, true, "This is my test"));
            MSAssert.AreEqual("Assert.IsNotBetween failed. NotExpectedMin:<1>. NotExpectedMax:<3>. Actual:<3>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsNotBetween_WithComparer_IncludeMax_Success_EqualMin()
        {
            using var comparer = CreateBetweenComparer(1);
            AssertUnderTest.IsNotBetween(1, 3, 1, false, true, comparer);
        }

        [TestMethod]
        public void IsNotBetween_WithComparer_IncludeMax_Success_SmallerMin()
        {
            using var comparer = CreateBetweenComparer(0);
            AssertUnderTest.IsNotBetween(1, 3, 0, false, true, comparer);
        }

        [TestMethod]
        public void IsNotBetween_WithComparer_IncludeMax_Success_GreaterMax()
        {
            using var comparer = CreateBetweenComparer(4);
            AssertUnderTest.IsNotBetween(1, 3, 4, false, true, comparer);
        }

        [TestMethod]
        public void IsNotBetween_WithComparer_IncludeMax_Fail_SmallerMax()
        {
            using var comparer = CreateBetweenComparer(2);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotBetween(1, 3, 2, false, true, comparer));
            MSAssert.AreEqual("Assert.IsNotBetween failed. NotExpectedMin:<1>. NotExpectedMax:<3>. Actual:<2>.", ex.Message);
        }

        [TestMethod]
        public void IsNotBetween_WithComparer_IncludeMax_Fail_EqualMax()
        {
            using var comparer = CreateBetweenComparer(3);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotBetween(1, 3, 3, false, true, comparer));
            MSAssert.AreEqual("Assert.IsNotBetween failed. NotExpectedMin:<1>. NotExpectedMax:<3>. Actual:<3>.", ex.Message);
        }

        [TestMethod]
        public void IsNotBetween_WithComparer_IncludeMax_FailWithMessage_SmallerMax()
        {
            using var comparer = CreateBetweenComparer(2);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotBetween(1, 3, 2, false, true, comparer, "This is my test"));
            MSAssert.AreEqual("Assert.IsNotBetween failed. NotExpectedMin:<1>. NotExpectedMax:<3>. Actual:<2>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsNotBetween_WithComparer_IncludeMax_FailWithMessage_EqualMax()
        {
            using var comparer = CreateBetweenComparer(3);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotBetween(1, 3, 3, false, true, comparer, "This is my test"));
            MSAssert.AreEqual("Assert.IsNotBetween failed. NotExpectedMin:<1>. NotExpectedMax:<3>. Actual:<3>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsNotBetween_IncludeMaxAndMax_Success_SmallerMin()
        {
            AssertUnderTest.IsNotBetween(1, 3, 0, true, true);
        }

        [TestMethod]
        public void IsNotBetween_IncludeMaxAndMax_Success_GreaterMax()
        {
            AssertUnderTest.IsNotBetween(1, 3, 4, true, true);
        }

        [TestMethod]
        public void IsNotBetween_IncludeMinAndMax_Fail_SmallerMax_GreaterMin()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotBetween(1, 3, 2, true, true));
            MSAssert.AreEqual("Assert.IsNotBetween failed. NotExpectedMin:<1>. NotExpectedMax:<3>. Actual:<2>.", ex.Message);
        }

        [TestMethod]
        public void IsNotBetween_IncludeMinAndMax_Fail_EqualMin()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotBetween(1, 3, 1, true, true));
            MSAssert.AreEqual("Assert.IsNotBetween failed. NotExpectedMin:<1>. NotExpectedMax:<3>. Actual:<1>.", ex.Message);
        }

        [TestMethod]
        public void IsNotBetween_IncludeMinAndMax_Fail_EqualMax()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotBetween(1, 3, 3, true, true));
            MSAssert.AreEqual("Assert.IsNotBetween failed. NotExpectedMin:<1>. NotExpectedMax:<3>. Actual:<3>.", ex.Message);
        }

        [TestMethod]
        public void IsNotBetween_IncludeMinAndMax_FailWithMessage_SmallerMax_GreaterMin()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotBetween(1, 3, 2, true, true, "This is my test"));
            MSAssert.AreEqual("Assert.IsNotBetween failed. NotExpectedMin:<1>. NotExpectedMax:<3>. Actual:<2>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsNotBetween_IncludeMinAndMax_FailWithMessage_EqualMin()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotBetween(1, 3, 1, true, true, "This is my test"));
            MSAssert.AreEqual("Assert.IsNotBetween failed. NotExpectedMin:<1>. NotExpectedMax:<3>. Actual:<1>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsNotBetween_IncludeMinAndMax_FailWithMessage_EqualMax()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotBetween(1, 3, 3, true, true, "This is my test"));
            MSAssert.AreEqual("Assert.IsNotBetween failed. NotExpectedMin:<1>. NotExpectedMax:<3>. Actual:<3>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsNotBetween_WithComparer_IncludeMaxAndMax_Success_SmallerMin()
        {
            using var comparer = CreateBetweenComparer(0);
            AssertUnderTest.IsNotBetween(1, 3, 0, true, true, comparer);
        }

        [TestMethod]
        public void IsNotBetween_WithComparer_IncludeMaxAndMax_Success_GreaterMax()
        {
            using var comparer = CreateBetweenComparer(4);
            AssertUnderTest.IsNotBetween(1, 3, 4, true, true, comparer);
        }

        [TestMethod]
        public void IsNotBetween_WithComparer_IncludeMinAndMax_Fail_SmallerMax_GreaterMin()
        {
            using var comparer = CreateBetweenComparer(2);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotBetween(1, 3, 2, true, true, comparer));
            MSAssert.AreEqual("Assert.IsNotBetween failed. NotExpectedMin:<1>. NotExpectedMax:<3>. Actual:<2>.", ex.Message);
        }

        [TestMethod]
        public void IsNotBetween_WithComparer_IncludeMinAndMax_Fail_EqualMin()
        {
            using var comparer = CreateBetweenComparer(1);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotBetween(1, 3, 1, true, true, comparer));
            MSAssert.AreEqual("Assert.IsNotBetween failed. NotExpectedMin:<1>. NotExpectedMax:<3>. Actual:<1>.", ex.Message);
        }

        [TestMethod]
        public void IsNotBetween_WithComparer_IncludeMinAndMax_Fail_EqualMax()
        {
            using var comparer = CreateBetweenComparer(3);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotBetween(1, 3, 3, true, true, comparer));
            MSAssert.AreEqual("Assert.IsNotBetween failed. NotExpectedMin:<1>. NotExpectedMax:<3>. Actual:<3>.", ex.Message);
        }

        [TestMethod]
        public void IsNotBetween_WithComparer_IncludeMinAndMax_FailWithMessage_SmallerMax_GreaterMin()
        {
            using var comparer = CreateBetweenComparer(2);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotBetween(1, 3, 2, true, true, comparer, "This is my test"));
            MSAssert.AreEqual("Assert.IsNotBetween failed. NotExpectedMin:<1>. NotExpectedMax:<3>. Actual:<2>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsNotBetween_WithComparer_IncludeMinAndMax_FailWithMessage_EqualMin()
        {
            using var comparer = CreateBetweenComparer(1);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotBetween(1, 3, 1, true, true, comparer, "This is my test"));
            MSAssert.AreEqual("Assert.IsNotBetween failed. NotExpectedMin:<1>. NotExpectedMax:<3>. Actual:<1>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsNotBetween_WithComparer_IncludeMinAndMax_FailWithMessage_EqualMax()
        {
            using var comparer = CreateBetweenComparer(3);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotBetween(1, 3, 3, true, true, comparer, "This is my test"));
            MSAssert.AreEqual("Assert.IsNotBetween failed. NotExpectedMin:<1>. NotExpectedMax:<3>. Actual:<3>. This is my test", ex.Message);
        }

        #endregion

        private static DisposableComparer<int> CreateComparer(int x, int y, int result)
        {
            var comparerMock = new Mock<IComparer<int>>(MockBehavior.Strict);
            comparerMock.Setup(m => m.Compare(x, y)).Returns(result);

            return new DisposableComparer<int>(
                comparerMock.Object,
                new ActionOnDispose(() => comparerMock.Verify(m => m.Compare(x, y), Times.Once())));
        }

        private static DisposableComparer<int> CreateBetweenComparer(int value)
        {
            var comparerMock = new Mock<IComparer<int>>(MockBehavior.Strict);
            comparerMock.Setup(m => m.Compare(value, It.IsAny<int>())).Returns<int, int>((x, y) => x.CompareTo(y));

            return new DisposableComparer<int>(
                comparerMock.Object,
                new ActionOnDispose(() =>
                {
                    comparerMock.Verify(m => m.Compare(value, 1), Times.Once());
                    comparerMock.Verify(m => m.Compare(value, 3), Times.Once());
                }));
        }
    }
}
