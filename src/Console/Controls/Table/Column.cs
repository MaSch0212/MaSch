using System.Collections.Generic;
using System.Linq;

namespace MaSch.Console.Controls.Table
{
    /// <summary>
    /// Represents a coplumn of a <see cref="TableControl"/>.
    /// </summary>
    public class Column
    {
        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        public string? Header { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public double Width { get; set; } = double.NaN;

        /// <summary>
        /// Gets or sets the width mode.
        /// </summary>
        public ColumnWidthMode WidthMode { get; set; } = ColumnWidthMode.Auto;

        /// <summary>
        /// Gets or sets the minimum width.
        /// </summary>
        public int? MinWidth { get; set; } = null;

        /// <summary>
        /// Gets or sets the maximum width.
        /// </summary>
        public int? MaxWidth { get; set; } = null;

        /// <summary>
        /// Gets or sets a list of characters that should prevent to beeing wrapped.
        /// </summary>
        public IList<char> NonWrappingChars { get; set; } = TextBlockControl.DefaultNonWrappingChars.ToList();
    }
}
