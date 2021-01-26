using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MaSch.Presentation.Wpf.DependencyProperties
{
    public static class Expander
    {
        public static readonly DependencyProperty HeaderPaddingProperty =
            DependencyProperty.RegisterAttached("HeaderPadding", typeof(Thickness), typeof(Expander),
            new PropertyMetadata(new Thickness(0)));
        public static void SetHeaderPadding(DependencyObject target, Thickness value)
        {
            target.SetValue(HeaderPaddingProperty, value);
        }
        public static Thickness GetHeaderPadding(DependencyObject target)
        {
            return (Thickness)target.GetValue(HeaderPaddingProperty);
        }
    }
}
