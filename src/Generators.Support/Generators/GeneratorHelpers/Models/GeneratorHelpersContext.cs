using MaSch.Generators.Support;
using Microsoft.CodeAnalysis.Text;
using System;

namespace MaSch.Generators.GeneratorHelpers.Models;

internal class GeneratorHelpersContext : IAddSource
{
    private readonly Action<string, SourceText> _addSource;

    public GeneratorHelpersContext(Action<string, SourceText> addSource)
    {
        _addSource = addSource;
    }

    public void AddSource(string hintName, SourceText sourceText) => _addSource(hintName, sourceText);
}
