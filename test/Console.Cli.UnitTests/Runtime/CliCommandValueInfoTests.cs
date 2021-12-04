using MaSch.Console.Cli.Configuration;
using MaSch.Console.Cli.Runtime;
using MaSch.Core;

namespace MaSch.Console.Cli.UnitTests.Runtime;

[TestClass]
public class CliCommandValueInfoTests : TestClassBase
{
    [TestMethod]
    public void Ctor_NullChecks()
    {
        var command = Mocks.Create<ICliCommandInfo>();
        var property = typeof(DummyClass).GetProperty(nameof(DummyClass.NormalProperty), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        var extensionStorage = new ObjectExtensionDataStorage();

        _ = Assert.ThrowsException<ArgumentNullException>(() => new CliCommandValueInfo(extensionStorage, command.Object, property!, null!));
    }

    [TestMethod]
    public void Ctor()
    {
        var command = Mocks.Create<ICliCommandInfo>();
        var property = typeof(DummyClass).GetProperty(nameof(DummyClass.NormalProperty), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)!;
        var attr = new CliCommandValueAttribute(0, "blub");
        var extensionStorage = new ObjectExtensionDataStorage();

        var option = new CliCommandValueInfo(extensionStorage, command.Object, property, attr);

        Assert.AreSame(attr, option.Attribute);
    }

    [TestMethod]
    public void GetterProperties()
    {
        var command = Mocks.Create<ICliCommandInfo>();
        var property = typeof(DummyClass).GetProperty(nameof(DummyClass.NormalProperty), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)!;
        var attr = new CliCommandValueAttribute(4711, "blub")
        {
            Default = new object(),
            Required = true,
            HelpText = "My Help Text",
            Hidden = true,
        };
        var extensionStorage = new ObjectExtensionDataStorage();

        var option = new CliCommandValueInfo(extensionStorage, command.Object, property, attr);

        Assert.AreSame(attr.Default, option.DefaultValue);
        Assert.IsTrue(option.IsRequired);
        Assert.AreEqual("My Help Text", option.HelpText);
        Assert.AreEqual(4711, option.Order);
        Assert.IsTrue(option.Hidden);
    }

    [ExcludeFromCodeCoverage]
    private class DummyClass
    {
        public int NormalProperty { get; set; }
    }
}
