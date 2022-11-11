namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface ISuperConstructorConfiguration : ICodeConfiguration
{
    ISuperConstructorConfiguration WithParameter(string value);
}

internal sealed class SuperConstructorConfiguration : CodeConfiguration, ISuperConstructorConfiguration
{
    private readonly string _superConstructorKeyword;
    private readonly List<string> _parameterValues = new();

    public SuperConstructorConfiguration(string superConstructorKeyword)
    {
        _superConstructorKeyword = superConstructorKeyword;
    }

    protected override int StartCapacity => 16;

    public ISuperConstructorConfiguration WithParameter(string value)
    {
        _parameterValues.Add(value);
        return this;
    }

    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        sourceBuilder.Append(": ").Append(_superConstructorKeyword).Append('(');
        using (sourceBuilder.Indent())
        {
            for (int i = 0; i < _parameterValues.Count; i++)
            {
                sourceBuilder.Append(_parameterValues[i]);
                if (i < _parameterValues.Count - 1)
                    sourceBuilder.Append(", ");
            }
        }

        sourceBuilder.Append(')');
    }
}
