using System.Windows.Media;
using MaSch.Core;
using MaSch.Presentation.Wpf.JsonConverters;
using MaSch.Presentation.Wpf.Models;
using Newtonsoft.Json;

#nullable disable

namespace MaSch.Presentation.Wpf.ThemeValues
{
    /// <summary>
    /// <see cref="IThemeValue"/> representing <see cref="Color"/> values.
    /// </summary>
    /// <seealso cref="MaSch.Presentation.Wpf.ThemeValues.ThemeValueBase{T}" />
    public class ColorThemeValue : ThemeValueBase<Color>
    {
        /// <inheritdoc/>
        [JsonConverter(typeof(ThemeValuePropertyJsonConverter<Color>))]
        public override object RawValue
        {
            get => base.RawValue;
            set => base.RawValue = Guard.OfType(value, nameof(value), typeof(ThemeValueReference), typeof(Color));
        }

        /// <summary>
        /// Creates a new <see cref="ColorThemeValue"/>.
        /// </summary>
        /// <param name="value">The value to use.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static ColorThemeValue Create(Color value) => CreateInternal(value);

        /// <summary>
        /// Creates a new <see cref="ColorThemeValue"/>.
        /// </summary>
        /// <param name="valueRef">The value reference.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static ColorThemeValue Create(ThemeValueReference valueRef) => CreateInternal(valueRef);

        private static ColorThemeValue CreateInternal(object value)
        {
            return new ColorThemeValue
            {
                RawValue = value,
            };
        }

        public static implicit operator Color(ColorThemeValue themeValue) => themeValue.Value;
    }
}
