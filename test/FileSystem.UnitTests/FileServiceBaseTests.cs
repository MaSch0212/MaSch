using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSch.FileSystem.UnitTests;

[TestClass]
public class FileServiceBaseTests : TestClassBase
{
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
