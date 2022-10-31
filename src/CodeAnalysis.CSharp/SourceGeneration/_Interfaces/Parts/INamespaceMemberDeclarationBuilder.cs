using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface INamespaceMemberDeclarationBuilder : ISourceBuilder
{
    SourceBuilderCodeBlock AppendClass<TParams>(string className, out IClassBuilder classBuilder, TParams @params, Action<IClassConfiguration, TParams> classConfiguration);
    SourceBuilderCodeBlock AppendRecord<TParams>(string recordName, out IRecordBuilder recordBuilder, TParams @params, Action<IRecordConfiguration, TParams> recordConfiguration);
    SourceBuilderCodeBlock AppendInterface<TParams>(string interfaceName, out IInterfaceBuilder interfaceBuilder, TParams @params, Action<IInterfaceConfguration, TParams> interfaceConfiguration);
    SourceBuilderCodeBlock AppendStruct<TParams>(string structName, out IStructBuilder structBuilder, TParams @params, Action<IStructConfiguration, TParams> structConfiguration);
    SourceBuilderCodeBlock AppendEnum<TParams>(string enumName, out IEnumBuilder enumBuilder, TParams @params, Action<IEnumConfiguration, TParams> enumConfiguration);
    INamespaceMemberDeclarationBuilder AppendDelegate<TParams>(string delegateName, TParams @params, Action<IDelegateConfiguration, TParams> delegateConfiguration);
}

public interface INamespaceMemberDeclarationBuilder<T> : INamespaceMemberDeclarationBuilder
    where T : INamespaceMemberDeclarationBuilder<T>
{
    new T AppendDelegate<TParams>(string delegateName, TParams @params, Action<IDelegateConfiguration, TParams> delegateConfiguration);
}

public partial class SourceBuilder : INamespaceMemberDeclarationBuilder
{
    /// <inheritdoc/>
    SourceBuilderCodeBlock INamespaceMemberDeclarationBuilder.AppendClass<TParams>(string className, out IClassBuilder classBuilder, TParams @params, Action<IClassConfiguration, TParams> classConfiguration)
        => AppendClass(className, out classBuilder, @params, classConfiguration);

    /// <inheritdoc/>
    INamespaceMemberDeclarationBuilder INamespaceMemberDeclarationBuilder.AppendDelegate<TParams>(string delegateName, TParams @params, Action<IDelegateConfiguration, TParams> delegateConfiguration)
        => AppendDelegate(delegateName, @params, delegateConfiguration);

    /// <inheritdoc/>
    SourceBuilderCodeBlock INamespaceMemberDeclarationBuilder.AppendEnum<TParams>(string enumName, out IEnumBuilder enumBuilder, TParams @params, Action<IEnumConfiguration, TParams> enumConfiguration)
        => AppendEnum(enumName, out enumBuilder, @params, enumConfiguration);

    /// <inheritdoc/>
    SourceBuilderCodeBlock INamespaceMemberDeclarationBuilder.AppendInterface<TParams>(string interfaceName, out IInterfaceBuilder interfaceBuilder, TParams @params, Action<IInterfaceConfguration, TParams> interfaceConfiguration)
        => AppendInterface(interfaceName, out interfaceBuilder, @params, interfaceConfiguration);

    /// <inheritdoc/>
    SourceBuilderCodeBlock INamespaceMemberDeclarationBuilder.AppendRecord<TParams>(string recordName, out IRecordBuilder recordBuilder, TParams @params, Action<IRecordConfiguration, TParams> recordConfiguration)
        => AppendRecord(recordName, out recordBuilder, @params, recordConfiguration);

    /// <inheritdoc/>
    SourceBuilderCodeBlock INamespaceMemberDeclarationBuilder.AppendStruct<TParams>(string structName, out IStructBuilder structBuilder, TParams @params, Action<IStructConfiguration, TParams> structConfiguration)
        => AppendStruct(structName, out structBuilder, @params, structConfiguration);
}

/// <summary>
/// Provides extension methods for the <see cref="INamespaceMemberDeclarationBuilder{T}"/> interface.
/// </summary>
public static class NamespaceMemberDeclarationBuilderExtensions
{
    public static SourceBuilderCodeBlock AppendClass<TBuilder>(this TBuilder builder, string className, out IClassBuilder classBuilder, Action<IClassConfiguration> classConfiguration)
        where TBuilder : INamespaceMemberDeclarationBuilder
        => builder.AppendClass(className, out classBuilder, classConfiguration, static (builder, config) => config?.Invoke(builder));

