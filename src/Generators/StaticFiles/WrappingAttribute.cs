#nullable enable

namespace MaSch.Core
{
    /// <summary>
    /// When applied to a class, the WrapperGenerator generator from MaSch.Generators will create wrapper members for the given type.
    /// </summary>
    /// <seealso cref="global::System.Attribute" />
    [global::System.AttributeUsage(global::System.AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    internal class WrappingAttribute : global::System.Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WrappingAttribute"/> class.
        /// </summary>
        /// <param name="typeToWrap">The type to wrap.</param>
        public WrappingAttribute(global::System.Type typeToWrap)
        {
            TypeToWrap = typeToWrap;
        }

        /// <summary>
        /// Gets the type to wrap.
        /// </summary>
        public global::System.Type TypeToWrap { get; }

        /// <summary>
        /// Gets or sets the name of the property for the instance that is wrapped.
        /// </summary>
        public string? WrappingPropName { get; set; }
    }
}