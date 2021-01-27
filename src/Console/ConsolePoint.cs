using MaSch.Core.Lazy;
using System;
using System.Drawing;
using System.Runtime.Versioning;

namespace MaSch.Console
{
    /// <summary>
    /// Stores a point in the context of the <see cref="System.Console"/>.
    /// </summary>
    /// <seealso cref="ModifiableLazyPoint" />
    public class ConsolePoint : ModifiableLazyPoint
    {
        /// <summary>
        /// Gets or sets the x position of this <see cref="ConsolePoint"/>.
        /// </summary>
        public override int X
        {
            get => base.X;
            [SupportedOSPlatform("windows")]
            set => base.X = value;
        }

        /// <summary>
        /// Gets or sets the y position of this <see cref="ConsolePoint"/>.
        /// </summary>
        public override int Y
        {
            get => base.Y;
            [SupportedOSPlatform("windows")]
            set => base.Y = value;
        }

        /// <summary>
        /// Gets or sets the current position as a <see cref="System.Drawing.Point"/>.
        /// </summary>
        public override Point Point
        {
            get => base.Point;
            [SupportedOSPlatform("windows")]
            set => base.Point = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsolePoint"/> class.
        /// </summary>
        /// <param name="xFactory">The function that is used to get the x position.</param>
        /// <param name="xCallback">The action that is executed when the x position is changed.</param>
        /// <param name="yFactory">The function that is used to get the y position.</param>
        /// <param name="yCallback">The action that is executed when the y position is changed.</param>
        public ConsolePoint(Func<int> xFactory, Action<int> xCallback, Func<int> yFactory, Action<int> yCallback)
            : base(xFactory, xCallback, yFactory, yCallback)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsolePoint"/> class.
        /// </summary>
        /// <param name="xFactory">The function that is used to get the x position.</param>
        /// <param name="xCallback">The action that is executed when the x position is changed.</param>
        /// <param name="yFactory">The function that is used to get the y position.</param>
        /// <param name="yCallback">The action that is executed when the y position is changed.</param>
        /// <param name="useCaching">if set to <c>true</c> the factory functions are only executed once for this instance of the <see cref="ConsolePoint"/>.</param>
        public ConsolePoint(Func<int> xFactory, Action<int> xCallback, Func<int> yFactory, Action<int> yCallback, bool useCaching)
            : base(xFactory, xCallback, yFactory, yCallback, useCaching)
        {
        }
    }
}
