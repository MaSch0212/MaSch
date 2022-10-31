namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IGenericParameterConfiguration
{
    /// <summary>
    /// Gets a value indicating whether this <see cref="GenericParameterConfiguration"/> has any constraints defined.
    /// </summary>
    bool HasConstraints { get; }

    IGenericParameterConfiguration WithVariance(GenericParameterVariance variance);
    IGenericParameterConfiguration WithConstraint(string constraint);
    void WriteParameterTo(ISourceBuilder sourceBuilder);
    void WriteConstraintTo(ISourceBuilder sourceBuilder);
}

public sealed class GenericParameterConfiguration : IGenericParameterConfiguration
{
    private readonly string _name;
    private readonly List<string> _constraints = new();
    private GenericParameterVariance _variance = GenericParameterVariance.None;

    public GenericParameterConfiguration(string name)
    {
        _name = name;
    }

    /// <inheritdoc/>
    public bool HasConstraints => _constraints.Count > 0;

    /// <inheritdoc/>
    public IGenericParameterConfiguration WithVariance(GenericParameterVariance variance)
    {
        _variance = variance;
        return this;
    }

    /// <inheritdoc/>
    public IGenericParameterConfiguration WithConstraint(string constraint)
    {
        _constraints.Add(constraint);
        return this;
    }

    /// <inheritdoc/>
    public void WriteParameterTo(ISourceBuilder sourceBuilder)
    {
        if (_variance is not GenericParameterVariance.None)
            sourceBuilder.Append(_variance.ToParameterPrefix());
        sourceBuilder.Append(_name);
    }

    /// <inheritdoc/>
    public void WriteConstraintTo(ISourceBuilder sourceBuilder)
    {
        if (_constraints.Count == 0)
            return;

        sourceBuilder.Append($"where {_name} : ");

        bool isFirst = true;
        foreach (var constraint in _constraints)
        {
            if (!isFirst)
                sourceBuilder.Append(", ");
            sourceBuilder.Append(constraint);

            isFirst = false;
        }
    }
}

public static class GenericParameterConfigurationExtensions
{
    public static IGenericParameterConfiguration AsCovariant(this IGenericParameterConfiguration config)
        => config.WithVariance(GenericParameterVariance.Covariant);

    public static IGenericParameterConfiguration AsContravariant(this IGenericParameterConfiguration config)
        => config.WithVariance(GenericParameterVariance.Contravariant);
}