    public static TBuilder AppendClass<TBuilder, TConfigParams, TBuilderParams>(
        this TBuilder builder,
        string className,
        TConfigParams classConfigurationParams,
        Action<IClassConfiguration, TConfigParams> classConfiguration,
        TBuilderParams classBuilderParams,
        Action<IClassBuilder, TBuilderParams> classBuilderAction)
        where TBuilder : INamespaceMemberDeclarationBuilder
    {
        using (builder.AppendClass(className, out var classBuilder, classConfigurationParams, classConfiguration))
            classBuilderAction?.Invoke(classBuilder, classBuilderParams);
        return builder;
    }

    public static TBuilder AppendClass<TBuilder, TBuilderParams>(
        this TBuilder builder,
        string className,
        Action<IClassConfiguration> classConfiguration,
        TBuilderParams classBuilderParams,
        Action<IClassBuilder, TBuilderParams> classBuilderAction)
        where TBuilder : INamespaceMemberDeclarationBuilder
    {
        using (builder.AppendClass(className, out var classBuilder, classConfiguration, static (builder, config) => config?.Invoke(builder)))
            classBuilderAction?.Invoke(classBuilder, classBuilderParams);
        return builder;
    }

    public static TBuilder AppendClass<TBuilder, TConfigParams>(
        this TBuilder builder,
        string className,
        TConfigParams classConfigurationParams,
        Action<IClassConfiguration, TConfigParams> classConfiguration,
        Action<IClassBuilder> classBuilderAction)
        where TBuilder : INamespaceMemberDeclarationBuilder
    {
        using (builder.AppendClass(className, out var classBuilder, classConfigurationParams, classConfiguration))
            classBuilderAction?.Invoke(classBuilder);
        return builder;
    }

    public static TBuilder AppendClass<TBuilder>(
        this TBuilder builder,
        string className,
        Action<IClassConfiguration> classConfiguration,
        Action<IClassBuilder> classBuilderAction)
        where TBuilder : INamespaceMemberDeclarationBuilder
    {
        using (builder.AppendClass(className, out var classBuilder, classConfiguration, static (builder, config) => config?.Invoke(builder)))
            classBuilderAction?.Invoke(classBuilder);
        return builder;
    }

    public static SourceBuilderCodeBlock AppendRecord<TBuilder>(this TBuilder builder, string recordName, out IRecordBuilder recordBuilder, Action<IRecordConfiguration> recordConfiguration)
        where TBuilder : INamespaceMemberDeclarationBuilder
        => builder.AppendRecord(recordName, out recordBuilder, recordConfiguration, static (builder, config) => config?.Invoke(builder));

    public static TBuilder AppendRecord<TBuilder, TConfigParams, TBuilderParams>(
        this TBuilder builder,
        string recordName,
        TConfigParams recordConfigurationParams,
        Action<IRecordConfiguration, TConfigParams> recordConfiguration,
        TBuilderParams recordBuilderParams,
        Action<IRecordBuilder, TBuilderParams> recordBuilderAction)
        where TBuilder : INamespaceMemberDeclarationBuilder
    {
        using (builder.AppendRecord(recordName, out var recordBuilder, recordConfigurationParams, recordConfiguration))
            recordBuilderAction?.Invoke(recordBuilder, recordBuilderParams);
        return builder;
    }

    public static TBuilder AppendRecord<TBuilder, TBuilderParams>(
        this TBuilder builder,
        string recordName,
        Action<IRecordConfiguration> recordConfiguration,
        TBuilderParams recordBuilderParams,
        Action<IRecordBuilder, TBuilderParams> recordBuilderAction)
        where TBuilder : INamespaceMemberDeclarationBuilder
    {
        using (builder.AppendRecord(recordName, out var recordBuilder, recordConfiguration, static (builder, config) => config?.Invoke(builder)))
            recordBuilderAction?.Invoke(recordBuilder, recordBuilderParams);
        return builder;
    }

    public static TBuilder AppendRecord<TBuilder, TConfigParams>(
        this TBuilder builder,
        string recordName,
        TConfigParams recordConfigurationParams,
        Action<IRecordConfiguration, TConfigParams> recordConfiguration,
        Action<IRecordBuilder> recordBuilderAction)
        where TBuilder : INamespaceMemberDeclarationBuilder
    {
        using (builder.AppendRecord(recordName, out var recordBuilder, recordConfigurationParams, recordConfiguration))
            recordBuilderAction?.Invoke(recordBuilder);
        return builder;
    }

