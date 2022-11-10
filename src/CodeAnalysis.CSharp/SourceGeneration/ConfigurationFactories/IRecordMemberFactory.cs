namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

public interface IRecordMemberFactory :
    IFieldConfigurationFactory,
    IDelegateConfigurationFactory,
    IPropertyConfigurationFactory,
    IMethodConfigurationFactory,
    IEventConfigurationFactory,
    // TODO: Indexer
    IConstructorConfigurationFactory,
    IFinalizerConfigurationFactory,
    IEnumConfigurationFactory,
    IInterfaceConfigurationFactory,
    IClassConfigurationFactory,
    IStructConfigurationFactory,
    IRecordConfgurationFactory
{
}

partial class CodeConfigurationFactory : IRecordMemberFactory
{
}