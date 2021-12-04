using System.Windows;
using System.Windows.Controls;

namespace MaSch.Presentation.Wpf.Controls;

/// <summary>
/// Check box that is displayed like a switch.
/// </summary>
/// <seealso cref="CheckBox" />
public class Switch : CheckBox
{
    static Switch()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Switch), new FrameworkPropertyMetadata(typeof(Switch)));
    }
}
