using System;
using System.Linq;
using System.Windows;
using Win = System.Windows.Controls.Primitives;

namespace MaSch.Presentation.Wpf.DependencyProperties
{
    /// <summary>
    /// Provides dependency properties for the <see cref="System.Windows.Controls.Primitives.Popup"/> control.
    /// </summary>
    public static class Popup
    {
        /// <summary>
        /// Dependency property. Gets or sets the relative offset in the horizontal direction.
        /// </summary>
        public static readonly DependencyProperty RelativeHorizontalOffsetProperty =
            DependencyProperty.RegisterAttached(
                "RelativeHorizontalOffset",
                typeof(double),
                typeof(Popup),
                new PropertyMetadata(0D, OnRelativeHorizontalOffsetChanged));

        /// <summary>
        /// Dependency property. Gets or sets the relative offset in the vertical direction.
        /// </summary>
        public static readonly DependencyProperty RelativeVerticalOffsetProperty =
            DependencyProperty.RegisterAttached(
                "RelativeVerticalOffset",
                typeof(double),
                typeof(Popup),
                new PropertyMetadata(0D, OnRelativeVerticalOffsetChanged));

        private static void OnRelativeHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            static void LocationChangedHandler(object? sender, EventArgs ea)
            {
                if (sender is not Win.Popup p)
                    return;
                var source = PresentationSource.CurrentSources.OfType<PresentationSource>().FirstOrDefault();
                var scaleX = source?.CompositionTarget?.TransformToDevice.M11 ?? 1;
                var pPos = p.Child.PointToScreen(new Point(0, 0));
                if ((p.PlacementTarget ?? p.Parent) is not FrameworkElement pt)
                    return;
                var ptPos = pt.PointToScreen(new Point(0, 0));
                var expectedXPos = p.Placement switch
                {
                    Win.PlacementMode.Bottom or Win.PlacementMode.Top => (ptPos.X / scaleX) + p.HorizontalOffset,
                    Win.PlacementMode.Right => (ptPos.X / scaleX) + pt.ActualWidth + p.HorizontalOffset,
                    Win.PlacementMode.Left => (ptPos.X / scaleX) - ((p.Child as FrameworkElement)?.ActualWidth ?? 0D) + p.HorizontalOffset,
                    _ => throw new ArgumentOutOfRangeException(),
                };
                p.HorizontalOffset = Math.Abs((pPos.X / scaleX) - expectedXPos) < 1 ? GetRelativeHorizontalOffset(p) : -GetRelativeHorizontalOffset(p);
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
            static void PopupOpenedHandler(object? sender, EventArgs ea)
            {
                if (sender is not Win.Popup p)
                    return;
                var source = PresentationSource.CurrentSources.OfType<PresentationSource>().FirstOrDefault();
                var scaleY = source?.CompositionTarget?.TransformToDevice.M22 ?? 1;
                var pPos = p.Child.PointToScreen(new Point(0, 0));
                if ((p.PlacementTarget ?? p.Parent) is not FrameworkElement pt)
                    return;
                var ptPos = pt.PointToScreen(new Point(0, 0));
                var expectedYPos = p.Placement switch
                {
                    Win.PlacementMode.Right or Win.PlacementMode.Left => (ptPos.Y / scaleY) + p.VerticalOffset,
                    Win.PlacementMode.Bottom => (ptPos.Y / scaleY) + pt.ActualHeight + p.VerticalOffset,
                    Win.PlacementMode.Top => (ptPos.Y / scaleY) - p.ActualWidth + p.VerticalOffset,
                    _ => throw new ArgumentOutOfRangeException(),
                };
                p.VerticalOffset = Math.Abs((pPos.Y / scaleY) - expectedYPos) < 1 ? GetRelativeVerticalOffset(p) : -GetRelativeVerticalOffset(p);
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

        /// <summary>
        /// Sets the value of the <see cref="RelativeHorizontalOffsetProperty"/>.
        /// </summary>
        /// <param name="obj">The element to set the value to.</param>
        /// <param name="value">The value to set.</param>
        public static void SetRelativeHorizontalOffset(DependencyObject obj, double value)
        {
            obj.SetValue(RelativeHorizontalOffsetProperty, value);
        }

        /// <summary>
        /// Gets the value of the <see cref="RelativeHorizontalOffsetProperty"/>.
        /// </summary>
        /// <param name="obj">The element to get the value from.</param>
        /// <returns>The value of the <see cref="RelativeHorizontalOffsetProperty"/>.</returns>
        public static double GetRelativeHorizontalOffset(DependencyObject obj)
        {
            return (double)obj.GetValue(RelativeHorizontalOffsetProperty);
        }

        /// <summary>
        /// Sets the value of the <see cref="RelativeVerticalOffsetProperty"/>.
        /// </summary>
        /// <param name="obj">The element to set the value to.</param>
        /// <param name="value">The value to set.</param>
        public static void SetRelativeVerticalOffset(DependencyObject obj, double value)
        {
            obj.SetValue(RelativeVerticalOffsetProperty, value);
        }

        /// <summary>
        /// Gets the value of the <see cref="RelativeVerticalOffsetProperty"/>.
        /// </summary>
        /// <param name="obj">The element to get the value from.</param>
        /// <returns>The value of the <see cref="RelativeVerticalOffsetProperty"/>.</returns>
        public static double GetRelativeVerticalOffset(DependencyObject obj)
        {
            return (double)obj.GetValue(RelativeVerticalOffsetProperty);
        }
    }
}
