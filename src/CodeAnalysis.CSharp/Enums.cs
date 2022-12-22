﻿#pragma warning disable SA1649 // File name should match first type name
#pragma warning disable SA1402 // File may only contain a single type

namespace MaSch.CodeAnalysis.CSharp;

/// <summary>
/// Specifies an access modifier of a member in C#.
/// </summary>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/access-modifiers"/>
public enum AccessModifier
{
    /// <summary>
    /// Automatically detect access modifier from the context.
    /// </summary>
    Default,

    /// <summary>
    /// The type or member can be accessed by any other code in the same assembly or another assembly that references it. The accessibility level of public members of a type is controlled by the accessibility level of the type itself.
    /// </summary>
    Public,

    /// <summary>
    /// The type or member can be accessed only by code in the same <c>class</c> or <c>struct</c>.
    /// </summary>
    Private,

    /// <summary>
    /// The type or member can be accessed only by code in the same <c>class</c>, or in a <c>class</c> that is derived from that <c>class</c>.
    /// </summary>
    Protected,

    /// <summary>
    /// The type or member can be accessed by any code in the same assembly, but not from another assembly. In other words, <c>internal</c> types or members can be accessed from code that is part of the same compilation.
    /// </summary>
    Internal,

    /// <summary>
    /// The type or member can be accessed by any code in the assembly in which it's declared, or from within a derived <c>class</c> in another assembly.
    /// </summary>
    ProtectedInternal,

    /// <summary>
    /// The type or member can be accessed by types derived from the <c>class</c> that are declared within its containing assembly.
    /// </summary>
    PrivateProtected,

    /// <summary>
    /// The type can be accessed by types that are declared within its containing file.
    /// </summary>
    File,
}

/// <summary>
/// Specifies keywords for a member of a namespace, class, record, struct or interface.
/// </summary>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/"/>
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "Enum member names are self-explanatory.")]
[Flags]
public enum MemberKeyword : uint
{
    None = 0,
    Static = 0x1,
    ReadOnly = 0x2,
    Sealed = 0x4,
    Const = 0x8,
    New = 0x10,
    Override = 0x20,
    Abstract = 0x40,
    Virtual = 0x80,
    Extern = 0x100,
    Unsafe = 0x200,
    Ref = 0x400,
    Partial = 0x4000,
}

/// <summary>
/// Specified the target for a code attribute.
/// </summary>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/attributes/#attribute-targets"/>
public enum CodeAttributeTarget
{
    /// <summary>
    /// Automatically detect target from the context.
    /// </summary>
    Default,

    /// <summary>
    /// Entire assembly.
    /// </summary>
    Assembly,

    /// <summary>
    /// Current assembly module.
    /// </summary>
    Module,

    /// <summary>
    /// Field in a class or a struct.
    /// </summary>
    Field,

    /// <summary>
    /// Event.
    /// </summary>
    Event,

    /// <summary>
    /// Method or <c>get</c> and <c>set</c> property accessors.
    /// </summary>
    Method,

    /// <summary>
    /// Method parameters or <c>set</c> property accessor parameters.
    /// </summary>
    Parameter,

    /// <summary>
    /// Property.
    /// </summary>
    Property,

    /// <summary>
    /// Return value of a method, property indexer, or <c>get</c> property accessor.
    /// </summary>
    Return,

    /// <summary>
    /// Struct, class, interface, enum, or delegate.
    /// </summary>
    Type,
}

/// <summary>
/// Specified the variance of a generic parameter.
/// </summary>
public enum GenericParameterVariance
{
    /// <summary>
    /// Does not specify a variance.
    /// </summary>
    None,

    /// <summary>
    /// For generic type parameters, the in keyword specifies that the type parameter is contravariant. You can use the in keyword in generic interfaces and delegates.
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/in-generic-modifier"/>
    Contravariant,

    /// <summary>
    /// For generic type parameters, the out keyword specifies that the type parameter is covariant. You can use the out keyword in generic interfaces and delegates.
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/out-generic-modifier"/>
    Covariant,

    /// <inheritdoc cref="Contravariant"/>
    In = Contravariant,

    /// <inheritdoc cref="Covariant"/>
    Out = Covariant,
}

/// <summary>
/// Specifies the style to use when writing a method body.
/// </summary>
public enum MethodBodyType
{
    /// <summary>
    /// Creates a C# code block around the method body.
    /// </summary>
    /// <remarks>
    /// Example:
    /// <code>
    /// void MyMethod()
    /// {
    ///     // Method body
    /// }
    /// </code>
    /// </remarks>
    Block,

    /// <summary>
    /// Uses the expression body syntax (=>).
    /// </summary>
    /// <remarks>
    /// Example:
    /// <code>
    /// void MyMethod() => [...];
    /// </code>
    /// </remarks>
    Expression,

    /// <summary>
    /// Uses the expression body syntax (=>) and makes sure the expression starts in a new line that is indented.
    /// </summary>
    /// <remarks>
    /// Example:
    /// <code>
    /// void MyMethod()
    ///     => [...];
    /// </code>
    /// </remarks>
    ExpressionNewLine,
}

