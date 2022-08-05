using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Text.RegularExpressions;

#nullable enable

namespace MaSch.Generators.Support
{
    /// <summary>
    /// Provides extensions for context interfaces providing "AddSource" methods.
    /// </summary>
    public static class AddSourceExtensions
    {
        private static readonly char[] HexChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
        private static readonly Regex FileNameReplaceRegex = new(@"[\.+]", RegexOptions.Compiled);

        /// <summary>
        /// Adds a <see cref="SourceText"/> to the compilation.
        /// </summary>
        /// <param name="context">The context to add the <see cref="SourceText"/>.</param>
        /// <param name="sourceText">The <see cref="SourceText"/> to add.</param>
        /// <param name="fileNameWithoutExtension">The display name of the generated source.</param>
        /// <param name="purpose">The purpose of the source. Used to generate a different hash for the hint name. Useful when potentially generating multiple files with the same name.</param>
        public static void AddSource(this GeneratorExecutionContext context, SourceText sourceText, string fileNameWithoutExtension, string? purpose = null)
            => AddSource(context.AddSource, sourceText, null, purpose, fileNameWithoutExtension);

        /// <summary>
        /// Adds a <see cref="SourceText"/> to the compilation.
        /// </summary>
        /// <param name="context">The context to add the <see cref="SourceText"/>.</param>
        /// <param name="sourceText">The <see cref="SourceText"/> to add.</param>
        /// <param name="relatedSymbol">A <see cref="ISymbol"/> that is related to the generated code. Used to generate a different hash for the hint name and to automatically determine <paramref name="fileNameWithoutExtension"/>.</param>
        /// <param name="fileNameWithoutExtension">The display name of the generated source.</param>
        /// <param name="purpose">The purpose of the source. Used to generate a different hash for the hint name. Useful when potentially generating multiple files with the same name.</param>
        public static void AddSource(this GeneratorExecutionContext context, SourceText sourceText, ISymbol relatedSymbol, string? fileNameWithoutExtension = null, string? purpose = null)
            => AddSource(context.AddSource, sourceText, relatedSymbol, purpose, fileNameWithoutExtension);

        /// <summary>
        /// Adds a <see cref="SourceText"/> to the compilation.
        /// </summary>
        /// <param name="context">The context to add the <see cref="SourceText"/>.</param>
        /// <param name="sourceText">The <see cref="SourceText"/> to add.</param>
        /// <param name="fileNameWithoutExtension">The display name of the generated source.</param>
        /// <param name="purpose">The purpose of the source. Used to generate a different hash for the hint name. Useful when potentially generating multiple files with the same name.</param>
        public static void AddSource(this GeneratorPostInitializationContext context, SourceText sourceText, string fileNameWithoutExtension, string? purpose = null)
            => AddSource(context.AddSource, sourceText, null, purpose, fileNameWithoutExtension);

        /// <summary>
        /// Adds a <see cref="SourceText"/> to the compilation.
        /// </summary>
        /// <param name="context">The context to add the <see cref="SourceText"/>.</param>
        /// <param name="sourceText">The <see cref="SourceText"/> to add.</param>
        /// <param name="relatedSymbol">A <see cref="ISymbol"/> that is related to the generated code. Used to generate a different hash for the hint name and to automatically determine <paramref name="fileNameWithoutExtension"/>.</param>
        /// <param name="fileNameWithoutExtension">The display name of the generated source.</param>
        /// <param name="purpose">The purpose of the source. Used to generate a different hash for the hint name. Useful when potentially generating multiple files with the same name.</param>
        public static void AddSource(this GeneratorPostInitializationContext context, SourceText sourceText, ISymbol relatedSymbol, string? fileNameWithoutExtension = null, string? purpose = null)
            => AddSource(context.AddSource, sourceText, relatedSymbol, purpose, fileNameWithoutExtension);

        /// <summary>
        /// Adds a <see cref="SourceText"/> to the compilation.
        /// </summary>
        /// <param name="context">The context to add the <see cref="SourceText"/>.</param>
        /// <param name="sourceText">The <see cref="SourceText"/> to add.</param>
        /// <param name="fileNameWithoutExtension">The display name of the generated source.</param>
        /// <param name="purpose">The purpose of the source. Used to generate a different hash for the hint name. Useful when potentially generating multiple files with the same name.</param>
        public static void AddSource(this IncrementalGeneratorPostInitializationContext context, SourceText sourceText, string fileNameWithoutExtension, string? purpose = null)
            => AddSource(context.AddSource, sourceText, null, purpose, fileNameWithoutExtension);

