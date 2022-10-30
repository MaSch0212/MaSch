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
}

/// <summary>
/// Specifies keywords for a member of a namespace, class, record, struct or interface.
/// </summary>
public enum MemberKeyword
{
    Static,
    ReadOnly,
    Sealed,
    Const,
    New,
    Override,
    Abstract,
    Virtual,
    Extern,
    Ref,
    Unsafe,
    Operator,
    Explicit,
    Implcitit,
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
            _ => string.Empty;
        };
    }
}

public static class MemberKeywordExtensions
{
    public static string ToMemberPrefix(this MemberKeyword keyword)
    {
        return keyword switch
        {
            MemberKeyword.Static => "static ",
            MemberKeyword.ReadOnly => "readonly ",
            MemberKeyword.Sealed => "sealed ",
            MemberKeyword.Const => "const ",
            MemberKeyword.New => "new ",
            MemberKeyword.Override => "override ",
            MemberKeyword.Abstract => "abstract ",
            MemberKeyword.Virtual => "virtual ",
            MemberKeyword.Extern => "extern ",
            MemberKeyword.Ref => "ref ",
            MemberKeyword.Unsafe => "unsafe ",
            MemberKeyword.Operator => "operator ",
            MemberKeyword.Explicit => "explicit ",
            MemberKeyword.Implcitit => "implicit ",
            _ => string.Empty,
        };
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