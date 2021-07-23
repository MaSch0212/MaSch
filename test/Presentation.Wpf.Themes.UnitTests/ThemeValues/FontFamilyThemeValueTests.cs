using MaSch.Presentation.Wpf.Models;
using MaSch.Presentation.Wpf.ThemeValues;
using MaSch.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Windows.Media;

namespace MaSch.Presentation.Wpf.Themes.UnitTests.ThemeValues
{
    [TestClass]
    public class FontFamilyThemeValueTests : TestClassBase
    {
        private const string ValueJson = "{\"Type\":\"FontFamily\",\"Value\":\"Arial\"}";
        private const string RefJson = "{\"Type\":\"FontFamily\",\"Value\":\"{Bind MyTestKey}\"}";

        [TestMethod]
        public void JsonSerialize_Value()
        {
            var value = new FontFamilyThemeValue { RawValue = new FontFamily("Arial") };
            var json = JsonConvert.SerializeObject(value);
            Assert.AreEqual(ValueJson, json, "Wrong Json");
        }

        [TestMethod]
        public void JsonSerialize_Reference()
        {
            var value = new FontFamilyThemeValue { RawValue = new ThemeValueReference("MyTestKey") };
            var json = JsonConvert.SerializeObject(value);
            Assert.AreEqual(RefJson, json, "Wrong Json");
        }

        [TestMethod]
        public void JsonDeserialize_Value()
        {
            var value = JsonConvert.DeserializeObject<FontFamilyThemeValue>(ValueJson);
            Assert.IsInstanceOfType(value.RawValue, typeof(FontFamily));

            var ff = (FontFamily)value.RawValue;
            Assert.AreEqual("Arial", ff.Source);
        }

        [TestMethod]
        public void JsonDeserialize_Reference()
        {
            var value = JsonConvert.DeserializeObject<FontFamilyThemeValue>(RefJson);
            Assert.IsInstanceOfType(value.RawValue, typeof(ThemeValueReference));

            var reference = (ThemeValueReference)value.RawValue;
            Assert.AreEqual(("MyTestKey", string.Empty), (reference.CustomKey, reference.Property));
        }

        [TestMethod]
        public void SetRawValue_WrongType()
        {
            var value = new FontFamilyThemeValue();
            Assert.ThrowsException<ArgumentException>(() => value.RawValue = "This should not succeed.");
        }
    }
}
