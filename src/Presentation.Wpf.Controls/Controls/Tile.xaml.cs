using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace MaSch.Presentation.Wpf.Controls;

/// <summary>
/// Button that is displayed as a tile.
/// </summary>
/// <seealso cref="Button" />
public class Tile : Button
{
    private const double CenterBox = 0.4D;
    private const double EdgeBox = 0.133334D;

    private RotateTransform3D? _rotate;
    private TranslateTransform3D? _translate;
    private AxisAngleRotation3D? _angle;
    private Grid? _sizeGrid;

    static Tile()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Tile), new FrameworkPropertyMetadata(typeof(Tile)));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Tile"/> class.
    /// </summary>
    public Tile()
    {
        PreviewMouseLeftButtonDown += Viewport3D_MouseLeftButtonDown;
        PreviewMouseLeftButtonUp += Viewport3D_MouseLeftButtonUp;
        SizeChanged += ModernUITile_SizeChanged;
    }

    /// <summary>
    /// Gets the flip direction depending on the mouse position.
    /// </summary>
    /// <param name="mousePos">The mouse position.</param>
    /// <param name="size">The size.</param>
    /// <param name="border">The border.</param>
    /// <returns>A value representing the direction the tile should flip.</returns>
    public static FlipDirection GetFlipDirection(Point mousePos, double size, double border)
    {
        var pwb = new Point(mousePos.X - border, mousePos.Y - border); // Point without border
        if (pwb.X < 0)
            pwb.X = 0;
        else if (pwb.X > size)
            pwb.X = size;
        if (pwb.Y < 0)
            pwb.Y = 0;
        else if (pwb.Y > size)
            pwb.Y = size;

        var centerBorder = size / 2 * (1 - CenterBox);
        var centerBoxRect = new Rect(centerBorder, centerBorder, CenterBox * size, CenterBox * size);
        if (centerBoxRect.Contains(pwb))
            return FlipDirection.Center;

        var edgeBoxSize = EdgeBox * size;
        Rect edgeTl = new(0, 0, edgeBoxSize, edgeBoxSize),
             edgeTr = new(size - edgeBoxSize, 0, edgeBoxSize, edgeBoxSize),
             edgeBl = new(0, size - edgeBoxSize, edgeBoxSize, edgeBoxSize),
             edgeBr = new(size - edgeBoxSize, size - edgeBoxSize, edgeBoxSize, edgeBoxSize);
        if (edgeTl.Contains(pwb))
            return FlipDirection.TopLeft;
        if (edgeTr.Contains(pwb))
            return FlipDirection.TopRight;
        if (edgeBl.Contains(pwb))
            return FlipDirection.BottomLeft;
        if (edgeBr.Contains(pwb))
            return FlipDirection.BottomRight;

        if (pwb.X < pwb.Y)
        {
            return pwb.X < size - pwb.Y ? FlipDirection.Left : FlipDirection.Bottom;
        }

        return size - pwb.X < pwb.Y ? FlipDirection.Right : FlipDirection.Top;
    }

    /// <inheritdoc/>
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        _rotate = GetTemplateChild("TILE_Rotate") as RotateTransform3D ?? throw new KeyNotFoundException($"Could not found control \"TILE_Rotate\".");
        _translate = GetTemplateChild("TILE_Translate") as TranslateTransform3D ?? throw new KeyNotFoundException($"Could not found control \"TILE_Translate\".");
        _angle = GetTemplateChild("TILE_Angle") as AxisAngleRotation3D ?? throw new KeyNotFoundException($"Could not found control \"TILE_Angle\".");
        _sizeGrid = GetTemplateChild("SizeGrid") as Grid ?? throw new KeyNotFoundException($"Could not found control \"SizeGrid\".");
    }

    /// <inheritdoc/>
    protected override void OnIsPressedChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnIsPressedChanged(e);
        if (IsPressed)
        {
            var pos = Mouse.GetPosition(this);
            var fd = GetFlipDirection(pos, 120, 3);
            BeginPressAnimation(fd);
        }
        else
        {
            BeginReleaseAnimation();
        }
    }

    private void ModernUITile_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        _sizeGrid!.Margin = new Thickness(-ActualWidth / 2, -ActualHeight / 2, -ActualWidth / 2, -ActualHeight / 2);
    }

    private void Viewport3D_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        _ = CaptureMouse();
    }

    private void Viewport3D_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        ReleaseMouseCapture();
        if (IsMouseOver)
            OnClick();
    }

    private void SetRotation(double centerX, double centerY, double axisX, double axisY)
    {
        _rotate!.CenterX = centerX;
        _rotate.CenterY = centerY;
        _rotate.CenterZ = 0;
        _angle!.Axis = new Vector3D(axisX, axisY, 0);

        _angle.BeginAnimation(
            AxisAngleRotation3D.AngleProperty,
            new DoubleAnimation(3.6, new Duration(TimeSpan.FromSeconds(0.1))));
    }

    private void BeginReleaseAnimation()
    {
        _angle!.BeginAnimation(
            AxisAngleRotation3D.AngleProperty,
            new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(0.1))));

        _translate!.BeginAnimation(
            TranslateTransform3D.OffsetZProperty,
            new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(0.1))));
    }

    private void BeginPressAnimation(FlipDirection fd)
    {
        switch (fd)
        {
            case FlipDirection.Center:
                _translate!.BeginAnimation(
                    TranslateTransform3D.OffsetZProperty,
                    new DoubleAnimation(-0.025, new Duration(TimeSpan.FromSeconds(0.1))));
                break;
            case FlipDirection.Right:
                SetRotation(0, 0, 0, 1);
                break;
            case FlipDirection.Left:
                SetRotation(1, 0, 0, -1);
                break;
            case FlipDirection.Top:
                SetRotation(0, 0, -1, 0);
                break;
            case FlipDirection.Bottom:
                SetRotation(0, 1, 1, 0);
                break;
            case FlipDirection.TopLeft:
                SetRotation(1, 0, -1, -1);
                break;
            case FlipDirection.TopRight:
                SetRotation(0, 0, -1, 1);
                break;
            case FlipDirection.BottomLeft:
                SetRotation(1, 1, 1, -1);
                break;
            case FlipDirection.BottomRight:
                SetRotation(0, 1, 1, 1);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(fd), fd, null);
        }
    }
}
