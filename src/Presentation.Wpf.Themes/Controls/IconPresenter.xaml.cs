using System.Windows;
using System.Windows.Controls;

namespace MaSch.Presentation.Wpf.Controls
{
    public class IconPresenter : Control
    {
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(IIcon), typeof(IconPresenter), new PropertyMetadata(null));

        public IIcon Icon
        {
            get => (IIcon)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        static IconPresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IconPresenter), new FrameworkPropertyMetadata(typeof(IconPresenter)));
        }
    }
}
