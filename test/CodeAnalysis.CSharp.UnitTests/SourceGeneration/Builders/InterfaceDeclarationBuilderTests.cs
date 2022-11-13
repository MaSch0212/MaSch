using MaSch.CodeAnalysis.CSharp.SourceGeneration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

namespace MaSch.CodeAnalysis.CSharp.UnitTests.SourceGeneration.Builders;

[TestClass]
public class InterfaceDeclarationBuilderTests : SourceBuilderTestBase<IInterfaceDeclarationBuilder>
{
    [TestMethod]
    public async Task Append_SimpleInterface()
    {
        Builder.Append(Interface("MyInterface"), InterfaceDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    [SourceBuilderOptions(EnsureEmptyLineBeforeTypes = true)]
    public async Task Append_InterfaceesWithLineSeparation()
    {
        Builder.Append(Interface("MyInterface1"), InterfaceDummyContent);
        Builder.Append(Interface("MyInterface2"), InterfaceDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    [SourceBuilderOptions(EnsureEmptyLineBeforeTypes = false)]
    public async Task Append_InterfaceesWithoutLineSeparation()
    {
        Builder.Append(Interface("MyInterface1"), InterfaceDummyContent);
        Builder.Append(Interface("MyInterface2"), InterfaceDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Interfacees_WithCodeAttributes()
    {
        Builder.Append(Interface("MyInterface1").WithCodeAttribute("Obsolete"), InterfaceDummyContent);
        Builder.Append(Interface("MyInterface2").WithCodeAttribute("MyAttr", x => x.WithParameters("4711", "\"Test\"")).WithCodeAttribute("Obsolete"), InterfaceDummyContent);
        Builder.Append(Interface("MyInterface3").WithCodeAttribute("MyAttr3", x => x.OnTarget(CodeAttributeTarget.Type)), InterfaceDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Interfacees_WithAccessModifiersAndKeywords()
    {
        Builder.Append(Interface("MyInterface1").WithAccessModifier(AccessModifier.Public), InterfaceDummyContent);
        Builder.Append(Interface("MyInterface2").WithKeyword(MemberKeyword.Static), InterfaceDummyContent);
        Builder.Append(Interface("MyInterface3").WithAccessModifier(AccessModifier.Internal).WithKeyword(MemberKeyword.Sealed), InterfaceDummyContent);
        Builder.Append(Interface("MyInterface3").WithAccessModifier(AccessModifier.Internal).WithKeyword(MemberKeyword.Sealed).WithKeyword(MemberKeyword.Partial), InterfaceDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Interfacees_WithGenericParameters()
    {
        Builder.Append(Interface("MyInterface1").WithGenericParameter("T"), InterfaceDummyContent);
        Builder.Append(Interface("MyInterface2").WithGenericParameter("T", g => g.WithConstraint("class")), InterfaceDummyContent);
        Builder.Append(Interface("MyInterface3").WithGenericParameter("T", g => g.WithConstraint("class").WithConstraint("IDisposable")), InterfaceDummyContent);
        Builder.Append(Interface("MyInterface4").WithGenericParameter("T1", g => g.WithConstraint("class")).WithGenericParameter("T2", g => g.WithConstraint("struct")), InterfaceDummyContent);
        Builder.Append(Interface("MyInterface5").WithGenericParameter("T1").WithGenericParameter("T2"), InterfaceDummyContent);
        Builder.Append(Interface("MyInterface6").WithGenericParameter("T", g => g.WithVariance(GenericParameterVariance.In)), InterfaceDummyContent);
        Builder.Append(Interface("MyInterface7").WithGenericParameter("T", g => g.WithVariance(GenericParameterVariance.Out)), InterfaceDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Interfacees_WithBaseTypes()
    {
        Builder.Append(Interface("MyInterface2").Implements("IDisposable"), InterfaceDummyContent);
        Builder.Append(Interface("MyInterface3").Implements("IDisposable").Implements("ISerializable"), InterfaceDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Interface_WithEverything()
    {
        Builder.Append(
            Interface("MyInterface")
                .WithCodeAttribute("MyAttr", x => x.WithParameters("4711", "\"Test\""))
                .WithCodeAttribute("Obsolete")
                .WithCodeAttribute("MyAttr3", x => x.OnTarget(CodeAttributeTarget.Type))
                .WithAccessModifier(AccessModifier.Internal)
                .WithKeyword(MemberKeyword.Sealed)
                .WithKeyword(MemberKeyword.Partial)
                .WithGenericParameter("T1", g => g.WithVariance(GenericParameterVariance.In).WithConstraint("class").WithConstraint("IDisposable"))
                .WithGenericParameter("T2", g => g.WithVariance(GenericParameterVariance.Out).WithConstraint("struct"))
                .Implements("IDisposable")
                .Implements("ISerializable"),
            InterfaceDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public void Append_Interface_SetsCurrentTypeName()
    {
        Builder.Append(Interface("MyInterface"), x =>
        {
            x.CurrentTypeName.Should().Be("MyInterface");
        });
    }

    private void InterfaceDummyContent(IInterfaceBuilder builder)
    {
        builder.AppendLine("// Interface Content");
    }
}
