using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MaSch.Core.Observable.Modules
{
    /// <summary>
    /// Describes a class that handles change tracking.
    /// </summary>
    /// <seealso cref="IObservableObjectModule" />
    public interface IChangeTracker : IObservableObjectModule
    {
        /// <summary>
        /// Occurs when the <see cref="HasChanges"/> property changed.
        /// </summary>
        event EventHandler? HasChangesChanged;

        /// <summary>
        /// Gets or sets the change tracking disabled properties.
        /// </summary>
        /// <value>
        /// The change tracking disabled properties.
        /// </value>
        ICollection<string> ChangeTrackingDisabledProperties { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has changes.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has changes; otherwise, <c>false</c>.
        /// </value>
        bool HasChanges { get; }

        /// <summary>
        /// Gets a value indicating whether change trackiug should work recursively implicitly.
        /// </summary>
        /// <value>
        ///   <c>true</c> if change trackiug should work recursively implicitly; otherwise, <c>false</c>.
        /// </value>
        bool ImplicitlyRecurse { get; }

        /// <summary>
        /// Gets or sets a function that is executed if it is determined wether this <see cref="IChangeTracker"/> has changes.
        /// </summary>
        /// <value>
        /// The function that is executed if it is determined wether this <see cref="IChangeTracker"/> has changes.
        /// </value>
        Func<bool, bool> HasChangesExtension { get; set; }


        /// <summary>
        /// Gets the changed properties.
        /// </summary>
        /// <param name="recursive">If set to <c>true</c> change tracking for <see cref="IChangeTrackedObject"/> properties is used recursively.</param>
        /// <returns>An array containig all properties that has changes.</returns>
        string[] GetChangedProperties(bool recursive);

        /// <summary>
        /// Determines whether the property with a specified name has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        ///   <c>true</c> if the property with the name <paramref name="propertyName"/> has changed; otherwise, <c>false</c>.
        /// </returns>
        bool HasPropertyChanged([CallerMemberName] string propertyName = "");

        /// <summary>
        /// Sets the base value that is used to determine if the property changed.
        /// </summary>
        /// <param name="value">The value to use as base value.</param>
        /// <param name="propertyName">Name of the property.</param>
        void SetBaseValue(object? value, [CallerMemberName] string propertyName = "");

        /// <summary>
        /// Evaluates if the <see cref="HasChanges"/> property changed and raises the <see cref="HasChangesChanged"/> event if so.
        /// </summary>
        void EvaluateHasChangesChanged();

        /// <summary>
        /// Resets the change tracking or the specified properties.
        /// </summary>
        /// <param name="propertyNames">The property names.</param>
        void ResetChangeTracking(params string[] propertyNames);

        /// <summary>
        /// Resets the change tracking.
        /// </summary>
        void ResetChangeTracking();


        /// <summary>
        /// Adds a fixed change, so that the <see cref="HasChanges"/> property always returns <c>true</c>.
        /// </summary>
        void AddFixedChange();

        /// <summary>
        /// Removes the fixed change, so that the <see cref="HasChanges"/> property only returns <c>true</c> if some property changed.
        /// </summary>
        void RemoveFixedChange();
    }

    /// <summary>
    /// Represents a class that uses change tracking.
    /// </summary>
    public interface IChangeTrackedObject
    {
        /// <summary>
        /// Gets the <see cref="IChangeTracker"/> that is used by this <see cref="IChangeTrackedObject"/>.
        /// </summary>
        /// <value>
        /// The <see cref="IChangeTracker"/> that is used by this <see cref="IChangeTrackedObject"/>.
        /// </value>
        IChangeTracker ChangeTracker { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has changes.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has changes; otherwise, <c>false</c>.
        /// </value>
        bool HasChanges { get; }

        /// <summary>
        /// Gets a value indicating whether change trackiug should work recursively implicitly.
        /// </summary>
        /// <value>
        ///   <c>true</c> if change trackiug should work recursively implicitly; otherwise, <c>false</c>.
        /// </value>
        bool ImplicitlyRecurse { get; }

        /// <summary>
        /// Resets the change tracking.
        /// </summary>
        void ResetChangeTracking();
    }
}
