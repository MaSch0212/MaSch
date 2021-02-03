using System.Windows;
using System.Windows.Controls;

namespace MaSch.Presentation.Wpf.Controls
{
    /// <summary>
    /// Control that displays an <see cref="IIcon"/>.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.Control" />
    public class IconPresenter : Control
    {
        /// <summary>
        /// Dependency property. Gets or sets the icon.
        /// </summary>
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(IIcon), typeof(IconPresenter), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
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
