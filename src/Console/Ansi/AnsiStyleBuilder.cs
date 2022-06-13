using MaSch.Core;

namespace MaSch.Console.Ansi;

/// <summary>
/// Represents a mutable <see cref="AnsiStyle"/>.
/// </summary>
public class AnsiStyleBuilder
{
    private AnsiTextStyle _addedStyles = AnsiTextStyle.None;
    private AnsiTextStyle _removedStyles = AnsiTextStyle.None;
    private AnsiColor _foreground = AnsiColor.None;
    private AnsiColor _background = AnsiColor.None;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnsiStyleBuilder"/> class.
    /// </summary>
    public AnsiStyleBuilder()
    {
    }

    /// <summary>
    /// Adds the information to this <see cref="AnsiStyle"/> instance that the specified styles should be used.
    /// </summary>
    /// <param name="styles">The styles to use.</param>
    /// <returns>The same <see cref="AnsiStyleBuilder"/> instance this method was called on.</returns>
    public AnsiStyleBuilder WithStyles(AnsiTextStyle styles)
    {
        _ = Guard.NotUndefinedFlagInEnumValue(styles);
        _addedStyles |= styles;
        _removedStyles ^= styles & _removedStyles;
        return this;
    }

    /// <summary>
    /// Adds the information to this <see cref="AnsiStyle"/> instance that the specified styles should not be used.
    /// </summary>
    /// <param name="styles">The styles to not use.</param>
    /// <returns>The same <see cref="AnsiStyleBuilder"/> instance this method was called on.</returns>
    public AnsiStyleBuilder WithoutStyles(AnsiTextStyle styles)
    {
        _ = Guard.NotUndefinedFlagInEnumValue(styles);
        _addedStyles ^= styles & _addedStyles;
        _removedStyles |= styles;
        return this;
    }

    /// <summary>
    /// Adds the information to this <see cref="AnsiStyle"/> instance that the specified styles should be used exclusively.
    /// </summary>
    /// <param name="exactStyles">The exact styles to use.</param>
    /// <returns>The same <see cref="AnsiStyleBuilder"/> instance this method was called on.</returns>
    public AnsiStyleBuilder OverrideStyles(AnsiTextStyle exactStyles)
    {
        _ = Guard.NotUndefinedFlagInEnumValue(exactStyles);
        _addedStyles = exactStyles;
        _removedStyles = AnsiTextStyle.All ^ AnsiTextStyle.Invert ^ exactStyles;
        return this;
    }

    /// <summary>
    /// Adds the information to this <see cref="AnsiStyle"/> instance that text should be bold.
    /// </summary>
    /// <returns>The same <see cref="AnsiStyleBuilder"/> instance this method was called on.</returns>
    public AnsiStyleBuilder Bold()
    {
        return WithStyles(AnsiTextStyle.Bold);
    }

    /// <summary>
    /// Adds the information to this <see cref="AnsiStyle"/> instance that text should be fainted, dimmed or have decreased intensity.
    /// </summary>
    /// <returns>The same <see cref="AnsiStyleBuilder"/> instance this method was called on.</returns>
    public AnsiStyleBuilder Faint()
    {
        return WithStyles(AnsiTextStyle.Faint);
    }

    /// <summary>
    /// Adds the information to this <see cref="AnsiStyle"/> instance that text should be italic.
    /// </summary>
    /// <returns>The same <see cref="AnsiStyleBuilder"/> instance this method was called on.</returns>
    public AnsiStyleBuilder Italic()
    {
        return WithStyles(AnsiTextStyle.Italic);
    }

    /// <summary>
    /// Adds the information to this <see cref="AnsiStyle"/> instance that text should be underlined.
    /// </summary>
    /// <returns>The same <see cref="AnsiStyleBuilder"/> instance this method was called on.</returns>
    public AnsiStyleBuilder Underlined()
    {
        return WithStyles(AnsiTextStyle.Underline);
    }

    /// <summary>
    /// Adds the information to this <see cref="AnsiStyle"/> instance that text should blink.
    /// </summary>
    /// <returns>The same <see cref="AnsiStyleBuilder"/> instance this method was called on.</returns>
    public AnsiStyleBuilder Blinking()
    {
        return WithStyles(AnsiTextStyle.Blink);
    }

    /// <summary>
    /// Adds the information to this <see cref="AnsiStyle"/> instance that foreground and background colors should be swapped.
    /// </summary>
    /// <returns>The same <see cref="AnsiStyleBuilder"/> instance this method was called on.</returns>
    public AnsiStyleBuilder Inverted()
    {
        return WithStyles(AnsiTextStyle.Invert);
    }

    /// <summary>
    /// Adds the information to this <see cref="AnsiStyle"/> instance that text should be crossed-out.
    /// </summary>
    /// <returns>The same <see cref="AnsiStyleBuilder"/> instance this method was called on.</returns>
    public AnsiStyleBuilder CrossedOut()
    {
        return WithStyles(AnsiTextStyle.CrossedOut);
    }

