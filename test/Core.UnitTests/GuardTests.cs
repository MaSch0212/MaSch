namespace MaSch.Core.UnitTests;

[TestClass]
public class GuardTests : TestClassBase
{
    private interface ITestInterfaceType
    {
    }

    [TestMethod]
    public void NotNull_Null()
    {
        var ex = Assert.ThrowsException<ArgumentNullException>(() => Guard.NotNull<object?>(null!));

        Assert.AreEqual("null", ex.ParamName);
    }

    [TestMethod]
    public void NotNull_Null_WithName()
    {
        var ex = Assert.ThrowsException<ArgumentNullException>(() => Guard.NotNull<object?>(null!, "MyParamName"));

        Assert.AreEqual("MyParamName", ex.ParamName);
    }

    [TestMethod]
    public void NotNull_Success()
    {
        var obj = new object();

        var result = Guard.NotNull(obj);

        Assert.AreSame(obj, result);
    }

    [TestMethod]
    public void NotNullOrEmpty_String_Null()
    {
        var ex = Assert.ThrowsException<ArgumentNullException>(() => Guard.NotNullOrEmpty(null));

        Assert.AreEqual("null", ex.ParamName);
    }

    [TestMethod]
    public void NotNullOrEmpty_String_Null_WithName()
    {
        var ex = Assert.ThrowsException<ArgumentNullException>(() => Guard.NotNullOrEmpty(null, "MyParamName"));

        Assert.AreEqual("MyParamName", ex.ParamName);
    }

    [TestMethod]
    public void NotNullOrEmpty_String_Empty()
    {
        var ex = Assert.ThrowsException<ArgumentException>(() => Guard.NotNullOrEmpty(string.Empty));

        Assert.AreEqual("string.Empty", ex.ParamName);
    }

    [TestMethod]
    public void NotNullOrEmpty_String_Empty_WithName()
    {
        var ex = Assert.ThrowsException<ArgumentException>(() => Guard.NotNullOrEmpty(string.Empty, "MyParamName"));

        Assert.AreEqual("MyParamName", ex.ParamName);
    }

    [TestMethod]
    public void NotNullOrEmpty_String_Success()
    {
        var obj = "Test";

        var result = Guard.NotNullOrEmpty(obj);

        Assert.AreSame(obj, result);
    }

    [TestMethod]
    public void NotNullOrEmpty_Collection_Null()
    {
        var ex = Assert.ThrowsException<ArgumentNullException>(() => Guard.NotNullOrEmpty<ICollection>(null));

        Assert.AreEqual("null", ex.ParamName);
    }

    [TestMethod]
    public void NotNullOrEmpty_Collection_Null_WithName()
    {
        var ex = Assert.ThrowsException<ArgumentNullException>(() => Guard.NotNullOrEmpty<ICollection>(null, "MyParamName"));

        Assert.AreEqual("MyParamName", ex.ParamName);
    }

    [TestMethod]
    public void NotNullOrEmpty_Collection_Empty()
    {
        var ex = Assert.ThrowsException<ArgumentException>(() => Guard.NotNullOrEmpty(Array.Empty<object>()));

        Assert.AreEqual("Array.Empty<object>()", ex.ParamName);
    }

    [TestMethod]
    public void NotNullOrEmpty_Collection_Empty_WithName()
    {
        var ex = Assert.ThrowsException<ArgumentException>(() => Guard.NotNullOrEmpty(Array.Empty<object>(), "MyParamName"));

        Assert.AreEqual("MyParamName", ex.ParamName);
    }

    [TestMethod]
    public void NotNullOrEmpty_Collection_Success()
    {
        var obj = new[] { "Test" };

        var result = Guard.NotNullOrEmpty(obj);

        Assert.AreSame(obj, result);
    }

    [TestMethod]
    public void NotOutOfRange_Null()
    {
        var ex = Assert.ThrowsException<ArgumentNullException>(() => Guard.NotOutOfRange(null, string.Empty, string.Empty));

        Assert.AreEqual("null", ex.ParamName);
    }

    [TestMethod]
    public void NotOutOfRange_Null_WithName()
    {
        var ex = Assert.ThrowsException<ArgumentNullException>(() => Guard.NotOutOfRange(null, string.Empty, string.Empty, name: "MyParamName"));

        Assert.AreEqual("MyParamName", ex.ParamName);
    }

