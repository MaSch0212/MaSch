#pragma warning disable SA1402 // File may only contain a single type

namespace MaSch.Presentation.Wpf.Attributes;

/// <summary>
/// When applied to a class, code for using an icon font is generated that can be used in WPF application.
/// </summary>
/// <seealso cref="Attribute" />
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class WpfIconFontAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WpfIconFontAttribute"/> class.
    /// </summary>
    /// <param name="fontName">Name of the font. The font is expected to be included as a resource to the same assembly as this class. The font name is not the file name, but the actual name of the font.</param>
    /// <param name="cssFileName">Name of the CSS file to generate the IconCode enum from.</param>
    /// <param name="cssClassPrefix">The prefix of the classes in the CSS file.</param>
    /// <param name="extraIconIdStart">The number from which to start generate ids for extra geometries. This should be some value which is not associated with an icon yet in the font.</param>
    public WpfIconFontAttribute(string fontName, string cssFileName, string cssClassPrefix, uint extraIconIdStart)
    {
        FontName = fontName;
        CssFileName = cssFileName;
        CssClassPrefix = cssClassPrefix;
        ExtraIconIdStart = extraIconIdStart;
    }

    /// <summary>
    /// Gets the name of the font. The font is expected to be included as a resource to the same assembly as this class. The font name is not the file name, but the actual name of the font.
    /// </summary>
    public string FontName { get; }

    /// <summary>
    /// Gets the name of the CSS file to generate the IconCode enum from.
    /// </summary>
    public string CssFileName { get; }

    /// <summary>
    /// Gets the prefix of the classes in the CSS file.
    /// </summary>
    public string CssClassPrefix { get; }

    /// <summary>
    /// Gets the number from which to start generate ids for extra geometries. This should be some value which is not associated with an icon yet in the font.
    /// </summary>
    public uint ExtraIconIdStart { get; }

    /// <summary>
    /// Gets or sets the suffix of the classes in the CSS file.
    /// </summary>
    public string? CssClassSuffix { get; set; }

    /// <summary>
    /// Gets or sets the default icon code.
    /// </summary>
    public uint DefaultIconCode { get; set; }
}

/// <summary>
/// Adds extra geometries to a class that has the <see cref="WpfIconFontAttribute"/> defined. The geometry is added to the icons in the icon font.
/// </summary>
/// <seealso cref="Attribute" />
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class WpfIconFontExtraGeometryAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WpfIconFontExtraGeometryAttribute"/> class.
    /// </summary>
    /// <param name="name">The name of the icon.</param>
    /// <param name="geometryPath">The geometry path.</param>
    public WpfIconFontExtraGeometryAttribute(string name, string geometryPath)
    {
        Name = name;
        GeometryPath = geometryPath;
    }

    /// <summary>
    /// Gets the name of the icon.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the geometry path.
    /// </summary>
    public string GeometryPath { get; }
}
