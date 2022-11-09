namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IGenericMemberConfiguration : IMemberConfiguration
{
    string MemberNameWithoutGenericParameters { get; }

    IGenericMemberConfiguration WithGenericParameter(string name, Action<IGenericParameterConfiguration> parameterConfiguration);
}

public interface IGenericMemberConfiguration<T> : IGenericMemberConfiguration, IMemberConfiguration<T>
    where T : IGenericMemberConfiguration<T>
{
    T WithGenericParameter(string name, Action<IGenericParameterConfiguration> parameterConfiguration);
}