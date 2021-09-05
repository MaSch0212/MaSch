using System;
using System.Diagnostics.CodeAnalysis;

namespace MaSch.Core.Attributes
{
    /// <summary>
    /// When applied to a class, the WrapperGenerator generator from MaSch.Generators will create wrapper members for the given type.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    [ExcludeFromCodeCoverage]
    public class WrappingAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WrappingAttribute"/> class.
        /// </summary>
        /// <param name="typeToWrap">The type to wrap.</param>
        public WrappingAttribute(Type typeToWrap)
        {
            TypeToWrap = typeToWrap;
        }

        /// <summary>
        /// Gets the type to wrap.
        /// </summary>
        public Type TypeToWrap { get; }

        /// <summary>
        /// Gets or sets the name of the property for the instance that is wrapped.
        /// </summary>
        public string? WrappingPropName { get; set; }
    }
}
