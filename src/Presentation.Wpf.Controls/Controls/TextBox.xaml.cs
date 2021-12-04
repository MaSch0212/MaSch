using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace MaSch.Presentation.Wpf.Controls;

/// <summary>
/// Event handler that has a <see cref="double"/> as event arguments.
/// </summary>
/// <param name="sender">The sender.</param>
/// <param name="newValue">The new value.</param>
public delegate void DoubleEventHandler(object sender, double? newValue);

/// <summary>
/// Event handler that has a <see cref="bool"/> as event arguments.
/// </summary>
/// <param name="sender">The sender.</param>
/// <param name="newValue">The new value.</param>
public delegate void BooleanEventHandler(object sender, bool? newValue);

/// <summary>
/// Text box with a lot more features than the default <see cref="System.Windows.Controls.TextBox"/>.
/// </summary>
/// <seealso cref="System.Windows.Controls.TextBox" />
public class TextBox : System.Windows.Controls.TextBox
{
    /// <summary>
    /// Dependency property. Gets or sets the numeric value of the text box. Only works when <see cref="OnlyNumericValuesProperty"/> is set to <c>true</c>.
    /// </summary>
    public static readonly DependencyProperty NumericValueProperty =
        DependencyProperty.Register(
            "NumericValue",
            typeof(double),
            typeof(TextBox),
            new FrameworkPropertyMetadata(0D, OnNumericValueChanged, CoerceNumericValue)
            {
                BindsTwoWayByDefault = true,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            });

    /// <summary>
    /// Dependency property. Gets or sets the decimal places for numeric values.
    /// </summary>
    public static readonly DependencyProperty DecimalPlacesProperty =
        DependencyProperty.Register(
            "DecimalPlaces",
            typeof(int),
            typeof(TextBox),
            new PropertyMetadata(2));

    /// <summary>
    /// Dependency property. Gets or sets the maximum numeric value possible.
    /// </summary>
    public static readonly DependencyProperty MaximumProperty =
        DependencyProperty.Register(
            "Maximum",
            typeof(double),
            typeof(TextBox),
            new PropertyMetadata(100D, OnMaximumChanged, CoercMaximum));

    /// <summary>
    /// Dependency property. Gets or sets the minimum numeric value possible.
    /// </summary>
    public static readonly DependencyProperty MinimumProperty =
        DependencyProperty.Register(
            "Minimum",
            typeof(double),
            typeof(TextBox),
            new PropertyMetadata(0D, OnMinimumChanged, CoerceMinimum));

    /// <summary>
    /// Dependency property. Gets or sets the size of the step that is used for the up and down buttons for numeric values.
    /// </summary>
    public static readonly DependencyProperty StepSizeProperty =
        DependencyProperty.Register(
            "StepSize",
            typeof(double),
            typeof(TextBox),
            new PropertyMetadata(1D) { CoerceValueCallback = CoerceStepSize });

    /// <summary>
    /// Dependency property. Gets or sets a value indicating whether only numeric values are allowed for this <see cref="TextBox"/>.
    /// </summary>
    public static readonly DependencyProperty OnlyNumericValuesProperty =
        DependencyProperty.Register(
            "OnlyNumericValues",
            typeof(bool),
            typeof(TextBox),
            new PropertyMetadata(false, OnOnlyNumericValuesChanged));

    /// <summary>
    /// Dependency property. Gets or sets the description in form of a placeholder.
    /// </summary>
    public static readonly DependencyProperty DescriptionProperty =
        DependencyProperty.Register(
            "Description",
            typeof(string),
            typeof(TextBox),
            new PropertyMetadata(string.Empty));

    /// <summary>
    /// Dependency property. Gets or sets a value indicating whether the up and down buttons for numeric values are shown.
    /// </summary>
    public static readonly DependencyProperty IsUpDownEnabledProperty =
        DependencyProperty.Register(
            "IsUpDownEnabled",
            typeof(bool),
            typeof(TextBox),
            new PropertyMetadata(true));

    /// <summary>
    /// Dependency property. Gets or sets a value indicating whether throusand seperator characters are displayed for numeric values.
    /// </summary>
    public static readonly DependencyProperty IsThrousandSeperatorEnabledProperty =
        DependencyProperty.Register(
            "IsThrousandSeperatorEnabled",
            typeof(bool),
            typeof(TextBox),
            new PropertyMetadata(true));