    [TestMethod]
    public void NotOutOfRange_SmallerThanMin()
    {
        var ex = Assert.ThrowsException<ArgumentOutOfRangeException>(() => Guard.NotOutOfRange("0", "a", "c"));

        Assert.AreEqual("\"0\"", ex.ParamName);
    }

    [TestMethod]
    public void NotOutOfRange_SmallerThanMin_WithName()
    {
        var ex = Assert.ThrowsException<ArgumentOutOfRangeException>(() => Guard.NotOutOfRange("0", "a", "c", name: "MyParamName"));

        Assert.AreEqual("MyParamName", ex.ParamName);
    }

    [TestMethod]
    public void NotOutOfRange_GreaterThanMax()
    {
        var ex = Assert.ThrowsException<ArgumentOutOfRangeException>(() => Guard.NotOutOfRange("d", "a", "c"));

        Assert.AreEqual("\"d\"", ex.ParamName);
    }

    [TestMethod]
    public void NotOutOfRange_GreaterThanMax_WithName()
    {
        var ex = Assert.ThrowsException<ArgumentOutOfRangeException>(() => Guard.NotOutOfRange("d", "a", "c", name: "MyParamName"));

        Assert.AreEqual("MyParamName", ex.ParamName);
    }

    [TestMethod]
    public void NotOutOfRange_BetweenMinAndMax()
    {
        var obj = "b";

        var result = Guard.NotOutOfRange(obj, "a", "c");

        Assert.AreSame(obj, result);
    }

    [TestMethod]
    public void NotOutOfRange_EqualToMin()
    {
        var obj = "a";

        var result = Guard.NotOutOfRange(obj, "a", "c");

        Assert.AreSame(obj, result);
    }

    [TestMethod]
    public void NotOutOfRange_EqualToMax()
    {
        var obj = "c";

        var result = Guard.NotOutOfRange(obj, "a", "c");

        Assert.AreSame(obj, result);
    }

    [TestMethod]
    public void OfType_Null()
    {
        var ex = Assert.ThrowsException<ArgumentNullException>(() => Guard.OfType<string>(null));

        Assert.AreEqual("null", ex.ParamName);
    }

    [TestMethod]
    public void OfType_Null_WithName()
    {
        var ex = Assert.ThrowsException<ArgumentNullException>(() => Guard.OfType<string>(null, "MyParamName"));

        Assert.AreEqual("MyParamName", ex.ParamName);
    }

    [TestMethod]
    public void OfType_WrongType()
    {
        var ex = Assert.ThrowsException<ArgumentException>(() => Guard.OfType<string>(1));

        Assert.AreEqual("1", ex.ParamName);
    }

    [TestMethod]
    public void OfType_WrongType_WithName()
    {
        var ex = Assert.ThrowsException<ArgumentException>(() => Guard.OfType<string>(1, "MyParamName"));

        Assert.AreEqual("MyParamName", ex.ParamName);
    }

    [TestMethod]
    public void OfType_ExactTypeMatch()
    {
        var obj = new TestClassType();

        var result = Guard.OfType<TestClassType>(obj);

        Assert.AreSame(obj, result);
    }

    [TestMethod]
    public void OfType_DerivedTypeMatch()
    {
        var obj = new TestDerivedType();

        var result = Guard.OfType<TestClassType>(obj);

        Assert.AreSame(obj, result);
    }

    [TestMethod]
    public void OfType_InterfaceMatch()
    {
        var obj = new TestClassType();

        var result = Guard.OfType<ITestInterfaceType>(obj);

        Assert.AreSame(obj, result);
    }

    [TestMethod]
    public void OfType_AllowNull_Null()
    {
        var result = Guard.OfType<TestClassType>(null, true);

        Assert.IsNull(result);
    }

    [TestMethod]
    public void OfType_AllowNull_WrongType()
    {
        var ex = Assert.ThrowsException<ArgumentException>(() => Guard.OfType<string>(1, true));

        Assert.AreEqual("1", ex.ParamName);
    }

    [TestMethod]
    public void OfType_AllowNull_WrongType_WithName()
    {
        var ex = Assert.ThrowsException<ArgumentException>(() => Guard.OfType<string>(1, true, "MyParamName"));

        Assert.AreEqual("MyParamName", ex.ParamName);
    }

    [TestMethod]
    public void OfType_AllowNull_ExactTypeMatch()
    {
        var obj = new TestClassType();

        var result = Guard.OfType<TestClassType>(obj, true);

        Assert.AreSame(obj, result);
    }