    public static TBuilder AppendRecord<TBuilder>(
        this TBuilder builder,
        string recordName,
        Action<IRecordConfiguration> recordConfiguration,
        Action<IRecordBuilder> recordBuilderAction)
        where TBuilder : INamespaceMemberDeclarationBuilder
    {
        using (builder.AppendRecord(recordName, out var recordBuilder, recordConfiguration, static (builder, config) => config?.Invoke(builder)))
            recordBuilderAction?.Invoke(recordBuilder);
        return builder;
    }

    public static SourceBuilderCodeBlock AppendInterface<TBuilder>(this TBuilder builder, string interfaceName, out IInterfaceBuilder interfaceBuilder, Action<IInterfaceConfguration> interfaceConfiguration)
        where TBuilder : INamespaceMemberDeclarationBuilder
        => builder.AppendInterface(interfaceName, out interfaceBuilder, interfaceConfiguration, static (builder, config) => config?.Invoke(builder));

    public static TBuilder AppendInterface<TBuilder, TConfigParams, TBuilderParams>(
        this TBuilder builder,
        string interfaceName,
        TConfigParams interfaceConfigurationParams,
        Action<IInterfaceConfguration, TConfigParams> interfaceConfiguration,
        TBuilderParams interfaceBuilderParams,
        Action<IInterfaceBuilder, TBuilderParams> interfaceBuilderAction)
        where TBuilder : INamespaceMemberDeclarationBuilder
    {
        using (builder.AppendInterface(interfaceName, out var interfaceBuilder, interfaceConfigurationParams, interfaceConfiguration))
            interfaceBuilderAction?.Invoke(interfaceBuilder, interfaceBuilderParams);
        return builder;
    }

    public static TBuilder AppendInterface<TBuilder, TBuilderParams>(
        this TBuilder builder,
        string interfaceName,
        Action<IInterfaceConfguration> interfaceConfiguration,
        TBuilderParams interfaceBuilderParams,
        Action<IInterfaceBuilder, TBuilderParams> interfaceBuilderAction)
        where TBuilder : INamespaceMemberDeclarationBuilder
    {
        using (builder.AppendInterface(interfaceName, out var interfaceBuilder, interfaceConfiguration, static (builder, config) => config?.Invoke(builder)))
            interfaceBuilderAction?.Invoke(interfaceBuilder, interfaceBuilderParams);
        return builder;
    }

    public static TBuilder AppendInterface<TBuilder, TConfigParams>(
        this TBuilder builder,
        string interfaceName,
        TConfigParams interfaceConfigurationParams,
        Action<IInterfaceConfguration, TConfigParams> interfaceConfiguration,
        Action<IInterfaceBuilder> interfaceBuilderAction)
        where TBuilder : INamespaceMemberDeclarationBuilder
    {
        using (builder.AppendInterface(interfaceName, out var interfaceBuilder, interfaceConfigurationParams, interfaceConfiguration))
            interfaceBuilderAction?.Invoke(interfaceBuilder);
        return builder;
    }

    public static TBuilder AppendInterface<TBuilder>(
        this TBuilder builder,
        string interfaceName,
        Action<IInterfaceConfguration> interfaceConfiguration,
        Action<IInterfaceBuilder> interfaceBuilderAction)
        where TBuilder : INamespaceMemberDeclarationBuilder
    {
        using (builder.AppendInterface(interfaceName, out var interfaceBuilder, interfaceConfiguration, static (builder, config) => config?.Invoke(builder)))
            interfaceBuilderAction?.Invoke(interfaceBuilder);
        return builder;
    }

    public static SourceBuilderCodeBlock AppendStruct<TBuilder>(this TBuilder builder, string structName, out IStructBuilder structBuilder, Action<IStructConfiguration> structConfiguration)
        where TBuilder : INamespaceMemberDeclarationBuilder
        => builder.AppendStruct(structName, out structBuilder, structConfiguration, static (builder, config) => config?.Invoke(builder));

    public static TBuilder AppendStruct<TBuilder, TConfigParams, TBuilderParams>(
        this TBuilder builder,
        string structName,
        TConfigParams structConfigurationParams,
        Action<IStructConfiguration, TConfigParams> structConfiguration,
        TBuilderParams structBuilderParams,
        Action<IStructBuilder, TBuilderParams> structBuilderAction)
        where TBuilder : INamespaceMemberDeclarationBuilder
    {
        using (builder.AppendStruct(structName, out var structBuilder, structConfigurationParams, structConfiguration))
            structBuilderAction?.Invoke(structBuilder, structBuilderParams);
        return builder;
    }

