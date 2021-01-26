using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MaSch.Presentation.Wpf.ColorPicker;

namespace MaSch.Presentation.Wpf.Controls
{
    public partial class ColorPickerControl
    {
        public event EventHandler<Color> SelectedColorChanged;
        
        #region Private Fields

        private Color _color = Colors.White;
        private Point? _colorPosition = new Point(0,0);
        private bool _shouldSetPoint = true;
        private bool _update = true;

        #endregion

        #region DependencyProperties

        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register("SelectedColor", typeof(Color), typeof(ColorPickerControl), new PropertyMetadata(Colors.White, OnSelectedColorChanged));
        public static readonly DependencyProperty AProperty =
            DependencyProperty.Register("A", typeof(byte), typeof(ColorPickerControl), new PropertyMetadata((byte)255, OnAChanged));
        public static readonly DependencyProperty RProperty =
            DependencyProperty.Register("R", typeof(byte), typeof(ColorPickerControl), new PropertyMetadata((byte)255, OnRChanged));
        public static readonly DependencyProperty GProperty =
            DependencyProperty.Register("G", typeof(byte), typeof(ColorPickerControl), new PropertyMetadata((byte)255, OnGChanged));
        public static readonly DependencyProperty BProperty =
            DependencyProperty.Register("B", typeof(byte), typeof(ColorPickerControl), new PropertyMetadata((byte)255, OnBChanged));
        public static readonly DependencyProperty HexadecimalStringProperty =
            DependencyProperty.Register("HexadecimalString", typeof(string), typeof(ColorPickerControl), new PropertyMetadata("#FFFFFFFF", OnHexadecimalChanged));
        public static readonly DependencyProperty ScAProperty =
            DependencyProperty.Register("ScA", typeof(float), typeof(ColorPickerControl), new PropertyMetadata(1F, OnScAChanged));
        public static readonly DependencyProperty ScRProperty =
            DependencyProperty.Register("ScR", typeof(float), typeof(ColorPickerControl), new PropertyMetadata(1F, OnScRChanged));
        public static readonly DependencyProperty ScGProperty =
            DependencyProperty.Register("ScG", typeof(float), typeof(ColorPickerControl), new PropertyMetadata(1F, OnScGChanged));
        public static readonly DependencyProperty ScBProperty =
            DependencyProperty.Register("ScB", typeof(float), typeof(ColorPickerControl), new PropertyMetadata(1F, OnScBChanged));
        public static readonly DependencyProperty IsMiniViewProperty =
            DependencyProperty.Register("IsMiniView", typeof(bool), typeof(ColorPickerControl), new PropertyMetadata(false));

        #endregion

        #region Dependency Property Changed Handlers

        public static void OnSelectedColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
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

