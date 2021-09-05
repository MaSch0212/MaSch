using System.Windows;
using System.Windows.Controls;

namespace MaSch.Presentation.Wpf.Controls
{
    /// <summary>
    /// Button that has an <see cref="Wpf.Icon"/> as content.
    /// </summary>
    /// <seealso cref="Button" />
    public class IconButton : Button
    {
        /// <summary>
        /// Dependency property. Gets or sets the icon.
        /// </summary>
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(
                "Icon",
                typeof(Icon),
                typeof(IconButton),
                new PropertyMetadata(null));

        /// <summary>
        /// Dependency property. Gets or sets the corner radius of the button.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                "CornerRadius",
                typeof(CornerRadius),
                typeof(IconButton),
                new PropertyMetadata(new CornerRadius(0)));

        /// <summary>
        /// Dependency property. Gets or sets the content anchor.
        /// </summary>
        public static readonly DependencyProperty ContentAnchorProperty =
            DependencyProperty.Register(
                "ContentAnchor",
                typeof(AnchorStyle),
                typeof(IconButton),
                new PropertyMetadata(AnchorStyle.None));

        static IconButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IconButton), new FrameworkPropertyMetadata(typeof(IconButton)));
        }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        public Icon Icon
        {
            get => (Icon)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        /// <summary>
        /// Gets or sets the corner radius of the button.
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        /// <summary>
        /// Gets or sets the content anchor.
        /// </summary>
        public AnchorStyle ContentAnchor
        {
            get { return (AnchorStyle)GetValue(ContentAnchorProperty); }
            set { SetValue(ContentAnchorProperty, value); }
        }
    }
}
