using MaSch.Core;
using MaSch.Test.Assertion.UnitTests.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using MSAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace MaSch.Test.Assertion.UnitTests
{
    [TestClass]
    public class CollectionAssertionsTests
    {
        private static MaSch.Test.Assertion.Assert AssertUnderTest => MaSch.Test.Assertion.Assert.Instance;

        #region IsEmpty

        [TestMethod]
        public void IsEmpty_NullCollection()
        {
            MSAssert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.IsEmpty(null!));
        }

        [TestMethod]
        public void IsEmpty_Success()
        {
            AssertUnderTest.IsEmpty(Array.Empty<object>());
        }

        [TestMethod]
        public void IsEmpty_Fail()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsEmpty(new[] { "Test" }));
            MSAssert.AreEqual("Assert.IsEmpty failed.", ex.Message);
        }

        #endregion

        #region IsNullOrEmpty

        [TestMethod]
        public void IsNullOrEmpty_Success_Null()
        {
            AssertUnderTest.IsNullOrEmpty((IEnumerable?)null);
        }

        [TestMethod]
        public void IsNullOrEmpty_Success_Empty()
        {
            AssertUnderTest.IsNullOrEmpty(Array.Empty<object>());
        }

        [TestMethod]
        public void IsNullOrEmpty_Fail_NotEmpty()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNullOrEmpty(new[] { "Test" }));
            MSAssert.AreEqual("Assert.IsNullOrEmpty failed.", ex.Message);
        }

        [TestMethod]
        public void IsNullOrEmpty_WithMessage_Fail_NotEmpty()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNullOrEmpty(new[] { "Test" }, "This is my test"));
            MSAssert.AreEqual("Assert.IsNullOrEmpty failed. This is my test", ex.Message);
        }

        #endregion

        #region IsNotEmpty

        [TestMethod]
        public void IsNotEmpty_NullCollection()
        {
            MSAssert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.IsNotEmpty(null!));
        }

        [TestMethod]
        public void IsNotEmpty_Success()
        {
            AssertUnderTest.IsNotEmpty(new[] { "Test" });
        }

        [TestMethod]
        public void IsNotEmpty_Fail()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotEmpty(Array.Empty<object>()));
            MSAssert.AreEqual("Assert.IsNotEmpty failed.", ex.Message);
        }

        #endregion

        #region IsNotNullOrEmpty

        [TestMethod]
        public void IsNotNullOrEmpty_Success_NotEmpty()
        {
            AssertUnderTest.IsNotNullOrEmpty(new[] { "Test" });
        }

        [TestMethod]
        public void IsNotNullOrEmpty_Fail_Null()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotNullOrEmpty((IEnumerable?)null));
            MSAssert.AreEqual("Assert.IsNotNullOrEmpty failed.", ex.Message);
        }

        [TestMethod]
        public void IsNotNullOrEmpty_Fail_Empty()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotNullOrEmpty(Array.Empty<object>()));
            MSAssert.AreEqual("Assert.IsNotNullOrEmpty failed.", ex.Message);
        }

        [TestMethod]
        public void IsNotNullOrEmpty_WithMessage_Fail_Null()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotNullOrEmpty((IEnumerable?)null, "This is my test"));
            MSAssert.AreEqual("Assert.IsNotNullOrEmpty failed. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsNotNullOrEmpty_WithMessage_Fail_Empty()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotNullOrEmpty(Array.Empty<object>(), "This is my test"));
            MSAssert.AreEqual("Assert.IsNotNullOrEmpty failed. This is my test", ex.Message);
        }

        #endregion

        #region Contains

        [TestMethod]
        public void Contains_Null()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", (IEnumerable<string>?)null));
            MSAssert.AreEqual("Assert.Contains failed. Expected:<Test>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void Contains_Empty()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", Array.Empty<string>()));
            MSAssert.AreEqual("Assert.Contains failed. Expected:<Test>. Actual:<[]>.", ex.Message);
        }

        [TestMethod]
        public void Contains_NotContained()
        {
            var nl = Environment.NewLine;
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", new[] { "abc", "def", "ghi" }));
            MSAssert.AreEqual($"Assert.Contains failed. Expected:<Test>. Actual:<[{nl}\tabc,{nl}\tdef,{nl}\tghi{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void Contains_Contained()
        {
            AssertUnderTest.Contains("Test", new[] { "abc", "def", "Test", "ghi" });
        }

        [TestMethod]
        public void Contains_WithMessage_Null()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", (IEnumerable<string>?)null, "This is my test"));
            MSAssert.AreEqual("Assert.Contains failed. Expected:<Test>. Actual:<(null)>. This is my test", ex.Message);
        }

        [TestMethod]
        public void Contains_WithMessage_Empty()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", Array.Empty<string>(), "This is my test"));
            MSAssert.AreEqual("Assert.Contains failed. Expected:<Test>. Actual:<[]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void Contains_WithMessage_NotContained()
        {
            var nl = Environment.NewLine;
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", new[] { "abc", "def", "ghi" }, "This is my test"));
            MSAssert.AreEqual($"Assert.Contains failed. Expected:<Test>. Actual:<[{nl}\tabc,{nl}\tdef,{nl}\tghi{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void Contains_WithMessage_Contained()
        {
            AssertUnderTest.Contains("Test", new[] { "abc", "def", "Test", "ghi" }, "This is my test");
        }

        [TestMethod]
        public void Contains_WithComparerT_Null()
        {
            using var comparer = CreateEqualityComparerT<string>();
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", null, comparer));
            MSAssert.AreEqual("Assert.Contains failed. Expected:<Test>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void Contains_WithComparerT_Empty()
        {
            using var comparer = CreateEqualityComparerT<string>();
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", Array.Empty<string>(), comparer));
            MSAssert.AreEqual("Assert.Contains failed. Expected:<Test>. Actual:<[]>.", ex.Message);
        }

        [TestMethod]
        public void Contains_WithComparerT_NotContained()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparerT(("abc", "Test"), ("def", "Test"), ("ghi", "Test"));
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", new[] { "abc", "def", "ghi" }, comparer));
            MSAssert.AreEqual($"Assert.Contains failed. Expected:<Test>. Actual:<[{nl}\tabc,{nl}\tdef,{nl}\tghi{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void Contains_WithComparerT_Contained()
        {
            using var comparer = CreateEqualityComparerT(("abc", "Test"), ("def", "Test"), ("Test", "Test"));
            AssertUnderTest.Contains("Test", new[] { "abc", "def", "Test", "ghi" }, comparer);
        }

        [TestMethod]
        public void Contains_WithComparerT_WithMessage_Null()
        {
            using var comparer = CreateEqualityComparerT<string>();
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", null, comparer, "This is my test"));
            MSAssert.AreEqual("Assert.Contains failed. Expected:<Test>. Actual:<(null)>. This is my test", ex.Message);
        }

        [TestMethod]
        public void Contains_WithComparerT_WithMessage_Empty()
        {
            using var comparer = CreateEqualityComparerT<string>();
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", Array.Empty<string>(), comparer, "This is my test"));
            MSAssert.AreEqual("Assert.Contains failed. Expected:<Test>. Actual:<[]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void Contains_WithComparerT_WithMessage_NotContained()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparerT(("abc", "Test"), ("def", "Test"), ("ghi", "Test"));
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", new[] { "abc", "def", "ghi" }, comparer, "This is my test"));
            MSAssert.AreEqual($"Assert.Contains failed. Expected:<Test>. Actual:<[{nl}\tabc,{nl}\tdef,{nl}\tghi{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void Contains_WithComparerT_WithMessage_Contained()
        {
            using var comparer = CreateEqualityComparerT(("abc", "Test"), ("def", "Test"), ("Test", "Test"));
            AssertUnderTest.Contains("Test", new[] { "abc", "def", "Test", "ghi" }, comparer, "This is my test");
        }

        [TestMethod]
        public void Contains_WithComparer_Null()
        {
            using var comparer = CreateEqualityComparer();
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", null, comparer));
            MSAssert.AreEqual("Assert.Contains failed. Expected:<Test>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void Contains_WithComparer_Empty()
        {
            using var comparer = CreateEqualityComparer();
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", Array.Empty<string>(), comparer));
            MSAssert.AreEqual("Assert.Contains failed. Expected:<Test>. Actual:<[]>.", ex.Message);
        }

        [TestMethod]
        public void Contains_WithComparer_NotContained()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparer(("abc", "Test"), ("def", "Test"), ("ghi", "Test"));
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", new[] { "abc", "def", "ghi" }, comparer));
            MSAssert.AreEqual($"Assert.Contains failed. Expected:<Test>. Actual:<[{nl}\tabc,{nl}\tdef,{nl}\tghi{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void Contains_WithComparer_Contained()
        {
            using var comparer = CreateEqualityComparer(("abc", "Test"), ("def", "Test"), ("Test", "Test"));
            AssertUnderTest.Contains("Test", new[] { "abc", "def", "Test", "ghi" }, comparer);
        }

        [TestMethod]
        public void Contains_WithComparer_WithMessage_Null()
        {
            using var comparer = CreateEqualityComparer();
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", null, comparer, "This is my test"));
            MSAssert.AreEqual("Assert.Contains failed. Expected:<Test>. Actual:<(null)>. This is my test", ex.Message);
        }

        [TestMethod]
        public void Contains_WithComparer_WithMessage_Empty()
        {
            using var comparer = CreateEqualityComparer();
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", Array.Empty<string>(), comparer, "This is my test"));
            MSAssert.AreEqual("Assert.Contains failed. Expected:<Test>. Actual:<[]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void Contains_WithComparer_WithMessage_NotContained()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparer(("abc", "Test"), ("def", "Test"), ("ghi", "Test"));
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", new[] { "abc", "def", "ghi" }, comparer, "This is my test"));
            MSAssert.AreEqual($"Assert.Contains failed. Expected:<Test>. Actual:<[{nl}\tabc,{nl}\tdef,{nl}\tghi{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void Contains_WithComparer_WithMessage_Contained()
        {
            using var comparer = CreateEqualityComparer(("abc", "Test"), ("def", "Test"), ("Test", "Test"));
            AssertUnderTest.Contains("Test", new[] { "abc", "def", "Test", "ghi" }, comparer, "This is my test");
        }

        [TestMethod]
        public void Contains_WithPredicate_Null()
        {
            using var predicate = CreatePredicate<string, int>();
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", null, predicate.Object));
            MSAssert.AreEqual("Assert.Contains failed. Expected:<Test>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void Contains_WithPredicate_Empty()
        {
            using var predicate = CreatePredicate<string, int>();
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", Array.Empty<int>(), predicate.Object));
            MSAssert.AreEqual("Assert.Contains failed. Expected:<Test>. Actual:<[]>.", ex.Message);
        }

        [TestMethod]
        public void Contains_WithPredicate_NotContained()
        {
            var nl = Environment.NewLine;
            using var predicate = CreatePredicate(("Test", 1, false), ("Test", 2, false), ("Test", 3, false));
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", new[] { 1, 2, 3 }, predicate.Object));
            MSAssert.AreEqual($"Assert.Contains failed. Expected:<Test>. Actual:<[{nl}\t1,{nl}\t2,{nl}\t3{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void Contains_WithPredicate_Contained()
        {
            using var predicate = CreatePredicate(("Test", 1, false), ("Test", 2, false), ("Test", 3, true));
            AssertUnderTest.Contains("Test", new[] { 1, 2, 3, 4 }, predicate.Object);
        }

        [TestMethod]
        public void Contains_WithPredicate_WithMessage_Null()
        {
            using var predicate = CreatePredicate<string, int>();
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", null, predicate.Object, "This is my test"));
            MSAssert.AreEqual("Assert.Contains failed. Expected:<Test>. Actual:<(null)>. This is my test", ex.Message);
        }

        [TestMethod]
        public void Contains_WithPredicate_WithMessage_Empty()
        {
            using var predicate = CreatePredicate<string, int>();
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", Array.Empty<int>(), predicate.Object, "This is my test"));
            MSAssert.AreEqual("Assert.Contains failed. Expected:<Test>. Actual:<[]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void Contains_WithPredicate_WithMessage_NotContained()
        {
            var nl = Environment.NewLine;
            using var predicate = CreatePredicate(("Test", 1, false), ("Test", 2, false), ("Test", 3, false));
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", new[] { 1, 2, 3 }, predicate.Object, "This is my test"));
            MSAssert.AreEqual($"Assert.Contains failed. Expected:<Test>. Actual:<[{nl}\t1,{nl}\t2,{nl}\t3{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void Contains_WithPredicate_WithMessage_Contained()
        {
            using var predicate = CreatePredicate(("Test", 1, false), ("Test", 2, false), ("Test", 3, true));
            AssertUnderTest.Contains("Test", new[] { 1, 2, 3, 4 }, predicate.Object, "This is my test");
        }

        #endregion

        #region DoesNotContain

        [TestMethod]
        public void DoesNotContain_Null()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("Test", (IEnumerable<string>?)null));
            MSAssert.AreEqual("Assert.DoesNotContain failed. NotExpected:<Test>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void DoesNotContain_Empty()
        {
            AssertUnderTest.DoesNotContain("Test", Array.Empty<string>());
        }

        [TestMethod]
        public void DoesNotContain_NotContained()
        {
            AssertUnderTest.DoesNotContain("Test", new[] { "abc", "def", "ghi" });
        }

        [TestMethod]
        public void DoesNotContain_Contained()
        {
            var nl = Environment.NewLine;
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("Test", new[] { "abc", "def", "Test", "ghi" }));
            MSAssert.AreEqual($"Assert.DoesNotContain failed. NotExpected:<Test>. Actual:<[{nl}\tabc,{nl}\tdef,{nl}\tTest,{nl}\tghi{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void DoesNotContain_WithMessage_Null()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("Test", (IEnumerable<string>?)null, "This is my test"));
            MSAssert.AreEqual("Assert.DoesNotContain failed. NotExpected:<Test>. Actual:<(null)>. This is my test", ex.Message);
        }

        [TestMethod]
        public void DoesNotContain_WithMessage_Empty()
        {
            AssertUnderTest.DoesNotContain("Test", Array.Empty<string>(), "This is my test");
        }

        [TestMethod]
        public void DoesNotContain_WithMessage_NotContained()
        {
            AssertUnderTest.DoesNotContain("Test", new[] { "abc", "def", "ghi" }, "This is my test");
        }

        [TestMethod]
        public void DoesNotContain_WithMessage_Contained()
        {
            var nl = Environment.NewLine;
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("Test", new[] { "abc", "def", "Test", "ghi" }, "This is my test"));
            MSAssert.AreEqual($"Assert.DoesNotContain failed. NotExpected:<Test>. Actual:<[{nl}\tabc,{nl}\tdef,{nl}\tTest,{nl}\tghi{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void DoesNotContain_WithComparerT_Null()
        {
            using var comparer = CreateEqualityComparerT<string>();
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("Test", null, comparer));
            MSAssert.AreEqual("Assert.DoesNotContain failed. NotExpected:<Test>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void DoesNotContain_WithComparerT_Empty()
        {
            using var comparer = CreateEqualityComparerT<string>();
            AssertUnderTest.DoesNotContain("Test", Array.Empty<string>(), comparer);
        }

        [TestMethod]
        public void DoesNotContain_WithComparerT_NotContained()
        {
            using var comparer = CreateEqualityComparerT(("abc", "Test"), ("def", "Test"), ("ghi", "Test"));
            AssertUnderTest.DoesNotContain("Test", new[] { "abc", "def", "ghi" }, comparer);
        }

        [TestMethod]
        public void DoesNotContain_WithComparerT_Contained()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparerT(("abc", "Test"), ("def", "Test"), ("Test", "Test"));
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("Test", new[] { "abc", "def", "Test", "ghi" }, comparer));
            MSAssert.AreEqual($"Assert.DoesNotContain failed. NotExpected:<Test>. Actual:<[{nl}\tabc,{nl}\tdef,{nl}\tTest,{nl}\tghi{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void DoesNotContain_WithComparerT_WithMessage_Null()
        {
            using var comparer = CreateEqualityComparerT<string>();
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("Test", null, comparer, "This is my test"));
            MSAssert.AreEqual("Assert.DoesNotContain failed. NotExpected:<Test>. Actual:<(null)>. This is my test", ex.Message);
        }

        [TestMethod]
        public void DoesNotContain_WithComparerT_WithMessage_Empty()
        {
            using var comparer = CreateEqualityComparerT<string>();
            AssertUnderTest.DoesNotContain("Test", Array.Empty<string>(), comparer, "This is my test");
        }

        [TestMethod]
        public void DoesNotContain_WithComparerT_WithMessage_NotContained()
        {
            using var comparer = CreateEqualityComparerT(("abc", "Test"), ("def", "Test"), ("ghi", "Test"));
            AssertUnderTest.DoesNotContain("Test", new[] { "abc", "def", "ghi" }, comparer, "This is my test");
        }

        [TestMethod]
        public void DoesNotContain_WithComparerT_WithMessage_Contained()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparerT(("abc", "Test"), ("def", "Test"), ("Test", "Test"));
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("Test", new[] { "abc", "def", "Test", "ghi" }, comparer, "This is my test"));
            MSAssert.AreEqual($"Assert.DoesNotContain failed. NotExpected:<Test>. Actual:<[{nl}\tabc,{nl}\tdef,{nl}\tTest,{nl}\tghi{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void DoesNotContain_WithComparer_Null()
        {
            using var comparer = CreateEqualityComparer();
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("Test", null, comparer));
            MSAssert.AreEqual("Assert.DoesNotContain failed. NotExpected:<Test>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void DoesNotContain_WithComparer_Empty()
        {
            using var comparer = CreateEqualityComparer();
            AssertUnderTest.DoesNotContain("Test", Array.Empty<string>(), comparer);
        }

        [TestMethod]
        public void DoesNotContain_WithComparer_NotContained()
        {
            using var comparer = CreateEqualityComparer(("abc", "Test"), ("def", "Test"), ("ghi", "Test"));
            AssertUnderTest.DoesNotContain("Test", new[] { "abc", "def", "ghi" }, comparer);
        }

        [TestMethod]
        public void DoesNotContain_WithComparer_Contained()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparer(("abc", "Test"), ("def", "Test"), ("Test", "Test"));
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("Test", new[] { "abc", "def", "Test", "ghi" }, comparer));
            MSAssert.AreEqual($"Assert.DoesNotContain failed. NotExpected:<Test>. Actual:<[{nl}\tabc,{nl}\tdef,{nl}\tTest,{nl}\tghi{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void DoesNotContain_WithComparer_WithMessage_Null()
        {
            using var comparer = CreateEqualityComparer();
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("Test", null, comparer, "This is my test"));
            MSAssert.AreEqual("Assert.DoesNotContain failed. NotExpected:<Test>. Actual:<(null)>. This is my test", ex.Message);
        }

        [TestMethod]
        public void DoesNotContain_WithComparer_WithMessage_Empty()
        {
            using var comparer = CreateEqualityComparer();
            AssertUnderTest.DoesNotContain("Test", Array.Empty<string>(), comparer, "This is my test");
        }

        [TestMethod]
        public void DoesNotContain_WithComparer_WithMessage_NotContained()
        {
            using var comparer = CreateEqualityComparer(("abc", "Test"), ("def", "Test"), ("ghi", "Test"));
            AssertUnderTest.DoesNotContain("Test", new[] { "abc", "def", "ghi" }, comparer, "This is my test");
        }

        [TestMethod]
        public void DoesNotContain_WithComparer_WithMessage_Contained()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparer(("abc", "Test"), ("def", "Test"), ("Test", "Test"));
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("Test", new[] { "abc", "def", "Test", "ghi" }, comparer, "This is my test"));
            MSAssert.AreEqual($"Assert.DoesNotContain failed. NotExpected:<Test>. Actual:<[{nl}\tabc,{nl}\tdef,{nl}\tTest,{nl}\tghi{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void DoesNotContain_WithPredicate_Null()
        {
            using var predicate = CreatePredicate<string, int>();
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("Test", null, predicate.Object));
            MSAssert.AreEqual("Assert.DoesNotContain failed. NotExpected:<Test>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void DoesNotContain_WithPredicate_Empty()
        {
            using var predicate = CreatePredicate<string, int>();
            AssertUnderTest.DoesNotContain("Test", Array.Empty<int>(), predicate.Object);
        }

        [TestMethod]
        public void DoesNotContain_WithPredicate_NotContained()
        {
            using var predicate = CreatePredicate(("Test", 1, false), ("Test", 2, false), ("Test", 3, false));
            AssertUnderTest.DoesNotContain("Test", new[] { 1, 2, 3 }, predicate.Object);
        }

        [TestMethod]
        public void DoesNotContain_WithPredicate_Contained()
        {
            var nl = Environment.NewLine;
            using var predicate = CreatePredicate(("Test", 1, false), ("Test", 2, false), ("Test", 3, true));
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("Test", new[] { 1, 2, 3, 4 }, predicate.Object));
            MSAssert.AreEqual($"Assert.DoesNotContain failed. NotExpected:<Test>. Actual:<[{nl}\t1,{nl}\t2,{nl}\t3,{nl}\t4{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void DoesNotContain_WithPredicate_WithMessage_Null()
        {
            using var predicate = CreatePredicate<string, int>();
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("Test", null, predicate.Object, "This is my test"));
            MSAssert.AreEqual("Assert.DoesNotContain failed. NotExpected:<Test>. Actual:<(null)>. This is my test", ex.Message);
        }

        [TestMethod]
        public void DoesNotContain_WithPredicate_WithMessage_Empty()
        {
            using var predicate = CreatePredicate<string, int>();
            AssertUnderTest.DoesNotContain("Test", Array.Empty<int>(), predicate.Object, "This is my test");
        }

        [TestMethod]
        public void DoesNotContain_WithPredicate_WithMessage_NotContained()
        {
            using var predicate = CreatePredicate(("Test", 1, false), ("Test", 2, false), ("Test", 3, false));
            AssertUnderTest.DoesNotContain("Test", new[] { 1, 2, 3 }, predicate.Object, "This is my test");
        }

        [TestMethod]
        public void DoesNotContain_WithPredicate_WithMessage_Contained()
        {
            var nl = Environment.NewLine;
            using var predicate = CreatePredicate(("Test", 1, false), ("Test", 2, false), ("Test", 3, true));
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("Test", new[] { 1, 2, 3, 4 }, predicate.Object, "This is my test"));
            MSAssert.AreEqual($"Assert.DoesNotContain failed. NotExpected:<Test>. Actual:<[{nl}\t1,{nl}\t2,{nl}\t3,{nl}\t4{nl}]>. This is my test", ex.Message);
        }

        #endregion

        #region AllItemsAreNotNull

        [TestMethod]
        public void AllItemsAreNotNull_NullCollection()
        {
            MSAssert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.AllItemsAreNotNull(null!));
        }

        [TestMethod]
        public void AllItemsAreNotNull_Success()
        {
            AssertUnderTest.AllItemsAreNotNull(new[] { "Test", "Blub" });
        }

        [TestMethod]
        public void AllItemsAreNotNull_Fail_OneNull()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreNotNull(new[] { "Test", null }));
            MSAssert.AreEqual("Assert.AllItemsAreNotNull failed. Index:<1>.", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreNotNull_Fail_MultipleNull()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreNotNull(new[] { "Test", null, "Blub", null }));
            MSAssert.AreEqual("Assert.AllItemsAreNotNull failed. Indices:<1, 3>.", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreNotNull_Fail_OneNull_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreNotNull(new[] { "Test", null }, "This is my test"));
            MSAssert.AreEqual("Assert.AllItemsAreNotNull failed. Index:<1>. This is my test", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreNotNull_Fail_MultipleNull_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreNotNull(new[] { "Test", null, "Blub", null }, "This is my test"));
            MSAssert.AreEqual("Assert.AllItemsAreNotNull failed. Indices:<1, 3>. This is my test", ex.Message);
        }

        #endregion

        #region AllItemsAreUnique

        [TestMethod]
        public void AllItemsAreUnique_NullCollection()
        {
            MSAssert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.AllItemsAreUnique<string>(null!));
        }

        [TestMethod]
        public void AllItemsAreUnique_NullComparer()
        {
            MSAssert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.AllItemsAreUnique(Array.Empty<string>(), (IEqualityComparer<string>)null!));
        }

        [TestMethod]
        public void AllItemsAreUnique_Success()
        {
            AssertUnderTest.AllItemsAreUnique(new[] { "Test", "Blub", "Hello" });
        }

        [TestMethod]
        public void AllItemsAreUnique_Fail_OneDuplicate()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreUnique(new[] { "Test", "Blub", "Test", "Hello" }));
            MSAssert.AreEqual("Assert.AllItemsAreUnique failed. Index:<0=2>.", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreUnique_Fail_OneDuplicate_Nulls()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreUnique(new[] { null, "Blub", null, "Hello" }));
            MSAssert.AreEqual("Assert.AllItemsAreUnique failed. Index:<0=2>.", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreUnique_Fail_MultipleDuplicate()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreUnique(new[] { "Test", "Blub", "Test", "Blub" }));
            MSAssert.AreEqual("Assert.AllItemsAreUnique failed. Indices:<0=2, 1=3>.", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreUnique_Fail_MultipleSameDuplicate()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreUnique(new[] { "Test", "Blub", "Test", "Test" }));
            MSAssert.AreEqual("Assert.AllItemsAreUnique failed. Indices:<0=2, 0=3>.", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreUnique_Fail_OneDuplicate_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreUnique(new[] { "Test", "Blub", "Test", "Hello" }, "This is my test"));
            MSAssert.AreEqual("Assert.AllItemsAreUnique failed. Index:<0=2>. This is my test", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreUnique_Fail_OneDuplicate_Nulls_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreUnique(new[] { null, "Blub", null, "Hello" }, "This is my test"));
            MSAssert.AreEqual("Assert.AllItemsAreUnique failed. Index:<0=2>. This is my test", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreUnique_Fail_MultipleDuplicate_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreUnique(new[] { "Test", "Blub", "Test", "Blub" }, "This is my test"));
            MSAssert.AreEqual("Assert.AllItemsAreUnique failed. Indices:<0=2, 1=3>. This is my test", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreUnique_Fail_MultipleSameDuplicate_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreUnique(new[] { "Test", "Blub", "Test", "Test" }, "This is my test"));
            MSAssert.AreEqual("Assert.AllItemsAreUnique failed. Indices:<0=2, 0=3>. This is my test", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreUnique_WithComparer_Success()
        {
            using var comparer = CreateEqualityComparerTForHash("Test", "Blub", "Hello");
            AssertUnderTest.AllItemsAreUnique(new[] { "Test", "Blub", "Hello" }, comparer);
        }

        [TestMethod]
        public void AllItemsAreUnique_WithComparer_Fail_OneDuplicate()
        {
            using var comparer = CreateEqualityComparerTForHash("Test", "Blub", "Test", "Hello");
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreUnique(new[] { "Test", "Blub", "Test", "Hello" }, comparer));
            MSAssert.AreEqual("Assert.AllItemsAreUnique failed. Index:<0=2>.", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreUnique_WithComparer_Fail_OneDuplicate_Nulls()
        {
            using var comparer = CreateEqualityComparerTForHash<string?>("Blub", "Hello");
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreUnique(new[] { null, "Blub", null, "Hello" }, comparer));
            MSAssert.AreEqual("Assert.AllItemsAreUnique failed. Index:<0=2>.", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreUnique_WithComparer_Fail_MultipleDuplicate()
        {
            using var comparer = CreateEqualityComparerTForHash("Test", "Blub", "Test", "Blub");
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreUnique(new[] { "Test", "Blub", "Test", "Blub" }, comparer));
            MSAssert.AreEqual("Assert.AllItemsAreUnique failed. Indices:<0=2, 1=3>.", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreUnique_WithComparer_Fail_MultipleSameDuplicate()
        {
            using var comparer = CreateEqualityComparerTForHash("Test", "Blub", "Test", "Test");
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreUnique(new[] { "Test", "Blub", "Test", "Test" }, comparer));
            MSAssert.AreEqual("Assert.AllItemsAreUnique failed. Indices:<0=2, 0=3>.", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreUnique_WithComparer_Fail_OneDuplicate_WithMessage()
        {
            using var comparer = CreateEqualityComparerTForHash("Test", "Blub", "Test", "Hello");
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreUnique(new[] { "Test", "Blub", "Test", "Hello" }, comparer, "This is my test"));
            MSAssert.AreEqual("Assert.AllItemsAreUnique failed. Index:<0=2>. This is my test", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreUnique_WithComparer_Fail_OneDuplicate_Nulls_WithMessage()
        {
            using var comparer = CreateEqualityComparerTForHash<string?>("Blub", "Hello");
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreUnique(new[] { null, "Blub", null, "Hello" }, comparer, "This is my test"));
            MSAssert.AreEqual("Assert.AllItemsAreUnique failed. Index:<0=2>. This is my test", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreUnique_WithComparer_Fail_MultipleDuplicate_WithMessage()
        {
            using var comparer = CreateEqualityComparerTForHash("Test", "Blub", "Test", "Blub");
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreUnique(new[] { "Test", "Blub", "Test", "Blub" }, comparer, "This is my test"));
            MSAssert.AreEqual("Assert.AllItemsAreUnique failed. Indices:<0=2, 1=3>. This is my test", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreUnique_WithComparer_Fail_MultipleSameDuplicate_WithMessage()
        {
            using var comparer = CreateEqualityComparerTForHash("Test", "Blub", "Test", "Test");
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreUnique(new[] { "Test", "Blub", "Test", "Test" }, comparer, "This is my test"));
            MSAssert.AreEqual("Assert.AllItemsAreUnique failed. Indices:<0=2, 0=3>. This is my test", ex.Message);
        }

        #endregion

        #region IsSubsetOf

        [TestMethod]
        public void IsSubsetOf_NullSubset()
        {
            MSAssert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.IsSubsetOf(null!, Array.Empty<string>()));
        }

        [TestMethod]
        public void IsSubsetOf_NullSuperset()
        {
            MSAssert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.IsSubsetOf(Array.Empty<string>(), null!));
        }

        [TestMethod]
        public void IsSubsetOf_NullComparer()
        {
            MSAssert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.IsSubsetOf(Array.Empty<string>(), Array.Empty<string>(), (IEqualityComparer<string>)null!));
        }

        [TestMethod]
        public void IsSubsetOf_Success()
        {
            AssertUnderTest.IsSubsetOf(new string[] { "Test", "blub" }, new string[] { "Hello", "blub", "bbbb", "Test" });
        }

        [TestMethod]
        public void IsSubsetOf_Fail_OneMissingItem()
        {
            var nl = Environment.NewLine;
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsSubsetOf(new[] { "Test", "blub" }, new[] { "Hello", "blub", "bbb" }));
            MSAssert.AreEqual($"Assert.IsSubsetOf failed. MissingItems:<[{nl}\tTest{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void IsSubsetOf_Fail_MultipleMissingItem()
        {
            var nl = Environment.NewLine;
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsSubsetOf(new[] { "Test", "blub" }, new[] { "Hello", "bbb" }));
            MSAssert.AreEqual($"Assert.IsSubsetOf failed. MissingItems:<[{nl}\tTest,{nl}\tblub{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void IsSubsetOf_Fail_WithMessage_OneMissingItem()
        {
            var nl = Environment.NewLine;
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsSubsetOf(new[] { "Test", "blub" }, new[] { "Hello", "blub", "bbb" }, "This is my test"));
            MSAssert.AreEqual($"Assert.IsSubsetOf failed. MissingItems:<[{nl}\tTest{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsSubsetOf_Fail_WithMessage_MultipleMissingItem()
        {
            var nl = Environment.NewLine;
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsSubsetOf(new[] { "Test", "blub" }, new[] { "Hello", "bbb" }, "This is my test"));
            MSAssert.AreEqual($"Assert.IsSubsetOf failed. MissingItems:<[{nl}\tTest,{nl}\tblub{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsSubsetOf_WithComparer_Success()
        {
            using var comparer = CreateEqualityComparerTForHash("Hello", "blub", "bbbb", "Test", "Test", "blub");
            AssertUnderTest.IsSubsetOf(new string[] { "Test", "blub" }, new string[] { "Hello", "blub", "bbbb", "Test" }, comparer);
        }

        [TestMethod]
        public void IsSubsetOf_WithComparer_Fail_OneMissingItem()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparerTForHash("Hello", "blub", "bbb", "Test", "blub");
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsSubsetOf(new[] { "Test", "blub" }, new[] { "Hello", "blub", "bbb" }, comparer));
            MSAssert.AreEqual($"Assert.IsSubsetOf failed. MissingItems:<[{nl}\tTest{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void IsSubsetOf_WithComparer_Fail_MultipleMissingItem()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparerTForHash("Hello", "bbb", "Test", "blub");
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsSubsetOf(new[] { "Test", "blub" }, new[] { "Hello", "bbb" }, comparer));
            MSAssert.AreEqual($"Assert.IsSubsetOf failed. MissingItems:<[{nl}\tTest,{nl}\tblub{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void IsSubsetOf_WithComparer_Fail_WithMessage_OneMissingItem()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparerTForHash("Hello", "blub", "bbb", "Test", "blub");
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsSubsetOf(new[] { "Test", "blub" }, new[] { "Hello", "blub", "bbb" }, comparer, "This is my test"));
            MSAssert.AreEqual($"Assert.IsSubsetOf failed. MissingItems:<[{nl}\tTest{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsSubsetOf_WithComparer_Fail_WithMessage_MultipleMissingItem()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparerTForHash("Hello", "bbb", "Test", "blub");
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsSubsetOf(new[] { "Test", "blub" }, new[] { "Hello", "bbb" }, comparer, "This is my test"));
            MSAssert.AreEqual($"Assert.IsSubsetOf failed. MissingItems:<[{nl}\tTest,{nl}\tblub{nl}]>. This is my test", ex.Message);
        }

        #endregion

        #region IsNotSubsetOf

        [TestMethod]
        public void IsNotSubsetOf_NullSubset()
        {
            MSAssert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.IsNotSubsetOf(null!, Array.Empty<string>()));
        }

        [TestMethod]
        public void IsNotSubsetOf_NullSuperset()
        {
            MSAssert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.IsNotSubsetOf(Array.Empty<string>(), null!));
        }

        [TestMethod]
        public void IsNotSubsetOf_NullComparer()
        {
            MSAssert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.IsNotSubsetOf(Array.Empty<string>(), Array.Empty<string>(), (IEqualityComparer<string>)null!));
        }

        [TestMethod]
        public void IsNotSubsetOf_Success()
        {
            AssertUnderTest.IsNotSubsetOf(new[] { "Test", "blub" }, new[] { "Hello", "blub", "bbb" });
        }

        [TestMethod]
        public void IsNotSubsetOf_Fail()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotSubsetOf(new[] { "Test", "blub" }, new[] { "Hello", "blub", "bbb", "Test" }));
            MSAssert.AreEqual($"Assert.IsNotSubsetOf failed.", ex.Message);
        }

        [TestMethod]
        public void IsNotSubsetOf_Fail_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotSubsetOf(new[] { "Test", "blub" }, new[] { "Hello", "blub", "bbb", "Test" }, "This is my test"));
            MSAssert.AreEqual($"Assert.IsNotSubsetOf failed. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsNotSubsetOf_WithComparer_Success()
        {
            using var comparer = CreateEqualityComparerTForHash("Hello", "blub", "bbb", "Test", "blub");
            AssertUnderTest.IsNotSubsetOf(new[] { "Test", "blub" }, new[] { "Hello", "blub", "bbb" }, comparer);
        }

        [TestMethod]
        public void IsNotSubsetOf_WithComparer_Fail()
        {
            using var comparer = CreateEqualityComparerTForHash("Hello", "blub", "bbb", "Test", "Test", "blub");
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotSubsetOf(new[] { "Test", "blub" }, new[] { "Hello", "blub", "bbb", "Test" }, comparer));
            MSAssert.AreEqual($"Assert.IsNotSubsetOf failed.", ex.Message);
        }

        [TestMethod]
        public void IsNotSubsetOf_WithComparer_Fail_WithMessage()
        {
            using var comparer = CreateEqualityComparerTForHash("Hello", "blub", "bbb", "Test", "Test", "blub");
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotSubsetOf(new[] { "Test", "blub" }, new[] { "Hello", "blub", "bbb", "Test" }, comparer, "This is my test"));
            MSAssert.AreEqual($"Assert.IsNotSubsetOf failed. This is my test", ex.Message);
        }

        #endregion

        #region AreCollectionsEquivalent

        [TestMethod]
        public void AreCollectionsEquivalent_NullSubset()
        {
            MSAssert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.AreCollectionsEquivalent(null!, Array.Empty<string>()));
        }

        [TestMethod]
        public void AreCollectionsEquivalent_NullSuperset()
        {
            MSAssert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.AreCollectionsEquivalent(Array.Empty<string>(), null!));
        }

        [TestMethod]
        public void AreCollectionsEquivalent_NullComparer()
        {
            MSAssert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.AreCollectionsEquivalent(Array.Empty<string>(), Array.Empty<string>(), (IEqualityComparer<string>)null!));
        }

        [TestMethod]
        public void AreCollectionsEquivalent_Success()
        {
            AssertUnderTest.AreCollectionsEquivalent(new[] { "Test", "blub", "Hello" }, new[] { "Hello", "Test", "blub" });
        }

        [TestMethod]
        public void AreCollectionsEquivalent_Fail_MissingInExpected()
        {
            var nl = Environment.NewLine;
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsEquivalent(new[] { "Test", "blub" }, new[] { "Hello", "blub", "Test" }));
            MSAssert.AreEqual($"Assert.AreCollectionsEquivalent failed. UnexpectedItems:<[{nl}\tHello{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void AreCollectionsEquivalent_Fail_MissingInActual()
        {
            var nl = Environment.NewLine;
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsEquivalent(new[] { "Test", "blub", "Hello" }, new[] { "blub", "Test" }));
            MSAssert.AreEqual($"Assert.AreCollectionsEquivalent failed. MissingItems:<[{nl}\tHello{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void AreCollectionsEquivalent_Fail_MissingInExpectedAndActual()
        {
            var nl = Environment.NewLine;
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsEquivalent(new[] { "Test", "Hello" }, new[] { "blub", "Test" }));
            MSAssert.AreEqual($"Assert.AreCollectionsEquivalent failed. MissingItems:<[{nl}\tHello{nl}]>. UnexpectedItems:<[{nl}\tblub{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void AreCollectionsEquivalent_Fail_MissingInExpected_WithMessage()
        {
            var nl = Environment.NewLine;
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsEquivalent(new[] { "Test", "blub" }, new[] { "Hello", "blub", "Test" }, "This is my test"));
            MSAssert.AreEqual($"Assert.AreCollectionsEquivalent failed. UnexpectedItems:<[{nl}\tHello{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void AreCollectionsEquivalent_Fail_MissingInActual_WithMessage()
        {
            var nl = Environment.NewLine;
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsEquivalent(new[] { "Test", "blub", "Hello" }, new[] { "blub", "Test" }, "This is my test"));
            MSAssert.AreEqual($"Assert.AreCollectionsEquivalent failed. MissingItems:<[{nl}\tHello{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void AreCollectionsEquivalent_Fail_MissingInExpectedAndActual_WithMessage()
        {
            var nl = Environment.NewLine;
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsEquivalent(new[] { "Test", "Hello" }, new[] { "blub", "Test" }, "This is my test"));
            MSAssert.AreEqual($"Assert.AreCollectionsEquivalent failed. MissingItems:<[{nl}\tHello{nl}]>. UnexpectedItems:<[{nl}\tblub{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void AreCollectionsEquivalent_WithComparer_Success()
        {
            using var comparer = CreateEqualityComparerTForHash(false, "Test", "blub", "Hello", "Hello", "Test", "blub");
            AssertUnderTest.AreCollectionsEquivalent(new[] { "Test", "blub", "Hello" }, new[] { "Hello", "Test", "blub" }, comparer);
        }

        [TestMethod]
        public void AreCollectionsEquivalent_WithComparer_Fail_MissingInExpected()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparerTForHash(false, "Test", "blub", "Hello", "Test", "blub");
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsEquivalent(new[] { "Test", "blub" }, new[] { "Hello", "blub", "Test" }, comparer));
            MSAssert.AreEqual($"Assert.AreCollectionsEquivalent failed. UnexpectedItems:<[{nl}\tHello{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void AreCollectionsEquivalent_WithComparer_Fail_MissingInActual()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparerTForHash(false, "Test", "blub", "Hello", "Test", "blub");
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsEquivalent(new[] { "Test", "blub", "Hello" }, new[] { "blub", "Test" }, comparer));
            MSAssert.AreEqual($"Assert.AreCollectionsEquivalent failed. MissingItems:<[{nl}\tHello{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void AreCollectionsEquivalent_WithComparer_Fail_MissingInExpectedAndActual()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparerTForHash(false, "Test", "Hello", "Test", "blub");
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsEquivalent(new[] { "Test", "Hello" }, new[] { "blub", "Test" }, comparer));
            MSAssert.AreEqual($"Assert.AreCollectionsEquivalent failed. MissingItems:<[{nl}\tHello{nl}]>. UnexpectedItems:<[{nl}\tblub{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void AreCollectionsEquivalent_WithComparer_Fail_MissingInExpected_WithMessage()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparerTForHash(false, "Test", "blub", "Hello", "Test", "blub");
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsEquivalent(new[] { "Test", "blub" }, new[] { "Hello", "blub", "Test" }, comparer, "This is my test"));
            MSAssert.AreEqual($"Assert.AreCollectionsEquivalent failed. UnexpectedItems:<[{nl}\tHello{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void AreCollectionsEquivalent_WithComparer_Fail_MissingInActual_WithMessage()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparerTForHash(false, "Test", "blub", "Hello", "Test", "blub");
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsEquivalent(new[] { "Test", "blub", "Hello" }, new[] { "blub", "Test" }, comparer, "This is my test"));
            MSAssert.AreEqual($"Assert.AreCollectionsEquivalent failed. MissingItems:<[{nl}\tHello{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void AreCollectionsEquivalent_WithComparer_Fail_MissingInExpectedAndActual_WithMessage()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparerTForHash(false, "Test", "Hello", "Test", "blub");
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsEquivalent(new[] { "Test", "Hello" }, new[] { "blub", "Test" }, comparer, "This is my test"));
            MSAssert.AreEqual($"Assert.AreCollectionsEquivalent failed. MissingItems:<[{nl}\tHello{nl}]>. UnexpectedItems:<[{nl}\tblub{nl}]>. This is my test", ex.Message);
        }

        #endregion

        #region AreCollectionsNotEquivalent

        [TestMethod]
        public void AreCollectionsNotEquivalent_NullSubset()
        {
            MSAssert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.AreCollectionsNotEquivalent(null!, Array.Empty<string>()));
        }

        [TestMethod]
        public void AreCollectionsNotEquivalent_NullSuperset()
        {
            MSAssert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.AreCollectionsNotEquivalent(Array.Empty<string>(), null!));
        }

        [TestMethod]
        public void AreCollectionsNotEquivalent_NullComparer()
        {
            MSAssert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.AreCollectionsNotEquivalent(Array.Empty<string>(), Array.Empty<string>(), (IEqualityComparer<string>)null!));
        }

        [TestMethod]
        public void AreCollectionsNotEquivalent_Success_MissingInExpected()
        {
            AssertUnderTest.AreCollectionsNotEquivalent(new[] { "Test", "blub" }, new[] { "Hello", "blub", "Test" });
        }

        [TestMethod]
        public void AreCollectionsNotEquivalent_Success_MissingInActual()
        {
            AssertUnderTest.AreCollectionsNotEquivalent(new[] { "Test", "blub", "Hello" }, new[] { "blub", "Test" });
        }

        [TestMethod]
        public void AreCollectionsNotEquivalent_Success_MissingInExpectedAndActual()
        {
            AssertUnderTest.AreCollectionsNotEquivalent(new[] { "Test", "Hello" }, new[] { "blub", "Test" });
        }

        [TestMethod]
        public void AreCollectionsNotEquivalent_Fail()
        {
            var nl = Environment.NewLine;
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsNotEquivalent(new[] { "Test", "blub", "Hello" }, new[] { "Hello", "Test", "blub" }));
            MSAssert.AreEqual($"Assert.AreCollectionsNotEquivalent failed.", ex.Message);
        }

        [TestMethod]
        public void AreCollectionsNotEquivalent_Fail_WithMessage()
        {
            var nl = Environment.NewLine;
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsNotEquivalent(new[] { "Test", "blub", "Hello" }, new[] { "Hello", "Test", "blub" }, "This is my test"));
            MSAssert.AreEqual($"Assert.AreCollectionsNotEquivalent failed. This is my test", ex.Message);
        }

        [TestMethod]
        public void AreCollectionsNotEquivalent_WithComparer_Success_MissingInExpected()
        {
            using var comparer = CreateEqualityComparerTForHash(false, "Test", "blub", "Hello", "Test", "blub");
            AssertUnderTest.AreCollectionsNotEquivalent(new[] { "Test", "blub" }, new[] { "Hello", "blub", "Test" }, comparer);
        }

        [TestMethod]
        public void AreCollectionsNotEquivalent_WithComparer_Success_MissingInActual()
        {
            using var comparer = CreateEqualityComparerTForHash(false, "Test", "blub", "Hello", "Test", "blub");
            AssertUnderTest.AreCollectionsNotEquivalent(new[] { "Test", "blub", "Hello" }, new[] { "blub", "Test" }, comparer);
        }

        [TestMethod]
        public void AreCollectionsNotEquivalent_WithComparer_Success_MissingInExpectedAndActual()
        {
            using var comparer = CreateEqualityComparerTForHash(false, "Test", "Hello", "Test", "blub");
            AssertUnderTest.AreCollectionsNotEquivalent(new[] { "Test", "Hello" }, new[] { "blub", "Test" }, comparer);
        }

        [TestMethod]
        public void AreCollectionsNotEquivalent_WithComparer_Fail()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparerTForHash(false, "Test", "blub", "Hello", "Hello", "Test", "blub");
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsNotEquivalent(new[] { "Test", "blub", "Hello" }, new[] { "Hello", "Test", "blub" }, comparer));
            MSAssert.AreEqual($"Assert.AreCollectionsNotEquivalent failed.", ex.Message);
        }

        [TestMethod]
        public void AreCollectionsNotEquivalent_WithComparer_Fail_WithMessage()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparerTForHash(false, "Test", "blub", "Hello", "Hello", "Test", "blub");
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsNotEquivalent(new[] { "Test", "blub", "Hello" }, new[] { "Hello", "Test", "blub" }, comparer, "This is my test"));
            MSAssert.AreEqual($"Assert.AreCollectionsNotEquivalent failed. This is my test", ex.Message);
        }

        #endregion

        #region AllItemsAreInstancesOfType

        [TestMethod]
        public void AllItemsAreInstancesOfType_NullCollection()
        {
            MSAssert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.AllItemsAreInstancesOfType(null!, typeof(string)));
        }

        [TestMethod]
        public void AllItemsAreInstancesOfType_NullExpectedType()
        {
            MSAssert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.AllItemsAreInstancesOfType(Array.Empty<string>(), null!));
        }

        [TestMethod]
        public void AllItemsAreInstancesOfType_Success()
        {
            AssertUnderTest.AllItemsAreInstancesOfType(new object[] { new TestBaseClass(), new TestClass() }, typeof(ITestInterface));
        }

        [TestMethod]
        public void AllItemsAreInstancesOfType_Fail_SingleItem()
        {
            var nl = Environment.NewLine;
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreInstancesOfType(new object[] { "Test", 3, "Hello" }, typeof(string)));
            MSAssert.AreEqual($"Assert.AllItemsAreInstancesOfType failed. ExpectedType:<System.String>. WrongItems:<[{nl}\t[1] 3 (Type: System.Int32){nl}]>.", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreInstancesOfType_Fail_MultipleItems()
        {
            var nl = Environment.NewLine;
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreInstancesOfType(new object?[] { "Test", 3, null }, typeof(string)));
            MSAssert.AreEqual($"Assert.AllItemsAreInstancesOfType failed. ExpectedType:<System.String>. WrongItems:<[{nl}\t[1] 3 (Type: System.Int32),{nl}\t[2] (null) (Type: (null)){nl}]>.", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreInstancesOfType_Fail_SingleItem_WithMessage()
        {
            var nl = Environment.NewLine;
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreInstancesOfType(new object[] { "Test", 3, "Hello" }, typeof(string), "This is my test"));
            MSAssert.AreEqual($"Assert.AllItemsAreInstancesOfType failed. ExpectedType:<System.String>. WrongItems:<[{nl}\t[1] 3 (Type: System.Int32){nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreInstancesOfType_Fail_MultipleItems_WithMessage()
        {
            var nl = Environment.NewLine;
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreInstancesOfType(new object?[] { "Test", 3, null }, typeof(string), "This is my test"));
            MSAssert.AreEqual($"Assert.AllItemsAreInstancesOfType failed. ExpectedType:<System.String>. WrongItems:<[{nl}\t[1] 3 (Type: System.Int32),{nl}\t[2] (null) (Type: (null)){nl}]>. This is my test", ex.Message);
        }

        #endregion

        #region AreCollectionsEqual

        [TestMethod]
        public void AreCollectionsEqual_NullExpected()
        {
            MSAssert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.AreCollectionsEqual(null!, Array.Empty<string>()));
        }

        [TestMethod]
        public void AreCollectionsEqual_NullActual()
        {
            MSAssert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.AreCollectionsEqual(Array.Empty<string>(), null!));
        }

        [TestMethod]
        public void AreCollectionsEqual_NullComparer()
        {
            MSAssert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.AreCollectionsEqual(Array.Empty<string>(), Array.Empty<string>(), (IEqualityComparer<string>)null!));
        }

        [TestMethod]
        public void AreCollectionsEqual_Success()
        {
            AssertUnderTest.AreCollectionsEqual(new[] { "Test", "Hello", "blub" }, new[] { "Test", "Hello", "blub" });
        }

        [TestMethod]
        public void AreCollectionsEqual_Fail_MismatchingItem()
        {
            var nl = Environment.NewLine;
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsEqual(new string[] { "Test", "Hello", "blub" }, new[] { "Test", "blub", "Hello" }));
            MSAssert.AreEqual($"Assert.AreCollectionsEqual failed. WrongItems:<[{nl}\t[1] Expected:<Hello> Actual:<blub>,{nl}\t[2] Expected:<blub> Actual:<Hello>{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void AreCollectionsEqual_Fail_MissingItem()
        {
            var nl = Environment.NewLine;
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsEqual(new string[] { "Test", "Hello", "blub" }, new[] { "Test", "Hello" }));
            MSAssert.AreEqual($"Assert.AreCollectionsEqual failed. MissingItems:<[{nl}\tblub{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void AreCollectionsEqual_Fail_UnexpectedItem()
        {
            var nl = Environment.NewLine;
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsEqual(new string[] { "Test", "Hello" }, new[] { "Test", "Hello", "blub" }));
            MSAssert.AreEqual($"Assert.AreCollectionsEqual failed. UnexpectedItems:<[{nl}\tblub{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void AreCollectionsEqual_Fail_MismatchingAndMissingItems()
        {
            var nl = Environment.NewLine;
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsEqual(new string[] { "Test", "Hello", "blub", "blub2" }, new[] { "Test", "bbb" }));
            MSAssert.AreEqual($"Assert.AreCollectionsEqual failed. WrongItems:<[{nl}\t[1] Expected:<Hello> Actual:<bbb>{nl}]>. MissingItems:<[{nl}\tblub,{nl}\tblub2{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void AreCollectionsEqual_Fail_MismatchingAndUnexpectedItems()
        {
            var nl = Environment.NewLine;
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsEqual(new string[] { "Test", "Hello" }, new[] { "Test", "bbb", "blub", "blub2" }));
            MSAssert.AreEqual($"Assert.AreCollectionsEqual failed. WrongItems:<[{nl}\t[1] Expected:<Hello> Actual:<bbb>{nl}]>. UnexpectedItems:<[{nl}\tblub,{nl}\tblub2{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void AreCollectionsEqual_Fail_MismatchingItem_WithMessage()
        {
            var nl = Environment.NewLine;
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsEqual(new string[] { "Test", "Hello", "blub" }, new[] { "Test", "blub", "Hello" }, "This is my test"));
            MSAssert.AreEqual($"Assert.AreCollectionsEqual failed. WrongItems:<[{nl}\t[1] Expected:<Hello> Actual:<blub>,{nl}\t[2] Expected:<blub> Actual:<Hello>{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void AreCollectionsEqual_Fail_MissingItem_WithMessage()
        {
            var nl = Environment.NewLine;
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsEqual(new string[] { "Test", "Hello", "blub" }, new[] { "Test", "Hello" }, "This is my test"));
            MSAssert.AreEqual($"Assert.AreCollectionsEqual failed. MissingItems:<[{nl}\tblub{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void AreCollectionsEqual_Fail_UnexpectedItem_WithMessage()
        {
            var nl = Environment.NewLine;
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsEqual(new string[] { "Test", "Hello" }, new[] { "Test", "Hello", "blub" }, "This is my test"));
            MSAssert.AreEqual($"Assert.AreCollectionsEqual failed. UnexpectedItems:<[{nl}\tblub{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void AreCollectionsEqual_Fail_MismatchingAndMissingItems_WithMessage()
        {
            var nl = Environment.NewLine;
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsEqual(new string[] { "Test", "Hello", "blub", "blub2" }, new[] { "Test", "bbb" }, "This is my test"));
            MSAssert.AreEqual($"Assert.AreCollectionsEqual failed. WrongItems:<[{nl}\t[1] Expected:<Hello> Actual:<bbb>{nl}]>. MissingItems:<[{nl}\tblub,{nl}\tblub2{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void AreCollectionsEqual_Fail_MismatchingAndUnexpectedItems_WithMessage()
        {
            var nl = Environment.NewLine;
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsEqual(new string[] { "Test", "Hello" }, new[] { "Test", "bbb", "blub", "blub2" }, "This is my test"));
            MSAssert.AreEqual($"Assert.AreCollectionsEqual failed. WrongItems:<[{nl}\t[1] Expected:<Hello> Actual:<bbb>{nl}]>. UnexpectedItems:<[{nl}\tblub,{nl}\tblub2{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void AreCollectionsEqual_WithComparer_Success()
        {
            using var comparer = CreateEqualityComparerT(("Test", "Test"), ("Hello", "Hello"), ("blub", "blub"));
            AssertUnderTest.AreCollectionsEqual(new[] { "Test", "Hello", "blub" }, new[] { "Test", "Hello", "blub" }, comparer);
        }

        [TestMethod]
        public void AreCollectionsEqual_WithComparer_Fail_MismatchingItem()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparerT(("Test", "Test"), ("blub", "Hello"), ("Hello", "blub"));
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsEqual(new string[] { "Test", "Hello", "blub" }, new[] { "Test", "blub", "Hello" }, comparer));
            MSAssert.AreEqual($"Assert.AreCollectionsEqual failed. WrongItems:<[{nl}\t[1] Expected:<Hello> Actual:<blub>,{nl}\t[2] Expected:<blub> Actual:<Hello>{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void AreCollectionsEqual_WithComparer_Fail_MissingItem()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparerT(("Test", "Test"), ("Hello", "Hello"));
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsEqual(new string[] { "Test", "Hello", "blub" }, new[] { "Test", "Hello" }, comparer));
            MSAssert.AreEqual($"Assert.AreCollectionsEqual failed. MissingItems:<[{nl}\tblub{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void AreCollectionsEqual_WithComparer_Fail_UnexpectedItem()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparerT(("Test", "Test"), ("Hello", "Hello"));
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsEqual(new string[] { "Test", "Hello" }, new[] { "Test", "Hello", "blub" }, comparer));
            MSAssert.AreEqual($"Assert.AreCollectionsEqual failed. UnexpectedItems:<[{nl}\tblub{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void AreCollectionsEqual_WithComparer_Fail_MismatchingAndMissingItems()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparerT(("Test", "Test"), ("bbb", "Hello"));
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsEqual(new string[] { "Test", "Hello", "blub", "blub2" }, new[] { "Test", "bbb" }, comparer));
            MSAssert.AreEqual($"Assert.AreCollectionsEqual failed. WrongItems:<[{nl}\t[1] Expected:<Hello> Actual:<bbb>{nl}]>. MissingItems:<[{nl}\tblub,{nl}\tblub2{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void AreCollectionsEqual_WithComparer_Fail_MismatchingAndUnexpectedItems()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparerT(("Test", "Test"), ("bbb", "Hello"));
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsEqual(new string[] { "Test", "Hello" }, new[] { "Test", "bbb", "blub", "blub2" }, comparer));
            MSAssert.AreEqual($"Assert.AreCollectionsEqual failed. WrongItems:<[{nl}\t[1] Expected:<Hello> Actual:<bbb>{nl}]>. UnexpectedItems:<[{nl}\tblub,{nl}\tblub2{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void AreCollectionsEqual_WithComparer_Fail_MismatchingItem_WithMessage()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparerT(("Test", "Test"), ("blub", "Hello"), ("Hello", "blub"));
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsEqual(new string[] { "Test", "Hello", "blub" }, new[] { "Test", "blub", "Hello" }, comparer, "This is my test"));
            MSAssert.AreEqual($"Assert.AreCollectionsEqual failed. WrongItems:<[{nl}\t[1] Expected:<Hello> Actual:<blub>,{nl}\t[2] Expected:<blub> Actual:<Hello>{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void AreCollectionsEqual_WithComparer_Fail_MissingItem_WithMessage()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparerT(("Test", "Test"), ("Hello", "Hello"));
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsEqual(new string[] { "Test", "Hello", "blub" }, new[] { "Test", "Hello" }, comparer, "This is my test"));
            MSAssert.AreEqual($"Assert.AreCollectionsEqual failed. MissingItems:<[{nl}\tblub{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void AreCollectionsEqual_WithComparer_Fail_UnexpectedItem_WithMessage()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparerT(("Test", "Test"), ("Hello", "Hello"));
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsEqual(new string[] { "Test", "Hello" }, new[] { "Test", "Hello", "blub" }, comparer, "This is my test"));
            MSAssert.AreEqual($"Assert.AreCollectionsEqual failed. UnexpectedItems:<[{nl}\tblub{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void AreCollectionsEqual_WithComparer_Fail_MismatchingAndMissingItems_WithMessage()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparerT(("Test", "Test"), ("bbb", "Hello"));
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsEqual(new string[] { "Test", "Hello", "blub", "blub2" }, new[] { "Test", "bbb" }, comparer, "This is my test"));
            MSAssert.AreEqual($"Assert.AreCollectionsEqual failed. WrongItems:<[{nl}\t[1] Expected:<Hello> Actual:<bbb>{nl}]>. MissingItems:<[{nl}\tblub,{nl}\tblub2{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void AreCollectionsEqual_WithComparer_Fail_MismatchingAndUnexpectedItems_WithMessage()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparerT(("Test", "Test"), ("bbb", "Hello"));
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsEqual(new string[] { "Test", "Hello" }, new[] { "Test", "bbb", "blub", "blub2" }, comparer, "This is my test"));
            MSAssert.AreEqual($"Assert.AreCollectionsEqual failed. WrongItems:<[{nl}\t[1] Expected:<Hello> Actual:<bbb>{nl}]>. UnexpectedItems:<[{nl}\tblub,{nl}\tblub2{nl}]>. This is my test", ex.Message);
        }

        #endregion

        #region AreCollectionsNotEqual

        [TestMethod]
        public void AreCollectionsNotEqual_NullExpected()
        {
            MSAssert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.AreCollectionsNotEqual(null!, Array.Empty<string>()));
        }

        [TestMethod]
        public void AreCollectionsNotEqual_NullActual()
        {
            MSAssert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.AreCollectionsNotEqual(Array.Empty<string>(), null!));
        }

        [TestMethod]
        public void AreCollectionsNotEqual_NullComparer()
        {
            MSAssert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.AreCollectionsNotEqual(Array.Empty<string>(), Array.Empty<string>(), (IEqualityComparer<string>)null!));
        }

        [TestMethod]
        public void AreCollectionsNotEqual_Success_MismatchingItem()
        {
            AssertUnderTest.AreCollectionsNotEqual(new string[] { "Test", "Hello", "blub" }, new[] { "Test", "blub", "Hello" });
        }

        [TestMethod]
        public void AreCollectionsNotEqual_Success_MissingItem()
        {
            AssertUnderTest.AreCollectionsNotEqual(new string[] { "Test", "Hello", "blub" }, new[] { "Test", "Hello" });
        }

        [TestMethod]
        public void AreCollectionsNotEqual_Success_UnexpectedItem()
        {
            AssertUnderTest.AreCollectionsNotEqual(new string[] { "Test", "Hello" }, new[] { "Test", "Hello", "blub" });
        }

        [TestMethod]
        public void AreCollectionsNotEqual_Fail()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsNotEqual(new[] { "Test", "Hello", "blub" }, new[] { "Test", "Hello", "blub" }));
            MSAssert.AreEqual($"Assert.AreCollectionsNotEqual failed.", ex.Message);
        }

        [TestMethod]
        public void AreCollectionsNotEqual_Fail_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsNotEqual(new[] { "Test", "Hello", "blub" }, new[] { "Test", "Hello", "blub" }, "This is my test"));
            MSAssert.AreEqual($"Assert.AreCollectionsNotEqual failed. This is my test", ex.Message);
        }

        [TestMethod]
        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP001:Dispose created.", Justification = "Comparer calls should not be validated here.")]
        public void AreCollectionsNotEqual_WithComparer_Success_MismatchingItem()
        {
            var comparer = CreateEqualityComparerT(("Test", "Test"), ("blub", "Hello"), ("Hello", "blub"));
            AssertUnderTest.AreCollectionsNotEqual(new string[] { "Test", "Hello", "blub" }, new[] { "Test", "blub", "Hello" }, comparer);
        }

        [TestMethod]
        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP001:Dispose created.", Justification = "Comparer calls should not be validated here.")]
        public void AreCollectionsNotEqual_WithComparer_Success_MissingItem()
        {
            var comparer = CreateEqualityComparerT(("Test", "Test"), ("Hello", "Hello"));
            AssertUnderTest.AreCollectionsNotEqual(new string[] { "Test", "Hello", "blub" }, new[] { "Test", "Hello" }, comparer);
        }

        [TestMethod]
        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP001:Dispose created.", Justification = "Comparer calls should not be validated here.")]
        public void AreCollectionsNotEqual_WithComparer_Success_UnexpectedItem()
        {
            var comparer = CreateEqualityComparerT(("Test", "Test"), ("Hello", "Hello"));
            AssertUnderTest.AreCollectionsNotEqual(new string[] { "Test", "Hello" }, new[] { "Test", "Hello", "blub" }, comparer);
        }

        [TestMethod]
        public void AreCollectionsNotEqual_WithComparer_Fail()
        {
            using var comparer = CreateEqualityComparerT(("Test", "Test"), ("Hello", "Hello"), ("blub", "blub"));
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsNotEqual(new[] { "Test", "Hello", "blub" }, new[] { "Test", "Hello", "blub" }, comparer));
            MSAssert.AreEqual($"Assert.AreCollectionsNotEqual failed.", ex.Message);
        }

        [TestMethod]
        public void AreCollectionsNotEqual_WithComparer_Fail_WithMessage()
        {
            using var comparer = CreateEqualityComparerT(("Test", "Test"), ("Hello", "Hello"), ("blub", "blub"));
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreCollectionsNotEqual(new[] { "Test", "Hello", "blub" }, new[] { "Test", "Hello", "blub" }, comparer, "This is my test"));
            MSAssert.AreEqual($"Assert.AreCollectionsNotEqual failed. This is my test", ex.Message);
        }

        #endregion

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

        private static DisposableEqualityComparer<T> CreateEqualityComparerTForHash<T>(params T[] expectedHashCalls)
            => CreateEqualityComparerTForHash(true, expectedHashCalls);
        private static DisposableEqualityComparer<T> CreateEqualityComparerTForHash<T>(bool matchExactCount, params T[] expectedHashCalls)
        {
            var comparerMock = new Mock<IEqualityComparer<T>>(MockBehavior.Strict);
            comparerMock.Setup(m => m.GetHashCode(It.IsAny<T>()!)).Returns<T>(x => x!.GetHashCode());
            comparerMock.Setup(m => m.Equals(It.IsAny<T?>(), It.IsAny<T?>())).Returns<T?, T?>((x, y) => Equals(x, y));

            return new DisposableEqualityComparer<T>(
                comparerMock.Object,
                new ActionOnDispose(() =>
                {
                    comparerMock.Verify(m => m.GetHashCode(It.IsAny<T>()!), matchExactCount ? Times.Exactly(expectedHashCalls.Length) : Times.AtLeast(expectedHashCalls.Length));
                    foreach (var x in expectedHashCalls.GroupBy(x => x))
                        comparerMock.Verify(m => m.GetHashCode(x.Key!), matchExactCount ? Times.Exactly(x.Count()) : Times.AtLeast(x.Count()));
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

        private interface ITestInterface
        {
        }

        private class TestBaseClass : ITestInterface
        {
        }

        private class TestClass : TestBaseClass
        {
        }
    }
}
