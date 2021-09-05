using MaSch.Core.Attributes;
using MaSch.Core.Observable;
using System.Windows;

namespace MaSch.Presentation.Wpf.Controls
{
    /// <summary>
    /// Represents a dialog window that is also an <see cref="IObservableObject"/>.
    /// </summary>
    /// <seealso cref="Window" />
    /// <seealso cref="IObservableObject" />
    [GenerateObservableObject]
    public partial class DialogWindow : Window
    {
        static DialogWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogWindow), new FrameworkPropertyMetadata(typeof(DialogWindow)));
        }
    }
}
