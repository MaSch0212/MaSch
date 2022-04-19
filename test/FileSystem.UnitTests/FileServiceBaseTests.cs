using MaSch.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace MaSch.FileSystem.UnitTests;

[TestClass]
public class FileServiceBaseTests : TestClassBase
{
    private static readonly Random _rng = new();

    private const int DefaultBufferSize = 4096;
    private const string Path = "C:\\Folder\\SubFolder\\file.txt";
    private FileServiceBase Sut => SutMock.Object;
    private Mock<FileServiceBase> SutMock { get; set; } = null!;
    private Mock<IDirectoryService> DirectoryService { get; set; } = null!;

    [TestMethod]
    public void Constructor_NullChecks()
    {
        var ex = Assert.ThrowsException<TargetInvocationException>(() => Mocks.Create<FileServiceBase>(new object?[] { null }).Object);
        Assert.IsInstanceOfType<ArgumentNullException>(ex.InnerException);
    }

    [TestMethod]
    [DataRow(null, typeof(ArgumentNullException))]
    [DataRow("", typeof(ArgumentException))]
    [DataRow("   ", typeof(ArgumentException))]
    public void GetDirectory_ParameterChecks(string path, Type expectedExceptionType)
    {
        Assert.ThrowsException(expectedExceptionType, () => Sut.GetDirectory(path));
    }

    [TestMethod]
    public void GetDirectory_DirectInRoot()
    {
        Assert.IsNull(Sut.GetDirectory("C:\\file.txt"));
    }

    [TestMethod]
    public void GetDirectory()
    {
        var expected = Mocks.Create<IDirectoryInfo>().Object;
        DirectoryService
            .Setup(x => x.GetInfo(System.IO.Path.GetDirectoryName(Path)!))
            .Returns(expected)
            .Verifiable(Verifiables, Times.Once());

        var actual = Sut.GetDirectory(Path);
        Assert.AreSame(expected, actual);
    }

    [TestMethod]
    public void GetDirectory_NoParentDir()
    {
        var actual = Sut.GetDirectory("C:\\");
        Assert.IsNull(actual);
    }

    [TestMethod]
    [DataRow(true, DisplayName = "Default Encoding")]
    [DataRow(false, DisplayName = "ASCII Encoding")]
    public void AppendAllLines(bool useDefaultEncoding)
    {
        var encoding = useDefaultEncoding ? Encoding.Default : Encoding.ASCII;
        using var stream = new MemoryStream();
        SutMock.Protected()
            .Setup<Stream>("Open", Path, FileMode.Append, FileAccess.Write, FileShare.Read, DefaultBufferSize, FileOptions.None)
            .Returns(stream)
            .Verifiable(Verifiables, Times.Once());
        var contents = new[] { "Line1", "Line2", "Line3" };
        var expectedFileContent = $"Line1{Environment.NewLine}Line2{Environment.NewLine}Line3{Environment.NewLine}";

        if (useDefaultEncoding)
            Sut.AppendAllLines(Path, contents);
        else
            Sut.AppendAllLines(Path, contents, encoding);

        var actualFileContent = encoding.GetString(stream.ToArray());
        Assert.AreEqual(expectedFileContent, actualFileContent);
    }

