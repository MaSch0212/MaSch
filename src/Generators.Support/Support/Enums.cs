namespace MaSch.Generators.Support
{
    /// <summary>
    /// Specifies an access modifier of a member in C#.
    /// </summary>
    public enum AccessModifier
    {
        /// <summary>
        /// The type or member can be accessed by any other code in the same assembly or another assembly that references it. The accessibility level of public members of a type is controlled by the accessibility level of the type itself.
        /// </summary>
        Public,

        /// <summary>
        /// The type or member can be accessed only by code in the same <c>class</c> or <c>struct</c>.
        /// </summary>
        Private,

        /// <summary>
        /// The type or member can be accessed only by code in the same <c>class</c>, or in a <c>class</c> that is derived from that <c>class</c>.
        /// </summary>
        Protected,

        /// <summary>
        /// The type or member can be accessed by any code in the same assembly, but not from another assembly. In other words, <c>internal</c> types or members can be accessed from code that is part of the same compilation.
        /// </summary>
        Internal,

        /// <summary>
        /// The type or member can be accessed by any code in the assembly in which it's declared, or from within a derived <c>class</c> in another assembly.
        /// </summary>
        ProtectedInternal,

        /// <summary>
        /// The type or member can be accessed by types derived from the <c>class</c> that are declared within its containing assembly.
        /// </summary>
        PrivateProtected,
    }

    /// <summary>
    /// Specified the target for a code attribute.
    /// </summary>
    public enum CodeAttributeTarget
    {
        /// <summary>
        /// Automatically detect target from the context.
        /// </summary>
        Default,

        /// <summary>
        /// Entire assembly.
        /// </summary>
        Assembly,

        /// <summary>
        /// Current assembly module.
        /// </summary>
        Module,

        /// <summary>
        /// Field in a class or a struct.
        /// </summary>
        Field,

        /// <summary>
        /// Event.
        /// </summary>
        Event,

        /// <summary>
        /// Method or <c>get</c> and <c>set</c> property accessors.
        /// </summary>
        Method,

        /// <summary>
        /// Method parameters or <c>set</c> property accessor parameters.
        /// </summary>
        Parameter,

        /// <summary>
        /// Property.
        /// </summary>
        Property,

        /// <summary>
        /// Return value of a method, property indexer, or <c>get</c> property accessor.
        /// </summary>
        Return,

        /// <summary>
        /// Struct, class, interface, enum, or delegate.
        /// </summary>
        Type,
    }
}