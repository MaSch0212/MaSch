using MaSch.Core;
using System.Windows.Controls;

namespace MaSch.Presentation.Wpf.Controls;

/// <summary>
/// Backing class for <see cref="System.Windows.Controls.ContextMenu"/> styles.
/// </summary>
/// <seealso cref="System.Windows.ResourceDictionary" />
/// <seealso cref="System.Windows.Markup.IComponentConnector" />
public partial class ContextMenu
{
    /// <summary>
    /// The menu item template key.
    /// </summary>
    public static readonly string MenuItemTemplateKey = "MaSch_MenuItemTemplateKey";

    /// <summary>
    /// Initializes a new instance of the <see cref="ContextMenu"/> class.
    /// </summary>
    public ContextMenu()
    {
        InitializeComponent();

        var menuItemTemplate = Guard.NotNull(this[MenuItemTemplateKey], MenuItemTemplateKey);
        this[MenuItem.SubmenuHeaderTemplateKey] = menuItemTemplate;
        this[MenuItem.SubmenuItemTemplateKey] = menuItemTemplate;
    }
}
