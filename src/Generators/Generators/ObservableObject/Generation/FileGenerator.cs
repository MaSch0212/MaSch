using MaSch.Generators.Generators.ObservableObject.Models;
using MaSch.Generators.Support;

namespace MaSch.Generators.Generators.ObservableObject.Generation;

[FileGenerator(typeof(ObservableObjectGeneratorContext))]
internal readonly partial struct FileGenerator
{
    public void ObservableObject(GeneratorData data)
    {
        var sourceBuilder = new SourceBuilder();

        _context.AddSource(sourceBuilder.ToSourceText(), data.TypeSymbol);
    }
}
