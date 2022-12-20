﻿namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IStaticConstructorConfiguration : ISupportsCodeAttributeConfiguration<IStaticConstructorConfiguration>
{
    MethodBodyType BodyType { get; }

    IStaticConstructorConfiguration AsExpression(bool placeInNewLine = true);
}

internal sealed class StaticConstructorConfiguration : CodeConfigurationBase, IStaticConstructorConfiguration
{
    private readonly string? _containingTypeName;
    private readonly List<ICodeAttributeConfiguration> _codeAttributes = new();

    public StaticConstructorConfiguration(string? containingTypeName)
    {
        _containingTypeName = containingTypeName;
    }

    public MethodBodyType BodyType { get; private set; } = MethodBodyType.Block;
    public bool HasAttributes => _codeAttributes.Count > 0;

    protected override int StartCapacity => 16;

    public IStaticConstructorConfiguration AsExpression(bool placeInNewLine = true)
    {
        BodyType = placeInNewLine ? MethodBodyType.ExpressionNewLine : MethodBodyType.Expression;
        return this;
    }

    public IStaticConstructorConfiguration WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration>? attributeConfiguration)
    {
        CodeAttributeConfiguration.AddCodeAttribute(_codeAttributes, attributeTypeName, attributeConfiguration);
        return this;
    }

    public IStaticConstructorConfiguration WithCodeAttribute(string attributeTypeName)
        => WithCodeAttribute(attributeTypeName, null);

    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        foreach (var codeAttribute in _codeAttributes)
        {
            codeAttribute.WriteTo(sourceBuilder);
            sourceBuilder.AppendLine();
        }

        sourceBuilder.Append("static ").Append(_containingTypeName ?? sourceBuilder.CurrentTypeName).Append("()");
    }

    ISupportsCodeAttributeConfiguration ISupportsCodeAttributeConfiguration.WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration> attributeConfiguration)
        => WithCodeAttribute(attributeTypeName, attributeConfiguration);

    ISupportsCodeAttributeConfiguration ISupportsCodeAttributeConfiguration.WithCodeAttribute(string attributeTypeName)
        => WithCodeAttribute(attributeTypeName, null);
}