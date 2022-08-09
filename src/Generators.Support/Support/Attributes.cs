using System;

#pragma warning disable SA1649 // File name should match first type name
#pragma warning disable SA1402 // File may only contain a single type

#nullable enable

namespace MaSch.Generators.Support
{
    /// <summary>
    /// If added to a class or struct, boiler plat code is generated.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public class FileGeneratorAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileGeneratorAttribute"/> class.
        /// </summary>
        /// <param name="contextType">The type of context for the file generator.</param>
        public FileGeneratorAttribute(Type contextType)
        {
            ContextType = contextType;
        }

        /// <summary>
        /// Gets the type of context for the file generator.
        /// </summary>
        public Type ContextType { get; }
    }

    /// <summary>
    /// If added to a class or struct, boiler plat code is generated.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public class MemberGeneratorAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberGeneratorAttribute"/> class.
        /// </summary>
        /// <param name="contextType">The type of context for the file generator.</param>
        public MemberGeneratorAttribute(Type contextType)
        {
            ContextType = contextType;
        }

        /// <summary>
        /// Gets the type of context for the file generator.
        /// </summary>
        public Type ContextType { get; }
    }

    /// <summary>
    /// If added to a class or struct, boiler plat code is generated.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public class SyntaxValidatorAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyntaxValidatorAttribute"/> class.
        /// </summary>
        /// <param name="syntaxType">The type of syntax to validate.</param>
        public SyntaxValidatorAttribute(Type syntaxType)
        {
            SyntaxType = syntaxType;
        }

        /// <summary>
        /// Gets the type of syntax to validate.
        /// </summary>
        public Type SyntaxType { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the validator needs the semantic model.
        /// </summary>
        public bool NeedsSemanticModel { get; set; } = true;
    }

    /// <summary>
    /// If added to a class or struct, boiler plat code is generated.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public class IncrementalValueProviderFactoryAttribute : Attribute
    {
    }
}