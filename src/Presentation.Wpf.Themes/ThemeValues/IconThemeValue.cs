using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Media;
using MaSch.Core;
using MaSch.Core.Attributes;
using MaSch.Core.Extensions;
using MaSch.Presentation.Wpf.JsonConverters;
using MaSch.Presentation.Wpf.Models;
using Newtonsoft.Json;

#nullable disable

namespace MaSch.Presentation.Wpf.ThemeValues
{
    /// <summary>
    /// <see cref="IThemeValue"/> representing <see cref="Icon"/> values.
    /// </summary>
    /// <seealso cref="MaSch.Presentation.Wpf.ThemeValues.ThemeValueBase{T}" />
    public class IconThemeValue : ThemeValueBase<Icon>
    {
        private const double DefaultFontSize = 12D;
        private const bool DefaultIsGeometryFilled = true;
        private const double DefaultGeometryStrokeThickness = 0D;
        private const Stretch DefaultStretch = Stretch.Uniform;

        private object _rawIconType;
        private object _rawCharacter;
        private object _rawFont;
        private object _rawFontSize = DefaultFontSize;
        private object _rawGeometry;
        private object _rawIsGeometryFilled = DefaultIsGeometryFilled;
        private object _rawGeometryStrokeThickness = DefaultGeometryStrokeThickness;
        private object _rawStretch = DefaultStretch;

        /// <inheritdoc/>
        [JsonIgnore]
        public override object RawValue
        {
            get => Value;
            set => Value = Guard.OfType<Icon>(value, nameof(value));
        }

        /// <summary>
        /// Gets or sets the raw value of the <see cref="IconType"/> property.
        /// </summary>
        [JsonProperty("IconType")]
        [JsonConverter(typeof(ThemeValuePropertyJsonConverter<SymbolType>))]
        public object RawIconType
        {
            get => _rawIconType;
            set => SetProperty(ref _rawIconType, value);
        }

        /// <summary>
        /// Gets or sets the raw value of the <see cref="Character"/> property.
        /// </summary>
        [JsonProperty("Character", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(ThemeValuePropertyJsonConverter<string>))]
        [DefaultValue(null)]
        public object RawCharacter
        {
            get => _rawCharacter;
            set => SetProperty(ref _rawCharacter, value);
        }

        /// <summary>
        /// Gets or sets the raw value of the <see cref="Font"/> property.
        /// </summary>
        [JsonProperty("Font", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(ThemeValuePropertyJsonConverter<FontFamily>))]
        [DefaultValue(null)]
        public object RawFont
        {
            get => _rawFont;
            set => SetProperty(ref _rawFont, value);
        }

        /// <summary>
        /// Gets or sets the raw value of the <see cref="FontSize"/> property.
        /// </summary>
        [JsonProperty("FontSize", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(ThemeValuePropertyJsonConverter<double>))]
        [DefaultValue(DefaultFontSize)]
        public object RawFontSize
        {
            get => _rawFontSize;
            set => SetProperty(ref _rawFontSize, value);
        }

        /// <summary>
        /// Gets or sets the raw value of the <see cref="Geometry"/> property.
        /// </summary>
        [JsonProperty("Geometry", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(ThemeValuePropertyJsonConverter<Geometry>))]
        [DefaultValue(null)]
        public object RawGeometry
        {
            get => _rawGeometry;
            set => SetProperty(ref _rawGeometry, value);
        }

        /// <summary>
        /// Gets or sets the raw value of the <see cref="IsGeometryFilled"/> property.
        /// </summary>
        [JsonProperty("IsGeometryFilled", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(ThemeValuePropertyJsonConverter<bool>))]
        [DefaultValue(DefaultIsGeometryFilled)]
        public object RawIsGeometryFilled
        {
            get => _rawIsGeometryFilled;
            set => SetProperty(ref _rawIsGeometryFilled, value);
        }

        /// <summary>
        /// Gets or sets the raw value of the <see cref="GeometryStrokeThickness"/> property.
        /// </summary>
        [JsonProperty("GeometryStrokeThickness", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(ThemeValuePropertyJsonConverter<double>))]
        [DefaultValue(DefaultGeometryStrokeThickness)]
        public object RawGeometryStrokeThickness
        {
            get => _rawGeometryStrokeThickness;
            set => SetProperty(ref _rawGeometryStrokeThickness, value);
        }

