using System.Drawing;

namespace MaSch.Core.Lazy;

/// <summary>
/// Represents a point that is lazily loaded.
/// </summary>
/// <seealso cref="AdvancedLazy{T1, T2}" />
public class LazyPoint : AdvancedLazy<int, int>
{
    private bool _hasPoint;
    private Point _point;

    /// <summary>
    /// Initializes a new instance of the <see cref="LazyPoint"/> class.
    /// </summary>
    /// <param name="xFactory">The factory for the x position.</param>
    /// <param name="yFactory">The factory for the y position.</param>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:Field names should not use Hungarian notation", Justification = "False positive.")]
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
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:Field names should not use Hungarian notation", Justification = "False positive.")]
    public LazyPoint(Func<int> xFactory, Func<int> yFactory, bool useCaching)
        : base(xFactory, yFactory, useCaching)
    {
    }

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
    public virtual Point Point => GetValue(ref _hasPoint, ref _point, () => new Point(Item1, Item2));

    /// <inheritdoc />
    public override void ClearCache()
    {
        base.ClearCache();
        _hasPoint = false;
        _point = default;
    }
}
