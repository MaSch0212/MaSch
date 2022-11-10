using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

public interface IEventConfigurationFactory
{
    IEventConfiguration Event(string eventType, string eventName);
}

partial class CodeConfigurationFactory : IEventConfigurationFactory
{
    public IEventConfiguration Event(string eventType, string eventName)
    {
        return new EventConfiguration(eventType, eventName);
    }
}