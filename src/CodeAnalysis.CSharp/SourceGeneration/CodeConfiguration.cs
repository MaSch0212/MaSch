using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

/// <summary>
/// Provides methods to Creates all the different implementations of <see cref="ICodeConfiguration"/> that can be used to append code to the <see cref="ISourceBuilder"/>.
/// </summary>
public static class CodeConfiguration
{
    /// <summary>
    /// Creates a configuration for a namespace import.
    /// </summary>
    /// <param name="namespace">The namespace or type (in case of aliased or static import) to import.</param>
    /// <returns>An instance of <see cref="INamespaceImportConfiguration"/> that represents a namespace import.</returns>
    public static INamespaceImportConfiguration NamespaceImport(string @namespace)
    {
        return new NamespaceImportConfiguration(@namespace);
    }

    /// <summary>
    /// Creates a configuration for a namespace definition.
    /// </summary>
    /// <param name="name">The name of the namespace to define.</param>
    /// <returns>An instance of <see cref="INamespaceConfiguration"/> that represents a namespace definition.</returns>
    public static INamespaceConfiguration Namespace(string name)
    {
        return new NamespaceConfiguration(name);
    }

    /// <summary>
    /// Creates a configuration for a class definition.
    /// </summary>
    /// <param name="name">The name of the class to define.</param>
    /// <returns>An instance of <see cref="IClassConfiguration"/> that represents a class definition.</returns>
    public static IClassConfiguration Class(string name)
    {
        return new ClassConfiguration(name);
    }

    /// <summary>
    /// Creates a configuration for a record definition.
    /// </summary>
    /// <param name="name">The name of the record to define.</param>
    /// <returns>An instance of <see cref="IRecordConfiguration"/> that represents a record definition.</returns>
    public static IRecordConfiguration Record(string name)
    {
        return new RecordConfiguration(name);
    }

    /// <summary>
    /// Creates a configuration for a struct definition.
    /// </summary>
    /// <param name="name">The name of the struct to define.</param>
    /// <returns>An instance of <see cref="IStructConfiguration"/> that represents a struct definition.</returns>
    public static IStructConfiguration Struct(string name)
    {
        return new StructConfiguration(name);
    }

    /// <summary>
    /// Creates a configuration for an interface definition.
    /// </summary>
    /// <param name="name">The name of the interface to define.</param>
    /// <returns>An instance of <see cref="IInterfaceConfguration"/> that represents an interface definition.</returns>
    public static IInterfaceConfguration Interface(string name)
    {
        return new InterfaceConfguration(name);
    }

    /// <summary>
    /// Creates a configuration for an enum definition.
    /// </summary>
    /// <param name="name">The name of the enum to define.</param>
    /// <returns>An instance of <see cref="IEnumConfiguration"/> that represents an enum definition.</returns>
    public static IEnumConfiguration Enum(string name)
    {
        return new EnumConfiguration(name);
    }

    /// <summary>
    /// Creates a configuration for a delegate definition.
    /// </summary>
    /// <param name="name">The name of the delegate to define.</param>
    /// <returns>An instance of <see cref="IDelegateConfiguration"/> that represents a delegate definition.</returns>
    public static IDelegateConfiguration Delegate(string name)
    {
        return new DelegateConfiguration(name);
    }

    /// <summary>
    /// Creates a configuration for a field definition.
    /// </summary>
    /// <param name="fieldTypeName">The type of the field to define.</param>
    /// <param name="fieldName">The name of the field to define.</param>
    /// <returns>An instance of <see cref="IFieldConfiguration"/> that represents a field definition.</returns>
    public static IFieldConfiguration Field(string fieldTypeName, string fieldName)
    {
        return new FieldConfiguration(fieldTypeName, fieldName);
    }

    /// <summary>
    /// Creates a configuration for a constructor definition. The type name is automatically detected from the <see cref="ISourceBuilder"/>.
    /// </summary>
    /// <returns>An instance of <see cref="IConstructorConfiguration"/> that represents a constructor definition.</returns>
    public static IConstructorConfiguration Constructor()
    {
        return new ConstructorConfiguration(null);
    }

    /// <summary>
    /// Creates a configuration for a constructor definition.
    /// </summary>
    /// <param name="containingTypeName">The name of the type the constructor is defined in.</param>
    /// <returns>An instance of <see cref="IConstructorConfiguration"/> that represents a constructor definition.</returns>
    public static IConstructorConfiguration Constructor(string containingTypeName)
    {
        return new ConstructorConfiguration(containingTypeName);
    }

    /// <summary>
    /// Creates a configuration for a static constructor definition. The type name is automatically detected from the <see cref="ISourceBuilder"/>.
    /// </summary>
    /// <returns>An instance of <see cref="IStaticConstructorConfiguration"/> that represents a static constructor definition.</returns>
    public static IStaticConstructorConfiguration StaticConstructor()
    {
        return new StaticConstructorConfiguration(null);
    }

    /// <summary>
    /// Creates a configuration for a static constructor definition.
    /// </summary>
    /// <param name="containingTypeName">The name of the type the static constructor is defined in.</param>
    /// <returns>An instance of <see cref="IStaticConstructorConfiguration"/> that represents a static constructor definition.</returns>
    public static IStaticConstructorConfiguration StaticConstructor(string containingTypeName)
    {
        return new StaticConstructorConfiguration(containingTypeName);
    }

