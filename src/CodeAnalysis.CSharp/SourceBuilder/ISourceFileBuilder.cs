using System;

namespace MaSch.Generators.Support
{
    public interface ISourceFileBuilder :
        ISourceBuilder<ISourceFileBuilder>,
        ISourceNamespaceImportBuilder<ISourceFileBuilder>,
        ISourceNamespaceDeclarationBuilder<ISourceFileBuilder>
    {
        ISourceFileBuilder AppendFileNamespace(string @namespace);
        ISourceFileBuilder AppendAssemblyCodeAttribute<TParams>(string attributeTypeName, TParams @params, Action<ISourceCodeAttributeBuilder, TParams> attributeConfiguration);
    }

    /// <summary>
    /// Provides extension methods for the <see cref="ISourceFileBuilder"/> interface.
    /// </summary>
    public static class SourceFileBuilderExtensions
    {
        public static ISourceFileBuilder AppendAssemblyCodeAttribute(this ISourceFileBuilder builder, string attributeTypeName)
            => builder.AppendAssemblyCodeAttribute<object?>(attributeTypeName, null, static (_, _) => { });

        public static ISourceFileBuilder AppendAssemblyCodeAttribute(this ISourceFileBuilder builder, string attributeTypeName, Action<ISourceCodeAttributeBuilder> attributeConfiguration)
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
}