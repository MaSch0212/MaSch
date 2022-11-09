using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface IEnumValueConfigurationFactory
{
    IEnumValueConfiguration Value(string name, string value);
}

partial class CodeConfigurationFactory : IEnumValueConfigurationFactory
{
    public IEnumValueConfiguration Value(string name, string value)
    {
        return new EnumValueConfiguration(name, value);
    }
}