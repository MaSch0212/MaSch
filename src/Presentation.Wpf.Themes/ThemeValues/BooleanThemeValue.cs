using MaSch.Core;
using MaSch.Presentation.Wpf.JsonConverters;
using MaSch.Presentation.Wpf.Models;

#nullable disable

namespace MaSch.Presentation.Wpf.ThemeValues;

/// <summary>
/// <see cref="IThemeValue"/> representing <see cref="bool"/> values.
/// </summary>
/// <seealso cref="ThemeValueBase{T}" />
public class BooleanThemeValue : ThemeValueBase<bool>
{
    /// <inheritdoc/>
    [JsonConverter(typeof(ThemeValuePropertyJsonConverter<bool>))]
    [SuppressMessage("Critical Bug", "S4275:Getters and setters should access the expected fields", Justification = "Field is set via base class.")]
    public override object RawValue
    {
        get => base.RawValue;
        set => base.RawValue = Guard.OfType(value, new[] { typeof(ThemeValueReference), typeof(bool) });
    }

    public static implicit operator bool(BooleanThemeValue themeValue)
    {
        return themeValue.Value;
    }

    /// <summary>
    /// Creates a new <see cref="BooleanThemeValue"/>.
    /// </summary>
    /// <param name="value">The value to use.</param>
    /// <returns>The created <see cref="IThemeValue"/>.</returns>
    public static BooleanThemeValue Create(bool value)
    {
        return CreateInternal(value);
    }

    /// <summary>
    /// Creates a new <see cref="BooleanThemeValue"/>.
    /// </summary>
    /// <param name="valueRef">The value reference.</param>
    /// <returns>The created <see cref="IThemeValue"/>.</returns>
    public static BooleanThemeValue Create(ThemeValueReference valueRef)
    {
        return CreateInternal(valueRef);
    }

    private static BooleanThemeValue CreateInternal(object value)
    {
        return new BooleanThemeValue
        {
            RawValue = value,
        };
    }
}
