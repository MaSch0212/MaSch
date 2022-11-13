namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

internal abstract class TypeConfiguration<T> : GenericMemberConfiguration<T>, ITypeConfiguration<T>
    where T : ITypeConfiguration<T>
{
    protected TypeConfiguration(string typeName)
        : base(typeName)
    {
    }

    protected override int StartCapacity => 128;

    protected List<string> InterfaceImplementations { get; } = new();
    protected string? BaseType { get; set; }

    protected bool HasBaseTypes => BaseType is not null || InterfaceImplementations.Count > 0;

    public T DerivesFrom(string typeName)
    {
        BaseType = typeName;
        return This;
    }

    ISupportsInheritanceConfiguration ISupportsInheritanceConfiguration.DerivesFrom(string typeName)
        => DerivesFrom(typeName);

    public T Implements(string interfaceTypeName)
    {
        InterfaceImplementations.Add(interfaceTypeName);
        return This;
    }

    ITypeConfiguration ITypeConfiguration.Implements(string interfaceTypeName)
        => Implements(interfaceTypeName);

    protected void WriteBaseTypesTo(ISourceBuilder sourceBuilder)
    {
        if (!HasBaseTypes)
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
