using MaSch.Core;

#pragma warning disable SA1629 // Documentation text should end with a period

namespace MaSch.Console.Ansi;

/// <summary>
/// Provides functionality in regards to ANSI escape sequences.
/// For more information about ANSI escape sequences see <see href="https://en.wikipedia.org/wiki/ANSI_escape_code"/>.
/// </summary>
public static class AnsiEscapeUtility
{
    /// <summary>
    /// The ANSI escape code starting character (ESC) (0x1B).
    /// </summary>
    public static readonly char ESC = '\u001b';

    /// <summary>
    /// Gets a regular expression that matches an ANSI escape sequence.
    /// </summary>
    public static Regex EscapeSequenceRegex { get; } = new(@"(?:\x1B[78@-Z\\-_]|[\x80-\x9A\x9C-\x9F]|(?:\x1B\[|\x9B)[0-?]*[ -/]*[@-~])", RegexOptions.Compiled);

    /// <summary>
    /// Gets the ANSI escape sequence for "Cursor Up" (CUU).<br/>
    /// Moves the cursor <paramref name="count"/> cells up. If the cursor is already at the edge of the screen, this has no effect.
    /// </summary>
    /// <param name="count">The number of times to move the cursor up.</param>
    /// <returns><c>ESC [ <i><paramref name="count"/></i> A</c></returns>
    public static string GetCursorUp(int count)
    {
        _ = Guard.NotSmallerThan(count, nameof(count), 0);
        return $"{ESC}[{count}A";
    }

    /// <summary>
    /// Gets the ANSI escape sequence for "Cursor Down" (CUD).<br/>
    /// Moves the cursor <paramref name="count"/> cells down. If the cursor is already at the edge of the screen, this has no effect.
    /// </summary>
    /// <param name="count">The number of times to move the cursor down.</param>
    /// <returns><c>ESC [ <i><paramref name="count"/></i> B</c></returns>
    public static string GetCursorDown(int count)
    {
        _ = Guard.NotSmallerThan(count, nameof(count), 0);
        return $"{ESC}[{count}B";
    }

    /// <summary>
    /// Gets the ANSI escape sequence for "Cursor Back" (CUB).<br/>
    /// Moves the cursor <paramref name="count"/> cells back. If the cursor is already at the edge of the screen, this has no effect.
    /// </summary>
    /// <param name="count">The number of times to move the cursor back.</param>
    /// <returns><c>ESC [ <i><paramref name="count"/></i> D</c></returns>
    public static string GetCursorBack(int count)
    {
        _ = Guard.NotSmallerThan(count, nameof(count), 0);
        return $"{ESC}[{count}D";
    }

    /// <summary>
    /// Gets the ANSI escape sequence for "Cursor Forward" (CUF).<br/>
    /// Moves the cursor <paramref name="count"/> cells forward. If the cursor is already at the edge of the screen, this has no effect.
    /// </summary>
    /// <param name="count">The number of times to move the cursor forward.</param>
    /// <returns><c>ESC [ <i><paramref name="count"/></i> C</c></returns>
    public static string GetCursorForward(int count)
    {
        _ = Guard.NotSmallerThan(count, nameof(count), 0);
        return $"{ESC}[{count}C";
    }

    /// <summary>
    /// Gets the ANSI escape sequence for "Cursor Next Line" (CNL).<br/>
    /// Moves cursor to beginning of the line <paramref name="count"/> lines down.
    /// </summary>
    /// <param name="count">The number of lines to move.</param>
    /// <returns><c>ESC [ <i><paramref name="count"/></i> E</c></returns>
    public static string GetCursorNextLine(int count)
    {
        _ = Guard.NotSmallerThan(count, nameof(count), 0);
        return $"{ESC}[{count}E";
    }

    /// <summary>
    /// Gets the ANSI escape sequence for "Cursor Previous Line" (CPL).<br/>
    /// Moves cursor to beginning of the line <paramref name="count"/> lines up.
    /// </summary>
    /// <param name="count">The number of lines to move.</param>
    /// <returns><c>ESC [ <i><paramref name="count"/></i> F</c></returns>
    public static string GetCursorPreviousLine(int count)
    {
        _ = Guard.NotSmallerThan(count, nameof(count), 0);
        return $"{ESC}[{count}F";
    }

    /// <summary>
    /// Gets the ANSI escape sequence for "Cursor Horizontal Absolute" (CHA).<br/>
    /// Moves the cursor to <paramref name="column"/>.
    /// </summary>
    /// <param name="column">The column (starting with 1) to move the cursor to.</param>
    /// <returns><c>ESC [ <i><paramref name="column"/></i> G</c></returns>
    public static string GetCursorToColumn(int column)
    {
        _ = Guard.NotSmallerThan(column, nameof(column), 0);
        return $"{ESC}[{column}G";
    }