    [TestMethod]
    [DataRow(true, DisplayName = "Default Encoding")]
    [DataRow(false, DisplayName = "ASCII Encoding")]
    public async Task AppendAllLinesAsync_IEnumerable(bool useDefaultEncoding)
    {
        var encoding = useDefaultEncoding ? Encoding.Default : Encoding.ASCII;
        using var stream = new MemoryStream();
        SutMock.Protected()
            .Setup<Stream>("Open", Path, FileMode.Append, FileAccess.Write, FileShare.Read, DefaultBufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan)
            .Returns(stream)
            .Verifiable(Verifiables, Times.Once());
        var contents = new[] { "Line1", "Line2", "Line3" };
        var expectedFileContent = $"Line1{Environment.NewLine}Line2{Environment.NewLine}Line3{Environment.NewLine}";

        if (useDefaultEncoding)
            await Sut.AppendAllLinesAsync(Path, contents);
        else
            await Sut.AppendAllLinesAsync(Path, contents, encoding);

        var actualFileContent = encoding.GetString(stream.ToArray());
        Assert.AreEqual(expectedFileContent, actualFileContent);
    }

#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
    [TestMethod]
    [DataRow(true, DisplayName = "Default Encoding")]
    [DataRow(false, DisplayName = "ASCII Encoding")]
    public async Task AppendAllLinesAsync_IAsyncEnumerable(bool useDefaultEncoding)
    {
        var encoding = useDefaultEncoding ? Encoding.Default : Encoding.ASCII;
        using var stream = new MemoryStream();
        SutMock.Protected()
            .Setup<Stream>("Open", Path, FileMode.Append, FileAccess.Write, FileShare.Read, DefaultBufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan)
            .Returns(stream)
            .Verifiable(Verifiables, Times.Once());
        var contents = GetContents();
        var expectedFileContent = $"Line1{Environment.NewLine}Line2{Environment.NewLine}Line3{Environment.NewLine}";

        if (useDefaultEncoding)
            await Sut.AppendAllLinesAsync(Path, contents);
        else
            await Sut.AppendAllLinesAsync(Path, contents, encoding);

        var actualFileContent = encoding.GetString(stream.ToArray());
        Assert.AreEqual(expectedFileContent, actualFileContent);

        async IAsyncEnumerable<string> GetContents()
        {
            yield return "Line1";
            await Task.Delay(0);
            yield return "Line2";
            await Task.Delay(0);
            yield return "Line3";
        }
    }
#endif

    [TestMethod]
    [DataRow(true, DisplayName = "Default Encoding")]
    [DataRow(false, DisplayName = "ASCII Encoding")]
    public void AppendAllText(bool useDefaultEncoding)
    {
        var encoding = useDefaultEncoding ? Encoding.Default : Encoding.ASCII;
        using var stream = new MemoryStream();
        SutMock.Protected()
            .Setup<Stream>("Open", Path, FileMode.Append, FileAccess.Write, FileShare.Read, DefaultBufferSize, FileOptions.None)
            .Returns(stream)
            .Verifiable(Verifiables, Times.Once());
        var expectedFileContent = $"Line1{Environment.NewLine}Line2{Environment.NewLine}Line3";

        if (useDefaultEncoding)
            Sut.AppendAllText(Path, expectedFileContent);
        else
            Sut.AppendAllText(Path, expectedFileContent, encoding);

        var actualFileContent = encoding.GetString(stream.ToArray());
        Assert.AreEqual(expectedFileContent, actualFileContent);
    }

    [TestMethod]
    [DataRow(true, DisplayName = "Default Encoding")]
    [DataRow(false, DisplayName = "ASCII Encoding")]
    public async Task AppendAllTextAsync(bool useDefaultEncoding)
    {
        var encoding = useDefaultEncoding ? Encoding.Default : Encoding.ASCII;
        using var stream = new MemoryStream();
        SutMock.Protected()
            .Setup<Stream>("Open", Path, FileMode.Append, FileAccess.Write, FileShare.Read, DefaultBufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan)
            .Returns(stream)
            .Verifiable(Verifiables, Times.Once());
        var expectedFileContent = $"Line1{Environment.NewLine}Line2{Environment.NewLine}Line3";

        if (useDefaultEncoding)
            await Sut.AppendAllTextAsync(Path, expectedFileContent);
        else
            await Sut.AppendAllTextAsync(Path, expectedFileContent, encoding);

        var actualFileContent = encoding.GetString(stream.ToArray());
        Assert.AreEqual(expectedFileContent, actualFileContent);
    }

    [TestMethod]
    public void AppendText()
    {
        using var stream = new MemoryStream();
        SutMock.Protected()
            .Setup<Stream>("Open", Path, FileMode.Append, FileAccess.Write, FileShare.Read, DefaultBufferSize, FileOptions.None)
            .Returns(stream)
            .Verifiable(Verifiables, Times.Once());

        using var actual = Sut.AppendText(Path);

        Assert.AreSame(stream, actual.BaseStream);
    }

