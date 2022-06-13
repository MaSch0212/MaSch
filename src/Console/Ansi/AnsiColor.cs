using MaSch.Core;
using System.Drawing;

namespace MaSch.Console.Ansi;

/// <summary>
/// Represents a color settable using an ANSI escape sequence.
/// </summary>
public readonly struct AnsiColor
{
    private readonly AnsiColorType _type;
    private readonly AnsiColorCode _colorCode;
    private readonly byte _red;
    private readonly byte _green;
    private readonly byte _blue;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnsiColor"/> struct.
    /// </summary>
    private AnsiColor(AnsiColorType type)
    {
        _type = type;
        _colorCode = default;
        _red = _green = _blue = default;
    }

    private AnsiColor(AnsiColorCode color)
    {
        _type = AnsiColorType.ColorCode;
        _colorCode = Guard.NotUndefinedEnumMember(color, AnsiColorCodeExtensions.IsDefined, AnsiColorCodeExtensions.ToStringFast);
        _red = _green = _blue = default;
    }

    private AnsiColor(byte red, byte green, byte blue)
    {
        _type = AnsiColorType.Rgb;
        _colorCode = default;
        _red = red;
        _green = green;
        _blue = blue;
    }

    private AnsiColor(int red, int green, int blue)
    {
        _type = AnsiColorType.Rgb;
        _colorCode = default;
        _red = (byte)Guard.NotOutOfRange(red, 0, 255);
        _green = (byte)Guard.NotOutOfRange(green, 0, 255);
        _blue = (byte)Guard.NotOutOfRange(blue, 0, 255);
    }

    private enum AnsiColorType
    {
        None,
        Default,
        ColorCode,
        Rgb,
    }

    /// <summary>
    /// Gets an <see cref="AnsiColor"/> insstance representing no color.
    /// </summary>
    public static AnsiColor None => new(AnsiColorType.None);

    /// <summary>
    /// Gets the default <see cref="AnsiColor"/>.
    /// </summary>
    public static AnsiColor Default => new(AnsiColorType.Default);

    /// <summary>
    /// Gets the ANSI escape sequence for setting the foreground to the color represented by this <see cref="AnsiColor"/>.
    /// </summary>
    public string ForegroundSequence =>
        _type switch
        {
            AnsiColorType.ColorCode => AnsiEscapeUtility.GetSetForegroundColor(_colorCode),
            AnsiColorType.Rgb => AnsiEscapeUtility.GetSetForegroundColor(_red, _green, _blue),
            AnsiColorType.Default => AnsiEscapeUtility.GetResetForegroundColor(),
            _ => string.Empty,
        };

    /// <summary>
    /// Gets the ANSI escape sequence for setting the background to the color represented by this <see cref="AnsiColor"/>.
    /// </summary>
    public string BackgroundSequence =>
        _type switch
        {
            AnsiColorType.ColorCode => AnsiEscapeUtility.GetSetBackgroundColor(_colorCode),
            AnsiColorType.Rgb => AnsiEscapeUtility.GetSetBackgroundColor(_red, _green, _blue),
            AnsiColorType.Default => AnsiEscapeUtility.GetResetBackgroundColor(),
            _ => string.Empty,
        };

    /// <summary>
    /// Gets a value indicating whether this <see cref="AnsiColor"/> represents any color.
    /// </summary>
    public bool HasColor => _type != AnsiColorType.None;

    /// <summary>
    /// Creates an <see cref="AnsiColor"/> using a specified <see cref="AnsiColorCode"/>.
    /// </summary>
    /// <param name="colorCode">The <see cref="AnsiColorCode"/> to use.</param>
    /// <returns>A <see cref="AnsiColor"/> instance that represents the <paramref name="colorCode"/>.</returns>
    public static AnsiColor FromColorCode(AnsiColorCode colorCode)
    {
        return new(colorCode);
    }

    /// <summary>
    /// Creates an <see cref="AnsiColor"/> using specified RGB values.
    /// </summary>
    /// <param name="red">The red channel (0-255).</param>
    /// <param name="green">The green channel (0-255).</param>
    /// <param name="blue">The blue channel (0-255).</param>
    /// <returns>A <see cref="AnsiColor"/> instance that represents the given RGB values.</returns>
    public static AnsiColor FromRgb(int red, int green, int blue)
    {
        return new(red, green, blue);
    }

    /// <summary>
    /// Creates an <see cref="AnsiColor"/> using specified RGB values.
    /// </summary>
    /// <param name="red">The red channel (0-255).</param>
    /// <param name="green">The green channel (0-255).</param>
    /// <param name="blue">The blue channel (0-255).</param>
    /// <returns>A <see cref="AnsiColor"/> instance that represents the given RGB values.</returns>
    public static AnsiColor FromRgb(byte red, byte green, byte blue)
    {
        return new(red, green, blue);
    }

    /// <summary>
    /// Creates an <see cref="AnsiColor"/> using a specified <see cref="Color"/> instance.
    /// </summary>
    /// <param name="color">The <see cref="Color"/> to use.</param>
    /// <returns>A <see cref="AnsiColor"/> instance that represents the <paramref name="color"/>.</returns>
    public static AnsiColor FromColor(Color color)
    {
        return new(color.R, color.G, color.B);
    }

    /// <summary>
    /// Creates an <see cref="AnsiColor"/> using a specified <see cref="ConsoleColor"/>.
    /// </summary>
    /// <param name="color">The <see cref="ConsoleColor"/> to use.</param>
    /// <returns>A <see cref="AnsiColor"/> instance that represents the <paramref name="color"/>.</returns>
    public static AnsiColor FromConsoleColor(ConsoleColor color)
    {
        return new(
            color switch
            {
                ConsoleColor.Black => AnsiColorCode.Black,
                ConsoleColor.DarkBlue => AnsiColorCode.DarkBlue,
                ConsoleColor.DarkGreen => AnsiColorCode.DarkGreen,
                ConsoleColor.DarkCyan => AnsiColorCode.DarkCyan,
                ConsoleColor.DarkRed => AnsiColorCode.DarkRed,
                ConsoleColor.DarkMagenta => AnsiColorCode.DarkMagenta,
                ConsoleColor.DarkYellow => AnsiColorCode.DarkYellow,
                ConsoleColor.Gray => AnsiColorCode.Gray,
                ConsoleColor.DarkGray => AnsiColorCode.DarkGray,
                ConsoleColor.Blue => AnsiColorCode.Blue,
                ConsoleColor.Green => AnsiColorCode.Green,
                ConsoleColor.Cyan => AnsiColorCode.Cyan,
                ConsoleColor.Red => AnsiColorCode.Red,
                ConsoleColor.Magenta => AnsiColorCode.Magenta,
                ConsoleColor.Yellow => AnsiColorCode.Yellow,
                ConsoleColor.White => AnsiColorCode.White,
                _ => throw new ArgumentOutOfRangeException(nameof(color)),
            });
    }

    /// <summary>
    /// Returns the string representation of the color represented by this struct.
    /// </summary>
    /// <returns>The string representation of the color.</returns>
    public override string ToString()
    {
        return _type switch
        {
            AnsiColorType.ColorCode => _colorCode.ToStringFast(),
            AnsiColorType.Rgb => $"#{_red:x2}{_green:x2}{_blue:x2} - rgb({_red}, {_green}, {_blue})",
            AnsiColorType.Default => "Default",
            _ => "None",
        };
    }
}