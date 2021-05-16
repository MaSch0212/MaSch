using MaSch.Presentation.Wpf.ColorPicker;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace MaSch.Presentation.Wpf.Controls
{
    /// <summary>
    /// Control with which the user can pick a color.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.UserControl" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class ColorPickerControl
    {
        /// <summary>
        /// Dependency property. Gets or sets the currently selected color.
        /// </summary>
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(
                "SelectedColor",
                typeof(Color),
                typeof(ColorPickerControl),
                new PropertyMetadata(Colors.White, OnSelectedColorChanged));

        /// <summary>
        /// Dependency property. Gets or sets the transparency of the color.
        /// </summary>
        public static readonly DependencyProperty AProperty =
            DependencyProperty.Register(
                "A",
                typeof(byte),
                typeof(ColorPickerControl),
                new PropertyMetadata((byte)255, OnAChanged));

        /// <summary>
        /// Dependency property. Gets or sets the red value of the color.
        /// </summary>
        public static readonly DependencyProperty RProperty =
            DependencyProperty.Register(
                "R",
                typeof(byte),
                typeof(ColorPickerControl),
                new PropertyMetadata((byte)255, OnRChanged));

        /// <summary>
        /// Dependency property. Gets or sets the green value of the color.
        /// </summary>
        public static readonly DependencyProperty GProperty =
            DependencyProperty.Register(
                "G",
                typeof(byte),
                typeof(ColorPickerControl),
                new PropertyMetadata((byte)255, OnGChanged));

        /// <summary>
        /// Dependency property. Gets or sets the blue value of the color.
        /// </summary>
        public static readonly DependencyProperty BProperty =
            DependencyProperty.Register(
                "B",
                typeof(byte),
                typeof(ColorPickerControl),
                new PropertyMetadata((byte)255, OnBChanged));

        /// <summary>
        /// Dependency property. Gets or sets the hexadecimal representation of the selected color.
        /// </summary>
        public static readonly DependencyProperty HexadecimalStringProperty =
            DependencyProperty.Register(
                "HexadecimalString",
                typeof(string),
                typeof(ColorPickerControl),
                new PropertyMetadata("#FFFFFFFF", OnHexadecimalChanged));

        /// <summary>
        /// Dependency property. Gets or sets the ScRGB transparency of the color.
        /// </summary>
        public static readonly DependencyProperty ScAProperty =
            DependencyProperty.Register(
                "ScA",
                typeof(float),
                typeof(ColorPickerControl),
                new PropertyMetadata(1F, OnScAChanged));

        /// <summary>
        /// Dependency property. Gets or sets the ScRGB red value of the color.
        /// </summary>
        public static readonly DependencyProperty ScRProperty =
            DependencyProperty.Register(
                "ScR",
                typeof(float),
                typeof(ColorPickerControl),
                new PropertyMetadata(1F, OnScRChanged));

        /// <summary>
        /// Dependency property. Gets or sets the ScRGB green value of the color.
        /// </summary>
        public static readonly DependencyProperty ScGProperty =
            DependencyProperty.Register(
                "ScG",
                typeof(float),
                typeof(ColorPickerControl),
                new PropertyMetadata(1F, OnScGChanged));

        /// <summary>
        /// Dependency property. Gets or sets the ScRGB blue value of the value.
        /// </summary>
        public static readonly DependencyProperty ScBProperty =
            DependencyProperty.Register(
                "ScB",
                typeof(float),
                typeof(ColorPickerControl),
                new PropertyMetadata(1F, OnScBChanged));

        /// <summary>
        /// Dependency property. Gets or sets a value indicating whether this control should be shown in a minimized view.
        /// </summary>
        public static readonly DependencyProperty IsMiniViewProperty =
            DependencyProperty.Register(
                "IsMiniView",
                typeof(bool),
                typeof(ColorPickerControl),
                new PropertyMetadata(false));

        private Color _color = Colors.White;
        private Point? _colorPosition = new Point(0, 0);
        private bool _update = true;

        /// <summary>
        /// Occurs when the selected color changed.
        /// </summary>
        public event EventHandler<Color>? SelectedColorChanged;

        /// <summary>
        /// Gets or sets the currently selected color.
        /// </summary>
        public Color SelectedColor
        {
            get => GetValue(SelectedColorProperty) as Color? ?? Colors.White;
            set => SetValue(SelectedColorProperty, value);
        }

        /// <summary>
        /// Gets or sets the transparency of the color.
        /// </summary>
        public byte A
        {
            get => GetValue(AProperty) as byte? ?? 255;
            set => SetValue(AProperty, value);
        }

        /// <summary>
        /// Gets or sets the red value of the color.
        /// </summary>
        public byte R
        {
            get => GetValue(RProperty) as byte? ?? 255;
            set => SetValue(RProperty, value);
        }

        /// <summary>
        /// Gets or sets the green value of the color.
        /// </summary>
        public byte G
        {
            get => GetValue(GProperty) as byte? ?? 255;
            set => SetValue(GProperty, value);
        }

        /// <summary>
        /// Gets or sets the blue value of the color.
        /// </summary>
        public byte B
        {
            get => GetValue(BProperty) as byte? ?? 255;
            set => SetValue(BProperty, value);
        }

        /// <summary>
        /// Gets or sets the hexadecimal representation of the selected color.
        /// </summary>
        public string HexadecimalString
        {
            get => (string)GetValue(HexadecimalStringProperty);
            set => SetValue(HexadecimalStringProperty, value);
        }

        /// <summary>
        /// Gets or sets the ScRGB transparency of the color.
        /// </summary>
        public float ScA
        {
            get => GetValue(ScAProperty) as float? ?? 1F;
            set => SetValue(ScAProperty, value);
        }

        /// <summary>
        /// Gets or sets the ScRGB red value of the color.
        /// </summary>
        public float ScR
        {
            get => GetValue(ScRProperty) as float? ?? 1F;
            set => SetValue(ScRProperty, value);
        }

        /// <summary>
        /// Gets or sets the ScRGB green value of the color.
        /// </summary>
        public float ScG
        {
            get => GetValue(ScGProperty) as float? ?? 1F;
            set => SetValue(ScGProperty, value);
        }

        /// <summary>
        /// Gets or sets the ScRGB blue value of the value.
        /// </summary>
        public float ScB
        {
            get => GetValue(ScBProperty) as float? ?? 1F;
            set => SetValue(ScBProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this control should be shown in a minimized view.
        /// </summary>
        public bool IsMiniView
        {
            get => GetValue(IsMiniViewProperty) as bool? ?? false;
            set => SetValue(IsMiniViewProperty, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorPickerControl"/> class.
        /// </summary>
        public ColorPickerControl()
        {
            InitializeComponent();
            ColorDetail.PreviewMouseLeftButtonUp += (s, e) => Mouse.Capture(null);
            ColorDetail.PreviewMouseLeftButtonDown += ColorDetail_MouseLeftButtonDown;
            ColorDetail.PreviewMouseMove += ColorDetail_PreviewMouseMove;
            ColorDetail.SizeChanged += ColorDetail_SizeChanged;
            ColorSlider.ValueChanged += ColorSlider_ValueChanged;
        }

        private void ColorSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_colorPosition.HasValue)
            {
                ColorSlider.SelectedColor = ColorUtilities.ConvertHsvToRgb(360 - ColorSlider.Value, 1, 1);
                DetermineColor(_colorPosition.Value);
            }
        }

        private void ColorDetail_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.PreviousSize != Size.Empty &&
                Math.Abs(e.PreviousSize.Width) > 0 && Math.Abs(e.PreviousSize.Height) > 0)
            {
                var widthDifference = e.NewSize.Width / e.PreviousSize.Width;
                var heightDifference = e.NewSize.Height / e.PreviousSize.Height;
                MarkTransform.X *= widthDifference;
                MarkTransform.Y *= heightDifference;
            }
            else if (_colorPosition != null)
            {
                MarkTransform.X = ((Point)_colorPosition).X * e.NewSize.Width;
                MarkTransform.Y = ((Point)_colorPosition).Y * e.NewSize.Height;
            }
        }

        private void ColorDetail_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var p = e.GetPosition(ColorDetail);
                UpdateMarkerPosition(p);
                Mouse.Synchronize();
                Mouse.Capture(ColorDetail);
            }
        }

        private void ColorDetail_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            var p = e.GetPosition(ColorDetail);
            UpdateMarkerPosition(p);
            Mouse.Capture(ColorDetail);
        }

        private void UpdateMarkerPosition(Point p)
        {
            var maxWidth = ColorDetail.ActualWidth;
            var maxHeight = ColorDetail.ActualHeight;

            if (p.X > maxWidth)
                p.X = maxWidth;
            else if (p.X < 0)
                p.X = 0;
            if (p.Y > maxHeight)
                p.Y = maxHeight;
            else if (p.Y < 0)
                p.Y = 0;
            MarkTransform.X = p.X;
            MarkTransform.Y = p.Y;
            p.X /= maxWidth;
            p.Y /= maxHeight;
            _colorPosition = p;
            DetermineColor(p);
        }

        private void UpdateMarkerPosition(Color theColor)
        {
            var hsv = ColorUtilities.ConvertRgbToHsv(theColor.R, theColor.G, theColor.B);
            if (hsv.S > 0)
                ColorSlider.Value = hsv.H;
            var p = new Point(hsv.S, 1 - hsv.V);
            _colorPosition = p;
            p.X *= ColorDetail.ActualWidth;
            p.Y *= ColorDetail.ActualHeight;
            MarkTransform.X = p.X;
            MarkTransform.Y = p.Y;
        }

        private void DetermineColor(Point p)
        {
            var hsv = new HsvColor(360 - ColorSlider.Value, 1, 1)
            {
                S = p.X,
                V = 1 - p.Y,
            };
            _color = ColorUtilities.ConvertHsvToRgb(hsv.H, hsv.S, hsv.V);
            _color.ScA = ScA;
            if (_update)
                SelectedColor = _color;
        }

        private void ModernUITextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is System.Windows.Controls.TextBox tbx)
            {
                var previewText = tbx.Text.Substring(0, tbx.SelectionStart) + e.Text +
                    tbx.Text.Substring(tbx.SelectionStart + tbx.SelectionLength, tbx.Text.Length - tbx.SelectionStart - tbx.SelectionLength);
                if (!Regex.IsMatch(previewText, "\\A\\#[0-9a-fA-F]*\\Z"))
                    e.Handled = true;
            }
        }

        private static void OnSelectedColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ColorPickerControl owner && owner._update)
            {
                owner._update = false;
                try
                {
                    var c = (Color)e.NewValue;
                    owner.SelectedColorChanged?.Invoke(owner, c);
                    owner._color = c;
                    owner.A = c.A;
                    owner.R = c.R;
                    owner.G = c.G;
                    owner.B = c.B;
                    owner.ScA = c.ScA;
                    owner.ScR = c.ScR;
                    owner.ScG = c.ScG;
                    owner.ScB = c.ScB;
                    owner.HexadecimalString = owner._color.ToString();
                    owner.UpdateMarkerPosition(c);
                }
                finally
                {
                    owner._update = true;
                }
            }
        }

        private static void OnHexadecimalChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ColorPickerControl owner)
            {
                try
                {
                    owner._color = (Color)ColorConverter.ConvertFromString((string)e.NewValue);
                }
                catch (FormatException)
                {
                    owner.HexadecimalString = (string)e.OldValue;
                }

                owner.SelectedColor = owner._color;
            }
        }

        private static void OnAChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ColorPickerControl owner)
            {
                owner._color.A = (byte)e.NewValue;
                owner.SelectedColor = owner._color;
            }
        }

        private static void OnRChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ColorPickerControl owner)
            {
                owner._color.R = (byte)e.NewValue;
                owner.SelectedColor = owner._color;
            }
        }

        private static void OnGChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ColorPickerControl owner)
            {
                owner._color.G = (byte)e.NewValue;
                owner.SelectedColor = owner._color;
            }
        }

        private static void OnBChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ColorPickerControl owner)
            {
                owner._color.B = (byte)e.NewValue;
                owner.SelectedColor = owner._color;
            }
        }

        private static void OnScAChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ColorPickerControl owner)
            {
                owner._color.ScA = (float)e.NewValue;
                owner.SelectedColor = owner._color;
            }
        }

        private static void OnScRChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ColorPickerControl owner)
            {
                owner._color.ScR = (float)e.NewValue;
                owner.SelectedColor = owner._color;
            }
        }

        private static void OnScGChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ColorPickerControl owner)
            {
                owner._color.ScG = (float)e.NewValue;
                owner.SelectedColor = owner._color;
            }
        }

        private static void OnScBChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ColorPickerControl owner)
            {
                owner._color.ScB = (float)e.NewValue;
                owner.SelectedColor = owner._color;
            }
        }
    }
}
