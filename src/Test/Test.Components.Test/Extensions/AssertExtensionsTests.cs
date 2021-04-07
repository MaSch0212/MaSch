using MaSch.Core;
using MaSch.Test.Components.Test.TestHelper;
using MaSch.Test.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
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

        #region Contains (string)
        [TestMethod]
        public void ContainsStr_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.Contains("blub", (string?)null));
            Assert.AreEqual("Assert.That.Contains failed. Expected:<blub>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void ContainsStr_Fail_DifferentContent()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.Contains("blub", "bbb"));
            Assert.AreEqual("Assert.That.Contains failed. Expected:<blub>. Actual:<bbb>.", ex.Message);
        }

        [TestMethod]
        public void ContainsStr_Fail_DifferentCasing()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.Contains("blub", "BLUB"));
            Assert.AreEqual("Assert.That.Contains failed. Expected:<blub>. Actual:<BLUB>.", ex.Message);
        }

        [TestMethod]
        public void ContainsStr_Success()
        {
            Assert.That.Contains("blub", "jhfkjhfdgblubkjfh");
        }

        [TestMethod]
        public void ContainsStr_WithMessage_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.Contains("blub", (string?)null, "This is my test"));
            Assert.AreEqual("Assert.That.Contains failed. Expected:<blub>. Actual:<(null)>. This is my test", ex.Message);
        }

        [TestMethod]
        public void ContainsStr_WithMessage_Fail_DifferentContent()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.Contains("blub", "bbb", "This is my test"));
            Assert.AreEqual("Assert.That.Contains failed. Expected:<blub>. Actual:<bbb>. This is my test", ex.Message);
        }

        [TestMethod]
        public void ContainsStr_WithMessage_Fail_DifferentCasing()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.Contains("blub", "BLUB", "This is my test"));
            Assert.AreEqual("Assert.That.Contains failed. Expected:<blub>. Actual:<BLUB>. This is my test", ex.Message);
        }

        [TestMethod]
        public void ContainsStr_WithMessage_Success()
        {
            Assert.That.Contains("blub", "jhfkjhfdgblubkjfh", "This is my test");
        }

        [TestMethod]
        public void ContainsStr_WithComparison_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.Contains("blub", null, StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual("Assert.That.Contains failed. Expected:<blub>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void ContainsStr_WithComparison_Fail_DifferentContent()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.Contains("blub", "bbb", StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual("Assert.That.Contains failed. Expected:<blub>. Actual:<bbb>.", ex.Message);
        }

        [TestMethod]
        public void ContainsStr_WithComparison_Success_SameCasing()
        {
            Assert.That.Contains("blub", "jhfkjhfdgblubkjfh", StringComparison.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void ContainsStr_WithComparison_Success_DifferentCasing()
        {
            Assert.That.Contains("blub", "jhfkjhfdgBLUBkjfh", StringComparison.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void ContainsStr_WithComparison_WithMessage_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.Contains("blub", null, StringComparison.OrdinalIgnoreCase, "This is my test"));
            Assert.AreEqual("Assert.That.Contains failed. Expected:<blub>. Actual:<(null)>. This is my test", ex.Message);
        }

        [TestMethod]
        public void ContainsStr_WithComparison_WithMessage_Fail_DifferentContent()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.Contains("blub", "bbb", StringComparison.OrdinalIgnoreCase, "This is my test"));
            Assert.AreEqual("Assert.That.Contains failed. Expected:<blub>. Actual:<bbb>. This is my test", ex.Message);
        }

        [TestMethod]
        public void ContainsStr_WithComparison_WithMessage_Success_SameCasing()
        {
            Assert.That.Contains("blub", "jhfkjhfdgblubkjfh", StringComparison.OrdinalIgnoreCase, "This is my test");
        }

        [TestMethod]
        public void ContainsStr_WithComparison_WithMessage_Success_DifferentCasing()
        {
            Assert.That.Contains("blub", "jhfkjhfdgBLUBkjfh", StringComparison.OrdinalIgnoreCase, "This is my test");
        }
        #endregion

        #region Contains (IEnumerable)
        [TestMethod]
        public void ContainsEnum_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.Contains("Test", (IEnumerable<string>?)null));
            Assert.AreEqual("Assert.That.Contains failed. Expected:<Test>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void ContainsEnum_Empty()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.Contains("Test", Array.Empty<string>()));
            Assert.AreEqual("Assert.That.Contains failed. Expected:<Test>. Actual:<[]>.", ex.Message);
        }

        [TestMethod]
        public void ContainsEnum_NotContained()
        {
            var nl = Environment.NewLine;
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.Contains("Test", new[] { "abc", "def", "ghi" }));
            Assert.AreEqual($"Assert.That.Contains failed. Expected:<Test>. Actual:<[{nl}\tabc,{nl}\tdef,{nl}\tghi{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void ContainsEnum_Contained()
        {
            Assert.That.Contains("Test", new[] { "abc", "def", "Test", "ghi" });
        }

        [TestMethod]
        public void ContainsEnum_WithMessage_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.Contains("Test", (IEnumerable<string>?)null, "This is my test"));
            Assert.AreEqual("Assert.That.Contains failed. Expected:<Test>. Actual:<(null)>. This is my test", ex.Message);
        }

        [TestMethod]
        public void ContainsEnum_WithMessage_Empty()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.Contains("Test", Array.Empty<string>(), "This is my test"));
            Assert.AreEqual("Assert.That.Contains failed. Expected:<Test>. Actual:<[]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void ContainsEnum_WithMessage_NotContained()
        {
            var nl = Environment.NewLine;
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.Contains("Test", new[] { "abc", "def", "ghi" }, "This is my test"));
            Assert.AreEqual($"Assert.That.Contains failed. Expected:<Test>. Actual:<[{nl}\tabc,{nl}\tdef,{nl}\tghi{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void ContainsEnum_WithMessage_Contained()
        {
            Assert.That.Contains("Test", new[] { "abc", "def", "Test", "ghi" }, "This is my test");
        }

        [TestMethod]
        public void ContainsEnum_WithComparerT_Null()
        {
            using var comparer = CreateEqualityComparerT<string>();
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.Contains("Test", null, comparer));
            Assert.AreEqual("Assert.That.Contains failed. Expected:<Test>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void ContainsEnum_WithComparerT_Empty()
        {
            using var comparer = CreateEqualityComparerT<string>();
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.Contains("Test", Array.Empty<string>(), comparer));
            Assert.AreEqual("Assert.That.Contains failed. Expected:<Test>. Actual:<[]>.", ex.Message);
        }

        [TestMethod]
        public void ContainsEnum_WithComparerT_NotContained()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparerT(("abc", "Test"), ("def", "Test"), ("ghi", "Test"));
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.Contains("Test", new[] { "abc", "def", "ghi" }, comparer));
            Assert.AreEqual($"Assert.That.Contains failed. Expected:<Test>. Actual:<[{nl}\tabc,{nl}\tdef,{nl}\tghi{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void ContainsEnum_WithComparerT_Contained()
        {
            using var comparer = CreateEqualityComparerT(("abc", "Test"), ("def", "Test"), ("Test", "Test"));
            Assert.That.Contains("Test", new[] { "abc", "def", "Test", "ghi" }, comparer);
        }

        [TestMethod]
        public void ContainsEnum_WithComparerT_WithMessage_Null()
        {
            using var comparer = CreateEqualityComparerT<string>();
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.Contains("Test", null, comparer, "This is my test"));
            Assert.AreEqual("Assert.That.Contains failed. Expected:<Test>. Actual:<(null)>. This is my test", ex.Message);
        }

        [TestMethod]
        public void ContainsEnum_WithComparerT_WithMessage_Empty()
        {
            using var comparer = CreateEqualityComparerT<string>();
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.Contains("Test", Array.Empty<string>(), comparer, "This is my test"));
            Assert.AreEqual("Assert.That.Contains failed. Expected:<Test>. Actual:<[]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void ContainsEnum_WithComparerT_WithMessage_NotContained()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparerT(("abc", "Test"), ("def", "Test"), ("ghi", "Test"));
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.Contains("Test", new[] { "abc", "def", "ghi" }, comparer, "This is my test"));
            Assert.AreEqual($"Assert.That.Contains failed. Expected:<Test>. Actual:<[{nl}\tabc,{nl}\tdef,{nl}\tghi{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void ContainsEnum_WithComparerT_WithMessage_Contained()
        {
            using var comparer = CreateEqualityComparerT(("abc", "Test"), ("def", "Test"), ("Test", "Test"));
            Assert.That.Contains("Test", new[] { "abc", "def", "Test", "ghi" }, comparer, "This is my test");
        }

        [TestMethod]
        public void ContainsEnum_WithComparer_Null()
        {
            using var comparer = CreateEqualityComparer();
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.Contains("Test", null, comparer));
            Assert.AreEqual("Assert.That.Contains failed. Expected:<Test>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void ContainsEnum_WithComparer_Empty()
        {
            using var comparer = CreateEqualityComparer();
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.Contains("Test", Array.Empty<string>(), comparer));
            Assert.AreEqual("Assert.That.Contains failed. Expected:<Test>. Actual:<[]>.", ex.Message);
        }

        [TestMethod]
        public void ContainsEnum_WithComparer_NotContained()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparer(("abc", "Test"), ("def", "Test"), ("ghi", "Test"));
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.Contains("Test", new[] { "abc", "def", "ghi" }, comparer));
            Assert.AreEqual($"Assert.That.Contains failed. Expected:<Test>. Actual:<[{nl}\tabc,{nl}\tdef,{nl}\tghi{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void ContainsEnum_WithComparer_Contained()
        {
            using var comparer = CreateEqualityComparer(("abc", "Test"), ("def", "Test"), ("Test", "Test"));
            Assert.That.Contains("Test", new[] { "abc", "def", "Test", "ghi" }, comparer);
        }

        [TestMethod]
        public void ContainsEnum_WithComparer_WithMessage_Null()
        {
            using var comparer = CreateEqualityComparer();
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.Contains("Test", null, comparer, "This is my test"));
            Assert.AreEqual("Assert.That.Contains failed. Expected:<Test>. Actual:<(null)>. This is my test", ex.Message);
        }

        [TestMethod]
        public void ContainsEnum_WithComparer_WithMessage_Empty()
        {
            using var comparer = CreateEqualityComparer();
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.Contains("Test", Array.Empty<string>(), comparer, "This is my test"));
            Assert.AreEqual("Assert.That.Contains failed. Expected:<Test>. Actual:<[]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void ContainsEnum_WithComparer_WithMessage_NotContained()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparer(("abc", "Test"), ("def", "Test"), ("ghi", "Test"));
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.Contains("Test", new[] { "abc", "def", "ghi" }, comparer, "This is my test"));
            Assert.AreEqual($"Assert.That.Contains failed. Expected:<Test>. Actual:<[{nl}\tabc,{nl}\tdef,{nl}\tghi{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void ContainsEnum_WithComparer_WithMessage_Contained()
        {
            using var comparer = CreateEqualityComparer(("abc", "Test"), ("def", "Test"), ("Test", "Test"));
            Assert.That.Contains("Test", new[] { "abc", "def", "Test", "ghi" }, comparer, "This is my test");
        }

        [TestMethod]
        public void ContainsEnum_WithPredicate_Null()
        {
            using var predicate = CreatePredicate<string, int>();
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.Contains("Test", null, predicate.Object));
            Assert.AreEqual("Assert.That.Contains failed. Expected:<Test>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void ContainsEnum_WithPredicate_Empty()
        {
            using var predicate = CreatePredicate<string, int>();
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.Contains("Test", Array.Empty<int>(), predicate.Object));
            Assert.AreEqual("Assert.That.Contains failed. Expected:<Test>. Actual:<[]>.", ex.Message);
        }

        [TestMethod]
        public void ContainsEnum_WithPredicate_NotContained()
        {
            var nl = Environment.NewLine;
            using var predicate = CreatePredicate(("Test", 1, false), ("Test", 2, false), ("Test", 3, false));
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.Contains("Test", new[] { 1, 2, 3 }, predicate.Object));
            Assert.AreEqual($"Assert.That.Contains failed. Expected:<Test>. Actual:<[{nl}\t1,{nl}\t2,{nl}\t3{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void ContainsEnum_WithPredicate_Contained()
        {
            using var predicate = CreatePredicate(("Test", 1, false), ("Test", 2, false), ("Test", 3, true));
            Assert.That.Contains("Test", new[] { 1, 2, 3, 4 }, predicate.Object);
        }

        [TestMethod]
        public void ContainsEnum_WithPredicate_WithMessage_Null()
        {
            using var predicate = CreatePredicate<string, int>();
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.Contains("Test", null, predicate.Object, "This is my test"));
            Assert.AreEqual("Assert.That.Contains failed. Expected:<Test>. Actual:<(null)>. This is my test", ex.Message);
        }

        [TestMethod]
        public void ContainsEnum_WithPredicate_WithMessage_Empty()
        {
            using var predicate = CreatePredicate<string, int>();
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.Contains("Test", Array.Empty<int>(), predicate.Object, "This is my test"));
            Assert.AreEqual("Assert.That.Contains failed. Expected:<Test>. Actual:<[]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void ContainsEnum_WithPredicate_WithMessage_NotContained()
        {
            var nl = Environment.NewLine;
            using var predicate = CreatePredicate(("Test", 1, false), ("Test", 2, false), ("Test", 3, false));
            var ex = Assert.ThrowsException<AssertFailedException>(() => Assert.That.Contains("Test", new[] { 1, 2, 3 }, predicate.Object, "This is my test"));
            Assert.AreEqual($"Assert.That.Contains failed. Expected:<Test>. Actual:<[{nl}\t1,{nl}\t2,{nl}\t3{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void ContainsEnum_WithPredicate_WithMessage_Contained()
        {
            using var predicate = CreatePredicate(("Test", 1, false), ("Test", 2, false), ("Test", 3, true));
            Assert.That.Contains("Test", new[] { 1, 2, 3, 4 }, predicate.Object, "This is my test");
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

        private static DisposableEqualityComparer<T> CreateEqualityComparerT<T>(params (T X, T Y)[] expectedCalls)
        {
            var comparerMock = new Mock<IEqualityComparer<T>>(MockBehavior.Strict);
            comparerMock.Setup(m => m.GetHashCode(It.IsAny<T>()!)).Returns<T>(x => x!.GetHashCode());
            comparerMock.Setup(m => m.Equals(It.IsAny<T?>(), It.IsAny<T?>())).Returns<T?, T?>((x, y) => Equals(x, y));

            return new DisposableEqualityComparer<T>(
                comparerMock.Object,
                new ActionOnDispose(() =>
                {
                    comparerMock.Verify(m => m.Equals(It.IsAny<T?>(), It.IsAny<T?>()), Times.Exactly(expectedCalls.Length));
                    foreach (var (x, y) in expectedCalls)
                        comparerMock.Verify(m => m.Equals(x, y), Times.Once());
                }));
        }

        private static DisposableEqualityComparer CreateEqualityComparer(params (object? X, object? Y)[] expectedCalls)
        {
            var comparerMock = new Mock<IEqualityComparer>(MockBehavior.Strict);
            comparerMock.Setup(m => m.GetHashCode(It.IsAny<object>()!)).Returns<object?>(x => x!.GetHashCode());
            comparerMock.Setup(m => m.Equals(It.IsAny<object?>(), It.IsAny<object?>())).Returns<object?, object?>((x, y) => Equals(x, y));

            return new DisposableEqualityComparer(
                comparerMock.Object,
                new ActionOnDispose(() =>
                {
                    comparerMock.Verify(m => m.Equals(It.IsAny<object?>(), It.IsAny<object?>()), Times.Exactly(expectedCalls.Length));
                    foreach (var (x, y) in expectedCalls)
                        comparerMock.Verify(m => m.Equals(x, y), Times.Once());
                }));
        }

        private static DisposableWrapper<Func<T1, T2, bool>> CreatePredicate<T1, T2>(params (T1 X, T2 Y, bool Result)[] calls)
        {
            var funcMock = new Mock<Func<T1, T2, bool>>(MockBehavior.Strict);
            foreach (var (x, y, result) in calls)
                funcMock.Setup(m => m(x, y)).Returns(result);

            return new DisposableWrapper<Func<T1, T2, bool>>(
                funcMock.Object,
                new ActionOnDispose(() =>
                {
                    funcMock.Verify(m => m(It.IsAny<T1>(), It.IsAny<T2>()), Times.Exactly(calls.Length));
                    foreach (var (x, y, _) in calls)
                        funcMock.Verify(m => m(x, y), Times.Once());
                }));
        }
    }
}
