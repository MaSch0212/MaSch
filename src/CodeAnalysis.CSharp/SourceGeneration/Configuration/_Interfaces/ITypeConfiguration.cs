using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration
{
    /// <summary>
    /// Represents configuration of a code element that is a type (like class, interface, struct, ...). This is used to generate code in the <see cref="ISourceBuilder"/>.
    /// </summary>
    public interface ITypeConfiguration : IGenericMemberConfiguration, ISupportsInheritanceConfiguration
    {
        /// <summary>
        /// Gets a read-only list of interfaces that the type represented by this <see cref="ITypeConfiguration"/> implements.
        /// </summary>
        IReadOnlyList<string> InterfaceImplementations { get; }

        /// <summary>
        /// Adds an interface to the list of implemented interface of this <see cref="ITypeConfiguration"/>.
        /// </summary>
        /// <param name="interfaceTypeName">The interface type to add.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        ITypeConfiguration Implements(string interfaceTypeName);
    }

    /// <inheritdoc cref="ITypeConfiguration"/>
    /// <typeparam name="T">The type of <see cref="ICodeConfiguration"/>.</typeparam>
    public interface ITypeConfiguration<T> : ITypeConfiguration, IGenericMemberConfiguration<T>, ISupportsInheritanceConfiguration<T>
        where T : ITypeConfiguration<T>
    {
        /// <inheritdoc cref="ITypeConfiguration.Implements(string)"/>
        new T Implements(string interfaceTypeName);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    /// <summary>
    /// Provides extension methods for the <see cref="ITypeConfiguration"/> interface.
    /// </summary>
    public static class TypeConfigurationExtensions
    {
        /// <summary>
        /// Sets the access modifier of this <see cref="ITypeConfiguration"/> to <see cref="AccessModifier.File"/>.
        /// </summary>
        /// <typeparam name="TConfig">The type of <see cref="ICodeConfiguration"/>.</typeparam>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static TConfig AsFileScoped<TConfig>(this TConfig config)
            where TConfig : ITypeConfiguration
        {
            config.WithAccessModifier(AccessModifier.File);
            return config;
        }
    }
}