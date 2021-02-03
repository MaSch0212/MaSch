using MaSch.Core;
using MaSch.Presentation.Wpf.JsonConverters;
using MaSch.Presentation.Wpf.Models;
using Newtonsoft.Json;

namespace MaSch.Presentation.Wpf.ThemeValues
{
    /// <summary>
    /// <see cref="IThemeValue"/> representing <see cref="bool"/> values.
    /// </summary>
    /// <seealso cref="MaSch.Presentation.Wpf.ThemeValues.ThemeValueBase{T}" />
    public class BooleanThemeValue : ThemeValueBase<bool>
    {
        /// <inheritdoc/>
        [JsonConverter(typeof(ThemeValuePropertyJsonConverter<bool>))]
        public override object RawValue
        {
            get => base.RawValue;
            set => base.RawValue = Guard.OfType(value, nameof(value), typeof(ThemeValueReference), typeof(bool));
        }

        /// <summary>
        /// Creates a new <see cref="BooleanThemeValue"/>.
        /// </summary>
        /// <param name="value">The value to use.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static BooleanThemeValue Create(bool value) => CreateInternal(value);

        /// <summary>
        /// Creates a new <see cref="BooleanThemeValue"/>.
        /// </summary>
        /// <param name="valueRef">The value reference.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static BooleanThemeValue Create(ThemeValueReference valueRef) => CreateInternal(valueRef);

        private static BooleanThemeValue CreateInternal(object value)
        {
            return new BooleanThemeValue
            {
                RawValue = value,
            };
        }

        public static implicit operator bool(BooleanThemeValue themeValue) => themeValue.Value;
    }
}
