namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface ISourceTypeBuilder<T>
    where T : ISourceTypeBuilder<T>
{
    T DrivesFrom(string typeName);
}