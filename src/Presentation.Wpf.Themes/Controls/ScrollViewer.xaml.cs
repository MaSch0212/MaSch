using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MaSch.Presentation.Wpf.Extensions;
using Win = System.Windows.Controls;

namespace MaSch.Presentation.Wpf.Controls
{
    public partial class ScrollViewer
    {
        public ScrollViewer()
        {
            InitializeComponent();
        }

        [SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is Win.ScrollViewer sv && sv.Parent is UIElement parentElement)
            {
                var actualScrollSource = e.OriginalSource as Win.ScrollViewer ??
                    (e.OriginalSource as DependencyObject).Parents().OfType<Win.ScrollViewer>().FirstOrDefault(x => x.IsHitTestVisible);

                if (actualScrollSource == sv && (e.Delta > 0 && Math.Abs(sv.VerticalOffset) < 0.5 ||
                    e.Delta < 0 && Math.Abs(sv.VerticalOffset - sv.ScrollableHeight) < 0.5))
                {
                    e.Handled = true;

                    var routedArgs = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
                    {
                        RoutedEvent = UIElement.MouseWheelEvent
                    };
                    parentElement.RaiseEvent(routedArgs);
                }
            }
        }
    }
}