    [TestMethod]
    public void Copy()
    {
        var sourceFileName = "C:\\Folder\\SubFolder\\file1.txt";
        var destFileName = "C:\\Folder\\SubFolder\\file2.txt";
        SutMock
            .Setup(x => x.Copy(sourceFileName, destFileName, false))
            .Verifiable(Verifiables, Times.Once());

        Sut.Copy(sourceFileName, destFileName);
    }

    [TestMethod]
    public void Create()
    {
        using var stream = new MemoryStream();
        SutMock.Protected()
            .Setup<Stream>("Open", Path, FileMode.Create, FileAccess.ReadWrite, FileShare.None, DefaultBufferSize, FileOptions.None)
            .Returns(stream)
            .Verifiable(Verifiables, Times.Once());

        var actual = Sut.Create(Path);

        Assert.AreSame(stream, actual);
    }

    [TestMethod]
    public void Create_BufferSize()
    {
        using var stream = new MemoryStream();
        SutMock.Protected()
            .Setup<Stream>("Open", Path, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 4711, FileOptions.None)
            .Returns(stream)
            .Verifiable(Verifiables, Times.Once());

        var actual = Sut.Create(Path, 4711);

        Assert.AreSame(stream, actual);
    }

    [TestMethod]
    public void Create_BufferSize_Options()
    {
        var options = _rng.NextEnum<FileOptions>();
        using var stream = new MemoryStream();
        SutMock.Protected()
            .Setup<Stream>("Open", Path, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 4711, options)
            .Returns(stream)
            .Verifiable(Verifiables, Times.Once());

        var actual = Sut.Create(Path, 4711, options);

        Assert.AreSame(stream, actual);
    }

    [TestMethod]
    public void CreateText()
    {
        using var stream = new MemoryStream();
        SutMock.Protected()
            .Setup<Stream>("Open", Path, FileMode.Create, FileAccess.ReadWrite, FileShare.None, DefaultBufferSize, FileOptions.None)
            .Returns(stream)
            .Verifiable(Verifiables, Times.Once());

        using var actual = Sut.CreateText(Path);

        Assert.AreSame(stream, actual.BaseStream);
    }

    [TestMethod]
    public void GetCreationTime()
    {
        var creationTimeUtc = DateTime.UtcNow;
        SutMock
            .Setup(x => x.GetCreationTimeUtc(Path))
            .Returns(creationTimeUtc)
            .Verifiable(Verifiables, Times.Once());

        var creationTime = Sut.GetCreationTime(Path);

        Assert.AreEqual(creationTimeUtc.ToLocalTime(), creationTime);
    }

    [TestMethod]
    public void GetLastAccessTime()
    {
        var lastAccessTimeUtc = DateTime.UtcNow;
        SutMock
            .Setup(x => x.GetLastAccessTimeUtc(Path))
            .Returns(lastAccessTimeUtc)
            .Verifiable(Verifiables, Times.Once());

        var lastAccessTime = Sut.GetLastAccessTime(Path);

        Assert.AreEqual(lastAccessTimeUtc.ToLocalTime(), lastAccessTime);
    }

    [TestMethod]
    public void GetLastWriteTime()
    {
        var lastWriteTimeUtc = DateTime.UtcNow;
        SutMock
            .Setup(x => x.GetLastWriteTimeUtc(Path))
            .Returns(lastWriteTimeUtc)
            .Verifiable(Verifiables, Times.Once());

        var lastWriteTime = Sut.GetLastWriteTime(Path);

        Assert.AreEqual(lastWriteTimeUtc.ToLocalTime(), lastWriteTime);
    }

    [TestMethod]
    public void Move()
    {
        var sourceFileName = "C:\\Folder\\SubFolder\\file1.txt";
        var destFileName = "C:\\Folder\\SubFolder\\file2.txt";
        SutMock
            .Setup(x => x.Move(sourceFileName, destFileName, false))
            .Verifiable(Verifiables, Times.Once());

        Sut.Move(sourceFileName, destFileName);
    }

