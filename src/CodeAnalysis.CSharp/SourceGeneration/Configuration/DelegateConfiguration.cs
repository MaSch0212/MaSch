namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface IDelegateConfiguration : IMethodConfiguration<IDelegateConfiguration>
{
}

public sealed class DelegateConfiguration : MemberConfiguration<IDelegateConfiguration>, IDelegateConfiguration
{
    private readonly string _delegateName;
    private readonly string _returnType;

    public DelegateConfiguration(string delegateName)
    {
        _delegateName = delegateName;
        _returnType = "void";
    }

    protected override int StartCapacity => 128;
}
