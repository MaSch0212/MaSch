using System.Windows;
using MaSch.Core.Attributes;
using MaSch.Core.Observable;

namespace MaSch.Presentation.Wpf.Controls
{
    /// <summary>
    /// Represents a dialog window that is also an <see cref="IObservableObject"/>.
    /// </summary>
    /// <seealso cref="MaSch.Presentation.Wpf.Controls.Window" />
    /// <seealso cref="MaSch.Core.Observable.IObservableObject" />
    [GenerateObservableObject]
    public partial class DialogWindow : Window
    {
        static DialogWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogWindow), new FrameworkPropertyMetadata(typeof(DialogWindow)));
        }
    }
}
