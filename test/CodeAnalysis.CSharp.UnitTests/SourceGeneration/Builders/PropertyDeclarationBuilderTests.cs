using MaSch.CodeAnalysis.CSharp.SourceGeneration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace MaSch.CodeAnalysis.CSharp.UnitTests.SourceGeneration.Builders;

[TestClass]
public class PropertyDeclarationBuilderTests : SourceBuilderTestBase<IPropertyDeclarationBuilder>
{
    [TestMethod]
    [SourceBuilderOptions(EnsureEmptyLineBeforeProperties = true)]
    public async Task Append_Property_WithLineSeparation()
    {
        Builder.Append(Property("string", "MyProperty1"));
        Builder.Append(Property("string", "MyProperty2"));

        await VerifyBuilder();
    }

    [TestMethod]
    [SourceBuilderOptions(EnsureEmptyLineBeforeProperties = false)]
    public async Task Append_Property_WithoutLineSeparation()
    {
        Builder.Append(Property("string", "MyProperty1"));
        Builder.Append(Property("string", "MyProperty2"));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Property_WithCodeAttributes()
    {
        Builder.Append(Property("string", "MyProperty1").WithCodeAttribute("Obsolete"));
        Builder.Append(Property("string", "MyProperty2").WithCodeAttribute("MyAttr", x => x.WithParameters("4711", "\"Test\"")).WithCodeAttribute("Obsolete"));
        Builder.Append(Property("string", "MyProperty3").WithCodeAttribute("MyAttr3", x => x.OnTarget(CodeAttributeTarget.Property)));
        Builder.Append(Property("string", "MyProperty4").WithCodeAttribute("MyAttr4", x => x.OnTarget(CodeAttributeTarget.Field)));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Property_WithAccessModifiersAndKeywords()
    {
        Builder.Append(Property("string", "MyProperty1").WithAccessModifier(AccessModifier.Public));
        Builder.Append(Property("string", "MyProperty2").WithKeyword(MemberKeyword.Static));
        Builder.Append(Property("string", "MyProperty3").WithAccessModifier(AccessModifier.Internal).WithKeyword(MemberKeyword.Sealed));
        Builder.Append(Property("string", "MyProperty4").WithAccessModifier(AccessModifier.Internal).WithKeyword(MemberKeyword.Sealed).WithKeyword(MemberKeyword.Partial));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Property_WithValue()
    {
        Builder.Append(Property("int", "MyProperty").WithValue("4711"));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Property_ReadOnly()
    {
        Builder.Append(Property("string", "MyProperty1").AsReadOnly());
        Builder.Append(Property("string", "MyProperty2").AsReadOnly().WithValue("\"Test\""));
        Builder.Append(Property("string", "MyProperty3").AsReadOnly(), x => x.AppendLine("return _data;"));
        Builder.Append(Property("string", "MyProperty4").AsReadOnly().ConfigureGet(x => x.AsExpression()), x => x.Append("_data"));
        Builder.Append(Property("string", "MyProperty5").AsReadOnly().AsExpression(false), x => x.Append("_data"));
        Builder.Append(Property("string", "MyProperty6").AsReadOnly().AsExpression(true), x => x.Append("_data"));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Property_WriteOnly()
    {
        Builder.Append(Property("string", "MyProperty1").AsWriteOnly(), x => x.AppendLine("_data = value;"));
        Builder.Append(Property("string", "MyProperty2").AsWriteOnly().ConfigureSet(x => x.AsExpression()), x => x.Append("_data = value"));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Property_WithMethodCodeAttributes()
    {
        Builder.Append(Property("string", "MyProperty1").ConfigureGet(g => g.WithCodeAttribute("MyAttr")));
        Builder.Append(Property("string", "MyProperty2").ConfigureSet(g => g.WithCodeAttribute("MyAttr")));
        Builder.Append(Property("string", "MyProperty3").ConfigureGet(g => g.WithCodeAttribute("MyAttr")).ConfigureSet(g => g.WithCodeAttribute("MyAttr")));
        Builder.Append(Property("string", "MyProperty4").AsReadOnly().ConfigureGet(g => g.WithCodeAttribute("MyAttr")));
        Builder.Append(Property("string", "MyProperty5").AsReadOnly().ConfigureGet(g => g.WithCodeAttribute("MyAttr")).WithValue("\"Test\""));
        Builder.Append(Property("string", "MyProperty6").AsReadOnly().ConfigureGet(g => g.WithCodeAttribute("MyAttr")), x => x.AppendLine("return _data;"));
        Builder.Append(Property("string", "MyProperty7").AsReadOnly().ConfigureGet(g => g.WithCodeAttribute("MyAttr").AsExpression()), x => x.Append("_data"));
        Builder.Append(Property("string", "MyProperty8").AsWriteOnly().ConfigureSet(g => g.WithCodeAttribute("MyAttr")), x => x.Append("_data = value;"));
        Builder.Append(Property("string", "MyProperty9").AsWriteOnly().ConfigureSet(g => g.WithCodeAttribute("MyAttr").AsExpression()), x => x.Append("_data = value"));
        Builder.Append(Property("string", "MyProperty10").ConfigureGet(g => g.WithCodeAttribute("MyAttr", a => a.OnTarget(CodeAttributeTarget.Method))).ConfigureSet(g => g.WithCodeAttribute("MyAttr", a => a.OnTarget(CodeAttributeTarget.Method))));
        Builder.Append(Property("string", "MyProperty11").ConfigureGet(g => g.WithCodeAttribute("MyAttr", a => a.OnTarget(CodeAttributeTarget.Return))).ConfigureSet(g => g.WithCodeAttribute("MyAttr", a => a.OnTarget(CodeAttributeTarget.Return))));
        Builder.Append(Property("string", "MyProperty12").ConfigureGet(g => g.WithCodeAttribute("MyAttr")).ConfigureSet(g => g.WithCodeAttribute("MyAttr")), x => x.AppendLine("return _data;"), x => x.AppendLine("_data = value;"));
        Builder.Append(Property("string", "MyProperty13").AsExpression().ConfigureGet(g => g.WithCodeAttribute("MyAttr")).ConfigureSet(g => g.WithCodeAttribute("MyAttr")), x => x.Append("_data"), x => x.Append("_data = value"));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Property_Init()
    {
        Builder.Append(Property("string", "MyProperty1").AsInitOnly());
        Builder.Append(Property("string", "MyProperty1").AsWriteOnly().AsInitOnly(), x => x.AppendLine("_data = value;"));
        Builder.Append(Property("string", "MyProperty1").AsWriteOnly().AsInitOnly().AsExpression(), x => x.Append("_data = value"));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Property_WithEverything()
    {
        Builder.Append(
            Property("string", "MyProperty")
                .AsInitOnly()
                .WithCodeAttribute("MyAttr", x => x.WithParameters("4711", "\"Test\""))
                .WithCodeAttribute("Obsolete")
                .WithCodeAttribute("MyAttr3", x => x.OnTarget(CodeAttributeTarget.Property))
                .WithCodeAttribute("MyAttr4", x => x.OnTarget(CodeAttributeTarget.Field))
                .WithAccessModifier(AccessModifier.Internal)
                .WithKeyword(MemberKeyword.Sealed)
                .WithKeyword(MemberKeyword.Partial)
                .ConfigureGet(g => g
                    .WithCodeAttribute("MyAttr")
                    .WithCodeAttribute("MyAttr3", a => a.OnTarget(CodeAttributeTarget.Method))
                    .WithCodeAttribute("MyAttr4", a => a.OnTarget(CodeAttributeTarget.Return)))
                .ConfigureSet(g => g
                    .WithCodeAttribute("MyAttr")
                    .WithCodeAttribute("MyAttr3", a => a.OnTarget(CodeAttributeTarget.Method))
                    .WithCodeAttribute("MyAttr4", a => a.OnTarget(CodeAttributeTarget.Return)))
                .WithValue("\"Test\""));

        await VerifyBuilder();
    }
}
