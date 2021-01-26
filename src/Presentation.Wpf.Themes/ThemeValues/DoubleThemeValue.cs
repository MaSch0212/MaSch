using MaSch.Core;
using MaSch.Presentation.Wpf.JsonConverters;
using MaSch.Presentation.Wpf.Models;
using Newtonsoft.Json;

namespace MaSch.Presentation.Wpf.ThemeValues
{
    public class DoubleThemeValue : ThemeValueBase<double>
    {
        #region Properties
        [JsonConverter(typeof(ThemeValuePropertyJsonConverter<double>))]
        public override object RawValue
        {
            get => base.RawValue;
            set => base.RawValue = Guard.OfType(value, nameof(value), typeof(ThemeValueReference), typeof(double));
        }
        #endregion

        #region Static Members
        public static DoubleThemeValue Create(double value) => CreateInternal(value);
        public static DoubleThemeValue Create(ThemeValueReference valueRef) => CreateInternal(valueRef);
        private static DoubleThemeValue CreateInternal(object value)
        {
            return new DoubleThemeValue
            {
                RawValue = value
            };
        }

        public static implicit operator double(DoubleThemeValue themeValue) => themeValue.Value;
        #endregion
    }
}
