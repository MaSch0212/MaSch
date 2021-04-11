using MaSch.Core;
using MaSch.Test.Components.Test.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;

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
