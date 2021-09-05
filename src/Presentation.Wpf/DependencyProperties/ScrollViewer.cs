using MaSch.Core;
using MaSch.Presentation.Wpf.Extensions;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Win = System.Windows.Controls;

namespace MaSch.Presentation.Wpf.DependencyProperties
{
    /// <summary>
    /// Provides dependency properties for the <see cref="Win.ScrollViewer"/> control.
    /// </summary>
    public static class ScrollViewer
    {
        /// <summary>
        /// Dependency property. Gets or sets the vertical offset.
        /// </summary>
        public static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.RegisterAttached(
                "VerticalOffset",
                typeof(double),
                typeof(ScrollViewer),
                new PropertyMetadata(0.0, OnVerticalOffsetPropertyChanged));

        /// <summary>
        /// Dependency property. Gets or sets the horizontal offset.
        /// </summary>
        public static readonly DependencyProperty HorizontalOffsetProperty =
            DependencyProperty.RegisterAttached(
                "HorizontalOffset",
                typeof(double),
                typeof(ScrollViewer),
                new PropertyMetadata(0.0, OnHorizontalOffsetPropertyChanged));

        /// <summary>
        /// Dependency property. Gets or sets a scroll viewer that should use the same scroll position as the assigned one.
        /// </summary>
        public static readonly DependencyProperty SynchronizedScrollViewerProperty =
            DependencyProperty.RegisterAttached(
                "SynchronizedScrollViewer",
                typeof(Win.ScrollViewer),
                typeof(ScrollViewer),
                new PropertyMetadata(null, OnSynchronizedScrollViewerChanged));

        /// <summary>
        /// Dependency property. Gets or sets a value indicating whether the scroll viewer should scroll during drag and drop.
        /// </summary>
        public static readonly DependencyProperty ScrollOnDragDropProperty =
            DependencyProperty.RegisterAttached(
                "ScrollOnDragDrop",
                typeof(bool),
                typeof(ScrollViewer),
                new PropertyMetadata(false, OnScrollOnDragDropPropertyChanged));

