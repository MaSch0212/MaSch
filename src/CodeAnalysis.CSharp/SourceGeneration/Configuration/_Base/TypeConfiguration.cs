namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

internal abstract class TypeConfiguration<T> : GenericMemberConfiguration<T>, ITypeConfiguration<T>
    where T : ITypeConfiguration<T>
{
    private readonly List<string> _interfaceImplementations = new();

    protected TypeConfiguration(string typeName)
        : base(typeName)
    {
    }

    public IReadOnlyList<string> InterfaceImplementations => new ReadOnlyCollection<string>(_interfaceImplementations);
    public string? BaseType { get; private set; }

    protected override int StartCapacity => 128;
    protected bool IsDerivingOrImplementingInterface => BaseType is not null && _interfaceImplementations.Count > 0;

    public T DerivesFrom(string typeName)
    {
        BaseType = typeName;
        return This;
    }

    ISupportsInheritanceConfiguration ISupportsInheritanceConfiguration.DerivesFrom(string typeName)
        => DerivesFrom(typeName);

    public T Implements(string interfaceTypeName)
    {
        _interfaceImplementations.Add(interfaceTypeName);
        return This;
    }

    ITypeConfiguration ITypeConfiguration.Implements(string interfaceTypeName)
        => Implements(interfaceTypeName);

    protected void WriteBaseTypesTo(ISourceBuilder sourceBuilder)
    {
        if (!IsDerivingOrImplementingInterface)
            return;

        sourceBuilder.Append(": ");

        var baseTypes = BaseType is null ? InterfaceImplementations : InterfaceImplementations.Prepend(BaseType);
        bool isFirst = true;
        foreach (var baseType in baseTypes)
        {
            if (!isFirst)
                sourceBuilder.Append(", ");
            sourceBuilder.Append(baseType);

            isFirst = false;
        }
    }
}
