namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

public interface IStructMemberFactory :
    IFieldConfigurationFactory,
    IDelegateConfigurationFactory,
    IPropertyConfigurationFactory,
    IMethodConfigurationFactory,
    IEventConfigurationFactory,
    // TODO: Indexer
    IConstructorConfigurationFactory,
    IEnumConfigurationFactory,
    IInterfaceConfigurationFactory,
    IClassConfigurationFactory,
    IStructConfigurationFactory,
    IRecordConfgurationFactory
{
}

partial class CodeConfigurationFactory : IStructMemberFactory
{
}