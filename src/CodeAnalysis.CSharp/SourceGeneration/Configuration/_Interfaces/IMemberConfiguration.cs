namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IMemberConfiguration : ISupportsCodeAttributeConfiguration, ISupportsAccessModifierConfiguration, ISupportsLineCommentsConfiguration
{
    string MemberName { get; }

    IMemberConfiguration WithKeyword(MemberKeyword keyword);
}

public interface IMemberConfiguration<T> : IMemberConfiguration, ISupportsCodeAttributeConfiguration<T>, ISupportsAccessModifierConfiguration<T>, ISupportsLineCommentsConfiguration<T>
    where T : IMemberConfiguration<T>
{
    new T WithKeyword(MemberKeyword keyword);
}