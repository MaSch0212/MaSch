namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IMemberConfiguration : ISupportsCodeAttributeConfiguration, ISupportsAccessModifierConfiguration
{
    string MemberName { get; }

    IMemberConfiguration WithKeyword(MemberKeyword keyword);
}

public interface IMemberConfiguration<T> : IMemberConfiguration, ISupportsCodeAttributeConfiguration<T>, ISupportsAccessModifierConfiguration<T>
    where T : IMemberConfiguration<T>
{
    new T WithKeyword(MemberKeyword keyword);
}