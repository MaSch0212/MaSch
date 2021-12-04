using MaSch.Core;
using MaSch.Core.Attributes;
using MaSch.Presentation.Wpf.JsonConverters;
using MaSch.Presentation.Wpf.Models;
using System.ComponentModel;
using System.Windows.Media;

#nullable disable

namespace MaSch.Presentation.Wpf.ThemeValues;

/// <summary>
/// <see cref="IThemeValue"/> representing <see cref="SolidColorBrush"/> values.
/// </summary>
/// <seealso cref="ThemeValueBase{T}" />
public class SolidColorBrushThemeValue : ThemeValueBase<SolidColorBrush>
{
    private object _rawColor;
    private object _rawOpacity = 1D;

    /// <inheritdoc/>
    [JsonIgnore]
    public override object RawValue
    {
        get => Value;
        set => Value = Guard.OfType<SolidColorBrush>(value, nameof(value));
    }

    /// <summary>
    /// Gets or sets the raw value of the <see cref="Color"/> property.
    /// </summary>
    [JsonProperty(nameof(Color))]
    [JsonConverter(typeof(ThemeValuePropertyJsonConverter<Color>))]
    public object RawColor
    {
        get => _rawColor;
        set => SetProperty(ref _rawColor, value);
    }

    /// <summary>
    /// Gets or sets the raw value of the <see cref="Opacity"/> property.
    /// </summary>
    [JsonProperty(nameof(Opacity), DefaultValueHandling = DefaultValueHandling.Ignore)]
    [JsonConverter(typeof(ThemeValuePropertyJsonConverter<double>))]
    [DefaultValue(1.0D)]
    public object RawOpacity
    {
        get => _rawOpacity;
        set => SetProperty(ref _rawOpacity, value);
    }

    /// <inheritdoc/>
    [JsonIgnore]
    [DependsOn(nameof(Opacity), nameof(Color))]
    public override SolidColorBrush Value
    {
        get => new(Color) { Opacity = Opacity };
        set
        {
            Color = value.Color;
            Opacity = value.Opacity;
        }
    }

    /// <summary>
    /// Gets or sets the color of the brush.
    /// </summary>
    [JsonIgnore]
    [ThemeValueParsedProperty(nameof(RawColor))]
    [DependsOn(nameof(RawColor))]
    public Color Color
    {
        get => ParseValue<Color>(RawColor);
        set => RawColor = value;
    }

    /// <summary>
    /// Gets or sets the opacity of the brush.
    /// </summary>
    [JsonIgnore]
    [ThemeValueParsedProperty(nameof(RawOpacity))]
    [DependsOn(nameof(RawOpacity))]
    public double Opacity
    {
        get => ParseValue<double>(RawOpacity);
        set => RawOpacity = value;
    }

    public static implicit operator SolidColorBrush(SolidColorBrushThemeValue themeValue)
    {
        return themeValue.Value;
    }

    /// <summary>
    /// Creates a new <see cref="SolidColorBrushThemeValue"/>.
    /// </summary>
    /// <param name="color">The color of the brush.</param>
    /// <returns>The created <see cref="IThemeValue"/>.</returns>
    public static SolidColorBrushThemeValue Create(Color color)
    {
        return CreateInternal(color);
    }

    /// <summary>
    /// Creates a new <see cref="SolidColorBrushThemeValue"/>.
    /// </summary>
    /// <param name="colorRef">The reference to the color of the brush.</param>
    /// <returns>The created <see cref="IThemeValue"/>.</returns>
    public static SolidColorBrushThemeValue Create(ThemeValueReference colorRef)
    {
        return CreateInternal(colorRef);
    }

    /// <summary>
    /// Creates a new <see cref="SolidColorBrushThemeValue"/>.
    /// </summary>
    /// <param name="color">The color of the brush.</param>
    /// <param name="opacity">The opacity of the brush.</param>
    /// <returns>The created <see cref="IThemeValue"/>.</returns>
    public static SolidColorBrushThemeValue Create(Color color, double opacity)
    {
        return CreateInternal(color, opacity);
    }

    /// <summary>
    /// Creates a new <see cref="SolidColorBrushThemeValue"/>.
    /// </summary>
    /// <param name="colorRef">The reference to the color of the brush.</param>
    /// <param name="opacity">The opacity of the brush.</param>
    /// <returns>The created <see cref="IThemeValue"/>.</returns>
    public static SolidColorBrushThemeValue Create(ThemeValueReference colorRef, double opacity)
    {
        return CreateInternal(colorRef, opacity);
    }

    /// <summary>
    /// Creates a new <see cref="SolidColorBrushThemeValue"/>.
    /// </summary>
    /// <param name="color">The color of the brush.</param>
    /// <param name="opacityRef">The reference to the opacity of the brush.</param>
    /// <returns>The created <see cref="IThemeValue"/>.</returns>
    public static SolidColorBrushThemeValue Create(Color color, ThemeValueReference opacityRef)
    {
        return CreateInternal(color, opacityRef);
    }

    /// <summary>
    /// Creates a new <see cref="SolidColorBrushThemeValue"/>.
    /// </summary>
    /// <param name="colorRef">The reference to the color of the brush.</param>
    /// <param name="opacityRef">The reference to the opacity of the brush.</param>
    /// <returns>The created <see cref="IThemeValue"/>.</returns>
    public static SolidColorBrushThemeValue Create(ThemeValueReference colorRef, ThemeValueReference opacityRef)
    {
        return CreateInternal(colorRef, opacityRef);
    }

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        return obj is SolidColorBrushThemeValue other && Equals(other.RawColor, RawColor) && Equals(other.RawOpacity, RawOpacity);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return (RawColor, RawOpacity).GetHashCode();
    }

    /// <inheritdoc/>
    public override object Clone()
    {
        return new SolidColorBrushThemeValue
        {
            Key = Key,
            RawColor = RawColor.CloneIfPossible(),
            RawOpacity = RawOpacity.CloneIfPossible(),
        };
    }

    private static SolidColorBrushThemeValue CreateInternal(object color)
    {
        return new SolidColorBrushThemeValue
        {
            RawColor = color,
        };
    }

    private static SolidColorBrushThemeValue CreateInternal(object color, object opacity)
    {
        return new SolidColorBrushThemeValue
        {
            RawColor = color,
            RawOpacity = opacity,
        };
    }
}
