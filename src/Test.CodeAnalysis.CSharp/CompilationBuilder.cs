using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;

#pragma warning disable SA1649 // File name should match first type name

namespace MaSch.Test.CodeAnalysis.CSharp;

/// <summary>
/// Represents a builder that is used to add generators, analyzers and additional texts to a compilation.
/// </summary>
/// <typeparam name="TBuilder">The type of build to return for each method.</typeparam>
public interface ICompilationBuilder<TBuilder>
    where TBuilder : ICompilationBuilder<TBuilder>
{
    /// <summary>
    /// Adds a generator to the compilation.
    /// </summary>
    /// <param name="generator">The generator to add.</param>
    /// <returns>A reference to this builder.</returns>
    TBuilder WithGenerator(ISourceGenerator generator);

    /// <summary>
    /// Adds a generator to the compilation.
    /// </summary>
    /// <param name="generator">The generator to add.</param>
    /// <returns>A reference to this builder.</returns>
    TBuilder WithGenerator(IIncrementalGenerator generator);

    /// <summary>
    /// Adds generators to the compilation.
    /// </summary>
    /// <param name="generators">The generators to add.</param>
    /// <returns>A reference to this builder.</returns>
    TBuilder WithGenerators(params ISourceGenerator[] generators);

    /// <summary>
    /// Adds generators to the compilation.
    /// </summary>
    /// <param name="generators">The generators to add.</param>
    /// <returns>A reference to this builder.</returns>
    TBuilder WithGenerators(params IIncrementalGenerator[] generators);

    /// <summary>
    /// Adds an analyzer to the compilation.
    /// </summary>
    /// <param name="analyzer">The analyzer to add.</param>
    /// <returns>A reference to this builder.</returns>
    TBuilder WithAnalyzer(DiagnosticAnalyzer analyzer);

    /// <summary>
    /// Adds analyzers to the compilation.
    /// </summary>
    /// <param name="analyzers">The analyzers to add.</param>
    /// <returns>A reference to this builder.</returns>
    TBuilder WithAnalyzers(params DiagnosticAnalyzer[] analyzers);

    /// <summary>
    /// Adds an additional text to the compilation.
    /// </summary>
    /// <param name="additionalText">The additional text to add.</param>
    /// <returns>A reference to this builder.</returns>
    TBuilder WithAdditionalText(AdditionalText additionalText);

    /// <summary>
    /// Adds an additional text to the compilation.
    /// </summary>
    /// <param name="path">The path of the additional text.</param>
    /// <param name="content">The content of the additional text.</param>
    /// <returns>A reference to this builder.</returns>
    TBuilder WithAdditionalText(string path, string content);

    /// <summary>
    /// Adds additional texts to the compilation.
    /// </summary>
    /// <param name="additionalTexts">The additional texts to add.</param>
    /// <returns>A reference to this builder.</returns>
    TBuilder WithAdditionalTexts(params AdditionalText[] additionalTexts);

    /// <summary>
    /// Configures the C-Sharp parse options that are used for parsing the generated sources from the generators.
    /// </summary>
    /// <param name="configurationFunc">The function to configure the options.</param>
    /// <returns>A reference to this builder.</returns>
    TBuilder ConfigureGeneratorParseOptions(Action<CSharpParseOptions> configurationFunc);

    /// <summary>
    /// Builds the compilation.
    /// </summary>
    /// <returns>An object containing the result of the compilation.</returns>
    CompilationResult Build();
}

/// <summary>
/// Represents a builder that is used to create a new compilation and add generators, analyzers and additional texts.
/// </summary>
public interface ICreateCompilationBuilder : ICompilationBuilder<ICreateCompilationBuilder>
{
    /// <summary>
    /// Sets the assembly name of the compilation.
    /// </summary>
    /// <param name="name">The assembly name to use.</param>
    /// <returns>A reference to this builder.</returns>
    ICreateCompilationBuilder WithAssemblyName(string name);

    /// <summary>
    /// Adds a syntax tree to the compilation.
    /// </summary>
    /// <param name="syntaxTree">The syntax tree to add.</param>
    /// <returns>A reference to this builder.</returns>
    ICreateCompilationBuilder WithSource(SyntaxTree syntaxTree);

    /// <summary>
    /// Adds code to the compilation.
    /// </summary>
    /// <param name="code">The code to add.</param>
    /// <param name="configureOptions">Function that can be used to configure the parse options of the code.</param>
    /// <returns>A reference to this builder.</returns>
    ICreateCompilationBuilder WithSource(string code, Action<CSharpParseOptions>? configureOptions = null);

    /// <summary>
    /// Adds syntax trees to the compilation.
    /// </summary>
    /// <param name="syntaxTrees">The syntax trees to add.</param>
    /// <returns>A reference to this builder.</returns>
    ICreateCompilationBuilder WithSources(params SyntaxTree[] syntaxTrees);