    /// <summary>
    /// Gets the ANSI escape sequence for "Cursor Position" (CUP).<br/>
    /// Moves the cursor to <paramref name="row"/>, <paramref name="column"/>.
    /// </summary>
    /// <param name="row">The row (starting with 1) to move the cursor to.</param>
    /// <param name="column">The column (starting with 1) to move the cursor to.</param>
    /// <returns><c>ESC [ <i><paramref name="row"/></i> ; <i><paramref name="column"/></i> H</c></returns>
    public static string GetCursorToPosition(int row, int column)
    {
        _ = Guard.NotSmallerThan(row, nameof(row), 0);
        _ = Guard.NotSmallerThan(column, nameof(column), 0);
        return $"{ESC}[{row};{column}H";
    }

    /// <summary>
    /// Gets the ANSI escape sequence for "Erase in Display" (ED).<br/>
    /// Clears part of the screen.
    /// </summary>
    /// <param name="mode">The clear mode to use.</param>
    /// <returns><c>ESC [ <i><paramref name="mode"/></i> J</c></returns>
    public static string GetEraseScreen(AnsiClearMode mode)
    {
        _ = Guard.NotUndefinedEnumMember(mode, nameof(mode));
        return $"{ESC}[{(int)mode}J";
    }

    /// <summary>
    /// Gets the ANSI escape sequence for "Erase in Line" (EL).<br/>
    /// Erases part of the line.
    /// </summary>
    /// <param name="mode">The clear mode to use.</param>
    /// <returns><c>ESC [ <i><paramref name="mode"/></i> K</c></returns>
    public static string GetEraseLine(AnsiLineClearMode mode)
    {
        _ = Guard.NotUndefinedEnumMember(mode, nameof(mode));
        return $"{ESC}[{(int)mode}K";
    }

    /// <summary>
    /// Gets the ANSI escape sequence for "Scroll Up" (SU).<br/>
    /// Scroll whole page up by <paramref name="count"/> lines. New lines are added at the bottom.
    /// </summary>
    /// <param name="count">The number of lines to scroll.</param>
    /// <returns><c>ESC [ <i><paramref name="count"/></i> S</c></returns>
    public static string GetScrollUp(int count)
    {
        _ = Guard.NotSmallerThan(count, nameof(count), 0);
        return $"{ESC}[{count}S";
    }

    /// <summary>
    /// Gets the ANSI escape sequence for "Scroll Down" (SD).<br/>
    /// Scroll whole page down by <paramref name="count"/> lines. New lines are added at the top.
    /// </summary>
    /// <param name="count">The number of lines to scroll.</param>
    /// <returns><c>ESC [ <i><paramref name="count"/></i> T</c></returns>
    public static string GetScrollDown(int count)
    {
        _ = Guard.NotSmallerThan(count, nameof(count), 0);
        return $"{ESC}[{count}T";
    }

    /// <summary>
    /// Gets the ANSI escape sequence for deleting lines.<br/>
    /// Deletes <paramref name="count"/> lines beginning with the line the cursor is currently at.
    /// After lines are deleted the cursor is at the start of the line after the last deleted line.
    /// </summary>
    /// <param name="count">The number of lines to delete.</param>
    /// <returns><c>ESC [ <i><paramref name="count"/></i> M</c></returns>
    public static string GetDeleteLines(int count)
    {
        _ = Guard.NotSmallerThan(count, nameof(count), 0);
        return $"{ESC}[{count}M";
    }

    /// <summary>
    /// Gets the ANSI escape sequence for iserting lines.<br/>
    /// Inserts <paramref name="count"/> lines before the line the cursor is currently at.
    /// After lines are inserted the cursor is at the start of the first inserted line.
    /// </summary>
    /// <param name="count">The number of lines to insert.</param>
    /// <returns><c>ESC [ <i><paramref name="count"/></i> L</c></returns>
    public static string GetInsertLines(int count)
    {
        _ = Guard.NotSmallerThan(count, nameof(count), 0);
        return $"{ESC}[{count}L";
    }

    /// <summary>
    /// Gets the ANSI escape sequence for "Erase Character" (ECH).<br/>
    /// Erases <paramref name="count"/> characters in the line the cursor is currently at from the cursors position.
    /// Characters that occur after the erased characters are not moved.
    /// After characters are erased the cursor is at the same position as before.
    /// </summary>
    /// <param name="count">The number of characters to erase.</param>
    /// <returns><c>ESC [ <i><paramref name="count"/></i> X</c></returns>
    public static string GetEraseCharacters(int count)
    {
        _ = Guard.NotSmallerThan(count, nameof(count), 0);
        return $"{ESC}[{count}X";
    }

