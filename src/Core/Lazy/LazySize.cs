using System.Drawing;

namespace MaSch.Core.Lazy;

/// <summary>
/// Represents a size that is lazily loaded.
/// </summary>
/// <seealso cref="AdvancedLazy{T1, T2}" />
public class LazySize : AdvancedLazy<int, int>
{
    private bool _hasSize;
    private Size _size;

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
    public virtual Size Size => GetValue(ref _hasSize, ref _size, () => new Size(Item1, Item2));

    /// <inheritdoc />
    public override void ClearCache()
    {
        base.ClearCache();
        _hasSize = false;
        _size = default;
    }
}
