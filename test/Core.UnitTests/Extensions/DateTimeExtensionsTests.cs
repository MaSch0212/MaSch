using MaSch.Core.Extensions;

namespace MaSch.Core.UnitTests.Extensions;

[TestClass]
public class DateTimeExtensionsTests : TestClassBase
{
    [TestMethod]
    [DataRow(DateTimeKind.Unspecified, DisplayName = "Unspecified")]
    [DataRow((DateTimeKind)4711, DisplayName = "Unknown enum value")]
    public void ToUniversalTime_InvalidAssumedKind(DateTimeKind assumedKind)
    {
        var dateTime = DateTime.Now;
        Assert.ThrowsException<ArgumentException>(() => DateTimeExtensions.ToUniversalTime(dateTime, assumedKind));
    }

    [TestMethod]
    public void ToUniversalTime_KindUtc()
    {
        var dateTime = new DateTime(2022, 3, 22, 19, 26, 12, DateTimeKind.Utc);
        var utcDateTime = DateTimeExtensions.ToUniversalTime(dateTime, DateTimeKind.Local);
        Assert.AreEqual(dateTime, utcDateTime);
    }

    [TestMethod]
    public void ToUniversalTime_KindLocal()
    {
        var dateTime = new DateTime(2022, 3, 22, 19, 26, 12, DateTimeKind.Local);
        var utcDateTime = DateTimeExtensions.ToUniversalTime(dateTime, DateTimeKind.Utc);
        Assert.AreEqual(dateTime.ToUniversalTime(), utcDateTime);
    }

    [TestMethod]
    public void ToUniversalTime_KindUnspecified_UtcAssumedTime()
    {
        var dateTime = new DateTime(2022, 3, 22, 19, 26, 12, DateTimeKind.Unspecified);
        var utcDateTime = DateTimeExtensions.ToUniversalTime(dateTime, DateTimeKind.Utc);
        Assert.AreEqual(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc), utcDateTime);
    }

    [TestMethod]
    public void ToUniversalTime_KindUnspecified_LocalAssumedTime()
    {
        var dateTime = new DateTime(2022, 3, 22, 19, 26, 12, DateTimeKind.Unspecified);
        var utcDateTime = DateTimeExtensions.ToUniversalTime(dateTime, DateTimeKind.Local);
        Assert.AreEqual(DateTime.SpecifyKind(dateTime, DateTimeKind.Local).ToUniversalTime(), utcDateTime);
    }

    [TestMethod]
    [DataRow(DateTimeKind.Unspecified, DisplayName = "Unspecified")]
    [DataRow((DateTimeKind)4711, DisplayName = "Unknown enum value")]
    public void ToLocalTime_InvalidAssumedKind(DateTimeKind assumedKind)
    {
        var dateTime = DateTime.Now;
        Assert.ThrowsException<ArgumentException>(() => DateTimeExtensions.ToLocalTime(dateTime, assumedKind));
    }

    [TestMethod]
    public void ToLocalTime_KindUtc()
    {
        var dateTime = new DateTime(2022, 3, 22, 19, 26, 12, DateTimeKind.Utc);
        var utcDateTime = DateTimeExtensions.ToLocalTime(dateTime, DateTimeKind.Local);
        Assert.AreEqual(dateTime.ToLocalTime(), utcDateTime);
    }

    [TestMethod]
    public void ToLocalTime_KindLocal()
    {
        var dateTime = new DateTime(2022, 3, 22, 19, 26, 12, DateTimeKind.Local);
        var utcDateTime = DateTimeExtensions.ToLocalTime(dateTime, DateTimeKind.Utc);
        Assert.AreEqual(dateTime, utcDateTime);
    }

    [TestMethod]
    public void ToLocalTime_KindUnspecified_UtcAssumedTime()
    {
        var dateTime = new DateTime(2022, 3, 22, 19, 26, 12, DateTimeKind.Unspecified);
        var utcDateTime = DateTimeExtensions.ToLocalTime(dateTime, DateTimeKind.Utc);
        Assert.AreEqual(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc).ToLocalTime(), utcDateTime);
    }

    [TestMethod]
    public void ToLocalTime_KindUnspecified_LocalAssumedTime()
    {
        var dateTime = new DateTime(2022, 3, 22, 19, 26, 12, DateTimeKind.Unspecified);
        var utcDateTime = DateTimeExtensions.ToLocalTime(dateTime, DateTimeKind.Local);
        Assert.AreEqual(DateTime.SpecifyKind(dateTime, DateTimeKind.Local), utcDateTime);
    }
}
