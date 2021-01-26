using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace MaSch.Core.Observable.Modules
{
    /// <summary>
    /// Represents an observable class that handles validation errors in its properties.
    /// </summary>
    /// <seealso cref="INotifyDataErrorInfo" />
    /// <seealso cref="IObservableObjectModule" />
    public interface IDataErrorHandler : IDataErrorObject, IObservableObjectModule
    {
        /// <summary>
        /// Determines whether a property exists that has a given name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        ///   <c>true</c> if the property exists that has the name <paramref name="propertyName"/>; otherwise, <c>false</c>.
        /// </returns>
        bool IsPropertyExistant(string? propertyName);
    }

    /// <summary>
    /// Represents a class that handles validation errors in its properties.
    /// </summary>
    /// <seealso cref="INotifyDataErrorInfo" />
    public interface IDataErrorObject : INotifyDataErrorInfo
    {
        /// <summary>
        /// Gets the validation errors for the entire entity.
        /// </summary>
        /// <returns>The validation errors for the property or entity.</returns>
        IDictionary<string, IEnumerable> GetErrors();

        /// <summary>
        /// Checks for errors for the netire entity.
        /// </summary>
        /// <returns><c>true</c> if an error exists; otherwise, <c>false</c>.</returns>
        bool CheckForErrors();

        /// <summary>
        /// Checks a property for errors.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns><c>true</c> if an error exists; otherwise, <c>false</c>.</returns>
        bool CheckForError(string? propertyName);
    }
}
