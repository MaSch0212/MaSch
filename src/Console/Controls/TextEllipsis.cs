namespace MaSch.Console.Controls
{
    /// <summary>
    /// Text ellipsis mode that is used by the <see cref="TextBlockControl"/>.
    /// </summary>
    public enum TextEllipsis
    {
        /// <summary>
        /// The text is cut at the end without any ellipsis (e.g.: "Lorem ipsum dolo").
        /// </summary>
        None,

        /// <summary>
        /// The text is cut at the end. The last three visible characters are replaced by "..." (e.g.: "Lorem ipsum d...").
        /// </summary>
        EndCharacter,

        /// <summary>
        /// The text is cut at the start. The first three visible characters are replaced by "..." (e.g.: "...olor sit amet").
        /// </summary>
        StartCharacter,

        /// <summary>
        /// The text is cut at the center. The most center visible characters are replaced by "..." (e.g.: "Lorem ip...sit amet").
        /// </summary>
        CenterCharacter,

        /// <summary>
        /// The text is cut at the end of the last word that still fits. "..." is appended at the end of the text (e.g.: "Lorem ipsum...").
        /// </summary>
        EndWord,

        /// <summary>
        /// The text is cut at the start of the first word that still fits. "..." is prepended at the start of the text (e.g.: "...sit amet").
        /// </summary>
        StartWord,

        /// <summary>
        /// The text is cut at the center without cutting though words. "..." is added to the center of the text (e.g.: "Lorem...sit amet").
        /// </summary>
        CenterWord,
    }
}
