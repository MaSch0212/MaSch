using MaSch.Core;
using MaSch.Presentation.Wpf.JsonConverters;
using MaSch.Presentation.Wpf.Models;
using System.Windows;

#nullable disable

namespace MaSch.Presentation.Wpf.ThemeValues;

/// <summary>
/// <see cref="IThemeValue"/> representing <see cref="FontWeight"/> values.
/// </summary>
/// <seealso cref="ThemeValueBase{T}" />
public class FontWeightThemeValue : ThemeValueBase<FontWeight>
{
    /// <inheritdoc/>
    [JsonConverter(typeof(ThemeValuePropertyJsonConverter<FontWeight>))]
    public override object RawValue
    {
        get => base.RawValue;
        set => base.RawValue = Guard.OfType(value, new[] { typeof(ThemeValueReference), typeof(FontWeight) });
    }

    public static implicit operator FontWeight(FontWeightThemeValue themeValue)
    {
        return themeValue.Value;
    }

    /// <summary>
    /// Creates a new <see cref="FontWeightThemeValue"/>.
    /// </summary>
    /// <param name="value">The value to use.</param>
    /// <returns>The created <see cref="IThemeValue"/>.</returns>
    public static FontWeightThemeValue Create(FontWeight value)
    {
        return CreateInternal(value);
    }

    /// <summary>
    /// Creates a new <see cref="FontWeightThemeValue"/>.
    /// </summary>
    /// <param name="valueRef">The value reference.</param>
    /// <returns>The created <see cref="IThemeValue"/>.</returns>
    public static FontWeightThemeValue Create(ThemeValueReference valueRef)
    {
        return CreateInternal(valueRef);
    }

    private static FontWeightThemeValue CreateInternal(object value)
    {
        return new FontWeightThemeValue
        {
            RawValue = value,
        };
    }
}
