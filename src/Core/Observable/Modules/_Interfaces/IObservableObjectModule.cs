using System.Runtime.CompilerServices;

namespace MaSch.Common.Observable.Modules
{
    /// <summary>
    /// Describes a module for an <see cref="IObservableObject"/>.
    /// </summary>
    public interface IObservableObjectModule
    {
        /// <summary>
        /// Called when a value has been set.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="propertyName">Name of the property.</param>
        void OnSetValue(object? value, [CallerMemberName] string propertyName = "");
    }
}