    /// <summary>
    /// Gets the ANSI escape sequence for "Delete Character" (DCH).<br/>
    /// Deletes <paramref name="count"/> characters in the line the cursor is currently at from the cursors position.
    /// Characters that occur after the deleted characters are moved <paramref name="count"/> cells backwards.
    /// After characters are erased the cursor is at the same position as before.
    /// </summary>
    /// <param name="count">The number of characters to erase.</param>
    /// <returns><c>ESC [ <i><paramref name="count"/></i> P</c></returns>
    public static string GetDeleteCharacters(int count)
    {
        _ = Guard.NotSmallerThan(count, nameof(count), 0);
        return $"{ESC}[{count}P";
    }

    /// <summary>
    /// Gets the ANSI escape sequence for "Insert Character" (ICH).<br/>
    /// Inserts <paramref name="count"/> characters in the line the cursor is currently at from the cursors position.
    /// Characters that occur after the cursor are moved <paramref name="count"/> cells forward.
    /// After characters are inserted the cursor is at the same position as before.
    /// </summary>
    /// <param name="count">The number of characters to insert.</param>
    /// <returns><c>ESC [ <i><paramref name="count"/></i> @</c></returns>
    public static string GetInsertCharacters(int count)
    {
        _ = Guard.NotSmallerThan(count, nameof(count), 0);
        return $"{ESC}[{count}@";
    }

    /// <summary>
    /// Gets the ANSI escape sequence for "DEC Save Cursor" (DECSC).<br/>
    /// Saves the cursor position, encoding shift state and formatting attributes.
    /// </summary>
    /// <returns><c>ESC 7</c></returns>
    public static string GetSaveCursor()
    {
        return $"{ESC}7";
    }

    /// <summary>
    /// Gets the ANSI escape sequence for "DEC Restore Cursor" (DECRC).<br/>
    /// Restores the cursor position, encoding shift state and formatting attribute from the previous DECSC if any,
    /// otherwise resets these all to their defaults.
    /// </summary>
    /// <returns><c>ESC 8</c></returns>
    public static string GetRestoreCursor()
    {
        return $"{ESC}8";
    }

    /// <summary>
    /// Gets the ANSI escape sequence for hiding the cursor (DECTCEM).
    /// </summary>
    /// <returns><c>ESC [ ? 25 h</c></returns>
    public static string GetHideCursor()
    {
        return $"{ESC}[?25l";
    }

    /// <summary>
    /// Gets the ANSI escape sequence for showing the cursor (DECTCEM).
    /// </summary>
    /// <returns><c>ESC [ ? 25 l</c></returns>
    public static string GetShowCursor()
    {
        return $"{ESC}[?25h";
    }

    /// <summary>
    /// Gets the ANSI escape sequence for resetting the "Select Graphic Rendition" (SGR).
    /// Resets all text styles to normal.
    /// </summary>
    /// <returns><c>ESC [ 0 m</c></returns>
    public static string GetResetStyle()
    {
        return $"{ESC}[0m";
    }

    /// <summary>
    /// Gets the ANSI escape sequence for adding a style attribute using the "Select Graphic Rendition" (SGR) sequence.
    /// </summary>
    /// <param name="style">The style attributes to add.</param>
    /// <returns><c>ESC [ <i>#</i> m</c></returns>
    public static string GetAddStyle(AnsiTextStyle style)
    {
        _ = Guard.NotUndefinedFlagInEnumValue(style, nameof(style));

        static void Add(StringBuilder sb, int n) => sb.Append($"{ESC}[{n}m");

        var result = new StringBuilder();
        if (style.HasFlag(AnsiTextStyle.Bold))
            Add(result, 1);
        if (style.HasFlag(AnsiTextStyle.Faint))
            Add(result, 2);
        if (style.HasFlag(AnsiTextStyle.Italic))
            Add(result, 3);
        if (style.HasFlag(AnsiTextStyle.Underline))
            Add(result, 4);
        if (style.HasFlag(AnsiTextStyle.Blink))
            Add(result, 5);
        if (style.HasFlag(AnsiTextStyle.Invert))
            Add(result, 7);
        if (style.HasFlag(AnsiTextStyle.CrossedOut))
            Add(result, 9);
        if (style.HasFlag(AnsiTextStyle.DoublyUnderlined))
            Add(result, 21);
        if (style.HasFlag(AnsiTextStyle.Overlined))
            Add(result, 53);
        return result.ToString();
    }

