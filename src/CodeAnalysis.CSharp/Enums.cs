namespace MaSch.CodeAnalysis.CSharp;

/// <summary>
/// Specifies an access modifier of a member in C#.
/// </summary>
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

public enum MethodBodyType
{
    Block,
    Expression,
    ExpressionNewLine,
}

public static class AccessModifierExtensions
{
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

public static class MemberKeywordExtensions
{
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

public static class CodeAttributeTargetExtensions
{
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

public static class GenericParameterVarianceExtensions
{
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