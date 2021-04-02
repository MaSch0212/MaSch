using MaSch.Core;
using MaSch.Test.Components.Test.TestHelper;
using MaSch.Test.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace MaSch.Test.Components.Test.Extensions
{
    [TestClass]
    public class AssertExtensionsTests
    {
        #region IsGreaterThan
        [TestMethod]
        public void IsGreaterThan_Success()
        {
            Assert.That.IsGreaterThan(1, 2);
        }

        [TestMethod]
        public void IsGreaterThan_Fail_Smaller()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsGreaterThan(2, 1));
            Assert.AreEqual("Assert.That.IsGreaterThan failed. Expected:<2>. Actual:<1>.", ex.Message);
        }

        [TestMethod]
        public void IsGreaterThan_Fail_Equal()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsGreaterThan(1, 1));
            Assert.AreEqual("Assert.That.IsGreaterThan failed. Expected:<1>. Actual:<1>.", ex.Message);
        }

        [TestMethod]
        public void IsGreaterThan_FailWithMessage_Smaller()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsGreaterThan(2, 1, "This is my test"));
            Assert.AreEqual("Assert.That.IsGreaterThan failed. Expected:<2>. Actual:<1>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsGreaterThan_FailWithMessage_Equal()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsGreaterThan(1, 1, "This is my test"));
            Assert.AreEqual("Assert.That.IsGreaterThan failed. Expected:<1>. Actual:<1>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsGreaterThan_WithComparer_Success()
        {
            using var comparer = CreateComparer(1, 2, 1);
            Assert.That.IsGreaterThan(2, 1, comparer);
        }

        [TestMethod]
        public void IsGreaterThan_WithComparer_Fail_Smaller()
        {
            using var comparer = CreateComparer(2, 1, -1);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsGreaterThan(1, 2, comparer));
            Assert.AreEqual("Assert.That.IsGreaterThan failed. Expected:<1>. Actual:<2>.", ex.Message);
        }

        [TestMethod]
        public void IsGreaterThan_WithComparer_Fail_Equal()
        {
            using var comparer = CreateComparer(1, 1, 0);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsGreaterThan(1, 1, comparer));
            Assert.AreEqual("Assert.That.IsGreaterThan failed. Expected:<1>. Actual:<1>.", ex.Message);
        }

        [TestMethod]
        public void IsGreaterThan_WithComparer_FailWithMessage_Smaller()
        {
            using var comparer = CreateComparer(2, 1, -1);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsGreaterThan(1, 2, comparer, "This is my test"));
            Assert.AreEqual("Assert.That.IsGreaterThan failed. Expected:<1>. Actual:<2>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsGreaterThan_WithComparer_FailWithMessage_Equal()
        {
            using var comparer = CreateComparer(1, 1, 0);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsGreaterThan(1, 1, comparer, "This is my test"));
            Assert.AreEqual("Assert.That.IsGreaterThan failed. Expected:<1>. Actual:<1>. This is my test", ex.Message);
        }
        #endregion

        #region IsGreaterThanOrEqualTo
        [TestMethod]
        public void IsGreaterThanOrEqualTo_Success_Equal()
        {
            Assert.That.IsGreaterThanOrEqualTo(1, 1);
        }

        [TestMethod]
        public void IsGreaterThanOrEqualTo_Success_Greater()
        {
            Assert.That.IsGreaterThanOrEqualTo(1, 2);
        }

        [TestMethod]
        public void IsGreaterThanOrEqualTo_Fail()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsGreaterThanOrEqualTo(2, 1));
            Assert.AreEqual("Assert.That.IsGreaterThanOrEqualTo failed. Expected:<2>. Actual:<1>.", ex.Message);
        }

        [TestMethod]
        public void IsGreaterThanOrEqualTo_FailWithMessage()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsGreaterThanOrEqualTo(2, 1, "This is my test"));
            Assert.AreEqual("Assert.That.IsGreaterThanOrEqualTo failed. Expected:<2>. Actual:<1>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsGreaterThanOrEqualTo_WithComparer_Success_Equal()
        {
            using var comparer = CreateComparer(1, 2, 0);
            Assert.That.IsGreaterThanOrEqualTo(2, 1, comparer);
        }

        [TestMethod]
        public void IsGreaterThanOrEqualTo_WithComparer_Success_Greater()
        {
            using var comparer = CreateComparer(1, 2, 1);
            Assert.That.IsGreaterThanOrEqualTo(2, 1, comparer);
        }

        [TestMethod]
        public void IsGreaterThanOrEqualTo_WithComparer_Fail()
        {
            using var comparer = CreateComparer(2, 1, -1);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsGreaterThanOrEqualTo(1, 2, comparer));
            Assert.AreEqual("Assert.That.IsGreaterThanOrEqualTo failed. Expected:<1>. Actual:<2>.", ex.Message);
        }

        [TestMethod]
        public void IsGreaterThanOrEqualTo_WithComparer_FailWithMessage()
        {
            using var comparer = CreateComparer(2, 1, -1);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsGreaterThanOrEqualTo(1, 2, comparer, "This is my test"));
            Assert.AreEqual("Assert.That.IsGreaterThanOrEqualTo failed. Expected:<1>. Actual:<2>. This is my test", ex.Message);
        }
        #endregion

        #region IsSmallerThan
        [TestMethod]
        public void IsSmallerThan_Success()
        {
            Assert.That.IsSmallerThan(2, 1);
        }

        [TestMethod]
        public void IsSmallerThan_Fail_Greater()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsSmallerThan(1, 2));
            Assert.AreEqual("Assert.That.IsSmallerThan failed. Expected:<1>. Actual:<2>.", ex.Message);
        }

        [TestMethod]
        public void IsSmallerThan_Fail_Equal()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsSmallerThan(1, 1));
            Assert.AreEqual("Assert.That.IsSmallerThan failed. Expected:<1>. Actual:<1>.", ex.Message);
        }

        [TestMethod]
        public void IsSmallerThan_FailWithMessage_Greater()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsSmallerThan(1, 2, "This is my test"));
            Assert.AreEqual("Assert.That.IsSmallerThan failed. Expected:<1>. Actual:<2>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsSmallerThan_FailWithMessage_Equal()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsSmallerThan(1, 1, "This is my test"));
            Assert.AreEqual("Assert.That.IsSmallerThan failed. Expected:<1>. Actual:<1>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsSmallerThan_WithComparer_Success()
        {
            using var comparer = CreateComparer(2, 1, -1);
            Assert.That.IsSmallerThan(1, 2, comparer);
        }

        [TestMethod]
        public void IsSmallerThan_WithComparer_Fail_Greater()
        {
            using var comparer = CreateComparer(1, 2, 1);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsSmallerThan(2, 1, comparer));
            Assert.AreEqual("Assert.That.IsSmallerThan failed. Expected:<2>. Actual:<1>.", ex.Message);
        }

        [TestMethod]
        public void IsSmallerThan_WithComparer_Fail_Equal()
        {
            using var comparer = CreateComparer(1, 1, 0);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsSmallerThan(1, 1, comparer));
            Assert.AreEqual("Assert.That.IsSmallerThan failed. Expected:<1>. Actual:<1>.", ex.Message);
        }

        [TestMethod]
        public void IsSmallerThan_WithComparer_FailWithMessage_Greater()
        {
            using var comparer = CreateComparer(1, 2, 1);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsSmallerThan(2, 1, comparer, "This is my test"));
            Assert.AreEqual("Assert.That.IsSmallerThan failed. Expected:<2>. Actual:<1>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsSmallerThan_WithComparer_FailWithMessage_Equal()
        {
            using var comparer = CreateComparer(1, 1, 0);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsSmallerThan(1, 1, comparer, "This is my test"));
            Assert.AreEqual("Assert.That.IsSmallerThan failed. Expected:<1>. Actual:<1>. This is my test", ex.Message);
        }
        #endregion

        #region IsSmallerThanOrEqualTo
        [TestMethod]
        public void IsSmallerThanOrEqualTo_Success_Equal()
        {
            Assert.That.IsSmallerThanOrEqualTo(1, 1);
        }

        [TestMethod]
        public void IsSmallerThanOrEqualTo_Success_Smaller()
        {
            Assert.That.IsSmallerThanOrEqualTo(2, 1);
        }

        [TestMethod]
        public void IsSmallerThanOrEqualTo_Fail()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsSmallerThanOrEqualTo(1, 2));
            Assert.AreEqual("Assert.That.IsSmallerThanOrEqualTo failed. Expected:<1>. Actual:<2>.", ex.Message);
        }

        [TestMethod]
        public void IsSmallerThanOrEqualTo_FailWithMessage()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsSmallerThanOrEqualTo(1, 2, "This is my test"));
            Assert.AreEqual("Assert.That.IsSmallerThanOrEqualTo failed. Expected:<1>. Actual:<2>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsSmallerThanOrEqualTo_WithComparer_Success_Equal()
        {
            using var comparer = CreateComparer(2, 1, 0);
            Assert.That.IsSmallerThanOrEqualTo(1, 2, comparer);
        }

        [TestMethod]
        public void IsSmallerThanOrEqualTo_WithComparer_Success_Smaller()
        {
            using var comparer = CreateComparer(2, 1, -1);
            Assert.That.IsSmallerThanOrEqualTo(1, 2, comparer);
        }

        [TestMethod]
        public void IsSmallerThanOrEqualTo_WithComparer_Fail()
        {
            using var comparer = CreateComparer(1, 2, 1);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsSmallerThanOrEqualTo(2, 1, comparer));
            Assert.AreEqual("Assert.That.IsSmallerThanOrEqualTo failed. Expected:<2>. Actual:<1>.", ex.Message);
        }

        [TestMethod]
        public void IsSmallerThanOrEqualTo_WithComparer_FailWithMessage()
        {
            using var comparer = CreateComparer(1, 2, 1);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsSmallerThanOrEqualTo(2, 1, comparer, "This is my test"));
            Assert.AreEqual("Assert.That.IsSmallerThanOrEqualTo failed. Expected:<2>. Actual:<1>. This is my test", ex.Message);
        }
        #endregion

        #region IsBetween
        [TestMethod]
        public void IsBetween_Success()
        {
            Assert.That.IsBetween(1, 3, 2);
        }

        [TestMethod]
        public void IsBetween_Fail_EqualMin()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 1));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<1>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_Fail_SmallerMin()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 0));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<0>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_Fail_EqualMax()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 3));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<3>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_Fail_GreaterMax()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 4));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<4>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_FailWithMessage_EqualMin()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 1, "This is my test"));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<1>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_FailWithMessage_SmallerMin()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 0, "This is my test"));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<0>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_FailWithMessage_EqualMax()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 3, "This is my test"));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<3>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_FailWithMessage_GreaterMax()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 4, "This is my test"));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<4>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_Success()
        {
            using var comparer = CreateBetweenComparer(2);
            Assert.That.IsBetween(1, 3, 2, comparer);
        }

        [TestMethod]
        public void IsBetween_WithComparer_Fail_EqualMin()
        {
            using var comparer = CreateBetweenComparer(1);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 1, comparer));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<1>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_Fail_SmallerMin()
        {
            using var comparer = CreateBetweenComparer(0);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 0, comparer));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<0>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_Fail_EqualMax()
        {
            using var comparer = CreateBetweenComparer(3);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 3, comparer));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<3>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_Fail_GreaterMax()
        {
            using var comparer = CreateBetweenComparer(4);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 4, comparer));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<4>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_FailWithMessage_EqualMin()
        {
            using var comparer = CreateBetweenComparer(1);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 1, comparer, "This is my test"));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<1>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_FailWithMessage_SmallerMin()
        {
            using var comparer = CreateBetweenComparer(0);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 0, comparer, "This is my test"));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<0>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_FailWithMessage_EqualMax()
        {
            using var comparer = CreateBetweenComparer(3);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 3, comparer, "This is my test"));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<3>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_FailWithMessage_GreaterMax()
        {
            using var comparer = CreateBetweenComparer(4);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 4, comparer, "This is my test"));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<4>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_IncludeMin_Success_GreaterMin()
        {
            Assert.That.IsBetween(1, 3, 2, true, false);
        }

        [TestMethod]
        public void IsBetween_IncludeMin_Success_EqualMin()
        {
            Assert.That.IsBetween(1, 3, 1, true, false);
        }

        [TestMethod]
        public void IsBetween_IncludeMin_Fail_SmallerMin()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 0, true, false));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<0>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_IncludeMin_Fail_EqualMax()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 3, true, false));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<3>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_IncludeMin_Fail_GreaterMax()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 4, true, false));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<4>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_IncludeMin_FailWithMessage_SmallerMin()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 0, true, false, "This is my test"));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<0>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_IncludeMin_FailWithMessage_EqualMax()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 3, true, false, "This is my test"));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<3>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_IncludeMin_FailWithMessage_GreaterMax()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 4, true, false, "This is my test"));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<4>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMin_Success_GreaterMin()
        {
            using var comparer = CreateBetweenComparer(2);
            Assert.That.IsBetween(1, 3, 2, true, false, comparer);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMin_Success_EqualMin()
        {
            using var comparer = CreateBetweenComparer(1);
            Assert.That.IsBetween(1, 3, 1, true, false, comparer);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMin_Fail_SmallerMin()
        {
            using var comparer = CreateBetweenComparer(0);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 0, true, false, comparer));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<0>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMin_Fail_EqualMax()
        {
            using var comparer = CreateBetweenComparer(3);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 3, true, false, comparer));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<3>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMin_Fail_GreaterMax()
        {
            using var comparer = CreateBetweenComparer(4);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 4, true, false, comparer));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<4>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMin_FailWithMessage_SmallerMin()
        {
            using var comparer = CreateBetweenComparer(0);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 0, true, false, comparer, "This is my test"));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<0>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMin_FailWithMessage_EqualMax()
        {
            using var comparer = CreateBetweenComparer(3);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 3, true, false, comparer, "This is my test"));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<3>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMin_FailWithMessage_GreaterMax()
        {
            using var comparer = CreateBetweenComparer(4);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 4, true, false, comparer, "This is my test"));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<4>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_IncludeMax_Success_SmallerMax()
        {
            Assert.That.IsBetween(1, 3, 2, false, true);
        }

        [TestMethod]
        public void IsBetween_IncludeMax_Success_EqualMax()
        {
            Assert.That.IsBetween(1, 3, 3, false, true);
        }

        [TestMethod]
        public void IsBetween_IncludeMin_Fail_EqualMin()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 1, false, true));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<1>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_IncludeMax_Fail_SmallerMin()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 0, false, true));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<0>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_IncludeMax_Fail_GreaterMax()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 4, false, true));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<4>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_IncludeMax_FailWithMessage_EqualMin()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 1, false, true, "This is my test"));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<1>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_IncludeMax_FailWithMessage_SmallerMin()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 0, false, true, "This is my test"));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<0>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_IncludeMax_FailWithMessage_GreaterMax()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 4, false, true, "This is my test"));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<4>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMax_Success_SmallerMax()
        {
            using var comparer = CreateBetweenComparer(2);
            Assert.That.IsBetween(1, 3, 2, false, true, comparer);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMax_Success_EqualMax()
        {
            using var comparer = CreateBetweenComparer(3);
            Assert.That.IsBetween(1, 3, 3, false, true, comparer);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMax_Fail_EqualMin()
        {
            using var comparer = CreateBetweenComparer(1);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 1, false, true, comparer));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<1>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMax_Fail_SmallerMin()
        {
            using var comparer = CreateBetweenComparer(0);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 0, false, true, comparer));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<0>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMax_Fail_GreaterMax()
        {
            using var comparer = CreateBetweenComparer(4);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 4, false, true, comparer));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<4>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMax_FailWithMessage_EqualMin()
        {
            using var comparer = CreateBetweenComparer(1);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 1, false, true, comparer, "This is my test"));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<1>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMax_FailWithMessage_SmallerMin()
        {
            using var comparer = CreateBetweenComparer(0);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 0, false, true, comparer, "This is my test"));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<0>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMax_FailWithMessage_GreaterMax()
        {
            using var comparer = CreateBetweenComparer(4);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 4, false, true, comparer, "This is my test"));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<4>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_IncludeMinAndMax_Success_SmallerMax_GreaterMin()
        {
            Assert.That.IsBetween(1, 3, 2, true, true);
        }

        [TestMethod]
        public void IsBetween_IncludeMinAndMax_Success_EqualMin()
        {
            Assert.That.IsBetween(1, 3, 1, true, true);
        }

        [TestMethod]
        public void IsBetween_IncludeMinAndMax_Success_EqualMax()
        {
            Assert.That.IsBetween(1, 3, 3, true, true);
        }

        [TestMethod]
        public void IsBetween_IncludeMaxAndMax_Fail_SmallerMin()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 0, true, true));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<0>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_IncludeMaxAndMax_Fail_GreaterMax()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 4, true, true));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<4>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_IncludeMaxAndMax_FailWithMessage_SmallerMin()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 0, true, true, "This is my test"));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<0>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_IncludeMaxAndMax_FailWithMessage_GreaterMax()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 4, true, true, "This is my test"));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<4>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMinAndMax_Success_SmallerMax_GreaterMin()
        {
            using var comparer = CreateBetweenComparer(2);
            Assert.That.IsBetween(1, 3, 2, true, true, comparer);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMinAndMax_Success_EqualMin()
        {
            using var comparer = CreateBetweenComparer(1);
            Assert.That.IsBetween(1, 3, 1, true, true, comparer);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMinAndMax_Success_EqualMax()
        {
            using var comparer = CreateBetweenComparer(3);
            Assert.That.IsBetween(1, 3, 3, true, true, comparer);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMaxAndMax_Fail_SmallerMin()
        {
            using var comparer = CreateBetweenComparer(0);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 0, true, true, comparer));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<0>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMaxAndMax_Fail_GreaterMax()
        {
            using var comparer = CreateBetweenComparer(4);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 4, true, true, comparer));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<4>.", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMaxAndMax_FailWithMessage_SmallerMin()
        {
            using var comparer = CreateBetweenComparer(0);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 0, true, true, comparer, "This is my test"));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<0>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsBetween_WithComparer_IncludeMaxAndMax_FailWithMessage_GreaterMax()
        {
            using var comparer = CreateBetweenComparer(4);
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsBetween(1, 3, 4, true, true, comparer, "This is my test"));
            Assert.AreEqual("Assert.That.IsBetween failed. ExpectedMin:<1>. ExpectedMax:<3>. Actual:<4>. This is my test", ex.Message);
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