    [TestMethod]
    public void Open_Mode()
    {
        var mode = FileMode.Create;
        using var stream = new MemoryStream();
        SutMock.Protected()
            .Setup<Stream>("Open", Path, mode, FileAccess.ReadWrite, FileShare.None, DefaultBufferSize, FileOptions.None)
            .Returns(stream)
            .Verifiable(Verifiables, Times.Once());

        var actual = Sut.Open(Path, mode);

        Assert.AreSame(stream, actual);
    }

    [TestMethod]
    public void Open_Mode_Append()
    {
        var mode = FileMode.Append;
        using var stream = new MemoryStream();
        SutMock.Protected()
            .Setup<Stream>("Open", Path, mode, FileAccess.Write, FileShare.None, DefaultBufferSize, FileOptions.None)
            .Returns(stream)
            .Verifiable(Verifiables, Times.Once());

        var actual = Sut.Open(Path, mode);

        Assert.AreSame(stream, actual);
    }

    [TestMethod]
    public void Open_Mode_Access()
    {
        var mode = _rng.NextEnum<FileMode>();
        var access = _rng.NextEnum<FileAccess>();
        using var stream = new MemoryStream();
        SutMock.Protected()
            .Setup<Stream>("Open", Path, mode, access, FileShare.None, DefaultBufferSize, FileOptions.None)
            .Returns(stream)
            .Verifiable(Verifiables, Times.Once());

        var actual = Sut.Open(Path, mode, access);

        Assert.AreSame(stream, actual);
    }

    [TestMethod]
    public void Open_Mode_Access_Share()
    {
        var mode = _rng.NextEnum<FileMode>();
        var access = _rng.NextEnum<FileAccess>();
        var share = _rng.NextEnum<FileShare>();
        using var stream = new MemoryStream();
        SutMock.Protected()
            .Setup<Stream>("Open", Path, mode, access, share, DefaultBufferSize, FileOptions.None)
            .Returns(stream)
            .Verifiable(Verifiables, Times.Once());

        var actual = Sut.Open(Path, mode, access, share);

        Assert.AreSame(stream, actual);
    }

    [TestMethod]
    public void OpenRead()
    {
        using var stream = new MemoryStream();
        SutMock.Protected()
            .Setup<Stream>("Open", Path, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize, FileOptions.None)
            .Returns(stream)
            .Verifiable(Verifiables, Times.Once());

        var actual = Sut.OpenRead(Path);

        Assert.AreSame(stream, actual);
    }

    [TestMethod]
    public void OpenText()
    {
        using var stream = new MemoryStream();
        SutMock.Protected()
            .Setup<Stream>("Open", Path, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize, FileOptions.None)
            .Returns(stream)
            .Verifiable(Verifiables, Times.Once());

        using var actual = Sut.OpenText(Path);

        Assert.AreSame(stream, actual.BaseStream);
    }

    [TestMethod]
    public void OpenWrite()
    {
        using var stream = new MemoryStream();
        SutMock.Protected()
            .Setup<Stream>("Open", Path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, DefaultBufferSize, FileOptions.None)
            .Returns(stream)
            .Verifiable(Verifiables, Times.Once());

        var actual = Sut.OpenWrite(Path);

        Assert.AreSame(stream, actual);
    }

    [TestMethod]
    public void ReadAllBytes()
    {
        var expectedBytes = new byte[256];
        _rng.NextBytes(expectedBytes);
        using var stream = new MemoryStream(expectedBytes);
        SutMock.Protected()
            .Setup<Stream>("Open", Path, FileMode.Open, FileAccess.Read, FileShare.Read, 1, FileOptions.SequentialScan)
            .Returns(stream)
            .Verifiable(Verifiables, Times.Once());

        var actualBytes = Sut.ReadAllBytes(Path);

        Assert.AreCollectionsEqual(expectedBytes, actualBytes);
    }

    [TestMethod]
    public async Task ReadAllBytesAsync()
    {
        var expectedBytes = new byte[256];
        _rng.NextBytes(expectedBytes);
        using var stream = new MemoryStream(expectedBytes);
        SutMock.Protected()
            .Setup<Stream>("Open", Path, FileMode.Open, FileAccess.Read, FileShare.Read, 1, FileOptions.Asynchronous | FileOptions.SequentialScan)
            .Returns(stream)
            .Verifiable(Verifiables, Times.Once());

        var actualBytes = await Sut.ReadAllBytesAsync(Path);

        Assert.AreCollectionsEqual(expectedBytes, actualBytes);
    }

