using MaSch.Presentation.Wpf.Models;
using MaSch.Presentation.Wpf.ThemeValues;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Windows.Media;

namespace MaSch.Presentation.Wpf.Themes.Test.ThemeValues
{
    [TestClass]
    public class ColorThemeValueTests
    {
        private const string ValueJson = "{\"Type\":\"Color\",\"Value\":\"#7C3ECD59\"}";
        private const string RefJson = "{\"Type\":\"Color\",\"Value\":\"{Bind MyTestKey}\"}";

        [TestMethod]
        public void JsonSerialize_Value()
        {
            var value = new ColorThemeValue { RawValue = Color.FromArgb(124, 62, 205, 89) };
            var json = JsonConvert.SerializeObject(value);
            Assert.AreEqual(ValueJson, json, "Wrong Json");
        }

        [TestMethod]
        public void JsonSerialize_Reference()
        {
            var value = new ColorThemeValue { RawValue = new ThemeValueReference("MyTestKey") };
            var json = JsonConvert.SerializeObject(value);
            Assert.AreEqual(RefJson, json, "Wrong Json");
        }

        [TestMethod]
        public void JsonDeserialize_Value()
        {
            var value = JsonConvert.DeserializeObject<ColorThemeValue>(ValueJson);
            Assert.IsInstanceOfType(value.RawValue, typeof(Color));

            var color = (Color)value.RawValue;
            Assert.AreEqual((124, 62, 205, 89), (color.A, color.R, color.G, color.B));
        }

        [TestMethod]
        public void JsonDeserialize_Reference()
        {
            var value = JsonConvert.DeserializeObject<ColorThemeValue>(RefJson);
            Assert.IsInstanceOfType(value.RawValue, typeof(ThemeValueReference));

            var reference = (ThemeValueReference)value.RawValue;
            Assert.AreEqual(("MyTestKey", ""), (reference.CustomKey, reference.Property));
        }

        [TestMethod]
        public void SetRawValue_WrongType()
        {
            var value = new ColorThemeValue();
            Assert.ThrowsException<ArgumentException>(() => value.RawValue = "This should not succeed.");
        }
    }
}
