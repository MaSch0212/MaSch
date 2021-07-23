using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using MSAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace MaSch.Test.Assertion.UnitTests
{
    [TestClass]
    public class CommonAssertionsTests
    {
        private static MaSch.Test.Assertion.Assert AssertUnderTest => MaSch.Test.Assertion.Assert.Instance;

        #region IsTrue

        [TestMethod]
        public void IsTrue_True()
        {
            AssertUnderTest.IsTrue(true);
        }

        [TestMethod]
        public void IsTrue_False()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsTrue(false));
            MSAssert.AreEqual("Assert.IsTrue failed.", ex.Message);
        }

        [TestMethod]
        public void IsTrue_False_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsTrue(false, "My test message"));
            MSAssert.AreEqual("Assert.IsTrue failed. My test message", ex.Message);
        }

        [TestMethod]
        public void IsTrue_Nullable_True()
        {
            AssertUnderTest.IsTrue((bool?)true);
        }

        [TestMethod]
        public void IsTrue_Nullable_Null()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsTrue(null));
            MSAssert.AreEqual("Assert.IsTrue failed.", ex.Message);
        }

        [TestMethod]
        public void IsTrue_Nullable_False()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsTrue((bool?)false));
            MSAssert.AreEqual("Assert.IsTrue failed.", ex.Message);
        }

        [TestMethod]
        public void IsTrue_Nullable_False_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsTrue((bool?)false, "My test message"));
            MSAssert.AreEqual("Assert.IsTrue failed. My test message", ex.Message);
        }

        #endregion

        #region IsFalse

        [TestMethod]
        public void IsFalse_False()
        {
            AssertUnderTest.IsFalse(false);
        }

        [TestMethod]
        public void IsFalse_True()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsFalse(true));
            MSAssert.AreEqual("Assert.IsFalse failed.", ex.Message);
        }

        [TestMethod]
        public void IsFalse_True_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsFalse(true, "My test message"));
            MSAssert.AreEqual("Assert.IsFalse failed. My test message", ex.Message);
        }

        [TestMethod]
        public void IsFalse_Nullable_False()
        {
            AssertUnderTest.IsFalse((bool?)false);
        }

        [TestMethod]
        public void IsFalse_Nullable_Null()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsFalse(null));
            MSAssert.AreEqual("Assert.IsFalse failed.", ex.Message);
        }

        [TestMethod]
        public void IsFalse_Nullable_True()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsFalse((bool?)true));
            MSAssert.AreEqual("Assert.IsFalse failed.", ex.Message);
        }

        [TestMethod]
        public void IsFalse_Nullable_True_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsFalse((bool?)true, "My test message"));
            MSAssert.AreEqual("Assert.IsFalse failed. My test message", ex.Message);
        }

        #endregion

        #region IsNull

        [TestMethod]
        public void IsNull_Null()
        {
            AssertUnderTest.IsNull(null);
        }

        [TestMethod]
        public void IsNull_NotNull()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNull(new object()));
            MSAssert.AreEqual("Assert.IsNull failed.", ex.Message);
        }

        [TestMethod]
        public void IsNull_NotNull_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNull(new object(), "My test message"));
            MSAssert.AreEqual("Assert.IsNull failed. My test message", ex.Message);
        }

        #endregion

        #region IsNotNull

        [TestMethod]
        public void IsNotNull_NotNull()
        {
            AssertUnderTest.IsNotNull(new object());
        }

        [TestMethod]
        public void IsNotNull_Null()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotNull(null));
            MSAssert.AreEqual("Assert.IsNotNull failed.", ex.Message);
        }

        [TestMethod]
        public void IsNotNull_Null_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotNull(null, "My test message"));
            MSAssert.AreEqual("Assert.IsNotNull failed. My test message", ex.Message);
        }

        #endregion

        #region AreSame

        [TestMethod]
        public void AreSame_Success()
        {
            var obj = new string("Test".ToCharArray());
            AssertUnderTest.AreSame(obj, obj);
        }

        [TestMethod]
        public void AreSame_Fail()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreSame(new string("Test".ToCharArray()), new string("Test".ToCharArray())));
            MSAssert.AreEqual("Assert.AreSame failed. Expected:<Test>. Actual:<Test>.", ex.Message);
        }

        [TestMethod]
        public void AreSame_Fail_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreSame(new string("Test".ToCharArray()), new string("Test".ToCharArray()), "My test message"));
            MSAssert.AreEqual("Assert.AreSame failed. Expected:<Test>. Actual:<Test>. My test message", ex.Message);
        }

        #endregion

        #region AreNotSame

        [TestMethod]
        public void AreNotSame_Success()
        {
            AssertUnderTest.AreNotSame(new string("Test".ToCharArray()), new string("Test".ToCharArray()));
        }

        [TestMethod]
        public void AreNotSame_Fail()
        {
            var obj = new string("Test".ToCharArray());
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreNotSame(obj, obj));
            MSAssert.AreEqual("Assert.AreNotSame failed. NotExpected:<Test>. Actual:<Test>.", ex.Message);
        }

        [TestMethod]
        public void AreNotSame_Fail_WithMessage()
        {
            var obj = new string("Test".ToCharArray());
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreNotSame(obj, obj, "My test message"));
            MSAssert.AreEqual("Assert.AreNotSame failed. NotExpected:<Test>. Actual:<Test>. My test message", ex.Message);
        }

        #endregion

        #region AreEqual

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
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual("Test", "test"));
            MSAssert.AreEqual("Assert.AreEqual failed. Expected:<Test>. Actual:<test>.", ex.Message);
        }

        [TestMethod]
        public void AreEqual_Fail_DifferentType()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual("5", 5));
            MSAssert.AreEqual("Assert.AreEqual failed. Expected:<5>. ExpectedType:<System.String>. Actual:<5>. ActualType:<System.Int32>.", ex.Message);
        }

        [TestMethod]
        public void AreEqual_Fail_SameType_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual("Test", "test", "My test message"));
            MSAssert.AreEqual("Assert.AreEqual failed. Expected:<Test>. Actual:<test>. My test message", ex.Message);
        }

        [TestMethod]
        public void AreEqual_Fail_DifferentType_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual("5", 5, "My test message"));
            MSAssert.AreEqual("Assert.AreEqual failed. Expected:<5>. ExpectedType:<System.String>. Actual:<5>. ActualType:<System.Int32>. My test message", ex.Message);
        }

        [TestMethod]
        public void AreEqual_Fail_DifferentType_ActualNull()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual("5", null));
            MSAssert.AreEqual("Assert.AreEqual failed. Expected:<5>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void AreEqual_Fail_DifferentType_ActualNull_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual("5", null, "My test message"));
            MSAssert.AreEqual("Assert.AreEqual failed. Expected:<5>. Actual:<(null)>. My test message", ex.Message);
        }

        [TestMethod]
        public void AreEqual_Fail_DifferentType_ExpectedNull()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual(null, 5));
            MSAssert.AreEqual("Assert.AreEqual failed. Expected:<(null)>. Actual:<5>.", ex.Message);
        }

        [TestMethod]
        public void AreEqual_Fail_DifferentType_ExpectedNull_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual(null, 5, "My test message"));
            MSAssert.AreEqual("Assert.AreEqual failed. Expected:<(null)>. Actual:<5>. My test message", ex.Message);
        }

        [TestMethod]
        public void AreEqual_Float_Success()
        {
            AssertUnderTest.AreEqual(5f, 5.05f, 0.1f);
        }

        [TestMethod]
        public void AreEqual_Float_Fail()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual(5f, 5.15f, 0.1f));
            MSAssert.AreEqual("Assert.AreEqual failed. Expected:<5>. Actual:<5.15>. Delta:<0.1>.", ex.Message);
        }

        [TestMethod]
        public void AreEqual_Float_Fail_ExpectedNaN()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual(float.NaN, 5.05f, 0.1f));
            MSAssert.AreEqual("Assert.AreEqual failed. Expected:<NaN>. Actual:<5.05>. Delta:<0.1>.", ex.Message);
        }

        [TestMethod]
        public void AreEqual_Float_Fail_ActualNaN()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual(5f, float.NaN, 0.1f));
            MSAssert.AreEqual("Assert.AreEqual failed. Expected:<5>. Actual:<NaN>. Delta:<0.1>.", ex.Message);
        }

        [TestMethod]
        public void AreEqual_Float_Fail_DeltaNaN()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual(5f, 5.05f, float.NaN));
            MSAssert.AreEqual("Assert.AreEqual failed. Expected:<5>. Actual:<5.05>. Delta:<NaN>.", ex.Message);
        }

        [TestMethod]
        public void AreEqual_Float_Fail_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual(5f, 5.15f, 0.1f, "My test message"));
            MSAssert.AreEqual("Assert.AreEqual failed. Expected:<5>. Actual:<5.15>. Delta:<0.1>. My test message", ex.Message);
        }

        [TestMethod]
        public void AreEqual_Double_Success()
        {
            AssertUnderTest.AreEqual(5d, 5.05d, 0.1d);
        }

        [TestMethod]
        public void AreEqual_Double_Fail()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual(5d, 5.15d, 0.1d));
            MSAssert.AreEqual("Assert.AreEqual failed. Expected:<5>. Actual:<5.15>. Delta:<0.1>.", ex.Message);
        }

        [TestMethod]
        public void AreEqual__Fail_ExpectedNaN()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual(double.NaN, 5.05d, 0.1d));
            MSAssert.AreEqual("Assert.AreEqual failed. Expected:<NaN>. Actual:<5.05>. Delta:<0.1>.", ex.Message);
        }

        [TestMethod]
        public void AreEqual_Double_Fail_ActualNaN()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual(5d, double.NaN, 0.1d));
            MSAssert.AreEqual("Assert.AreEqual failed. Expected:<5>. Actual:<NaN>. Delta:<0.1>.", ex.Message);
        }

        [TestMethod]
        public void AreEqual_Double_Fail_DeltaNaN()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual(5d, 5.05d, double.NaN));
            MSAssert.AreEqual("Assert.AreEqual failed. Expected:<5>. Actual:<5.05>. Delta:<NaN>.", ex.Message);
        }

        [TestMethod]
        public void AreEqual_Double_Fail_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual(5d, 5.15d, 0.1d, "My test message"));
            MSAssert.AreEqual("Assert.AreEqual failed. Expected:<5>. Actual:<5.15>. Delta:<0.1>. My test message", ex.Message);
        }

        #endregion

        #region AreNotEqual

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
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreNotEqual(new string("Test".ToCharArray()), new string("Test".ToCharArray())));
            MSAssert.AreEqual("Assert.AreNotEqual failed. NotExpected:<Test>. Actual:<Test>.", ex.Message);
        }

        [TestMethod]
        public void AreNotEqual_Fail_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreNotEqual(new string("Test".ToCharArray()), new string("Test".ToCharArray()), "My test message"));
            MSAssert.AreEqual("Assert.AreNotEqual failed. NotExpected:<Test>. Actual:<Test>. My test message", ex.Message);
        }

        [TestMethod]
        public void AreNotEqual_Fail_Object()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreNotEqual((object)new string("Test".ToCharArray()), new string("Test".ToCharArray())));
            MSAssert.AreEqual("Assert.AreNotEqual failed. NotExpected:<Test>. Actual:<Test>.", ex.Message);
        }

        [TestMethod]
        public void AreNotEqual_Fail_Object_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreNotEqual((object)new string("Test".ToCharArray()), new string("Test".ToCharArray()), "My test message"));
            MSAssert.AreEqual("Assert.AreNotEqual failed. NotExpected:<Test>. Actual:<Test>. My test message", ex.Message);
        }

        [TestMethod]
        public void AreNotEqual_Float_Success()
        {
            AssertUnderTest.AreNotEqual(5f, 5.15f, 0.1f);
        }

        [TestMethod]
        public void AreNotEqual_Float_Fail()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreNotEqual(5f, 5.05f, 0.1f));
            MSAssert.AreEqual("Assert.AreNotEqual failed. NotExpected:<5>. Actual:<5.05>. Delta:<0.1>.", ex.Message);
        }

        [TestMethod]
        public void AreNotEqual_Float_Fail_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreNotEqual(5f, 5.05f, 0.1f, "My test message"));
            MSAssert.AreEqual("Assert.AreNotEqual failed. NotExpected:<5>. Actual:<5.05>. Delta:<0.1>. My test message", ex.Message);
        }

        [TestMethod]
        public void AreNotEqual_Double_Success()
        {
            AssertUnderTest.AreNotEqual(5d, 5.15d, 0.1d);
        }

        [TestMethod]
        public void AreNotEqual_Double_Fail()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreNotEqual(5d, 5.05d, 0.1d));
            MSAssert.AreEqual("Assert.AreNotEqual failed. NotExpected:<5>. Actual:<5.05>. Delta:<0.1>.", ex.Message);
        }

        [TestMethod]
        public void AreNotEqual_Double_Fail_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreNotEqual(5d, 5.05d, 0.1d, "My test message"));
            MSAssert.AreEqual("Assert.AreNotEqual failed. NotExpected:<5>. Actual:<5.05>. Delta:<0.1>. My test message", ex.Message);
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
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual("TEST", "test", false));
            MSAssert.AreEqual("Assert.AreEqual failed. Expected:<TEST>. Actual:<test>.", ex.Message);
        }

        [TestMethod]
        public void AreEqual_String_Fail_IgnoreCase()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual("_test_", "test", true));
            MSAssert.AreEqual("Assert.AreEqual failed. Expected:<_test_>. Actual:<test>.", ex.Message);
        }

        [TestMethod]
        public void AreEqual_String_Fail_MatchCase_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual("TEST", "test", false, "My test message"));
            MSAssert.AreEqual("Assert.AreEqual failed. Expected:<TEST>. Actual:<test>. My test message", ex.Message);
        }

        [TestMethod]
        public void AreEqual_String_Fail_IgnoreCase_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreEqual("_test_", "test", true, "My test message"));
            MSAssert.AreEqual("Assert.AreEqual failed. Expected:<_test_>. Actual:<test>. My test message", ex.Message);
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
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreNotEqual("Test", "Test", false));
            MSAssert.AreEqual("Assert.AreNotEqual failed. NotExpected:<Test>. Actual:<Test>.", ex.Message);
        }

        [TestMethod]
        public void AreNotEqual_String_Fail_IgnoreCase()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreNotEqual("TEST", "test", true));
            MSAssert.AreEqual("Assert.AreNotEqual failed. NotExpected:<TEST>. Actual:<test>.", ex.Message);
        }

        [TestMethod]
        public void AreNotEqual_String_Fail_MatchCase_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreNotEqual("Test", "Test", false, "My test message"));
            MSAssert.AreEqual("Assert.AreNotEqual failed. NotExpected:<Test>. Actual:<Test>. My test message", ex.Message);
        }

        [TestMethod]
        public void AreNotEqual_String_Fail_IgnoreCase_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.AreNotEqual("TEST", "test", true, "My test message"));
            MSAssert.AreEqual("Assert.AreNotEqual failed. NotExpected:<TEST>. Actual:<test>. My test message", ex.Message);
        }

        #endregion

        #region IsInstanceOfType

        [TestMethod]
        public void IsInstanceOfType_Success_SameType()
        {
            AssertUnderTest.IsInstanceOfType("Test", typeof(string));
        }

        [TestMethod]
        public void IsInstanceOfType_Success_DerivedType()
        {
            AssertUnderTest.IsInstanceOfType(AssertUnderTest, typeof(MaSch.Test.Assertion.AssertBase));
        }

        [TestMethod]
        public void IsInstanceOfType_Fail_NullType()
        {
            MSAssert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.IsInstanceOfType("Test", null!));
        }

        [TestMethod]
        public void IsInstanceOfType_Fail_Null()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsInstanceOfType(null, typeof(string)));
            MSAssert.AreEqual("Assert.IsInstanceOfType failed. Expected:<System.String>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void IsInstanceOfType_Fail_WrongType()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsInstanceOfType(5, typeof(string)));
            MSAssert.AreEqual("Assert.IsInstanceOfType failed. Expected:<System.String>. Actual:<System.Int32>.", ex.Message);
        }

        [TestMethod]
        public void IsInstanceOfType_Fail_Null_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsInstanceOfType(null, typeof(string), "My test message"));
            MSAssert.AreEqual("Assert.IsInstanceOfType failed. Expected:<System.String>. Actual:<(null)>. My test message", ex.Message);
        }

        [TestMethod]
        public void IsInstanceOfType_Fail_WrongType_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsInstanceOfType(5, typeof(string), "My test message"));
            MSAssert.AreEqual("Assert.IsInstanceOfType failed. Expected:<System.String>. Actual:<System.Int32>. My test message", ex.Message);
        }

        [TestMethod]
        public void IsInstanceOfTypeT_Success_SameType()
        {
            object? obj = "Test";

            var result = AssertUnderTest.IsInstanceOfType<string>(obj);

            MSAssert.AreSame(obj, result);
        }

        [TestMethod]
        public void IsInstanceOfTypeT_Success_DerivedType()
        {
            object? obj = AssertUnderTest;

            var result = AssertUnderTest.IsInstanceOfType<MaSch.Test.Assertion.AssertBase>(obj);

            MSAssert.AreSame(obj, result);
        }

        [TestMethod]
        public void IsInstanceOfTypeT_Fail_Null()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsInstanceOfType<string>(null));
            MSAssert.AreEqual("Assert.IsInstanceOfType failed. Expected:<System.String>. Actual:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void IsInstanceOfTypeT_Fail_WrongType()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsInstanceOfType<string>(5));
            MSAssert.AreEqual("Assert.IsInstanceOfType failed. Expected:<System.String>. Actual:<System.Int32>.", ex.Message);
        }

        [TestMethod]
        public void IsInstanceOfTypeT_Fail_Null_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsInstanceOfType<string>(null, "My test message"));
            MSAssert.AreEqual("Assert.IsInstanceOfType failed. Expected:<System.String>. Actual:<(null)>. My test message", ex.Message);
        }

        [TestMethod]
        public void IsInstanceOfTypeT_Fail_WrongType_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsInstanceOfType<string>(5, "My test message"));
            MSAssert.AreEqual("Assert.IsInstanceOfType failed. Expected:<System.String>. Actual:<System.Int32>. My test message", ex.Message);
        }

        #endregion

        #region IsNotInstanceOfType

        [TestMethod]
        public void IsNotInstanceOfType_Success_Null()
        {
            AssertUnderTest.IsNotInstanceOfType(null, typeof(string));
        }

        [TestMethod]
        public void IsNotInstanceOfType_Success_WrongType()
        {
            AssertUnderTest.IsNotInstanceOfType(5, typeof(string));
        }

        [TestMethod]
        public void IsNotInstanceOfType_Fail_NullType()
        {
            MSAssert.ThrowsException<ArgumentNullException>(() => AssertUnderTest.IsNotInstanceOfType("Test", null!));
        }

        [TestMethod]
        public void IsNotInstanceOfType_Fail_SameType()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotInstanceOfType("Test", typeof(string)));
            MSAssert.AreEqual("Assert.IsNotInstanceOfType failed. NotExpected:<System.String>. Actual:<System.String>.", ex.Message);
        }

        [TestMethod]
        public void IsNotInstanceOfType_Fail_DerivedType()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotInstanceOfType(AssertUnderTest, typeof(MaSch.Test.Assertion.AssertBase)));
            MSAssert.AreEqual("Assert.IsNotInstanceOfType failed. NotExpected:<MaSch.Test.Assertion.AssertBase>. Actual:<MaSch.Test.Assertion.Assert>.", ex.Message);
        }

        [TestMethod]
        public void IsNotInstanceOfType_Fail_SameType_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotInstanceOfType("Test", typeof(string), "My test message"));
            MSAssert.AreEqual("Assert.IsNotInstanceOfType failed. NotExpected:<System.String>. Actual:<System.String>. My test message", ex.Message);
        }

        [TestMethod]
        public void IsNotInstanceOfType_Fail_DerivedType_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotInstanceOfType(AssertUnderTest, typeof(MaSch.Test.Assertion.AssertBase), "My test message"));
            MSAssert.AreEqual("Assert.IsNotInstanceOfType failed. NotExpected:<MaSch.Test.Assertion.AssertBase>. Actual:<MaSch.Test.Assertion.Assert>. My test message", ex.Message);
        }

        [TestMethod]
        public void IsNotInstanceOfTypeT_Success_Null()
        {
            AssertUnderTest.IsNotInstanceOfType<string>(null);
        }

        [TestMethod]
        public void IsNotInstanceOfTypeT_Success_WrongType()
        {
            AssertUnderTest.IsNotInstanceOfType<string>(5);
        }

        [TestMethod]
        public void IsNotInstanceOfTypeT_Fail_SameType()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotInstanceOfType<string>("Test"));
            MSAssert.AreEqual("Assert.IsNotInstanceOfType failed. NotExpected:<System.String>. Actual:<System.String>.", ex.Message);
        }

        [TestMethod]
        public void IsNotInstanceOfTypeT_Fail_DerivedType()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotInstanceOfType<MaSch.Test.Assertion.AssertBase>(AssertUnderTest));
            MSAssert.AreEqual("Assert.IsNotInstanceOfType failed. NotExpected:<MaSch.Test.Assertion.AssertBase>. Actual:<MaSch.Test.Assertion.Assert>.", ex.Message);
        }

        [TestMethod]
        public void IsNotInstanceOfTypeT_Fail_SameType_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotInstanceOfType<string>("Test", "My test message"));
            MSAssert.AreEqual("Assert.IsNotInstanceOfType failed. NotExpected:<System.String>. Actual:<System.String>. My test message", ex.Message);
        }

        [TestMethod]
        public void IsNotInstanceOfTypeT_Fail_DerivedType_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.IsNotInstanceOfType<MaSch.Test.Assertion.AssertBase>(AssertUnderTest, "My test message"));
            MSAssert.AreEqual("Assert.IsNotInstanceOfType failed. NotExpected:<MaSch.Test.Assertion.AssertBase>. Actual:<MaSch.Test.Assertion.Assert>. My test message", ex.Message);
        }

        #endregion

        #region Fail

        [TestMethod]
        public void Fail()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Fail());
            MSAssert.AreEqual("Assert.Fail failed.", ex.Message);
        }

        [TestMethod]
        public void Fail_WithMessage()
        {
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.Fail("My test message"));
            MSAssert.AreEqual("Assert.Fail failed. My test message", ex.Message);
        }

        #endregion

        #region ThrowsException

        [TestMethod]
        public void ThrowsException_Action_Success()
        {
            var mockEx = new InvalidOperationException("My test exception");
            var mock = new Mock<Action>();
            mock.Setup(x => x()).Throws(mockEx);
            var ex = AssertUnderTest.ThrowsException<InvalidOperationException>(() => mock.Object());
            MSAssert.AreSame(mockEx, ex);
        }

        [TestMethod]
        public void ThrowsException_Action_Fail_NoException()
        {
            var mock = new Mock<Action>();
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ThrowsException<InvalidOperationException>(() => mock.Object()));
            MSAssert.AreEqual("Assert.ThrowsException failed. ExpectedException:<InvalidOperationException>. ActualException:<(null)>. ExceptionDetails:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void ThrowsException_Action_Fail_DifferentException()
        {
            var mock = new Mock<Action>();
            var exex = new ArgumentException();
            mock.Setup(x => x()).Throws(exex);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ThrowsException<InvalidOperationException>(() => mock.Object()));
            MSAssert.AreEqual($"Assert.ThrowsException failed. ExpectedException:<InvalidOperationException>. ActualException:<ArgumentException>. ExceptionDetails:<{exex}>.", ex.Message);
        }

        [TestMethod]
        public void ThrowsException_Action_Fail_DerivedException()
        {
            var mock = new Mock<Action>();
            var exex = new ArgumentException();
            mock.Setup(x => x()).Throws(exex);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ThrowsException<Exception>(() => mock.Object()));
            MSAssert.AreEqual($"Assert.ThrowsException failed. ExpectedException:<Exception>. ActualException:<ArgumentException>. ExceptionDetails:<{exex}>.", ex.Message);
        }

        [TestMethod]
        public void ThrowsException_Action_Fail_NoException_WithMessage()
        {
            var mock = new Mock<Action>();
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ThrowsException<InvalidOperationException>(() => mock.Object(), "My test message"));
            MSAssert.AreEqual("Assert.ThrowsException failed. ExpectedException:<InvalidOperationException>. ActualException:<(null)>. ExceptionDetails:<(null)>. My test message", ex.Message);
        }

        [TestMethod]
        public void ThrowsException_Action_Fail_DifferentException_WithMessage()
        {
            var mock = new Mock<Action>();
            var exex = new ArgumentException();
            mock.Setup(x => x()).Throws(exex);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ThrowsException<InvalidOperationException>(() => mock.Object(), "My test message"));
            MSAssert.AreEqual($"Assert.ThrowsException failed. ExpectedException:<InvalidOperationException>. ActualException:<ArgumentException>. ExceptionDetails:<{exex}>. My test message", ex.Message);
        }

        [TestMethod]
        public void ThrowsException_Action_Fail_DerivedException_WithMessage()
        {
            var mock = new Mock<Action>();
            var exex = new ArgumentException();
            mock.Setup(x => x()).Throws(exex);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ThrowsException<Exception>(() => mock.Object(), "My test message"));
            MSAssert.AreEqual($"Assert.ThrowsException failed. ExpectedException:<Exception>. ActualException:<ArgumentException>. ExceptionDetails:<{exex}>. My test message", ex.Message);
        }

        [TestMethod]
        public void ThrowsException_Func_Success()
        {
            var mockEx = new InvalidOperationException("My test exception");
            var mock = new Mock<Func<object?>>();
            mock.Setup(x => x()).Throws(mockEx);
            var ex = AssertUnderTest.ThrowsException<InvalidOperationException>(() => mock.Object());
            MSAssert.AreSame(mockEx, ex);
        }

        [TestMethod]
        public void ThrowsException_Func_Fail_NoException()
        {
            var mock = new Mock<Func<object?>>();
            mock.Setup(x => x()).Returns(null);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ThrowsException<InvalidOperationException>(() => mock.Object()));
            MSAssert.AreEqual("Assert.ThrowsException failed. ExpectedException:<InvalidOperationException>. ActualException:<(null)>. ExceptionDetails:<(null)>.", ex.Message);
        }

        [TestMethod]
        public void ThrowsException_Func_Fail_DifferentException()
        {
            var mock = new Mock<Func<object?>>();
            var exex = new ArgumentException();
            mock.Setup(x => x()).Throws(exex);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ThrowsException<InvalidOperationException>(() => mock.Object()));
            MSAssert.AreEqual($"Assert.ThrowsException failed. ExpectedException:<InvalidOperationException>. ActualException:<ArgumentException>. ExceptionDetails:<{exex}>.", ex.Message);
        }

        [TestMethod]
        public void ThrowsException_Func_Fail_DerivedException()
        {
            var mock = new Mock<Func<object?>>();
            var exex = new ArgumentException();
            mock.Setup(x => x()).Throws(exex);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ThrowsException<Exception>(() => mock.Object()));
            MSAssert.AreEqual($"Assert.ThrowsException failed. ExpectedException:<Exception>. ActualException:<ArgumentException>. ExceptionDetails:<{exex}>.", ex.Message);
        }

        [TestMethod]
        public void ThrowsException_Func_Fail_NoException_WithMessage()
        {
            var mock = new Mock<Func<object?>>();
            mock.Setup(x => x()).Returns(null);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ThrowsException<InvalidOperationException>(() => mock.Object(), "My test message"));
            MSAssert.AreEqual("Assert.ThrowsException failed. ExpectedException:<InvalidOperationException>. ActualException:<(null)>. ExceptionDetails:<(null)>. My test message", ex.Message);
        }

        [TestMethod]
        public void ThrowsException_Func_Fail_DifferentException_WithMessage()
        {
            var mock = new Mock<Func<object?>>();
            var exex = new ArgumentException();
            mock.Setup(x => x()).Throws(exex);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ThrowsException<InvalidOperationException>(() => mock.Object(), "My test message"));
            MSAssert.AreEqual($"Assert.ThrowsException failed. ExpectedException:<InvalidOperationException>. ActualException:<ArgumentException>. ExceptionDetails:<{exex}>. My test message", ex.Message);
        }

        [TestMethod]
        public void ThrowsException_Func_Fail_DerivedException_WithMessage()
        {
            var mock = new Mock<Func<object?>>();
            var exex = new ArgumentException();
            mock.Setup(x => x()).Throws(exex);
            var ex = MSAssert.ThrowsException<AssertFailedException>(() => AssertUnderTest.ThrowsException<Exception>(() => mock.Object(), "My test message"));
            MSAssert.AreEqual($"Assert.ThrowsException failed. ExpectedException:<Exception>. ActualException:<ArgumentException>. ExceptionDetails:<{exex}>. My test message", ex.Message);
        }

        #endregion

        #region ThrowsExceptionAsync

        [TestMethod]
        public async Task ThrowsExceptionAsync_Action_Success()
        {
            var mockEx = new InvalidOperationException("My test exception");
            var mock = new Mock<Func<Task>>();
            mock.Setup(x => x()).Throws(mockEx);
            var ex = await AssertUnderTest.ThrowsExceptionAsync<InvalidOperationException>(() => mock.Object());
            MSAssert.AreSame(mockEx, ex);
        }

        [TestMethod]
        public async Task ThrowsExceptionAsync_Action_Fail_NoException()
        {
            var mock = new Mock<Func<Task>>();
            mock.Setup(x => x()).Returns(Task.CompletedTask);
            var ex = await MSAssert.ThrowsExceptionAsync<AssertFailedException>(async () => await AssertUnderTest.ThrowsExceptionAsync<InvalidOperationException>(() => mock.Object()));
            MSAssert.AreEqual("Assert.ThrowsExceptionAsync failed. ExpectedException:<InvalidOperationException>. ActualException:<(null)>. ExceptionDetails:<(null)>.", ex.Message);
        }

        [TestMethod]
        public async Task ThrowsExceptionAsync_Action_Fail_DifferentException()
        {
            var mock = new Mock<Func<Task>>();
            var exex = new ArgumentException();
            mock.Setup(x => x()).Throws(exex);
            var ex = await MSAssert.ThrowsExceptionAsync<AssertFailedException>(async () => await AssertUnderTest.ThrowsExceptionAsync<InvalidOperationException>(() => mock.Object()));
            MSAssert.AreEqual($"Assert.ThrowsExceptionAsync failed. ExpectedException:<InvalidOperationException>. ActualException:<ArgumentException>. ExceptionDetails:<{exex}>.", ex.Message);
        }

        [TestMethod]
        public async Task ThrowsExceptionAsync_Action_Fail_DerivedException()
        {
            var mock = new Mock<Func<Task>>();
            var exex = new ArgumentException();
            mock.Setup(x => x()).Throws(exex);
            var ex = await MSAssert.ThrowsExceptionAsync<AssertFailedException>(async () => await AssertUnderTest.ThrowsExceptionAsync<Exception>(() => mock.Object()));
            MSAssert.AreEqual($"Assert.ThrowsExceptionAsync failed. ExpectedException:<Exception>. ActualException:<ArgumentException>. ExceptionDetails:<{exex}>.", ex.Message);
        }

        [TestMethod]
        public async Task ThrowsExceptionAsync_Action_Fail_NoException_WithMessage()
        {
            var mock = new Mock<Func<Task>>();
            mock.Setup(x => x()).Returns(Task.CompletedTask);
            var ex = await MSAssert.ThrowsExceptionAsync<AssertFailedException>(async () => await AssertUnderTest.ThrowsExceptionAsync<InvalidOperationException>(() => mock.Object(), "My test message"));
            MSAssert.AreEqual("Assert.ThrowsExceptionAsync failed. ExpectedException:<InvalidOperationException>. ActualException:<(null)>. ExceptionDetails:<(null)>. My test message", ex.Message);
        }

        [TestMethod]
        public async Task ThrowsExceptionAsync_Action_Fail_DifferentException_WithMessage()
        {
            var mock = new Mock<Func<Task>>();
            var exex = new ArgumentException();
            mock.Setup(x => x()).Throws(exex);
            var ex = await MSAssert.ThrowsExceptionAsync<AssertFailedException>(async () => await AssertUnderTest.ThrowsExceptionAsync<InvalidOperationException>(() => mock.Object(), "My test message"));
            MSAssert.AreEqual($"Assert.ThrowsExceptionAsync failed. ExpectedException:<InvalidOperationException>. ActualException:<ArgumentException>. ExceptionDetails:<{exex}>. My test message", ex.Message);
        }

        [TestMethod]
        public async Task ThrowsExceptionAsync_Action_Fail_DerivedException_WithMessage()
        {
            var mock = new Mock<Func<Task>>();
            var exex = new ArgumentException();
            mock.Setup(x => x()).Throws(exex);
            var ex = await MSAssert.ThrowsExceptionAsync<AssertFailedException>(async () => await AssertUnderTest.ThrowsExceptionAsync<Exception>(() => mock.Object(), "My test message"));
            MSAssert.AreEqual($"Assert.ThrowsExceptionAsync failed. ExpectedException:<Exception>. ActualException:<ArgumentException>. ExceptionDetails:<{exex}>. My test message", ex.Message);
        }

        [TestMethod]
        public async Task ThrowsExceptionAsync_Func_Success()
        {
            var mockEx = new InvalidOperationException("My test exception");
            var mock = new Mock<Func<Task<object?>>>();
            mock.Setup(x => x()).Throws(mockEx);
            var ex = await AssertUnderTest.ThrowsExceptionAsync<InvalidOperationException>(() => mock.Object());
            MSAssert.AreSame(mockEx, ex);
        }

        [TestMethod]
        public async Task ThrowsExceptionAsync_Func_Fail_NoException()
        {
            var mock = new Mock<Func<Task<object?>>>();
            mock.Setup(x => x()).Returns(Task.FromResult<object?>(null));
            var ex = await MSAssert.ThrowsExceptionAsync<AssertFailedException>(async () => await AssertUnderTest.ThrowsExceptionAsync<InvalidOperationException>(() => mock.Object()));
            MSAssert.AreEqual("Assert.ThrowsExceptionAsync failed. ExpectedException:<InvalidOperationException>. ActualException:<(null)>. ExceptionDetails:<(null)>.", ex.Message);
        }

        [TestMethod]
        public async Task ThrowsExceptionAsync_Func_Fail_DifferentException()
        {
            var mock = new Mock<Func<Task<object?>>>();
            var exex = new ArgumentException();
            mock.Setup(x => x()).Throws(exex);
            var ex = await MSAssert.ThrowsExceptionAsync<AssertFailedException>(async () => await AssertUnderTest.ThrowsExceptionAsync<InvalidOperationException>(() => mock.Object()));
            MSAssert.AreEqual($"Assert.ThrowsExceptionAsync failed. ExpectedException:<InvalidOperationException>. ActualException:<ArgumentException>. ExceptionDetails:<{exex}>.", ex.Message);
        }

        [TestMethod]
        public async Task ThrowsExceptionAsync_Func_Fail_DerivedException()
        {
            var mock = new Mock<Func<Task<object?>>>();
            var exex = new ArgumentException();
            mock.Setup(x => x()).Throws(exex);
            var ex = await MSAssert.ThrowsExceptionAsync<AssertFailedException>(async () => await AssertUnderTest.ThrowsExceptionAsync<Exception>(() => mock.Object()));
            MSAssert.AreEqual($"Assert.ThrowsExceptionAsync failed. ExpectedException:<Exception>. ActualException:<ArgumentException>. ExceptionDetails:<{exex}>.", ex.Message);
        }

        [TestMethod]
        public async Task ThrowsExceptionAsync_Func_Fail_NoException_WithMessage()
        {
            var mock = new Mock<Func<Task<object?>>>();
            mock.Setup(x => x()).Returns(Task.FromResult<object?>(null));
            var ex = await MSAssert.ThrowsExceptionAsync<AssertFailedException>(async () => await AssertUnderTest.ThrowsExceptionAsync<InvalidOperationException>(() => mock.Object(), "My test message"));
            MSAssert.AreEqual("Assert.ThrowsExceptionAsync failed. ExpectedException:<InvalidOperationException>. ActualException:<(null)>. ExceptionDetails:<(null)>. My test message", ex.Message);
        }

        [TestMethod]
        public async Task ThrowsExceptionAsync_Func_Fail_DifferentException_WithMessage()
        {
            var mock = new Mock<Func<Task<object?>>>();
            var exex = new ArgumentException();
            mock.Setup(x => x()).Throws(exex);
            var ex = await MSAssert.ThrowsExceptionAsync<AssertFailedException>(async () => await AssertUnderTest.ThrowsExceptionAsync<InvalidOperationException>(() => mock.Object(), "My test message"));
            MSAssert.AreEqual($"Assert.ThrowsExceptionAsync failed. ExpectedException:<InvalidOperationException>. ActualException:<ArgumentException>. ExceptionDetails:<{exex}>. My test message", ex.Message);
        }

        [TestMethod]
        public async Task ThrowsExceptionAsync_Func_Fail_DerivedException_WithMessage()
        {
            var mock = new Mock<Func<Task<object?>>>();
            var exex = new ArgumentException();
            mock.Setup(x => x()).Throws(exex);
            var ex = await MSAssert.ThrowsExceptionAsync<AssertFailedException>(async () => await AssertUnderTest.ThrowsExceptionAsync<Exception>(() => mock.Object(), "My test message"));
            MSAssert.AreEqual($"Assert.ThrowsExceptionAsync failed. ExpectedException:<Exception>. ActualException:<ArgumentException>. ExceptionDetails:<{exex}>. My test message", ex.Message);
        }

        #endregion
    }
}
