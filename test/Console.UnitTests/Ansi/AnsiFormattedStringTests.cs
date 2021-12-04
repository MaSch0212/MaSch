using MaSch.Console.Ansi;

namespace MaSch.Console.UnitTests.Ansi;

[TestClass]
public class AnsiFormattedStringTests : TestClassBase
{
    [TestMethod]
    public void Ctor()
    {
        var s = new AnsiFormattedString();

        Assert.AreEqual(string.Empty, s.ToString(false));
    }

    [TestMethod]
    [DataRow(null, "")]
    [DataRow("", "")]
    [DataRow(" ", " ")]
    [DataRow("\ttest\n", "\ttest\n")]
    public void Ctor_Value(string? value, string expected)
    {
        var s = new AnsiFormattedString(value);

        Assert.AreEqual(expected, s.ToString(false));
        Assert.AreEqual($"\u001b[0m{expected}\u001b[0m", s.ToString());
    }

    [TestMethod]
    [DataRow(false, -1)]
    [DataRow(false, 4)]
    [DataRow(true, -1)]
    [DataRow(true, 4)]
    public void Indexer_Get_OutOfRange(bool withStyle, int index)
    {
        var s = new AnsiFormattedString();
        _ = s.Append("Test");
        if (withStyle)
            _ = s.ApplyStyle(0, 4, x => x.Bold());

        _ = Assert.ThrowsException<IndexOutOfRangeException>(() => s[index]);
    }

    [TestMethod]
    public void Indexer_Get_NoStyles()
    {
        var s = new AnsiFormattedString();
        _ = s.Append("Test");

        Assert.AreEqual('s', s[2]);
    }

    [TestMethod]
    public void Indexer_Get_WithStyles()
    {
        var s = new AnsiFormattedString();
        _ = s.Append("Test", x => x.Bold());

        Assert.AreEqual('s', s[2]);
    }

