using MaSch.Core.Observable;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

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
    [ExcludeFromCodeCoverage]
    public class DependsOnAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependsOnAttribute"/> class.
        /// </summary>
        /// <param name="propertyNames">The names of the dependent properties.</param>
        public DependsOnAttribute(params string[] propertyNames)
        {
            PropertyNames = propertyNames;
        }

        /// <summary>
        /// Gets the names of the dependent properties.
        /// </summary>
        /// <value>
        /// The names of the dependent properties.
        /// </value>
        public string[] PropertyNames { get; }
    }

    /// <summary>
    /// When applied to a class, the <see cref="IObservableObject"/> interfaces is automatically implemented using MaSch.Generators.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class GenerateObservableObjectAttribute : Attribute
    {
    }

    /// <summary>
    /// When applied to a class, the <see cref="INotifyPropertyChanged"/> interfaces is automatically implemented using MaSch.Generators.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class GenerateNotifyPropertyChangedAttribute : Attribute
    {
    }

    /// <summary>
    /// When applied to an interface, the class that implements it will be marked for the ObservableObjectGenerator from
    /// MaSch.Generators to automatically generate the properties as observable properties.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class ObservablePropertyDefinitionAttribute : Attribute
    {
    }

    /// <summary>
    /// When applied to a property inside an interface with the <see cref="ObservablePropertyDefinitionAttribute"/>, sets the access modifier of the generated property.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    [ExcludeFromCodeCoverage]
    public class ObservablePropertyAccessModifierAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObservablePropertyAccessModifierAttribute"/> class.
        /// </summary>
        public ObservablePropertyAccessModifierAttribute()
            : this(AccessModifier.Public)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservablePropertyAccessModifierAttribute"/> class.
        /// </summary>
        /// <param name="accessModifier">The access modifier to use for the generated property.</param>
        public ObservablePropertyAccessModifierAttribute(AccessModifier accessModifier)
        {
            AccessModifier = accessModifier;
            SetModifier = accessModifier;
            GetModifier = accessModifier;
        }

        /// <summary>
        /// Gets the access modifier to use for the generated property.
        /// </summary>
        public AccessModifier AccessModifier { get; }

        /// <summary>
        /// Gets or sets the modifier to use for the setter of the generated property.
        /// </summary>
        public AccessModifier SetModifier { get; set; }

        /// <summary>
        /// Gets or sets the modifier to use for the getter of the generated property.
        /// </summary>
        public AccessModifier GetModifier { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the generated property should have the <c>virtual</c> keyword.
        /// </summary>
        public bool IsVirtual { get; set; }
    }
}
