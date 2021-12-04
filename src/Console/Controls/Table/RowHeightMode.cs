namespace MaSch.Console.Controls.Table;

/// <summary>
/// Height modes that can be used for <see cref="Row"/>s.
/// </summary>
public enum RowHeightMode
{
    /// <summary>
    /// The height is automatically determined by the content of the <see cref="Row"/>.
    /// </summary>
    Auto,

    /// <summary>
    /// The height is determined by the fixed <see cref="Row.Height"/> of the <see cref="Row"/>.
    /// </summary>
    Fixed,
}
