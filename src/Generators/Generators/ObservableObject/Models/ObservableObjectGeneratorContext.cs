using MaSch.Generators.Support;
using Microsoft.CodeAnalysis.Text;
using System;

namespace MaSch.Generators.Generators.ObservableObject.Models;

internal class ObservableObjectGeneratorContext : IAddSource
{
    private readonly Action<string, SourceText> _addSource;

    public ObservableObjectGeneratorContext(Action<string, SourceText> addSource)
    {
        _addSource = addSource;
    }

    public void AddSource(string hintName, SourceText sourceText) => _addSource(hintName, sourceText);
}