    /// <summary>
    /// Creates a configuration for a finalizer definition. The type name is automatically detected from the <see cref="ISourceBuilder"/>.
    /// </summary>
    /// <returns>An instance of <see cref="IFinalizerConfiguration"/> that represents a finalizer definition.</returns>
    public static IFinalizerConfiguration Finalizer()
    {
        return new FinalizerConfiguration(null);
    }

    /// <summary>
    /// Creates a configuration for a finalizer definition.
    /// </summary>
    /// <param name="containingTypeName">The name of the type the finalizer is defined in.</param>
    /// <returns>An instance of <see cref="IFinalizerConfiguration"/> that represents a finalizer definition.</returns>
    public static IFinalizerConfiguration Finalizer(string containingTypeName)
    {
        return new FinalizerConfiguration(containingTypeName);
    }

    /// <summary>
    /// Creates a configuration for an event definition.
    /// </summary>
    /// <param name="eventType">The type of the event to define.</param>
    /// <param name="eventName">The name of the event to define.</param>
    /// <returns>An instance of <see cref="IEventConfiguration"/> that represents an event definition.</returns>
    public static IEventConfiguration Event(string eventType, string eventName)
    {
        return new EventConfiguration(eventType, eventName);
    }

    /// <summary>
    /// Creates a configuration for a property definition.
    /// </summary>
    /// <param name="propertyTypeName">The type of the property to define.</param>
    /// <param name="propertyName">The name of the property to define.</param>
    /// <returns>An instance of <see cref="IPropertyConfiguration"/> that represents a property definition.</returns>
    public static IPropertyConfiguration Property(string propertyTypeName, string propertyName)
    {
        return new PropertyConfiguration(propertyTypeName, propertyName);
    }

    /// <summary>
    /// Creates a configuration for an indexer definition.
    /// </summary>
    /// <param name="indexerType">The type of the indexer to define.</param>
    /// <returns>An instance of <see cref="IPropertyConfiguration"/> that represents an indexer definition.</returns>
    public static IIndexerConfiguration Indexer(string indexerType)
    {
        return new IndexerConfiguration(indexerType);
    }

    /// <summary>
    /// Creates a configuration for a method definition.
    /// </summary>
    /// <param name="name">The name of the method to define.</param>
    /// <returns>An instance of <see cref="IMethodConfiguration"/> that represents a method definition.</returns>
    public static IMethodConfiguration Method(string name)
    {
        return new MethodConfiguration(name);
    }

    /// <summary>
    /// Creates a configuration for a method definition.
    /// </summary>
    /// <param name="returnType">The type of the method to define.</param>
    /// <param name="name">The name of the method to define.</param>
    /// <returns>An instance of <see cref="IMethodConfiguration"/> that represents a method definition.</returns>
    public static IMethodConfiguration Method(string returnType, string name)
    {
        return new MethodConfiguration(name).WithReturnType(returnType);
    }

    /// <summary>
    /// Creates a configuration for an enum value definition.
    /// </summary>
    /// <param name="name">The name of the enum value to define.</param>
    /// <returns>An instance of <see cref="IEnumValueConfiguration"/> that represents an enum value definition.</returns>
    public static IEnumValueConfiguration EnumValue(string name)
    {
        return new EnumValueConfiguration(name);
    }

    /// <summary>
    /// Creates a configuration for an enum value definition.
    /// </summary>
    /// <param name="name">The name of the enum value to define.</param>
    /// <param name="value">The value of the enum value to define.</param>
    /// <returns>An instance of <see cref="IEnumValueConfiguration"/> that represents an enum value definition.</returns>
    public static IEnumValueConfiguration EnumValue(string name, string value)
    {
        return new EnumValueConfiguration(name, value);
    }

    /// <summary>
    /// Creates a configuration for an assembly attribute definition.
    /// </summary>
    /// <param name="attributeTypeName">The type of the assembly attribute to define.</param>
    /// <returns>An instance of <see cref="IEnumValueConfiguration"/> that represents an assembly attribute definition.</returns>
    public static ICodeAttributeConfiguration AssemblyAttribute(string attributeTypeName)
    {
        return new CodeAttributeConfiguration(attributeTypeName).OnAssembly();
    }

    /// <summary>
    /// Creates a configuration for a region.
    /// </summary>
    /// <param name="name">The name of the region.</param>
    /// <returns>An instance of <see cref="IRegionConfiguration"/> that represents a region.</returns>
    public static IRegionConfiguration Region(string name)
    {
        return new RegionConfiguration(name);
    }

    /// <summary>
    /// Creates a configuration for a code block.
    /// </summary>
    /// <returns>An instance of <see cref="IRegionConfiguration"/> that represents a code block.</returns>
    public static ICodeBlockConfiguration Block()
    {
        return new CodeBlockConfiguration();
    }

    /// <summary>
    /// Creates a configuration for a code block.
    /// </summary>
    /// <param name="blockPrefix">The prefix of the code block.</param>
    /// <returns>An instance of <see cref="IRegionConfiguration"/> that represents a code block.</returns>
    public static ICodeBlockConfiguration Block(string? blockPrefix)
    {
        return new CodeBlockConfiguration(blockPrefix);
    }

    /// <summary>
    /// Creates a configuration for a code block.
    /// </summary>
    /// <param name="blockPrefix">The prefix of the code block.</param>
    /// <param name="blockSuffix">The suffix of the code block.</param>
    /// <returns>An instance of <see cref="IRegionConfiguration"/> that represents a code block.</returns>
    public static ICodeBlockConfiguration Block(string? blockPrefix, string? blockSuffix)
    {
        return new CodeBlockConfiguration(blockPrefix, blockSuffix);
    }
}
