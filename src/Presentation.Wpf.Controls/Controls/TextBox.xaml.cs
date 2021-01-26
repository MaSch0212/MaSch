using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace MaSch.Presentation.Wpf.Controls
{
    public delegate void DoubleEventHandler(object sender, double? newValue);
    public delegate void BooleanEventHandler(object sender, bool? newValue);

    public class TextBox : System.Windows.Controls.TextBox
    {
        #region Events

        public event DoubleEventHandler NumericValueChanged;
        public event DoubleEventHandler MaximumChanged;
        public event DoubleEventHandler MinimumChanged;
        public event BooleanEventHandler OnlyNumericValuesChanged;

        #endregion

        #region Fields

        private TextBlock _suffixTextBlock;
        private double _actualValue;
        private readonly Regex _valueRegex;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty NumericValueProperty =
            DependencyProperty.Register("NumericValue", typeof(double), typeof(TextBox), new FrameworkPropertyMetadata(0D, OnNumericValueChanged, CoerceNumericValue)
            { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged});
        public static readonly DependencyProperty DecimalPlacesProperty =
            DependencyProperty.Register("DecimalPlaces", typeof(int), typeof(TextBox), new PropertyMetadata(2));
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(TextBox), new PropertyMetadata(100D, OnMaximumChanged, CoercMaximum));
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(TextBox), new PropertyMetadata(0D, OnMinimumChanged, CoerceMinimum));
        public static readonly DependencyProperty StepSizeProperty =
            DependencyProperty.Register("StepSize", typeof(double), typeof(TextBox), new PropertyMetadata(1D, OnStepSizeChanged, CoerceStepSize));
        public static readonly DependencyProperty OnlyNumericValuesProperty =
            DependencyProperty.Register("OnlyNumericValues", typeof(bool), typeof(TextBox), new PropertyMetadata(false, OnOnlyNumericValuesChanged));
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(TextBox), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty IsUpDownEnabledProperty =
            DependencyProperty.Register("IsUpDownEnabled", typeof(bool), typeof(TextBox), new PropertyMetadata(true));
        public static readonly DependencyProperty IsThrousandSeperatorEnabledProperty =
            DependencyProperty.Register("IsThrousandSeperatorEnabled", typeof(bool), typeof(TextBox), new PropertyMetadata(true));
        public static readonly DependencyProperty HighlightTextProperty =
            DependencyProperty.Register("HighlightText", typeof(string), typeof(TextBox), new PropertyMetadata(string.Empty, OnHighlightChanged));
        public static readonly DependencyProperty HighlightBrushProperty =
            DependencyProperty.Register("HighlightBrush", typeof(Brush), typeof(TextBox), new PropertyMetadata(new SolidColorBrush(Colors.Yellow), OnHighlightChanged));
        public static readonly DependencyProperty HighlightSingleWordProperty =
            DependencyProperty.Register("HighlightSingleWord", typeof(bool), typeof(TextBox), new PropertyMetadata(false, OnHighlightChanged));
        public static readonly DependencyProperty HighlightMatchCaseProperty =
            DependencyProperty.Register("HighlightMatchCase", typeof(bool), typeof(TextBox), new PropertyMetadata(false, OnHighlightChanged));
        public static readonly DependencyProperty SuffixProperty =
            DependencyProperty.Register("Suffix", typeof(string), typeof(TextBox), new PropertyMetadata(null, OnSuffixChanged));
        public static readonly DependencyProperty SelectAllOnFocusProperty =
            DependencyProperty.Register("SelectAllOnFocus", typeof(bool), typeof(TextBox), new PropertyMetadata(true));
        public static readonly DependencyProperty EndContentProperty =
            DependencyProperty.Register("EndContent", typeof(object), typeof(TextBox), new PropertyMetadata(null));
        public static readonly DependencyProperty StartContentProperty =
            DependencyProperty.Register("StartContent", typeof(object), typeof(TextBox), new PropertyMetadata(null));

        #endregion

        #region Dependency Property Changed Handlers

        private static void OnNumericValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is TextBox tbx && tbx.OnlyNumericValues)
            {
                tbx.NumericValueChanged?.Invoke(tbx, e.NewValue as double?);
                tbx.Text = string.Format($"{{0:{(tbx.IsThrousandSeperatorEnabled ? "N" : "F") + tbx.DecimalPlaces}}}", e.NewValue);
            }
        }

        private static void OnMaximumChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is TextBox tbx && e.NewValue != e.OldValue)
            {
                if (tbx.NumericValue > (double)e.NewValue)
                    tbx.NumericValue = (double)e.NewValue;
                tbx.MaximumChanged?.Invoke(tbx, e.NewValue as double?);
                tbx.NumericValue = tbx._actualValue;
            }
        }

        private static void OnMinimumChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is TextBox tbx && e.NewValue != e.OldValue)
            {
                if (tbx.NumericValue < (double)e.NewValue)
                    tbx.NumericValue = (double)e.NewValue;
                tbx.MinimumChanged?.Invoke(tbx, e.NewValue as double?);
                tbx.NumericValue = tbx._actualValue;
            }
        }

        private static void OnStepSizeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is TextBox && e.NewValue != e.OldValue)
            {

            }
        }

        private static void OnOnlyNumericValuesChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is TextBox tbx && e.NewValue != e.OldValue)
            {
                tbx.OnlyNumericValuesChanged?.Invoke(tbx, e.NewValue as bool?);
            }
        }

        public static void OnSuffixChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var owner = obj as TextBox;
            if (owner?._suffixTextBlock != null)
                owner._suffixTextBlock.Visibility = string.IsNullOrEmpty((string)e.NewValue) ? Visibility.Collapsed : Visibility.Visible;
        }

        #endregion

        #region Dependency Property Coerce Handlers

        private static object CoerceNumericValue(DependencyObject obj, object value)
        {
            if (obj is TextBox tbx)
            {
                var val = Math.Round((double)value, tbx.DecimalPlaces);
                tbx._actualValue = val;
                if (val > tbx.Maximum)
                    val = tbx.Maximum;
                if (val < tbx.Minimum)
                    val = tbx.Minimum;
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (val == tbx.NumericValue)
                    tbx.Text = string.Format($"{{0:{(tbx.IsThrousandSeperatorEnabled ? "N" : "F") + tbx.DecimalPlaces}}}", val);
                return val;
            }
            return 0D;
        }

        private static object CoercMaximum(DependencyObject obj, object value)
        {
            if (obj is TextBox tbx)
            {
                var val = Math.Round((double)value, tbx.DecimalPlaces);
                return val;
            }
            return 100D;
        }
        
        private static object CoerceMinimum(DependencyObject obj, object value)
        {
            if (obj is TextBox tbx)
            {
                var val = Math.Round((double)value, tbx.DecimalPlaces);
                return val;
            }
            return 0D;
        }

        private static object CoerceStepSize(DependencyObject obj, object value)
        {
            if (obj is TextBox tbx)
            {
                var val = Math.Round((double)value, tbx.DecimalPlaces);
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (val == 0D)
                    val = 1D;
                return val;
            }
            return 1D;
        }

        #endregion

        #region Properties
        
        public double NumericValue
        {
            get => GetValue(NumericValueProperty) as double? ?? 0D;
            set => SetValue(NumericValueProperty, value);
        }
        public int DecimalPlaces
        {
            get => GetValue(DecimalPlacesProperty) as int? ?? 2;
            set => SetValue(DecimalPlacesProperty, value);
        }
        public double Maximum
        {
            get => GetValue(MaximumProperty) as double? ?? 100D;
            set => SetValue(MaximumProperty, value);
        }
        public double Minimum
        {
            get => GetValue(MinimumProperty) as double? ?? 0D;
            set => SetValue(MinimumProperty, value);
        }
        public double StepSize
        {
            get => GetValue(StepSizeProperty) as double? ?? 1D;
            set => SetValue(StepSizeProperty, value);
        }
        public bool OnlyNumericValues
        {
            get => GetValue(OnlyNumericValuesProperty) as bool? ?? false;
            set => SetValue(OnlyNumericValuesProperty, value);
        }
        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }
        public bool IsUpDownEnabled
        {
            get => GetValue(IsUpDownEnabledProperty) as bool? ?? true;
            set => SetValue(IsUpDownEnabledProperty, value);
        }
        public bool IsThrousandSeperatorEnabled
        {
            get => GetValue(IsThrousandSeperatorEnabledProperty) as bool? ?? true;
            set => SetValue(IsThrousandSeperatorEnabledProperty, value);
        }
        public string HighlightText
        {
            get => (string)GetValue(HighlightTextProperty);
            set => SetValue(HighlightTextProperty, value);
        }
        public Brush HighlightBrush
        {
            get => (Brush)GetValue(HighlightBrushProperty);
            set => SetValue(HighlightBrushProperty, value);
        }
        public bool HighlightSingleWord
        {
            get => GetValue(HighlightSingleWordProperty) as bool? ?? false;
            set => SetValue(HighlightSingleWordProperty, value);
        }
        public bool HighlightMatchCase
        {
            get => GetValue(HighlightMatchCaseProperty) as bool? ?? false;
            set => SetValue(HighlightMatchCaseProperty, value);
        }
        public string Suffix
        {
            get => (string)GetValue(SuffixProperty);
            set => SetValue(SuffixProperty, value);
        }
        public bool SelectAllOnFocus
        {
            get => GetValue(SelectAllOnFocusProperty) as bool? ?? true;
            set => SetValue(SelectAllOnFocusProperty, value);
        }
        public object EndContent
        {
            get => (object)GetValue(EndContentProperty);
            set => SetValue(EndContentProperty, value);
        }
        public object StartContent
        {
            get => (object)GetValue(StartContentProperty);
            set => SetValue(StartContentProperty, value);
        }

        #endregion

        static TextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBox), new FrameworkPropertyMetadata(typeof(TextBox)));
            PaddingProperty.OverrideMetadata(typeof(TextBox), new FrameworkPropertyMetadata(new Thickness(0), OnPaddingChanged));
        }
        
        public TextBox()
        {
            PreviewTextInput += ModernUITextBox_PreviewTextInput;
            PreviewKeyDown += ModernUITextBox_PreviewKeyDown;
            Loaded += (s, e) => OnNumericValueChanged(this, new DependencyPropertyChangedEventArgs(NumericValueProperty, null, NumericValue));
            DataObject.AddPastingHandler(this, OnPaste);
            var numberSep = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;
            _valueRegex = new Regex("\\A(\\+|\\-)?[0-9]*\\" + numberSep + "?[0-9]*[Ee]?[0-9]*\\Z", RegexOptions.Compiled);
        }

        public static void OnHighlightChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var owner = obj as TextBox;
            owner?.InvalidateVisual();
        }

        public static void OnPaddingChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is TextBox tb)
                tb.OnPaddingChanged(e);
        }

        protected virtual void OnPaddingChanged(DependencyPropertyChangedEventArgs e)
        {
            if(_suffixTextBlock != null)
                _suffixTextBlock.Margin = new Thickness(0, Padding.Top, Padding.Right, Padding.Bottom);
        }

        void ModernUITextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                var be = GetBindingExpression(TextProperty);
                be?.UpdateSource();
            }
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                if (OnlyNumericValues)
                {
                    var text = (string)e.DataObject.GetData(typeof(string));
                    var previewText = Text.Substring(0, SelectionStart) + text +
                        Text.Substring(SelectionStart + SelectionLength, Text.Length - SelectionStart - SelectionLength);
                    if (!_valueRegex.IsMatch(previewText))
                        e.CancelCommand();
                }
            }
            else
                e.CancelCommand();
        }

        private void ModernUITextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (OnlyNumericValues)
            {
                var previewText = Text.Substring(0, SelectionStart) + e.Text +
                    Text.Substring(SelectionStart + SelectionLength, Text.Length - SelectionStart - SelectionLength);
                e.Handled = !_valueRegex.IsMatch(previewText.Replace(" ", ""));
            }
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            //drawingContext.DrawRectangle(Background, new Pen(), new Rect(0, 0, ActualWidth, ActualHeight));

            if (Text != string.Empty)
            {
                var leftMargin = BorderThickness.Left + Padding.Left;
                var topMargin = BorderThickness.Top + Padding.Top - VerticalOffset;
                var rightMargin = BorderThickness.Right + Padding.Right;

                var dpi = (PresentationSource.FromVisual(this)?.CompositionTarget?.TransformToDevice.M11 ?? 1D) * 96D;
                var ft = new FormattedText(Text,
                    CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
                    new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
                    FontSize, Foreground, new NumberSubstitution(), TextFormattingMode.Ideal, dpi)
                {
                    LineHeight = FontSize*FontFamily.LineSpacing,
                    MaxTextWidth = ActualWidth - leftMargin - rightMargin
                };

                if (!string.IsNullOrEmpty(HighlightText))
                {
                    var highlights = HighlightSingleWord ? HighlightText.Split(' ') : new[] { HighlightText };
                    // ReSharper disable once UnusedVariable
                    foreach (var highlight in highlights.Distinct())
                    {
                        var start = 0;
                        while (true)
                        {
                            var index = Text.IndexOf(HighlightText, start, HighlightMatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
                            if (index >= 0)
                            {
                                var geom = ft.BuildHighlightGeometry(new Point(leftMargin, topMargin), index, HighlightText.Length);
                                if (geom != null)
                                    drawingContext.DrawGeometry(HighlightBrush, null, geom);
                                start = index + 1;
                            }
                            else
                                return;
                        }
                    }
                }
            }
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            if(SelectAllOnFocus)
                SelectAll();
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == PaddingProperty && _suffixTextBlock != null)
            {
                var padding = e.NewValue as Thickness?;
                if (padding.HasValue)
                    _suffixTextBlock.Margin = new Thickness(2, padding.Value.Top, 2, padding.Value.Bottom);
            }

            base.OnPropertyChanged(e);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var btnUp = GetTemplateChild("MaSch_Up") as Button;
            var btnDown = GetTemplateChild("MaSch_Down") as Button;
            _suffixTextBlock = GetTemplateChild("MaSch_Suffix") as TextBlock;
            if (btnUp != null)
            {
                var upRunning = false;
                var upRunningLock = new object();
                btnUp.PreviewMouseLeftButtonDown += async (s, e) =>
                {
                    NumericValue += StepSize;

                    lock (upRunningLock)
                    {
                        if (upRunning)
                            return;
                        upRunning = true;
                    }
                    await Task.Delay(1000);
                    int wait = 400, count = 1;
                    while (btnUp.IsPressed && NumericValue < Maximum)
                    {
                        NumericValue += StepSize;
                        count++;
                        if (count >= 20)
                            wait = 50;
                        else if (count >= 8)
                            wait = 150;
                        await Task.Delay(wait);
                    }
                    lock (upRunningLock)
                        upRunning = false;
                };
            }
            if (btnDown != null)
            {
                var downRunning = false;
                var downRunningLock = new object();
                btnDown.PreviewMouseLeftButtonDown += async (s, e) =>
                {
                    NumericValue -= StepSize;

                    lock (downRunningLock)
                    {
                        if (downRunning)
                            return;
                        downRunning = true;
                    }
                    await Task.Delay(1000);
                    int wait = 400, count = 1;
                    while (btnDown.IsPressed && NumericValue > Minimum)
                    {
                        NumericValue -= StepSize;
                        count++;
                        if (count >= 20)
                            wait = 50;
                        else if (count >= 8)
                            wait = 150;
                        await Task.Delay(wait);
                    }
                    lock (downRunningLock)
                        downRunning = false;
                };
            }
            if (_suffixTextBlock != null)
                _suffixTextBlock.Margin = new Thickness(0, Padding.Top, Padding.Right, Padding.Bottom);

            var textField = GetTemplateChild("PART_ContentHost") as System.Windows.Controls.ScrollViewer;

            void NumberChange()
            {
                if (OnlyNumericValues)
                {
                    var previewText = Text.Replace(" ", "");
                    var numberSep = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;
                    var matches = Regex.Matches(previewText, "(\\-|\\+|\\" + numberSep + "|E|e)(?=([^0-9]|\\Z))");
                    previewText = matches.Cast<Match>().Aggregate(previewText, (current, match) => current.Replace(match.Value, match.Value + "0"));
                    if (string.IsNullOrEmpty(previewText) || previewText.StartsWith("e0"))
                        previewText = "0";
                    NumericValue = double.Parse(previewText);
                }
            }

            if (textField != null)
            {
                textField.LostFocus += (s, e) => NumberChange();
                textField.PreviewKeyDown += (s, e) =>
                {
                    if (e.Key == Key.Enter)
                        NumberChange();
                };
            }

        }
    }
}
