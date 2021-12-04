using MaSch.Generators.Common;
using Microsoft.CodeAnalysis;
using static MaSch.Generators.Common.CodeGenerationHelpers;

namespace MaSch.Generators;

/// <summary>
/// A C# 9 Source Generator that generates classes to easily access icon fonts in Wpf using a CSS and font file.
/// </summary>
/// <seealso cref="ISourceGenerator" />
[Generator]
public class WpfIconFontGenerator : ISourceGenerator
{
    /// <inheritdoc />
    public void Initialize(GeneratorInitializationContext context)
    {
        // No initialization required
    }

    /// <inheritdoc />
    public void Execute(GeneratorExecutionContext context)
    {
        var debugGeneratorSymbol = context.Compilation.GetTypeByMetadataName("MaSch.Core.Attributes.DebugGeneratorAttribute");
        var iconFontAttribute = context.Compilation.GetTypeByMetadataName("MaSch.Presentation.Wpf.Attributes.WpfIconFontAttribute");
        var iconFontExtraGeomAttribute = context.Compilation.GetTypeByMetadataName("MaSch.Presentation.Wpf.Attributes.WpfIconFontExtraGeometryAttribute");

        if (iconFontAttribute == null || iconFontExtraGeomAttribute == null)
            return;

        var query = from typeSymbol in context.Compilation.SourceModule.GlobalNamespace.GetNamespaceTypes()
                    let attributes = typeSymbol.GetAllAttributes()
                    let iconFont = attributes.FirstOrDefault(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, iconFontAttribute))
                    where iconFont != null
                    let extraGeoms = (from attr in attributes
                                      where SymbolEqualityComparer.Default.Equals(attr.AttributeClass, iconFontExtraGeomAttribute)
                                      let name = attr.ConstructorArguments.FirstOrDefault().Value as string
                                      let geomPath = attr.ConstructorArguments.Skip(1).FirstOrDefault().Value as string
                                      where !string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(geomPath)
                                      select (name, geomPath)).ToArray()
                    select (typeSymbol, iconFont, extraGeoms);

        foreach (var (typeSymbol, iconFont, extraGeoms) in query)
        {
            if (typeSymbol.GetAttributes().Any(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, debugGeneratorSymbol)))
                LaunchDebuggerOnBuild();

            var fontName = iconFont.ConstructorArguments.FirstOrDefault().Value as string;
            var cssFileName = iconFont.ConstructorArguments.Skip(1).FirstOrDefault().Value as string;
            var cssClassPrefix = iconFont.ConstructorArguments.Skip(2).FirstOrDefault().Value as string;
            var extraIconIdStart = iconFont.ConstructorArguments.Skip(3).FirstOrDefault().Value as uint?;
            var cssClassSuffix = iconFont.NamedArguments.FirstOrDefault(x => x.Key == "CssClassSuffix").Value.Value as string;
            var defaultIconCode = iconFont.NamedArguments.FirstOrDefault(x => x.Key == "DefaultIconCode").Value.Value as uint?;

            if (string.IsNullOrWhiteSpace(fontName) || string.IsNullOrWhiteSpace(cssFileName) || !extraIconIdStart.HasValue)
                continue;

            var cssFile = context.AdditionalFiles.FirstOrDefault(x => Path.GetFileName(x.Path) == cssFileName);
            if (cssFile == null)
                continue;
            var cssText = cssFile.GetText()?.ToString();
            var regex = new Regex($@"\.{Regex.Escape(cssClassPrefix ?? string.Empty)}(?<name>[^:]*){Regex.Escape(cssClassSuffix ?? string.Empty)}:?:before\s*{{\s*content:\s*""\\(?<code>[0-9a-fA-F]*)"";\s*}}");
            var codes = GetCodes(regex, cssText).ToArray();

            var builder = new SourceBuilder();

            _ = builder.AppendLine("using MaSch.Presentation.Wpf;")
                   .AppendLine("using System;")
                   .AppendLine("using System.Collections.Generic;")
                   .AppendLine("using System.ComponentModel;")
                   .AppendLine("using System.Text;")
                   .AppendLine("using System.Windows;")
                   .AppendLine("using System.Windows.Markup;")
                   .AppendLine("using System.Windows.Media;")
                   .AppendLine();

