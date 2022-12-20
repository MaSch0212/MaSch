using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Linq;

namespace MaSch.Generators.ObservableObject;

internal static class DiagnosticsFactory
{
    private const string Category = "ObservableObjectGenerator";

    private static readonly DiagnosticDescriptor AttributeIgnoredDescriptor = new(
        GetId(1),
        "Attribute will be ignored by the source generator",
        "The '{0}' attribute on type '{1}' will be ignored because it also has the '{2}' attribute.",
        Category,
        DiagnosticSeverity.Warning,
        true);

    public static Diagnostic AttributeIgnored(string typeName, ImmutableArray<AttributeData> attributes, string ignoredAttributeName, string otherAttributeName)
    {
        var attributeLocation = attributes.FirstOrDefault(x => x.AttributeClass.Name == ignoredAttributeName)?.ApplicationSyntaxReference.GetSyntax().GetLocation();
        return Diagnostic.Create(AttributeIgnoredDescriptor, attributeLocation, ignoredAttributeName, typeName, otherAttributeName);
    }

    private static string GetId(int id) => $"MSCG01{id}";
}