    [TestMethod]
    public void OfType_AllowNull_DerivedTypeMatch()
    {
        var obj = new TestDerivedType();

        var result = Guard.OfType<TestClassType>(obj, true);

        Assert.AreSame(obj, result);
    }

    [TestMethod]
    public void OfType_AllowNull_InterfaceMatch()
    {
        var obj = new TestClassType();

        var result = Guard.OfType<ITestInterfaceType>(obj, true);

        Assert.AreSame(obj, result);
    }

    [TestMethod]
    public void OfType_AllowedType_Null()
    {
        var ex = Assert.ThrowsException<ArgumentNullException>(() => Guard.OfType(null, typeof(string)));

        Assert.AreEqual("null", ex.ParamName);
    }

    [TestMethod]
    public void OfType_AllowedType_Null_WithName()
    {
        var ex = Assert.ThrowsException<ArgumentNullException>(() => Guard.OfType(null, typeof(string), "MyParamName"));

        Assert.AreEqual("MyParamName", ex.ParamName);
    }

    [TestMethod]
    public void OfType_AllowedTypes_Null()
    {
        var ex = Assert.ThrowsException<ArgumentNullException>(() => Guard.OfType(null, new[] { typeof(string) }));

        Assert.AreEqual("null", ex.ParamName);
    }

    [TestMethod]
    public void OfType_AllowedTypes_Null_WithName()
    {
        var ex = Assert.ThrowsException<ArgumentNullException>(() => Guard.OfType(null, new[] { typeof(string) }, "MyParamName"));

        Assert.AreEqual("MyParamName", ex.ParamName);
    }

    [TestMethod]
    public void OfType_AllowedTypes_NullArray()
    {
        var ex = Assert.ThrowsException<ArgumentNullException>(() => Guard.OfType(1, (Type[])null!));

        Assert.AreEqual("allowedTypes", ex.ParamName);
    }

    [TestMethod]
    public void OfType_AllowedTypes_EmptyArray()
    {
        var ex = Assert.ThrowsException<ArgumentException>(() => Guard.OfType(1, Type.EmptyTypes));

        Assert.AreEqual("allowedTypes", ex.ParamName);
    }

    [TestMethod]
    public void OfType_AllowedType_WrongType()
    {
        var ex = Assert.ThrowsException<ArgumentException>(() => Guard.OfType(1, typeof(string)));

        Assert.AreEqual("1", ex.ParamName);
    }

    [TestMethod]
    public void OfType_AllowedType_WrongType_WithName()
    {
        var ex = Assert.ThrowsException<ArgumentException>(() => Guard.OfType(1, typeof(string), "MyParamName"));

        Assert.AreEqual("MyParamName", ex.ParamName);
    }

    [TestMethod]
    public void OfType_AllowedTypes_WrongType()
    {
        var ex = Assert.ThrowsException<ArgumentException>(() => Guard.OfType(1, new[] { typeof(string) }));

        Assert.AreEqual("1", ex.ParamName);
    }

    [TestMethod]
    public void OfType_AllowedTypes_WrongType_WithName()
    {
        var ex = Assert.ThrowsException<ArgumentException>(() => Guard.OfType(1, new[] { typeof(string) }, "MyParamName"));

        Assert.AreEqual("MyParamName", ex.ParamName);
    }

    [TestMethod]
    public void OfType_AllowedType_ExactTypeMatch()
    {
        var obj = new TestClassType();

        var result = Guard.OfType(obj, typeof(TestClassType));

        Assert.AreSame(obj, result);
    }

    [TestMethod]
    public void OfType_AllowedType_DerivedTypeMatch()
    {
        var obj = new TestDerivedType();

        var result = Guard.OfType(obj, typeof(TestClassType));

        Assert.AreSame(obj, result);
    }

    [TestMethod]
    public void OfType_AllowedType_InterfaceMatch()
    {
        var obj = new TestClassType();

        var result = Guard.OfType(obj, typeof(ITestInterfaceType));

        Assert.AreSame(obj, result);
    }

    [TestMethod]
    public void OfType_AllowedTypes_ExactTypeMatch()
    {
        var obj = new TestClassType();

        var result = Guard.OfType(obj, new[] { typeof(int), typeof(TestClassType), typeof(double) });

        Assert.AreSame(obj, result);
    }

    [TestMethod]
    public void OfType_AllowedTypes_DerivedTypeMatch()
    {
        var obj = new TestDerivedType();

        var result = Guard.OfType(obj, new[] { typeof(int), typeof(TestClassType), typeof(double) });

        Assert.AreSame(obj, result);
    }

