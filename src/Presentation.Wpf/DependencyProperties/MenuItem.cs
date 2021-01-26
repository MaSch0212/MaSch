using System.Windows;

namespace MaSch.Presentation.Wpf.DependencyProperties
{
    public class MenuItem
    {
        public static readonly DependencyProperty IsCheckedNullProperty =
            DependencyProperty.RegisterAttached("IsCheckedNull", typeof(bool), typeof(MenuItem),
            new PropertyMetadata(false));
        public static void SetIsCheckedNull(DependencyObject target, bool value)
        {
            target.SetValue(IsCheckedNullProperty, value);
        }
        public static bool GetIsCheckedNull(DependencyObject target)
        {
            return (bool)target.GetValue(IsCheckedNullProperty);
        }
    }
}
