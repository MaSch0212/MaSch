namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface INamespaceMemberFactory : INamespaceConfigurationFactory, IClassConfigurationFactory
{
}

partial class CodeConfigurationFactory : INamespaceMemberFactory
{
}
