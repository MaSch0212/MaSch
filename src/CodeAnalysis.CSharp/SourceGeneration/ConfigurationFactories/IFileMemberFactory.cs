namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface IFileMemberFactory :
    INamespaceConfigurationFactory,
    INamespaceImportConfigurationFactory
{
}

partial class CodeConfigurationFactory : IFileMemberFactory
{
}