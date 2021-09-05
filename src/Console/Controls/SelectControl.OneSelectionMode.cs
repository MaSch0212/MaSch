#pragma warning disable SA1601 // Partial elements should be documented

namespace MaSch.Console.Controls
{
    public partial class SelectControl
    {
        /// <summary>
        /// The selection mode for the <see cref="SelectControl"/>.
        /// </summary>
        public enum OneSelectionMode
        {
            /// <summary>
            /// Displays one item at a time. The user can switch between the items using the up and down arrow keys.
            /// </summary>
            UpDown,

            /// <summary>
            /// Displays the items next to each other. The user can switch between the items using the left and right arrow keys.
            /// </summary>
            LeftRight,

            /// <summary>
            /// Displays a maximum of three items at a time. The user can switch between the items using the up and down arrow keys.
            /// </summary>
            UpDown3,
        }
    }
}
