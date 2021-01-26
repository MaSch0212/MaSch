using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MaSch.Core;
using MaSch.Presentation.Wpf.JsonConverters;
using MaSch.Presentation.Wpf.Models;
using Newtonsoft.Json;

namespace MaSch.Presentation.Wpf.ThemeValues
{
    public class FontStyleThemeValue : ThemeValueBase<FontStyle>
    {
        #region Properties
        [JsonConverter(typeof(ThemeValuePropertyJsonConverter<FontStyle>))]
        public override object RawValue
        {
            get => base.RawValue;
            set => base.RawValue = Guard.OfType(value, nameof(value), typeof(ThemeValueReference), typeof(FontStyle));
        }
        #endregion

        #region Static Members
        public static FontStyleThemeValue Create(FontStyle value) => CreateInternal(value);
        public static FontStyleThemeValue Create(ThemeValueReference valueRef) => CreateInternal(valueRef);
        private static FontStyleThemeValue CreateInternal(object value)
        {
            return new FontStyleThemeValue
            {
                RawValue = value
            };
        }

        public static implicit operator FontStyle(FontStyleThemeValue themeValue) => themeValue.Value;
        #endregion
    }
}
