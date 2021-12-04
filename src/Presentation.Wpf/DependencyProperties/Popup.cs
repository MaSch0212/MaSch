using System.Windows;
using Win = System.Windows.Controls.Primitives;

namespace MaSch.Presentation.Wpf.DependencyProperties;

/// <summary>
/// Provides dependency properties for the <see cref="Win.Popup"/> control.
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

    private static void OnRelativeHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        static void LocationChangedHandler(object? sender, EventArgs ea)
        {
            if (sender is not Win.Popup popup)
                return;
            var source = PresentationSource.CurrentSources.OfType<PresentationSource>().FirstOrDefault();
            var scaleX = source?.CompositionTarget?.TransformToDevice.M11 ?? 1;
            var popupPosition = popup.Child.PointToScreen(new Point(0, 0));
            if ((popup.PlacementTarget ?? popup.Parent) is not FrameworkElement popupOrigin)
                return;
            var popupOriginPosition = popupOrigin.PointToScreen(new Point(0, 0));
            var expectedXPos = popup.Placement switch
            {
                Win.PlacementMode.Bottom or Win.PlacementMode.Top => (popupOriginPosition.X / scaleX) + popup.HorizontalOffset,
                Win.PlacementMode.Right => (popupOriginPosition.X / scaleX) + popupOrigin.ActualWidth + popup.HorizontalOffset,
                Win.PlacementMode.Left => (popupOriginPosition.X / scaleX) - ((popup.Child as FrameworkElement)?.ActualWidth ?? 0D) + popup.HorizontalOffset,
                _ => throw new ArgumentOutOfRangeException($"The placement \"{popup.Placement}\" is unknown."),
            };
            popup.HorizontalOffset = Math.Abs((popupPosition.X / scaleX) - expectedXPos) < 1 ? GetRelativeHorizontalOffset(popup) : -GetRelativeHorizontalOffset(popup);
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
            if (sender is not Win.Popup popup)
                return;
            var source = PresentationSource.CurrentSources.OfType<PresentationSource>().FirstOrDefault();
            var scaleY = source?.CompositionTarget?.TransformToDevice.M22 ?? 1;
            var popupPosition = popup.Child.PointToScreen(new Point(0, 0));
            if ((popup.PlacementTarget ?? popup.Parent) is not FrameworkElement popupOrigin)
                return;
            var popupOriginPosition = popupOrigin.PointToScreen(new Point(0, 0));
            var expectedYPos = popup.Placement switch
            {
                Win.PlacementMode.Right or Win.PlacementMode.Left => (popupOriginPosition.Y / scaleY) + popup.VerticalOffset,
                Win.PlacementMode.Bottom => (popupOriginPosition.Y / scaleY) + popupOrigin.ActualHeight + popup.VerticalOffset,
                Win.PlacementMode.Top => (popupOriginPosition.Y / scaleY) - popup.ActualWidth + popup.VerticalOffset,
                _ => throw new ArgumentOutOfRangeException($"The placement \"{popup.Placement}\" is unknown."),
            };
            popup.VerticalOffset = Math.Abs((popupPosition.Y / scaleY) - expectedYPos) < 1 ? GetRelativeVerticalOffset(popup) : -GetRelativeVerticalOffset(popup);
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
}
