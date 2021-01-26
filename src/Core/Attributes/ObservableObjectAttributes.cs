using System;
using System.ComponentModel;

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

    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class ObservablePropertyDefinition : Attribute
    {
    }
}
