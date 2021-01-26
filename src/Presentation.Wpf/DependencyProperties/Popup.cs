using System;
using System.Linq;
using System.Windows;
using Win = System.Windows.Controls.Primitives;

namespace MaSch.Presentation.Wpf.DependencyProperties
{
    public static class Popup
    {
        public static readonly DependencyProperty RelativeHorizontalOffsetProperty =
            DependencyProperty.RegisterAttached("RelativeHorizontalOffset", typeof(double), typeof(Popup), new PropertyMetadata(0D, OnRelativeHorizontalOffsetChanged));
        public static readonly DependencyProperty RelativeVerticalOffsetProperty =
            DependencyProperty.RegisterAttached("RelativeVerticalOffset", typeof(double), typeof(Popup), new PropertyMetadata(0D, OnRelativeVerticalOffsetChanged));

        private static void OnRelativeHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            void LocationChangedHandler(object sender, EventArgs ea)
            {
                if (!(sender is Win.Popup p))
                    return;
                var source = PresentationSource.CurrentSources.OfType<PresentationSource>().FirstOrDefault();
                var scaleX = source?.CompositionTarget?.TransformToDevice.M11 ?? 1;
                var pPos = p.Child.PointToScreen(new Point(0, 0));
                if (!((p.PlacementTarget ?? p.Parent) is FrameworkElement pt))
                    return;
                var ptPos = pt.PointToScreen(new Point(0, 0));
                double expectedXPos;
                switch (p.Placement)
                {
                    case Win.PlacementMode.Bottom:
                    case Win.PlacementMode.Top:
                        expectedXPos = ptPos.X / scaleX + p.HorizontalOffset;
                        break;
                    case Win.PlacementMode.Right:
                        expectedXPos = ptPos.X / scaleX + pt.ActualWidth + p.HorizontalOffset;
                        break;
                    case Win.PlacementMode.Left:
                        expectedXPos = ptPos.X / scaleX - ((p.Child as FrameworkElement)?.ActualWidth ?? 0D) + p.HorizontalOffset;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                p.HorizontalOffset = Math.Abs(pPos.X / scaleX - expectedXPos) < 1 ? GetRelativeHorizontalOffset(p) : -GetRelativeHorizontalOffset(p);
            }

            if (d is Win.Popup popup)
            {
                var newDefined = e.NewValue is double newV && Math.Abs(newV) >= 0.0001;
                var oldDefined = e.OldValue is double oldV && Math.Abs(oldV) >= 0.0001;
                if (!oldDefined && newDefined)
                    popup.Opened += LocationChangedHandler;
                if (oldDefined && !newDefined)
                    popup.Opened -= LocationChangedHandler;
                if (!newDefined)
                    popup.HorizontalOffset = 0;
            }
        }

        private static void OnRelativeVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            void PopupOpenedHandler(object sender, EventArgs ea)
            {
                if (!(sender is Win.Popup p))
                    return;
                var source = PresentationSource.CurrentSources.OfType<PresentationSource>().FirstOrDefault();
                var scaleY = source?.CompositionTarget?.TransformToDevice.M22 ?? 1;
                var pPos = p.Child.PointToScreen(new Point(0, 0));
                if (!((p.PlacementTarget ?? p.Parent) is FrameworkElement pt))
                    return;
                var ptPos = pt.PointToScreen(new Point(0, 0));
                double expectedYPos;
                switch (p.Placement)
                {
                    case Win.PlacementMode.Right:
                    case Win.PlacementMode.Left:
                        expectedYPos = ptPos.Y / scaleY + p.VerticalOffset;
                        break;
                    case Win.PlacementMode.Bottom:
                        expectedYPos = ptPos.Y / scaleY + pt.ActualHeight + p.VerticalOffset;
                        break;
                    case Win.PlacementMode.Top:
                        expectedYPos = ptPos.Y / scaleY - p.ActualWidth + p.VerticalOffset;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                p.VerticalOffset = Math.Abs(pPos.Y / scaleY - expectedYPos) < 1 ? GetRelativeVerticalOffset(p) : -GetRelativeVerticalOffset(p);
            }

            if (d is Win.Popup popup)
            {
                var newDefined = e.NewValue is double newV && Math.Abs(newV) >= 0.0001;
                var oldDefined = e.OldValue is double oldV && Math.Abs(oldV) >= 0.0001;
                if (!oldDefined && newDefined)
                    popup.Opened += PopupOpenedHandler;
                if (oldDefined && !newDefined)
                    popup.Opened -= PopupOpenedHandler;
                if (!newDefined)
                    popup.VerticalOffset = 0;
            }
        }

        public static void SetRelativeHorizontalOffset(DependencyObject obj, double value)
        {
            obj.SetValue(RelativeHorizontalOffsetProperty, value);
        }

        public static double GetRelativeHorizontalOffset(DependencyObject obj)
        {
            return (double)obj.GetValue(RelativeHorizontalOffsetProperty);
        }

        public static void SetRelativeVerticalOffset(DependencyObject obj, double value)
        {
            obj.SetValue(RelativeVerticalOffsetProperty, value);
        }

        public static double GetRelativeVerticalOffset(DependencyObject obj)
        {
            return (double)obj.GetValue(RelativeVerticalOffsetProperty);
        }
    }
}
