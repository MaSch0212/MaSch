using MaSch.Console.Cli.Runtime;

namespace MaSch.Console.Cli.UnitTests.Runtime;

[TestClass]
public class CliExecutionContextTests : TestClassBase
{
    [TestMethod]
    public void Ctor_NullChecks()
    {
        var serviceProviderMock = Mocks.Create<IServiceProvider>();
        var commandMock = Mocks.Create<ICliCommandInfo>();

        _ = Assert.ThrowsException<ArgumentNullException>(() => new CliExecutionContext(null!, commandMock.Object));
        _ = Assert.ThrowsException<ArgumentNullException>(() => new CliExecutionContext(serviceProviderMock.Object, null!));
    }

    [TestMethod]
    public void Ctor()
    {
        var serviceProviderMock = Mocks.Create<IServiceProvider>();
        var commandMock = Mocks.Create<ICliCommandInfo>();

        var ctx = new CliExecutionContext(serviceProviderMock.Object, commandMock.Object);

        Assert.AreSame(serviceProviderMock.Object, ctx.ServiceProvider);
        Assert.AreSame(commandMock.Object, ctx.Command);
    }
}
