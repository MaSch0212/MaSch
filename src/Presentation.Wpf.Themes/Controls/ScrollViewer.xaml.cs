using MaSch.Presentation.Wpf.Extensions;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Win = System.Windows.Controls;

namespace MaSch.Presentation.Wpf.Controls
{
    /// <summary>
    /// Backing class for styles of the <see cref="Win.ScrollViewer"/>.
    /// </summary>
    /// <seealso cref="ResourceDictionary" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    /// <seealso cref="System.Windows.Markup.IStyleConnector" />
    public partial class ScrollViewer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollViewer"/> class.
        /// </summary>
        public ScrollViewer()
        {
            InitializeComponent();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is Win.ScrollViewer sv && sv.Parent is UIElement parentElement)
            {
                var actualScrollSource = e.OriginalSource as Win.ScrollViewer ??
                    (e.OriginalSource as DependencyObject)?.Parents().OfType<Win.ScrollViewer>().FirstOrDefault(x => x.IsHitTestVisible);

                if (actualScrollSource == sv && ((e.Delta > 0 && Math.Abs(sv.VerticalOffset) < 0.5) || (e.Delta < 0 && Math.Abs(sv.VerticalOffset - sv.ScrollableHeight) < 0.5)))
                {
                    e.Handled = true;

                    var routedArgs = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
                    {
                        RoutedEvent = UIElement.MouseWheelEvent,
                    };
                    parentElement.RaiseEvent(routedArgs);
                }
            }
        }
    }
}
