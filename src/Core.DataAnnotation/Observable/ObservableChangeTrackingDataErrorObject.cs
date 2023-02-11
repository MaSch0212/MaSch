using MaSch.Core.Observable.Modules;
using System.ComponentModel;
using System.Xml.Serialization;

namespace MaSch.Core.Observable;

/// <summary>
/// Represents an observable class that track changes and also handles errors.
/// </summary>
/// <seealso cref="ObservableDataErrorObject" />
/// <seealso cref="IChangeTrackedObject" />
[RequiresUnreferencedCode("Uses DataErrorHandler.")]
public class ObservableChangeTrackingDataErrorObject : ObservableDataErrorObject, IChangeTrackedObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ObservableChangeTrackingDataErrorObject"/> class.
    /// </summary>
    public ObservableChangeTrackingDataErrorObject()
    {
        ChangeTracker = new ChangeTracker(GetType(), ImplicitlyRecurse);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ObservableChangeTrackingDataErrorObject"/> class.
    /// </summary>
    /// <param name="changeTracker">The change tracker.</param>
    /// <exception cref="ArgumentNullException">changeTracker is null.</exception>
    protected ObservableChangeTrackingDataErrorObject(IChangeTracker changeTracker)
    {
        ChangeTracker = changeTracker ?? throw new ArgumentNullException(nameof(changeTracker));
    }

    /// <inheritdoc />
    [XmlIgnore]
    public virtual IChangeTracker ChangeTracker { get; }

    /// <inheritdoc />
    [XmlIgnore]
    public bool HasChanges => ChangeTracker.HasChanges;

    /// <inheritdoc />
    [XmlIgnore]
    public virtual bool ImplicitlyRecurse => true;

    /// <summary>
    /// Tracks a change.
    /// </summary>
    /// <typeparam name="T">The type of the property.</typeparam>
    /// <param name="value">The value to track.</param>
    /// <param name="notifyChange">if set to <c>true</c> raise the <see cref="INotifyPropertyChanged.PropertyChanged"/> event.</param>
    /// <param name="propertyName">Name of the property.</param>
    public void TrackChange<T>(T value, bool notifyChange = true, [CallerMemberName] string propertyName = "")
    {
        ChangeTracker.OnSetValue(value, propertyName);
        if (notifyChange)
            NotifyPropertyChanged(propertyName);
    }

    /// <inheritdoc />
    public override void SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = "")
    {
        ChangeTracker.OnSetValue(value, propertyName);
        base.SetProperty(ref property, value, propertyName);
    }

    /// <inheritdoc />
    public virtual void ResetChangeTracking()
    {
        foreach (var property in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var value = property.GetValue(this);
            ChangeTracker.SetBaseValue(value, property.Name);
        }

        ChangeTracker.ResetChangeTracking();
    }

    /// <summary>
    /// Gets a value indicating wether the <see cref="ChangeTracker"/> property should be serialized.
    /// </summary>
    /// <returns><c>true</c> if the <see cref="ChangeTracker"/> property should be serialized; otherwise, <c>false</c>.</returns>
    public virtual bool ShouldSerializeChangeTracker()
    {
        return false;
    }

    /// <summary>
    /// Gets a value indicating wether the <see cref="HasChanges"/> property should be serialized.
    /// </summary>
    /// <returns><c>true</c> if the <see cref="HasChanges"/> property should be serialized; otherwise, <c>false</c>.</returns>
    public virtual bool ShouldSerializeHasChanges()
    {
        return false;
    }

    /// <summary>
    /// Gets a value indicating wether the <see cref="ImplicitlyRecurse"/> property should be serialized.
    /// </summary>
    /// <returns><c>true</c> if the <see cref="ImplicitlyRecurse"/> property should be serialized; otherwise, <c>false</c>.</returns>
    public virtual bool ShouldSerializeImplicitlyRecurse()
    {
        return false;
    }
}
