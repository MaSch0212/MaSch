namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

public interface IClassMemberFactory :
    IFieldConfigurationFactory,
    IDelegateConfigurationFactory,
    IPropertyConfigurationFactory,
    IMethodConfigurationFactory,
    IEventConfigurationFactory,
    IIndexerConfigurationFactory,
    IConstructorConfigurationFactory,
    IFinalizerConfigurationFactory,
    IEnumConfigurationFactory,
    IInterfaceConfigurationFactory,
    IClassConfigurationFactory,
    IStructConfigurationFactory,
    IRecordConfgurationFactory
{
}

partial class CodeConfigurationFactory : IClassMemberFactory
{
}