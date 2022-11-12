using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration
{
    public interface ITypeConfiguration : IGenericMemberConfiguration, ISupportsInheritanceConfiguration
    {
        ITypeConfiguration Implements(string interfaceTypeName);
    }

    public interface ITypeConfiguration<T> : ITypeConfiguration, IGenericMemberConfiguration<T>, ISupportsInheritanceConfiguration<T>
        where T : ITypeConfiguration<T>
    {
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
        /// Marks the code configuration as <c>file</c>.
        /// </summary>
        /// <typeparam name="TConfig">The type of code configuration.</typeparam>
        /// <param name="config">The code configuration to change.</param>
        /// <returns>A self-reference to <paramref name="config"/>.</returns>
        public static TConfig AsFileScoped<TConfig>(this TConfig config)
            where TConfig : ITypeConfiguration
        {
            config.WithAccessModifier(AccessModifier.File);
            return config;
        }
    }
}