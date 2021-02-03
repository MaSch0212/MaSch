using System.Windows;
using MaSch.Core;
using MaSch.Presentation.Wpf.JsonConverters;
using MaSch.Presentation.Wpf.Models;
using Newtonsoft.Json;

namespace MaSch.Presentation.Wpf.ThemeValues
{
    /// <summary>
    /// <see cref="IThemeValue"/> representing <see cref="CornerRadius"/> values.
    /// </summary>
    /// <seealso cref="MaSch.Presentation.Wpf.ThemeValues.ThemeValueBase{T}" />
    public class CornerRadiusThemeValue : ThemeValueBase<CornerRadius>
    {
        /// <inheritdoc/>
        [JsonConverter(typeof(ThemeValuePropertyJsonConverter<CornerRadius>))]
        public override object RawValue
        {
            get => base.RawValue;
            set => base.RawValue = Guard.OfType(value, nameof(value), typeof(ThemeValueReference), typeof(CornerRadius));
        }

        /// <summary>
        /// Creates a new <see cref="CornerRadiusThemeValue"/>.
        /// </summary>
        /// <param name="value">The value to use.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static CornerRadiusThemeValue Create(CornerRadius value) => CreateInternal(value);

        /// <summary>
        /// Creates a new <see cref="CornerRadiusThemeValue"/>.
        /// </summary>
        /// <param name="valueRef">The value reference.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static CornerRadiusThemeValue Create(ThemeValueReference valueRef) => CreateInternal(valueRef);

        private static CornerRadiusThemeValue CreateInternal(object value)
        {
            return new CornerRadiusThemeValue
            {
                RawValue = value,
            };
        }

        public static implicit operator CornerRadius(CornerRadiusThemeValue themeValue) => themeValue.Value;
    }
}
