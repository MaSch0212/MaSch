using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

public interface IInterfaceConfigurationFactory
{
    IInterfaceConfguration Interface(string name);
}

partial class CodeConfigurationFactory : IInterfaceConfigurationFactory
{
    public IInterfaceConfguration Interface(string name)
    {
        _lastTypeName = name;
        return new InterfaceConfguration(name);
    }
}