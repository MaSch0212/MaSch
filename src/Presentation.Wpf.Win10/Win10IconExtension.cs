using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace MaSch.Presentation.Wpf.Win10;

/// <summary>
/// Markup extension that creates an <see cref="Icon"/> using a Windows 10 icon.
/// </summary>
/// <seealso cref="MarkupExtension" />
[MarkupExtensionReturnType(typeof(Icon))]
public class Win10IconExtension : MarkupExtension
{
    /// <summary>
    /// The font family to use for the <see cref="Win10IconExtension"/> class.
    /// </summary>
    public static readonly FontFamily FontFamily;

    [SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "Hardcoded URI to internal resource should be fine.")]
    static Win10IconExtension()
    {
        if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
        {
            FontFamily = new FontFamily("Segoe MDL2 Assets");
        }
        else
        {
            FontFamily = Application.Current != null
                ? new FontFamily(new Uri("pack://application:,,,/MaSch.Presentation.Wpf.Win10;component/"), "./#Segoe MDL2 Assets")
                : new FontFamily("Segoe MDL2 Assets");
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Win10IconExtension"/> class.
    /// </summary>
    public Win10IconExtension()
        : this(Win10IconCode.Close)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Win10IconExtension"/> class.
    /// </summary>
    /// <param name="symbol">The symbol to use.</param>
    public Win10IconExtension(Win10IconCode symbol)
    {
        Symbol = symbol;
    }

    /// <summary>
    /// Gets or sets the symbol.
    /// </summary>
    [ConstructorArgument("symbol")]
    public Win10IconCode Symbol { get; set; }

    /// <summary>
    /// Gets or sets the size of the font.
    /// </summary>
    public double FontSize { get; set; } = 12;

    /// <summary>
    /// Gets or sets the stretch mode.
    /// </summary>
    public Stretch Stretch { get; set; } = Stretch.Uniform;

    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var pvt = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
        if (pvt?.TargetObject is Setter)
            return this;
        return new Icon(FontFamily, Symbol.GetChar(), FontSize, Stretch);
    }
}
