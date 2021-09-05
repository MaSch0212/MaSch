using MaSch.Core;
using MaSch.Core.Observable.Modules;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace MaSch.Presentation.Wpf.Observable
{
    /// <summary>
    /// Represents an observable dependency object that does change tracking.
    /// </summary>
    /// <seealso cref="ObservableDependencyObject" />
    /// <seealso cref="IChangeTrackedObject" />
    public class ObservableChangeTrackingDependencyObject : ObservableDependencyObject, IChangeTrackedObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableChangeTrackingDependencyObject"/> class.
        /// </summary>
        public ObservableChangeTrackingDependencyObject()
        {
            ChangeTracker = new ChangeTracker(GetType(), ImplicitlyRecurse);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableChangeTrackingDependencyObject"/> class.
        /// </summary>
        /// <param name="changeTracker">The change tracker.</param>
        protected ObservableChangeTrackingDependencyObject(IChangeTracker changeTracker)
        {
            ChangeTracker = Guard.NotNull(changeTracker, nameof(changeTracker));
        }

        /// <inheritdoc />
        [XmlIgnore]
        public virtual IChangeTracker ChangeTracker { get; private set; }

        /// <inheritdoc />
        [XmlIgnore]
        public bool HasChanges => ChangeTracker.HasChanges;

        /// <inheritdoc />
        [XmlIgnore]
        public virtual bool ImplicitlyRecurse => true;

        /// <summary>
        /// Tracks a change.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="notifyChange">if set to <c>true</c> the change is notified.</param>
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
        /// Gets a value indicating wether the <see cref="ChangeTracker"/> property should be serialized.
        /// </summary>
        /// <returns><c>true</c> if the <see cref="HasChanges"/> property should be serialized; otherwise, <c>false</c>.</returns>
        public virtual bool ShouldSerializeHasChanges()
        {
            return false;
        }

        /// <summary>
        /// Gets a value indicating wether the <see cref="ChangeTracker"/> property should be serialized.
        /// </summary>
        /// <returns><c>true</c> if the <see cref="ImplicitlyRecurse"/> property should be serialized; otherwise, <c>false</c>.</returns>
        public virtual bool ShouldSerializeImplicitlyRecurse()
        {
            return false;
        }
    }
}
