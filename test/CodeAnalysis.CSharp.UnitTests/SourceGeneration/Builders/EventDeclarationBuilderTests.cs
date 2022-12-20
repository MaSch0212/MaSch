using MaSch.CodeAnalysis.CSharp.SourceGeneration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

namespace MaSch.CodeAnalysis.CSharp.UnitTests.SourceGeneration.Builders;

[TestClass]
public class EventDeclarationBuilderTests : SourceBuilderTestBase<IEventDeclarationBuilder>
{
    public event Action Event1;

    public event Action Event2
    {
        [field: Obsolete]
        add => Event1 += value;
        remove => Event2 -= value;
    }

    [TestMethod]
    [SourceBuilderOptions(EnsureEmptyLineBeforeProperties = true)]
    public async Task Append_Event_WithLineSeparation()
    {
        Builder.Append(Event("Action", "MyEvent1"));
        Builder.Append(Event("Action", "MyEvent2"));

        await VerifyBuilder();
    }

    [TestMethod]
    [SourceBuilderOptions(EnsureEmptyLineBeforeProperties = false)]
    public async Task Append_Event_WithoutLineSeparation()
    {
        Builder.Append(Event("Action", "MyEvent1"));
        Builder.Append(Event("Action", "MyEvent2"));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Event_WithCodeAttributes()
    {
        Builder.Append(Event("Action", "MyEvent1").WithCodeAttribute("Obsolete"));
        Builder.Append(Event("Action", "MyEvent2").WithCodeAttribute("MyAttr", x => x.WithParameters("4711", "\"Test\"")).WithCodeAttribute("Obsolete"));
        Builder.Append(Event("Action", "MyEvent3").WithCodeAttribute("MyAttr3", x => x.OnTarget(CodeAttributeTarget.Event)));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Event_WithAccessModifiersAndKeywords()
    {
        Builder.Append(Event("Action", "MyEvent1").WithAccessModifier(AccessModifier.Public));
        Builder.Append(Event("Action", "MyEvent2").WithKeyword(MemberKeyword.Static));
        Builder.Append(Event("Action", "MyEvent3").WithAccessModifier(AccessModifier.Internal).WithKeyword(MemberKeyword.Sealed));
        Builder.Append(Event("Action", "MyEvent4").WithAccessModifier(AccessModifier.Internal).WithKeyword(MemberKeyword.Sealed).WithKeyword(MemberKeyword.Partial));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Event_WithCustomAddAndRemove()
    {
        Builder.Append(
            Event("EventHandler", "MyEvent1"),
            x => x.AppendLine("// Add Content"),
            x => x.AppendLine("// Remove Content"));

        Builder.Append(
            Event("EventHandler", "MyEvent1").ConfigureAdd(x => x.AsExpression()),
            x => x.Append("OtherEvent += value"),
            x => x.AppendLine("// Remove Content"));

        Builder.Append(
            Event("EventHandler", "MyEvent1").ConfigureRemove(x => x.AsExpression()),
            x => x.AppendLine("// Add Content"),
            x => x.Append("OtherEvent -= value"));

        Builder.Append(
            Event("EventHandler", "MyEvent1")
                .ConfigureAdd(x => x.WithCodeAttribute("MyAttr1"))
                .ConfigureRemove(x => x.WithCodeAttribute("MyAttr1").WithCodeAttribute("MyAttr2", c => c.WithParameters("4711", "\"Test\""))),
            x => x.AppendLine("// Add Content"),
            x => x.AppendLine("// Remove Content"));

        Builder.Append(
            Event("EventHandler", "MyEvent1")
                .ConfigureAdd(x => x.AsExpression().WithCodeAttribute("MyAttr1"))
                .ConfigureRemove(x => x.AsExpression().WithCodeAttribute("MyAttr1").WithCodeAttribute("MyAttr2", c => c.WithParameters("4711", "\"Test\""))),
            x => x.Append("OtherEvent += value"),
            x => x.Append("OtherEvent -= value"));

        Builder.Append(
            Event("EventHandler", "MyEvent1")
                .ConfigureAdd(x => x.AsExpression()
                    .WithCodeAttribute("MyAttr3", x => x.OnTarget(CodeAttributeTarget.Method))
                    .WithCodeAttribute("MyAttr4", x => x.OnTarget(CodeAttributeTarget.Parameter))
                    .WithCodeAttribute("MyAttr5", x => x.OnTarget(CodeAttributeTarget.Return)))
                .ConfigureRemove(x => x.AsExpression()
                    .WithCodeAttribute("MyAttr3", x => x.OnTarget(CodeAttributeTarget.Method))
                    .WithCodeAttribute("MyAttr4", x => x.OnTarget(CodeAttributeTarget.Parameter))
                    .WithCodeAttribute("MyAttr5", x => x.OnTarget(CodeAttributeTarget.Return))),
            x => x.Append("OtherEvent += value"),
            x => x.Append("OtherEvent -= value"));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Event_WithComments()
    {
        Builder.Append(Event("EventHandler", "MyEvent1").WithLineComment("Single Line Line Comment"));
        Builder.Append(Event("EventHandler", "MyEvent2").WithLineComment("Multi Line Line Comment (Line 1)\r\nMulti Line Line Comment (Line 2)\nMulti Line Line Comment (Line 3)"));
        Builder.Append(Event("EventHandler", "MyEvent3").WithBlockComment("Single Line Block Comment"));
        Builder.Append(Event("EventHandler", "MyEvent4").WithBlockComment("Multi Line Block Comment (Line 1)\r\nMulti Line Block Comment (Line 2)\nMulti Line Block Comment (Line 3)"));
        Builder.Append(Event("EventHandler", "MyEvent5").WithDocComment("Single Line Doc Comment"));
        Builder.Append(Event("EventHandler", "MyEvent6").WithDocComment("Multi Line Doc Comment (Line 1)\r\nMulti Line Doc Comment (Line 2)\nMulti Line Doc Comment (Line 3)"));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Event_WithEverything()
    {
        Builder.Append(
            Event("EventHandler", "MyEvent1")
                .WithCodeAttribute("MyAttr", x => x.WithParameters("4711", "\"Test\""))
                .WithCodeAttribute("Obsolete")
                .WithCodeAttribute("MyAttr3", x => x.OnTarget(CodeAttributeTarget.Event))
                .WithAccessModifier(AccessModifier.Internal)
                .WithKeyword(MemberKeyword.Sealed)
                .WithKeyword(MemberKeyword.Partial)
                .ConfigureAdd(x => x.AsExpression()
                    .WithCodeAttribute("MyAttr1")
                    .WithCodeAttribute("MyAttr3", x => x.OnTarget(CodeAttributeTarget.Method))
                    .WithCodeAttribute("MyAttr4", x => x.OnTarget(CodeAttributeTarget.Parameter))
                    .WithCodeAttribute("MyAttr5", x => x.OnTarget(CodeAttributeTarget.Return)))
                .ConfigureRemove(x => x.AsExpression()
                    .WithCodeAttribute("MyAttr1")
                    .WithCodeAttribute("MyAttr2", c => c.WithParameters("4711", "\"Test\""))
                    .WithCodeAttribute("MyAttr3", x => x.OnTarget(CodeAttributeTarget.Method))
                    .WithCodeAttribute("MyAttr4", x => x.OnTarget(CodeAttributeTarget.Parameter))
                    .WithCodeAttribute("MyAttr5", x => x.OnTarget(CodeAttributeTarget.Return))),
            x => x.Append("OtherEvent += value"),
            x => x.Append("OtherEvent -= value"));

        await VerifyBuilder();
    }
}
