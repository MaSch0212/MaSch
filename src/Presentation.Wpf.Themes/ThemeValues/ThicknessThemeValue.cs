using MaSch.Core;
using MaSch.Presentation.Wpf.JsonConverters;
using MaSch.Presentation.Wpf.Models;
using System.Windows;

#nullable disable

namespace MaSch.Presentation.Wpf.ThemeValues;

/// <summary>
/// <see cref="IThemeValue"/> representing <see cref="Thickness"/> values.
/// </summary>
/// <seealso cref="ThemeValueBase{T}" />
public class ThicknessThemeValue : ThemeValueBase<Thickness>
{
    /// <inheritdoc/>
    [JsonConverter(typeof(ThemeValuePropertyJsonConverter<Thickness>))]
    [SuppressMessage("Critical Bug", "S4275:Getters and setters should access the expected fields", Justification = "Field is set via base class.")]
    public override object RawValue
    {
        get => base.RawValue;
        set => base.RawValue = Guard.OfType(value, new[] { typeof(ThemeValueReference), typeof(Thickness) });
    }

    public static implicit operator Thickness(ThicknessThemeValue themeValue)
    {
        return themeValue.Value;
    }

    /// <summary>
    /// Creates a new <see cref="ThicknessThemeValue"/>.
    /// </summary>
    /// <param name="value">The value to use.</param>
    /// <returns>The created <see cref="IThemeValue"/>.</returns>
    public static ThicknessThemeValue Create(Thickness value)
    {
        return CreateInternal(value);
    }

    /// <summary>
    /// Creates a new <see cref="ThicknessThemeValue"/>.
    /// </summary>
    /// <param name="valueRef">The value reference.</param>
    /// <returns>The created <see cref="IThemeValue"/>.</returns>
    public static ThicknessThemeValue Create(ThemeValueReference valueRef)
    {
        return CreateInternal(valueRef);
    }

    private static ThicknessThemeValue CreateInternal(object value)
    {
        return new ThicknessThemeValue
        {
            RawValue = value,
        };
    }
}