        /// <summary>
        /// Adds a <see cref="SourceText"/> to the compilation.
        /// </summary>
        /// <param name="context">The context to add the <see cref="SourceText"/>.</param>
        /// <param name="sourceText">The <see cref="SourceText"/> to add.</param>
        /// <param name="relatedSymbol">A <see cref="ISymbol"/> that is related to the generated code. Used to generate a different hash for the hint name and to automatically determine <paramref name="fileNameWithoutExtension"/>.</param>
        /// <param name="fileNameWithoutExtension">The display name of the generated source.</param>
        /// <param name="purpose">The purpose of the source. Used to generate a different hash for the hint name. Useful when potentially generating multiple files with the same name.</param>
        public static void AddSource(this IncrementalGeneratorPostInitializationContext context, SourceText sourceText, ISymbol relatedSymbol, string? fileNameWithoutExtension = null, string? purpose = null)
            => AddSource(context.AddSource, sourceText, relatedSymbol, purpose, fileNameWithoutExtension);

        /// <summary>
        /// Adds a <see cref="SourceText"/> to the compilation.
        /// </summary>
        /// <param name="context">The context to add the <see cref="SourceText"/>.</param>
        /// <param name="sourceText">The <see cref="SourceText"/> to add.</param>
        /// <param name="fileNameWithoutExtension">The display name of the generated source.</param>
        /// <param name="purpose">The purpose of the source. Used to generate a different hash for the hint name. Useful when potentially generating multiple files with the same name.</param>
        public static void AddSource(this SourceProductionContext context, SourceText sourceText, string fileNameWithoutExtension, string? purpose = null)
            => AddSource(context.AddSource, sourceText, null, purpose, fileNameWithoutExtension);

        /// <summary>
        /// Adds a <see cref="SourceText"/> to the compilation.
        /// </summary>
        /// <param name="context">The context to add the <see cref="SourceText"/>.</param>
        /// <param name="sourceText">The <see cref="SourceText"/> to add.</param>
        /// <param name="relatedSymbol">A <see cref="ISymbol"/> that is related to the generated code. Used to generate a different hash for the hint name and to automatically determine <paramref name="fileNameWithoutExtension"/>.</param>
        /// <param name="fileNameWithoutExtension">The display name of the generated source.</param>
        /// <param name="purpose">The purpose of the source. Used to generate a different hash for the hint name. Useful when potentially generating multiple files with the same name.</param>
        public static void AddSource(this SourceProductionContext context, SourceText sourceText, ISymbol relatedSymbol, string? fileNameWithoutExtension = null, string? purpose = null)
            => AddSource(context.AddSource, sourceText, relatedSymbol, purpose, fileNameWithoutExtension);

        private static void AddSource(Action<string, SourceText> addSourceFunc, SourceText sourceText, ISymbol? relatedSymbol, string? purpose, string? fileNameWithoutExtension)
        {
            fileNameWithoutExtension ??= relatedSymbol is null ? "GeneratedSourceFile" : FileNameReplaceRegex.Replace(relatedSymbol.Name, "-");
            int hash = relatedSymbol switch
            {
                IAssemblySymbol assemblySymbol => (assemblySymbol.Name, purpose).GetHashCode(),
                null => (fileNameWithoutExtension, purpose).GetHashCode(),
                _ => (relatedSymbol.ToDisplayString(SymbolExtensions.DefinitionFormat.WithGenericsOptions(SymbolDisplayGenericsOptions.IncludeTypeParameters)), purpose).GetHashCode(),
            };

            addSourceFunc($"{fileNameWithoutExtension}-{GetHexString(hash)}.g.cs", sourceText);
        }

        private static string GetHexString(int hexCode)
        {
            var result = new char[8];
            uint hc = unchecked((uint)hexCode);
            for (int i = 0; i < 8; i++)
            {
                result[i] = HexChars[hc << (i * 4) >> 28];
            }

            return new string(result);
        }
    }
}