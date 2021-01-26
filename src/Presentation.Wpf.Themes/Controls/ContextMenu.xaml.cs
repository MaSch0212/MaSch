using MaSch.Core;
using System.Windows;
using System.Windows.Controls;

namespace MaSch.Presentation.Wpf.Controls
{
    public partial class ContextMenu
    {
        public static readonly string MenuItemTemplateKey = "MaSch_MenuItemTemplateKey";

        public ContextMenu()
        {
            InitializeComponent();

            var menuItemTemplate = Guard.NotNull(this[MenuItemTemplateKey], MenuItemTemplateKey);
            this[MenuItem.SubmenuHeaderTemplateKey] = menuItemTemplate;
            this[MenuItem.SubmenuItemTemplateKey] = menuItemTemplate;
        }
    }
}