    [TestMethod]
    [DataRow(true, DisplayName = "Default Encoding")]
    [DataRow(false, DisplayName = "ASCII encoding")]
    public void ReadAllLines(bool useDefaultEncoding)
    {
        var encoding = useDefaultEncoding ? Encoding.Default : Encoding.ASCII;
        var expectedLines = new[] { "This is a test!!!", "Line 2", "Line 3" };
        using var stream = new MemoryStream(encoding.GetBytes(string.Join(Environment.NewLine, expectedLines)));
        SutMock.Protected()
            .Setup<Stream>("Open", Path, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize, FileOptions.None)
            .Returns(stream)
            .Verifiable(Verifiables, Times.Once());

        string[] actualLines;
        if (useDefaultEncoding)
            actualLines = Sut.ReadAllLines(Path);
        else
            actualLines = Sut.ReadAllLines(Path, encoding);

        Assert.AreCollectionsEqual(expectedLines, actualLines);
    }

    [TestMethod]
    [DataRow(true, DisplayName = "Default Encoding")]
    [DataRow(false, DisplayName = "ASCII encoding")]
    public async Task ReadAllLinesAsync(bool useDefaultEncoding)
    {
        var encoding = useDefaultEncoding ? Encoding.Default : Encoding.ASCII;
        var expectedLines = new[] { "This is a test!!!", "Line 2", "Line 3" };
        using var stream = new MemoryStream(encoding.GetBytes(string.Join(Environment.NewLine, expectedLines)));
        SutMock.Protected()
            .Setup<Stream>("Open", Path, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize, FileOptions.Asynchronous)
            .Returns(stream)
            .Verifiable(Verifiables, Times.Once());

        string[] actualLines;
        if (useDefaultEncoding)
            actualLines = await Sut.ReadAllLinesAsync(Path);
        else
            actualLines = await Sut.ReadAllLinesAsync(Path, encoding);

        Assert.AreCollectionsEqual(expectedLines, actualLines);
    }

    [TestMethod]
    [DataRow(true, DisplayName = "Default Encoding")]
    [DataRow(false, DisplayName = "ASCII encoding")]
    public void ReadAllText(bool useDefaultEncoding)
    {
        var encoding = useDefaultEncoding ? Encoding.Default : Encoding.ASCII;
        var expectedString = $"This is a test!!!{Environment.NewLine}Line 2{Environment.NewLine}Line 3";
        using var stream = new MemoryStream(encoding.GetBytes(string.Join(Environment.NewLine, expectedString)));
        SutMock.Protected()
            .Setup<Stream>("Open", Path, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize, FileOptions.None)
            .Returns(stream)
            .Verifiable(Verifiables, Times.Once());

        string actualString;
        if (useDefaultEncoding)
            actualString = Sut.ReadAllText(Path);
        else
            actualString = Sut.ReadAllText(Path, encoding);

        Assert.AreEqual(expectedString, actualString);
    }

    [TestMethod]
    [DataRow(true, DisplayName = "Default Encoding")]
    [DataRow(false, DisplayName = "ASCII encoding")]
    public async Task ReadAllTextAsync(bool useDefaultEncoding)
    {
        var encoding = useDefaultEncoding ? Encoding.Default : Encoding.ASCII;
        var expectedString = $"This is a test!!!{Environment.NewLine}Line 2{Environment.NewLine}Line 3";
        using var stream = new MemoryStream(encoding.GetBytes(string.Join(Environment.NewLine, expectedString)));
        SutMock.Protected()
            .Setup<Stream>("Open", Path, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize, FileOptions.Asynchronous)
            .Returns(stream)
            .Verifiable(Verifiables, Times.Once());

        string actualString;
        if (useDefaultEncoding)
            actualString = await Sut.ReadAllTextAsync(Path);
        else
            actualString = await Sut.ReadAllTextAsync(Path, encoding);

        Assert.AreEqual(expectedString, actualString);
    }

