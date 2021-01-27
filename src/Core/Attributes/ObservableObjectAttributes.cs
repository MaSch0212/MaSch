using System;
using System.ComponentModel;

#pragma warning disable SA1649 // File name should match first type name
#pragma warning disable SA1402 // File may only contain a single type

namespace MaSch.Core.Attributes
{
    /// <summary>
    /// Marks the property dependent on other properties in the same class.
    /// If any of these properties are changed, the <see cref="INotifyPropertyChanged.PropertyChanged"/> event is invoked for the marked property as well.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Property)]
    public class DependsOnAttribute : Attribute
    {
        /// <summary>
        /// Gets the names of the dependent properties.
        /// </summary>
        /// <value>
        /// The names of the dependent properties.
        /// </value>
        public string[] PropertyNames { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependsOnAttribute"/> class.
        /// </summary>
        /// <param name="propertyNames">The names of the dependent properties.</param>
        public DependsOnAttribute(params string[] propertyNames)
        {
            PropertyNames = propertyNames;
        }
    }

    /// <summary>
    /// When applied to an interface, the class that implements it will be marked for the ObservableObjectGenerator from
    /// MaSch.Generators to automatically generate the properties as observable properties.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class ObservablePropertyDefinition : Attribute
    {
    }
}
