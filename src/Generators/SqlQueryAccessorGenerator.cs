using MaSch.Generators.Support;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MaSch.Generators;

/// <summary>
/// A C# 9 Source Generator that generates an SqlQueryAccessor that contains the contents of all sql queries in the current project.
/// </summary>
/// <seealso cref="ISourceGenerator" />
[Generator]
public class SqlQueryAccessorGenerator : ISourceGenerator
{
    private const string DefaultClassName = "SqlQueryAccessor";
    private const string DefaultNamespace = "MaSch.Data";

    private static readonly Regex IllegalClassNameCharsRegex = new(@"[\s<>\[\]()\{\}\-?\.,:~=+#\\\/]", RegexOptions.Compiled);

    /// <inheritdoc />
    public void Initialize(GeneratorInitializationContext context)
    {
        // No initialization required
    }

    /// <inheritdoc />
    public void Execute(GeneratorExecutionContext context)
    {
        if (!context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.generatesqlqueryaccessor", out var strShouldGenerate) || !bool.TryParse(strShouldGenerate, out var shouldGenerate) || !shouldGenerate)
            return;

        var assemblyAttributes = context.Compilation.Assembly.GetAttributes();

        _ = context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.rootnamespace", out var globalNamespaceName);
        _ = context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.projectdir", out var projectDir);
        _ = context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.sqlqueryaccessorrootdir", out var rootDir);
        _ = context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.sqlqueryaccessorname", out var className);

        if (string.IsNullOrWhiteSpace(projectDir))
            return;

        rootDir = Path.Combine(projectDir, rootDir?.Trim('/', '\\') ?? string.Empty);

        if (string.IsNullOrWhiteSpace(globalNamespaceName))
            globalNamespaceName = DefaultNamespace;
        if (string.IsNullOrWhiteSpace(className))
            className = DefaultClassName;

        var source = GenerateSqlQueryAccessor(globalNamespaceName, className, rootDir, context.AdditionalFiles.Where(x => x.Path.StartsWith(rootDir)));
        context.AddSource(SourceText.From(source), className);
    }

    private static string GenerateSqlQueryAccessor(string rootNamespace, string className, string rootDir, IEnumerable<AdditionalText> files)
    {
        var rootNode = new Node();
        var dirStartIndex = rootDir.Length;
        foreach (var file in files)
        {
            var relativePath = file.Path.Substring(dirStartIndex).Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
            rootNode.Add(relativePath, file);
        }

        var builder = new SourceBuilder();
        using (builder.AddBlock($"namespace {rootNamespace}"))
        using (builder.AddBlock($"internal static class {className}"))
            rootNode.WriteMembers(builder);

        return builder.ToString();
    }

    private sealed class Node
    {
        public Node(string? name = null)
        {
            Name = name;
        }

        public string? Name { get; set; }
        public List<Node> SubNodes { get; set; } = new List<Node>();
        public List<AdditionalText> Files { get; set; } = new List<AdditionalText>();

        public void Add(string[] pathParts, AdditionalText file)
        {
            if (pathParts.Length <= 1)
            {
                Files.Add(file);
            }
            else
            {
                var subNode = SubNodes.SingleOrDefault(x => x.Name == pathParts[0]);
                if (subNode == null)
                {
                    subNode = new Node(pathParts[0]);
                    SubNodes.Add(subNode);
                }

                subNode.Add(pathParts.Skip(1).ToArray(), file);
            }
        }

        public void Write(SourceBuilder builder)
        {
            using (builder.AddBlock($"internal static class {Name}"))
                WriteMembers(builder);
        }

        public void WriteMembers(SourceBuilder builder)
        {
            bool isFirst = true;
            foreach (var node in SubNodes)
            {
                if (!isFirst)
                    _ = builder.AppendLine();
                node.Write(builder);
                isFirst = false;
            }

            if (!isFirst)
                _ = builder.AppendLine();

            foreach (var file in Files)
            {
                var name = IllegalClassNameCharsRegex.Replace(Path.GetFileNameWithoutExtension(file.Path), "_");
                if (int.TryParse(name[0].ToString(), out _))
                    name = "_" + name;
                _ = builder.AppendLine($"internal static readonly string {name} = \"{StringCStyle(file.GetText().ToString())}\";");
            }
        }

        private static string StringCStyle(string value)
        {
            var badChars = new Dictionary<char, string>
                {
                    { '\r', "\\r" },
                    { '\t', "\\t" },
                    { '\"', "\\\"" },
                    { '\'', "\\\'" },
                    { '\\', "\\\\" },
                    { '\0', "\\0" },
                    { '\n', "\\n" },
                    { '\u2028', "\\u2028" },
                    { '\u2029', "\\u2029" },
                };

            return new string((from c1 in value.ToCharArray()
                               from c2 in badChars.ContainsKey(c1) ? badChars[c1].ToCharArray() : new[] { c1 }
                               select c2).ToArray());
        }
    }
}