    /// <summary>
    /// Gets the ANSI escape sequence for removing a style attribute using the "Select Graphic Rendition" (SGR) sequence.
    /// </summary>
    /// <param name="style">The style attributes to remove.</param>
    /// <returns><c>ESC [ <i>#</i> m</c></returns>
    public static string GetRemoveStyle(AnsiTextStyle style)
    {
        _ = Guard.NotUndefinedFlagInEnumValue(style, nameof(style));

        static void Add(StringBuilder sb, int n) => sb.Append($"{ESC}[{n}m");

        var result = new StringBuilder();
        if (style.HasFlag(AnsiTextStyle.Bold) || style.HasFlag(AnsiTextStyle.Faint))
            Add(result, 22);
        if (style.HasFlag(AnsiTextStyle.Italic))
            Add(result, 23);
        if (style.HasFlag(AnsiTextStyle.Underline) || style.HasFlag(AnsiTextStyle.DoublyUnderlined))
            Add(result, 24);
        if (style.HasFlag(AnsiTextStyle.Blink))
            Add(result, 25);
        if (style.HasFlag(AnsiTextStyle.Invert))
            Add(result, 7);
        if (style.HasFlag(AnsiTextStyle.CrossedOut))
            Add(result, 29);
        if (style.HasFlag(AnsiTextStyle.Overlined))
            Add(result, 55);
        return result.ToString();
    }

    /// <summary>
    /// Gets the ANSI escape sequence for setting the foreground color using the "Select Graphic Rendition" (SGR) sequence.
    /// </summary>
    /// <param name="colorCode">The color to set as foreground.</param>
    /// <returns><c>ESC [ 38 ; 5 ; <i><paramref name="colorCode"/></i> m</c></returns>
    public static string GetSetForegroundColor(AnsiColorCode colorCode)
    {
        _ = Guard.NotUndefinedEnumMember(colorCode, nameof(colorCode));
        return $"{ESC}[38;5;{(int)colorCode}m";
    }

    /// <summary>
    /// Gets the ANSI escape sequence for setting the foreground color using the "Select Graphic Rendition" (SGR) sequence.
    /// </summary>
    /// <param name="red">The red color channel (0-255).</param>
    /// <param name="green">The green color channel (0-255).</param>
    /// <param name="blue">The blue color channel (0-255).</param>
    /// <returns><c>ESC [ 38 ; 2 ; <i><paramref name="red"/></i> ; <i><paramref name="green"/></i> ; <i><paramref name="blue"/></i> m</c></returns>
    public static string GetSetForegroundColor(byte red, byte green, byte blue)
    {
        return $"{ESC}[38;2;{red};{green};{blue}m";
    }

    /// <summary>
    /// Gets the ANSI escape sequence for resetting the foreground color using the "Select Graphic Rendition" (SGR) sequence.
    /// </summary>
    /// <returns><c>ESC [ 39 m</c></returns>
    public static string GetResetForegroundColor()
    {
        return $"{ESC}[39m";
    }

    /// <summary>
    /// Gets the ANSI escape sequence for setting the background color using the "Select Graphic Rendition" (SGR) sequence.
    /// </summary>
    /// <param name="colorCode">The color to set as background.</param>
    /// <returns><c>ESC [ 48 ; 5 ; <i><paramref name="colorCode"/></i> m</c></returns>
    public static string GetSetBackgroundColor(AnsiColorCode colorCode)
    {
        _ = Guard.NotUndefinedEnumMember(colorCode, nameof(colorCode));
        return $"{ESC}[48;5;{(int)colorCode}m";
    }

    /// <summary>
    /// Gets the ANSI escape sequence for setting the background color using the "Select Graphic Rendition" (SGR) sequence.
    /// </summary>
    /// <param name="red">The red color channel (0-255).</param>
    /// <param name="green">The green color channel (0-255).</param>
    /// <param name="blue">The blue color channel (0-255).</param>
    /// <returns><c>ESC [ 48 ; 2 ; <i><paramref name="red"/></i> ; <i><paramref name="green"/></i> ; <i><paramref name="blue"/></i> m</c></returns>
    public static string GetSetBackgroundColor(byte red, byte green, byte blue)
    {
        return $"{ESC}[48;2;{red};{green};{blue}m";
    }

    /// <summary>
    /// Gets the ANSI escape sequence for resetting the background color using the "Select Graphic Rendition" (SGR) sequence.
    /// </summary>
    /// <returns><c>ESC [ 49 m</c></returns>
    public static string GetResetBackgroundColor()
    {
        return $"{ESC}[49m";
    }
}
