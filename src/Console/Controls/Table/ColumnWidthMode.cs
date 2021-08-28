namespace MaSch.Console.Controls.Table
{
    /// <summary>
    /// Width modes that can be used for <see cref="Column"/>s.
    /// </summary>
    public enum ColumnWidthMode
    {
        /// <summary>
        /// The width is automatically determined by the content of the <see cref="Column"/>.
        /// </summary>
        Auto,

        /// <summary>
        /// The width is determined by the fixed <see cref="Column.Width"/> of the <see cref="Column"/>.
        /// </summary>
        Fixed,

        /// <summary>
        /// The width is determined by an amount of the available with of the <see cref="TableControl"/>.
        /// </summary>
        Star,
    }
}
