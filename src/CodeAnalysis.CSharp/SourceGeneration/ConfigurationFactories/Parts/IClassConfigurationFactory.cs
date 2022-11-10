using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

public interface IClassConfigurationFactory
{
    IClassConfiguration Class(string name);
}

partial class CodeConfigurationFactory : IClassConfigurationFactory
{
    public IClassConfiguration Class(string name)
    {
        _lastTypeName = name;
        return new ClassConfiguration(name);
    }
}
