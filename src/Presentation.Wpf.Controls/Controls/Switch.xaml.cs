using System.Windows;
using System.Windows.Controls;

namespace MaSch.Presentation.Wpf.Controls
{
    public class Switch : CheckBox
    {
        static Switch()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Switch), new FrameworkPropertyMetadata(typeof(Switch)));
        }
    }
}