        /// <summary>
        /// Gets or sets the raw value of the <see cref="Stretch"/> property.
        /// </summary>
        [JsonProperty("Stretch", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(ThemeValuePropertyJsonConverter<Stretch>))]
        [DefaultValue(DefaultStretch)]
        public object RawStretch
        {
            get => _rawStretch;
            set => SetProperty(ref _rawStretch, value);
        }

        /// <inheritdoc/>
        [JsonIgnore]
        [DependsOn(nameof(IconType), nameof(Character), nameof(Font), nameof(FontSize), nameof(Geometry), nameof(IsGeometryFilled), nameof(GeometryStrokeThickness), nameof(Stretch))]
        public override Icon Value
        {
            get => new Icon
            {
                Type = IconType,
                Character = Character,
                Font = Font,
                FontSize = FontSize,
                Geometry = Geometry,
                IsGeometryFilled = IsGeometryFilled,
                GeometryStrokeThickness = GeometryStrokeThickness,
                Stretch = Stretch,
            };
            set
            {
                IconType = value.Type;
                Character = value.Character;
                Font = value.Font;
                FontSize = value.FontSize;
                Geometry = value.Geometry;
                IsGeometryFilled = value.IsGeometryFilled;
                GeometryStrokeThickness = value.GeometryStrokeThickness;
                Stretch = value.Stretch;
            }
        }

        /// <summary>
        /// Gets or sets the type of the icon.
        /// </summary>
        [JsonIgnore]
        [ThemeValueParsedProperty(nameof(RawIconType))]
        [DependsOn(nameof(RawIconType))]
        public SymbolType IconType
        {
            get => ParseValue<SymbolType>(RawIconType);
            set => RawIconType = value;
        }

        /// <summary>
        /// Gets or sets the character of the icon font.
        /// </summary>
        [JsonIgnore]
        [ThemeValueParsedProperty(nameof(RawCharacter))]
        [DependsOn(nameof(RawCharacter))]
        public string Character
        {
            get => ParseValue<string>(RawCharacter);
            set => RawCharacter = value;
        }

        /// <summary>
        /// Gets or sets the icon font.
        /// </summary>
        [JsonIgnore]
        [ThemeValueParsedProperty(nameof(RawFont))]
        [DependsOn(nameof(RawFont))]
        public FontFamily Font
        {
            get => ParseValue<FontFamily>(RawFont);
            set => RawFont = value;
        }

        /// <summary>
        /// Gets or sets the size of the font.
        /// </summary>
        [JsonIgnore]
        [ThemeValueParsedProperty(nameof(RawFontSize))]
        [DependsOn(nameof(RawFontSize))]
        public double FontSize
        {
            get => ParseValue<double>(RawFontSize);
            set => RawFontSize = value;
        }

