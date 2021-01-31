using System;
using System.Reflection;
using System.Windows;

namespace MaSch.Presentation.Wpf.Extensions
{
    /// <summary>
    /// Provides extensions for the <see cref="FrameworkElement"/> class.
    /// </summary>
    public static class FrameworkElementExtension
    {
        private static readonly EventInfo ResourcesChangedEvent = typeof(FrameworkElement).GetEvent("ResourcesChanged", BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>
        /// Sets the <see cref="FrameworkElement.Width"/> and <see cref="FrameworkElement.Height"/> properties.
        /// </summary>
        /// <param name="element">The element to set the properties on.</param>
        /// <param name="width">The width to set.</param>
        /// <param name="height">The height to set.</param>
        public static void SetSize(this FrameworkElement element, double width, double height)
        {
            element.Width = width;
            element.Height = height;
        }

        /// <summary>
        /// Sets the <see cref="FrameworkElement.HorizontalAlignment"/> and <see cref="FrameworkElement.VerticalAlignment"/> properties.
        /// </summary>
        /// <param name="element">The element to set the properties on.</param>
        /// <param name="horizontal">The horizontal alignment to set.</param>
        /// <param name="vertical">The vertical alignment to set.</param>
        public static void SetAlignment(this FrameworkElement element, HorizontalAlignment horizontal, VerticalAlignment vertical)
        {
            element.HorizontalAlignment = horizontal;
            element.VerticalAlignment = vertical;
        }

        /// <summary>
        /// Sets the <see cref="FrameworkElement.MinWidth"/> and <see cref="FrameworkElement.MinHeight"/> properties.
        /// </summary>
        /// <param name="element">The element to set the properties on.</param>
        /// <param name="width">The minimum width to set.</param>
        /// <param name="height">The minimum height to set.</param>
        public static void SetMinSize(this FrameworkElement element, double width, double height)
        {
            element.MinWidth = width;
            element.MinHeight = height;
        }

        /// <summary>
        /// Subscribes the internal ResourcesChanged event of this <see cref="FrameworkElement"/>.
        /// </summary>
        /// <param name="element">The framework element to subscribe to.</param>
        /// <param name="onResourcesChanged">The delegate to use for the subscription.</param>
        public static void SubscribeResourcesChanged(this FrameworkElement element, EventHandler onResourcesChanged)
        {
            ResourcesChangedEvent.AddMethod.Invoke(element, new object[] { onResourcesChanged });
        }

        /// <summary>
        /// Unsubscribes the internal ResourcesChanged event of this <see cref="FrameworkElement"/>.
        /// </summary>
        /// <param name="element">The framework element to unsubscribe from.</param>
        /// <param name="onResourcesChanged">The delegate that has been subscribes earlier.</param>
        public static void UnsubscribeResourcesChanged(this FrameworkElement element, EventHandler onResourcesChanged)
        {
            ResourcesChangedEvent.RemoveMethod.Invoke(element, new object[] { onResourcesChanged });
        }
    }
}
