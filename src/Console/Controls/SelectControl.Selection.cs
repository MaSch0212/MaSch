#pragma warning disable SA1601 // Partial elements should be documented

namespace MaSch.Console.Controls;

public partial class SelectControl
{
    /// <summary>
    /// Represents a user selection that has been made using the <see cref="SelectControl"/>.
    /// </summary>
    public struct Selection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Selection"/> struct.
        /// </summary>
        /// <param name="index">The index that has been selected..</param>
        /// <param name="value">The value that has been selected..</param>
        public Selection(int index, string? value)
        {
            Index = index;
            Value = value;
        }

        /// <summary>
        /// Gets or sets the index that has been selected.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the value that has been selected.
        /// </summary>
        public string? Value { get; set; }
    }
}
