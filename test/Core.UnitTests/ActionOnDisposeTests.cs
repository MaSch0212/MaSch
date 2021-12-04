namespace MaSch.Core.UnitTests;

[TestClass]
public class ActionOnDisposeTests : TestClassBase
{
    [TestMethod]
    public void Constructor_NullChecks()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() => new ActionOnDispose((Action)null!).Dispose());
        Assert.IsNotNull(new ActionOnDispose(() => { }));

        _ = Assert.ThrowsException<ArgumentNullException>(() => new ActionOnDispose((Action<TimeSpan>)null!).Dispose());
        Assert.IsNotNull(new ActionOnDispose((TimeSpan t) => { }));
    }

    [TestMethod]
    [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP017:Prefer using.", Justification = "Explicitly calling Dispose makes more sense in this test")]
    public void Dispose_WithoutTime()
    {
        int callCount = 0;

        var obj = new ActionOnDispose(() => Interlocked.Increment(ref callCount));

        Assert.AreEqual(0, callCount);
        obj.Dispose();
        Assert.AreEqual(1, callCount);
    }

    [TestMethod]
    [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP017:Prefer using.", Justification = "Explicitly calling Dispose makes more sense in this test")]
    public void Dispose_WithTime()
    {
        int callCount = 0;
        TimeSpan? lastTimeSpan = null;

        var sw = new Stopwatch();
        sw.Start();
        var obj = new ActionOnDispose(t =>
        {
            _ = Interlocked.Increment(ref callCount);
            lastTimeSpan = t;
        });

        Assert.AreEqual(0, callCount);
        Thread.Sleep(100);
        obj.Dispose();
        sw.Stop();
        Assert.AreEqual(1, callCount);
        Assert.IsBetween(sw.Elapsed.Add(TimeSpan.FromMilliseconds(-50)), sw.Elapsed.Add(TimeSpan.FromMilliseconds(50)), lastTimeSpan!.Value);
    }
}
