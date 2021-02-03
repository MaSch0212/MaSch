using System.Windows.Media;
using MaSch.Core;
using MaSch.Presentation.Wpf.JsonConverters;
using MaSch.Presentation.Wpf.Models;
using Newtonsoft.Json;

namespace MaSch.Presentation.Wpf.ThemeValues
{
    /// <summary>
    /// <see cref="IThemeValue"/> representing <see cref="FontFamily"/> values.
    /// </summary>
    /// <seealso cref="MaSch.Presentation.Wpf.ThemeValues.ThemeValueBase{T}" />
    public class FontFamilyThemeValue : ThemeValueBase<FontFamily>
    {
        /// <inheritdoc/>
        [JsonConverter(typeof(ThemeValuePropertyJsonConverter<FontFamily>))]
        public override object RawValue
        {
            get => base.RawValue;
            set => base.RawValue = Guard.OfType(value, nameof(value), typeof(ThemeValueReference), typeof(FontFamily));
        }

        /// <summary>
        /// Creates a new <see cref="FontFamilyThemeValue"/>.
        /// </summary>
        /// <param name="value">The value to use.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static FontFamilyThemeValue Create(FontFamily value) => CreateInternal(value);

        /// <summary>
        /// Creates a new <see cref="FontFamilyThemeValue"/>.
        /// </summary>
        /// <param name="valueRef">The value reference.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static FontFamilyThemeValue Create(ThemeValueReference valueRef) => CreateInternal(valueRef);

        private static FontFamilyThemeValue CreateInternal(object value)
        {
            return new FontFamilyThemeValue
            {
                RawValue = value,
            };
        }

        public static implicit operator FontFamily(FontFamilyThemeValue themeValue) => themeValue.Value;

        /// <inheritdoc/>
        public override bool Equals(object obj)
            => obj is FontFamilyThemeValue other && Equals(other.RawValue.GetHashCode(), RawValue.GetHashCode());

        /// <inheritdoc/>
        public override int GetHashCode()
            => RawValue.GetHashCode();
    }
}
