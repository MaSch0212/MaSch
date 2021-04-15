using MaSch.Core;
using MaSch.Test.Components.Test.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MaSch.Test.Components.Test.Assertion
{
    [TestClass]
    public class CollectionAssertionsTests
    {
        private static MaSch.Test.Assertion.Assert AssertUnderTest => MaSch.Test.Assertion.Assert.Instance;

        #region Contains

        [TestMethod]
        public void Contains_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", (IEnumerable<string>?)null));
            Assert.AreEqual("Assert.Contains failed. Expected:<Test>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void Contains_Empty()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", Array.Empty<string>()));
            Assert.AreEqual("Assert.Contains failed. Expected:<Test>. Actual:<[]>.", ex.Message);
        }

        [TestMethod]
        public void Contains_NotContained()
        {
            var nl = Environment.NewLine;
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", new[] { "abc", "def", "ghi" }));
            Assert.AreEqual($"Assert.Contains failed. Expected:<Test>. Actual:<[{nl}\tabc,{nl}\tdef,{nl}\tghi{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void Contains_Contained()
        {
            AssertUnderTest.Contains("Test", new[] { "abc", "def", "Test", "ghi" });
        }

        [TestMethod]
        public void Contains_WithMessage_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", (IEnumerable<string>?)null, "This is my test"));
            Assert.AreEqual("Assert.Contains failed. Expected:<Test>. Actual:<(null)>. This is my test", ex.Message);
        }

        [TestMethod]
        public void Contains_WithMessage_Empty()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", Array.Empty<string>(), "This is my test"));
            Assert.AreEqual("Assert.Contains failed. Expected:<Test>. Actual:<[]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void Contains_WithMessage_NotContained()
        {
            var nl = Environment.NewLine;
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", new[] { "abc", "def", "ghi" }, "This is my test"));
            Assert.AreEqual($"Assert.Contains failed. Expected:<Test>. Actual:<[{nl}\tabc,{nl}\tdef,{nl}\tghi{nl}]>. This is my test", ex.Message);
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
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", null, comparer));
            Assert.AreEqual("Assert.Contains failed. Expected:<Test>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void Contains_WithComparerT_Empty()
        {
            using var comparer = CreateEqualityComparerT<string>();
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", Array.Empty<string>(), comparer));
            Assert.AreEqual("Assert.Contains failed. Expected:<Test>. Actual:<[]>.", ex.Message);
        }

        [TestMethod]
        public void Contains_WithComparerT_NotContained()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparerT(("abc", "Test"), ("def", "Test"), ("ghi", "Test"));
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", new[] { "abc", "def", "ghi" }, comparer));
            Assert.AreEqual($"Assert.Contains failed. Expected:<Test>. Actual:<[{nl}\tabc,{nl}\tdef,{nl}\tghi{nl}]>.", ex.Message);
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
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", null, comparer, "This is my test"));
            Assert.AreEqual("Assert.Contains failed. Expected:<Test>. Actual:<(null)>. This is my test", ex.Message);
        }

        [TestMethod]
        public void Contains_WithComparerT_WithMessage_Empty()
        {
            using var comparer = CreateEqualityComparerT<string>();
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", Array.Empty<string>(), comparer, "This is my test"));
            Assert.AreEqual("Assert.Contains failed. Expected:<Test>. Actual:<[]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void Contains_WithComparerT_WithMessage_NotContained()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparerT(("abc", "Test"), ("def", "Test"), ("ghi", "Test"));
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", new[] { "abc", "def", "ghi" }, comparer, "This is my test"));
            Assert.AreEqual($"Assert.Contains failed. Expected:<Test>. Actual:<[{nl}\tabc,{nl}\tdef,{nl}\tghi{nl}]>. This is my test", ex.Message);
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
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", null, comparer));
            Assert.AreEqual("Assert.Contains failed. Expected:<Test>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void Contains_WithComparer_Empty()
        {
            using var comparer = CreateEqualityComparer();
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", Array.Empty<string>(), comparer));
            Assert.AreEqual("Assert.Contains failed. Expected:<Test>. Actual:<[]>.", ex.Message);
        }

        [TestMethod]
        public void Contains_WithComparer_NotContained()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparer(("abc", "Test"), ("def", "Test"), ("ghi", "Test"));
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", new[] { "abc", "def", "ghi" }, comparer));
            Assert.AreEqual($"Assert.Contains failed. Expected:<Test>. Actual:<[{nl}\tabc,{nl}\tdef,{nl}\tghi{nl}]>.", ex.Message);
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
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", null, comparer, "This is my test"));
            Assert.AreEqual("Assert.Contains failed. Expected:<Test>. Actual:<(null)>. This is my test", ex.Message);
        }

        [TestMethod]
        public void Contains_WithComparer_WithMessage_Empty()
        {
            using var comparer = CreateEqualityComparer();
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", Array.Empty<string>(), comparer, "This is my test"));
            Assert.AreEqual("Assert.Contains failed. Expected:<Test>. Actual:<[]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void Contains_WithComparer_WithMessage_NotContained()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparer(("abc", "Test"), ("def", "Test"), ("ghi", "Test"));
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", new[] { "abc", "def", "ghi" }, comparer, "This is my test"));
            Assert.AreEqual($"Assert.Contains failed. Expected:<Test>. Actual:<[{nl}\tabc,{nl}\tdef,{nl}\tghi{nl}]>. This is my test", ex.Message);
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
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", null, predicate.Object));
            Assert.AreEqual("Assert.Contains failed. Expected:<Test>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void Contains_WithPredicate_Empty()
        {
            using var predicate = CreatePredicate<string, int>();
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", Array.Empty<int>(), predicate.Object));
            Assert.AreEqual("Assert.Contains failed. Expected:<Test>. Actual:<[]>.", ex.Message);
        }

        [TestMethod]
        public void Contains_WithPredicate_NotContained()
        {
            var nl = Environment.NewLine;
            using var predicate = CreatePredicate(("Test", 1, false), ("Test", 2, false), ("Test", 3, false));
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", new[] { 1, 2, 3 }, predicate.Object));
            Assert.AreEqual($"Assert.Contains failed. Expected:<Test>. Actual:<[{nl}\t1,{nl}\t2,{nl}\t3{nl}]>.", ex.Message);
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
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", null, predicate.Object, "This is my test"));
            Assert.AreEqual("Assert.Contains failed. Expected:<Test>. Actual:<(null)>. This is my test", ex.Message);
        }

        [TestMethod]
        public void Contains_WithPredicate_WithMessage_Empty()
        {
            using var predicate = CreatePredicate<string, int>();
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", Array.Empty<int>(), predicate.Object, "This is my test"));
            Assert.AreEqual("Assert.Contains failed. Expected:<Test>. Actual:<[]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void Contains_WithPredicate_WithMessage_NotContained()
        {
            var nl = Environment.NewLine;
            using var predicate = CreatePredicate(("Test", 1, false), ("Test", 2, false), ("Test", 3, false));
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Contains("Test", new[] { 1, 2, 3 }, predicate.Object, "This is my test"));
            Assert.AreEqual($"Assert.Contains failed. Expected:<Test>. Actual:<[{nl}\t1,{nl}\t2,{nl}\t3{nl}]>. This is my test", ex.Message);
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
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("Test", (IEnumerable<string>?)null));
            Assert.AreEqual("Assert.DoesNotContain failed. NotExpected:<Test>. Actual:<(null)>.", ex.Message);
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
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("Test", new[] { "abc", "def", "Test", "ghi" }));
            Assert.AreEqual($"Assert.DoesNotContain failed. NotExpected:<Test>. Actual:<[{nl}\tabc,{nl}\tdef,{nl}\tTest,{nl}\tghi{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void DoesNotContain_WithMessage_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("Test", (IEnumerable<string>?)null, "This is my test"));
            Assert.AreEqual("Assert.DoesNotContain failed. NotExpected:<Test>. Actual:<(null)>. This is my test", ex.Message);
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
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("Test", new[] { "abc", "def", "Test", "ghi" }, "This is my test"));
            Assert.AreEqual($"Assert.DoesNotContain failed. NotExpected:<Test>. Actual:<[{nl}\tabc,{nl}\tdef,{nl}\tTest,{nl}\tghi{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void DoesNotContain_WithComparerT_Null()
        {
            using var comparer = CreateEqualityComparerT<string>();
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("Test", null, comparer));
            Assert.AreEqual("Assert.DoesNotContain failed. NotExpected:<Test>. Actual:<(null)>.", ex.Message);
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
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("Test", new[] { "abc", "def", "Test", "ghi" }, comparer));
            Assert.AreEqual($"Assert.DoesNotContain failed. NotExpected:<Test>. Actual:<[{nl}\tabc,{nl}\tdef,{nl}\tTest,{nl}\tghi{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void DoesNotContain_WithComparerT_WithMessage_Null()
        {
            using var comparer = CreateEqualityComparerT<string>();
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("Test", null, comparer, "This is my test"));
            Assert.AreEqual("Assert.DoesNotContain failed. NotExpected:<Test>. Actual:<(null)>. This is my test", ex.Message);
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
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("Test", new[] { "abc", "def", "Test", "ghi" }, comparer, "This is my test"));
            Assert.AreEqual($"Assert.DoesNotContain failed. NotExpected:<Test>. Actual:<[{nl}\tabc,{nl}\tdef,{nl}\tTest,{nl}\tghi{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void DoesNotContain_WithComparer_Null()
        {
            using var comparer = CreateEqualityComparer();
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("Test", null, comparer));
            Assert.AreEqual("Assert.DoesNotContain failed. NotExpected:<Test>. Actual:<(null)>.", ex.Message);
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
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("Test", new[] { "abc", "def", "Test", "ghi" }, comparer));
            Assert.AreEqual($"Assert.DoesNotContain failed. NotExpected:<Test>. Actual:<[{nl}\tabc,{nl}\tdef,{nl}\tTest,{nl}\tghi{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void DoesNotContain_WithComparer_WithMessage_Null()
        {
            using var comparer = CreateEqualityComparer();
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("Test", null, comparer, "This is my test"));
            Assert.AreEqual("Assert.DoesNotContain failed. NotExpected:<Test>. Actual:<(null)>. This is my test", ex.Message);
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
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("Test", new[] { "abc", "def", "Test", "ghi" }, comparer, "This is my test"));
            Assert.AreEqual($"Assert.DoesNotContain failed. NotExpected:<Test>. Actual:<[{nl}\tabc,{nl}\tdef,{nl}\tTest,{nl}\tghi{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void DoesNotContain_WithPredicate_Null()
        {
            using var predicate = CreatePredicate<string, int>();
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("Test", null, predicate.Object));
            Assert.AreEqual("Assert.DoesNotContain failed. NotExpected:<Test>. Actual:<(null)>.", ex.Message);
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
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("Test", new[] { 1, 2, 3, 4 }, predicate.Object));
            Assert.AreEqual($"Assert.DoesNotContain failed. NotExpected:<Test>. Actual:<[{nl}\t1,{nl}\t2,{nl}\t3,{nl}\t4{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void DoesNotContain_WithPredicate_WithMessage_Null()
        {
            using var predicate = CreatePredicate<string, int>();
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("Test", null, predicate.Object, "This is my test"));
            Assert.AreEqual("Assert.DoesNotContain failed. NotExpected:<Test>. Actual:<(null)>. This is my test", ex.Message);
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
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.DoesNotContain("Test", new[] { 1, 2, 3, 4 }, predicate.Object, "This is my test"));
            Assert.AreEqual($"Assert.DoesNotContain failed. NotExpected:<Test>. Actual:<[{nl}\t1,{nl}\t2,{nl}\t3,{nl}\t4{nl}]>. This is my test", ex.Message);
        }

        #endregion

        #region AllItemsAreNotNull

        [TestMethod]
        public void AllItemsAreNotNull_NullCollection()
        {
            Assert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.AllItemsAreNotNull(null!));
        }

        [TestMethod]
        public void AllItemsAreNotNull_Success()
        {
            AssertUnderTest.AllItemsAreNotNull(new[] { "Test", "Blub" });
        }

        [TestMethod]
        public void AllItemsAreNotNull_Fail_OneNull()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreNotNull(new[] { "Test", null }));
            Assert.AreEqual("Assert.AllItemsAreNotNull failed. Index:<1>.", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreNotNull_Fail_MultipleNull()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreNotNull(new[] { "Test", null, "Blub", null }));
            Assert.AreEqual("Assert.AllItemsAreNotNull failed. Indices:<1, 3>.", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreNotNull_Fail_OneNull_WithMessage()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreNotNull(new[] { "Test", null }, "This is my test"));
            Assert.AreEqual("Assert.AllItemsAreNotNull failed. Index:<1>. This is my test", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreNotNull_Fail_MultipleNull_WithMessage()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreNotNull(new[] { "Test", null, "Blub", null }, "This is my test"));
            Assert.AreEqual("Assert.AllItemsAreNotNull failed. Indices:<1, 3>. This is my test", ex.Message);
        }

        #endregion

        #region AllItemsAreUnique

        [TestMethod]
        public void AllItemsAreUnique_NullCollection()
        {
            Assert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.AllItemsAreUnique<string>(null!));
        }

        [TestMethod]
        public void AllItemsAreUnique_NullComparer()
        {
            Assert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.AllItemsAreUnique(Array.Empty<string>(), (IEqualityComparer<string>)null!));
        }

        [TestMethod]
        public void AllItemsAreUnique_Success()
        {
            AssertUnderTest.AllItemsAreUnique(new[] { "Test", "Blub", "Hello" });
        }

        [TestMethod]
        public void AllItemsAreUnique_Fail_OneDuplicate()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreUnique(new[] { "Test", "Blub", "Test", "Hello" }));
            Assert.AreEqual("Assert.AllItemsAreUnique failed. Index:<0=2>.", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreUnique_Fail_OneDuplicate_Nulls()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreUnique(new[] { null, "Blub", null, "Hello" }));
            Assert.AreEqual("Assert.AllItemsAreUnique failed. Index:<0=2>.", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreUnique_Fail_MultipleDuplicate()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreUnique(new[] { "Test", "Blub", "Test", "Blub" }));
            Assert.AreEqual("Assert.AllItemsAreUnique failed. Indices:<0=2, 1=3>.", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreUnique_Fail_MultipleSameDuplicate()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreUnique(new[] { "Test", "Blub", "Test", "Test" }));
            Assert.AreEqual("Assert.AllItemsAreUnique failed. Indices:<0=2, 0=3>.", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreUnique_Fail_OneDuplicate_WithMessage()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreUnique(new[] { "Test", "Blub", "Test", "Hello" }, "This is my test"));
            Assert.AreEqual("Assert.AllItemsAreUnique failed. Index:<0=2>. This is my test", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreUnique_Fail_OneDuplicate_Nulls_WithMessage()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreUnique(new[] { null, "Blub", null, "Hello" }, "This is my test"));
            Assert.AreEqual("Assert.AllItemsAreUnique failed. Index:<0=2>. This is my test", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreUnique_Fail_MultipleDuplicate_WithMessage()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreUnique(new[] { "Test", "Blub", "Test", "Blub" }, "This is my test"));
            Assert.AreEqual("Assert.AllItemsAreUnique failed. Indices:<0=2, 1=3>. This is my test", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreUnique_Fail_MultipleSameDuplicate_WithMessage()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreUnique(new[] { "Test", "Blub", "Test", "Test" }, "This is my test"));
            Assert.AreEqual("Assert.AllItemsAreUnique failed. Indices:<0=2, 0=3>. This is my test", ex.Message);
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
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreUnique(new[] { "Test", "Blub", "Test", "Hello" }, comparer));
            Assert.AreEqual("Assert.AllItemsAreUnique failed. Index:<0=2>.", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreUnique_WithComparer_Fail_OneDuplicate_Nulls()
        {
            using var comparer = CreateEqualityComparerTForHash<string?>("Blub", "Hello");
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreUnique(new[] { null, "Blub", null, "Hello" }, comparer));
            Assert.AreEqual("Assert.AllItemsAreUnique failed. Index:<0=2>.", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreUnique_WithComparer_Fail_MultipleDuplicate()
        {
            using var comparer = CreateEqualityComparerTForHash("Test", "Blub", "Test", "Blub");
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreUnique(new[] { "Test", "Blub", "Test", "Blub" }, comparer));
            Assert.AreEqual("Assert.AllItemsAreUnique failed. Indices:<0=2, 1=3>.", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreUnique_WithComparer_Fail_MultipleSameDuplicate()
        {
            using var comparer = CreateEqualityComparerTForHash("Test", "Blub", "Test", "Test");
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreUnique(new[] { "Test", "Blub", "Test", "Test" }, comparer));
            Assert.AreEqual("Assert.AllItemsAreUnique failed. Indices:<0=2, 0=3>.", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreUnique_WithComparer_Fail_OneDuplicate_WithMessage()
        {
            using var comparer = CreateEqualityComparerTForHash("Test", "Blub", "Test", "Hello");
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreUnique(new[] { "Test", "Blub", "Test", "Hello" }, comparer, "This is my test"));
            Assert.AreEqual("Assert.AllItemsAreUnique failed. Index:<0=2>. This is my test", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreUnique_WithComparer_Fail_OneDuplicate_Nulls_WithMessage()
        {
            using var comparer = CreateEqualityComparerTForHash<string?>("Blub", "Hello");
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreUnique(new[] { null, "Blub", null, "Hello" }, comparer, "This is my test"));
            Assert.AreEqual("Assert.AllItemsAreUnique failed. Index:<0=2>. This is my test", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreUnique_WithComparer_Fail_MultipleDuplicate_WithMessage()
        {
            using var comparer = CreateEqualityComparerTForHash("Test", "Blub", "Test", "Blub");
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreUnique(new[] { "Test", "Blub", "Test", "Blub" }, comparer, "This is my test"));
            Assert.AreEqual("Assert.AllItemsAreUnique failed. Indices:<0=2, 1=3>. This is my test", ex.Message);
        }

        [TestMethod]
        public void AllItemsAreUnique_WithComparer_Fail_MultipleSameDuplicate_WithMessage()
        {
            using var comparer = CreateEqualityComparerTForHash("Test", "Blub", "Test", "Test");
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AllItemsAreUnique(new[] { "Test", "Blub", "Test", "Test" }, comparer, "This is my test"));
            Assert.AreEqual("Assert.AllItemsAreUnique failed. Indices:<0=2, 0=3>. This is my test", ex.Message);
        }

        #endregion

        #region IsSubsetOf

        [TestMethod]
        public void IsSubsetOf_NullSubset()
        {
            Assert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.IsSubsetOf(null!, Array.Empty<string>()));
        }

        [TestMethod]
        public void IsSubsetOf_NullSuperset()
        {
            Assert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.IsSubsetOf(Array.Empty<string>(), null!));
        }

        [TestMethod]
        public void IsSubsetOf_NullComparer()
        {
            Assert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.IsSubsetOf(Array.Empty<string>(), Array.Empty<string>(), (IEqualityComparer<string>)null!));
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
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsSubsetOf(new[] { "Test", "blub" }, new[] { "Hello", "blub", "bbb" }));
            Assert.AreEqual($"Assert.IsSubsetOf failed. MissingItems:<[{nl}\tTest{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void IsSubsetOf_Fail_MultipleMissingItem()
        {
            var nl = Environment.NewLine;
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsSubsetOf(new[] { "Test", "blub" }, new[] { "Hello", "bbb" }));
            Assert.AreEqual($"Assert.IsSubsetOf failed. MissingItems:<[{nl}\tTest,{nl}\tblub{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void IsSubsetOf_Fail_WithMessage_OneMissingItem()
        {
            var nl = Environment.NewLine;
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsSubsetOf(new[] { "Test", "blub" }, new[] { "Hello", "blub", "bbb" }, "This is my test"));
            Assert.AreEqual($"Assert.IsSubsetOf failed. MissingItems:<[{nl}\tTest{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsSubsetOf_Fail_WithMessage_MultipleMissingItem()
        {
            var nl = Environment.NewLine;
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsSubsetOf(new[] { "Test", "blub" }, new[] { "Hello", "bbb" }, "This is my test"));
            Assert.AreEqual($"Assert.IsSubsetOf failed. MissingItems:<[{nl}\tTest,{nl}\tblub{nl}]>. This is my test", ex.Message);
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
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsSubsetOf(new[] { "Test", "blub" }, new[] { "Hello", "blub", "bbb" }, comparer));
            Assert.AreEqual($"Assert.IsSubsetOf failed. MissingItems:<[{nl}\tTest{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void IsSubsetOf_WithComparer_Fail_MultipleMissingItem()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparerTForHash("Hello", "bbb", "Test", "blub");
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsSubsetOf(new[] { "Test", "blub" }, new[] { "Hello", "bbb" }, comparer));
            Assert.AreEqual($"Assert.IsSubsetOf failed. MissingItems:<[{nl}\tTest,{nl}\tblub{nl}]>.", ex.Message);
        }

        [TestMethod]
        public void IsSubsetOf_WithComparer_Fail_WithMessage_OneMissingItem()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparerTForHash("Hello", "blub", "bbb", "Test", "blub");
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsSubsetOf(new[] { "Test", "blub" }, new[] { "Hello", "blub", "bbb" }, comparer, "This is my test"));
            Assert.AreEqual($"Assert.IsSubsetOf failed. MissingItems:<[{nl}\tTest{nl}]>. This is my test", ex.Message);
        }

        [TestMethod]
        public void IsSubsetOf_WithComparer_Fail_WithMessage_MultipleMissingItem()
        {
            var nl = Environment.NewLine;
            using var comparer = CreateEqualityComparerTForHash("Hello", "bbb", "Test", "blub");
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsSubsetOf(new[] { "Test", "blub" }, new[] { "Hello", "bbb" }, comparer, "This is my test"));
            Assert.AreEqual($"Assert.IsSubsetOf failed. MissingItems:<[{nl}\tTest,{nl}\tblub{nl}]>. This is my test", ex.Message);
        }

        #endregion

        #region IsNotSubsetOf

        [TestMethod]
        public void IsNotSubsetOf_NullSubset()
        {
            Assert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.IsNotSubsetOf(null!, Array.Empty<string>()));
        }

        [TestMethod]
        public void IsNotSubsetOf_NullSuperset()
        {
            Assert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.IsNotSubsetOf(Array.Empty<string>(), null!));
        }

        [TestMethod]
        public void IsNotSubsetOf_NullComparer()
        {
            Assert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.IsNotSubsetOf(Array.Empty<string>(), Array.Empty<string>(), (IEqualityComparer<string>)null!));
        }

        [TestMethod]
        public void IsNotSubsetOf_Success()
        {
            AssertUnderTest.IsNotSubsetOf(new[] { "Test", "blub" }, new[] { "Hello", "blub", "bbb" });
        }

        [TestMethod]
        public void IsNotSubsetOf_Fail()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotSubsetOf(new[] { "Test", "blub" }, new[] { "Hello", "blub", "bbb", "Test" }));
            Assert.AreEqual($"Assert.IsNotSubsetOf failed.", ex.Message);
        }

        [TestMethod]
        public void IsNotSubsetOf_Fail_WithMessage()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotSubsetOf(new[] { "Test", "blub" }, new[] { "Hello", "blub", "bbb", "Test" }, "This is my test"));
            Assert.AreEqual($"Assert.IsNotSubsetOf failed. This is my test", ex.Message);
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
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotSubsetOf(new[] { "Test", "blub" }, new[] { "Hello", "blub", "bbb", "Test" }, comparer));
            Assert.AreEqual($"Assert.IsNotSubsetOf failed.", ex.Message);
        }

        [TestMethod]
        public void IsNotSubsetOf_WithComparer_Fail_WithMessage()
        {
            using var comparer = CreateEqualityComparerTForHash("Hello", "blub", "bbb", "Test", "Test", "blub");
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotSubsetOf(new[] { "Test", "blub" }, new[] { "Hello", "blub", "bbb", "Test" }, comparer, "This is my test"));
            Assert.AreEqual($"Assert.IsNotSubsetOf failed. This is my test", ex.Message);
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
        {
            var comparerMock = new Mock<IEqualityComparer<T>>(MockBehavior.Strict);
            comparerMock.Setup(m => m.GetHashCode(It.IsAny<T>()!)).Returns<T>(x => x!.GetHashCode());
            comparerMock.Setup(m => m.Equals(It.IsAny<T?>(), It.IsAny<T?>())).Returns<T?, T?>((x, y) => Equals(x, y));

            return new DisposableEqualityComparer<T>(
                comparerMock.Object,
                new ActionOnDispose(() =>
                {
                    comparerMock.Verify(m => m.GetHashCode(It.IsAny<T>()!), Times.Exactly(expectedHashCalls.Length));
                    foreach (var x in expectedHashCalls.GroupBy(x => x))
                        comparerMock.Verify(m => m.GetHashCode(x.Key!), Times.Exactly(x.Count()));
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
