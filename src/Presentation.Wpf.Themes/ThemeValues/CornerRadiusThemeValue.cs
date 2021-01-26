using System.Windows;
using MaSch.Core;
using MaSch.Presentation.Wpf.JsonConverters;
using MaSch.Presentation.Wpf.Models;
using Newtonsoft.Json;

namespace MaSch.Presentation.Wpf.ThemeValues
{
    public class CornerRadiusThemeValue : ThemeValueBase<CornerRadius>
    {
        #region Properties
        [JsonConverter(typeof(ThemeValuePropertyJsonConverter<CornerRadius>))]
        public override object RawValue
        {
            get => base.RawValue;
            set => base.RawValue = Guard.OfType(value, nameof(value), typeof(ThemeValueReference), typeof(CornerRadius));
        }
        #endregion

        #region Static Members
        public static CornerRadiusThemeValue Create(CornerRadius value) => CreateInternal(value);
        public static CornerRadiusThemeValue Create(ThemeValueReference valueRef) => CreateInternal(valueRef);
        private static CornerRadiusThemeValue CreateInternal(object value)
        {
            return new CornerRadiusThemeValue
            {
                RawValue = value
            };
        }

        public static implicit operator CornerRadius(CornerRadiusThemeValue themeValue) => themeValue.Value;
        #endregion
    }
}