        /// <summary>
        /// Dependency property. Gets or sets the margin of the vertifcal scroll bar.
        /// </summary>
        public static readonly DependencyProperty VerticalScrollBarMarginProperty =
            DependencyProperty.RegisterAttached(
                "VerticalScrollBarMargin",
                typeof(Thickness),
                typeof(ScrollViewer),
                new FrameworkPropertyMetadata(new Thickness(0), FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// Dependency property. Gets or sets the margin of the horizontal scroll bar.
        /// </summary>
        public static readonly DependencyProperty HorizontalScrollBarMarginProperty =
            DependencyProperty.RegisterAttached(
                "HorizontalScrollBarMargin",
                typeof(Thickness),
                typeof(ScrollViewer),
                new FrameworkPropertyMetadata(new Thickness(0), FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// Dependency property. Gets or sets a value indicating whether the vertical scroll bar should overlay the content.
        /// </summary>
        public static readonly DependencyProperty IsVerticalScrollBarOverlayingProperty =
            DependencyProperty.RegisterAttached(
                "IsVerticalScrollBarOverlaying",
                typeof(bool),
                typeof(ScrollViewer),
                new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// Dependency property. Gets or sets a value indicating whether the horizontal scroll bar should overlay the content.
        /// </summary>
        public static readonly DependencyProperty IsHorizontalScrollBarOverlayingProperty =
            DependencyProperty.RegisterAttached(
                "IsHorizontalScrollBarOverlaying",
                typeof(bool),
                typeof(ScrollViewer),
                new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// Gets the value of the <see cref="VerticalOffsetProperty"/>.
        /// </summary>
        /// <param name="depObj">The element to get the value from.</param>
        /// <returns>The value of the <see cref="VerticalOffsetProperty"/>.</returns>
        public static double GetVerticalOffset(DependencyObject depObj)
        {
            return (double)depObj.GetValue(VerticalOffsetProperty);
        }

        /// <summary>
        /// Sets the value of the <see cref="VerticalOffsetProperty"/>.
        /// </summary>
        /// <param name="depObj">The element to set the value to.</param>
        /// <param name="value">The value to set.</param>
        public static void SetVerticalOffset(DependencyObject depObj, double value)
        {
            depObj.SetValue(VerticalOffsetProperty, value);
        }

        /// <summary>
        /// Gets the value of the <see cref="HorizontalOffsetProperty"/>.
        /// </summary>
        /// <param name="depObj">The element to get the value from.</param>
        /// <returns>The value of the <see cref="HorizontalOffsetProperty"/>.</returns>
        public static double GetHorizontalOffset(DependencyObject depObj)
        {
            return (double)depObj.GetValue(HorizontalOffsetProperty);
        }

        /// <summary>
        /// Sets the value of the <see cref="HorizontalOffsetProperty"/>.
        /// </summary>
        /// <param name="depObj">The element to set the value to.</param>
        /// <param name="value">The value to set.</param>
        public static void SetHorizontalOffset(DependencyObject depObj, double value)
        {
            depObj.SetValue(HorizontalOffsetProperty, value);
        }

        /// <summary>
        /// Gets the value of the <see cref="SynchronizedScrollViewerProperty"/>.
        /// </summary>
        /// <param name="depObj">The element to get the value from.</param>
        /// <returns>The value of the <see cref="SynchronizedScrollViewerProperty"/>.</returns>
        public static Win.ScrollViewer GetSynchronizedScrollViewer(DependencyObject depObj)
        {
            return (Win.ScrollViewer)depObj.GetValue(SynchronizedScrollViewerProperty);
        }

        /// <summary>
        /// Sets the value of the <see cref="SynchronizedScrollViewerProperty"/>.
        /// </summary>
        /// <param name="depObj">The element to set the value to.</param>
        /// <param name="value">The value to set.</param>
        public static void SetSynchronizedScrollViewer(DependencyObject depObj, Win.ScrollViewer value)
        {
            depObj.SetValue(SynchronizedScrollViewerProperty, value);
        }

        /// <summary>
        /// Gets the value of the <see cref="ScrollOnDragDropProperty"/>.
        /// </summary>
        /// <param name="element">The element to get the value from.</param>
        /// <returns>The value of the <see cref="ScrollOnDragDropProperty"/>.</returns>
        public static bool GetScrollOnDragDrop(DependencyObject element)
        {
            _ = Guard.NotNull(element, nameof(element));
            return (bool)element.GetValue(ScrollOnDragDropProperty);
        }

        /// <summary>
        /// Sets the value of the <see cref="ScrollOnDragDropProperty"/>.
        /// </summary>
        /// <param name="element">The element to set the value to.</param>
        /// <param name="value">The value to set.</param>
        public static void SetScrollOnDragDrop(DependencyObject element, bool value)
        {
            _ = Guard.NotNull(element, nameof(element));
            element.SetValue(ScrollOnDragDropProperty, value);
        }

        /// <summary>
        /// Sets the value of the <see cref="VerticalScrollBarMarginProperty"/>.
        /// </summary>
        /// <param name="target">The element to set the value to.</param>
        /// <param name="value">The value to set.</param>
        public static void SetVerticalScrollBarMargin(DependencyObject target, Thickness value)
        {
            target.SetValue(VerticalScrollBarMarginProperty, value);
        }

        /// <summary>
        /// Gets the value of the <see cref="VerticalScrollBarMarginProperty"/>.
        /// </summary>
        /// <param name="target">The element to get the value from.</param>
        /// <returns>The value of the <see cref="VerticalScrollBarMarginProperty"/>.</returns>
        public static Thickness GetVerticalScrollBarMargin(DependencyObject target)
        {
            return (Thickness)target.GetValue(VerticalScrollBarMarginProperty);
        }

        /// <summary>
        /// Sets the value of the <see cref="HorizontalScrollBarMarginProperty"/>.
        /// </summary>
        /// <param name="target">The element to set the value to.</param>
        /// <param name="value">The value to set.</param>
        public static void SetHorizontalScrollBarMargin(DependencyObject target, Thickness value)
        {
            target.SetValue(HorizontalScrollBarMarginProperty, value);
        }

        /// <summary>
        /// Gets the value of the <see cref="HorizontalScrollBarMarginProperty"/>.
        /// </summary>
        /// <param name="target">The element to get the value from.</param>
        /// <returns>The value of the <see cref="HorizontalScrollBarMarginProperty"/>.</returns>
        public static Thickness GetHorizontalScrollBarMargin(DependencyObject target)
        {
            return (Thickness)target.GetValue(HorizontalScrollBarMarginProperty);
        }

        /// <summary>
        /// Sets the value of the <see cref="IsVerticalScrollBarOverlayingProperty"/>.
        /// </summary>
        /// <param name="target">The element to set the value to.</param>
        /// <param name="value">The value to set.</param>
        public static void SetIsVerticalScrollBarOverlaying(DependencyObject target, bool value)
        {
            target.SetValue(IsVerticalScrollBarOverlayingProperty, value);
        }

        /// <summary>
        /// Gets the value of the <see cref="IsVerticalScrollBarOverlayingProperty"/>.
        /// </summary>
        /// <param name="target">The element to get the value from.</param>
        /// <returns>The value of the <see cref="IsVerticalScrollBarOverlayingProperty"/>.</returns>
        public static bool GetIsVerticalScrollBarOverlaying(DependencyObject target)
        {
            return (bool)target.GetValue(IsVerticalScrollBarOverlayingProperty);
        }

        /// <summary>
        /// Sets the value of the <see cref="IsHorizontalScrollBarOverlayingProperty"/>.
        /// </summary>
        /// <param name="target">The element to set the value to.</param>
        /// <param name="value">The value to set.</param>
        public static void SetIsHorizontalScrollBarOverlaying(DependencyObject target, bool value)
        {
            target.SetValue(IsHorizontalScrollBarOverlayingProperty, value);
        }

        /// <summary>
        /// Gets the value of the <see cref="IsHorizontalScrollBarOverlayingProperty"/>.
        /// </summary>
        /// <param name="target">The element to get the value from.</param>
        /// <returns>The value of the <see cref="IsHorizontalScrollBarOverlayingProperty"/>.</returns>
        public static bool GetIsHorizontalScrollBarOverlaying(DependencyObject target)
        {
            return (bool)target.GetValue(IsHorizontalScrollBarOverlayingProperty);
        }

        private static void OnVerticalOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Win.ScrollViewer sv)
            {
                sv.ScrollToVerticalOffset((double)e.NewValue);
            }
        }

        private static void OnHorizontalOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Win.ScrollViewer sv)
            {
                sv.ScrollToHorizontalOffset((double)e.NewValue);
            }
        }

        private static void OnSynchronizedScrollViewerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Win.ScrollViewer sv)
            {
                void ScrollChangedEventHandler(object s, ScrollChangedEventArgs ea)
                {
                    if (s is Win.ScrollViewer ssv)
                    {
                        if (sv.HorizontalScrollBarVisibility != ScrollBarVisibility.Disabled && Math.Abs(ssv.HorizontalOffset - sv.HorizontalOffset) > 0.0001)
                            sv.ScrollToHorizontalOffset(ssv.HorizontalOffset);
                        if (sv.VerticalScrollBarVisibility != ScrollBarVisibility.Disabled && Math.Abs(ssv.VerticalOffset - sv.VerticalOffset) > 0.0001)
                            sv.ScrollToVerticalOffset(ssv.VerticalOffset);
                    }
                }

                void PreviewMouseWheelEventHandler(object s, MouseWheelEventArgs ea)
                {
                    if (s is Win.ScrollViewer ssv && ssv.VerticalScrollBarVisibility != ScrollBarVisibility.Disabled)
                    {
                        var sssv = GetSynchronizedScrollViewer(ssv);
                        if (sssv == null)
                            return;

                        ea.Handled = true;
                        var routedArgs = new MouseWheelEventArgs(ea.MouseDevice, ea.Timestamp, ea.Delta)
                        {
                            RoutedEvent = UIElement.MouseWheelEvent,
                        };
                        sssv.RaiseEvent(routedArgs);
                    }
                }

                if (e.OldValue is Win.ScrollViewer oldSv)
                {
                    oldSv.ScrollChanged -= ScrollChangedEventHandler;
                    sv.PreviewMouseWheel -= PreviewMouseWheelEventHandler;
                }

                if (e.NewValue is Win.ScrollViewer newSv)
                {
                    newSv.ScrollChanged += ScrollChangedEventHandler;
                    sv.PreviewMouseWheel += PreviewMouseWheelEventHandler;
                }
            }
        }

