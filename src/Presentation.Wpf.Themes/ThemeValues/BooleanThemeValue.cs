using MaSch.Common;
using MaSch.Presentation.Wpf.JsonConverters;
using MaSch.Presentation.Wpf.Models;
using Newtonsoft.Json;

namespace MaSch.Presentation.Wpf.ThemeValues
{
    public class BooleanThemeValue : ThemeValueBase<bool>
    {
        #region Properties
        [JsonConverter(typeof(ThemeValuePropertyJsonConverter<bool>))]
        public override object RawValue
        {
            get => base.RawValue;
            set => base.RawValue = Guard.OfType(value, nameof(value), typeof(ThemeValueReference), typeof(bool));
        }
        #endregion

        #region Static Members
        public static BooleanThemeValue Create(bool value) => CreateInternal(value);
        public static BooleanThemeValue Create(ThemeValueReference valueRef) => CreateInternal(valueRef);
        private static BooleanThemeValue CreateInternal(object value)
        {
            return new BooleanThemeValue
            {
                RawValue = value
            };
        }

        public static implicit operator bool(BooleanThemeValue themeValue) => themeValue.Value;
        #endregion
    }
}
