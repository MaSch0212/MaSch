using MaSch.Presentation.Wpf.Models;
using MaSch.Presentation.Wpf.ThemeValues;
using MaSch.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;
using System.Windows.Media;

namespace MaSch.Presentation.Wpf.Themes.Test.ThemeValues
{
    [TestClass]
    public class IconThemeValueTests : UnitTestBase
    {
        private const string ValueJson = "{\"Type\":\"Icon\",\"IconType\":\"Geometry\",\"Character\":\"A\",\"Font\":\"Arial\",\"FontSize\":4.74,\"Geometry\":\"M0,0L1,0 1,1 0,1z\",\"IsGeometryFilled\":false,\"GeometryStrokeThickness\":2.4,\"Stretch\":\"Fill\"}";
        private const string RefJson = "{\"Type\":\"Icon\",\"IconType\":\"{Bind R1}\",\"Character\":\"{Bind R2}\",\"Font\":\"{Bind R3}\",\"FontSize\":\"{Bind R4}\",\"Geometry\":\"{Bind R5}\",\"IsGeometryFilled\":\"{Bind R6}\",\"GeometryStrokeThickness\":\"{Bind R7}\",\"Stretch\":\"{Bind R8}\"}";

        [TestMethod]
        public void JsonSerialize_Value()
        {
            var value = new IconThemeValue
            {
                RawIconType = SymbolType.Geometry,
                RawCharacter = "A",
                RawFont = new FontFamily("Arial"),
                RawFontSize = 4.74,
                RawGeometry = Geometry.Parse("M0,0L1,0 1,1 0,1z"),
                RawIsGeometryFilled = false,
                RawGeometryStrokeThickness = 2.4,
                RawStretch = Stretch.Fill,
            };
            var jsonSerializer = new JsonSerializerSettings();
            jsonSerializer.Converters.Add(new StringEnumConverter());
            var json = JsonConvert.SerializeObject(value, jsonSerializer);
            Assert.AreEqual(ValueJson, json, "Wrong Json");
        }

        [TestMethod]
        public void JsonSerialize_Reference()
        {
            var value = new IconThemeValue
            {
                RawIconType = new ThemeValueReference("R1"),
                RawCharacter = new ThemeValueReference("R2"),
                RawFont = new ThemeValueReference("R3"),
                RawFontSize = new ThemeValueReference("R4"),
                RawGeometry = new ThemeValueReference("R5"),
                RawIsGeometryFilled = new ThemeValueReference("R6"),
                RawGeometryStrokeThickness = new ThemeValueReference("R7"),
                RawStretch = new ThemeValueReference("R8"),
            };
            var json = JsonConvert.SerializeObject(value);
            Assert.AreEqual(RefJson, json, "Wrong Json");
        }

        [TestMethod]
        public void JsonDeserialize_Value()
        {
            var value = JsonConvert.DeserializeObject<IconThemeValue>(ValueJson);
            Assert.IsInstanceOfType(value.RawIconType, typeof(SymbolType));
            Assert.IsInstanceOfType(value.RawCharacter, typeof(string));
            Assert.IsInstanceOfType(value.RawFont, typeof(FontFamily));
            Assert.IsInstanceOfType(value.RawFontSize, typeof(double));
            Assert.IsInstanceOfType(value.RawGeometry, typeof(Geometry));
            Assert.IsInstanceOfType(value.RawIsGeometryFilled, typeof(bool));
            Assert.IsInstanceOfType(value.RawGeometryStrokeThickness, typeof(double));
            Assert.IsInstanceOfType(value.RawStretch, typeof(Stretch));

            Assert.AreEqual(SymbolType.Geometry, value.RawIconType);
            Assert.AreEqual("A", value.RawCharacter);
            Assert.AreEqual("Arial", ((FontFamily)value.RawFont).Source);
            Assert.AreEqual(4.74, value.FontSize);
            Assert.AreEqual("M0,0L1,0 1,1 0,1z", ((Geometry)value.RawGeometry).ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(false, value.RawIsGeometryFilled);
            Assert.AreEqual(2.4, value.RawGeometryStrokeThickness);
            Assert.AreEqual(Stretch.Fill, value.RawStretch);
        }

        [TestMethod]
        public void JsonDeserialize_Reference()
        {
            var value = JsonConvert.DeserializeObject<IconThemeValue>(RefJson);
            Assert.IsInstanceOfType(value.RawIconType, typeof(ThemeValueReference));
            Assert.IsInstanceOfType(value.RawCharacter, typeof(ThemeValueReference));
            Assert.IsInstanceOfType(value.RawFont, typeof(ThemeValueReference));
            Assert.IsInstanceOfType(value.RawFontSize, typeof(ThemeValueReference));
            Assert.IsInstanceOfType(value.RawGeometry, typeof(ThemeValueReference));
            Assert.IsInstanceOfType(value.RawIsGeometryFilled, typeof(ThemeValueReference));
            Assert.IsInstanceOfType(value.RawGeometryStrokeThickness, typeof(ThemeValueReference));
            Assert.IsInstanceOfType(value.RawStretch, typeof(ThemeValueReference));

            AssertReference("R1", value.RawIconType);
            AssertReference("R2", value.RawCharacter);
            AssertReference("R3", value.RawFont);
            AssertReference("R4", value.RawFontSize);
            AssertReference("R5", value.RawGeometry);
            AssertReference("R6", value.RawIsGeometryFilled);
            AssertReference("R7", value.RawGeometryStrokeThickness);
            AssertReference("R8", value.RawStretch);
        }

        [TestMethod]
        public void SetRawValue_WrongType()
        {
            var value = new IconThemeValue();
            Assert.ThrowsException<ArgumentException>(() => value.RawValue = "This should not succeed.");
        }

        private void AssertReference(string keyName, object reference)
        {
            var @ref = (ThemeValueReference)reference;
            Assert.AreEqual((keyName, string.Empty), (@ref.CustomKey, @ref.Property));
        }
    }
}
