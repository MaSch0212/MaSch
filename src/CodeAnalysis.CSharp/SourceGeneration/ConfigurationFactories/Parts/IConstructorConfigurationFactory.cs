using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface IConstructorConfigurationFactory
{
    IConstructorConfiguration Constructor();
    IConstructorConfiguration Constructor(string containingTypeName);
    IStaticConstructorConfiguration StaticConstructor();
    IStaticConstructorConfiguration StaticConstructor(string containingTypeName);
    IFinalizerConfiguration Finalizer();
    IFinalizerConfiguration Finalizer(string containingTypeName);
}

partial class CodeConfigurationFactory : IConstructorConfigurationFactory
{
    public IConstructorConfiguration Constructor()
    {
        return new ConstructorConfiguration(_lastTypeName);
    }

    public IConstructorConfiguration Constructor(string containingTypeName)
    {
        _lastTypeName = containingTypeName;
        return new ConstructorConfiguration(containingTypeName);
    }

    public IStaticConstructorConfiguration StaticConstructor()
    {
        return new StaticConstructorConfiguration(_lastTypeName);
    }

    public IStaticConstructorConfiguration StaticConstructor(string containingTypeName)
    {
        _lastTypeName = containingTypeName;
        return new StaticConstructorConfiguration(containingTypeName);
    }

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
