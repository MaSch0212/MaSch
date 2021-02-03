namespace MaSch.Presentation.Wpf
{
    /// <summary>
    /// Specifies the type of change of a theme value.
    /// </summary>
    public enum ThemeValueChangeType
    {
        /// <summary>
        /// A theme value has been added.
        /// </summary>
        Add,

        /// <summary>
        /// A theme value has been removed.
        /// </summary>
        Remove,

        /// <summary>
        /// The value of a theme value has been changed.
        /// </summary>
        Change,

        /// <summary>
        /// A theme has been cleared.
        /// </summary>
        Clear,
    }

    /// <summary>
    /// Specifies default themes.
    /// </summary>
    public enum DefaultTheme
    {
        /// <summary>
        /// The default light theme.
        /// </summary>
        Light,

        /// <summary>
        /// The default dark theme.
        /// </summary>
        Dark,
    }
}
