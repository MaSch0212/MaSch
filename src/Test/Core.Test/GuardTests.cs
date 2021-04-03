using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MaSch.Core.Test
{
    [TestClass]
    public class GuardTests
    {
        [TestMethod]
        public void NotNull_Null()
        {
            var ex = Assert.ThrowsException<ArgumentNullException>(() => Guard.NotNull<object?>(null!, "MyParamName"));

            Assert.AreEqual("MyParamName", ex.ParamName);
        }

        [TestMethod]
        public void NotNull_Success()
        {
            var obj = new object();

            var result = Guard.NotNull(obj, nameof(obj));

            Assert.AreSame(obj, result);
        }

        [TestMethod]
        public void NotNullOrEmpty_Null()
        {
            var ex = Assert.ThrowsException<ArgumentNullException>(() => Guard.NotNullOrEmpty(null, "MyParamName"));

            Assert.AreEqual("MyParamName", ex.ParamName);
        }

        [TestMethod]
        public void NotNullOrEmpty_Empty()
        {
            var ex = Assert.ThrowsException<ArgumentException>(() => Guard.NotNullOrEmpty(string.Empty, "MyParamName"));

            Assert.AreEqual("MyParamName", ex.ParamName);
        }

        [TestMethod]
        public void NotNullOrEmpty_Success()
        {
            var obj = "Test";

            var result = Guard.NotNullOrEmpty(obj, nameof(obj));

            Assert.AreSame(obj, result);
        }

        [TestMethod]
        public void NotOutOfRange_Null()
        {
            var ex = Assert.ThrowsException<ArgumentNullException>(() => Guard.NotOutOfRange(null, "MyParamName", string.Empty, string.Empty));

            Assert.AreEqual("MyParamName", ex.ParamName);
        }

        [TestMethod]
        public void NotOutOfRange_SmallerThanMin()
        {
            var ex = Assert.ThrowsException<ArgumentOutOfRangeException>(() => Guard.NotOutOfRange("0", "MyParamName", "a", "c"));

            Assert.AreEqual("MyParamName", ex.ParamName);
        }

        [TestMethod]
        public void NotOutOfRange_GreaterThanMax()
        {
            var ex = Assert.ThrowsException<ArgumentOutOfRangeException>(() => Guard.NotOutOfRange("d", "MyParamName", "a", "c"));

            Assert.AreEqual("MyParamName", ex.ParamName);
        }

        [TestMethod]
        public void NotOutOfRange_BetweenMinAndMax()
        {
            var obj = "b";

            var result = Guard.NotOutOfRange(obj, nameof(obj), "a", "c");

            Assert.AreSame(obj, result);
        }

        [TestMethod]
        public void NotOutOfRange_EqualToMin()
        {
            var obj = "a";

            var result = Guard.NotOutOfRange(obj, nameof(obj), "a", "c");

            Assert.AreSame(obj, result);
        }

        [TestMethod]
        public void NotOutOfRange_EqualToMax()
        {
            var obj = "c";

            var result = Guard.NotOutOfRange(obj, nameof(obj), "a", "c");

            Assert.AreSame(obj, result);
        }

        [TestMethod]
        public void OfType_Null()
        {
            var ex = Assert.ThrowsException<ArgumentNullException>(() => Guard.OfType<string>(null, "MyParamName"));

            Assert.AreEqual("MyParamName", ex.ParamName);
        }

        [TestMethod]
        public void OfType_WrongType()
        {
            var ex = Assert.ThrowsException<ArgumentException>(() => Guard.OfType<string>(1, "MyParamName"));

            Assert.AreEqual("MyParamName", ex.ParamName);
        }

        [TestMethod]
        public void OfType_ExactTypeMatch()
        {
            var obj = new TestClassType();

            var result = Guard.OfType<TestClassType>(obj, nameof(obj));

            Assert.AreSame(obj, result);
        }

        [TestMethod]
        public void OfType_DerivedTypeMatch()
        {
            var obj = new TestDerivedType();

            var result = Guard.OfType<TestClassType>(obj, nameof(obj));

            Assert.AreSame(obj, result);
        }

        [TestMethod]
        public void OfType_InterfaceMatch()
        {
            var obj = new TestClassType();

            var result = Guard.OfType<ITestInterfaceType>(obj, nameof(obj));

            Assert.AreSame(obj, result);
        }

        [TestMethod]
        public void OfType_AllowNull_Null()
        {
            var result = Guard.OfType<TestClassType>(null, "blub", true);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void OfType_AllowNull_WrongType()
        {
            var ex = Assert.ThrowsException<ArgumentException>(() => Guard.OfType<string>(1, "MyParamName", true));

            Assert.AreEqual("MyParamName", ex.ParamName);
        }

        [TestMethod]
        public void OfType_AllowNull_ExactTypeMatch()
        {
            var obj = new TestClassType();

            var result = Guard.OfType<TestClassType>(obj, nameof(obj), true);

            Assert.AreSame(obj, result);
        }

        [TestMethod]
        public void OfType_AllowNull_DerivedTypeMatch()
        {
            var obj = new TestDerivedType();

            var result = Guard.OfType<TestClassType>(obj, nameof(obj), true);

            Assert.AreSame(obj, result);
        }

        [TestMethod]
        public void OfType_AllowNull_InterfaceMatch()
        {
            var obj = new TestClassType();

            var result = Guard.OfType<ITestInterfaceType>(obj, nameof(obj), true);

            Assert.AreSame(obj, result);
        }

        [TestMethod]
        public void OfType_AllowedTypes_Null()
        {
            var ex = Assert.ThrowsException<ArgumentNullException>(() => Guard.OfType(null, "MyParamName", typeof(string)));

            Assert.AreEqual("MyParamName", ex.ParamName);
        }

        [TestMethod]
        public void OfType_AllowedTypes_NullArray()
        {
            var ex = Assert.ThrowsException<ArgumentNullException>(() => Guard.OfType(1, "MyParamName", (Type[])null!));

            Assert.AreEqual("allowedTypes", ex.ParamName);
        }

        [TestMethod]
        public void OfType_AllowedTypes_EmptyArray()
        {
            var ex = Assert.ThrowsException<ArgumentException>(() => Guard.OfType(1, "MyParamName", Array.Empty<Type>()));

            Assert.AreEqual("allowedTypes", ex.ParamName);
        }

        [TestMethod]
        public void OfType_AllowedTypes_WrongType()
        {
            var ex = Assert.ThrowsException<ArgumentException>(() => Guard.OfType(1, "MyParamName", typeof(string)));

            Assert.AreEqual("MyParamName", ex.ParamName);
        }

        [TestMethod]
        public void OfType_AllowedTypes_ExactTypeMatch()
        {
            var obj = new TestClassType();

            var result = Guard.OfType(obj, nameof(obj), typeof(int), typeof(TestClassType), typeof(double));

            Assert.AreSame(obj, result);
        }

        [TestMethod]
        public void OfType_AllowedTypes_DerivedTypeMatch()
        {
            var obj = new TestDerivedType();

            var result = Guard.OfType(obj, nameof(obj), typeof(int), typeof(TestClassType), typeof(double));

            Assert.AreSame(obj, result);
        }

        [TestMethod]
        public void OfType_AllowedTypes_InterfaceMatch()
        {
            var obj = new TestClassType();

            var result = Guard.OfType(obj, nameof(obj), typeof(int), typeof(ITestInterfaceType), typeof(double));

            Assert.AreSame(obj, result);
        }

        [TestMethod]
        public void OfType_AllowedTypes_AllowNull_Null()
        {
            var result = Guard.OfType(null, "MyParamName", true, typeof(string));

            Assert.IsNull(result);
        }

        [TestMethod]
        public void OfType_AllowedTypes_AllowNull_NullArray()
        {
            var ex = Assert.ThrowsException<ArgumentNullException>(() => Guard.OfType(1, "MyParamName", true, (Type[])null!));

            Assert.AreEqual("allowedTypes", ex.ParamName);
        }

        [TestMethod]
        public void OfType_AllowedTypes_AllowNull_EmptyArray()
        {
            var ex = Assert.ThrowsException<ArgumentException>(() => Guard.OfType(1, "MyParamName", true, Array.Empty<Type>()));

            Assert.AreEqual("allowedTypes", ex.ParamName);
        }

        [TestMethod]
        public void OfType_AllowedTypes_AllowNull_WrongType()
        {
            var ex = Assert.ThrowsException<ArgumentException>(() => Guard.OfType(1, "MyParamName", true, typeof(string)));

            Assert.AreEqual("MyParamName", ex.ParamName);
        }

        [TestMethod]
        public void OfType_AllowedTypes_AllowNull_ExactTypeMatch()
        {
            var obj = new TestClassType();

            var result = Guard.OfType(obj, nameof(obj), true, typeof(int), typeof(TestClassType), typeof(double));

            Assert.AreSame(obj, result);
        }

        [TestMethod]
        public void OfType_AllowedTypes_AllowNull_DerivedTypeMatch()
        {
            var obj = new TestDerivedType();

            var result = Guard.OfType(obj, nameof(obj), true, typeof(int), typeof(TestClassType), typeof(double));

            Assert.AreSame(obj, result);
        }

        [TestMethod]
        public void OfType_AllowedTypes_AllowNull_InterfaceMatch()
        {
            var obj = new TestClassType();

            var result = Guard.OfType(obj, nameof(obj), true, typeof(int), typeof(ITestInterfaceType), typeof(double));

            Assert.AreSame(obj, result);
        }

        private interface ITestInterfaceType
        {
        }

        private class TestClassType : ITestInterfaceType
        {
        }

        private class TestDerivedType : TestClassType
        {
        }
    }
}
