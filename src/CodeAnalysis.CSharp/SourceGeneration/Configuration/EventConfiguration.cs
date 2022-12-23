namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

/// <summary>
/// Represents configuration of an event code element. This is used to generate code in the <see cref="ISourceBuilder"/>.
/// </summary>
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Interface")]
public interface IEventConfiguration : IMemberConfiguration<IEventConfiguration>
{
    /// <summary>
    /// Gets the delegate type of the event represented by this <see cref="IEventConfiguration"/>.
    /// </summary>
    string EventType { get; }

    /// <summary>
    /// Gets the configuration of the add method of the event represented by this <see cref="IEventConfiguration"/>.
    /// </summary>
    IEventMethodConfiguration AddMethod { get; }

    /// <summary>
    /// Gets the configuration of the remove method of the event represented by this <see cref="IEventConfiguration"/>.
    /// </summary>
    IEventMethodConfiguration RemoveMethod { get; }

    /// <summary>
    /// Configures the add method of the event represented by this <see cref="IEventConfiguration"/>.
    /// </summary>
    /// <param name="configurationFunc">A function to configure the add method.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    IEventConfiguration ConfigureAdd(Action<IEventMethodConfiguration> configurationFunc);

    /// <summary>
    /// Configures the remove method of the event represented by this <see cref="IEventConfiguration"/>.
    /// </summary>
    /// <param name="configurationFunc">A function to configure the remove method.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    IEventConfiguration ConfigureRemove(Action<IEventMethodConfiguration> configurationFunc);
}

internal sealed class EventConfiguration : MemberConfiguration<IEventConfiguration>, IEventConfiguration
{
    public EventConfiguration(string eventType, string eventName)
        : base(eventName)
    {
        AddMethod = new EventMethodConfiguration("add");
        RemoveMethod = new EventMethodConfiguration("remove");
        EventType = eventType;
    }

    public string EventType { get; }
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
        sourceBuilder.Append("event ").Append(EventType).Append(' ');
        WriteNameTo(sourceBuilder);
    }
}
