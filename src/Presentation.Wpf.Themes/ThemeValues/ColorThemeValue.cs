using MaSch.Core;
using MaSch.Presentation.Wpf.JsonConverters;
using MaSch.Presentation.Wpf.Models;
using System.Windows.Media;

#nullable disable

namespace MaSch.Presentation.Wpf.ThemeValues;

/// <summary>
/// <see cref="IThemeValue"/> representing <see cref="Color"/> values.
/// </summary>
/// <seealso cref="ThemeValueBase{T}" />
public class ColorThemeValue : ThemeValueBase<Color>
{
    /// <inheritdoc/>
    [JsonConverter(typeof(ThemeValuePropertyJsonConverter<Color>))]
    [SuppressMessage("Critical Bug", "S4275:Getters and setters should access the expected fields", Justification = "Field is set via base class.")]
    public override object RawValue
    {
        get => base.RawValue;
        set => base.RawValue = Guard.OfType(value, new[] { typeof(ThemeValueReference), typeof(Color) });
    }

    public static implicit operator Color(ColorThemeValue themeValue)
    {
        return themeValue.Value;
    }

    /// <summary>
    /// Creates a new <see cref="ColorThemeValue"/>.
    /// </summary>
    /// <param name="value">The value to use.</param>
    /// <returns>The created <see cref="IThemeValue"/>.</returns>
    public static ColorThemeValue Create(Color value)
    {
        return CreateInternal(value);
    }

    /// <summary>
    /// Creates a new <see cref="ColorThemeValue"/>.
    /// </summary>
    /// <param name="valueRef">The value reference.</param>
    /// <returns>The created <see cref="IThemeValue"/>.</returns>
    public static ColorThemeValue Create(ThemeValueReference valueRef)
    {
        return CreateInternal(valueRef);
    }

    private static ColorThemeValue CreateInternal(object value)
    {
        return new ColorThemeValue
        {
            RawValue = value,
        };
    }
}
