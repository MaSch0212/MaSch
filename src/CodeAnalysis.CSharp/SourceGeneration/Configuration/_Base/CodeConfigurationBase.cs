﻿namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

internal abstract class CodeConfigurationBase : ICodeConfiguration
{
    protected abstract int StartCapacity { get; }

    public abstract void WriteTo(ISourceBuilder sourceBuilder);

    public override string ToString()
    {
        var builder = CreateSourceBuilderForToString();
        WriteTo(builder);
        return builder.ToString();
    }

    protected virtual ISourceBuilder CreateSourceBuilderForToString()
    {
        var options = new SourceBuilderOptions
        {
            Capacity = StartCapacity,
            IncludeFileHeader = false,
        };
        var builder = SourceBuilder.Create(options);
        builder.CurrentTypeName = "[ClassName]";
        return builder;
    }
}
