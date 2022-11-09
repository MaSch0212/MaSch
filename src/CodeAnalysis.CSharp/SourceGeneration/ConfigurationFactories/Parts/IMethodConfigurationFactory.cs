using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface IMethodConfigurationFactory
{
    IMethodConfiguration Method(string name);
    IMethodConfiguration Method(string returnType, string name);
}

partial class CodeConfigurationFactory : IMethodConfigurationFactory
{
    public IMethodConfiguration Method(string name)
    {
        return new MethodConfiguration(name);
    }

    public IMethodConfiguration Method(string returnType, string name)
    {
        return new MethodConfiguration(name).WithReturnType(returnType);
    }
}