    [TestMethod]
    [DataRow(true, DisplayName = "Default Encoding")]
    [DataRow(false, DisplayName = "ASCII encoding")]
    public void ReadLines(bool useDefaultEncoding)
    {
        var encoding = useDefaultEncoding ? Encoding.Default : Encoding.ASCII;
        var expectedLines = new[] { "This is a test!!!", "Line 2", "Line 3" };
        using var stream = new MemoryStream(encoding.GetBytes(string.Join(Environment.NewLine, expectedLines)));
        SutMock.Protected()
            .Setup<Stream>("Open", Path, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize, FileOptions.None)
            .Returns(stream)
            .Verifiable(Verifiables, Times.Once());

        IEnumerable<string> actualLines;
        if (useDefaultEncoding)
            actualLines = Sut.ReadLines(Path);
        else
            actualLines = Sut.ReadLines(Path, encoding);

        Assert.AreCollectionsEqual(expectedLines, actualLines);
    }

#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
    [TestMethod]
    [DataRow(true, DisplayName = "Default Encoding")]
    [DataRow(false, DisplayName = "ASCII encoding")]
    public async Task ReadLinesAsync(bool useDefaultEncoding)
    {
        var encoding = useDefaultEncoding ? Encoding.Default : Encoding.ASCII;
        var expectedLines = new[] { "This is a test!!!", "Line 2", "Line 3" };
        using var stream = new MemoryStream(encoding.GetBytes(string.Join(Environment.NewLine, expectedLines)));
        SutMock.Protected()
            .Setup<Stream>("Open", Path, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize, FileOptions.Asynchronous)
            .Returns(stream)
            .Verifiable(Verifiables, Times.Once());

        IAsyncEnumerable<string> actualLinesAsync;
        if (useDefaultEncoding)
            actualLinesAsync = Sut.ReadLinesAsync(Path);
        else
            actualLinesAsync = Sut.ReadLinesAsync(Path, encoding);

        var actualLines = new List<string>();
        await foreach (var line in actualLinesAsync)
            actualLines.Add(line);

        Assert.AreCollectionsEqual(expectedLines, actualLines);
    }
#endif

    [TestMethod]
    public void Replace()
    {
        var sourceFileName = "C:\\Folder\\SubFolder\\file1.txt";
        var destFileName = "C:\\Folder\\SubFolder\\file2.txt";
        var backupFileName = "C:\\Folder\\SubFolder\\file3.txt";
        SutMock
            .Setup(x => x.Replace(sourceFileName, destFileName, backupFileName, false))
            .Verifiable(Verifiables, Times.Once());

        Sut.Replace(sourceFileName, destFileName, backupFileName);
    }

    [TestMethod]
    public void Replace_WithoutBackupFile()
    {
        var sourceFileName = "C:\\Folder\\SubFolder\\file1.txt";
        var destFileName = "C:\\Folder\\SubFolder\\file2.txt";
        SutMock
            .Setup(x => x.Delete(destFileName))
            .Verifiable(Verifiables, Times.Once());
        SutMock
            .Setup(x => x.Move(sourceFileName, destFileName))
            .Verifiable(Verifiables, Times.Once());

        Sut.Replace(sourceFileName, destFileName, null, false);
    }

    [TestMethod]
    public void Replace_WithBackupFile()
    {
        var sourceFileName = "C:\\Folder\\SubFolder\\file1.txt";
        var destFileName = "C:\\Folder\\SubFolder\\file2.txt";
        var backupFileName = "C:\\Folder\\SubFolder\\file3.txt";
        SutMock
            .Setup(x => x.Move(destFileName, backupFileName))
            .Verifiable(Verifiables, Times.Once());
        SutMock
            .Setup(x => x.Move(sourceFileName, destFileName))
            .Verifiable(Verifiables, Times.Once());

        Sut.Replace(sourceFileName, destFileName, backupFileName, false);
    }

    [TestMethod]
    public void WriteAllBytes()
    {
        var expectedBytes = new byte[256];
        _rng.NextBytes(expectedBytes);
        using var stream = new MemoryStream();
        SutMock.Protected()
            .Setup<Stream>("Open", Path, FileMode.Create, FileAccess.Write, FileShare.Read, DefaultBufferSize, FileOptions.None)
            .Returns(stream)
            .Verifiable(Verifiables, Times.Once());

        Sut.WriteAllBytes(Path, expectedBytes);

        Assert.AreCollectionsEqual(expectedBytes, stream.ToArray());
    }