        private static void OnScrollOnDragDropPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not FrameworkElement container)
                return;

            Unsubscribe(container);
            if (true.Equals(e.NewValue))
                Subscribe(container);
        }

        private static void Subscribe(FrameworkElement container)
        {
            _ = Guard.NotNull(container, nameof(container));
            container.PreviewDragOver += OnContainerPreviewDragOver;
        }

        private static void Unsubscribe(FrameworkElement container)
        {
            _ = Guard.NotNull(container, nameof(container));
            container.PreviewDragOver -= OnContainerPreviewDragOver;
        }

        private static void OnContainerPreviewDragOver(object sender, DragEventArgs e)
        {
            var scrollViewer = sender as Win.ScrollViewer ??
                (sender as DependencyObject)?.Parents().OfType<Win.ScrollViewer>().FirstOrDefault();
            if (scrollViewer == null)
                return;

            const double tolerance = 60;
            const double offset = 10;
            double verticalPos = e.GetPosition(scrollViewer).Y;

            // Top of visible list?
            if (verticalPos < tolerance)
            {
                // Scroll up.
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - offset);
            }

            // Bottom of visible list?
            else if (verticalPos > scrollViewer.ActualHeight - tolerance)
            {
                // Scroll down.
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + offset);
            }
        }
    }
}
