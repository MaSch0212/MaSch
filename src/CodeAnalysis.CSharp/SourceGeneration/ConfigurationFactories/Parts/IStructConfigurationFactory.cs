using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface IStructConfigurationFactory
{
    IStructConfiguration Struct(string name);
}

partial class CodeConfigurationFactory : IStructConfigurationFactory
{
    public IStructConfiguration Struct(string name)
    {
        _lastTypeName = name;
        return new StructConfiguration(name);
    }
}
