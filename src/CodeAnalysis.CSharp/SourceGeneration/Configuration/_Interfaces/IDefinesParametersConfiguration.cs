﻿namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IDefinesParametersConfiguration : ICodeConfiguration
{
    bool MultilineParameters { get; set; }

    IDefinesParametersConfiguration WithParameter(string type, string name, Action<IParameterConfiguration> parameterConfiguration);
}

public interface IDefinesParametersConfiguration<T> : IDefinesParametersConfiguration
    where T : IDefinesParametersConfiguration<T>
{
    new T WithParameter(string type, string name, Action<IParameterConfiguration> parameterConfiguration);
}