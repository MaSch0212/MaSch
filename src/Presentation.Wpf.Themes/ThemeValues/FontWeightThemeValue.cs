using System.Windows;
using MaSch.Core;
using MaSch.Presentation.Wpf.JsonConverters;
using MaSch.Presentation.Wpf.Models;
using Newtonsoft.Json;

namespace MaSch.Presentation.Wpf.ThemeValues
{
    public class FontWeightThemeValue : ThemeValueBase<FontWeight>
    {
        #region Properties
        [JsonConverter(typeof(ThemeValuePropertyJsonConverter<FontWeight>))]
        public override object RawValue
        {
            get => base.RawValue;
            set => base.RawValue = Guard.OfType(value, nameof(value), typeof(ThemeValueReference), typeof(FontWeight));
        }
        #endregion

        #region Static Members
        public static FontWeightThemeValue Create(FontWeight value) => CreateInternal(value);
        public static FontWeightThemeValue Create(ThemeValueReference valueRef) => CreateInternal(valueRef);
        private static FontWeightThemeValue CreateInternal(object value)
        {
            return new FontWeightThemeValue
            {
                RawValue = value
            };
        }

        public static implicit operator FontWeight(FontWeightThemeValue themeValue) => themeValue.Value;
        #endregion
    }
}
