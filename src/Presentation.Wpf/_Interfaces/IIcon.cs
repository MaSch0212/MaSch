using System.Windows.Media;

namespace MaSch.Presentation.Wpf;

/// <summary>
/// Providers properties to render an icon in a WPF application.
/// </summary>
public interface IIcon
{
    /// <summary>
    /// Gets or sets the rendering type for this <see cref="IIcon"/>.
    /// </summary>
    SymbolType Type { get; set; }

    /// <summary>
    /// Gets or sets the stretch mode to use for this <see cref="IIcon"/>.
    /// </summary>
    Stretch Stretch { get; set; }

    /// <summary>
    /// Gets or sets a transformation that is applied when rendering this <see cref="IIcon"/>.
    /// </summary>
    Transform? Transform { get; set; }

    #region SymbolType Character

    /// <summary>
    /// Gets or sets the character that represents the <see cref="IIcon"/> in the <see cref="Font"/>.
    /// </summary>
    string? Character { get; set; }

    /// <summary>
    /// Gets or sets the font that contains the <see cref="Character"/> that represents the <see cref="IIcon"/>.
    /// </summary>
    FontFamily? Font { get; set; }

    /// <summary>
    /// Gets or sets the size of the font to use when rendering this <see cref="IIcon"/>.
    /// </summary>
    double FontSize { get; set; }

    #endregion

    #region SymbolType Geometry

    /// <summary>
    /// Gets or sets the geometry that represents this <see cref="IIcon"/>.
    /// </summary>
    Geometry? Geometry { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="Geometry"/> should be filled when rendering this <see cref="IIcon"/>.
    /// </summary>
    bool IsGeometryFilled { get; set; }

    /// <summary>
    /// Gets or sets the stroke thickness of the <see cref="Geometry"/> while rendering this <see cref="IIcon"/>.
    /// </summary>
    double GeometryStrokeThickness { get; set; }

    #endregion
}