    /// <summary>
    /// Dependency property. Gets or sets the text to highlight in this text box.
    /// </summary>
    public static readonly DependencyProperty HighlightTextProperty =
        DependencyProperty.Register(
            "HighlightText",
            typeof(string),
            typeof(TextBox),
            new PropertyMetadata(string.Empty, OnHighlightChanged));

    /// <summary>
    /// Dependency property. Gets or sets the brush that should be used to highlight text.
    /// </summary>
    public static readonly DependencyProperty HighlightBrushProperty =
        DependencyProperty.Register(
            "HighlightBrush",
            typeof(Brush),
            typeof(TextBox),
            new PropertyMetadata(new SolidColorBrush(Colors.Yellow), OnHighlightChanged));

    /// <summary>
    /// Dependency property. Gets or sets a value indicating whether only single words should be highlighted.
    /// </summary>
    public static readonly DependencyProperty HighlightSingleWordProperty =
        DependencyProperty.Register(
            "HighlightSingleWord",
            typeof(bool),
            typeof(TextBox),
            new PropertyMetadata(false, OnHighlightChanged));

    /// <summary>
    /// Dependency property. Gets or sets a value indicating whether the highlighting should be case sensitive.
    /// </summary>
    public static readonly DependencyProperty HighlightMatchCaseProperty =
        DependencyProperty.Register(
            "HighlightMatchCase",
            typeof(bool),
            typeof(TextBox),
            new PropertyMetadata(false, OnHighlightChanged));

    /// <summary>
    /// Dependency property. Gets or sets text that is displayed after the input field.
    /// </summary>
    public static readonly DependencyProperty SuffixProperty =
        DependencyProperty.Register(
            "Suffix",
            typeof(string),
            typeof(TextBox),
            new PropertyMetadata(null, OnSuffixChanged));

    /// <summary>
    /// Dependency property. Gets or sets a value indicating whether the whole text should be selected when the control got focus.
    /// </summary>
    public static readonly DependencyProperty SelectAllOnFocusProperty =
        DependencyProperty.Register(
            "SelectAllOnFocus",
            typeof(bool),
            typeof(TextBox),
            new PropertyMetadata(true));

    /// <summary>
    /// Dependency property. Gets or sets the content that is displayed on the most right side of the control.
    /// </summary>
    public static readonly DependencyProperty EndContentProperty =
        DependencyProperty.Register(
            "EndContent",
            typeof(object),
            typeof(TextBox),
            new PropertyMetadata(null));

    /// <summary>
    /// Dependency property. Gets or sets the content that is displayed on the most left side of the control.
    /// </summary>
    public static readonly DependencyProperty StartContentProperty =
        DependencyProperty.Register(
            "StartContent",
            typeof(object),
            typeof(TextBox),
            new PropertyMetadata(null));

    private readonly Regex _valueRegex;
    private TextBlock? _suffixTextBlock;
    private double _actualValue;

