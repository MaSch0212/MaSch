using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Globalization;

namespace MaSch.Test.Components.Test.Assertion
{
    [TestClass]
    public class CommonAssertionsTests
    {
        private static MaSch.Test.Assertion.Assert AssertUnderTest => MaSch.Test.Assertion.Assert.Instance;

        [TestMethod]
        public void IsTrue_True()
        {
            AssertUnderTest.IsTrue(true);
        }

        [TestMethod]
        public void IsTrue_False()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsTrue(false));
            Assert.AreEqual("Assert.IsTrue failed.", ex.Message);
        }

        [TestMethod]
        public void IsTrue_False_WithMessage()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsTrue(false, "My test message"));
            Assert.AreEqual("Assert.IsTrue failed. My test message", ex.Message);
        }

        [TestMethod]
        public void IsTrue_Nullable_True()
        {
            AssertUnderTest.IsTrue((bool?)true);
        }

        [TestMethod]
        public void IsTrue_Nullable_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsTrue(null));
            Assert.AreEqual("Assert.IsTrue failed.", ex.Message);
        }

        [TestMethod]
        public void IsTrue_Nullable_False()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsTrue((bool?)false));
            Assert.AreEqual("Assert.IsTrue failed.", ex.Message);
        }

        [TestMethod]
        public void IsTrue_Nullable_False_WithMessage()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsTrue((bool?)false, "My test message"));
            Assert.AreEqual("Assert.IsTrue failed. My test message", ex.Message);
        }

        [TestMethod]
        public void IsFalse_False()
        {
            AssertUnderTest.IsFalse(false);
        }

        [TestMethod]
        public void IsFalse_True()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsFalse(true));
            Assert.AreEqual("Assert.IsFalse failed.", ex.Message);
        }

        [TestMethod]
        public void IsFalse_True_WithMessage()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsFalse(true, "My test message"));
            Assert.AreEqual("Assert.IsFalse failed. My test message", ex.Message);
        }

        [TestMethod]
        public void IsFalse_Nullable_False()
        {
            AssertUnderTest.IsFalse((bool?)false);
        }

        [TestMethod]
        public void IsFalse_Nullable_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsFalse(null));
            Assert.AreEqual("Assert.IsFalse failed.", ex.Message);
        }

        [TestMethod]
        public void IsFalse_Nullable_True()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsFalse((bool?)true));
            Assert.AreEqual("Assert.IsFalse failed.", ex.Message);
        }

        [TestMethod]
        public void IsFalse_Nullable_True_WithMessage()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsFalse((bool?)true, "My test message"));
            Assert.AreEqual("Assert.IsFalse failed. My test message", ex.Message);
        }

        [TestMethod]
        public void IsNull_Null()
        {
            AssertUnderTest.IsNull(null);
        }

        [TestMethod]
        public void IsNull_NotNull()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNull(new object()));
            Assert.AreEqual("Assert.IsNull failed.", ex.Message);
        }

        [TestMethod]
        public void IsNull_NotNull_WithMessage()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNull(new object(), "My test message"));
            Assert.AreEqual("Assert.IsNull failed. My test message", ex.Message);
        }

        [TestMethod]
        public void IsNotNull_NotNull()
        {
            AssertUnderTest.IsNotNull(new object());
        }

        [TestMethod]
        public void IsNotNull_Null()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotNull(null));
            Assert.AreEqual("Assert.IsNotNull failed.", ex.Message);
        }

        [TestMethod]
        public void IsNotNull_Null_WithMessage()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotNull(null, "My test message"));
            Assert.AreEqual("Assert.IsNotNull failed. My test message", ex.Message);
        }

        [TestMethod]
        public void AreSame_Success()
        {
            var obj = new string("Test".ToCharArray());
            AssertUnderTest.AreSame(obj, obj);
        }

        [TestMethod]
        public void AreSame_Fail()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreSame(new string("Test".ToCharArray()), new string("Test".ToCharArray())));
            Assert.AreEqual("Assert.AreSame failed. Expected:<Test>. Actual:<Test>.", ex.Message);
        }

        [TestMethod]
        public void AreSame_Fail_WithMessage()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreSame(new string("Test".ToCharArray()), new string("Test".ToCharArray()), "My test message"));
            Assert.AreEqual("Assert.AreSame failed. Expected:<Test>. Actual:<Test>. My test message", ex.Message);
        }

        [TestMethod]
        public void AreNotSame_Success()
        {
            AssertUnderTest.AreNotSame(new string("Test".ToCharArray()), new string("Test".ToCharArray()));
        }

        [TestMethod]
        public void AreNotSame_Fail()
        {
            var obj = new string("Test".ToCharArray());
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreNotSame(obj, obj));
            Assert.AreEqual("Assert.AreNotSame failed. NotExpected:<Test>. Actual:<Test>.", ex.Message);
        }

        [TestMethod]
        public void AreNotSame_Fail_WithMessage()
        {
            var obj = new string("Test".ToCharArray());
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreNotSame(obj, obj, "My test message"));
            Assert.AreEqual("Assert.AreNotSame failed. NotExpected:<Test>. Actual:<Test>. My test message", ex.Message);
        }

        [TestMethod]
        public void AreEqual_Success()
        {
            AssertUnderTest.AreEqual(new string("Test".ToCharArray()), new string("Test".ToCharArray()));
        }

        [TestMethod]
        public void AreEqual_Success_Object()
        {
            AssertUnderTest.AreEqual((object)new string("Test".ToCharArray()), new string("Test".ToCharArray()));
        }

        [TestMethod]
        public void AreEqual_Fail_SameType()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual("Test", "test"));
            Assert.AreEqual("Assert.AreEqual failed. Expected:<Test>. Actual:<test>.", ex.Message);
        }

        [TestMethod]
        public void AreEqual_Fail_DifferentType()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual("5", 5));
            Assert.AreEqual("Assert.AreEqual failed. Expected:<5>. ExpectedType:<System.String>. Actual:<5>. ActualType:<System.Int32>.", ex.Message);
        }

        [TestMethod]
        public void AreEqual_Fail_SameType_WithMessage()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual("Test", "test", "My test message"));
            Assert.AreEqual("Assert.AreEqual failed. Expected:<Test>. Actual:<test>. My test message", ex.Message);
        }

        [TestMethod]
        public void AreEqual_Fail_DifferentType_WithMessage()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual("5", 5, "My test message"));
            Assert.AreEqual("Assert.AreEqual failed. Expected:<5>. ExpectedType:<System.String>. Actual:<5>. ActualType:<System.Int32>. My test message", ex.Message);
        }

        [TestMethod]
        public void AreEqual_Fail_DifferentType_ActualNull()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual("5", null));
            Assert.AreEqual("Assert.AreEqual failed. Expected:<5>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void AreEqual_Fail_DifferentType_ActualNull_WithMessage()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual("5", null, "My test message"));
            Assert.AreEqual("Assert.AreEqual failed. Expected:<5>. Actual:<(null)>. My test message", ex.Message);
        }

        [TestMethod]
        public void AreEqual_Fail_DifferentType_ExpectedNull()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual(null, 5));
            Assert.AreEqual("Assert.AreEqual failed. Expected:<(null)>. Actual:<5>.", ex.Message);
        }

        [TestMethod]
        public void AreEqual_Fail_DifferentType_ExpectedNull_WithMessage()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual(null, 5, "My test message"));
            Assert.AreEqual("Assert.AreEqual failed. Expected:<(null)>. Actual:<5>. My test message", ex.Message);
        }

        [TestMethod]
        public void AreNotEqual_Success_SameType()
        {
            AssertUnderTest.AreNotEqual("Test", "test");
        }

        [TestMethod]
        public void AreNotEqual_Success_DifferentType()
        {
            AssertUnderTest.AreNotEqual("5", 5);
        }

        [TestMethod]
        public void AreNotEqual_Fail()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreNotEqual(new string("Test".ToCharArray()), new string("Test".ToCharArray())));
            Assert.AreEqual("Assert.AreNotEqual failed. NotExpected:<Test>. Actual:<Test>.", ex.Message);
        }

        [TestMethod]
        public void AreNotEqual_Fail_WithMessage()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreNotEqual(new string("Test".ToCharArray()), new string("Test".ToCharArray()), "My test message"));
            Assert.AreEqual("Assert.AreNotEqual failed. NotExpected:<Test>. Actual:<Test>. My test message", ex.Message);
        }

        [TestMethod]
        public void AreNotEqual_Fail_Object()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreNotEqual((object)new string("Test".ToCharArray()), new string("Test".ToCharArray())));
            Assert.AreEqual("Assert.AreNotEqual failed. NotExpected:<Test>. Actual:<Test>.", ex.Message);
        }

        [TestMethod]
        public void AreNotEqual_Fail_Object_WithMessage()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreNotEqual((object)new string("Test".ToCharArray()), new string("Test".ToCharArray()), "My test message"));
            Assert.AreEqual("Assert.AreNotEqual failed. NotExpected:<Test>. Actual:<Test>. My test message", ex.Message);
        }

        [TestMethod]
        public void AreEqual_Float_Success()
        {
            AssertUnderTest.AreEqual(5f, 5.05f, 0.1f);
        }

        [TestMethod]
        public void AreEqual_Float_Fail()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual(5f, 5.15f, 0.1f));
            Assert.AreEqual("Assert.AreEqual failed. Expected:<5>. Actual:<5.15>. Delta:<0.1>.", ex.Message);
        }

        [TestMethod]
        public void AreEqual_Float_Fail_ExpectedNaN()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual(float.NaN, 5.05f, 0.1f));
            Assert.AreEqual("Assert.AreEqual failed. Expected:<NaN>. Actual:<5.05>. Delta:<0.1>.", ex.Message);
        }

        [TestMethod]
        public void AreEqual_Float_Fail_ActualNaN()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual(5f, float.NaN, 0.1f));
            Assert.AreEqual("Assert.AreEqual failed. Expected:<5>. Actual:<NaN>. Delta:<0.1>.", ex.Message);
        }

        [TestMethod]
        public void AreEqual_Float_Fail_DeltaNaN()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual(5f, 5.05f, float.NaN));
            Assert.AreEqual("Assert.AreEqual failed. Expected:<5>. Actual:<5.05>. Delta:<NaN>.", ex.Message);
        }

        [TestMethod]
        public void AreEqual_Float_Fail_WithMessage()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual(5f, 5.15f, 0.1f, "My test message"));
            Assert.AreEqual("Assert.AreEqual failed. Expected:<5>. Actual:<5.15>. Delta:<0.1>. My test message", ex.Message);
        }

        [TestMethod]
        public void AreNotEqual_Float_Success()
        {
            AssertUnderTest.AreNotEqual(5f, 5.15f, 0.1f);
        }

        [TestMethod]
        public void AreNotEqual_Float_Fail()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreNotEqual(5f, 5.05f, 0.1f));
            Assert.AreEqual("Assert.AreNotEqual failed. NotExpected:<5>. Actual:<5.05>. Delta:<0.1>.", ex.Message);
        }

        [TestMethod]
        public void AreNotEqual_Float_Fail_WithMessage()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreNotEqual(5f, 5.05f, 0.1f, "My test message"));
            Assert.AreEqual("Assert.AreNotEqual failed. NotExpected:<5>. Actual:<5.05>. Delta:<0.1>. My test message", ex.Message);
        }

        [TestMethod]
        public void AreEqual_Double_Success()
        {
            AssertUnderTest.AreEqual(5d, 5.05d, 0.1d);
        }

        [TestMethod]
        public void AreEqual_Double_Fail()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual(5d, 5.15d, 0.1d));
            Assert.AreEqual("Assert.AreEqual failed. Expected:<5>. Actual:<5.15>. Delta:<0.1>.", ex.Message);
        }

        [TestMethod]
        public void AreEqual__Fail_ExpectedNaN()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual(double.NaN, 5.05d, 0.1d));
            Assert.AreEqual("Assert.AreEqual failed. Expected:<NaN>. Actual:<5.05>. Delta:<0.1>.", ex.Message);
        }

        [TestMethod]
        public void AreEqual_Double_Fail_ActualNaN()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual(5d, double.NaN, 0.1d));
            Assert.AreEqual("Assert.AreEqual failed. Expected:<5>. Actual:<NaN>. Delta:<0.1>.", ex.Message);
        }

        [TestMethod]
        public void AreEqual_Double_Fail_DeltaNaN()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual(5d, 5.05d, double.NaN));
            Assert.AreEqual("Assert.AreEqual failed. Expected:<5>. Actual:<5.05>. Delta:<NaN>.", ex.Message);
        }

        [TestMethod]
        public void AreEqual_Double_Fail_WithMessage()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual(5d, 5.15d, 0.1d, "My test message"));
            Assert.AreEqual("Assert.AreEqual failed. Expected:<5>. Actual:<5.15>. Delta:<0.1>. My test message", ex.Message);
        }

        [TestMethod]
        public void AreNotEqual_Double_Success()
        {
            AssertUnderTest.AreNotEqual(5d, 5.15d, 0.1d);
        }

        [TestMethod]
        public void AreNotEqual_Double_Fail()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreNotEqual(5d, 5.05d, 0.1d));
            Assert.AreEqual("Assert.AreNotEqual failed. NotExpected:<5>. Actual:<5.05>. Delta:<0.1>.", ex.Message);
        }

        [TestMethod]
        public void AreNotEqual_Double_Fail_WithMessage()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreNotEqual(5d, 5.05d, 0.1d, "My test message"));
            Assert.AreEqual("Assert.AreNotEqual failed. NotExpected:<5>. Actual:<5.05>. Delta:<0.1>. My test message", ex.Message);
        }

        [TestMethod]
        public void AreEqual_String_Success_MatchCase()
        {
            AssertUnderTest.AreEqual("Test", "Test", false);
        }

        [TestMethod]
        public void AreEqual_String_Success_IgnoreCase()
        {
            AssertUnderTest.AreEqual("TEST", "test", true);
        }

        [TestMethod]
        public void AreEqual_String_Fail_MatchCase()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual("TEST", "test", false));
            Assert.AreEqual("Assert.AreEqual failed. Expected:<TEST>. Actual:<test>.", ex.Message);
        }

        [TestMethod]
        public void AreEqual_String_Fail_IgnoreCase()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual("_test_", "test", true));
            Assert.AreEqual("Assert.AreEqual failed. Expected:<_test_>. Actual:<test>.", ex.Message);
        }

        [TestMethod]
        public void AreEqual_String_Fail_MatchCase_WithMessage()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual("TEST", "test", false, "My test message"));
            Assert.AreEqual("Assert.AreEqual failed. Expected:<TEST>. Actual:<test>. My test message", ex.Message);
        }

        [TestMethod]
        public void AreEqual_String_Fail_IgnoreCase_WithMessage()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual("_test_", "test", true, "My test message"));
            Assert.AreEqual("Assert.AreEqual failed. Expected:<_test_>. Actual:<test>. My test message", ex.Message);
        }

        [TestMethod]
        public void AreNotEqual_String_Success_MatchCase()
        {
            AssertUnderTest.AreNotEqual("TEST", "test", false);
        }

        [TestMethod]
        public void AreNotEqual_String_Success_IgnoreCase()
        {
            AssertUnderTest.AreNotEqual("_test_", "test", true);
        }

        [TestMethod]
        public void AreNotEqual_String_Fail_MatchCase()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreNotEqual("Test", "Test", false));
            Assert.AreEqual("Assert.AreNotEqual failed. NotExpected:<Test>. Actual:<Test>.", ex.Message);
        }

        [TestMethod]
        public void AreNotEqual_String_Fail_IgnoreCase()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreNotEqual("TEST", "test", true));
            Assert.AreEqual("Assert.AreNotEqual failed. NotExpected:<TEST>. Actual:<test>.", ex.Message);
        }

        [TestMethod]
        public void AreNotEqual_String_Fail_MatchCase_WithMessage()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreNotEqual("Test", "Test", false, "My test message"));
            Assert.AreEqual("Assert.AreNotEqual failed. NotExpected:<Test>. Actual:<Test>. My test message", ex.Message);
        }

        [TestMethod]
        public void AreNotEqual_String_Fail_IgnoreCase_WithMessage()
        {
            var ex = Assert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreNotEqual("TEST", "test", true, "My test message"));
            Assert.AreEqual("Assert.AreNotEqual failed. NotExpected:<TEST>. Actual:<test>. My test message", ex.Message);
        }
    }
}
