using System;
using System.Reflection;
using System.Windows;

namespace MaSch.Presentation.Wpf.Extensions
{
    public static class FrameworkElementExtension
    {
        private static readonly EventInfo ResourcesChangedEvent = typeof(FrameworkElement).GetEvent("ResourcesChanged", BindingFlags.Instance | BindingFlags.NonPublic);

        public static void SetSize(this FrameworkElement element, double width, double height)
        {
            element.Width = width;
            element.Height = height;
        }

        public static void SetAlignment(this FrameworkElement element, HorizontalAlignment horizontal, VerticalAlignment vertical)
        {
            element.HorizontalAlignment = horizontal;
            element.VerticalAlignment = vertical;
        }

        public static void SetMinSize(this FrameworkElement element, double width, double height)
        {
            element.MinWidth = width;
            element.MinHeight = height;
        }

        public static void SubscribeResourcesChanged(this FrameworkElement element, EventHandler onResourcesChanged)
        {
            ResourcesChangedEvent.AddMethod.Invoke(element, new object[] { onResourcesChanged });
        }

        public static void UnsubscribeResourcesChanged(this FrameworkElement element, EventHandler onResourcesChanged)
        {
            ResourcesChangedEvent.RemoveMethod.Invoke(element, new object[] { onResourcesChanged });
        }
    }
}
