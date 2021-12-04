using System.ComponentModel;

namespace MaSch.Core.Observable.Collections;

/// <summary>
/// The event handler delegate for the <see cref="FullyObservableCollection{T}.CollectionItemPropertyChanged"/> event.
/// </summary>
/// <param name="sender">The sender of the event.</param>
/// <param name="e">The <see cref="CollectionItemPropertyChangedEventArgs"/> instance containing the event data.</param>
public delegate void CollectionItemPropertyChangedEventHandler(object? sender, CollectionItemPropertyChangedEventArgs e);

/// <summary>
/// The event data for the <see cref="CollectionChangeEventHandler"/> event handler.
/// </summary>
/// <seealso cref="EventArgs" />
public class CollectionItemPropertyChangedEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CollectionItemPropertyChangedEventArgs"/> class.
    /// </summary>
    /// <param name="itemIndex">Index of the item that changed.</param>
    /// <param name="item">The item that changed.</param>
    /// <param name="propertyName">Name of the property that changed.</param>
    public CollectionItemPropertyChangedEventArgs(int itemIndex, object item, string? propertyName)
    {
        ItemIndex = itemIndex;
        Item = item;
        PropertyName = propertyName;
    }

    /// <summary>
    /// Gets the index of the item that changed.
    /// </summary>
    /// <value>
    /// The index of the item that changed.
    /// </value>
    public int ItemIndex { get; }

    /// <summary>
    /// Gets the item that changes.
    /// </summary>
    /// <value>
    /// The item that changed.
    /// </value>
    public object Item { get; }

    /// <summary>
    /// Gets the name of the property that changed.
    /// </summary>
    /// <value>
    /// The name of the property that changed.
    /// </value>
    public string? PropertyName { get; }
}
