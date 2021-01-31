using System.Windows;
using System.Windows.Controls.Primitives;

namespace MaSch.Presentation.Wpf.Controls
{
    public class IconToggleButton : ToggleButton
    {
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(
                "Icon",
                typeof(Icon),
                typeof(IconToggleButton),
                new PropertyMetadata(null));

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                "CornerRadius",
                typeof(CornerRadius),
                typeof(IconToggleButton),
                new PropertyMetadata(new CornerRadius(0)));

        public static readonly DependencyProperty ContentAnchorProperty =
            DependencyProperty.Register(
                "ContentAnchor",
                typeof(AnchorStyle),
                typeof(IconToggleButton),
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

        static IconToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IconToggleButton), new FrameworkPropertyMetadata(typeof(IconToggleButton)));
        }
    }
}