    /// <summary>
    /// Adds a reference to the compilation.
    /// </summary>
    /// <param name="metadataReference">The metadata reference to add.</param>
    /// <returns>A reference to this builder.</returns>
    ICreateCompilationBuilder WithReference(MetadataReference metadataReference);

    /// <summary>
    /// Adds a reference to the compilation.
    /// </summary>
    /// <param name="assembly">The assembly to add as reference.</param>
    /// <returns>A reference to this builder.</returns>
    ICreateCompilationBuilder WithReference(Assembly assembly);

    /// <summary>
    /// Adds references to the compilation.
    /// </summary>
    /// <param name="metadataReferences">The metadata references to add.</param>
    /// <returns>A reference to this builder.</returns>
    ICreateCompilationBuilder WithReferences(params MetadataReference[] metadataReferences);

    /// <summary>
    /// Adds references to the compilation.
    /// </summary>
    /// <param name="assemblies">The assemblies to add as reference.</param>
    /// <returns>A reference to this builder.</returns>
    ICreateCompilationBuilder WithReferences(params Assembly[] assemblies);

    /// <summary>
    /// Adds all assemblies from an <see cref="AppDomain"/> as reference to the compilation.
    /// </summary>
    /// <param name="appDomain">The <see cref="AppDomain"/> used as source for the assemblies to add.</param>
    /// <param name="predicate">Function that can be used to filter the assemblies to add.</param>
    /// <returns>A reference to this builder.</returns>
    ICreateCompilationBuilder WithReferencesFromAppDomain(AppDomain appDomain, Func<Assembly, bool>? predicate = null);

    /// <summary>
    /// Configures the compilation.
    /// </summary>
    /// <param name="configurationFunc">Function used to configure the compilation.</param>
    /// <returns>A reference to this builder.</returns>
    ICreateCompilationBuilder Configure(Action<CSharpCompilationOptions> configurationFunc);
}

/// <summary>
/// Represents a builder that is used to add generators, analyzers and additional texts to an existing compilation.
/// </summary>
public interface IExtendCompilationBuilder : ICompilationBuilder<IExtendCompilationBuilder>
{
}

/// <summary>
/// Starting point for building a compilation using Microsoft.CodeAnalysis.CSharp.
/// </summary>
public static class CompilationBuilder
{
    /// <summary>
    /// Create a completely new compilation.
    /// </summary>
    /// <returns>An object that is used to configure and build the compilation.</returns>
    public static ICreateCompilationBuilder Create() => new CreateCompilationBuilder();

    /// <summary>
    /// Creates a builder for an existing compilation to add analyzers and generators.
    /// </summary>
    /// <param name="compilation">The compilation to extend.</param>
    /// <returns>An object that is used to add additional things to the compilation.</returns>
    public static IExtendCompilationBuilder Extend(Compilation compilation) => new ExtendCompilationBuilder(compilation);

    private abstract class CompilationBuilderBase<TBuilder> : ICompilationBuilder<TBuilder>
        where TBuilder : ICompilationBuilder<TBuilder>
    {
        private readonly CSharpParseOptions _generatorParseOptions = new(LanguageVersion.Latest);
        private readonly List<ISourceGenerator> _generators = new();
        private readonly ImmutableArray<DiagnosticAnalyzer>.Builder _analyzers = ImmutableArray.CreateBuilder<DiagnosticAnalyzer>();
        private readonly List<AdditionalText> _additionalTexts = new();

        protected abstract TBuilder This { get; }

        public CompilationResult Build()
        {
            var initialCompilation = CreateCompilation();
            var finalCompilation = initialCompilation;
            var diagnostics = ImmutableArray.CreateBuilder<Diagnostic>();
            var generatedSourceResults = ImmutableArray.CreateBuilder<GeneratedSourceResult>();

            if (_generators.Count > 0)
            {
                var driver = CSharpGeneratorDriver.Create(_generators, _additionalTexts, _generatorParseOptions)
                    .RunGeneratorsAndUpdateCompilation(initialCompilation, out finalCompilation, out var generatorDiagnostics);

                diagnostics.AddRange(generatorDiagnostics);
                foreach (var result in driver.GetRunResult().Results)
                    generatedSourceResults.AddRange(result.GeneratedSources);
            }

            if (_analyzers.Count > 0)
            {
                var analyzerDiagnostics = initialCompilation
                    .WithAnalyzers(_analyzers.ToImmutable())
                    .GetAnalyzerDiagnosticsAsync()
                    .Result;
                diagnostics.AddRange(analyzerDiagnostics);
            }

            diagnostics.AddRange(finalCompilation.GetDiagnostics());
            return new CompilationResult(initialCompilation, finalCompilation, diagnostics.ToImmutable(), generatedSourceResults.ToImmutable());
        }

        public TBuilder ConfigureGeneratorParseOptions(Action<CSharpParseOptions> configurationFunc)
        {
            configurationFunc?.Invoke(_generatorParseOptions);
            return This;
        }

        public TBuilder WithAdditionalText(AdditionalText additionalText)
        {
            _additionalTexts.Add(additionalText);
            return This;
        }

        public TBuilder WithAdditionalText(string path, string content)
        {
            _additionalTexts.Add(new TestAdditionalText(path, content));
            return This;
        }

        public TBuilder WithAdditionalTexts(params AdditionalText[] additionalTexts)
        {
            _additionalTexts.AddRange(additionalTexts);
            return This;
        }

        public TBuilder WithAnalyzer(DiagnosticAnalyzer analyzer)
        {
            _analyzers.Add(analyzer);
            return This;
        }

        public TBuilder WithAnalyzers(params DiagnosticAnalyzer[] analyzers)
        {
            _analyzers.AddRange(analyzers);
            return This;
        }

        public TBuilder WithGenerator(ISourceGenerator generator)
        {
            _generators.Add(generator);
            return This;
        }

        public TBuilder WithGenerator(IIncrementalGenerator generator)
        {
            _generators.Add(generator.AsSourceGenerator());
            return This;
        }

        public TBuilder WithGenerators(params ISourceGenerator[] generators)
        {
            _generators.AddRange(generators);
            return This;
        }

        public TBuilder WithGenerators(params IIncrementalGenerator[] generators)
        {
            _generators.AddRange(generators.Select(x => x.AsSourceGenerator()));
            return This;
        }

        protected abstract Compilation CreateCompilation();
    }

