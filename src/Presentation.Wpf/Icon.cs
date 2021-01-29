using MaSch.Core.Attributes;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Media;

namespace MaSch.Presentation.Wpf
{
    /// <summary>
    /// Provides observable properties for the <see cref="Icon"/> class.
    /// </summary>
    [ObservablePropertyDefinition]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Property definition interface should be first in file.")]
    internal interface IIcon_Props : IIcon
    {
    }

    /// <summary>
    /// Represents an icon that can be rendered by a WPF application.
    /// </summary>
    [ObservableObject]
    public partial class Icon : IIcon_Props
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Icon"/> class.
        /// </summary>
        public Icon()
        {
            Type = SymbolType.Geometry;
            Geometry = null;
            IsGeometryFilled = true;
            GeometryStrokeThickness = 0;
            Stretch = Stretch.Uniform;
            Font = null;
            Character = null;
            FontSize = 12;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Icon"/> class.
        /// </summary>
        /// <param name="geometry">The geometry to render.</param>
        /// <param name="filled">if set to <c>true</c> the geometry is filled.</param>
        /// <param name="strokeThickness">The stroke thickness for the geometry.</param>
        /// <param name="stretch">The stretch mode to use.</param>
        public Icon(Geometry geometry, bool filled = true, double strokeThickness = 0, Stretch stretch = Stretch.Uniform)
        {
            Type = SymbolType.Geometry;
            Geometry = geometry;
            IsGeometryFilled = filled;
            GeometryStrokeThickness = strokeThickness;
            Stretch = stretch;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Icon"/> class.
        /// </summary>
        /// <param name="font">The font in which the icon is located in.</param>
        /// <param name="character">The character that represents the icon in the <paramref name="font"/>.</param>
        /// <param name="fontSize">Size of the font to use when rendering.</param>
        /// <param name="stretch">The stretch mode to use.</param>
        public Icon(FontFamily font, string character, double fontSize = 12, Stretch stretch = Stretch.Uniform)
        {
            Type = SymbolType.Character;
            Font = font;
            FontSize = fontSize;
            Character = character;
            Stretch = stretch;
        }
    }
}