    [TestMethod]
    public async Task WriteAllBytesAsync()
    {
        var expectedBytes = new byte[256];
        _rng.NextBytes(expectedBytes);
        using var stream = new MemoryStream();
        SutMock.Protected()
            .Setup<Stream>("Open", Path, FileMode.Create, FileAccess.Write, FileShare.Read, DefaultBufferSize, FileOptions.Asynchronous)
            .Returns(stream)
            .Verifiable(Verifiables, Times.Once());

        await Sut.WriteAllBytesAsync(Path, expectedBytes);

        Assert.AreCollectionsEqual(expectedBytes, stream.ToArray());
    }

    [TestMethod]
    [DataRow(false, true, DisplayName = "Array with default encoding.")]
    [DataRow(false, false, DisplayName = "Array with ASCII encoding.")]
    [DataRow(true, true, DisplayName = "IEnumerable with default encoding.")]
    [DataRow(true, false, DisplayName = "IEnumerable with ASCII encoding.")]
    public void WriteAllLines(bool contentAsEnumerable, bool useDefaultEncoding)
    {
        var encoding = useDefaultEncoding ? Encoding.Default : Encoding.ASCII;
        var expectedLines = new[] { "This is a test!!!", "Line 2", "Line 3" };
        using var stream = new MemoryStream();
        SutMock.Protected()
            .Setup<Stream>("Open", Path, FileMode.Create, FileAccess.Write, FileShare.Read, DefaultBufferSize, FileOptions.None)
            .Returns(stream)
            .Verifiable(Verifiables, Times.Once());

        Action<FileServiceBase, string, string[], Encoding> action = (contentAsEnumerable, useDefaultEncoding) switch
        {
            (false, false) => (f, p, c, e) => f.WriteAllLines(p, c, e),
            (false, true) => (f, p, c, e) => f.WriteAllLines(p, c),
            (true, false) => (f, p, c, e) => f.WriteAllLines(p, (IEnumerable<string>)c, e),
            (true, true) => (f, p, c, e) => f.WriteAllLines(p, (IEnumerable<string>)c),
        };
        action(Sut, Path, expectedLines, encoding);

        var expectedString = string.Join(Environment.NewLine, expectedLines) + Environment.NewLine;
        var actualString = encoding.GetString(stream.ToArray());

        Assert.AreEqual(expectedString, actualString);
    }

    [TestMethod]
    [DataRow(true, DisplayName = "Default encoding.")]
    [DataRow(false, DisplayName = "ASCII encoding.")]
    public async Task WriteAllLinesAsync(bool useDefaultEncoding)
    {
        var encoding = useDefaultEncoding ? Encoding.Default : Encoding.ASCII;
        var expectedLines = new[] { "This is a test!!!", "Line 2", "Line 3" };
        using var stream = new MemoryStream();
        SutMock.Protected()
            .Setup<Stream>("Open", Path, FileMode.Create, FileAccess.Write, FileShare.Read, DefaultBufferSize, FileOptions.Asynchronous)
            .Returns(stream)
            .Verifiable(Verifiables, Times.Once());

        if (useDefaultEncoding)
            await Sut.WriteAllLinesAsync(Path, expectedLines);
        else
            await Sut.WriteAllLinesAsync(Path, expectedLines, encoding);

        var expectedString = string.Join(Environment.NewLine, expectedLines) + Environment.NewLine;
        var actualString = encoding.GetString(stream.ToArray());

        Assert.AreEqual(expectedString, actualString);
    }

#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
    [TestMethod]
    [DataRow(true, DisplayName = "Default encoding.")]
    [DataRow(false, DisplayName = "ASCII encoding.")]
    public async Task WriteAllLinesAsync_AsyncContent(bool useDefaultEncoding)
    {
        var encoding = useDefaultEncoding ? Encoding.Default : Encoding.ASCII;
        var expectedLines = new[] { "This is a test!!!", "Line 2", "Line 3" };
        using var stream = new MemoryStream();
        SutMock.Protected()
            .Setup<Stream>("Open", Path, FileMode.Create, FileAccess.Write, FileShare.Read, DefaultBufferSize, FileOptions.Asynchronous)
            .Returns(stream)
            .Verifiable(Verifiables, Times.Once());

        if (useDefaultEncoding)
            await Sut.WriteAllLinesAsync(Path, AsAsyncEnum(expectedLines));
        else
            await Sut.WriteAllLinesAsync(Path, AsAsyncEnum(expectedLines), encoding);

        var expectedString = string.Join(Environment.NewLine, expectedLines) + Environment.NewLine;
        var actualString = encoding.GetString(stream.ToArray());

        Assert.AreEqual(expectedString, actualString);

        static async IAsyncEnumerable<string> AsAsyncEnum(IEnumerable<string> enumerable)
        {
            foreach (var item in enumerable)
                yield return await Task.FromResult(item);
        }
    }
#endif

