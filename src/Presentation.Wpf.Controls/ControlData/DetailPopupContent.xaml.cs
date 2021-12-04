using MaSch.Presentation.Wpf.Controls;
using MaSch.Presentation.Wpf.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using Path = System.Windows.Shapes.Path;

namespace MaSch.Presentation.Wpf.ControlData;

/// <summary>
/// Content for a <see cref="DetailPopup"/>.
/// </summary>
/// <seealso cref="Control" />
[ContentProperty(nameof(PopupContent))]
public class DetailPopupContent : Control
{
    /// <summary>
    /// Dependency property. Gets or sets the arrow position.
    /// </summary>
    public static readonly DependencyProperty ArrowPositionProperty =
        DependencyProperty.Register(
            "ArrowPosition",
            typeof(AnchorStyle),
            typeof(DetailPopupContent),
            new PropertyMetadata(AnchorStyle.Top, OnArrowChanged));

    /// <summary>
    /// Dependency property. Gets or sets the size of the arrow.
    /// </summary>
    public static readonly DependencyProperty ArrowSizeProperty =
        DependencyProperty.Register(
            "ArrowSize",
            typeof(double),
            typeof(DetailPopupContent),
            new PropertyMetadata(16D, OnArrowChanged));

    /// <summary>
    /// Dependency property. Gets or sets the content of the popup.
    /// </summary>
    public static readonly DependencyProperty PopupContentProperty =
        DependencyProperty.Register(
            "PopupContent",
            typeof(UIElement),
            typeof(DetailPopupContent),
            new PropertyMetadata(null));

    private static readonly Dictionary<AnchorStyle, Geometry?> ArrowDataDict = new()
    {
        { AnchorStyle.None, null },
        { AnchorStyle.Left, Geometry.Parse("M 1,0 0,1 1,2") },
        { AnchorStyle.Top, Geometry.Parse("M 0,1 1,0 2,1") },
        { AnchorStyle.Right, Geometry.Parse("M 0,0 1,1 0,2") },
        { AnchorStyle.Bottom, Geometry.Parse("M 0,0 1,1 2,0") },
    };

    private Path? _arrow;
    private FrameworkElement? _border;

    static DetailPopupContent()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(DetailPopupContent), new FrameworkPropertyMetadata(typeof(DetailPopupContent)));
    }

    /// <summary>
    /// Gets or sets the arrow position.
    /// </summary>
    public AnchorStyle ArrowPosition
    {
        get => GetValue(ArrowPositionProperty) as AnchorStyle? ?? AnchorStyle.Top;
        set => SetValue(ArrowPositionProperty, value);
    }

    /// <summary>
    /// Gets or sets the size of the arrow.
    /// </summary>
    public double ArrowSize
    {
        get => GetValue(ArrowSizeProperty) as double? ?? 16D;
        set => SetValue(ArrowSizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the content of the popup.
    /// </summary>
    public UIElement PopupContent
    {
        get => (UIElement)GetValue(PopupContentProperty);
        set => SetValue(PopupContentProperty, value);
    }

    /// <inheritdoc />
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _arrow = GetTemplateChild("PART_Arrow") as Path ?? throw new KeyNotFoundException("Control could not be found: PART_Arrow");
        _border = GetTemplateChild("PART_Border") as FrameworkElement ?? throw new KeyNotFoundException("Control could not be found: PART_Border");

        RefreshArrow();
    }

    private static void OnArrowChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
        var owner = obj as DetailPopupContent;
        owner?.RefreshArrow();
    }

    private void RefreshArrow()
    {
        double shortSize = ArrowSize + 2, longSize = shortSize * 2;
        if (_arrow == null)
            return;
        _arrow.Data = ArrowDataDict[ArrowPosition];
        switch (ArrowPosition)
        {
            case AnchorStyle.Left:
                _arrow.SetSize(shortSize, longSize);
                _arrow.SetAlignment(HorizontalAlignment.Left, VerticalAlignment.Center);
                _arrow.StrokeThickness = BorderThickness.Left;
                _border!.Margin = new Thickness(ArrowSize, 0, 0, 0);
                _border.SetMinSize(0, longSize * 1.25);
                break;
            case AnchorStyle.Top:
                _arrow.SetSize(longSize, shortSize);
                _arrow.SetAlignment(HorizontalAlignment.Center, VerticalAlignment.Top);
                _arrow.StrokeThickness = BorderThickness.Top;
                _border!.Margin = new Thickness(0, ArrowSize, 0, 0);
                _border.SetMinSize(longSize * 1.25, 0);
                break;
            case AnchorStyle.Right:
                _arrow.SetSize(shortSize, longSize);
                _arrow.SetAlignment(HorizontalAlignment.Right, VerticalAlignment.Center);
                _arrow.StrokeThickness = BorderThickness.Right;
                _border!.Margin = new Thickness(0, 0, ArrowSize, 0);
                _border.SetMinSize(0, longSize * 1.25);
                break;
            case AnchorStyle.Bottom:
                _arrow.SetSize(longSize, shortSize);
                _arrow.SetAlignment(HorizontalAlignment.Center, VerticalAlignment.Bottom);
                _arrow.StrokeThickness = BorderThickness.Bottom;
                _border!.Margin = new Thickness(0, 0, 0, ArrowSize);
                _border.SetMinSize(longSize * 1.25, 0);
                break;
            case AnchorStyle.None:
                _border!.Margin = new Thickness(0);
                _border.SetMinSize(0, 0);
                break;
            default:
                throw new ArgumentOutOfRangeException($"The arrow position \"{ArrowPosition}\" is unknown.");
        }
    }
}
