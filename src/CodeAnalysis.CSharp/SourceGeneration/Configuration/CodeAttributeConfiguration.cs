namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface ICodeAttributeConfiguration : ICodeConfiguration
{
    ICodeAttributeConfiguration OnTarget(CodeAttributeTarget target);
    ICodeAttributeConfiguration WithParameter(string value);
}

/// <summary>
/// Represents a C# code attribute.
/// </summary>
public sealed class CodeAttributeConfiguration : CodeConfiguration, ICodeAttributeConfiguration
{
    private readonly string _typeName;
    private readonly List<string> _parameters = new();
    private CodeAttributeTarget _target;

    /// <summary>
    /// Initializes a new instance of the <see cref="CodeAttributeConfiguration"/> class.
    /// </summary>
    /// <param name="typeName">The type name of the code attribute.</param>
    public CodeAttributeConfiguration(string typeName)
    {
        _typeName = typeName;
    }

    /// <inheritdoc/>
    protected override int StartCapacity => 128;

    /// <inheritdoc/>
    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        sourceBuilder.Append('[');

        if (_target is not CodeAttributeTarget.Default)
            sourceBuilder.Append(_target.ToAttributePrefix());

        sourceBuilder.Append(_typeName);

        if (_parameters.Count > 0)
        {
            sourceBuilder.Append('(');
            bool isFirst = true;
            foreach (var p in _parameters)
            {
                if (!isFirst)
                    sourceBuilder.Append(", ");
                sourceBuilder.Append(p);
                isFirst = false;
            }

            sourceBuilder.Append(')');
        }

        sourceBuilder.Append(']');
    }

    /// <inheritdoc/>
    public ICodeAttributeConfiguration OnTarget(CodeAttributeTarget target)
    {
        _target = target;
        return this;
    }

    /// <inheritdoc/>
    public ICodeAttributeConfiguration WithParameter(string value)
    {
        _parameters.Add(value);
        return this;
    }
}

/// <summary>
/// Provides extension methods for the <see cref="ICodeAttributeConfiguration"/> interface.
/// </summary>
public static class CodeAttributeConfigurationExtensions
{
    public static ICodeAttributeConfiguration OnAssembly(this ICodeAttributeConfiguration builder)
        => builder.OnTarget(CodeAttributeTarget.Assembly);

    public static ICodeAttributeConfiguration OnModule(this ICodeAttributeConfiguration builder)
        => builder.OnTarget(CodeAttributeTarget.Module);

    public static ICodeAttributeConfiguration OnField(this ICodeAttributeConfiguration builder)
        => builder.OnTarget(CodeAttributeTarget.Field);

    public static ICodeAttributeConfiguration OnEvent(this ICodeAttributeConfiguration builder)
        => builder.OnTarget(CodeAttributeTarget.Event);

    public static ICodeAttributeConfiguration OnMethod(this ICodeAttributeConfiguration builder)
        => builder.OnTarget(CodeAttributeTarget.Method);

    public static ICodeAttributeConfiguration OnParameter(this ICodeAttributeConfiguration builder)
        => builder.OnTarget(CodeAttributeTarget.Parameter);

    public static ICodeAttributeConfiguration OnProperty(this ICodeAttributeConfiguration builder)
        => builder.OnTarget(CodeAttributeTarget.Property);

    public static ICodeAttributeConfiguration OnReturn(this ICodeAttributeConfiguration builder)
        => builder.OnTarget(CodeAttributeTarget.Return);

    public static ICodeAttributeConfiguration OnType(this ICodeAttributeConfiguration builder)
        => builder.OnTarget(CodeAttributeTarget.Type);

    public static ICodeAttributeConfiguration WithParameters(this ICodeAttributeConfiguration builder, string param1)
        => builder.WithParameter(param1);

    public static ICodeAttributeConfiguration WithParameters(this ICodeAttributeConfiguration builder, string param1, string param2)
        => builder.WithParameter(param1).WithParameter(param2);

    public static ICodeAttributeConfiguration WithParameters(this ICodeAttributeConfiguration builder, string param1, string param2, string param3)
        => builder.WithParameter(param1).WithParameter(param2).WithParameter(param3);

    public static ICodeAttributeConfiguration WithParameters(this ICodeAttributeConfiguration builder, string param1, string param2, string param3, string param4)
        => builder.WithParameter(param1).WithParameter(param2).WithParameter(param3).WithParameter(param4);

    public static ICodeAttributeConfiguration WithParameters(this ICodeAttributeConfiguration builder, params string[] @params)
    {
        foreach (var p in @params)
            builder = builder.WithParameter(p);
        return builder;
    }
}