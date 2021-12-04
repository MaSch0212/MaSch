using MaSch.Core;
using MaSch.Presentation.Wpf.JsonConverters;
using MaSch.Presentation.Wpf.Models;

#nullable disable

namespace MaSch.Presentation.Wpf.ThemeValues;

/// <summary>
/// <see cref="IThemeValue"/> representing <see cref="double"/> values.
/// </summary>
/// <seealso cref="ThemeValueBase{T}" />
public class DoubleThemeValue : ThemeValueBase<double>
{
    /// <inheritdoc/>
    [JsonConverter(typeof(ThemeValuePropertyJsonConverter<double>))]
    public override object RawValue
    {
        get => base.RawValue;
        set => base.RawValue = Guard.OfType(value, nameof(value), typeof(ThemeValueReference), typeof(double));
    }

    public static implicit operator double(DoubleThemeValue themeValue)
    {
        return themeValue.Value;
    }

    /// <summary>
    /// Creates a new <see cref="DoubleThemeValue"/>.
    /// </summary>
    /// <param name="value">The value to use.</param>
    /// <returns>The created <see cref="IThemeValue"/>.</returns>
    public static DoubleThemeValue Create(double value)
    {
        return CreateInternal(value);
    }

    /// <summary>
    /// Creates a new <see cref="DoubleThemeValue"/>.
    /// </summary>
    /// <param name="valueRef">The value reference.</param>
    /// <returns>The created <see cref="IThemeValue"/>.</returns>
    public static DoubleThemeValue Create(ThemeValueReference valueRef)
    {
        return CreateInternal(valueRef);
    }

    private static DoubleThemeValue CreateInternal(object value)
    {
        return new DoubleThemeValue
        {
            RawValue = value,
        };
    }
}
