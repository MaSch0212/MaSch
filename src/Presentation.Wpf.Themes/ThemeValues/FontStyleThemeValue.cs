using MaSch.Core;
using MaSch.Presentation.Wpf.JsonConverters;
using MaSch.Presentation.Wpf.Models;
using System.Windows;

#nullable disable

namespace MaSch.Presentation.Wpf.ThemeValues;

/// <summary>
/// <see cref="IThemeValue"/> representing <see cref="FontStyle"/> values.
/// </summary>
/// <seealso cref="ThemeValueBase{T}" />
public class FontStyleThemeValue : ThemeValueBase<FontStyle>
{
    /// <inheritdoc/>
    [JsonConverter(typeof(ThemeValuePropertyJsonConverter<FontStyle>))]
    public override object RawValue
    {
        get => base.RawValue;
        set => base.RawValue = Guard.OfType(value, new[] { typeof(ThemeValueReference), typeof(FontStyle) });
    }

    public static implicit operator FontStyle(FontStyleThemeValue themeValue)
    {
        return themeValue.Value;
    }

    /// <summary>
    /// Creates a new <see cref="FontStyleThemeValue"/>.
    /// </summary>
    /// <param name="value">The value to use.</param>
    /// <returns>The created <see cref="IThemeValue"/>.</returns>
    public static FontStyleThemeValue Create(FontStyle value)
    {
        return CreateInternal(value);
    }

    /// <summary>
    /// Creates a new <see cref="FontStyleThemeValue"/>.
    /// </summary>
    /// <param name="valueRef">The value reference.</param>
    /// <returns>The created <see cref="IThemeValue"/>.</returns>
    public static FontStyleThemeValue Create(ThemeValueReference valueRef)
    {
        return CreateInternal(valueRef);
    }

    private static FontStyleThemeValue CreateInternal(object value)
    {
        return new FontStyleThemeValue
        {
            RawValue = value,
        };
    }
}