            using (builder.AddBlock($"namespace {typeSymbol.ContainingNamespace}"))
            {
                CreatePartialIconClass(builder, context.Compilation.AssemblyName, typeSymbol.Name, fontName);

                _ = builder.AppendLine();

                CreateMarkupExtensionClass(builder, typeSymbol, defaultIconCode);

                _ = builder.AppendLine();

                using (builder.AddBlock($"internal static class {typeSymbol.Name}CodeCharMapper"))
                {
                    using (builder.AddBlock($"private static readonly Dictionary<{typeSymbol.Name}Code, string> GeometryPaths = new Dictionary<{typeSymbol.Name}Code, string>", true))
                    {
                        foreach (var (name, path) in extraGeoms)
                            _ = builder.AppendLine($"[{typeSymbol.Name}Code.{name}] = \"{path}\",");
                    }

                    _ = builder.AppendLine()
                           .AppendLine($"public static string GetChar(this {typeSymbol.Name}Code iconCode) => Encoding.UTF32.GetString(BitConverter.GetBytes((uint)iconCode));")
                           .AppendLine($"public static {typeSymbol.Name}Code Get{typeSymbol.Name}Code(this string s) => ({typeSymbol.Name}Code)BitConverter.ToUInt32(Encoding.UTF32.GetBytes(s), 0);")
                           .AppendLine($"public static bool IsGeometry(this {typeSymbol.Name}Code iconCode, out string geometryPath) => GeometryPaths.TryGetValue(iconCode, out geometryPath);");
                }

                _ = builder.AppendLine();

                using (builder.AddBlock($"public enum {typeSymbol.Name}Code : uint"))
                {
                    _ = builder.AppendLine("// Custom extra codes");
                    for (int i = 0; i < extraGeoms.Length; i++)
                        _ = builder.AppendLine($"{extraGeoms[i].name} = 0x{Convert.ToString(i + extraIconIdStart.Value, 16).PadLeft(8, '0')}u,");

                    _ = builder.AppendLine().AppendLine("// Codes from CSS");
                    foreach (var (name, code) in codes)
                        _ = builder.AppendLine($"{name} = 0x{code}u,");
                }
            }

