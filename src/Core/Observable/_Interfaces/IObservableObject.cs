using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MaSch.Common.Observable
{
    /// <summary>
    /// Describes an object on which property changes can be observed.
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    public interface IObservableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets a value indicating whether the subscribers should be notified on property change.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is notifying subscribers about property changes; otherwise, <c>false</c>.
        /// </value>
        bool IsNotifyEnabled { get; set; }

        /// <summary>
        /// Sets the specified property and notifies subscribers about the change.
        /// </summary>
        /// <typeparam name="T">The type of the property to set.</typeparam>
        /// <param name="property">The property backing field.</param>
        /// <param name="value">The value to set.</param>
        /// <param name="propertyName">Name of the property.</param>
        void SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = "");

        /// <summary>
        /// Notifies subscribers that a specific property has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="notifyDependencies">If set to <c>true</c> also properties that are dependent on <paramref name="propertyName"/> are notified.</param>
        void NotifyPropertyChanged([CallerMemberName] string propertyName = "", bool notifyDependencies = true);

        /// <summary>
        /// Notifies that a command has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        void NotifyCommandChanged([CallerMemberName] string propertyName = "");
    }
}