/// <summary>
/// Specifies the case style of a string.
/// </summary>
public enum CaseStyle
{
    /// <summary>
    /// Uses pascal case.
    /// </summary>
    /// <remarks><code>MyImportantMember</code></remarks>
    PascalCase,

    /// <summary>
    /// Uses camel case.
    /// </summary>
    /// <remarks><code>myImportantMember</code></remarks>
    CamelCase,

    /// <summary>
    /// Uses kebab case.
    /// </summary>
    /// <remarks><code>my-important-member</code></remarks>
    KebabCase,

    /// <summary>
    /// Uses snake case.
    /// </summary>
    /// <remarks><code>my_important_member</code></remarks>
    SnakeCase,

    /// <summary>
    /// Uses snake case in all upper case letters.
    /// </summary>
    /// <remarks><code>MY_IMPORTANT_MEMBER</code></remarks>
    UpperSnakeCase,
}

/// <summary>
/// Specifies the spacing style of a code block.
/// </summary>
[Flags]
[SuppressMessage("Critical Code Smell", "S2346:Flags enumerations zero-value members should be named \"None\"", Justification = "This would not fit.")]
public enum CodeBlockStyle
{
    /// <summary>
    /// No line breaks or any spacing.
    /// </summary>
    /// <remarks>
    /// <code>
    /// [CodeBeforeBlock][Prefix]{[Content]}[Suffix][CodeAfterBlock]
    /// </code>
    /// </remarks>
    NoLineBreaks = 0,

    /// <summary>
    /// Ensures that the block prefix is on an empty line.
    /// </summary>
    /// <remarks>
    /// <code>
    /// [CodeBeforeBlock]
    /// [Prefix]{[Content]}[Suffix][CodeAfterBlock]
    /// </code>
    /// </remarks>
    EnsureBlockPrefixOnEmptyLine = 1,

    /// <summary>
    /// Ensures that the opening bracket is on an empty line.
    /// </summary>
    /// <remarks>
    /// <code>
    /// [CodeBeforeBlock][Prefix]
    /// {[Content]}[Suffix][CodeAfterBlock]
    /// </code>
    /// </remarks>
    EnsureOpeningBracketOnEmptyLine = 2,

    /// <summary>
    /// Appends a new line after the opening bracket.
    /// </summary>
    /// <remarks>
    /// <code>
    /// [CodeBeforeBlock][Prefix]{
    /// [Content]}[Suffix][CodeAfterBlock]
    /// </code>
    /// </remarks>
    AppendLineAfterOpeningBracket = 4,

    /// <summary>
    /// Ensures that the closing bracket is on an empty line.
    /// </summary>
    /// <remarks>
    /// <code>
    /// [CodeBeforeBlock][Prefix]{[Content]
    /// }[Suffix][CodeAfterBlock]
    /// </code>
    /// </remarks>
    EnsureClosingBracketOnEmptyLine = 8,

    /// <summary>
    /// Ensures that the block suffix is on an empty line.
    /// </summary>
    /// <remarks>
    /// <code>
    /// [CodeBeforeBlock][Prefix]{[Content]}
    /// [Suffix][CodeAfterBlock]
    /// </code>
    /// </remarks>
    EnsureBlockSuffixOnEmptyLine = 16,

    /// <summary>
    /// Appends a new line after the code block.
    /// </summary>
    /// <remarks>
    /// <code>
    /// [CodeBeforeBlock][Prefix]{[Content]}[Suffix]
    /// [CodeAfterBlock]
    /// </code>
    /// </remarks>
    AppendLineAfterBlock = 32,

    /// <summary>
    /// Ensures that the brackets are spaces either by new line or whitespace.
    /// </summary>
    /// <remarks>
    /// <code>
    /// [CodeBeforeBlock][Prefix] { [Content] } [Suffix][CodeAfterBlock]
    /// </code>
    /// </remarks>
    EnsureBracketSpacing = 64,

    /// <summary>
    /// The default code block spacing.
    /// </summary>
    /// <remarks>
    /// <code>
    /// [CodeBeforeBlock]
    /// [Prefix]
    /// {
    ///     [Content]
    /// }[Suffix]
    /// [CodeAfterBlock]
    /// </code>
    /// </remarks>
    Default = EnsureBlockPrefixOnEmptyLine | EnsureOpeningBracketOnEmptyLine | AppendLineAfterOpeningBracket | EnsureClosingBracketOnEmptyLine | AppendLineAfterBlock | EnsureBracketSpacing,
}

/// <summary>
/// Represents the type of comment.
/// </summary>
public enum CommentType
{
    /// <summary>
    /// The comment is a line comment.
    /// </summary>
    /// <remarks>
    /// <code>
    /// // Single Comment Line
    ///
    /// // Multiple
    /// // Comment
    /// // Lines
    /// </code>
    /// </remarks>
    Line,

