using System.Windows;
using System.Windows.Controls;

namespace MaSch.Presentation.Wpf.Controls
{
    public class IconButton : Button
    {
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(
                "Icon",
                typeof(Icon),
                typeof(IconButton),
                new PropertyMetadata(null));

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                "CornerRadius",
                typeof(CornerRadius),
                typeof(IconButton),
                new PropertyMetadata(new CornerRadius(0)));

        public static readonly DependencyProperty ContentAnchorProperty =
            DependencyProperty.Register(
                "ContentAnchor",
                typeof(AnchorStyle),
                typeof(IconButton),
                new PropertyMetadata(AnchorStyle.None));

        public Icon Icon
        {
            get => (Icon)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public AnchorStyle ContentAnchor
        {
            get { return (AnchorStyle)GetValue(ContentAnchorProperty); }
            set { SetValue(ContentAnchorProperty, value); }
        }

        static IconButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IconButton), new FrameworkPropertyMetadata(typeof(IconButton)));
        }
    }
}
