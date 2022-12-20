namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IEventConfiguration : IMemberConfiguration<IEventConfiguration>
{
    IEventMethodConfiguration AddMethod { get; }
    IEventMethodConfiguration RemoveMethod { get; }

    IEventConfiguration ConfigureAdd(Action<IEventMethodConfiguration> configurationFunc);
    IEventConfiguration ConfigureRemove(Action<IEventMethodConfiguration> configurationFunc);
}

internal sealed class EventConfiguration : MemberConfiguration<IEventConfiguration>, IEventConfiguration
{
    private readonly string _eventType;

    public EventConfiguration(string eventType, string eventName)
        : base(eventName)
    {
        AddMethod = new EventMethodConfiguration("add");
        RemoveMethod = new EventMethodConfiguration("remove");
        _eventType = eventType;
    }

    public IEventMethodConfiguration AddMethod { get; }
    public IEventMethodConfiguration RemoveMethod { get; }

    protected override IEventConfiguration This => this;
    protected override int StartCapacity => 32;

    public IEventConfiguration ConfigureAdd(Action<IEventMethodConfiguration> configurationFunc)
    {
        configurationFunc(AddMethod);
        return this;
    }

    public IEventConfiguration ConfigureRemove(Action<IEventMethodConfiguration> configurationFunc)
    {
        configurationFunc(RemoveMethod);
        return this;
    }

    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        WriteCommentsTo(sourceBuilder);
        WriteCodeAttributesTo(sourceBuilder);
        WriteKeywordsTo(sourceBuilder);
        sourceBuilder.Append("event ").Append(_eventType).Append(' ');
        WriteNameTo(sourceBuilder);
    }
}
