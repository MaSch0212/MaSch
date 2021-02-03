using System.Windows;
using MaSch.Core;
using MaSch.Presentation.Wpf.JsonConverters;
using MaSch.Presentation.Wpf.Models;
using Newtonsoft.Json;

namespace MaSch.Presentation.Wpf.ThemeValues
{
    /// <summary>
    /// <see cref="IThemeValue"/> representing <see cref="Thickness"/> values.
    /// </summary>
    /// <seealso cref="MaSch.Presentation.Wpf.ThemeValues.ThemeValueBase{T}" />
    public class ThicknessThemeValue : ThemeValueBase<Thickness>
    {
        /// <inheritdoc/>
        [JsonConverter(typeof(ThemeValuePropertyJsonConverter<Thickness>))]
        public override object RawValue
        {
            get => base.RawValue;
            set => base.RawValue = Guard.OfType(value, nameof(value), typeof(ThemeValueReference), typeof(Thickness));
        }

        /// <summary>
        /// Creates a new <see cref="ThicknessThemeValue"/>.
        /// </summary>
        /// <param name="value">The value to use.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static ThicknessThemeValue Create(Thickness value) => CreateInternal(value);

        /// <summary>
        /// Creates a new <see cref="ThicknessThemeValue"/>.
        /// </summary>
        /// <param name="valueRef">The value reference.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static ThicknessThemeValue Create(ThemeValueReference valueRef) => CreateInternal(valueRef);

        private static ThicknessThemeValue CreateInternal(object value)
        {
            return new ThicknessThemeValue
            {
                RawValue = value,
            };
        }

        public static implicit operator Thickness(ThicknessThemeValue themeValue) => themeValue.Value;
    }
}
