namespace MaSch.Console.Controls;

/// <summary>
/// Text alignment mode that is used by the <see cref="TextBlockControl"/>.
/// </summary>
public enum TextAlignment
{
    /// <summary>
    /// Aligns the text to the left.
    /// </summary>
    Left,

    /// <summary>
    /// Align the text to the center.
    /// </summary>
    Center,

    /// <summary>
    /// Align the text to the right.
    /// </summary>
    Right,

    /// <summary>
    /// Align the text to the left and right. Additional space is filled in spaces between words.
    /// </summary>
    Block,
}