    [TestMethod]
    [DataRow(true, DisplayName = "Default encoding.")]
    [DataRow(false, DisplayName = "ASCII encoding.")]
    public void WriteAllText(bool useDefaultEncoding)
    {
        var encoding = useDefaultEncoding ? Encoding.Default : Encoding.ASCII;
        var expectedString = $"This is a test!!!{Environment.NewLine}Line 2{Environment.NewLine}Line 3";
        using var stream = new MemoryStream();
        SutMock.Protected()
            .Setup<Stream>("Open", Path, FileMode.Create, FileAccess.Write, FileShare.Read, DefaultBufferSize, FileOptions.None)
            .Returns(stream)
            .Verifiable(Verifiables, Times.Once());

        if (useDefaultEncoding)
            Sut.WriteAllText(Path, expectedString);
        else
            Sut.WriteAllText(Path, expectedString, encoding);

        var actualString = encoding.GetString(stream.ToArray());

        Assert.AreEqual(expectedString, actualString);
    }

    [TestMethod]
    [DataRow(true, DisplayName = "Default encoding.")]
    [DataRow(false, DisplayName = "ASCII encoding.")]
    public async Task WriteAllTextAsync(bool useDefaultEncoding)
    {
        var encoding = useDefaultEncoding ? Encoding.Default : Encoding.ASCII;
        var expectedString = $"This is a test!!!{Environment.NewLine}Line 2{Environment.NewLine}Line 3";
        using var stream = new MemoryStream();
        SutMock.Protected()
            .Setup<Stream>("Open", Path, FileMode.Create, FileAccess.Write, FileShare.Read, DefaultBufferSize, FileOptions.Asynchronous)
            .Returns(stream)
            .Verifiable(Verifiables, Times.Once());

        if (useDefaultEncoding)
            await Sut.WriteAllTextAsync(Path, expectedString);
        else
            await Sut.WriteAllTextAsync(Path, expectedString, encoding);

        var actualString = encoding.GetString(stream.ToArray());

        Assert.AreEqual(expectedString, actualString);
    }

    protected override void OnInitializeTest()
    {
        base.OnInitializeTest();
        SutMock = CreateService(out var ds);
        DirectoryService = ds;
    }

    private Mock<FileServiceBase> CreateService(out Mock<IDirectoryService> dirService)
    {
        var localDirService = dirService = Mocks.Create<IDirectoryService>();
        var fileSystemService = Mocks.Create<IFileSystemService>();
        var fileService = Mocks.Create<FileServiceBase>(MockBehavior.Loose, fileSystemService.Object);
        fileService.CallBase = true;

        fileSystemService.Setup(x => x.Directory).Returns(localDirService.Object);
        fileSystemService.Setup(x => x.File).Returns(fileService.Object);
        fileSystemService.Setup(x => x.GetDirectoryInfo(It.IsAny<string>())).Returns<string>(x => localDirService.Object.GetInfo(x));
        fileSystemService.Setup(x => x.GetFileInfo(It.IsAny<string>())).Returns<string>(x => fileService.Object.GetInfo(x));

        return fileService;
    }
}