    /// <summary>
    /// The comment is a block comment.
    /// </summary>
    /// <remarks>
    /// <code>
    /// /* Single Comment Line */
    ///
    /// /* Multiple
    ///  * Comment
    ///  * Lines
    ///  */
    /// </code>
    /// </remarks>
    Block,

    /// <summary>
    /// The comment is a XML documentation comment.
    /// </summary>
    /// <remarks>
    /// <code>
    /// /// Single Comment Line
    ///
    /// /// Multiple
    /// /// Comment
    /// /// Lines
    /// </code>
    /// </remarks>
    Doc,
}

/// <summary>
/// Provides extension methods for the <see cref="AccessModifier"/> enum.
/// </summary>
public static class AccessModifierExtensions
{
    /// <summary>
    /// Converts this <see cref="AccessModifier"/> to the prefix for a C# member.
    /// </summary>
    /// <param name="accessModifier">The access modifier to convert.</param>
    /// <returns>The prefix for a C# member representing the given access modifier.</returns>
    public static string ToMemberPrefix(this AccessModifier accessModifier)
    {
        return accessModifier switch
        {
            AccessModifier.Public => "public ",
            AccessModifier.Private => "private ",
            AccessModifier.Protected => "protected ",
            AccessModifier.Internal => "internal ",
            AccessModifier.ProtectedInternal => "protected internal ",
            AccessModifier.PrivateProtected => "private protected ",
            AccessModifier.File => "file ",
            _ => string.Empty,
        };
    }
}

/// <summary>
/// Provides extension methods for the <see cref="MemberKeyword"/> enum.
/// </summary>
public static class MemberKeywordExtensions
{
    /// <summary>
    /// Converts this <see cref="MemberKeyword"/> to the prefix for a C# member.
    /// </summary>
    /// <param name="keyword">The keyword to convert.</param>
    /// <returns>The prefix for a C# member representing the given keyword.</returns>
    public static string ToMemberPrefix(this MemberKeyword keyword)
    {
        string prefix = string.Empty;
        if (keyword.HasFlag(MemberKeyword.Static))
            prefix += "static ";
        if (keyword.HasFlag(MemberKeyword.ReadOnly))
            prefix += "readonly ";
        if (keyword.HasFlag(MemberKeyword.Sealed))
            prefix += "sealed ";
        if (keyword.HasFlag(MemberKeyword.Const))
            prefix += "const ";
        if (keyword.HasFlag(MemberKeyword.New))
            prefix += "new ";
        if (keyword.HasFlag(MemberKeyword.Override))
            prefix += "override ";
        if (keyword.HasFlag(MemberKeyword.Abstract))
            prefix += "abstract ";
        if (keyword.HasFlag(MemberKeyword.Virtual))
            prefix += "virtual ";
        if (keyword.HasFlag(MemberKeyword.Extern))
            prefix += "extern ";
        if (keyword.HasFlag(MemberKeyword.Ref))
            prefix += "ref ";
        if (keyword.HasFlag(MemberKeyword.Unsafe))
            prefix += "unsafe ";
        if (keyword.HasFlag(MemberKeyword.Partial))
            prefix += "partial ";
        return prefix;
    }
}

/// <summary>
/// Provides extension methods for the <see cref="CodeAttributeTarget"/> enum.
/// </summary>
public static class CodeAttributeTargetExtensions
{
    /// <summary>
    /// Converts this <see cref="CodeAttributeTarget"/> to the prefix for a C# code attribute.
    /// </summary>
    /// <param name="target">The target to convert.</param>
    /// <returns>The prefix for a C# code attribute representing the given target.</returns>
    public static string ToAttributePrefix(this CodeAttributeTarget target)
    {
        return target switch
        {
            CodeAttributeTarget.Assembly => "assembly: ",
            CodeAttributeTarget.Module => "module: ",
            CodeAttributeTarget.Field => "field: ",
            CodeAttributeTarget.Event => "event: ",
            CodeAttributeTarget.Method => "method: ",
            CodeAttributeTarget.Parameter => "param: ",
            CodeAttributeTarget.Property => "property: ",
            CodeAttributeTarget.Return => "return: ",
            CodeAttributeTarget.Type => "type: ",
            _ => string.Empty,
        };
    }
}

/// <summary>
/// Provides extension methods for the <see cref="GenericParameterVariance"/> enum.
/// </summary>
public static class GenericParameterVarianceExtensions
{
    /// <summary>
    /// Converts this <see cref="GenericParameterVariance"/> to the prefix for a generic parameter in C#.
    /// </summary>
    /// <param name="variance">The variance to convert.</param>
    /// <returns>The prefix for a generic parameter in C# representing the given variance.</returns>
    public static string ToParameterPrefix(this GenericParameterVariance variance)
    {
        return variance switch
        {
            GenericParameterVariance.Contravariant => "in ",
            GenericParameterVariance.Covariant => "out ",
            _ => string.Empty,
        };
    }
}