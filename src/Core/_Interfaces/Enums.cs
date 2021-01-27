namespace MaSch.Core
{
    /// <summary>
    /// Specifies how a string should be checked for emptiness.
    /// </summary>
    public enum StringNullMode
    {
        /// <summary>
        /// The string is checked if it is null.
        /// </summary>
        IsNull,

        /// <summary>
        /// The string is checked if it is null or an empty string.
        /// </summary>
        IsNullOrEmpty,

        /// <summary>
        /// The string is checked it it is null, an empty string or only contains whitespace characters.
        /// </summary>
        IsNullOrWhitespace,
    }
}
