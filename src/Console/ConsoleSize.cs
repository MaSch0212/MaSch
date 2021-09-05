using MaSch.Core.Lazy;
using System;
using System.Drawing;
using System.Runtime.Versioning;

namespace MaSch.Console
{
    /// <summary>
    /// Stores a size in the context of the <see cref="System.Console"/>.
    /// </summary>
    /// <seealso cref="ModifiableLazySize" />
    public class ConsoleSize : ModifiableLazySize
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleSize"/> class.
        /// </summary>
        /// <param name="widthFactory">The function that is used to get the width.</param>
        /// <param name="widthCallback">The action that is executed when the width is changed.</param>
        /// <param name="heightFactory">The function that is used to get the height.</param>
        /// <param name="heightCallback">The action that is executed when the height is changed.</param>
        public ConsoleSize(Func<int> widthFactory, Action<int> widthCallback, Func<int> heightFactory, Action<int> heightCallback)
            : base(widthFactory, widthCallback, heightFactory, heightCallback)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleSize"/> class.
        /// </summary>
        /// <param name="widthFactory">The function that is used to get the width.</param>
        /// <param name="widthCallback">The action that is executed when the width is changed.</param>
        /// <param name="heightFactory">The function that is used to get the height.</param>
        /// <param name="heightCallback">The action that is executed when the height is changed.</param>
        /// <param name="useCaching">if set to <c>true</c> the factory functions are only executed once for this instance of the <see cref="ConsoleSize"/>.</param>
        public ConsoleSize(Func<int> widthFactory, Action<int> widthCallback, Func<int> heightFactory, Action<int> heightCallback, bool useCaching)
            : base(widthFactory, widthCallback, heightFactory, heightCallback, useCaching)
        {
        }

        /// <summary>
        /// Gets or sets the height of this <see cref="ConsoleSize"/>.
        /// </summary>
        public override int Height
        {
            get => base.Height;
            [SupportedOSPlatform("windows")]
            set => base.Height = value;
        }

        /// <summary>
        /// Gets or sets the width of this <see cref="ConsoleSize"/>.
        /// </summary>
        public override int Width
        {
            get => base.Width;
            [SupportedOSPlatform("windows")]
            set => base.Width = value;
        }

        /// <summary>
        /// Gets or sets the current size as a <see cref="System.Drawing.Size"/>.
        /// </summary>
        public override Size Size
        {
            get => base.Size;
            [SupportedOSPlatform("windows")]
            set => base.Size = value;
        }
    }
}
