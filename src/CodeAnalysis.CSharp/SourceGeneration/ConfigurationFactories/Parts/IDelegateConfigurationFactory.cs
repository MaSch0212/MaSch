using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

public interface IDelegateConfigurationFactory
{
    IDelegateConfiguration Delegate(string name);
}

partial class CodeConfigurationFactory : IDelegateConfigurationFactory
{
    public IDelegateConfiguration Delegate(string name)
    {
        return new DelegateConfiguration(name);
    }
}
