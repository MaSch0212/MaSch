using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace MaSch.Core.Lazy
{
    /// <summary>
    /// Represents a point that is lazily loaded.
    /// </summary>
    /// <seealso cref="AdvancedLazy{T1, T2}" />
    public class LazyPoint : AdvancedLazy<int, int>
    {
        private bool _bPoint;
        private Point _iPoint;

        /// <summary>
        /// Gets the x position.
        /// </summary>
        public virtual int X => Item1;

        /// <summary>
        /// Gets the y position.
        /// </summary>
        public virtual int Y => Item2;

        /// <summary>
        /// Gets the point as <see cref="System.Drawing.Point"/>.
        /// </summary>
        public virtual Point Point => GetValue(ref _bPoint, ref _iPoint, () => new Point(Item1, Item2));

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyPoint"/> class.
        /// </summary>
        /// <param name="xFactory">The factory for the x position.</param>
        /// <param name="yFactory">The factory for the y position.</param>
        public LazyPoint(Func<int> xFactory, Func<int> yFactory)
            : base(xFactory, yFactory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyPoint"/> class.
        /// </summary>
        /// <param name="xFactory">The factory for the x position.</param>
        /// <param name="yFactory">The factory for the y position.</param>
        /// <param name="useCaching">if set to <c>true</c> the first value returned by the factory function is cached.</param>
        public LazyPoint(Func<int> xFactory, Func<int> yFactory, bool useCaching)
            : base(xFactory, yFactory, useCaching)
        {
        }

        /// <inheritdoc />
        public override void ClearCache()
        {
            base.ClearCache();
            _bPoint = false;
            _iPoint = default;
        }
    }

    /// <summary>
    /// Represents a modifiable point that is lazily loaded.
    /// </summary>
    /// <seealso cref="ModifiableAdvancedLazy{T1, T2}" />
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Can be in same file.")]
    public class ModifiableLazyPoint : ModifiableAdvancedLazy<int, int>
    {
        private bool _bPoint;
        private Point _iPoint;

        /// <summary>
        /// Gets or sets the x position.
        /// </summary>
        public virtual int X
        {
            get => Item1;
            set => Item1 = value;
        }

        /// <summary>
        /// Gets or sets the y position.
        /// </summary>
        public virtual int Y
        {
            get => Item2;
            set => Item2 = value;
        }

        /// <summary>
        /// Gets or sets the point as <see cref="System.Drawing.Point"/>.
        /// </summary>
        public virtual Point Point
        {
            get => GetValue(ref _bPoint, ref _iPoint, () => new Point(Item1, Item2));
            set => SetValue(ref _bPoint, ref _iPoint, value, p => (Item1, Item2) = (p.X, p.Y));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifiableLazyPoint"/> class.
        /// </summary>
        /// <param name="xFactory">The factory for the x position.</param>
        /// <param name="xCallback">The callback action that is called when the x position changed.</param>
        /// <param name="yFactory">The factory for the y position.</param>
        /// <param name="yCallback">The callback action that is called when the y position changed.</param>
        public ModifiableLazyPoint(Func<int> xFactory, Action<int> xCallback, Func<int> yFactory, Action<int> yCallback)
            : base(xFactory, xCallback, yFactory, yCallback)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifiableLazyPoint"/> class.
        /// </summary>
        /// <param name="xFactory">The factory for the x position.</param>
        /// <param name="xCallback">The callback action that is called when the x position changed.</param>
        /// <param name="yFactory">The factory for the y position.</param>
        /// <param name="yCallback">The callback action that is called when the y position changed.</param>
        /// <param name="useCaching">if set to <c>true</c> the first value returned by the factory function is cached.</param>
        public ModifiableLazyPoint(Func<int> xFactory, Action<int> xCallback, Func<int> yFactory, Action<int> yCallback, bool useCaching)
            : base(xFactory, xCallback, yFactory, yCallback, useCaching)
        {
        }

        /// <inheritdoc />
        public override void ClearCache()
        {
            base.ClearCache();
            _bPoint = false;
            _iPoint = default;
        }
    }
}
