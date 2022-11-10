namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

public interface INamespaceMemberFactory :
    INamespaceConfigurationFactory,
    IDelegateConfigurationFactory,
    IEnumConfigurationFactory,
    IInterfaceConfigurationFactory,
    IClassConfigurationFactory,
    IStructConfigurationFactory,
    IRecordConfgurationFactory
{
}

partial class CodeConfigurationFactory : INamespaceMemberFactory
{
}
