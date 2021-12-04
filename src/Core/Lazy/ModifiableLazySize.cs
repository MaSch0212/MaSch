using System.Drawing;

namespace MaSch.Core.Lazy;

/// <summary>
/// Represents a modifiable size that is lazily loaded.
/// </summary>
/// <seealso cref="ModifiableAdvancedLazy{T1, T2}" />
public class ModifiableLazySize : ModifiableAdvancedLazy<int, int>
{
    private bool _hasSize;
    private Size _size;

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
        get => GetValue(ref _hasSize, ref _size, () => new Size(Item1, Item2));
        set => SetValue(ref _hasSize, ref _size, value, p => (Item1, Item2) = (p.Width, p.Height));
    }

    /// <inheritdoc />
    public override void ClearCache()
    {
        base.ClearCache();
        _hasSize = false;
        _size = default;
    }
}
