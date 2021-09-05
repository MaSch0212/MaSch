namespace MaSch.Console.Controls
{
    /// <summary>
    /// Text wrapping mode that is used by the <see cref="TextBlockControl"/>.
    /// </summary>
    public enum TextWrap
    {
        /// <summary>
        /// The text is not wrapped across multiple lines.
        /// </summary>
        NoWrap,

        /// <summary>
        /// The text is wrapped across multiple lines. The wrapping can happen in the middle of words.
        /// </summary>
        CharacterWrap,

        /// <summary>
        /// The text is wrapped across multiple lines. The wrapping will not happen in the middle of words.
        /// </summary>
        WordWrap,
    }
}
