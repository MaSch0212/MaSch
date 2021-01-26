using System.Windows;
using MaSch.Core;
using MaSch.Presentation.Wpf.JsonConverters;
using MaSch.Presentation.Wpf.Models;
using Newtonsoft.Json;

namespace MaSch.Presentation.Wpf.ThemeValues
{
    public class ThicknessThemeValue : ThemeValueBase<Thickness>
    {
        #region Properties
        [JsonConverter(typeof(ThemeValuePropertyJsonConverter<Thickness>))]
        public override object RawValue
        {
            get => base.RawValue;
            set => base.RawValue = Guard.OfType(value, nameof(value), typeof(ThemeValueReference), typeof(Thickness));
        }
        #endregion

        #region Static Members
        public static ThicknessThemeValue Create(Thickness value) => CreateInternal(value);
        public static ThicknessThemeValue Create(ThemeValueReference valueRef) => CreateInternal(valueRef);
        private static ThicknessThemeValue CreateInternal(object value)
        {
            return new ThicknessThemeValue
            {
                RawValue = value
            };
        }

        public static implicit operator Thickness(ThicknessThemeValue themeValue) => themeValue.Value;
        #endregion
    }
}
