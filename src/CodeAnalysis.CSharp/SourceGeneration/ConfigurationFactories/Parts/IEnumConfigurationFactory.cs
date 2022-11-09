using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface IEnumConfigurationFactory
{
    IEnumConfiguration Enum(string name);
}

partial class CodeConfigurationFactory : IEnumConfigurationFactory
{
    public IEnumConfiguration Enum(string name)
    {
        _lastTypeName = name;
        return new EnumConfiguration(name);
    }
}
