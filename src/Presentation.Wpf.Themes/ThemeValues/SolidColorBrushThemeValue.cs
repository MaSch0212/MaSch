using System.ComponentModel;
using System.Windows.Media;
using MaSch.Core;
using MaSch.Core.Extensions;
using MaSch.Core.Attributes;
using MaSch.Presentation.Wpf.JsonConverters;
using MaSch.Presentation.Wpf.Models;
using Newtonsoft.Json;

namespace MaSch.Presentation.Wpf.ThemeValues
{
    public class SolidColorBrushThemeValue : ThemeValueBase<SolidColorBrush>
    {
        #region Fields
        private object _rawColor;
        private object _rawOpacity = 1D;
        #endregion

        #region Properties
        [JsonIgnore]
        public override object RawValue
        {
            get => Value;
            set => Value = Guard.OfType<SolidColorBrush>(value, nameof(value));
        }

        [JsonProperty(nameof(Color))]
        [JsonConverter(typeof(ThemeValuePropertyJsonConverter<Color>))]
        public object RawColor
        {
            get => _rawColor;
            set => SetProperty(ref _rawColor, value);
        }

        [JsonProperty(nameof(Opacity), DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(ThemeValuePropertyJsonConverter<double>))]
        [DefaultValue(1.0D)]
        public object RawOpacity
        {
            get => _rawOpacity;
            set => SetProperty(ref _rawOpacity, value);
        }
        #endregion

        #region Parsed Properties
        [JsonIgnore]
        [DependsOn(nameof(Opacity), nameof(Color))]
        public override SolidColorBrush Value
        {
            get => new SolidColorBrush(Color) { Opacity = Opacity };
            set
            {
                Color = value.Color;
                Opacity = value.Opacity;
            }
        }

        [JsonIgnore, ThemeValueParsedProperty(nameof(RawColor))]
        [DependsOn(nameof(RawColor))]
        public Color Color
        {
            get => ParseValue<Color>(RawColor);
            set => RawColor = value;
        }

        [JsonIgnore, ThemeValueParsedProperty(nameof(RawOpacity))]
        [DependsOn(nameof(RawOpacity))]
        public double Opacity
        {
            get => ParseValue<double>(RawOpacity);
            set => RawOpacity = value;
        }
        #endregion

        #region Static Members
        public static SolidColorBrushThemeValue Create(Color color) => CreateInternal(color);
        public static SolidColorBrushThemeValue Create(ThemeValueReference colorRef) => CreateInternal(colorRef);
        public static SolidColorBrushThemeValue Create(Color color, double opacity) => CreateInternal(color, opacity);
        public static SolidColorBrushThemeValue Create(ThemeValueReference colorRef, double opacity) => CreateInternal(colorRef, opacity);
        public static SolidColorBrushThemeValue Create(Color color, ThemeValueReference opacityRef) => CreateInternal(color, opacityRef);
        public static SolidColorBrushThemeValue Create(ThemeValueReference colorRef, ThemeValueReference opacityRef) => CreateInternal(colorRef, opacityRef);
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
                RawOpacity = opacity
            };
        }

        public static implicit operator SolidColorBrush(SolidColorBrushThemeValue themeValue) => themeValue.Value;
        #endregion

        public override bool Equals(object obj) 
            => obj is SolidColorBrushThemeValue other && Equals(other.RawColor, RawColor) && Equals(other.RawOpacity, RawOpacity);

        public override int GetHashCode() 
            => (RawColor, RawOpacity).GetHashCode();

        public override object Clone()
        {
            return new SolidColorBrushThemeValue
            {
                Key = Key,
                RawColor = RawColor.CloneIfPossible(),
                RawOpacity = RawOpacity.CloneIfPossible()
            };
        }
    }
}
