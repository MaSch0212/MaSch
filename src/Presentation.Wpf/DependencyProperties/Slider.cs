using System.Windows;
using System.Windows.Media;

namespace MaSch.Presentation.Wpf.DependencyProperties
{
    public static class Slider
    {
        public static readonly DependencyProperty LowerLineBrushProperty =
            DependencyProperty.RegisterAttached("LowerLineBrush", typeof(Brush), typeof(Slider),
            new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));
        public static void SetLowerLineBrush(DependencyObject target, Brush value)
        {
            target.SetValue(LowerLineBrushProperty, value);
        }
        public static Brush GetLowerLineBrush(DependencyObject target)
        {
            return (Brush)target.GetValue(LowerLineBrushProperty);
        }

        public static readonly DependencyProperty GreaterLineBrushProperty =
            DependencyProperty.RegisterAttached("GreaterLineBrush", typeof(Brush), typeof(Slider),
            new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));
        public static void SetGreaterLineBrush(DependencyObject target, Brush value)
        {
            target.SetValue(GreaterLineBrushProperty, value);
        }
        public static Brush GetGreaterLineBrush(DependencyObject target)
        {
            return (Brush)target.GetValue(GreaterLineBrushProperty);
        }
    }
}
