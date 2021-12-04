using MaSch.Presentation.Wpf.Models;
using MaSch.Presentation.Wpf.ThemeValues;
using System.Windows;

namespace MaSch.Presentation.Wpf.Themes.UnitTests.ThemeValues;

[TestClass]
public class CornerRadiusThemeValueTests : TestClassBase
{
    private const string ValueJson = "{\"Type\":\"CornerRadius\",\"Value\":\"1,2,3,4\"}";
    private const string RefJson = "{\"Type\":\"CornerRadius\",\"Value\":\"{Bind MyTestKey}\"}";

    [TestMethod]
    public void JsonSerialize_Value()
    {
        var value = new CornerRadiusThemeValue { RawValue = new CornerRadius(1, 2, 3, 4) };
        var json = JsonConvert.SerializeObject(value);
        Assert.AreEqual(ValueJson, json, "Wrong Json");
    }

    [TestMethod]
    public void JsonSerialize_Reference()
    {
        var value = new CornerRadiusThemeValue { RawValue = new ThemeValueReference("MyTestKey") };
        var json = JsonConvert.SerializeObject(value);
        Assert.AreEqual(RefJson, json, "Wrong Json");
    }

    [TestMethod]
    public void JsonDeserialize_Value()
    {
        var value = JsonConvert.DeserializeObject<CornerRadiusThemeValue>(ValueJson);
        Assert.IsInstanceOfType(value.RawValue, typeof(CornerRadius));

        var cr = (CornerRadius)value.RawValue;
        Assert.AreEqual((1, 2, 3, 4), (cr.TopLeft, cr.TopRight, cr.BottomRight, cr.BottomLeft));
    }

    [TestMethod]
    public void JsonDeserialize_Reference()
    {
        var value = JsonConvert.DeserializeObject<CornerRadiusThemeValue>(RefJson);
        Assert.IsInstanceOfType(value.RawValue, typeof(ThemeValueReference));

        var reference = (ThemeValueReference)value.RawValue;
        Assert.AreEqual(("MyTestKey", string.Empty), (reference.CustomKey, reference.Property));
    }

    [TestMethod]
    public void SetRawValue_WrongType()
    {
        var value = new CornerRadiusThemeValue();
        _ = Assert.ThrowsException<ArgumentException>(() => value.RawValue = "This should not succeed.");
    }
}
