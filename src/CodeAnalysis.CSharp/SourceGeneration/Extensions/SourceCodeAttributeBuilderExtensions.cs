namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

/// <summary>
/// Provides extension methods for the <see cref="ISourceCodeAttributeBuilder"/> interface.
/// </summary>
public static class SourceCodeAttributeBuilderExtensions
{
    public static ISourceCodeAttributeBuilder OnAssembly(this ISourceCodeAttributeBuilder builder)
        => builder.OnTarget(CodeAttributeTarget.Assembly);

    public static ISourceCodeAttributeBuilder OnModule(this ISourceCodeAttributeBuilder builder)
        => builder.OnTarget(CodeAttributeTarget.Module);

    public static ISourceCodeAttributeBuilder OnField(this ISourceCodeAttributeBuilder builder)
        => builder.OnTarget(CodeAttributeTarget.Field);

    public static ISourceCodeAttributeBuilder OnEvent(this ISourceCodeAttributeBuilder builder)
        => builder.OnTarget(CodeAttributeTarget.Event);

    public static ISourceCodeAttributeBuilder OnMethod(this ISourceCodeAttributeBuilder builder)
        => builder.OnTarget(CodeAttributeTarget.Method);

    public static ISourceCodeAttributeBuilder OnParameter(this ISourceCodeAttributeBuilder builder)
        => builder.OnTarget(CodeAttributeTarget.Parameter);

    public static ISourceCodeAttributeBuilder OnProperty(this ISourceCodeAttributeBuilder builder)
        => builder.OnTarget(CodeAttributeTarget.Property);

    public static ISourceCodeAttributeBuilder OnReturn(this ISourceCodeAttributeBuilder builder)
        => builder.OnTarget(CodeAttributeTarget.Return);

    public static ISourceCodeAttributeBuilder OnType(this ISourceCodeAttributeBuilder builder)
        => builder.OnTarget(CodeAttributeTarget.Type);

    public static ISourceCodeAttributeBuilder WithParameters(this ISourceCodeAttributeBuilder builder, string param1)
        => builder.WithParameter(param1);

    public static ISourceCodeAttributeBuilder WithParameters(this ISourceCodeAttributeBuilder builder, string param1, string param2)
        => builder.WithParameter(param1).WithParameter(param2);

    public static ISourceCodeAttributeBuilder WithParameters(this ISourceCodeAttributeBuilder builder, string param1, string param2, string param3)
        => builder.WithParameter(param1).WithParameter(param2).WithParameter(param3);

    public static ISourceCodeAttributeBuilder WithParameters(this ISourceCodeAttributeBuilder builder, string param1, string param2, string param3, string param4)
        => builder.WithParameter(param1).WithParameter(param2).WithParameter(param3).WithParameter(param4);

    public static ISourceCodeAttributeBuilder WithParameters(this ISourceCodeAttributeBuilder builder, params string[] @params)
    {
        foreach (var p in @params)
            builder = builder.WithParameter(p);
        return builder;
    }
}