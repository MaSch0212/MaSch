using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace MaSch.Generators.Common
{
    /// <summary>
    /// Contains helper methods and values for C# 9 Source Generators.
    /// </summary>
    public static class CodeGenerationHelpers
    {
        /// <summary>
        /// Format that can be used to get the definition syntax of a Symbol.
        /// </summary>
        public static readonly SymbolDisplayFormat DefinitionFormat = new SymbolDisplayFormat(
            SymbolDisplayGlobalNamespaceStyle.Omitted,
            SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
            SymbolDisplayGenericsOptions.IncludeTypeParameters | SymbolDisplayGenericsOptions.IncludeTypeConstraints | SymbolDisplayGenericsOptions.IncludeVariance,
            SymbolDisplayMemberOptions.IncludeType | SymbolDisplayMemberOptions.IncludeParameters | SymbolDisplayMemberOptions.IncludeRef,
            SymbolDisplayDelegateStyle.NameAndSignature,
            SymbolDisplayExtensionMethodStyle.Default,
            SymbolDisplayParameterOptions.IncludeParamsRefOut | SymbolDisplayParameterOptions.IncludeType | SymbolDisplayParameterOptions.IncludeName | SymbolDisplayParameterOptions.IncludeDefaultValue,
            SymbolDisplayPropertyStyle.NameOnly,
            SymbolDisplayLocalOptions.None,
            SymbolDisplayKindOptions.IncludeMemberKeyword,
            SymbolDisplayMiscellaneousOptions.UseSpecialTypes);

        /// <summary>
        /// Format that can be used to get the usage syntax of a Symbol.
        /// </summary>
        public static readonly SymbolDisplayFormat UsageFormat = new SymbolDisplayFormat(
            SymbolDisplayGlobalNamespaceStyle.Omitted,
            SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
            SymbolDisplayGenericsOptions.IncludeTypeParameters,
            SymbolDisplayMemberOptions.IncludeParameters,
            SymbolDisplayDelegateStyle.NameAndSignature,
            SymbolDisplayExtensionMethodStyle.Default,
            SymbolDisplayParameterOptions.IncludeParamsRefOut | SymbolDisplayParameterOptions.IncludeName,
            SymbolDisplayPropertyStyle.NameOnly,
            SymbolDisplayLocalOptions.None,
            SymbolDisplayKindOptions.None,
            SymbolDisplayMiscellaneousOptions.UseSpecialTypes);

        /// <summary>
        /// Determines all types that are defined inside a given namespace (includes descendant namespaces).
        /// </summary>
        /// <param name="symbol">The namespace to search in.</param>
        /// <returns>Returns an enumerable that enumerates through all types inside the given namespace <paramref name="symbol"/>.</returns>
        public static IEnumerable<INamedTypeSymbol> GetNamespaceTypes(this INamespaceSymbol symbol)
        {
            foreach (var child in symbol.GetTypeMembers())
            {
                yield return child;
            }

            foreach (var ns in symbol.GetNamespaceMembers())
            {
                foreach (var child2 in GetNamespaceTypes(ns))
                {
                    yield return child2;
                }
            }
        }

        /// <summary>
        /// Determines all attributes of a given symbol (includes base types).
        /// </summary>
        /// <param name="symbol">The symbol to search in.</param>
        /// <returns>Returns an enumerable that enumerates through all attributes defined for the symbol and its base types.</returns>
        public static IEnumerable<AttributeData> GetAllAttributes(this ISymbol? symbol)
        {
            while (symbol != null)
            {
                foreach (var attribute in symbol.GetAttributes())
                {
                    yield return attribute;
                }

                symbol = (symbol as INamedTypeSymbol)?.BaseType;
            }
        }

        /// <summary>
        /// Creates a name out of a <see cref="ITypeSymbol"/> that can be used for the generated file.
        /// </summary>
        /// <param name="symbol">The type symbol to use.</param>
        /// <returns>Returns a name that can be used for a generated file.</returns>
        public static string CreateHintName(this ITypeSymbol symbol)
            => Regex.Replace(symbol.ToDisplayString(), @"[\.+]", "_");

        /// <summary>
        /// Adds a <see cref="SourceBuilder"/> to the compilation.
        /// </summary>
        /// <param name="context">The generator execution context.</param>
        /// <param name="forType">The type for which the source was generated for.</param>
        /// <param name="builder">The builder that includes the generated source code.</param>
        public static void AddSource(this GeneratorExecutionContext context, ITypeSymbol forType, SourceBuilder builder)
            => context.AddSource(forType.CreateHintName(), SourceText.From(builder.ToString(), Encoding.UTF8));

        /// <summary>
        /// Launches the debugger if the generator was not executed from an IDE.
        /// </summary>
        public static void LaunchDebuggerOnBuild()
        {
            var processName = Process.GetCurrentProcess().ProcessName;
            if (!Debugger.IsAttached && processName != "ServiceHub.RoslynCodeAnalysisService" && processName != "devenv")
                Debugger.Launch();
        }
    }
}
