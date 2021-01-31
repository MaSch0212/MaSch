using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MaSch.Presentation.Wpf.Controls
{
    public class DefaultTileContent : Control
    {
        public static readonly DependencyProperty TileSymbolControlProperty =
            DependencyProperty.Register(
                "TileSymbolControl",
                typeof(FrameworkElement),
                typeof(DefaultTileContent),
                new PropertyMetadata(null));

        public static readonly DependencyProperty TileTitleProperty =
            DependencyProperty.Register(
                "TileTitle",
                typeof(string),
                typeof(DefaultTileContent),
                new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty TileSymbolFillProperty =
            DependencyProperty.Register(
                "TileSymbolFill",
                typeof(Brush),
                typeof(DefaultTileContent),
                new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public static readonly DependencyProperty TileSymbolGeometryProperty =
            DependencyProperty.Register(
                "TileSymbolGeometry",
                typeof(Geometry),
                typeof(DefaultTileContent),
                new PropertyMetadata(null));

        public static readonly DependencyProperty TileImageProperty =
            DependencyProperty.Register(
                "TileImage",
                typeof(ImageSource),
                typeof(DefaultTileContent),
                new PropertyMetadata(null));

        private Control _innerBorder;
        private Control _title;
        private bool _isInitialized;

        public ImageSource TileImage
        {
            get => (ImageSource)GetValue(TileImageProperty);
            set => SetValue(TileImageProperty, value);
        }

        public Geometry TileSymbolGeometry
        {
            get => (Geometry)GetValue(TileSymbolGeometryProperty);
            set => SetValue(TileSymbolGeometryProperty, value);
        }

        public Brush TileSymbolFill
        {
            get => (Brush)GetValue(TileSymbolFillProperty);
            set => SetValue(TileSymbolFillProperty, value);
        }

        public string TileTitle
        {
            get => (string)GetValue(TileTitleProperty);
            set => SetValue(TileTitleProperty, value);
        }

        public FrameworkElement TileSymbolControl
        {
            get => (FrameworkElement)GetValue(TileSymbolControlProperty);
            set => SetValue(TileSymbolControlProperty, value);
        }

        static DefaultTileContent()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DefaultTileContent), new FrameworkPropertyMetadata(typeof(DefaultTileContent)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultTileContent"/> class.
        /// </summary>
        public DefaultTileContent()
        {
            OnPropertyChanged(new DependencyPropertyChangedEventArgs(BackgroundProperty, null, null));
        }

        /// <inheritdoc />
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _innerBorder = GetTemplateChild("PART_InnerBorder") as Control ?? throw new KeyNotFoundException("Control could not be found: PART_InnerBorder");
            _title = GetTemplateChild("PART_Title") as Control ?? throw new KeyNotFoundException("Control could not be found: PART_Title");

            _isInitialized = true;
        }

        /// <inheritdoc />
        protected sealed override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (!_isInitialized)
                return;
            if ((e.Property == BackgroundProperty || e.Property == ForegroundProperty) && Background is SolidColorBrush solidBrush)
            {
                _innerBorder.BorderBrush = new SolidColorBrush(ContrastColor(solidBrush.Color));
                if (Foreground == null)
                    _title.Foreground = new SolidColorBrush(ContrastColor(solidBrush.Color));
            }
        }

        private static Color ContrastColor(Color color, byte alpha = 255)
        {
            // Counting the perceptive luminance - human eye favors green color...
            var a = 1 - (((0.299 * color.R) + (0.587 * color.G) + (0.114 * color.B)) / 255);
            var d = (byte)(a < 0.5 ? 0 : 255);

            return Color.FromArgb(alpha, d, d, d);
        }
    }
}
