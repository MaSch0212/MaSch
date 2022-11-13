using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public static class CodeConfiguration
{
    public static INamespaceImportConfiguration NamespaceImport(string @namespace)
    {
        return new NamespaceImportConfiguration(@namespace);
    }

    public static INamespaceConfiguration Namespace(string name)
    {
        return new NamespaceConfiguration(name);
    }

    public static IClassConfiguration Class(string name)
    {
        return new ClassConfiguration(name);
    }

    public static IRecordConfiguration Record(string name)
    {
        return new RecordConfiguration(name);
    }

    public static IStructConfiguration Struct(string name)
    {
        return new StructConfiguration(name);
    }

    public static IInterfaceConfguration Interface(string name)
    {
        return new InterfaceConfguration(name);
    }

    public static IEnumConfiguration Enum(string name)
    {
        return new EnumConfiguration(name);
    }

    public static IDelegateConfiguration Delegate(string name)
    {
        return new DelegateConfiguration(name);
    }

    public static IFieldConfiguration Field(string fieldTypeName, string fieldName)
    {
        return new FieldConfiguration(fieldTypeName, fieldName);
    }

    public static IConstructorConfiguration Constructor()
    {
        return new ConstructorConfiguration(null);
    }

    public static IConstructorConfiguration Constructor(string containingTypeName)
    {
        return new ConstructorConfiguration(containingTypeName);
    }

    public static IStaticConstructorConfiguration StaticConstructor()
    {
        return new StaticConstructorConfiguration(null);
    }

    public static IStaticConstructorConfiguration StaticConstructor(string containingTypeName)
    {
        return new StaticConstructorConfiguration(containingTypeName);
    }

    public static IFinalizerConfiguration Finalizer()
    {
        return new FinalizerConfiguration(null);
    }

    public static IFinalizerConfiguration Finalizer(string containingTypeName)
    {
        return new FinalizerConfiguration(containingTypeName);
    }

    public static IEventConfiguration Event(string eventType, string eventName)
    {
        return new EventConfiguration(eventType, eventName);
    }

    public static IPropertyConfiguration Property(string propertyTypeName, string propertyName)
    {
        return new PropertyConfiguration(propertyTypeName, propertyName);
    }

    public static IIndexerConfiguration Indexer(string indexerType)
    {
        return new IndexerConfiguration(indexerType);
    }

    public static IMethodConfiguration Method(string name)
    {
        return new MethodConfiguration(name);
    }

    public static IMethodConfiguration Method(string returnType, string name)
    {
        return new MethodConfiguration(name).WithReturnType(returnType);
    }

    public static IEnumValueConfiguration EnumValue(string name)
    {
        return new EnumValueConfiguration(name);
    }

    public static IEnumValueConfiguration EnumValue(string name, string value)
    {
        return new EnumValueConfiguration(name, value);
    }

    public static ICodeAttributeConfiguration AssemblyAttribute(string attributeTypeName)
    {
        return new CodeAttributeConfiguration(attributeTypeName).OnAssembly();
    }
}
