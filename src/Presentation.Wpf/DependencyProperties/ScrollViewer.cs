using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MaSch.Core;
using MaSch.Presentation.Wpf.Extensions;
using Win = System.Windows.Controls;

namespace MaSch.Presentation.Wpf.DependencyProperties
{
    public static class ScrollViewer
    {
        #region VerticalOffset attached property

        public static double GetVerticalOffset(DependencyObject depObj)
        {
            return (double)depObj.GetValue(VerticalOffsetProperty);
        }

        public static void SetVerticalOffset(DependencyObject depObj, double value)
        {
            depObj.SetValue(VerticalOffsetProperty, value);
        }

        public static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.RegisterAttached("VerticalOffset", typeof(double),
            typeof(ScrollViewer), new PropertyMetadata(0.0, OnVerticalOffsetPropertyChanged));

        private static void OnVerticalOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Win.ScrollViewer sv)
            {
                sv.ScrollToVerticalOffset((double)e.NewValue);
            }
        }

        #endregion

        #region HorizontalOffset attached property

        public static double GetHorizontalOffset(DependencyObject depObj)
        {
            return (double)depObj.GetValue(HorizontalOffsetProperty);
        }

        public static void SetHorizontalOffset(DependencyObject depObj, double value)
        {
            depObj.SetValue(HorizontalOffsetProperty, value);
        }

        public static readonly DependencyProperty HorizontalOffsetProperty =
            DependencyProperty.RegisterAttached("HorizontalOffset", typeof(double),
            typeof(ScrollViewer), new PropertyMetadata(0.0, OnHorizontalOffsetPropertyChanged));

        private static void OnHorizontalOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Win.ScrollViewer sv)
            {
                sv.ScrollToHorizontalOffset((double)e.NewValue);
            }
        }

        #endregion

        #region SychronizedScrollViewer attached property

        public static Win.ScrollViewer GetSynchronizedScrollViewer(DependencyObject depObj)
        {
            return (Win.ScrollViewer)depObj.GetValue(SynchronizedScrollViewerProperty);
        }

        public static void SetSynchronizedScrollViewer(DependencyObject depObj, Win.ScrollViewer value)
        {
            depObj.SetValue(SynchronizedScrollViewerProperty, value);
        }

        public static readonly DependencyProperty SynchronizedScrollViewerProperty =
            DependencyProperty.RegisterAttached("SynchronizedScrollViewer", typeof(Win.ScrollViewer),
            typeof(ScrollViewer), new PropertyMetadata(null, OnSynchronizedScrollViewerChanged));
        
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
                            RoutedEvent = UIElement.MouseWheelEvent
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

        #endregion

        #region ScrollOnDragDrop attached property

        public static readonly DependencyProperty ScrollOnDragDropProperty =
            DependencyProperty.RegisterAttached("ScrollOnDragDrop",
                typeof(bool),
                typeof(ScrollViewer),
                new PropertyMetadata(false, HandleScrollOnDragDropChanged));
 
        public static bool GetScrollOnDragDrop(DependencyObject element)
        {
            Guard.NotNull(element, nameof(element));
            return (bool)element.GetValue(ScrollOnDragDropProperty);
        }

        public static void SetScrollOnDragDrop(DependencyObject element, bool value)
        {
            Guard.NotNull(element, nameof(element));
            element.SetValue(ScrollOnDragDropProperty, value);
        }

        private static void HandleScrollOnDragDropChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var container = d as FrameworkElement;
            
            Unsubscribe(container);
            if (true.Equals(e.NewValue))
                Subscribe(container);
        }

        private static void Subscribe(FrameworkElement container)
        {
            Guard.NotNull(container, nameof(container));
            container.PreviewDragOver += OnContainerPreviewDragOver;
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

            if (verticalPos < tolerance) // Top of visible list? 
            {
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - offset); //Scroll up. 
            }
            else if (verticalPos > scrollViewer.ActualHeight - tolerance) //Bottom of visible list? 
            {
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + offset); //Scroll down.     
            }
        }

        private static void Unsubscribe(FrameworkElement container)
        {
            Guard.NotNull(container, nameof(container));
            container.PreviewDragOver -= OnContainerPreviewDragOver;
        }

        #endregion

        public static readonly DependencyProperty VerticalScrollBarMarginProperty =
            DependencyProperty.RegisterAttached("VerticalScrollBarMargin", typeof(Thickness), typeof(ScrollViewer),
                new FrameworkPropertyMetadata(new Thickness(0), FrameworkPropertyMetadataOptions.Inherits));
        public static void SetVerticalScrollBarMargin(DependencyObject target, Thickness value)
        {
            target.SetValue(VerticalScrollBarMarginProperty, value);
        }
        public static Thickness GetVerticalScrollBarMargin(DependencyObject target)
        {
            return (Thickness)target.GetValue(VerticalScrollBarMarginProperty);
        }

        public static readonly DependencyProperty HorizontalScrollBarMarginProperty =
            DependencyProperty.RegisterAttached("HorizontalScrollBarMargin", typeof(Thickness), typeof(ScrollViewer),
                new FrameworkPropertyMetadata(new Thickness(0), FrameworkPropertyMetadataOptions.Inherits));
        public static void SetHorizontalScrollBarMargin(DependencyObject target, Thickness value)
        {
            target.SetValue(HorizontalScrollBarMarginProperty, value);
        }
        public static Thickness GetHorizontalScrollBarMargin(DependencyObject target)
        {
            return (Thickness)target.GetValue(HorizontalScrollBarMarginProperty);
        }

        public static readonly DependencyProperty IsVerticalScrollBarOverlayingProperty =
            DependencyProperty.RegisterAttached("IsVerticalScrollBarOverlaying", typeof(bool), typeof(ScrollViewer),
                new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.Inherits));
        public static void SetIsVerticalScrollBarOverlaying(DependencyObject target, bool value)
        {
            target.SetValue(IsVerticalScrollBarOverlayingProperty, value);
        }
        public static bool GetIsVerticalScrollBarOverlaying(DependencyObject target)
        {
            return (bool)target.GetValue(IsVerticalScrollBarOverlayingProperty);
        }

        public static readonly DependencyProperty IsHorizontalScrollBarOverlayingProperty =
            DependencyProperty.RegisterAttached("IsHorizontalScrollBarOverlaying", typeof(bool), typeof(ScrollViewer),
                new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.Inherits));
        public static void SetIsHorizontalScrollBarOverlaying(DependencyObject target, bool value)
        {
            target.SetValue(IsHorizontalScrollBarOverlayingProperty, value);
        }
        public static bool GetIsHorizontalScrollBarOverlaying(DependencyObject target)
        {
            return (bool)target.GetValue(IsHorizontalScrollBarOverlayingProperty);
        }
    }
}
