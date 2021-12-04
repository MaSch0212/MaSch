namespace MaSch.Presentation.Wpf.ColorPicker;

/// <summary>
/// Represents a color in HSV format.
/// </summary>
internal struct HsvColor
{
    /// <summary>
    /// The hue of the color.
    /// </summary>
    public double H;

    /// <summary>
    /// The saturation of the color.
    /// </summary>
    public double S;

    /// <summary>
    /// The color value.
    /// </summary>
    public double V;

    /// <summary>
    /// Initializes a new instance of the <see cref="HsvColor"/> struct.
    /// </summary>
    /// <param name="h">The hue.</param>
    /// <param name="s">The saturation.</param>
    /// <param name="v">The color value.</param>
    public HsvColor(double h, double s, double v)
    {
        H = h;
        S = s;
        V = v;
    }
}
