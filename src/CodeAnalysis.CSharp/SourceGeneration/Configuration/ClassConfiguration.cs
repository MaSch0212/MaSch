namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface IClassConfiguration : ITypeConfiguration<IClassConfiguration>
{
}

public sealed class ClassConfiguration : TypeConfiguration<IClassConfiguration>, IClassConfiguration
{
    public ClassConfiguration(string className)
        : base(className)
    {

    }
}
