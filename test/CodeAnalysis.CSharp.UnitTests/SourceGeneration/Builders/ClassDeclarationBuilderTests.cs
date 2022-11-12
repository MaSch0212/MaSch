using MaSch.CodeAnalysis.CSharp.SourceGeneration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

namespace MaSch.CodeAnalysis.CSharp.UnitTests.SourceGeneration.Builders;

[TestClass]
public class ClassDeclarationBuilderTests : SourceBuilderTestBase<IClassDeclarationBuilder>
{
    [TestMethod]
    public async Task Append_SimpleClass()
    {
        Builder.Append(Class("MyClass"), ClassDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    [SourceBuilderOptions(EnsureEmptyLineBeforeTypes = true)]
    public async Task Append_ClassesWithLineSeparation()
    {
        Builder.Append(Class("MyClass1"), ClassDummyContent);
        Builder.Append(Class("MyClass2"), ClassDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    [SourceBuilderOptions(EnsureEmptyLineBeforeTypes = false)]
    public async Task Append_ClassesWithoutLineSeparation()
    {
        Builder.Append(Class("MyClass1"), ClassDummyContent);
        Builder.Append(Class("MyClass2"), ClassDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Classes_WithCodeAttributes()
    {
        Builder.Append(Class("MyClass1").WithCodeAttribute("Obsolete"), ClassDummyContent);
        Builder.Append(Class("MyClass2").WithCodeAttribute("MyAttr", x => x.WithParameters("4711", "\"Test\"")).WithCodeAttribute("Obsolete"), ClassDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Classes_WithAccessModifiersAndKeywords()
    {
        Builder.Append(Class("MyClass1").WithAccessModifier(AccessModifier.Public), ClassDummyContent);
        Builder.Append(Class("MyClass2").WithKeyword(MemberKeyword.Static), ClassDummyContent);
        Builder.Append(Class("MyClass3").WithAccessModifier(AccessModifier.Internal).WithKeyword(MemberKeyword.Sealed), ClassDummyContent);
        Builder.Append(Class("MyClass3").WithAccessModifier(AccessModifier.Internal).WithKeyword(MemberKeyword.Sealed).WithKeyword(MemberKeyword.Partial), ClassDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Classes_WithGenericParameters()
    {
        Builder.Append(Class("MyClass1").WithGenericParameter("T"), ClassDummyContent);
        Builder.Append(Class("MyClass2").WithGenericParameter("T", g => g.WithConstraint("class")), ClassDummyContent);
        Builder.Append(Class("MyClass3").WithGenericParameter("T", g => g.WithConstraint("class").WithConstraint("IDisposable")), ClassDummyContent);
        Builder.Append(Class("MyClass4").WithGenericParameter("T1", g => g.WithConstraint("class")).WithGenericParameter("T2", g => g.WithConstraint("struct")), ClassDummyContent);
        Builder.Append(Class("MyClass5").WithGenericParameter("T1").WithGenericParameter("T2"), ClassDummyContent);
        Builder.Append(Class("MyClass6").WithGenericParameter("T", g => g.WithVariance(GenericParameterVariance.In)), ClassDummyContent);
        Builder.Append(Class("MyClass7").WithGenericParameter("T", g => g.WithVariance(GenericParameterVariance.Out)), ClassDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Classes_WithBaseTypes()
    {
        Builder.Append(Class("MyClass1").DerivesFrom("object"), ClassDummyContent);
        Builder.Append(Class("MyClass2").Implements("IDisposable"), ClassDummyContent);
        Builder.Append(Class("MyClass3").Implements("IDisposable").Implements("ISerializable"), ClassDummyContent);
        Builder.Append(Class("MyClass4").DerivesFrom("object").Implements("IDisposable"), ClassDummyContent);
        Builder.Append(Class("MyClass5").DerivesFrom("object").Implements("IDisposable").Implements("ISerializable"), ClassDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Class_WithEverything()
    {
        Builder.Append(
            Class("MyClass")
                .WithCodeAttribute("MyAttr", x => x.WithParameters("4711", "\"Test\""))
                .WithCodeAttribute("Obsolete")
                .WithAccessModifier(AccessModifier.Internal)
                .WithKeyword(MemberKeyword.Sealed)
                .WithKeyword(MemberKeyword.Partial)
                .WithGenericParameter("T1", g => g.WithVariance(GenericParameterVariance.In).WithConstraint("class").WithConstraint("IDisposable"))
                .WithGenericParameter("T2", g => g.WithVariance(GenericParameterVariance.Out).WithConstraint("struct"))
                .DerivesFrom("object")
                .Implements("IDisposable")
                .Implements("ISerializable"),
            ClassDummyContent);

        await VerifyBuilder();
    }

    private void ClassDummyContent(IClassBuilder builder)
    {
        builder.AppendLine("// Class Content");
    }
}