    [TestMethod]
    [DataRow(false, -1)]
    [DataRow(false, 4)]
    [DataRow(true, -1)]
    [DataRow(true, 4)]
    public void Indexer_Set_OutOfRange(bool withStyle, int index)
    {
        var s = new AnsiFormattedString();
        _ = s.Append("Test");
        if (withStyle)
            _ = s.ApplyStyle(0, 4, x => x.Bold());

        _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => s[index] = '2');
    }

    [TestMethod]
    public void Indexer_Set_NoStyles()
    {
        var s = new AnsiFormattedString();
        _ = s.Append("Test");

        s[2] = '5';

        Assert.AreEqual("Te5t", s.ToString(false));
        Assert.AreEqual("\u001b[0mTe5t\u001b[0m", s.ToString());
    }

    [TestMethod]
    public void Indexer_Set_WithStyles()
    {
        var s = new AnsiFormattedString();
        _ = s.Append("Test", x => x.Bold());

        s[2] = '5';

        Assert.AreEqual("Te5t", s.ToString(false));
        Assert.AreEqual("\u001b[0m\u001b[1mTe5t\u001b[0m", s.ToString());
    }

    [TestMethod]
    public void Length_Get_NoStyles()
    {
        var s = new AnsiFormattedString();
        _ = s.Append("Test");

        Assert.AreEqual(4, s.Length);
    }

    [TestMethod]
    public void Length_Get_WithStyles()
    {
        var s = new AnsiFormattedString();
        _ = s.Append("Test", x => x.Bold());

        Assert.AreEqual(4, s.Length);
    }

    [TestMethod]
    public void Length_Set()
    {
        var s = new AnsiFormattedString();
        _ = s.Append("Test");

        s.Length = 6;

        Assert.AreEqual("Test\0\0", s.ToString(false));
        Assert.AreEqual("\u001b[0mTest\0\0\u001b[0m", s.ToString());
    }

    [TestMethod]
    public void Length_Set_WithStyles_More()
    {
        var s = new AnsiFormattedString();
        _ = s.Append("Test", x => x.Bold());

        s.Length = 6;

        Assert.AreEqual("Test\0\0", s.ToString(false));
        Assert.AreEqual("\u001b[0m\u001b[1mTest\u001b[22m\0\0\u001b[0m", s.ToString());
    }

    [TestMethod]
    public void Length_Set_WithStyles_Less()
    {
        var s = new AnsiFormattedString();
        _ = s.Append("Test", x => x.Bold()).Append("Test", x => x.Italic()).AppendStyle(x => x.Overlined());

        s.Length = 4;

        _ = s.Append("Test123");

        Assert.AreEqual("TestTest123", s.ToString(false));
        Assert.AreEqual("\u001b[0m\u001b[1mTest\u001b[22m\u001b[53mTest123\u001b[0m", s.ToString());
    }

    [TestMethod]
    public void AppendStyle_Null()
    {
        var s = new AnsiFormattedString().Append("Test").AppendStyle(null).Append("Test");

        Assert.AreEqual("TestTest", s.ToString(false));
        Assert.AreEqual("\u001b[0mTestTest\u001b[0m", s.ToString());
    }

    [TestMethod]
    public void AppendStyle()
    {
        var s = new AnsiFormattedString()
            .Append("Test")
            .AppendStyle(x => x.Bold().Foreground(AnsiColorCode.Cyan).Background(AnsiColorCode.DarkGray))
            .Append("Test");

        Assert.AreEqual("TestTest", s.ToString(false));
        Assert.AreEqual("\u001b[0mTest\u001b[1m\u001b[38;5;14m\u001b[48;5;8mTest\u001b[0m", s.ToString());
    }

    [TestMethod]
    [DataRow(3, 6, "Tes\u001b[1mtTestT\u001b[22mest")]
    [DataRow(0, 4, "\u001b[1mTest\u001b[22mTestTest")]
    [DataRow(8, 4, "TestTest\u001b[1mTest")]
    [DataRow(0, 12, "\u001b[1mTestTestTest")]
    public void ApplyStyle(int startIndex, int length, string expected)
    {
        var s = new AnsiFormattedString()
            .Append("TestTestTest")
            .ApplyStyle(startIndex, length, x => x.Bold());

        Assert.AreEqual("TestTestTest", s.ToString(false));
        Assert.AreEqual($"\u001b[0m{expected}\u001b[0m", s.ToString());
    }

    [TestMethod]
    public void ApplyStyle_Null()
    {
        var s = new AnsiFormattedString()
            .Append("TestTestTest")
            .ApplyStyle(3, 6, null);

        Assert.AreEqual("TestTestTest", s.ToString(false));
        Assert.AreEqual("\u001b[0mTestTestTest\u001b[0m", s.ToString());
    }

    [TestMethod]
    [DataRow(-1, 12)]
    [DataRow(0, 13)]
    [DataRow(12, 1)]
    [DataRow(0, -1)]
    public void ApplyStyle_OutOfRange(int startIndex, int length)
    {
        var s = new AnsiFormattedString()
            .Append("TestTestTest");

        _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => s.ApplyStyle(startIndex, length, x => x.Bold()));
    }

    [TestMethod]
    [DataRow(false, false, true)]
    [DataRow(false, false, false)]
    [DataRow(false, true, true)]
    [DataRow(false, true, false)]
    [DataRow(true, true, true)]
    [DataRow(true, true, false)]
    [DataRow(true, false, true)]
    [DataRow(false, false, false)]
    public void Append_IFormattable_Null(bool formattableNull, bool providerNull, bool explicitNull)
    {
        var formatProviderMock = Mocks.Create<IFormatProvider>();
        var provider = providerNull ? null : formatProviderMock.Object;
        var formattableMock = Mocks.Create<IFormattable>();
        if (!formattableNull)
            _ = formattableMock.Setup(x => x.ToString(null, provider)).Returns("blub").Verifiable(Verifiables, Times.Once());
        var formattable = formattableNull ? null : formattableMock.Object;

        var s = new AnsiFormattedString("Test");
        _ = explicitNull ? s.Append(formattable, provider, null) : s.Append(formattable, provider);

        var expected = formattableNull ? "Test" : "Testblub";
        Assert.AreEqual(expected, s.ToString(false));
        Assert.AreEqual($"\u001b[0m{expected}\u001b[0m", s.ToString());
    }
}
