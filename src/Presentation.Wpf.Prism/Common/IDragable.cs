namespace MaSch.Presentation.Wpf.Common;

/// <summary>
/// Represents a dragable object.
/// </summary>
public interface IDragable
{
    /// <summary>
    /// Gets the type of the data item.
    /// </summary>
    Type DataType { get; }

    /// <summary>
    /// Remove the object from the collection.
    /// </summary>
    /// <param name="i">The data to be removed.</param>
    void Remove(object i);
}
