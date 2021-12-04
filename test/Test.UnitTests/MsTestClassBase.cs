namespace MaSch.Test.UnitTests;

public class MsTestClassBase : TestClassBase
{
#if !MSTEST
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public TestContext TestContext { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    [TestInitialize]
    public void Init()
    {
        InitializeTest();
    }

    [TestCleanup]
    public void Cleanup()
    {
        CleanupTest();
    }
#endif
}
