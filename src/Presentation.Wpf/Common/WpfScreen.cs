using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace MaSch.Presentation.Wpf.Common;

/// <summary>
/// Represents a screen where Wpf elements can be rendered. Wraps the <see cref="Screen"/> class.
/// </summary>
public class WpfScreen
{
    private readonly Screen _screen;

    /// <summary>
    /// Initializes a new instance of the <see cref="WpfScreen"/> class.
    /// </summary>
    /// <param name="screen">The screen.</param>
    internal WpfScreen(Screen screen)
    {
        _screen = screen;
    }

    /// <summary>
    /// Gets a <see cref="WpfScreen"/> that represents the primary monitor.
    /// </summary>
    public static WpfScreen Primary => new(Screen.PrimaryScreen);

    /// <summary>
    /// Gets a value indicating whether the screen represented by this <see cref="WpfScreen"/> is the primary monitor.
    /// </summary>
    public bool IsPrimary => _screen.Primary;

    /// <summary>
    /// Gets the name of the device that is represented by this <see cref="WpfScreen"/>.
    /// </summary>
    public string DeviceName => _screen.DeviceName;

    /// <summary>
    /// Gets the bounds of the screen that is represented by this <see cref="WpfScreen"/>.
    /// </summary>
    public Rect DeviceBounds => GetRect(_screen.Bounds);

    /// <summary>
    /// Gets the working area of the screen that is represented by this <see cref="WpfScreen"/>.
    /// </summary>
    public Rect WorkingArea => GetRect(_screen.WorkingArea);

    /// <summary>
    /// Gets instances of the <see cref="WpfScreen"/> class representing all available screens.
    /// </summary>
    /// <returns>A <see cref="IEnumerable{T}"/> cotaining <see cref="WpfScreen"/>s representing all available screens.</returns>
    public static IEnumerable<WpfScreen> AllScreens()
    {
        return Screen.AllScreens.Select(screen => new WpfScreen(screen));
    }

    /// <summary>
    /// Gets a <see cref="WpfScreen"/> that represents the screen, the given window is rendered on.
    /// </summary>
    /// <param name="window">The window that is on the wanted screen.</param>
    /// <returns>A <see cref="WpfScreen"/> that represents the screen, the given window is rendered on.</returns>
    public static WpfScreen GetScreenFrom(Window window)
    {
        var windowInteropHelper = new WindowInteropHelper(window);
        var screen = Screen.FromHandle(windowInteropHelper.Handle);
        var wpfScreen = new WpfScreen(screen);
        return wpfScreen;
    }

    /// <summary>
    /// Gets a <see cref="WpfScreen"/> that represents the screen, the given point is on.
    /// </summary>
    /// <param name="point">The point that is on the wanted screen.</param>
    /// <returns>A <see cref="WpfScreen"/> that represents the screen, the given point is on.</returns>
    public static WpfScreen GetScreenFrom(System.Windows.Point point)
    {
        int x = (int)Math.Round(point.X);
        int y = (int)Math.Round(point.Y);

        // are x,y device-independent-pixels ??
        var drawingPoint = new System.Drawing.Point(x, y);
        var screen = Screen.FromPoint(drawingPoint);
        var wpfScreen = new WpfScreen(screen);

        return wpfScreen;
    }

    private static Rect GetRect(Rectangle value)
    {
        // should x, y, width, height be device-independent-pixels ??
        return new Rect
        {
            X = value.X,
            Y = value.Y,
            Width = value.Width,
            Height = value.Height,
        };
    }
}