    public static TBuilder AppendStruct<TBuilder, TBuilderParams>(
        this TBuilder builder,
        string structName,
        Action<IStructConfiguration> structConfiguration,
        TBuilderParams structBuilderParams,
        Action<IStructBuilder, TBuilderParams> structBuilderAction)
        where TBuilder : INamespaceMemberDeclarationBuilder
    {
        using (builder.AppendStruct(structName, out var structBuilder, structConfiguration, static (builder, config) => config?.Invoke(builder)))
            structBuilderAction?.Invoke(structBuilder, structBuilderParams);
        return builder;
    }

    public static TBuilder AppendStruct<TBuilder, TConfigParams>(
        this TBuilder builder,
        string structName,
        TConfigParams structConfigurationParams,
        Action<IStructConfiguration, TConfigParams> structConfiguration,
        Action<IStructBuilder> structBuilderAction)
        where TBuilder : INamespaceMemberDeclarationBuilder
    {
        using (builder.AppendStruct(structName, out var structBuilder, structConfigurationParams, structConfiguration))
            structBuilderAction?.Invoke(structBuilder);
        return builder;
    }

    public static TBuilder AppendStruct<TBuilder>(
        this TBuilder builder,
        string structName,
        Action<IStructConfiguration> structConfiguration,
        Action<IStructBuilder> structBuilderAction)
        where TBuilder : INamespaceMemberDeclarationBuilder
    {
        using (builder.AppendStruct(structName, out var structBuilder, structConfiguration, static (builder, config) => config?.Invoke(builder)))
            structBuilderAction?.Invoke(structBuilder);
        return builder;
    }

    public static SourceBuilderCodeBlock AppendEnum<TBuilder>(this TBuilder builder, string enumName, out IEnumBuilder enumBuilder, Action<IEnumConfiguration> enumConfiguration)
        where TBuilder : INamespaceMemberDeclarationBuilder
        => builder.AppendEnum(enumName, out enumBuilder, enumConfiguration, static (builder, config) => config?.Invoke(builder));

    public static TBuilder AppendEnum<TBuilder, TConfigParams, TBuilderParams>(
        this TBuilder builder,
        string enumName,
        TConfigParams enumConfigurationParams,
        Action<IEnumConfiguration, TConfigParams> enumConfiguration,
        TBuilderParams enumBuilderParams,
        Action<IEnumBuilder, TBuilderParams> enumBuilderAction)
        where TBuilder : INamespaceMemberDeclarationBuilder
    {
        using (builder.AppendEnum(enumName, out var enumBuilder, enumConfigurationParams, enumConfiguration))
            enumBuilderAction?.Invoke(enumBuilder, enumBuilderParams);
        return builder;
    }

    public static TBuilder AppendEnum<TBuilder, TBuilderParams>(
        this TBuilder builder,
        string enumName,
        Action<IEnumConfiguration> enumConfiguration,
        TBuilderParams enumBuilderParams,
        Action<IEnumBuilder, TBuilderParams> enumBuilderAction)
        where TBuilder : INamespaceMemberDeclarationBuilder
    {
        using (builder.AppendEnum(enumName, out var enumBuilder, enumConfiguration, static (builder, config) => config?.Invoke(builder)))
            enumBuilderAction?.Invoke(enumBuilder, enumBuilderParams);
        return builder;
    }

    public static TBuilder AppendEnum<TBuilder, TConfigParams>(
        this TBuilder builder,
        string enumName,
        TConfigParams enumConfigurationParams,
        Action<IEnumConfiguration, TConfigParams> enumConfiguration,
        Action<IEnumBuilder> enumBuilderAction)
        where TBuilder : INamespaceMemberDeclarationBuilder
    {
        using (builder.AppendEnum(enumName, out var enumBuilder, enumConfigurationParams, enumConfiguration))
            enumBuilderAction?.Invoke(enumBuilder);
        return builder;
    }

    public static TBuilder AppendEnum<TBuilder>(
        this TBuilder builder,
        string enumName,
        Action<IEnumConfiguration> enumConfiguration,
        Action<IEnumBuilder> enumBuilderAction)
        where TBuilder : INamespaceMemberDeclarationBuilder
    {
        using (builder.AppendEnum(enumName, out var enumBuilder, enumConfiguration, static (builder, config) => config?.Invoke(builder)))
            enumBuilderAction?.Invoke(enumBuilder);
        return builder;
    }

    public static TBuilder AppendDelegate<TBuilder>(this TBuilder builder, string delegateName, Action<IDelegateConfiguration> delegateConfiguration)
        where TBuilder : INamespaceMemberDeclarationBuilder
    {
        builder.AppendDelegate(delegateName, delegateConfiguration, static (builder, config) => config?.Invoke(builder));
        return builder;
    }
}
