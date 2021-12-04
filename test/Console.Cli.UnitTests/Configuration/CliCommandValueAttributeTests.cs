using MaSch.Console.Cli.Configuration;

namespace MaSch.Console.Cli.UnitTests.Configuration;

[TestClass]
public class CliCommandValueAttributeTests : TestClassBase
{
    [TestMethod]
    public void Ctor_Order_DisplayName_Null()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() => new CliCommandValueAttribute(0, null!));
    }

    [TestMethod]
    public void Ctor_Order_DisplayName_Empty()
    {
        _ = Assert.ThrowsException<ArgumentException>(() => new CliCommandValueAttribute(0, string.Empty));
    }

    [TestMethod]
    public void Ctor_Order_DisplayName_Valid()
    {
        var attr = new CliCommandValueAttribute(4711, "blub");

        Assert.AreEqual(4711, attr.Order);
        Assert.AreEqual("blub", attr.DisplayName);
    }

    [TestMethod]
    public void DefaultValues()
    {
        var attr = new CliCommandValueAttribute(0, "blub");

        Assert.IsNull(attr.Default);
        Assert.IsFalse(attr.Required);
        Assert.IsNull(attr.HelpText);
        Assert.IsFalse(attr.Hidden);
    }
}
