using MaSch.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace MaSch.Presentation.Wpf.Attributes
{
    /// <summary>
    /// When applied to a property inside a class derived from the <see cref="Observable.ObservableDependencyObject"/> class,
    /// the <see cref="INotifyPropertyChanged.PropertyChanged"/> event is raised for the property when the dependency property with a given name is changed.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class NotifyDependencyPropertyChangedAttribute : Attribute
    {
        private static readonly Dictionary<Type, List<(string PropertyName, NotifyDependencyPropertyChangedAttribute Attribute)>> AttributeCache =
            new Dictionary<Type, List<(string PropertyName, NotifyDependencyPropertyChangedAttribute Attribute)>>();

        /// <summary>
        /// Gets the name of the dependency property.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Gets the type in which the dependency property is defined in.
        /// </summary>
        public Type? OwnerType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyDependencyPropertyChangedAttribute"/> class.
        /// </summary>
        /// <param name="propertyName">Name of the dependency property.</param>
        public NotifyDependencyPropertyChangedAttribute(string propertyName)
            : this (propertyName, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyDependencyPropertyChangedAttribute"/> class.
        /// </summary>
        /// <param name="propertyName">Name of the dependency property.</param>
        /// <param name="ownerType">Type in which the dependency property is defined in.</param>
        public NotifyDependencyPropertyChangedAttribute(string propertyName, Type? ownerType)
        {
            PropertyName = propertyName;
            OwnerType = ownerType;
        }

        /// <summary>
        /// Gets all defined <see cref="NotifyDependencyPropertyChangedAttribute"/> attributes from all property of an object.
        /// </summary>
        /// <param name="classObject">The object to retrieve the attributes from.</param>
        /// <returns>A <see cref="List{T}"/> of the <see cref="NotifyDependencyPropertyChangedAttribute"/> and the name of the property, the attribute is defined on.</returns>
        public static List<(string PropertyName, NotifyDependencyPropertyChangedAttribute Attribute)> GetAttributes(object classObject)
        {
            Guard.NotNull(classObject, nameof(classObject));
            var classType = classObject.GetType();

            List<(string PropertyName, NotifyDependencyPropertyChangedAttribute Attribute)> result;
            if (AttributeCache.ContainsKey(classType))
            {
                result = AttributeCache[classType];
            }
            else
            {
                result = (from x in classType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                          let att = x.GetCustomAttribute<NotifyDependencyPropertyChangedAttribute>()
                          where att != null
                          select (x.Name, att)).ToList();
                AttributeCache[classType] = result;
            }

            return result;
        }
    }
}
