﻿namespace MaSch.Core.UnitTests;

[TestClass]
public class DelegateEqualityComparerTests : TestClassBase
{
    [TestMethod]
    public void Constructor_ParameterChecks()
    {
        var equalsFuncMock = new Mock<Func<object?, object?, bool>>();
        var getHashCodeFuncMock = new Mock<Func<object, int>>();

        _ = Assert.ThrowsException<ArgumentNullException>(() => new DelegateEqualityComparer<object>(null!, getHashCodeFuncMock.Object));
        _ = Assert.ThrowsException<ArgumentNullException>(() => new DelegateEqualityComparer<object>(equalsFuncMock.Object, null!));
    }

    [TestMethod]
    public void Equals()
    {
        var obj1 = new object();
        var obj2 = new object();
        var equalsFuncMock = new Mock<Func<object?, object?, bool>>();
        var getHashCodeFuncMock = new Mock<Func<object, int>>();
        _ = equalsFuncMock.Setup(x => x(It.IsAny<object?>(), It.IsAny<object?>())).Returns(true);

        var comparer = new DelegateEqualityComparer<object>(equalsFuncMock.Object, getHashCodeFuncMock.Object);
        var result = comparer.Equals(obj1, obj2);

        Assert.IsTrue(result);
        equalsFuncMock.Verify(x => x(obj1, obj2), Times.Once());
        getHashCodeFuncMock.Verify(x => x(It.IsAny<object>()), Times.Never());
    }

    [TestMethod]
    public void GetHashCode_()
    {
        var obj1 = new object();
        var equalsFuncMock = new Mock<Func<object?, object?, bool>>();
        var getHashCodeFuncMock = new Mock<Func<object, int>>();
        _ = getHashCodeFuncMock.Setup(x => x(It.IsAny<object>())).Returns(4711);

        var comparer = new DelegateEqualityComparer<object>(equalsFuncMock.Object, getHashCodeFuncMock.Object);
        var result = comparer.GetHashCode(obj1);

        Assert.AreEqual(4711, result);
        getHashCodeFuncMock.Verify(x => x(obj1), Times.Once());
        equalsFuncMock.Verify(x => x(It.IsAny<object?>(), It.IsAny<object?>()), Times.Never());
    }
}
