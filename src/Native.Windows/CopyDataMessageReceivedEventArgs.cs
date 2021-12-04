namespace MaSch.Native.Windows;

/// <summary>
/// Represents a method for handling the <see cref="PostHelper{T}.MessageReceived"/> event.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="sender">The sender.</param>
/// <param name="e">The <see cref="CopyDataMessageReceivedEventArgs{T}"/> instance containing the event data.</param>
public delegate void CopyDataMessageReceivedEventHandler<T>(object sender, CopyDataMessageReceivedEventArgs<T> e);

/// <summary>
/// Represents the event data for the <see cref="PostHelper{T}.MessageReceived"/> event.
/// </summary>
/// <typeparam name="T"></typeparam>
public class CopyDataMessageReceivedEventArgs<T>
{
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="CopyDataMessageReceivedEventArgs{T}"/> is handled.
    /// </summary>
    /// <value>
    ///   <c>true</c> if handled; otherwise, <c>false</c>.
    /// </value>
    public bool Handled { get; set; }

    /// <summary>
    /// Gets or sets the w parameter.
    /// </summary>
    /// <value>
    /// The w parameter.
    /// </value>
    public IntPtr WParam { get; set; }

    /// <summary>
    /// Gets or sets the data.
    /// </summary>
    /// <value>
    /// The data.
    /// </value>
    public T? Data { get; set; }
}
