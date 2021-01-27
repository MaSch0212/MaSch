using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace MaSch.Core.Lazy
{
    /// <summary>
    /// Represents a size that is lazily loaded.
    /// </summary>
    /// <seealso cref="AdvancedLazy{T1, T2}" />
    public class LazySize : AdvancedLazy<int, int>
    {
        private bool _bSize;
        private Size _iSize;

        /// <summary>
        /// Gets the width.
        /// </summary>
        public virtual int Width => Item1;

        /// <summary>
        /// Gets the height.
        /// </summary>
        public virtual int Height => Item2;

        /// <summary>
        /// Gets the size as <see cref="System.Drawing.Size"/>.
        /// </summary>
        public virtual Size Size => GetValue(ref _bSize, ref _iSize, () => new Size(Item1, Item2));

        /// <summary>
        /// Initializes a new instance of the <see cref="LazySize"/> class.
        /// </summary>
        /// <param name="widthFactory">The factory for the width.</param>
        /// <param name="heightFactory">The factory for the height.</param>
        public LazySize(Func<int> widthFactory, Func<int> heightFactory)
            : base(widthFactory, heightFactory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LazySize"/> class.
        /// </summary>
        /// <param name="widthFactory">The factory for the width.</param>
        /// <param name="heightFactory">The factory for the height.</param>
        /// <param name="useCaching">if set to <c>true</c> the first value returned by the factory function is cached.</param>
        public LazySize(Func<int> widthFactory, Func<int> heightFactory, bool useCaching)
            : base(widthFactory, heightFactory, useCaching)
        {
        }

        /// <inheritdoc />
        public override void ClearCache()
        {
            base.ClearCache();
            _bSize = false;
            _iSize = default;
        }
    }

    /// <summary>
    /// Represents a modifiable size that is lazily loaded.
    /// </summary>
    /// <seealso cref="ModifiableAdvancedLazy{T1, T2}" />
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Can be in same file.")]
    public class ModifiableLazySize : ModifiableAdvancedLazy<int, int>
    {
        private bool _bSize;
        private Size _iSize;

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public virtual int Width
        {
            get => Item1;
            set => Item1 = value;
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public virtual int Height
        {
            get => Item2;
            set => Item2 = value;
        }

        /// <summary>
        /// Gets or sets the size as <see cref="System.Drawing.Size"/>.
        /// </summary>
        public virtual Size Size
        {
            get => GetValue(ref _bSize, ref _iSize, () => new Size(Item1, Item2));
            set => SetValue(ref _bSize, ref _iSize, value, p => (Item1, Item2) = (p.Width, p.Height));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifiableLazySize"/> class.
        /// </summary>
        /// <param name="widthFactory">The factory for the width.</param>
        /// <param name="widthCallback">The callback action that is called when the width changed.</param>
        /// <param name="heightFactory">The factory for the height.</param>
        /// <param name="heightCallback">The callback action that is called when the height changed.</param>
        public ModifiableLazySize(Func<int> widthFactory, Action<int> widthCallback, Func<int> heightFactory, Action<int> heightCallback)
            : base(widthFactory, widthCallback, heightFactory, heightCallback)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifiableLazySize"/> class.
        /// </summary>
        /// <param name="widthFactory">The factory for the width.</param>
        /// <param name="widthCallback">The callback action that is called when the width changed.</param>
        /// <param name="heightFactory">The factory for the height.</param>
        /// <param name="heightCallback">The callback action that is called when the height changed.</param>
        /// <param name="useCaching">if set to <c>true</c> the first value returned by the factory function is cached.</param>
        public ModifiableLazySize(Func<int> widthFactory, Action<int> widthCallback, Func<int> heightFactory, Action<int> heightCallback, bool useCaching)
            : base(widthFactory, widthCallback, heightFactory, heightCallback, useCaching)
        {
        }

        /// <inheritdoc />
        public override void ClearCache()
        {
            base.ClearCache();
            _bSize = false;
            _iSize = default;
        }
    }
}
