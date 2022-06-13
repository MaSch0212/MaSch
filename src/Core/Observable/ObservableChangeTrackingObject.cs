using MaSch.Core.Observable.Modules;
using System.ComponentModel;
using System.Xml.Serialization;

namespace MaSch.Core.Observable;

/// <summary>
/// Represents an observable class that supports change tracking.
/// </summary>
/// <seealso cref="ObservableObject" />
/// <seealso cref="IChangeTrackedObject" />
public abstract class ObservableChangeTrackingObject : ObservableObject, IChangeTrackedObject
{
    #region Ctor

    /// <summary>
    /// Initializes a new instance of the <see cref="ObservableChangeTrackingObject"/> class with the default <see cref="IChangeTracker"/>.
    /// </summary>
    protected ObservableChangeTrackingObject()
    {
        ChangeTracker = new ChangeTracker(GetType(), ImplicitlyRecurse);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ObservableChangeTrackingObject"/> class.
    /// </summary>
    /// <param name="changeTracker">The change tracker to use.</param>
    /// <exception cref="ArgumentNullException"><paramref name="changeTracker"/>.</exception>
    protected ObservableChangeTrackingObject(IChangeTracker changeTracker)
    {
        ChangeTracker = Guard.NotNull(changeTracker);
    }

    #endregion

    #region Properties

    /// <inheritdoc/>
    [XmlIgnore]
    public virtual IChangeTracker ChangeTracker { get; }

    /// <inheritdoc/>
    [XmlIgnore]
    public virtual bool HasChanges => ChangeTracker.HasChanges;

    /// <inheritdoc/>
    [XmlIgnore]
    public virtual bool ImplicitlyRecurse => true;

    #endregion

    #region Public Methods

    /// <summary>
    /// Tracks the change of a property.
    /// </summary>
    /// <typeparam name="T">The type of the property value.</typeparam>
    /// <param name="value">The value of the property.</param>
    /// <param name="notifyChange">If set to <c>true</c> the <see cref="INotifyPropertyChanged.PropertyChanged"/> event is raised.</param>
    /// <param name="propertyName">Name of the property.</param>
    public void TrackChange<T>(T value, bool notifyChange = true, [CallerMemberName] string propertyName = "")
    {
        ChangeTracker.OnSetValue(value, propertyName);
        if (notifyChange)
            NotifyPropertyChanged(propertyName);
    }

    /// <inheritdoc/>
    public override void SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = "")
    {
        ChangeTracker.OnSetValue(value, propertyName);
        base.SetProperty(ref property, value, propertyName);
    }

    /// <inheritdoc/>
    public virtual void ResetChangeTracking()
    {
        foreach (var property in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var value = property.GetValue(this);
            ChangeTracker.SetBaseValue(value, property.Name);
        }

        ChangeTracker.ResetChangeTracking();
    }

    #endregion

    #region Serialization

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

    #endregion
}
