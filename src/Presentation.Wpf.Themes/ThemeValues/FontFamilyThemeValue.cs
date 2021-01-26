using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using MaSch.Common;
using MaSch.Presentation.Wpf.JsonConverters;
using MaSch.Presentation.Wpf.Models;
using Newtonsoft.Json;

namespace MaSch.Presentation.Wpf.ThemeValues
{
    public class FontFamilyThemeValue : ThemeValueBase<FontFamily>
    {
        #region Properties
        [JsonConverter(typeof(ThemeValuePropertyJsonConverter<FontFamily>))]
        public override object RawValue
        {
            get => base.RawValue;
            set => base.RawValue = Guard.OfType(value, nameof(value), typeof(ThemeValueReference), typeof(FontFamily));
        }
        #endregion

        #region Static Members
        public static FontFamilyThemeValue Create(FontFamily value) => CreateInternal(value);
        public static FontFamilyThemeValue Create(ThemeValueReference valueRef) => CreateInternal(valueRef);
        private static FontFamilyThemeValue CreateInternal(object value)
        {
            return new FontFamilyThemeValue
            {
                RawValue = value
            };
        }

        public static implicit operator FontFamily(FontFamilyThemeValue themeValue) => themeValue.Value;
        #endregion

        public override bool Equals(object obj) => obj is FontFamilyThemeValue other && Equals(other.RawValue.GetHashCode(), RawValue.GetHashCode());
        public override int GetHashCode() => RawValue.GetHashCode();
    }
}
