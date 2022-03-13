using MaSch.Core;
using MaSch.Presentation.Wpf.JsonConverters;
using MaSch.Presentation.Wpf.Models;
using System.Windows.Media;

#nullable disable

namespace MaSch.Presentation.Wpf.ThemeValues;

/// <summary>
/// <see cref="IThemeValue"/> representing <see cref="ImageSource"/> values.
/// </summary>
/// <seealso cref="ThemeValueBase{T}" />
public class ImageSourceThemeValue : ThemeValueBase<ImageSource>
{
    /// <inheritdoc/>
    [JsonConverter(typeof(ThemeValuePropertyJsonConverter<ImageSource>))]
    [SuppressMessage("Critical Bug", "S4275:Getters and setters should access the expected fields", Justification = "Field is set via base class.")]
    public override object RawValue
    {
        get => base.RawValue;
        set => base.RawValue = Guard.OfType(value, new[] { typeof(ThemeValueReference), typeof(ImageSource) });
    }

    public static implicit operator ImageSource(ImageSourceThemeValue themeValue)
    {
        return themeValue.Value;
    }

    /// <summary>
    /// Creates a new <see cref="ImageSourceThemeValue"/>.
    /// </summary>
    /// <param name="value">The value to use.</param>
    /// <returns>The created <see cref="IThemeValue"/>.</returns>
    public static ImageSourceThemeValue Create(ImageSource value)
    {
        return CreateInternal(value);
    }

    /// <summary>
    /// Creates a new <see cref="ImageSourceThemeValue"/>.
    /// </summary>
    /// <param name="valueRef">The value reference.</param>
    /// <returns>The created <see cref="IThemeValue"/>.</returns>
    public static ImageSourceThemeValue Create(ThemeValueReference valueRef)
    {
        return CreateInternal(valueRef);
    }

    private static ImageSourceThemeValue CreateInternal(object value)
    {
        return new ImageSourceThemeValue
        {
            RawValue = value,
        };
    }
}
