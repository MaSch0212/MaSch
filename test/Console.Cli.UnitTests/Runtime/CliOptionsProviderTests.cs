using MaSch.Console.Cli.Runtime;

namespace MaSch.Console.Cli.UnitTests.Runtime;

[TestClass]
public class CliOptionsProviderTests : TestClassBase
{
    [TestMethod]
    public void GetOptions_Null()
    {
        var sut = new CliOptionsProvider();

        Assert.ThrowsException<InvalidOperationException>(() => sut.GetOptions<object>());
    }

    [TestMethod]
    public void GetOptions_CorrectType()
    {
        var sut = new CliOptionsProvider();
        var po = new PrivateObject(sut);
        po.SetProperty(nameof(CliOptionsProvider.Options), "blub");

        Assert.AreEqual("blub", sut.GetOptions<string>());
    }

    [TestMethod]
    public void GetOptions_WrongType()
    {
        var sut = new CliOptionsProvider();
        var po = new PrivateObject(sut);
        po.SetProperty(nameof(CliOptionsProvider.Options), "blub");

        Assert.ThrowsException<InvalidCastException>(() => sut.GetOptions<CliApplication>());
    }

    [TestMethod]
    public void SetOptions()
    {
        var sut = new CliOptionsProvider();

        var obj = new object();
        sut.SetOptions(obj);

        Assert.AreSame(obj, sut.Options);
    }
}
