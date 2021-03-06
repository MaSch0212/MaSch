using System.Windows;
using MaSch.Core;
using MaSch.Presentation.Wpf.JsonConverters;
using MaSch.Presentation.Wpf.Models;
using Newtonsoft.Json;

#nullable disable

namespace MaSch.Presentation.Wpf.ThemeValues
{
    /// <summary>
    /// <see cref="IThemeValue"/> representing <see cref="FontStyle"/> values.
    /// </summary>
    /// <seealso cref="MaSch.Presentation.Wpf.ThemeValues.ThemeValueBase{T}" />
    public class FontStyleThemeValue : ThemeValueBase<FontStyle>
    {
        /// <inheritdoc/>
        [JsonConverter(typeof(ThemeValuePropertyJsonConverter<FontStyle>))]
        public override object RawValue
        {
            get => base.RawValue;
            set => base.RawValue = Guard.OfType(value, nameof(value), typeof(ThemeValueReference), typeof(FontStyle));
        }

        /// <summary>
        /// Creates a new <see cref="FontStyleThemeValue"/>.
        /// </summary>
        /// <param name="value">The value to use.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static FontStyleThemeValue Create(FontStyle value) => CreateInternal(value);

        /// <summary>
        /// Creates a new <see cref="FontStyleThemeValue"/>.
        /// </summary>
        /// <param name="valueRef">The value reference.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static FontStyleThemeValue Create(ThemeValueReference valueRef) => CreateInternal(valueRef);

        private static FontStyleThemeValue CreateInternal(object value)
        {
            return new FontStyleThemeValue
            {
                RawValue = value,
            };
        }

        public static implicit operator FontStyle(FontStyleThemeValue themeValue) => themeValue.Value;
    }
}
