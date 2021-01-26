using System.Windows.Media;
using MaSch.Core;
using MaSch.Presentation.Wpf.JsonConverters;
using MaSch.Presentation.Wpf.Models;
using Newtonsoft.Json;

namespace MaSch.Presentation.Wpf.ThemeValues
{
    public class ColorThemeValue : ThemeValueBase<Color>
    {
        #region Properties
        [JsonConverter(typeof(ThemeValuePropertyJsonConverter<Color>))]
        public override object RawValue
        {
            get => base.RawValue;
            set => base.RawValue = Guard.OfType(value, nameof(value), typeof(ThemeValueReference), typeof(Color));
        }
        #endregion

        #region Static Members
        public static ColorThemeValue Create(Color value) => CreateInternal(value);
        public static ColorThemeValue Create(ThemeValueReference valueRef) => CreateInternal(valueRef);
        private static ColorThemeValue CreateInternal(object value)
        {
            return new ColorThemeValue
            {
                RawValue = value
            };
        }

        public static implicit operator Color(ColorThemeValue themeValue) => themeValue.Value;
        #endregion
    }
}
