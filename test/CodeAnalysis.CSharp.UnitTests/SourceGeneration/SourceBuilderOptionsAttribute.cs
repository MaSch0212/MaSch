using MaSch.CodeAnalysis.CSharp.SourceGeneration;

namespace MaSch.CodeAnalysis.CSharp.UnitTests.SourceGeneration;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
internal class SourceBuilderOptionsAttribute : Attribute
{
    public static SourceBuilderOptionsAttribute Default { get; } = new();

    public int IndentSize { get; set; } = SourceBuilderOptions.Default.IndentSize;
    public bool IncludeFileHeader { get; set; } = false;
    public bool EnsureEmptyLineBeforeTypes { get; set; } = SourceBuilderOptions.Default.EnsureEmptyLineBeforeTypes;
    public bool EnsureEmptyLineBeforeNamespaces { get; set; } = SourceBuilderOptions.Default.EnsureEmptyLineBeforeNamespaces;
    public bool EnsureEmptyLineBeforeConstructors { get; set; } = SourceBuilderOptions.Default.EnsureEmptyLineBeforeConstructors;
    public bool EnsureEmptyLineBeforeMethods { get; set; } = SourceBuilderOptions.Default.EnsureEmptyLineBeforeMethods;
    public bool EnsureEmptyLineBeforeFields { get; set; } = SourceBuilderOptions.Default.EnsureEmptyLineBeforeFields;
    public bool EnsureEmptyLineBeforeProperties { get; set; } = SourceBuilderOptions.Default.EnsureEmptyLineBeforeProperties;
    public bool EnsureEmptyLineBeforeEnumValues { get; set; } = SourceBuilderOptions.Default.EnsureEmptyLineBeforeEnumValues;

    public SourceBuilderOptions CreateOptions()
    {
        return new SourceBuilderOptions
        {
            Capacity = 1024,
            IndentSize = IndentSize,
            IncludeFileHeader = IncludeFileHeader,
            EnsureEmptyLineBeforeTypes = EnsureEmptyLineBeforeTypes,
            EnsureEmptyLineBeforeNamespaces = EnsureEmptyLineBeforeNamespaces,
            EnsureEmptyLineBeforeConstructors = EnsureEmptyLineBeforeConstructors,
            EnsureEmptyLineBeforeMethods = EnsureEmptyLineBeforeMethods,
            EnsureEmptyLineBeforeFields = EnsureEmptyLineBeforeFields,
            EnsureEmptyLineBeforeProperties = EnsureEmptyLineBeforeProperties,
            EnsureEmptyLineBeforeEnumValues = EnsureEmptyLineBeforeEnumValues,
        };
    }
}