            context.AddSource(typeSymbol, builder, nameof(WpfIconFontGenerator));
        }
    }

    private static void CreatePartialIconClass(SourceBuilder builder, string? assemblyName, string typeName, string? fontName)
    {
        using (builder.AddBlock($"partial class {typeName} : Icon"))
        {
            _ = builder.AppendLine($"/// <summary>The font family to use for the <see cref=\"{typeName}\"/> class.</summary>")
                   .AppendLine("public static readonly FontFamily FontFamily;")
                   .AppendLine();
            using (builder.AddBlock($"static {typeName}()"))
            {
                using (builder.AddBlock("if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))"))
                    _ = builder.AppendLine($"FontFamily = new FontFamily(\"{fontName}\");");
                using (builder.AddBlock("else"))
                {
                    _ = builder.AppendLine("FontFamily = Application.Current != null")
                           .AppendLine($"    ? new FontFamily(new Uri(\"pack://application:,,,/{assemblyName};component/\"), \"./#{fontName}\")")
                           .AppendLine($"    : new FontFamily(\"{fontName}\");");
                }
            }

            _ = builder.AppendLine()
                   .AppendLine("/// <summary>Gets or sets the icon.</summary>");
            using (builder.AddBlock($"public {typeName}Code Icon"))
            {
                _ = builder.AppendLine($"get => Character == null ? 0 : Character.Get{typeName}Code();");
                using (builder.AddBlock("set"))
                {
                    using (builder.AddBlock("if (value.IsGeometry(out var geom))"))
                    {
                        _ = builder.AppendLine("Character = null;")
                               .AppendLine("Geometry = Geometry.Parse(geom);")
                               .AppendLine("Type = SymbolType.Geometry;");
                    }

                    using (builder.AddBlock("else"))
                    {
                        _ = builder.AppendLine("Character = value.GetChar();")
                               .AppendLine("Geometry = null;")
                               .AppendLine("Type = SymbolType.Character;");
                    }
                }
            }

            _ = builder.AppendLine()
                   .AppendLine($"/// <summary>Initializes a new instance of the <see cref=\"{typeName}\"/> class.</summary>");
            using (builder.AddBlock($"public {typeName}()"))
            {
                _ = builder.AppendLine("Font = FontFamily;")
                       .AppendLine("Type = SymbolType.Character;")
                       .AppendLine("SetTransform();");
            }

            _ = builder.AppendLine()
                   .AppendLine($"/// <summary>Initializes a new instance of the <see cref=\"{typeName}\"/> class.</summary>")
                   .AppendLine("/// <param name=\"icon\">The icon to use.</param>")
                   .AppendLine($"public {typeName}({typeName}Code icon) : this() => Icon = icon;")
                   .AppendLine()
                   .AppendLine($"/// <summary>Initializes a new instance of the <see cref=\"{typeName}\"/> class.</summary>")
                   .AppendLine("/// <param name=\"icon\">The icon to use.</param>")
                   .AppendLine("/// <param name=\"stretch\">The stretch mode.</param>")
                   .AppendLine($"public {typeName}({typeName}Code icon, Stretch stretch) : this(icon) => Stretch = stretch;")
                   .AppendLine()
                   .AppendLine($"/// <summary>Initializes a new instance of the <see cref=\"{typeName}\"/> class.</summary>")
                   .AppendLine("/// <param name=\"icon\">The icon to use.</param>")
                   .AppendLine("/// <param name=\"stretch\">The stretch mode.</param>")
                   .AppendLine("/// <param name=\"fontSize\">Size of the font.</param>")
                   .AppendLine($"public {typeName}({typeName}Code icon, Stretch stretch, double fontSize) : this(icon, stretch) => FontSize = fontSize;")
                   .AppendLine()
                   .AppendLine($"/// <summary>Initializes a new instance of the <see cref=\"{typeName}\"/> class.</summary>")
                   .AppendLine("/// <param name=\"icon\">The icon to use.</param>")
                   .AppendLine("/// <param name=\"stretch\">The stretch mode.</param>")
                   .AppendLine("/// <param name=\"fontSize\">Size of the font.</param>");
            using (builder.AddBlock($"public {typeName}({typeName}Code icon, Stretch? stretch, double? fontSize) : this(icon)"))
            {
                using (builder.AddBlock("if (stretch.HasValue)"))
                    _ = builder.AppendLine("Stretch = stretch.Value;");
                using (builder.AddBlock("if (fontSize.HasValue)"))
                    _ = builder.AppendLine("FontSize = fontSize.Value;");
            }

            _ = builder.AppendLine()
                   .AppendLine("partial void SetTransform();");
        }
    }

    private static void CreateMarkupExtensionClass(SourceBuilder builder, INamedTypeSymbol typeSymbol, uint? defaultIconCode)
    {
        _ = builder.AppendLine($"/// <summary>Markup extension that creates an <see cref=\"MaSch.Presentation.Wpf.Icon\"/> of type <see cref=\"{typeSymbol}\"/>.</summary>")
                                   .AppendLine("/// <seealso cref=\"MarkupExtension\" />")
                                   .AppendLine("[MarkupExtensionReturnType(typeof(MaSch.Presentation.Wpf.Icon))]");

        using (builder.AddBlock($"public class {typeSymbol.Name}Extension : MarkupExtension"))
        {
            _ = builder.AppendLine("/// <summary>Gets or sets the icon.</summary>")
                   .AppendLine("[ConstructorArgument(\"icon\")]")
                   .AppendLine($"public {typeSymbol.Name}Code Icon {{ get; set; }}")
                   .AppendLine()
                   .AppendLine("/// <summary>Gets or sets the size of the font.</summary>")
                   .AppendLine("public double? FontSize { get; set; }")
                   .AppendLine()
                   .AppendLine("/// <summary>Gets or sets the stretch mode.</summary>")
                   .AppendLine("public Stretch? Stretch { get; set; }")
                   .AppendLine()
                   .AppendLine($"/// <summary>Initializes a new instance of the <see cref=\"{typeSymbol.Name}Extension\"/> class.</summary>");
            using (builder.AddBlock($"public {typeSymbol.Name}Extension()"))
                _ = builder.AppendLine($"Icon = ({typeSymbol.Name}Code){defaultIconCode ?? 0};");
            _ = builder.AppendLine()
                   .AppendLine($"/// <summary>Initializes a new instance of the <see cref=\"{typeSymbol.Name}Extension\"/> class.</summary>")
                   .AppendLine("/// <param name=\"icon\">The icon to use.</param>");
            using (builder.AddBlock($"public {typeSymbol.Name}Extension({typeSymbol.Name}Code icon)"))
                _ = builder.AppendLine($"Icon = icon;");
            _ = builder.AppendLine()
                   .AppendLine("/// <inheritdoc />");
            using (builder.AddBlock("public override object ProvideValue(IServiceProvider serviceProvider)"))
            {
                _ = builder.AppendLine("var pvt = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;");
                using (builder.AddBlock("if (pvt?.TargetObject is Setter)"))
                    _ = builder.AppendLine("return this;");
                _ = builder.AppendLine($"return new {typeSymbol.Name}(Icon, Stretch, FontSize);");
            }
        }
    }

    private static IEnumerable<(string Name, string Code)> GetCodes(Regex regex, string? fileContent)
    {
        foreach (Match match in regex.Matches(fileContent ?? string.Empty))
        {
            var name = GetName(match.Groups["name"].Value);
            var code = match.Groups["code"].Value.PadLeft(8, '0');
            yield return (name, code);
        }
    }

    private static string GetName(string name)
    {
        var result = new StringBuilder();
        var nextUpper = true;
        foreach (var c in name)
        {
            if (c == '-')
            {
                nextUpper = true;
            }
            else if (nextUpper)
            {
                _ = result.Append(c.ToString().ToUpper());
                nextUpper = false;
            }
            else
            {
                _ = result.Append(c.ToString().ToLower());
            }
        }

        return result.ToString();
    }
}
