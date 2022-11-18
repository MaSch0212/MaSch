﻿using MaSch.CodeAnalysis.CSharp.SourceGeneration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace MaSch.CodeAnalysis.CSharp.UnitTests.SourceGeneration;

[TestClass]
public class SourceBuilderTests : SourceBuilderTestBase<ISourceBuilder>
{
    [TestMethod]
    [SourceBuilderOptions(IncludeFileHeader = true)]
    public void Create_DefaultOptions()
    {
        Assert.AreEqual(SourceBuilder.AutoGeneratedFileHeader, Builder.ToString());
    }

    [TestMethod]
    public void Create_WithoutHeader()
    {
        Assert.AreEqual(string.Empty, Builder.ToString());
    }

    [TestMethod]
    public async Task AppendRegion()
    {
        using (Builder.AppendRegion("Test Region"))
            Builder.AppendLine("// Test Comment");

        await VerifyBuilder();
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task AppendBlock_WithoutContent(bool addSemicolon)
    {
        Builder.AppendBlock(addSemicolon).Dispose();

        await VerifyBuilder(addSemicolon);
    }

    [TestMethod]
    public async Task AppendBlock_WithContent_DefaultIndent()
    {
        using (Builder.AppendBlock())
            Builder.AppendLine("// Test Comment");

        await VerifyBuilder();
    }

    [TestMethod]
    [SourceBuilderOptions(IndentSize = 2)]
    public async Task AppendBlock_WithContent_CustomIndent()
    {
        using (Builder.AppendBlock())
            Builder.AppendLine("// Test Comment");

        await VerifyBuilder();
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task AppendBlock_WithBlockLine(bool addSemicolon)
    {
        Builder.AppendBlock("namespace Hello", addSemicolon).Dispose();

        await VerifyBuilder(addSemicolon);
    }

    [TestMethod]
    public async Task Indent()
    {
        Builder.AppendLine("// Not indented");
        using (Builder.Indent())
            Builder.AppendLine("// Indented");

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Indent_NoAutoNewLine()
    {
        Builder.Append("// Not indented");
        using (Builder.Indent())
            Builder.Append(" - extra content");

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task AppendLine()
    {
        Builder.AppendLine().Should().BeSameAs(Builder);
        Builder.AppendLine("// Test").Should().BeSameAs(Builder);

        await VerifyBuilder();
    }

    [TestMethod]
    [DataRow("", DisplayName = "Empty")]
    [DataRow("    ", DisplayName = "Only Spaces")]
    [DataRow("\t", DisplayName = "Only Tabs")]
    [DataRow("{", DisplayName = "Only opening squirly bracket")]
    [DataRow("    {", DisplayName = "Opening squirly bracket with spaces")]
    [DataRow("\t{", DisplayName = "Opening squirly bracket with tabs")]
    [DataRow("// Text", DisplayName = "Text")]
    public async Task EnsurePreviousLineEmpty(string lineContent)
    {
        Builder.AppendLine(lineContent);
        Builder.EnsurePreviousLineEmpty().Should().BeSameAs(Builder);
        Builder.AppendLine("// Line above should be empty!");

        await VerifyBuilder(lineContent.Replace("\t", "\\t"));
    }

    [TestMethod]
    public async Task EnsurePreviousLineEmpty_CurrentLineNotEmpty()
    {
        Builder.Append("// Test");
        Builder.EnsurePreviousLineEmpty();
        Builder.AppendLine("// Line above should be empty!");

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task AppendInCodeBlock()
    {
        using (Builder.AppendBlock())
            Builder.Append("//").Append(' ').Append("Test");

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task ToSourceText()
    {
        Builder.AppendBlock("namespace Test").Dispose();

        await Verify(Builder.ToSourceText());
    }
}