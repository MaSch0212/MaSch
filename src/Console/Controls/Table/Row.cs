using System.Collections.Generic;

namespace MaSch.Console.Controls.Table
{
    /// <summary>
    /// Represents a row in a <see cref="TableControl"/>.
    /// </summary>
    public class Row
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is a seperator instead of a data row.
        /// </summary>
        public bool IsSeperator { get; set; } = false;

        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        public IList<string> Values { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public int? Height { get; set; } = null;

        /// <summary>
        /// Gets or sets the height mode.
        /// </summary>
        public RowHeightMode HeightMode { get; set; } = RowHeightMode.Auto;
    }

    /// <summary>
    /// Height modes that can be used for <see cref="Row"/>s.
    /// </summary>
    public enum RowHeightMode
    {
        /// <summary>
        /// The height is automatically determined by the content of the <see cref="Row"/>.
        /// </summary>
        Auto,

        /// <summary>
        /// The height is determined by the fixed <see cref="Row.Height"/> of the <see cref="Row"/>.
        /// </summary>
        Fixed,
    }
}
