namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

internal abstract class TypeConfiguration<T> : GenericMemberConfiguration<T>, ITypeConfiguration<T>
    where T : ITypeConfiguration<T>
{
    private readonly List<string> _interfaceImplementations = new();
    private string? _baseType;

    protected TypeConfiguration(string typeName)
        : base(typeName)
    {
    }

    protected override int StartCapacity => 128;

    public T DerivesFrom(string typeName)
    {
        _baseType = typeName;
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
        if (_baseType is null && _interfaceImplementations.Count == 0)
            return;

        sourceBuilder.Append(" : ");

        var baseTypes = _baseType is null ? _interfaceImplementations : _interfaceImplementations.Prepend(_baseType);
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