    [TestMethod]
    public void OfType_AllowedTypes_InterfaceMatch()
    {
        var obj = new TestClassType();

        var result = Guard.OfType(obj, new[] { typeof(int), typeof(ITestInterfaceType), typeof(double) });

        Assert.AreSame(obj, result);
    }

    [TestMethod]
    public void OfType_AllowedType_AllowNull_Null()
    {
        var result = Guard.OfType(null, typeof(string), true);

        Assert.IsNull(result);
    }

    [TestMethod]
    public void OfType_AllowedTypes_AllowNull_Null()
    {
        var result = Guard.OfType(null, new[] { typeof(string) }, true, "MyParamName");

        Assert.IsNull(result);
    }

    [TestMethod]
    public void OfType_AllowedTypes_AllowNull_NullArray()
    {
        var ex = Assert.ThrowsException<ArgumentNullException>(() => Guard.OfType(1, (Type[])null!, true));

        Assert.AreEqual("allowedTypes", ex.ParamName);
    }

    [TestMethod]
    public void OfType_AllowedTypes_AllowNull_EmptyArray()
    {
        var ex = Assert.ThrowsException<ArgumentException>(() => Guard.OfType(1, Type.EmptyTypes, true));

        Assert.AreEqual("allowedTypes", ex.ParamName);
    }

    [TestMethod]
    public void OfType_AllowedType_AllowNull_WrongType()
    {
        var ex = Assert.ThrowsException<ArgumentException>(() => Guard.OfType(1, typeof(string), true));

        Assert.AreEqual("1", ex.ParamName);
    }

    [TestMethod]
    public void OfType_AllowedType_AllowNull_WrongType_WithName()
    {
        var ex = Assert.ThrowsException<ArgumentException>(() => Guard.OfType(1, typeof(string), true, "MyParamName"));

        Assert.AreEqual("MyParamName", ex.ParamName);
    }

    [TestMethod]
    public void OfType_AllowedTypes_AllowNull_WrongType()
    {
        var ex = Assert.ThrowsException<ArgumentException>(() => Guard.OfType(1, new[] { typeof(string) }, true));

        Assert.AreEqual("1", ex.ParamName);
    }

    [TestMethod]
    public void OfType_AllowedTypes_AllowNull_WrongType_WithName()
    {
        var ex = Assert.ThrowsException<ArgumentException>(() => Guard.OfType(1, new[] { typeof(string) }, true, "MyParamName"));

        Assert.AreEqual("MyParamName", ex.ParamName);
    }

    [TestMethod]
    public void OfType_AllowedType_AllowNull_ExactTypeMatch()
    {
        var obj = new TestClassType();

        var result = Guard.OfType(obj, typeof(TestClassType), true);

        Assert.AreSame(obj, result);
    }

    [TestMethod]
    public void OfType_AllowedType_AllowNull_DerivedTypeMatch()
    {
        var obj = new TestDerivedType();

        var result = Guard.OfType(obj, typeof(TestClassType), true);

        Assert.AreSame(obj, result);
    }

    [TestMethod]
    public void OfType_AllowedType_AllowNull_InterfaceMatch()
    {
        var obj = new TestClassType();

        var result = Guard.OfType(obj, typeof(ITestInterfaceType), true);

        Assert.AreSame(obj, result);
    }

    [TestMethod]
    public void OfType_AllowedTypes_AllowNull_ExactTypeMatch()
    {
        var obj = new TestClassType();

        var result = Guard.OfType(obj, new[] { typeof(int), typeof(TestClassType), typeof(double) }, true);

        Assert.AreSame(obj, result);
    }

    [TestMethod]
    public void OfType_AllowedTypes_AllowNull_DerivedTypeMatch()
    {
        var obj = new TestDerivedType();

        var result = Guard.OfType(obj, new[] { typeof(int), typeof(TestClassType), typeof(double) }, true);

        Assert.AreSame(obj, result);
    }

    [TestMethod]
    public void OfType_AllowedTypes_AllowNull_InterfaceMatch()
    {
        var obj = new TestClassType();

        var result = Guard.OfType(obj, new[] { typeof(int), typeof(ITestInterfaceType), typeof(double) }, true);

        Assert.AreSame(obj, result);
    }

    private class TestClassType : ITestInterfaceType
    {
    }

    private class TestDerivedType : TestClassType
    {
    }
}
