using System.Windows;
using System.Windows.Controls.Primitives;

namespace MaSch.Presentation.Wpf.Controls
{
    /// <summary>
    /// Toggle button that has an <see cref="Wpf.Icon"/> as content.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.Primitives.ToggleButton" />
    public class IconToggleButton : ToggleButton
    {
        /// <summary>
        /// Dependency property. Gets or sets the icon.
        /// </summary>
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(
                "Icon",
                typeof(Icon),
                typeof(IconToggleButton),
                new PropertyMetadata(null));

        /// <summary>
        /// Dependency property. Gets or sets the corner radius of the toggle button.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                "CornerRadius",
                typeof(CornerRadius),
                typeof(IconToggleButton),
                new PropertyMetadata(new CornerRadius(0)));

        /// <summary>
        /// Dependency property. Gets or sets the content anchor.
        /// </summary>
        public static readonly DependencyProperty ContentAnchorProperty =
            DependencyProperty.Register(
                "ContentAnchor",
                typeof(AnchorStyle),
                typeof(IconToggleButton),
                new PropertyMetadata(AnchorStyle.None));

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        public Icon Icon
        {
            get => (Icon)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        /// <summary>
        /// Gets or sets the corner radius of the toggle button.
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

        static IconToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IconToggleButton), new FrameworkPropertyMetadata(typeof(IconToggleButton)));
        }
    }
}
