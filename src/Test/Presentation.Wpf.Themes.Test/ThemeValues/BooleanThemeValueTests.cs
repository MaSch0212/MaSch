using MaSch.Presentation.Wpf.Models;
using MaSch.Presentation.Wpf.ThemeValues;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;

namespace MaSch.Presentation.Wpf.Themes.Test.ThemeValues
{
    [TestClass]
    public class BooleanThemeValueTests
    {
        private const string TrueJson = "{\"Type\":\"Boolean\",\"Value\":true}";
        private const string FalseJson = "{\"Type\":\"Boolean\",\"Value\":false}";
        private const string RefJson = "{\"Type\":\"Boolean\",\"Value\":\"{Bind MyTestKey}\"}";

        [TestMethod]
        public void JsonSerialize_True()
        {
            var value = new BooleanThemeValue { RawValue = true };
            var json = JsonConvert.SerializeObject(value);
            Assert.AreEqual(TrueJson, json, "Wrong Json");
        }

        [TestMethod]
        public void JsonSerialize_False()
        {
            var value = new BooleanThemeValue { RawValue = false };
            var json = JsonConvert.SerializeObject(value);
            Assert.AreEqual(FalseJson, json, "Wrong Json");
        }

        [TestMethod]
        public void JsonSerialize_Reference()
        {
            var value = new BooleanThemeValue { RawValue = new ThemeValueReference("MyTestKey") };
            var json = JsonConvert.SerializeObject(value);
            Assert.AreEqual(RefJson, json, "Wrong Json");
        }

        [TestMethod]
        public void JsonDeserialize_True()
        {
            var value = JsonConvert.DeserializeObject<BooleanThemeValue>(TrueJson);
            Assert.AreEqual(true, value.RawValue);
        }

        [TestMethod]
        public void JsonDeserialize_False()
        {
            var value = JsonConvert.DeserializeObject<BooleanThemeValue>(FalseJson);
            Assert.AreEqual(false, value.RawValue);
        }

        [TestMethod]
        public void JsonDeserialize_Reference()
        {
            var value = JsonConvert.DeserializeObject<BooleanThemeValue>(RefJson);
            Assert.IsInstanceOfType(value.RawValue, typeof(ThemeValueReference));

            var reference = (ThemeValueReference)value.RawValue;
            Assert.AreEqual(("MyTestKey", ""), (reference.CustomKey, reference.Property));
        }

        [TestMethod]
        public void SetRawValue_WrongType()
        {
            var value = new BooleanThemeValue();
            Assert.ThrowsException<ArgumentException>(() => value.RawValue = "This should not succeed.");
        }
    }
}
