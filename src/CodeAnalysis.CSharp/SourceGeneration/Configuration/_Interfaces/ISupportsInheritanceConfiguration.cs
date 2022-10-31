namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface ISupportsInheritanceConfiguration : ICodeConfiguration
{
    ISupportsInheritanceConfiguration DerivesFrom(string typeName);
}

public interface ISupportsInheritanceConfiguration<T> : ISupportsInheritanceConfiguration
    where T : ISupportsInheritanceConfiguration<T>
{
    new T DerivesFrom(string typeName);
}
