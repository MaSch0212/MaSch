using MaSch.Presentation.Wpf.Models;
using MaSch.Presentation.Wpf.ThemeValues;
using MaSch.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Windows;

namespace MaSch.Presentation.Wpf.Themes.Test.ThemeValues
{
    [TestClass]
    public class FontStyleThemeValueTests : TestClassBase
    {
        private const string ValueJson = "{\"Type\":\"FontStyle\",\"Value\":\"Italic\"}";
        private const string RefJson = "{\"Type\":\"FontStyle\",\"Value\":\"{Bind MyTestKey}\"}";

        [TestMethod]
        public void JsonSerialize_Value()
        {
            var value = new FontStyleThemeValue { RawValue = FontStyles.Italic };
            var json = JsonConvert.SerializeObject(value);
            Assert.AreEqual(ValueJson, json, "Wrong Json");
        }

        [TestMethod]
        public void JsonSerialize_Reference()
        {
            var value = new FontStyleThemeValue { RawValue = new ThemeValueReference("MyTestKey") };
            var json = JsonConvert.SerializeObject(value);
            Assert.AreEqual(RefJson, json, "Wrong Json");
        }

        [TestMethod]
        public void JsonDeserialize_Value()
        {
            var value = JsonConvert.DeserializeObject<FontStyleThemeValue>(ValueJson);
            Assert.IsInstanceOfType(value.RawValue, typeof(FontStyle));

            var fs = (FontStyle)value.RawValue;
            Assert.AreEqual("Italic", fs.ToString());
        }

        [TestMethod]
        public void JsonDeserialize_Reference()
        {
            var value = JsonConvert.DeserializeObject<FontStyleThemeValue>(RefJson);
            Assert.IsInstanceOfType(value.RawValue, typeof(ThemeValueReference));

            var reference = (ThemeValueReference)value.RawValue;
            Assert.AreEqual(("MyTestKey", string.Empty), (reference.CustomKey, reference.Property));
        }

        [TestMethod]
        public void SetRawValue_WrongType()
        {
            var value = new FontStyleThemeValue();
            Assert.ThrowsException<ArgumentException>(() => value.RawValue = "This should not succeed.");
        }
    }
}
