using MaSch.Presentation.Wpf.Models;
using MaSch.Presentation.Wpf.ThemeValues;
using MaSch.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;

namespace MaSch.Presentation.Wpf.Themes.Test.ThemeValues
{
    [TestClass]
    public class DoubleThemeValueTests : TestClassBase
    {
        private const string ValueJson = "{\"Type\":\"Double\",\"Value\":5.42}";
        private const string RefJson = "{\"Type\":\"Double\",\"Value\":\"{Bind MyTestKey}\"}";

        [TestMethod]
        public void JsonSerialize_Value()
        {
            var value = new DoubleThemeValue { RawValue = 5.42 };
            var json = JsonConvert.SerializeObject(value);
            Assert.AreEqual(ValueJson, json, "Wrong Json");
        }

        [TestMethod]
        public void JsonSerialize_Reference()
        {
            var value = new DoubleThemeValue { RawValue = new ThemeValueReference("MyTestKey") };
            var json = JsonConvert.SerializeObject(value);
            Assert.AreEqual(RefJson, json, "Wrong Json");
        }

        [TestMethod]
        public void JsonDeserialize_Value()
        {
            var value = JsonConvert.DeserializeObject<DoubleThemeValue>(ValueJson);
            Assert.IsInstanceOfType(value.RawValue, typeof(double));

            Assert.AreEqual(5.42, value.RawValue);
        }

        [TestMethod]
        public void JsonDeserialize_Reference()
        {
            var value = JsonConvert.DeserializeObject<DoubleThemeValue>(RefJson);
            Assert.IsInstanceOfType(value.RawValue, typeof(ThemeValueReference));

            var reference = (ThemeValueReference)value.RawValue;
            Assert.AreEqual(("MyTestKey", string.Empty), (reference.CustomKey, reference.Property));
        }

        [TestMethod]
        public void SetRawValue_WrongType()
        {
            var value = new DoubleThemeValue();
            Assert.ThrowsException<ArgumentException>(() => value.RawValue = "This should not succeed.");
        }
    }
}