        public static void OnHexadecimalChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ColorPickerControl owner)
            {
                try
                {
                    // ReSharper disable once PossibleNullReferenceException
                    owner._color = (Color)ColorConverter.ConvertFromString((string)e.NewValue);
                }
                catch (FormatException)
                {
                    owner.HexadecimalString = (string)e.OldValue;
                }
                owner.SelectedColor = owner._color;
            }
        }

        public static void OnAChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ColorPickerControl owner)
            {
                owner._color.A = (byte)e.NewValue;
                owner.SelectedColor = owner._color;
            }
        }

        public static void OnRChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ColorPickerControl owner)
            {
                owner._color.R = (byte)e.NewValue;
                owner.SelectedColor = owner._color;
            }
        }

        public static void OnGChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ColorPickerControl owner)
            {
                owner._color.G = (byte)e.NewValue;
                owner.SelectedColor = owner._color;
            }
        }

        public static void OnBChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ColorPickerControl owner)
            {
                owner._color.B = (byte)e.NewValue;
                owner.SelectedColor = owner._color;
            }
        }

        public static void OnScAChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ColorPickerControl owner)
            {
                owner._color.ScA = (float)e.NewValue;
                owner.SelectedColor = owner._color;
            }
        }

        public static void OnScRChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ColorPickerControl owner)
            {
                owner._color.ScR = (float)e.NewValue;
                owner.SelectedColor = owner._color;
            }
        }

        public static void OnScGChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ColorPickerControl owner)
            {
                owner._color.ScG = (float)e.NewValue;
                owner.SelectedColor = owner._color;
            }
        }

        public static void OnScBChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ColorPickerControl owner)
            {
                owner._color.ScB = (float)e.NewValue;
                owner.SelectedColor = owner._color;
            }
        }

        #endregion

        #region Public Properties

        public Color SelectedColor
        {
            get => GetValue(SelectedColorProperty) as Color? ?? Colors.White;
            set => SetValue(SelectedColorProperty, value);
        }
        public byte A
        {
            get => GetValue(AProperty) as byte? ?? 255;
            set => SetValue(AProperty, value);
        }
        public byte R
        {
            get => GetValue(RProperty) as byte? ?? 255;
            set => SetValue(RProperty, value);
        }
        public byte G
        {
            get => GetValue(GProperty) as byte? ?? 255;
            set => SetValue(GProperty, value);
        }
        public byte B
        {
            get => GetValue(BProperty) as byte? ?? 255;
            set => SetValue(BProperty, value);
        }
        public string HexadecimalString
        {
            get => (string)GetValue(HexadecimalStringProperty);
            set => SetValue(HexadecimalStringProperty, value);
        }
        public float ScA
        {
            get => GetValue(ScAProperty) as float? ?? 1F;
            set => SetValue(ScAProperty, value);
        }
        public float ScR
        {
            get => GetValue(ScRProperty) as float? ?? 1F;
            set => SetValue(ScRProperty, value);
        }
        public float ScG
        {
            get => GetValue(ScGProperty) as float? ?? 1F;
            set => SetValue(ScGProperty, value);
        }
        public float ScB
        {
            get => GetValue(ScBProperty) as float? ?? 1F;
            set => SetValue(ScBProperty, value);
        }
        public bool IsMiniView
        {
            get => GetValue(IsMiniViewProperty) as bool? ?? false;
            set => SetValue(IsMiniViewProperty, value);
        }

        #endregion

        public ColorPickerControl()
        {
            InitializeComponent();
            ColorDetail.PreviewMouseLeftButtonUp += (s, e) => Mouse.Capture(null);
            ColorDetail.PreviewMouseLeftButtonDown += ColorDetail_MouseLeftButtonDown;
            ColorDetail.PreviewMouseMove += ColorDetail_PreviewMouseMove;
            ColorDetail.SizeChanged += ColorDetail_SizeChanged;
            ColorSlider.ValueChanged += ColorSlider_ValueChanged;
        }

        #region Event Handlers

        private void ColorSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_colorPosition.HasValue)
            {
                ColorSlider.SelectedColor = ColorUtilities.ConvertHsvToRgb(360 - ColorSlider.Value, 1, 1);
                _shouldSetPoint = false;
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
                MarkTransform.X = MarkTransform.X * widthDifference;
                MarkTransform.Y = MarkTransform.Y * heightDifference;
            }
            else if (_colorPosition != null)
            {
                MarkTransform.X = ((Point)_colorPosition).X * e.NewSize.Width;
                MarkTransform.Y = ((Point)_colorPosition).Y * e.NewSize.Height;
            }
        }

        private void ColorDetail_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
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

        #endregion

        #region Private Methods

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
            p.X = p.X / maxWidth;
            p.Y = p.Y / maxHeight;
            _colorPosition = p;
            _shouldSetPoint = false;
            DetermineColor(p);
        }

        private void UpdateMarkerPosition(Color theColor)
        {
            if (_shouldSetPoint)
            {
                var hsv = ColorUtilities.ConvertRgbToHsv(theColor.R, theColor.G, theColor.B);
                if(hsv.S > 0)
                    ColorSlider.Value = hsv.H;
                var p = new Point(hsv.S, 1 - hsv.V);
                _colorPosition = p;
                p.X = p.X * ColorDetail.ActualWidth;
                p.Y = p.Y * ColorDetail.ActualHeight;
                MarkTransform.X = p.X;
                MarkTransform.Y = p.Y;
            }
            else
                _shouldSetPoint = true;
        }

        private void DetermineColor(Point p)
        {
            var hsv = new HsvColor(360 - ColorSlider.Value, 1, 1)
            {
                S = p.X,
                V = 1 - p.Y
            };
            _color = ColorUtilities.ConvertHsvToRgb(hsv.H, hsv.S, hsv.V);
            _color.ScA = ScA;
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

        #endregion
    }
}
