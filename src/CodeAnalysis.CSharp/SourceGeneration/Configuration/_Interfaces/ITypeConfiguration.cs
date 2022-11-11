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
        public static TConfig AsFileScoped<TConfig>(this TConfig builder)
            where TConfig : ITypeConfiguration
        {
            builder.WithAccessModifier(AccessModifier.File);
            return builder;
        }
    }
}