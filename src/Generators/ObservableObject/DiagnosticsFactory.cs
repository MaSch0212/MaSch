using Microsoft.CodeAnalysis;

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

    public static Diagnostic AttributeIgnored(string typeName, string ignoredAttributeName, string otherAttributeName, Location ignoredAttributeLocation)
    {
        return Diagnostic.Create(AttributeIgnoredDescriptor, ignoredAttributeLocation, ignoredAttributeName, typeName, otherAttributeName);
    }

    private static string GetId(int id) => $"MSCG01{id}";
}
