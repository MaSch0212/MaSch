using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

public interface IRecordConfgurationFactory
{
    IRecordConfiguration Record(string name);
}

partial class CodeConfigurationFactory : IRecordConfgurationFactory
{
    public IRecordConfiguration Record(string name)
    {
        _lastTypeName = name;
        return new RecordConfiguration(name);
    }
}
