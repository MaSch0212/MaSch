namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IGenericMemberConfiguration : IMemberConfiguration
{
    string MemberNameWithoutGenericParameters { get; }

    IGenericMemberConfiguration WithGenericParameter(string name);
    IGenericMemberConfiguration WithGenericParameter(string name, Action<IGenericParameterConfiguration> parameterConfiguration);
}

public interface IGenericMemberConfiguration<T> : IGenericMemberConfiguration, IMemberConfiguration<T>
    where T : IGenericMemberConfiguration<T>
{
    new T WithGenericParameter(string name);
    new T WithGenericParameter(string name, Action<IGenericParameterConfiguration> parameterConfiguration);
}