    /// <summary>
    /// Adds the information to this <see cref="AnsiStyle"/> instance that text should be doubly underlined.
    /// </summary>
    /// <returns>The same <see cref="AnsiStyleBuilder"/> instance this method was called on.</returns>
    public AnsiStyleBuilder DoublyUnderlined()
    {
        return WithStyles(AnsiTextStyle.DoublyUnderlined);
    }

    /// <summary>
    /// Adds the information to this <see cref="AnsiStyle"/> instance that text should be overlined.
    /// </summary>
    /// <returns>The same <see cref="AnsiStyleBuilder"/> instance this method was called on.</returns>
    public AnsiStyleBuilder Overlined()
    {
        return WithStyles(AnsiTextStyle.Overlined);
    }

    /// <summary>
    /// Adds the information to this <see cref="AnsiStyle"/> instance that text should not be bold, fainted, dimmed or have decreased intensity.
    /// </summary>
    /// <returns>The same <see cref="AnsiStyleBuilder"/> instance this method was called on.</returns>
    public AnsiStyleBuilder NotBoldOrFaint()
    {
        return WithoutStyles(AnsiTextStyle.Bold | AnsiTextStyle.Faint);
    }

    /// <summary>
    /// Adds the information to this <see cref="AnsiStyle"/> instance that text should not be italic.
    /// </summary>
    /// <returns>The same <see cref="AnsiStyleBuilder"/> instance this method was called on.</returns>
    public AnsiStyleBuilder NotItalic()
    {
        return WithoutStyles(AnsiTextStyle.Italic);
    }

    /// <summary>
    /// Adds the information to this <see cref="AnsiStyle"/> instance that text should not be underlined.
    /// </summary>
    /// <returns>The same <see cref="AnsiStyleBuilder"/> instance this method was called on.</returns>
    public AnsiStyleBuilder NotUnderlined()
    {
        return WithoutStyles(AnsiTextStyle.Underline | AnsiTextStyle.DoublyUnderlined);
    }

    /// <summary>
    /// Adds the information to this <see cref="AnsiStyle"/> instance that text should not blink.
    /// </summary>
    /// <returns>The same <see cref="AnsiStyleBuilder"/> instance this method was called on.</returns>
    public AnsiStyleBuilder NotBlinking()
    {
        return WithoutStyles(AnsiTextStyle.Blink);
    }

    /// <summary>
    /// Adds the information to this <see cref="AnsiStyle"/> instance that text should not be crossed-out.
    /// </summary>
    /// <returns>The same <see cref="AnsiStyleBuilder"/> instance this method was called on.</returns>
    public AnsiStyleBuilder NotCrossedOut()
    {
        return WithoutStyles(AnsiTextStyle.CrossedOut);
    }

    /// <summary>
    /// Adds the information to this <see cref="AnsiStyle"/> instance that text should not be overlined.
    /// </summary>
    /// <returns>The same <see cref="AnsiStyleBuilder"/> instance this method was called on.</returns>
    public AnsiStyleBuilder NotOverlined()
    {
        return WithoutStyles(AnsiTextStyle.Overlined);
    }

    /// <summary>
    /// Adds the information to this <see cref="AnsiStyle"/> instance that text should have the specified foreground color.
    /// </summary>
    /// <param name="color">The foreground color to use.</param>
    /// <returns>The same <see cref="AnsiStyleBuilder"/> instance this method was called on.</returns>
    public AnsiStyleBuilder Foreground(AnsiColor color)
    {
        _foreground = color;
        return this;
    }

    /// <summary>
    /// Adds the information to this <see cref="AnsiStyle"/> instance that text should have the specified background color.
    /// </summary>
    /// <param name="color">The background color to use.</param>
    /// <returns>The same <see cref="AnsiStyleBuilder"/> instance this method was called on.</returns>
    public AnsiStyleBuilder Background(AnsiColor color)
    {
        _background = color;
        return this;
    }

    /// <summary>
    /// Clears the data of this <see cref="AnsiStyleBuilder"/>.
    /// </summary>
    /// <returns>The same <see cref="AnsiStyleBuilder"/> instance this method was called on.</returns>
    public AnsiStyleBuilder Clear()
    {
        _addedStyles = AnsiTextStyle.None;
        _removedStyles = AnsiTextStyle.None;
        _foreground = AnsiColor.None;
        _background = AnsiColor.None;
        return this;
    }

    /// <summary>
    /// Builds an <see cref="AnsiStyle"/> instance representing the current state of the <see cref="AnsiStyleBuilder"/>.
    /// </summary>
    /// <returns>An instance of type <see cref="AnsiStyle"/>.</returns>
    public AnsiStyle Build() => new(_addedStyles, _removedStyles, _foreground, _background);
}
