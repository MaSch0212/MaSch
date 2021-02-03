using System.Windows.Media;
using MaSch.Core;
using MaSch.Presentation.Wpf.JsonConverters;
using MaSch.Presentation.Wpf.Models;
using Newtonsoft.Json;

namespace MaSch.Presentation.Wpf.ThemeValues
{
    /// <summary>
    /// <see cref="IThemeValue"/> representing <see cref="ImageSource"/> values.
    /// </summary>
    /// <seealso cref="MaSch.Presentation.Wpf.ThemeValues.ThemeValueBase{T}" />
    public class ImageSourceThemeValue : ThemeValueBase<ImageSource>
    {
        /// <inheritdoc/>
        [JsonConverter(typeof(ThemeValuePropertyJsonConverter<ImageSource>))]
        public override object RawValue
        {
            get => base.RawValue;
            set => base.RawValue = Guard.OfType(value, nameof(value), typeof(ThemeValueReference), typeof(ImageSource));
        }

        /// <summary>
        /// Creates a new <see cref="ImageSourceThemeValue"/>.
        /// </summary>
        /// <param name="value">The value to use.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static ImageSourceThemeValue Create(ImageSource value) => CreateInternal(value);

        /// <summary>
        /// Creates a new <see cref="ImageSourceThemeValue"/>.
        /// </summary>
        /// <param name="valueRef">The value reference.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static ImageSourceThemeValue Create(ThemeValueReference valueRef) => CreateInternal(valueRef);

        private static ImageSourceThemeValue CreateInternal(object value)
        {
            return new ImageSourceThemeValue
            {
                RawValue = value,
            };
        }

        public static implicit operator ImageSource(ImageSourceThemeValue themeValue) => themeValue.Value;
    }
}
