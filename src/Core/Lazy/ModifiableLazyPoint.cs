using System.Drawing;

namespace MaSch.Core.Lazy;

/// <summary>
/// Represents a modifiable point that is lazily loaded.
/// </summary>
/// <seealso cref="ModifiableAdvancedLazy{T1, T2}" />
public class ModifiableLazyPoint : ModifiableAdvancedLazy<int, int>
{
    private bool _hasPoint;
    private Point _point;

    /// <summary>
    /// Initializes a new instance of the <see cref="ModifiableLazyPoint"/> class.
    /// </summary>
    /// <param name="xFactory">The factory for the x position.</param>
    /// <param name="xCallback">The callback action that is called when the x position changed.</param>
    /// <param name="yFactory">The factory for the y position.</param>
    /// <param name="yCallback">The callback action that is called when the y position changed.</param>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:Field names should not use Hungarian notation", Justification = "False positive.")]
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
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:Field names should not use Hungarian notation", Justification = "False positive.")]
    public ModifiableLazyPoint(Func<int> xFactory, Action<int> xCallback, Func<int> yFactory, Action<int> yCallback, bool useCaching)
        : base(xFactory, xCallback, yFactory, yCallback, useCaching)
    {
    }

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
        get => GetValue(ref _hasPoint, ref _point, () => new Point(Item1, Item2));
        set => SetValue(ref _hasPoint, ref _point, value, p => (Item1, Item2) = (p.X, p.Y));
    }

    /// <inheritdoc />
    public override void ClearCache()
    {
        base.ClearCache();
        _hasPoint = false;
        _point = default;
    }
}
