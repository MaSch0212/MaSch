using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace MaSch.Generators.Common
{
    public static class CodeGenerationHelpers
    {
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
            SymbolDisplayMiscellaneousOptions.UseSpecialTypes
        );
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
            SymbolDisplayMiscellaneousOptions.UseSpecialTypes
        );

        public static IEnumerable<INamedTypeSymbol> GetNamespaceTypes(this INamespaceSymbol sym)
        {
            foreach (var child in sym.GetTypeMembers())
            {
                yield return child;
            }

            foreach (var ns in sym.GetNamespaceMembers())
            {
                foreach (var child2 in GetNamespaceTypes(ns))
                {
                    yield return child2;
                }
            }
        }

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

        public static string CreateHintName(this ITypeSymbol symbol)
            => Regex.Replace(symbol.ToDisplayString(), @"[\.+]", "_");

        public static void AddSource(this GeneratorExecutionContext context, ITypeSymbol forType, SourceBuilder builder)
            => context.AddSource(forType.CreateHintName(), SourceText.From(builder.ToString(), Encoding.UTF8));

        public static void LaunchDebuggerOnBuild()
        {
            var processName = Process.GetCurrentProcess().ProcessName;
            if (!Debugger.IsAttached && processName != "ServiceHub.RoslynCodeAnalysisService" && processName != "devenv")
                Debugger.Launch();
        }
    }
}
