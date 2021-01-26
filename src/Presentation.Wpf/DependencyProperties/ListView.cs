using System.Windows;

namespace MaSch.Presentation.Wpf.DependencyProperties
{
    public static class ListView
    {
        public static readonly DependencyProperty AllowResizingProperty =
            DependencyProperty.RegisterAttached("AllowResizing", typeof(bool), typeof(ListView),
            new PropertyMetadata(true));
        public static void SetAllowResizing(DependencyObject target, bool value)
        {
            target.SetValue(AllowResizingProperty, value);
        }
        public static bool GetAllowResizing(DependencyObject target)
        {
            return (bool)target.GetValue(AllowResizingProperty);
        }
    }
}
