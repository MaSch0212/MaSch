﻿namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IInterfaceConfguration : ITypeConfiguration<IInterfaceConfguration>
{
}

internal sealed class InterfaceConfguration : TypeConfiguration<IInterfaceConfguration>, IInterfaceConfguration
{
    public InterfaceConfguration(string interfaceName)
        : base(interfaceName)
    {
    }

    /// <inheritdoc/>
    protected override IInterfaceConfguration This => this;

    /// <inheritdoc/>
    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        WriteCodeAttributesTo(sourceBuilder);
        WriteKeywordsTo(sourceBuilder);
        sourceBuilder.Append("interface ");
        WriteNameTo(sourceBuilder);
        WriteBaseTypesTo(sourceBuilder);
        WriteGenericConstraintsTo(sourceBuilder);
    }
}