    static TextBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBox), new FrameworkPropertyMetadata(typeof(TextBox)));
        PaddingProperty.OverrideMetadata(typeof(TextBox), new FrameworkPropertyMetadata(new Thickness(0), OnPaddingChanged));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TextBox"/> class.
    /// </summary>
    public TextBox()
    {
        PreviewTextInput += ModernUITextBox_PreviewTextInput;
        PreviewKeyDown += ModernUITextBox_PreviewKeyDown;
        Loaded += (s, e) => OnNumericValueChanged(this, new DependencyPropertyChangedEventArgs(NumericValueProperty, null, NumericValue));
        DataObject.AddPastingHandler(this, OnPaste);
        var numberSep = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;
        _valueRegex = new Regex("\\A(\\+|\\-)?[0-9]*\\" + numberSep + "?[0-9]*[Ee]?[0-9]*\\Z", RegexOptions.Compiled);
    }

    /// <summary>
    /// Occurs when the <see cref="NumericValue"/> property changed.
    /// </summary>
    public event DoubleEventHandler? NumericValueChanged;

    /// <summary>
    /// Occurs when the <see cref="Maximum"/> property changed.
    /// </summary>
    public event DoubleEventHandler? MaximumChanged;

    /// <summary>
    /// Occurs when the <see cref="Minimum"/> property changed.
    /// </summary>
    public event DoubleEventHandler? MinimumChanged;

    /// <summary>
    /// Occurs when the <see cref="OnlyNumericValues"/> property changed.
    /// </summary>
    public event BooleanEventHandler? OnlyNumericValuesChanged;

    /// <summary>
    /// Gets or sets the numeric value of the text box. Only works when <see cref="OnlyNumericValues"/> is set to <c>true</c>.
    /// </summary>
    public double NumericValue
    {
        get => GetValue(NumericValueProperty) as double? ?? 0D;
        set => SetValue(NumericValueProperty, value);
    }

    /// <summary>
    /// Gets or sets the decimal places for numeric values.
    /// </summary>
    public int DecimalPlaces
    {
        get => GetValue(DecimalPlacesProperty) as int? ?? 2;
        set => SetValue(DecimalPlacesProperty, value);
    }

    /// <summary>
    /// Gets or sets the maximum numeric value possible.
    /// </summary>
    public double Maximum
    {
        get => GetValue(MaximumProperty) as double? ?? 100D;
        set => SetValue(MaximumProperty, value);
    }

    /// <summary>
    /// Gets or sets the minimum numeric value possible.
    /// </summary>
    public double Minimum
    {
        get => GetValue(MinimumProperty) as double? ?? 0D;
        set => SetValue(MinimumProperty, value);
    }

    /// <summary>
    /// Gets or sets the size of the step that is used for the up and down buttons for numeric values.
    /// </summary>
    public double StepSize
    {
        get => GetValue(StepSizeProperty) as double? ?? 1D;
        set => SetValue(StepSizeProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether only numeric values are allowed for this <see cref="TextBox"/>.
    /// </summary>
    public bool OnlyNumericValues
    {
        get => GetValue(OnlyNumericValuesProperty) as bool? ?? false;
        set => SetValue(OnlyNumericValuesProperty, value);
    }

    /// <summary>
    /// Gets or sets the description in form of a placeholder.
    /// </summary>
    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the up and down buttons for numeric values are shown.
    /// </summary>
    public bool IsUpDownEnabled
    {
        get => GetValue(IsUpDownEnabledProperty) as bool? ?? true;
        set => SetValue(IsUpDownEnabledProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether throusand seperator characters are displayed for numeric values.
    /// </summary>
    public bool IsThrousandSeperatorEnabled
    {
        get => GetValue(IsThrousandSeperatorEnabledProperty) as bool? ?? true;
        set => SetValue(IsThrousandSeperatorEnabledProperty, value);
    }

    /// <summary>
    /// Gets or sets the text to highlight in this text box.
    /// </summary>
    public string HighlightText
    {
        get => (string)GetValue(HighlightTextProperty);
        set => SetValue(HighlightTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the brush that should be used to highlight text.
    /// </summary>
    public Brush HighlightBrush
    {
        get => (Brush)GetValue(HighlightBrushProperty);
        set => SetValue(HighlightBrushProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether only single words should be highlighted.
    /// </summary>
    public bool HighlightSingleWord
    {
        get => GetValue(HighlightSingleWordProperty) as bool? ?? false;
        set => SetValue(HighlightSingleWordProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the highlighting should be case sensitive.
    /// </summary>
    public bool HighlightMatchCase
    {
        get => GetValue(HighlightMatchCaseProperty) as bool? ?? false;
        set => SetValue(HighlightMatchCaseProperty, value);
    }

    /// <summary>
    /// Gets or sets text that is displayed after the input field.
    /// </summary>
    public string Suffix
    {
        get => (string)GetValue(SuffixProperty);
        set => SetValue(SuffixProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the whole text should be selected when the control got focus.
    /// </summary>
    public bool SelectAllOnFocus
    {
        get => GetValue(SelectAllOnFocusProperty) as bool? ?? true;
        set => SetValue(SelectAllOnFocusProperty, value);
    }

    /// <summary>
    /// Gets or sets the content that is displayed on the most right side of the control.
    /// </summary>
    public object EndContent
    {
        get => GetValue(EndContentProperty);
        set => SetValue(EndContentProperty, value);
    }

    /// <summary>
    /// Gets or sets the content that is displayed on the most left side of the control.
    /// </summary>
    public object StartContent
    {
        get => GetValue(StartContentProperty);
        set => SetValue(StartContentProperty, value);
    }

    /// <inheritdoc/>
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _suffixTextBlock = GetTemplateChild("MaSch_Suffix") as TextBlock;
        if (GetTemplateChild("MaSch_Up") is Button btnUp)
        {
#pragma warning disable SA1305 // Field names should not use Hungarian notation
            var upRunning = false;
            var upRunningLock = new object();
#pragma warning restore SA1305 // Field names should not use Hungarian notation
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

        if (GetTemplateChild("MaSch_Down") is Button btnDown)
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

        void NumberChange()
        {
            if (OnlyNumericValues)
            {
                var previewText = Text.Replace(" ", string.Empty);
                var numberSep = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;
                var matches = Regex.Matches(previewText, "(\\-|\\+|\\" + numberSep + "|E|e)(?=([^0-9]|\\Z))");
                previewText = matches.Cast<Match>().Aggregate(previewText, (current, match) => current.Replace(match.Value, match.Value + "0"));
                if (string.IsNullOrEmpty(previewText) || previewText.StartsWith("e0"))
                    previewText = "0";
                NumericValue = double.Parse(previewText);
            }
        }

        if (GetTemplateChild("PART_ContentHost") is System.Windows.Controls.ScrollViewer textField)
        {
            textField.LostFocus += (s, e) => NumberChange();
            textField.PreviewKeyDown += (s, e) =>
            {
                if (e.Key == Key.Enter)
                    NumberChange();
            };
        }
    }

    /// <summary>
    /// Raises the <see cref="E:PaddingChanged" /> event.
    /// </summary>
    /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
    protected virtual void OnPaddingChanged(DependencyPropertyChangedEventArgs e)
    {
        if (_suffixTextBlock != null)
            _suffixTextBlock.Margin = new Thickness(0, Padding.Top, Padding.Right, Padding.Bottom);
    }

    /// <inheritdoc/>
    protected override void OnTextChanged(TextChangedEventArgs e)
    {
        base.OnTextChanged(e);
        InvalidateVisual();
    }

    /// <inheritdoc/>
    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);

        if (Text != string.Empty)
        {
            var leftMargin = BorderThickness.Left + Padding.Left;
            var topMargin = BorderThickness.Top + Padding.Top - VerticalOffset;
            var rightMargin = BorderThickness.Right + Padding.Right;

            var dpi = (PresentationSource.FromVisual(this)?.CompositionTarget?.TransformToDevice.M11 ?? 1D) * 96D;
            var ft = new FormattedText(
                Text,
                CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
                FontSize,
                Foreground,
                new NumberSubstitution(),
                TextFormattingMode.Ideal,
                dpi)
            {
                LineHeight = FontSize * FontFamily.LineSpacing,
                MaxTextWidth = ActualWidth - leftMargin - rightMargin,
            };

            if (!string.IsNullOrEmpty(HighlightText))
            {
                var highlights = HighlightSingleWord ? HighlightText.Split(' ') : new[] { HighlightText };
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
                        {
                            return;
                        }
                    }
                }
            }
        }
    }

    /// <inheritdoc/>
    protected override void OnGotFocus(RoutedEventArgs e)
    {
        base.OnGotFocus(e);
        if (SelectAllOnFocus)
            SelectAll();
    }

    /// <inheritdoc/>
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

    private static void OnOnlyNumericValuesChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
        if (obj is TextBox tbx && e.NewValue != e.OldValue)
        {
            tbx.OnlyNumericValuesChanged?.Invoke(tbx, e.NewValue as bool?);
        }
    }

    private static void OnSuffixChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
        var owner = obj as TextBox;
        if (owner?._suffixTextBlock != null)
            owner._suffixTextBlock.Visibility = string.IsNullOrEmpty((string)e.NewValue) ? Visibility.Collapsed : Visibility.Visible;
    }

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
            if (val == 0D)
                val = 1D;
            return val;
        }

        return 1D;
    }

    private static void OnHighlightChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
        var owner = obj as TextBox;
        owner?.InvalidateVisual();
    }

    private static void OnPaddingChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
        if (obj is TextBox tb)
            tb.OnPaddingChanged(e);
    }

    private void ModernUITextBox_PreviewKeyDown(object sender, KeyEventArgs e)
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
        {
            e.CancelCommand();
        }
    }

    private void ModernUITextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        if (OnlyNumericValues)
        {
            var previewText = Text.Substring(0, SelectionStart) + e.Text +
                Text.Substring(SelectionStart + SelectionLength, Text.Length - SelectionStart - SelectionLength);
            e.Handled = !_valueRegex.IsMatch(previewText.Replace(" ", string.Empty));
        }
    }
}
