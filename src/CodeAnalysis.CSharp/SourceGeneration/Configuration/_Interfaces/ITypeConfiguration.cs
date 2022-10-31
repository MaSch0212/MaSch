namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface ITypeConfiguration : IGenericMemberConfiguration, ISupportsInheritanceConfiguration
{
    ITypeConfiguration Implements(string interfaceTypeName);
}

public interface ITypeConfiguration<T> : ITypeConfiguration, IGenericMemberConfiguration<T>, ISupportsInheritanceConfiguration<T>
    where T : ITypeConfiguration<T>
{
    new T Implements(string interfaceTypeName);
}