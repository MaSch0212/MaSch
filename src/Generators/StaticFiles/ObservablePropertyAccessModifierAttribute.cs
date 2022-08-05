#nullable enable

namespace MaSch.Core
{
    /// <summary>
    /// When applied to a property inside an interface with the <see cref="global::MaSch.Core.ObservablePropertyDefinitionAttribute"/>, sets the access modifier of the generated property.
    /// </summary>
    /// <seealso cref="global::System.Attribute" />
    [global::System.AttributeUsage(global::System.AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    internal class ObservablePropertyAccessModifierAttribute : global::System.Attribute
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