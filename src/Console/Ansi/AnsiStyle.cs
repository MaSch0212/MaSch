namespace MaSch.Console.Ansi;

/// <summary>
/// Represents a style that can be produced by ANSI control sequences.
/// </summary>
public readonly struct AnsiStyle
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AnsiStyle"/> struct.
    /// </summary>
    /// <param name="addedStyles">The styles that should be used for this style.</param>
    /// <param name="removedStyles">The styles that should not be used for this style.</param>
    /// <param name="foreground">The foreground color to use for this style.</param>
    /// <param name="background">The background color to use for this style.</param>
    public AnsiStyle(AnsiTextStyle addedStyles, AnsiTextStyle removedStyles, AnsiColor foreground, AnsiColor background)
    {
        AddedStyles = addedStyles;
        RemovedStyles = removedStyles;
        ForegroundColor = foreground;
        BackgroundColor = background;
    }

    /// <summary>
    /// Gets the styles that should be used for this style.
    /// </summary>
    public AnsiTextStyle AddedStyles { get; }

    /// <summary>
    /// Gets the styles that should not be used for this style.
    /// </summary>
    public AnsiTextStyle RemovedStyles { get; }

    /// <summary>
    /// Gets the foreground color to use for this style.
    /// </summary>
    public AnsiColor ForegroundColor { get; }

    /// <summary>
    /// Gets the background color to use for this style.
    /// </summary>
    public AnsiColor BackgroundColor { get; }

    /// <summary>
    /// Builds the ANSI control sequence for this <see cref="AnsiStyle"/>.
    /// </summary>
    /// <returns>An ANSI control sequence that will set the text style to the values specified by this <see cref="AnsiStyle"/>.</returns>
    public string BuildAnsiSequence()
    {
        var result = new StringBuilder();
        if (RemovedStyles != AnsiTextStyle.None)
            _ = result.Append(AnsiEscapeUtility.GetRemoveStyle(RemovedStyles));
        if (AddedStyles != AnsiTextStyle.None)
            _ = result.Append(AnsiEscapeUtility.GetAddStyle(AddedStyles));
        _ = result.Append(ForegroundColor.ForegroundSequence);
        _ = result.Append(BackgroundColor.BackgroundSequence);
        return result.ToString();
    }
}
