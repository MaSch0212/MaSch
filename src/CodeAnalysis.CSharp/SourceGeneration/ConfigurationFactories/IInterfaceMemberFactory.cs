namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

public interface IInterfaceMemberFactory :
    IDelegateConfigurationFactory,
    IPropertyConfigurationFactory,
    IMethodConfigurationFactory,
    IEventConfigurationFactory,
    IIndexerConfigurationFactory,
    IEnumConfigurationFactory,
    IInterfaceConfigurationFactory,
    IClassConfigurationFactory,
    IStructConfigurationFactory,
    IRecordConfgurationFactory
{
}

partial class CodeConfigurationFactory : IInterfaceMemberFactory
{
}
