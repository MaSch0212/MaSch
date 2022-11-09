using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface ISourceFileBuilder :
    ISourceBuilder<ISourceFileBuilder>,
    INamespaceImportBuilder<ISourceFileBuilder>,
    INamespaceDeclarationBuilder<ISourceFileBuilder, IFileMemberFactory>
{
    ISourceFileBuilder AppendFileNamespace(string @namespace);
    ISourceFileBuilder AppendAssemblyCodeAttribute<TParams>(string attributeTypeName, TParams @params, Action<ICodeAttributeConfiguration, TParams> attributeConfiguration);
}

public partial class SourceBuilder : ISourceFileBuilder
{
    /// <inheritdoc/>
    ISourceFileBuilder ISourceBuilder<ISourceFileBuilder>.AppendLine() => AppendLine();

    /// <inheritdoc/>
    ISourceFileBuilder ISourceBuilder<ISourceFileBuilder>.AppendLine(string value) => AppendLine(value);

    /// <inheritdoc/>
    ISourceFileBuilder ISourceBuilder<ISourceFileBuilder>.EnsurePreviousLineEmpty() => EnsurePreviousLineEmpty();

    /// <inheritdoc/>
    ISourceFileBuilder ISourceBuilder<ISourceFileBuilder>.Append(string value) => Append(value);

    /// <inheritdoc/>
    ISourceFileBuilder ISourceBuilder<ISourceFileBuilder>.Append(char value) => Append(value);

    /// <inheritdoc/>
    ISourceFileBuilder INamespaceImportBuilder<ISourceFileBuilder>.AppendNamespaceImport(string @namespace) => AppendNamespaceImport(@namespace);

    /// <inheritdoc/>
    ISourceFileBuilder INamespaceImportBuilder<ISourceFileBuilder>.AppendNamespaceImport(string @namespace, string alias) => AppendNamespaceImport(@namespace, alias);

    /// <inheritdoc/>
    ISourceFileBuilder INamespaceImportBuilder<ISourceFileBuilder>.AppendStaticNamespaceImport(string @namespace) => AppendStaticNamespaceImport(@namespace);

    /// <inheritdoc/>
    ISourceFileBuilder INamespaceImportBuilder<ISourceFileBuilder>.AppendGlobalNamespaceImport(string @namespace) => AppendGlobalNamespaceImport(@namespace);

    /// <inheritdoc/>
    ISourceFileBuilder INamespaceImportBuilder<ISourceFileBuilder>.AppendGlobalNamespaceImport(string @namespace, string alias) => AppendGlobalNamespaceImport(@namespace, alias);

    /// <inheritdoc/>
    ISourceFileBuilder INamespaceImportBuilder<ISourceFileBuilder>.AppendGlobalStaticNamespaceImport(string @namespace) => AppendGlobalStaticNamespaceImport(@namespace);

    /// <inheritdoc/>
    ISourceFileBuilder ISourceFileBuilder.AppendFileNamespace(string @namespace) => AppendFileNamespace(@namespace);

    /// <inheritdoc/>
    ISourceFileBuilder ISourceFileBuilder.AppendAssemblyCodeAttribute<TParams>(string attributeTypeName, TParams @params, Action<ICodeAttributeConfiguration, TParams> attributeConfiguration) => AppendAssemblyCodeAttribute(attributeTypeName, @params, attributeConfiguration);

    ISourceFileBuilder INamespaceDeclarationBuilder<ISourceFileBuilder, IFileMemberFactory>.Append(Func<IFileMemberFactory, INamespaceConfiguration> createFunc) => Append(createFunc(CodeConfigurationFactory.Instance), null);

    ISourceFileBuilder INamespaceDeclarationBuilder<ISourceFileBuilder, IFileMemberFactory>.Append(Func<IFileMemberFactory, INamespaceConfiguration> createFunc, Action<INamespaceBuilder> builderFunc) => Append(createFunc(CodeConfigurationFactory.Instance), builderFunc);
}

/// <summary>
/// Provides extension methods for the <see cref="ISourceFileBuilder"/> interface.
/// </summary>
public static class SourceFileBuilderExtensions
{
    public static ISourceFileBuilder AppendAssemblyCodeAttribute(this ISourceFileBuilder builder, string attributeTypeName)
        => builder.AppendAssemblyCodeAttribute<object?>(attributeTypeName, null, static (_, _) => { });

    public static ISourceFileBuilder AppendAssemblyCodeAttribute(this ISourceFileBuilder builder, string attributeTypeName, Action<ICodeAttributeConfiguration> attributeConfiguration)
        => builder.AppendAssemblyCodeAttribute(attributeTypeName, attributeConfiguration, static (builder, action) => action(builder));

    public static ISourceFileBuilder AppendAssemblyCodeAttribute(this ISourceFileBuilder builder, string attributeTypeName, string param1)
        => builder.AppendAssemblyCodeAttribute(attributeTypeName, param1, static (b, p) => b.WithParameter(p));

    public static ISourceFileBuilder AppendAssemblyCodeAttribute(this ISourceFileBuilder builder, string attributeTypeName, string param1, string param2)
        => builder.AppendAssemblyCodeAttribute(attributeTypeName, (param1, param2), static (b, p) => b.WithParameters(p.param1, p.param2));

    public static ISourceFileBuilder AppendAssemblyCodeAttribute(this ISourceFileBuilder builder, string attributeTypeName, string param1, string param2, string param3)
        => builder.AppendAssemblyCodeAttribute(attributeTypeName, (param1, param2, param3), static (b, p) => b.WithParameters(p.param1, p.param2, p.param3));

    public static ISourceFileBuilder AppendAssemblyCodeAttribute(this ISourceFileBuilder builder, string attributeTypeName, string param1, string param2, string param3, string param4)
        => builder.AppendAssemblyCodeAttribute(attributeTypeName, (param1, param2, param3, param4), static (b, p) => b.WithParameters(p.param1, p.param2, p.param3, p.param4));

    public static ISourceFileBuilder AppendAssemblyCodeAttribute(this ISourceFileBuilder builder, string attributeTypeName, params string[] @params)
        => builder.AppendAssemblyCodeAttribute(attributeTypeName, @params, static (b, p) => b.WithParameters(p));
}