        /// <summary>
        /// Gets or sets the geometry.
        /// </summary>
        [JsonIgnore]
        [ThemeValueParsedProperty(nameof(RawGeometry))]
        [DependsOn(nameof(RawGeometry))]
        public Geometry Geometry
        {
            get => ParseValue<Geometry>(RawGeometry);
            set => RawGeometry = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the geometry is filled.
        /// </summary>
        [JsonIgnore]
        [ThemeValueParsedProperty(nameof(RawIsGeometryFilled))]
        [DependsOn(nameof(RawIsGeometryFilled))]
        public bool IsGeometryFilled
        {
            get => ParseValue<bool>(RawIsGeometryFilled);
            set => RawIsGeometryFilled = value;
        }

        /// <summary>
        /// Gets or sets the geometry stroke thickness.
        /// </summary>
        [JsonIgnore]
        [ThemeValueParsedProperty(nameof(RawGeometryStrokeThickness))]
        [DependsOn(nameof(RawGeometryStrokeThickness))]
        public double GeometryStrokeThickness
        {
            get => ParseValue<double>(RawGeometryStrokeThickness);
            set => RawGeometryStrokeThickness = value;
        }

        /// <summary>
        /// Gets or sets the stretch mode for this icon.
        /// </summary>
        [JsonIgnore]
        [ThemeValueParsedProperty(nameof(RawStretch))]
        [DependsOn(nameof(RawStretch))]
        public Stretch Stretch
        {
            get => ParseValue<Stretch>(RawStretch);
            set => RawStretch = value;
        }

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using a geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateGeometry(Geometry geometry) => CreateGeometryInternal(geometry, DefaultIsGeometryFilled, DefaultGeometryStrokeThickness, DefaultStretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using a geometry.
        /// </summary>
        /// <param name="geometry">The reference to a geometry.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateGeometry(ThemeValueReference geometry) => CreateGeometryInternal(geometry, DefaultIsGeometryFilled, DefaultGeometryStrokeThickness, DefaultStretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using a geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="isGeometryFilled"><c>true</c> when the geometry should be filled.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateGeometry(Geometry geometry, bool isGeometryFilled) => CreateGeometryInternal(geometry, isGeometryFilled, DefaultGeometryStrokeThickness, DefaultStretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using a geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="isGeometryFilled">The reference to a value determining whether the geometry should be filled.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateGeometry(Geometry geometry, ThemeValueReference isGeometryFilled) => CreateGeometryInternal(geometry, isGeometryFilled, DefaultGeometryStrokeThickness, DefaultStretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using a geometry.
        /// </summary>
        /// <param name="geometry">The reference to a geometry.</param>
        /// <param name="isGeometryFilled"><c>true</c> when the geometry should be filled.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateGeometry(ThemeValueReference geometry, bool isGeometryFilled) => CreateGeometryInternal(geometry, isGeometryFilled, DefaultGeometryStrokeThickness, DefaultStretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using a geometry.
        /// </summary>
        /// <param name="geometry">The reference to a geometry.</param>
        /// <param name="isGeometryFilled">The reference to a value determining whether the geometry should be filled.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateGeometry(ThemeValueReference geometry, ThemeValueReference isGeometryFilled) => CreateGeometryInternal(geometry, isGeometryFilled, DefaultGeometryStrokeThickness, DefaultStretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using a geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="isGeometryFilled"><c>true</c> when the geometry should be filled.</param>
        /// <param name="geometryStrokeThickness">The stroke thickness of the geometry.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateGeometry(Geometry geometry, bool isGeometryFilled, double geometryStrokeThickness) => CreateGeometryInternal(geometry, isGeometryFilled, geometryStrokeThickness, DefaultStretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using a geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="isGeometryFilled"><c>true</c> when the geometry should be filled.</param>
        /// <param name="geometryStrokeThickness">The reference to the stroke thickness of the geometry.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateGeometry(Geometry geometry, bool isGeometryFilled, ThemeValueReference geometryStrokeThickness) => CreateGeometryInternal(geometry, isGeometryFilled, geometryStrokeThickness, DefaultStretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using a geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="isGeometryFilled">The reference to a value determining whether the geometry should be filled.</param>
        /// <param name="geometryStrokeThickness">The stroke thickness of the geometry.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateGeometry(Geometry geometry, ThemeValueReference isGeometryFilled, double geometryStrokeThickness) => CreateGeometryInternal(geometry, isGeometryFilled, geometryStrokeThickness, DefaultStretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using a geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="isGeometryFilled">The reference to a value determining whether the geometry should be filled.</param>
        /// <param name="geometryStrokeThickness">The reference to the stroke thickness of the geometry.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateGeometry(Geometry geometry, ThemeValueReference isGeometryFilled, ThemeValueReference geometryStrokeThickness) => CreateGeometryInternal(geometry, isGeometryFilled, geometryStrokeThickness, DefaultStretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using a geometry.
        /// </summary>
        /// <param name="geometry">The reference to a geometry.</param>
        /// <param name="isGeometryFilled"><c>true</c> when the geometry should be filled.</param>
        /// <param name="geometryStrokeThickness">The stroke thickness of the geometry.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateGeometry(ThemeValueReference geometry, bool isGeometryFilled, double geometryStrokeThickness) => CreateGeometryInternal(geometry, isGeometryFilled, geometryStrokeThickness, DefaultStretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using a geometry.
        /// </summary>
        /// <param name="geometry">The reference to a geometry.</param>
        /// <param name="isGeometryFilled"><c>true</c> when the geometry should be filled.</param>
        /// <param name="geometryStrokeThickness">The reference to the stroke thickness of the geometry.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateGeometry(ThemeValueReference geometry, bool isGeometryFilled, ThemeValueReference geometryStrokeThickness) => CreateGeometryInternal(geometry, isGeometryFilled, geometryStrokeThickness, DefaultStretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using a geometry.
        /// </summary>
        /// <param name="geometry">The reference to a geometry.</param>
        /// <param name="isGeometryFilled">The reference to a value determining whether the geometry should be filled.</param>
        /// <param name="geometryStrokeThickness">The stroke thickness of the geometry.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateGeometry(ThemeValueReference geometry, ThemeValueReference isGeometryFilled, double geometryStrokeThickness) => CreateGeometryInternal(geometry, isGeometryFilled, geometryStrokeThickness, DefaultStretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using a geometry.
        /// </summary>
        /// <param name="geometry">The reference to a geometry.</param>
        /// <param name="isGeometryFilled">The reference to a value determining whether the geometry should be filled.</param>
        /// <param name="geometryStrokeThickness">The reference to the stroke thickness of the geometry.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateGeometry(ThemeValueReference geometry, ThemeValueReference isGeometryFilled, ThemeValueReference geometryStrokeThickness) => CreateGeometryInternal(geometry, isGeometryFilled, geometryStrokeThickness, DefaultStretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using a geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="isGeometryFilled"><c>true</c> when the geometry should be filled.</param>
        /// <param name="geometryStrokeThickness">The stroke thickness of the geometry.</param>
        /// <param name="stretch">The stretch mode for the icon.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateGeometry(Geometry geometry, bool isGeometryFilled, double geometryStrokeThickness, Stretch stretch) => CreateGeometryInternal(geometry, isGeometryFilled, geometryStrokeThickness, stretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using a geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="isGeometryFilled"><c>true</c> when the geometry should be filled.</param>
        /// <param name="geometryStrokeThickness">The stroke thickness of the geometry.</param>
        /// <param name="stretch">The reference to the stretch mode for the icon.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateGeometry(Geometry geometry, bool isGeometryFilled, double geometryStrokeThickness, ThemeValueReference stretch) => CreateGeometryInternal(geometry, isGeometryFilled, geometryStrokeThickness, stretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using a geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="isGeometryFilled"><c>true</c> when the geometry should be filled.</param>
        /// <param name="geometryStrokeThickness">The reference to the stroke thickness of the geometry.</param>
        /// <param name="stretch">The stretch mode for the icon.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateGeometry(Geometry geometry, bool isGeometryFilled, ThemeValueReference geometryStrokeThickness, Stretch stretch) => CreateGeometryInternal(geometry, isGeometryFilled, geometryStrokeThickness, stretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using a geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="isGeometryFilled"><c>true</c> when the geometry should be filled.</param>
        /// <param name="geometryStrokeThickness">The reference to the stroke thickness of the geometry.</param>
        /// <param name="stretch">The reference to the stretch mode for the icon.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateGeometry(Geometry geometry, bool isGeometryFilled, ThemeValueReference geometryStrokeThickness, ThemeValueReference stretch) => CreateGeometryInternal(geometry, isGeometryFilled, geometryStrokeThickness, stretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using a geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="isGeometryFilled">The reference to a value determining whether the geometry should be filled.</param>
        /// <param name="geometryStrokeThickness">The stroke thickness of the geometry.</param>
        /// <param name="stretch">The stretch mode for the icon.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateGeometry(Geometry geometry, ThemeValueReference isGeometryFilled, double geometryStrokeThickness, Stretch stretch) => CreateGeometryInternal(geometry, isGeometryFilled, geometryStrokeThickness, stretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using a geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="isGeometryFilled">The reference to a value determining whether the geometry should be filled.</param>
        /// <param name="geometryStrokeThickness">The stroke thickness of the geometry.</param>
        /// <param name="stretch">The reference to the stretch mode for the icon.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateGeometry(Geometry geometry, ThemeValueReference isGeometryFilled, double geometryStrokeThickness, ThemeValueReference stretch) => CreateGeometryInternal(geometry, isGeometryFilled, geometryStrokeThickness, stretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using a geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="isGeometryFilled">The reference to a value determining whether the geometry should be filled.</param>
        /// <param name="geometryStrokeThickness">The reference to the stroke thickness of the geometry.</param>
        /// <param name="stretch">The stretch mode for the icon.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateGeometry(Geometry geometry, ThemeValueReference isGeometryFilled, ThemeValueReference geometryStrokeThickness, Stretch stretch) => CreateGeometryInternal(geometry, isGeometryFilled, geometryStrokeThickness, stretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using a geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="isGeometryFilled">The reference to a value determining whether the geometry should be filled.</param>
        /// <param name="geometryStrokeThickness">The reference to the stroke thickness of the geometry.</param>
        /// <param name="stretch">The reference to the stretch mode for the icon.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateGeometry(Geometry geometry, ThemeValueReference isGeometryFilled, ThemeValueReference geometryStrokeThickness, ThemeValueReference stretch) => CreateGeometryInternal(geometry, isGeometryFilled, geometryStrokeThickness, stretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using a geometry.
        /// </summary>
        /// <param name="geometry">The reference to a geometry.</param>
        /// <param name="isGeometryFilled"><c>true</c> when the geometry should be filled.</param>
        /// <param name="geometryStrokeThickness">The stroke thickness of the geometry.</param>
        /// <param name="stretch">The stretch mode for the icon.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateGeometry(ThemeValueReference geometry, bool isGeometryFilled, double geometryStrokeThickness, Stretch stretch) => CreateGeometryInternal(geometry, isGeometryFilled, geometryStrokeThickness, stretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using a geometry.
        /// </summary>
        /// <param name="geometry">The reference to a geometry.</param>
        /// <param name="isGeometryFilled"><c>true</c> when the geometry should be filled.</param>
        /// <param name="geometryStrokeThickness">The stroke thickness of the geometry.</param>
        /// <param name="stretch">The reference to the stretch mode for the icon.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateGeometry(ThemeValueReference geometry, bool isGeometryFilled, double geometryStrokeThickness, ThemeValueReference stretch) => CreateGeometryInternal(geometry, isGeometryFilled, geometryStrokeThickness, stretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using a geometry.
        /// </summary>
        /// <param name="geometry">The reference to a geometry.</param>
        /// <param name="isGeometryFilled"><c>true</c> when the geometry should be filled.</param>
        /// <param name="geometryStrokeThickness">The reference to the stroke thickness of the geometry.</param>
        /// <param name="stretch">The stretch mode for the icon.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateGeometry(ThemeValueReference geometry, bool isGeometryFilled, ThemeValueReference geometryStrokeThickness, Stretch stretch) => CreateGeometryInternal(geometry, isGeometryFilled, geometryStrokeThickness, stretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using a geometry.
        /// </summary>
        /// <param name="geometry">The reference to a geometry.</param>
        /// <param name="isGeometryFilled"><c>true</c> when the geometry should be filled.</param>
        /// <param name="geometryStrokeThickness">The reference to the stroke thickness of the geometry.</param>
        /// <param name="stretch">The reference to the stretch mode for the icon.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateGeometry(ThemeValueReference geometry, bool isGeometryFilled, ThemeValueReference geometryStrokeThickness, ThemeValueReference stretch) => CreateGeometryInternal(geometry, isGeometryFilled, geometryStrokeThickness, stretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using a geometry.
        /// </summary>
        /// <param name="geometry">The reference to a geometry.</param>
        /// <param name="isGeometryFilled">The reference to a value determining whether the geometry should be filled.</param>
        /// <param name="geometryStrokeThickness">The stroke thickness of the geometry.</param>
        /// <param name="stretch">The stretch mode for the icon.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateGeometry(ThemeValueReference geometry, ThemeValueReference isGeometryFilled, double geometryStrokeThickness, Stretch stretch) => CreateGeometryInternal(geometry, isGeometryFilled, geometryStrokeThickness, stretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using a geometry.
        /// </summary>
        /// <param name="geometry">The reference to a geometry.</param>
        /// <param name="isGeometryFilled">The reference to a value determining whether the geometry should be filled.</param>
        /// <param name="geometryStrokeThickness">The stroke thickness of the geometry.</param>
        /// <param name="stretch">The reference to the stretch mode for the icon.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateGeometry(ThemeValueReference geometry, ThemeValueReference isGeometryFilled, double geometryStrokeThickness, ThemeValueReference stretch) => CreateGeometryInternal(geometry, isGeometryFilled, geometryStrokeThickness, stretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using a geometry.
        /// </summary>
        /// <param name="geometry">The reference to a geometry.</param>
        /// <param name="isGeometryFilled">The reference to a value determining whether the geometry should be filled.</param>
        /// <param name="geometryStrokeThickness">The reference to the stroke thickness of the geometry.</param>
        /// <param name="stretch">The stretch mode for the icon.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateGeometry(ThemeValueReference geometry, ThemeValueReference isGeometryFilled, ThemeValueReference geometryStrokeThickness, Stretch stretch) => CreateGeometryInternal(geometry, isGeometryFilled, geometryStrokeThickness, stretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using a geometry.
        /// </summary>
        /// <param name="geometry">The reference to a geometry.</param>
        /// <param name="isGeometryFilled">The reference to a value determining whether the geometry should be filled.</param>
        /// <param name="geometryStrokeThickness">The reference to the stroke thickness of the geometry.</param>
        /// <param name="stretch">The reference to the stretch mode for the icon.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateGeometry(ThemeValueReference geometry, ThemeValueReference isGeometryFilled, ThemeValueReference geometryStrokeThickness, ThemeValueReference stretch) => CreateGeometryInternal(geometry, isGeometryFilled, geometryStrokeThickness, stretch);

        private static IconThemeValue CreateGeometryInternal(object geometry, object isGeometryFilled, object geometryStrokeThickness, object stretch)
        {
            return new IconThemeValue
            {
                RawIconType = SymbolType.Geometry,
                RawGeometry = geometry,
                RawIsGeometryFilled = isGeometryFilled,
                RawGeometryStrokeThickness = geometryStrokeThickness,
                RawStretch = stretch,
            };
        }

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using an icon font character.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <param name="character">The icon character.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateCharacter(FontFamily font, string character) => CreateCharacterInternal(font, character, DefaultFontSize, DefaultStretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using an icon font character.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <param name="character">The reference to the icon character.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateCharacter(FontFamily font, ThemeValueReference character) => CreateCharacterInternal(font, character, DefaultFontSize, DefaultStretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using an icon font character.
        /// </summary>
        /// <param name="font">The reference to the font.</param>
        /// <param name="character">The icon character.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateCharacter(ThemeValueReference font, string character) => CreateCharacterInternal(font, character, DefaultFontSize, DefaultStretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using an icon font character.
        /// </summary>
        /// <param name="font">The reference to the font.</param>
        /// <param name="character">The reference to the icon character.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateCharacter(ThemeValueReference font, ThemeValueReference character) => CreateCharacterInternal(font, character, DefaultFontSize, DefaultStretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using an icon font character.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <param name="character">The icon character.</param>
        /// <param name="fontSize">The font size.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateCharacter(FontFamily font, string character, double fontSize) => CreateCharacterInternal(font, character, fontSize, DefaultStretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using an icon font character.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <param name="character">The icon character.</param>
        /// <param name="fontSize">The reference to the font size.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateCharacter(FontFamily font, string character, ThemeValueReference fontSize) => CreateCharacterInternal(font, character, fontSize, DefaultStretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using an icon font character.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <param name="character">The reference to the icon character.</param>
        /// <param name="fontSize">The font size.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateCharacter(FontFamily font, ThemeValueReference character, double fontSize) => CreateCharacterInternal(font, character, fontSize, DefaultStretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using an icon font character.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <param name="character">The reference to the icon character.</param>
        /// <param name="fontSize">The reference to the font size.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateCharacter(FontFamily font, ThemeValueReference character, ThemeValueReference fontSize) => CreateCharacterInternal(font, character, fontSize, DefaultStretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using an icon font character.
        /// </summary>
        /// <param name="font">The reference to the font.</param>
        /// <param name="character">The icon character.</param>
        /// <param name="fontSize">The font size.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateCharacter(ThemeValueReference font, string character, double fontSize) => CreateCharacterInternal(font, character, fontSize, DefaultStretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using an icon font character.
        /// </summary>
        /// <param name="font">The reference to the font.</param>
        /// <param name="character">The icon character.</param>
        /// <param name="fontSize">The reference to the font size.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateCharacter(ThemeValueReference font, string character, ThemeValueReference fontSize) => CreateCharacterInternal(font, character, fontSize, DefaultStretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using an icon font character.
        /// </summary>
        /// <param name="font">The reference to the font.</param>
        /// <param name="character">The reference to the icon character.</param>
        /// <param name="fontSize">The font size.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateCharacter(ThemeValueReference font, ThemeValueReference character, double fontSize) => CreateCharacterInternal(font, character, fontSize, DefaultStretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using an icon font character.
        /// </summary>
        /// <param name="font">The reference to the font.</param>
        /// <param name="character">The reference to the icon character.</param>
        /// <param name="fontSize">The reference to the font size.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateCharacter(ThemeValueReference font, ThemeValueReference character, ThemeValueReference fontSize) => CreateCharacterInternal(font, character, fontSize, DefaultStretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using an icon font character.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <param name="character">The icon character.</param>
        /// <param name="fontSize">The font size.</param>
        /// <param name="stretch">The stretch mode for the icon.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateCharacter(FontFamily font, string character, double fontSize, Stretch stretch) => CreateCharacterInternal(font, character, fontSize, stretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using an icon font character.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <param name="character">The icon character.</param>
        /// <param name="fontSize">The font size.</param>
        /// <param name="stretch">The reference to the stretch mode for the icon.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateCharacter(FontFamily font, string character, double fontSize, ThemeValueReference stretch) => CreateCharacterInternal(font, character, fontSize, stretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using an icon font character.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <param name="character">The icon character.</param>
        /// <param name="fontSize">The reference to the font size.</param>
        /// <param name="stretch">The stretch mode for the icon.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateCharacter(FontFamily font, string character, ThemeValueReference fontSize, Stretch stretch) => CreateCharacterInternal(font, character, fontSize, stretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using an icon font character.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <param name="character">The icon character.</param>
        /// <param name="fontSize">The reference to the font size.</param>
        /// <param name="stretch">The reference to the stretch mode for the icon.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateCharacter(FontFamily font, string character, ThemeValueReference fontSize, ThemeValueReference stretch) => CreateCharacterInternal(font, character, fontSize, stretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using an icon font character.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <param name="character">The reference to the icon character.</param>
        /// <param name="fontSize">The font size.</param>
        /// <param name="stretch">The stretch mode for the icon.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateCharacter(FontFamily font, ThemeValueReference character, double fontSize, Stretch stretch) => CreateCharacterInternal(font, character, fontSize, stretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using an icon font character.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <param name="character">The reference to the icon character.</param>
        /// <param name="fontSize">The font size.</param>
        /// <param name="stretch">The reference to the stretch mode for the icon.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateCharacter(FontFamily font, ThemeValueReference character, double fontSize, ThemeValueReference stretch) => CreateCharacterInternal(font, character, fontSize, stretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using an icon font character.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <param name="character">The reference to the icon character.</param>
        /// <param name="fontSize">The reference to the font size.</param>
        /// <param name="stretch">The stretch mode for the icon.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateCharacter(FontFamily font, ThemeValueReference character, ThemeValueReference fontSize, Stretch stretch) => CreateCharacterInternal(font, character, fontSize, stretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using an icon font character.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <param name="character">The reference to the icon character.</param>
        /// <param name="fontSize">The reference to the font size.</param>
        /// <param name="stretch">The reference to the stretch mode for the icon.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateCharacter(FontFamily font, ThemeValueReference character, ThemeValueReference fontSize, ThemeValueReference stretch) => CreateCharacterInternal(font, character, fontSize, stretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using an icon font character.
        /// </summary>
        /// <param name="font">The reference to the font.</param>
        /// <param name="character">The icon character.</param>
        /// <param name="fontSize">The font size.</param>
        /// <param name="stretch">The stretch mode for the icon.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateCharacter(ThemeValueReference font, string character, double fontSize, Stretch stretch) => CreateCharacterInternal(font, character, fontSize, stretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using an icon font character.
        /// </summary>
        /// <param name="font">The reference to the font.</param>
        /// <param name="character">The icon character.</param>
        /// <param name="fontSize">The font size.</param>
        /// <param name="stretch">The reference to the stretch mode for the icon.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateCharacter(ThemeValueReference font, string character, double fontSize, ThemeValueReference stretch) => CreateCharacterInternal(font, character, fontSize, stretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using an icon font character.
        /// </summary>
        /// <param name="font">The reference to the font.</param>
        /// <param name="character">The icon character.</param>
        /// <param name="fontSize">The reference to the font size.</param>
        /// <param name="stretch">The stretch mode for the icon.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateCharacter(ThemeValueReference font, string character, ThemeValueReference fontSize, Stretch stretch) => CreateCharacterInternal(font, character, fontSize, stretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using an icon font character.
        /// </summary>
        /// <param name="font">The reference to the font.</param>
        /// <param name="character">The icon character.</param>
        /// <param name="fontSize">The reference to the font size.</param>
        /// <param name="stretch">The reference to the stretch mode for the icon.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateCharacter(ThemeValueReference font, string character, ThemeValueReference fontSize, ThemeValueReference stretch) => CreateCharacterInternal(font, character, fontSize, stretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using an icon font character.
        /// </summary>
        /// <param name="font">The reference to the font.</param>
        /// <param name="character">The reference to the icon character.</param>
        /// <param name="fontSize">The font size.</param>
        /// <param name="stretch">The stretch mode for the icon.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateCharacter(ThemeValueReference font, ThemeValueReference character, double fontSize, Stretch stretch) => CreateCharacterInternal(font, character, fontSize, stretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using an icon font character.
        /// </summary>
        /// <param name="font">The reference to the font.</param>
        /// <param name="character">The reference to the icon character.</param>
        /// <param name="fontSize">The font size.</param>
        /// <param name="stretch">The reference to the stretch mode for the icon.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateCharacter(ThemeValueReference font, ThemeValueReference character, double fontSize, ThemeValueReference stretch) => CreateCharacterInternal(font, character, fontSize, stretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using an icon font character.
        /// </summary>
        /// <param name="font">The reference to the font.</param>
        /// <param name="character">The reference to the icon character.</param>
        /// <param name="fontSize">The reference to the font size.</param>
        /// <param name="stretch">The stretch mode for the icon.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateCharacter(ThemeValueReference font, ThemeValueReference character, ThemeValueReference fontSize, Stretch stretch) => CreateCharacterInternal(font, character, fontSize, stretch);

        /// <summary>
        /// Creates a new <see cref="IconThemeValue"/> using an icon font character.
        /// </summary>
        /// <param name="font">The reference to the font.</param>
        /// <param name="character">The reference to the icon character.</param>
        /// <param name="fontSize">The reference to the font size.</param>
        /// <param name="stretch">The reference to the stretch mode for the icon.</param>
        /// <returns>The created <see cref="IThemeValue"/>.</returns>
        public static IconThemeValue CreateCharacter(ThemeValueReference font, ThemeValueReference character, ThemeValueReference fontSize, ThemeValueReference stretch) => CreateCharacterInternal(font, character, fontSize, stretch);

        private static IconThemeValue CreateCharacterInternal(object font, object character, object fontSize, object stretch)
        {
            return new IconThemeValue
            {
                RawIconType = SymbolType.Character,
                RawFont = font,
                RawCharacter = character,
                RawFontSize = fontSize,
                RawStretch = stretch,
            };
        }

        public static implicit operator Icon(IconThemeValue themeValue) => themeValue.Value;

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is IconThemeValue other &&
                Equals(other.RawIconType, RawIconType) &&
                Equals(other.RawCharacter, RawCharacter) &&
                Equals(other.RawFont?.GetHashCode(), RawFont?.GetHashCode()) &&
                Equals(other.RawFontSize, RawFontSize) &&
                (Equals(other.RawGeometry, RawGeometry) || (other.RawGeometry is Geometry g1 && RawGeometry is Geometry g2 && string.Equals(g1?.ToString(CultureInfo.InvariantCulture), g2?.ToString(CultureInfo.InvariantCulture), StringComparison.OrdinalIgnoreCase))) &&
                Equals(other.RawIsGeometryFilled, RawIsGeometryFilled) &&
                Equals(other.RawGeometryStrokeThickness, RawGeometryStrokeThickness) &&
                Equals(other.RawStretch, RawStretch);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (
                RawIconType,
                RawCharacter,
                RawFont.GetHashCode(),
                RawFontSize,
                RawGeometry is Geometry g ? g.ToString(CultureInfo.InvariantCulture) : RawGeometry,
                RawIsGeometryFilled,
                RawGeometryStrokeThickness,
                RawStretch).GetHashCode();
        }

        /// <inheritdoc/>
        public override object Clone()
        {
            return new IconThemeValue
            {
                Key = Key,
                RawIconType = RawIconType.CloneIfPossible(),
                RawCharacter = RawCharacter.CloneIfPossible(),
                RawFont = RawFont.CloneIfPossible(),
                RawFontSize = RawFontSize.CloneIfPossible(),
                RawGeometry = RawGeometry.CloneIfPossible(),
                RawIsGeometryFilled = RawIsGeometryFilled.CloneIfPossible(),
                RawGeometryStrokeThickness = RawGeometryStrokeThickness.CloneIfPossible(),
                RawStretch = RawStretch.CloneIfPossible(),
            };
        }
    }
}
