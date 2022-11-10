using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

public interface IFinalizerConfigurationFactory
{
    IFinalizerConfiguration Finalizer();
    IFinalizerConfiguration Finalizer(string containingTypeName);
}

partial class CodeConfigurationFactory : IFinalizerConfigurationFactory
{
    public IFinalizerConfiguration Finalizer()
    {
        return new FinalizerConfiguration(_lastTypeName);
    }

    public IFinalizerConfiguration Finalizer(string containingTypeName)
    {
        _lastTypeName = containingTypeName;
        return new FinalizerConfiguration(containingTypeName);
    }
}