    private sealed class CreateCompilationBuilder : CompilationBuilderBase<ICreateCompilationBuilder>, ICreateCompilationBuilder
    {
        private static int _counter = 0;

        private readonly CSharpCompilationOptions _compilationOptions = new(OutputKind.DynamicallyLinkedLibrary);
        private readonly List<MetadataReference> _references = new();
        private readonly List<SyntaxTree> _syntaxTrees = new();
        private string _assemblyName = "MaSch.Compilation" + Interlocked.Increment(ref _counter);

        protected override ICreateCompilationBuilder This => this;

        public ICreateCompilationBuilder Configure(Action<CSharpCompilationOptions> configurationFunc)
        {
            configurationFunc?.Invoke(_compilationOptions);
            return this;
        }

        public ICreateCompilationBuilder WithAssemblyName(string name)
        {
            _assemblyName = name;
            return this;
        }

        public ICreateCompilationBuilder WithReference(Assembly assembly)
        {
            _references.Add(MetadataReference.CreateFromFile(assembly.Location));
            return this;
        }

        public ICreateCompilationBuilder WithReference(MetadataReference metadataReference)
        {
            _references.Add(metadataReference);
            return this;
        }

        public ICreateCompilationBuilder WithReferences(params MetadataReference[] metadataReferences)
        {
            _references.AddRange(metadataReferences);
            return this;
        }

        public ICreateCompilationBuilder WithReferences(params Assembly[] assemblies)
        {
            _references.AddRange(assemblies.Select(x => MetadataReference.CreateFromFile(x.Location)));
            return this;
        }

        public ICreateCompilationBuilder WithReferencesFromAppDomain(AppDomain appDomain, Func<Assembly, bool>? predicate = null)
        {
            var refs = from a in appDomain.GetAssemblies()
                       where !a.IsDynamic && !string.IsNullOrWhiteSpace(a.Location) && predicate?.Invoke(a) != false
                       select MetadataReference.CreateFromFile(a.Location);
            _references.AddRange(refs);
            return this;
        }

        public ICreateCompilationBuilder WithSource(string code, Action<CSharpParseOptions>? configureOptions = null)
        {
            var options = new CSharpParseOptions(LanguageVersion.Latest);
            configureOptions?.Invoke(options);
            var syntaxTree = CSharpSyntaxTree.ParseText(code, options);
            _syntaxTrees.Add(syntaxTree);
            return this;
        }

        public ICreateCompilationBuilder WithSource(SyntaxTree syntaxTree)
        {
            _syntaxTrees.Add(syntaxTree);
            return this;
        }

        public ICreateCompilationBuilder WithSources(params SyntaxTree[] syntaxTrees)
        {
            _syntaxTrees.AddRange(syntaxTrees);
            return this;
        }

        protected override Compilation CreateCompilation()
        {
            return CSharpCompilation.Create(_assemblyName, _syntaxTrees, _references, _compilationOptions);
        }
    }

    private sealed class ExtendCompilationBuilder : CompilationBuilderBase<IExtendCompilationBuilder>, IExtendCompilationBuilder
    {
        private readonly Compilation _compilation;

        public ExtendCompilationBuilder(Compilation compilation)
        {
            _compilation = compilation;
        }

        protected override IExtendCompilationBuilder This => this;

        protected override Compilation CreateCompilation() => _compilation;
    }

    private sealed class TestAdditionalText : AdditionalText
    {
        private readonly SourceText _content;

        public TestAdditionalText(string path, string content)
        {
            Path = path;
            _content = SourceText.From(content, Encoding.UTF8);
        }

        public override string Path { get; }

        public override SourceText GetText(CancellationToken cancellationToken = default) => _content;
    }
}