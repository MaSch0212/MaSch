namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface ITypeConfiguration<T> : IMemberConfiguration<T>
    where T : ITypeConfiguration<T>
{
    T DrivesFrom(string typeName);
    T Implements(string interfaceTypeName);
}