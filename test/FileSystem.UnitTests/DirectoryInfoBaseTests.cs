namespace MaSch.FileSystem.UnitTests;

[TestClass]
public class DirectoryInfoBaseTests : TestClassBase
{
    private const string Path = "C:\\Folder\\SubFolder";
    private DirectoryInfoBase Sut { get; set; } = null!;
    private Mock<IDirectoryService> DirectoryService { get; set; } = null!;
    private Mock<IFileService> FileService { get; set; } = null!;

    [TestMethod]
    public void Parent()
    {
        var expected = Mocks.Create<IDirectoryInfo>();
        DirectoryService.Setup(x => x.GetParent(Path)).Returns(expected.Object).Verifiable(Verifiables, Times.Once());

        var actual = Sut.Parent;

        Assert.AreEqual(expected.Object, actual);
    }

    [TestMethod]
    public void Root()
    {
        var expected = Mocks.Create<IDirectoryInfo>();
        DirectoryService.Setup(x => x.GetDirectoryRoot(Path)).Returns("C:\\").Verifiable(Verifiables, Times.Once());
        DirectoryService.Setup(x => x.GetInfo("C:\\")).Returns(expected.Object).Verifiable(Verifiables, Times.Once());

        var actual = Sut.Root;

        Assert.AreEqual(expected.Object, actual);
    }

    [TestMethod]
    public void Exists()
    {
        DirectoryService.Setup(x => x.Exists(Path)).Returns(true).Verifiable(Verifiables, Times.Once());

        var actual = Sut.Exists;

        Assert.IsTrue(actual);
    }

    [TestMethod]
    public void CreationTimeUtc_Getter()
    {
        var expected = DateTime.UtcNow;
        DirectoryService.Setup(x => x.GetCreationTimeUtc(Path)).Returns(expected).Verifiable(Verifiables, Times.Once());

        var actual = Sut.CreationTimeUtc;

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void CreationTimeUtc_Setter()
    {
        var expected = DateTime.UtcNow;
        DirectoryService.Setup(x => x.SetCreationTimeUtc(Path, expected)).Verifiable(Verifiables, Times.Once());

        Sut.CreationTimeUtc = expected;
    }

    [TestMethod]
    public void CreationTime_Getter()
    {
        var expected = DateTime.Now;
        DirectoryService.Setup(x => x.GetCreationTime(Path)).Returns(expected).Verifiable(Verifiables, Times.Once());

        var actual = Sut.CreationTime;

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void CreationTime_Setter()
    {
        var expected = DateTime.Now;
        DirectoryService.Setup(x => x.SetCreationTime(Path, expected)).Verifiable(Verifiables, Times.Once());

        Sut.CreationTime = expected;
    }

    [TestMethod]
    public void LastAccessTimeUtc_Getter()
    {
        var expected = DateTime.UtcNow;
        DirectoryService.Setup(x => x.GetLastAccessTimeUtc(Path)).Returns(expected).Verifiable(Verifiables, Times.Once());

        var actual = Sut.LastAccessTimeUtc;

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void LastAccessTimeUtc_Setter()
    {
        var expected = DateTime.UtcNow;
        DirectoryService.Setup(x => x.SetLastAccessTimeUtc(Path, expected)).Verifiable(Verifiables, Times.Once());

        Sut.LastAccessTimeUtc = expected;
    }

    [TestMethod]
    public void LastAccessTime_Getter()
    {
        var expected = DateTime.Now;
        DirectoryService.Setup(x => x.GetLastAccessTime(Path)).Returns(expected).Verifiable(Verifiables, Times.Once());

        var actual = Sut.LastAccessTime;

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void LastAccessTime_Setter()
    {
        var expected = DateTime.Now;
        DirectoryService.Setup(x => x.SetLastAccessTime(Path, expected)).Verifiable(Verifiables, Times.Once());

        Sut.LastAccessTime = expected;
    }

    [TestMethod]
    public void LastWriteTimeUtc_Getter()
    {
        var expected = DateTime.UtcNow;
        DirectoryService.Setup(x => x.GetLastWriteTimeUtc(Path)).Returns(expected).Verifiable(Verifiables, Times.Once());

        var actual = Sut.LastWriteTimeUtc;

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void LastWriteTimeUtc_Setter()
    {
        var expected = DateTime.UtcNow;
        DirectoryService.Setup(x => x.SetLastWriteTimeUtc(Path, expected)).Verifiable(Verifiables, Times.Once());

        Sut.LastWriteTimeUtc = expected;
    }

    [TestMethod]
    public void LastWriteTime_Getter()
    {
        var expected = DateTime.Now;
        DirectoryService.Setup(x => x.GetLastWriteTime(Path)).Returns(expected).Verifiable(Verifiables, Times.Once());

        var actual = Sut.LastWriteTime;

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void LastWriteTime_Setter()
    {
        var expected = DateTime.Now;
        DirectoryService.Setup(x => x.SetLastWriteTime(Path, expected)).Verifiable(Verifiables, Times.Once());

        Sut.LastWriteTime = expected;
    }

    [TestMethod]
    public void Create()
    {
        DirectoryService.Setup(x => x.CreateDirectory(Path)).Returns((IDirectoryInfo)null!).Verifiable(Verifiables, Times.Once());

        Sut.Create();
    }

    [TestMethod]
    public void CreateSubdirectory_ParameterChecks()
    {
        DirectoryService.Setup(x => x.GetInfo(It.IsAny<string>())).Returns((IDirectoryInfo)null!).Verifiable(Verifiables, Times.Never());

        Assert.ThrowsException<ArgumentNullException>(() => Sut.CreateSubdirectory(null!));
        Assert.ThrowsException<ArgumentException>(() => Sut.CreateSubdirectory(string.Empty));
        Assert.ThrowsException<ArgumentException>(() => Sut.CreateSubdirectory("C:\\test"));
    }

    [TestMethod]
    public void CreateSubdirectory_Success()
    {
        var expected = Mocks.Create<IDirectoryInfo>().Object;
        DirectoryService.Setup(x => x.GetInfo(System.IO.Path.Combine(Path, "abc"))).Returns(expected).Verifiable(Verifiables, Times.Once());

        var actual = Sut.CreateSubdirectory("abc");

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Delete()
    {
        DirectoryService.Setup(x => x.Delete(Path)).Verifiable(Verifiables, Times.Once());
        Sut.Delete();
    }

    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void Delete_Recursive(bool recursive)
    {
        DirectoryService.Setup(x => x.Delete(Path, recursive)).Verifiable(Verifiables, Times.Once());
        Sut.Delete(recursive);
    }

    [TestMethod]
    public void EnumerateDirectories()
    {
        var expected = new Dictionary<string, IDirectoryInfo>()
        {
            [System.IO.Path.Combine(Path, "abc")] = Mocks.Create<IDirectoryInfo>().Object,
            [System.IO.Path.Combine(Path, "def")] = Mocks.Create<IDirectoryInfo>().Object,
        };
        DirectoryService.Setup(x => x.EnumerateDirectories(Path)).Returns(expected.Keys).Verifiable(Verifiables, Times.Once());
        foreach (var item in expected)
            DirectoryService.Setup(x => x.GetInfo(item.Key)).Returns(item.Value).Verifiable(Verifiables, Times.Once());

        var result = Sut.EnumerateDirectories();

        Assert.AreCollectionsEqual(expected.Values, result);
    }

    [TestMethod]
    public void EnumerateDirectories_SearchPattern()
    {
        var searchPattern = Guid.NewGuid().ToString();
        var expected = new Dictionary<string, IDirectoryInfo>()
        {
            [System.IO.Path.Combine(Path, "abc")] = Mocks.Create<IDirectoryInfo>().Object,
            [System.IO.Path.Combine(Path, "def")] = Mocks.Create<IDirectoryInfo>().Object,
        };
        DirectoryService.Setup(x => x.EnumerateDirectories(Path, searchPattern)).Returns(expected.Keys).Verifiable(Verifiables, Times.Once());
        foreach (var item in expected)
            DirectoryService.Setup(x => x.GetInfo(item.Key)).Returns(item.Value).Verifiable(Verifiables, Times.Once());

        var result = Sut.EnumerateDirectories(searchPattern);

        Assert.AreCollectionsEqual(expected.Values, result);
    }

    [TestMethod]
    [DataRow(SearchOption.AllDirectories)]
    [DataRow(SearchOption.TopDirectoryOnly)]
    public void EnumerateDirectories_SearchPattern_SearchOption(SearchOption option)
    {
        var searchPattern = Guid.NewGuid().ToString();
        var expected = new Dictionary<string, IDirectoryInfo>()
        {
            [System.IO.Path.Combine(Path, "abc")] = Mocks.Create<IDirectoryInfo>().Object,
            [System.IO.Path.Combine(Path, "def")] = Mocks.Create<IDirectoryInfo>().Object,
        };
        DirectoryService.Setup(x => x.EnumerateDirectories(Path, searchPattern, option)).Returns(expected.Keys).Verifiable(Verifiables, Times.Once());
        foreach (var item in expected)
            DirectoryService.Setup(x => x.GetInfo(item.Key)).Returns(item.Value).Verifiable(Verifiables, Times.Once());

        var result = Sut.EnumerateDirectories(searchPattern, option);

        Assert.AreCollectionsEqual(expected.Values, result);
    }

    [TestMethod]
    public void EnumerateFiles()
    {
        var expected = new Dictionary<string, IFileInfo>()
        {
            [System.IO.Path.Combine(Path, "abc")] = Mocks.Create<IFileInfo>().Object,
            [System.IO.Path.Combine(Path, "def")] = Mocks.Create<IFileInfo>().Object,
        };
        DirectoryService.Setup(x => x.EnumerateFiles(Path)).Returns(expected.Keys).Verifiable(Verifiables, Times.Once());
        foreach (var item in expected)
            FileService.Setup(x => x.GetInfo(item.Key)).Returns(item.Value).Verifiable(Verifiables, Times.Once());

        var result = Sut.EnumerateFiles();

        Assert.AreCollectionsEqual(expected.Values, result);
    }

    [TestMethod]
    public void EnumerateFiles_SearchPattern()
    {
        var searchPattern = Guid.NewGuid().ToString();
        var expected = new Dictionary<string, IFileInfo>()
        {
            [System.IO.Path.Combine(Path, "abc")] = Mocks.Create<IFileInfo>().Object,
            [System.IO.Path.Combine(Path, "def")] = Mocks.Create<IFileInfo>().Object,
        };
        DirectoryService.Setup(x => x.EnumerateFiles(Path, searchPattern)).Returns(expected.Keys).Verifiable(Verifiables, Times.Once());
        foreach (var item in expected)
            FileService.Setup(x => x.GetInfo(item.Key)).Returns(item.Value).Verifiable(Verifiables, Times.Once());

        var result = Sut.EnumerateFiles(searchPattern);

        Assert.AreCollectionsEqual(expected.Values, result);
    }

    [TestMethod]
    [DataRow(SearchOption.AllDirectories)]
    [DataRow(SearchOption.TopDirectoryOnly)]
    public void EnumerateFiles_SearchPattern_SearchOption(SearchOption option)
    {
        var searchPattern = Guid.NewGuid().ToString();
        var expected = new Dictionary<string, IFileInfo>()
        {
            [System.IO.Path.Combine(Path, "abc")] = Mocks.Create<IFileInfo>().Object,
            [System.IO.Path.Combine(Path, "def")] = Mocks.Create<IFileInfo>().Object,
        };
        DirectoryService.Setup(x => x.EnumerateFiles(Path, searchPattern, option)).Returns(expected.Keys).Verifiable(Verifiables, Times.Once());
        foreach (var item in expected)
            FileService.Setup(x => x.GetInfo(item.Key)).Returns(item.Value).Verifiable(Verifiables, Times.Once());

        var result = Sut.EnumerateFiles(searchPattern, option);

        Assert.AreCollectionsEqual(expected.Values, result);
    }

    [TestMethod]
    public void EnumerateFileSystemInfos()
    {
        var expected = new Dictionary<string, IFileSystemInfo>()
        {
            [System.IO.Path.Combine(Path, "abc")] = Mocks.Create<IFileInfo>().Object,
            [System.IO.Path.Combine(Path, "def")] = Mocks.Create<IDirectoryInfo>().Object,
        };
        var expectedArray = expected.ToArray();
        DirectoryService.Setup(x => x.EnumerateFileSystemEntries(Path)).Returns(expected.Keys).Verifiable(Verifiables, Times.Once());
        DirectoryService.Setup(x => x.Exists(expectedArray[0].Key)).Returns(false).Verifiable(Verifiables, Times.Once());
        DirectoryService.Setup(x => x.Exists(expectedArray[1].Key)).Returns(true).Verifiable(Verifiables, Times.Once());
        FileService.Setup(x => x.GetInfo(expectedArray[0].Key)).Returns((IFileInfo)expectedArray[0].Value).Verifiable(Verifiables, Times.Once());
        DirectoryService.Setup(x => x.GetInfo(expectedArray[1].Key)).Returns((IDirectoryInfo)expectedArray[1].Value).Verifiable(Verifiables, Times.Once());

        var result = Sut.EnumerateFileSystemInfos();

        Assert.AreCollectionsEqual(expected.Values, result);
    }

    [TestMethod]
    public void EnumerateFileSystemInfos_SearchPattern()
    {
        var searchPattern = Guid.NewGuid().ToString();
        var expected = new Dictionary<string, IFileSystemInfo>()
        {
            [System.IO.Path.Combine(Path, "abc")] = Mocks.Create<IFileInfo>().Object,
            [System.IO.Path.Combine(Path, "def")] = Mocks.Create<IDirectoryInfo>().Object,
        };
        var expectedArray = expected.ToArray();
        DirectoryService.Setup(x => x.EnumerateFileSystemEntries(Path, searchPattern)).Returns(expected.Keys).Verifiable(Verifiables, Times.Once());
        DirectoryService.Setup(x => x.Exists(expectedArray[0].Key)).Returns(false).Verifiable(Verifiables, Times.Once());
        DirectoryService.Setup(x => x.Exists(expectedArray[1].Key)).Returns(true).Verifiable(Verifiables, Times.Once());
        FileService.Setup(x => x.GetInfo(expectedArray[0].Key)).Returns((IFileInfo)expectedArray[0].Value).Verifiable(Verifiables, Times.Once());
        DirectoryService.Setup(x => x.GetInfo(expectedArray[1].Key)).Returns((IDirectoryInfo)expectedArray[1].Value).Verifiable(Verifiables, Times.Once());

        var result = Sut.EnumerateFileSystemInfos(searchPattern);

        Assert.AreCollectionsEqual(expected.Values, result);
    }

    [TestMethod]
    [DataRow(SearchOption.AllDirectories)]
    [DataRow(SearchOption.TopDirectoryOnly)]
    public void EnumerateFileSystemInfos_SearchPattern_SearchOption(SearchOption option)
    {
        var searchPattern = Guid.NewGuid().ToString();
        var expected = new Dictionary<string, IFileSystemInfo>()
        {
            [System.IO.Path.Combine(Path, "abc")] = Mocks.Create<IFileInfo>().Object,
            [System.IO.Path.Combine(Path, "def")] = Mocks.Create<IDirectoryInfo>().Object,
        };
        var expectedArray = expected.ToArray();
        DirectoryService.Setup(x => x.EnumerateFileSystemEntries(Path, searchPattern, option)).Returns(expected.Keys).Verifiable(Verifiables, Times.Once());
        DirectoryService.Setup(x => x.Exists(expectedArray[0].Key)).Returns(false).Verifiable(Verifiables, Times.Once());
        DirectoryService.Setup(x => x.Exists(expectedArray[1].Key)).Returns(true).Verifiable(Verifiables, Times.Once());
        FileService.Setup(x => x.GetInfo(expectedArray[0].Key)).Returns((IFileInfo)expectedArray[0].Value).Verifiable(Verifiables, Times.Once());
        DirectoryService.Setup(x => x.GetInfo(expectedArray[1].Key)).Returns((IDirectoryInfo)expectedArray[1].Value).Verifiable(Verifiables, Times.Once());

        var result = Sut.EnumerateFileSystemInfos(searchPattern, option);

        Assert.AreCollectionsEqual(expected.Values, result);
    }

    [TestMethod]
    public void GetDirectories()
    {
        var expected = new Dictionary<string, IDirectoryInfo>()
        {
            [System.IO.Path.Combine(Path, "abc")] = Mocks.Create<IDirectoryInfo>().Object,
            [System.IO.Path.Combine(Path, "def")] = Mocks.Create<IDirectoryInfo>().Object,
        };
        DirectoryService.Setup(x => x.GetDirectories(Path)).Returns(expected.Keys.ToArray()).Verifiable(Verifiables, Times.Once());
        foreach (var item in expected)
            DirectoryService.Setup(x => x.GetInfo(item.Key)).Returns(item.Value).Verifiable(Verifiables, Times.Once());

        var result = Sut.GetDirectories();

        Assert.AreCollectionsEqual(expected.Values, result);
    }

    [TestMethod]
    public void GetDirectories_SearchPattern()
    {
        var searchPattern = Guid.NewGuid().ToString();
        var expected = new Dictionary<string, IDirectoryInfo>()
        {
            [System.IO.Path.Combine(Path, "abc")] = Mocks.Create<IDirectoryInfo>().Object,
            [System.IO.Path.Combine(Path, "def")] = Mocks.Create<IDirectoryInfo>().Object,
        };
        DirectoryService.Setup(x => x.GetDirectories(Path, searchPattern)).Returns(expected.Keys.ToArray()).Verifiable(Verifiables, Times.Once());
        foreach (var item in expected)
            DirectoryService.Setup(x => x.GetInfo(item.Key)).Returns(item.Value).Verifiable(Verifiables, Times.Once());

        var result = Sut.GetDirectories(searchPattern);

        Assert.AreCollectionsEqual(expected.Values, result);
    }

    [TestMethod]
    [DataRow(SearchOption.AllDirectories)]
    [DataRow(SearchOption.TopDirectoryOnly)]
    public void GetDirectories_SearchPattern_SearchOption(SearchOption option)
    {
        var searchPattern = Guid.NewGuid().ToString();
        var expected = new Dictionary<string, IDirectoryInfo>()
        {
            [System.IO.Path.Combine(Path, "abc")] = Mocks.Create<IDirectoryInfo>().Object,
            [System.IO.Path.Combine(Path, "def")] = Mocks.Create<IDirectoryInfo>().Object,
        };
        DirectoryService.Setup(x => x.GetDirectories(Path, searchPattern, option)).Returns(expected.Keys.ToArray()).Verifiable(Verifiables, Times.Once());
        foreach (var item in expected)
            DirectoryService.Setup(x => x.GetInfo(item.Key)).Returns(item.Value).Verifiable(Verifiables, Times.Once());

        var result = Sut.GetDirectories(searchPattern, option);

        Assert.AreCollectionsEqual(expected.Values, result);
    }

    [TestMethod]
    public void GetFiles()
    {
        var expected = new Dictionary<string, IFileInfo>()
        {
            [System.IO.Path.Combine(Path, "abc")] = Mocks.Create<IFileInfo>().Object,
            [System.IO.Path.Combine(Path, "def")] = Mocks.Create<IFileInfo>().Object,
        };
        DirectoryService.Setup(x => x.GetFiles(Path)).Returns(expected.Keys.ToArray()).Verifiable(Verifiables, Times.Once());
        foreach (var item in expected)
            FileService.Setup(x => x.GetInfo(item.Key)).Returns(item.Value).Verifiable(Verifiables, Times.Once());

        var result = Sut.GetFiles();

        Assert.AreCollectionsEqual(expected.Values, result);
    }

    [TestMethod]
    public void GetFiles_SearchPattern()
    {
        var searchPattern = Guid.NewGuid().ToString();
        var expected = new Dictionary<string, IFileInfo>()
        {
            [System.IO.Path.Combine(Path, "abc")] = Mocks.Create<IFileInfo>().Object,
            [System.IO.Path.Combine(Path, "def")] = Mocks.Create<IFileInfo>().Object,
        };
        DirectoryService.Setup(x => x.GetFiles(Path, searchPattern)).Returns(expected.Keys.ToArray()).Verifiable(Verifiables, Times.Once());
        foreach (var item in expected)
            FileService.Setup(x => x.GetInfo(item.Key)).Returns(item.Value).Verifiable(Verifiables, Times.Once());

        var result = Sut.GetFiles(searchPattern);

        Assert.AreCollectionsEqual(expected.Values, result);
    }

    [TestMethod]
    [DataRow(SearchOption.AllDirectories)]
    [DataRow(SearchOption.TopDirectoryOnly)]
    public void GetFiles_SearchPattern_SearchOption(SearchOption option)
    {
        var searchPattern = Guid.NewGuid().ToString();
        var expected = new Dictionary<string, IFileInfo>()
        {
            [System.IO.Path.Combine(Path, "abc")] = Mocks.Create<IFileInfo>().Object,
            [System.IO.Path.Combine(Path, "def")] = Mocks.Create<IFileInfo>().Object,
        };
        DirectoryService.Setup(x => x.GetFiles(Path, searchPattern, option)).Returns(expected.Keys.ToArray()).Verifiable(Verifiables, Times.Once());
        foreach (var item in expected)
            FileService.Setup(x => x.GetInfo(item.Key)).Returns(item.Value).Verifiable(Verifiables, Times.Once());

        var result = Sut.GetFiles(searchPattern, option);

        Assert.AreCollectionsEqual(expected.Values, result);
    }

    [TestMethod]
    public void GetFileSystemInfos()
    {
        var expected = new Dictionary<string, IFileSystemInfo>()
        {
            [System.IO.Path.Combine(Path, "abc")] = Mocks.Create<IFileInfo>().Object,
            [System.IO.Path.Combine(Path, "def")] = Mocks.Create<IDirectoryInfo>().Object,
        };
        var expectedArray = expected.ToArray();
        DirectoryService.Setup(x => x.GetFileSystemEntries(Path)).Returns(expected.Keys.ToArray()).Verifiable(Verifiables, Times.Once());
        DirectoryService.Setup(x => x.Exists(expectedArray[0].Key)).Returns(false).Verifiable(Verifiables, Times.Once());
        DirectoryService.Setup(x => x.Exists(expectedArray[1].Key)).Returns(true).Verifiable(Verifiables, Times.Once());
        FileService.Setup(x => x.GetInfo(expectedArray[0].Key)).Returns((IFileInfo)expectedArray[0].Value).Verifiable(Verifiables, Times.Once());
        DirectoryService.Setup(x => x.GetInfo(expectedArray[1].Key)).Returns((IDirectoryInfo)expectedArray[1].Value).Verifiable(Verifiables, Times.Once());

        var result = Sut.GetFileSystemInfos();

        Assert.AreCollectionsEqual(expected.Values, result);
    }

    [TestMethod]
    public void GetFileSystemInfos_SearchPattern()
    {
        var searchPattern = Guid.NewGuid().ToString();
        var expected = new Dictionary<string, IFileSystemInfo>()
        {
            [System.IO.Path.Combine(Path, "abc")] = Mocks.Create<IFileInfo>().Object,
            [System.IO.Path.Combine(Path, "def")] = Mocks.Create<IDirectoryInfo>().Object,
        };
        var expectedArray = expected.ToArray();
        DirectoryService.Setup(x => x.GetFileSystemEntries(Path, searchPattern)).Returns(expected.Keys.ToArray()).Verifiable(Verifiables, Times.Once());
        DirectoryService.Setup(x => x.Exists(expectedArray[0].Key)).Returns(false).Verifiable(Verifiables, Times.Once());
        DirectoryService.Setup(x => x.Exists(expectedArray[1].Key)).Returns(true).Verifiable(Verifiables, Times.Once());
        FileService.Setup(x => x.GetInfo(expectedArray[0].Key)).Returns((IFileInfo)expectedArray[0].Value).Verifiable(Verifiables, Times.Once());
        DirectoryService.Setup(x => x.GetInfo(expectedArray[1].Key)).Returns((IDirectoryInfo)expectedArray[1].Value).Verifiable(Verifiables, Times.Once());

        var result = Sut.GetFileSystemInfos(searchPattern);

        Assert.AreCollectionsEqual(expected.Values, result);
    }

    [TestMethod]
    [DataRow(SearchOption.AllDirectories)]
    [DataRow(SearchOption.TopDirectoryOnly)]
    public void GetFileSystemInfos_SearchPattern_SearchOption(SearchOption option)
    {
        var searchPattern = Guid.NewGuid().ToString();
        var expected = new Dictionary<string, IFileSystemInfo>()
        {
            [System.IO.Path.Combine(Path, "abc")] = Mocks.Create<IFileInfo>().Object,
            [System.IO.Path.Combine(Path, "def")] = Mocks.Create<IDirectoryInfo>().Object,
        };
        var expectedArray = expected.ToArray();
        DirectoryService.Setup(x => x.GetFileSystemEntries(Path, searchPattern, option)).Returns(expected.Keys.ToArray()).Verifiable(Verifiables, Times.Once());
        DirectoryService.Setup(x => x.Exists(expectedArray[0].Key)).Returns(false).Verifiable(Verifiables, Times.Once());
        DirectoryService.Setup(x => x.Exists(expectedArray[1].Key)).Returns(true).Verifiable(Verifiables, Times.Once());
        FileService.Setup(x => x.GetInfo(expectedArray[0].Key)).Returns((IFileInfo)expectedArray[0].Value).Verifiable(Verifiables, Times.Once());
        DirectoryService.Setup(x => x.GetInfo(expectedArray[1].Key)).Returns((IDirectoryInfo)expectedArray[1].Value).Verifiable(Verifiables, Times.Once());

        var result = Sut.GetFileSystemInfos(searchPattern, option);

        Assert.AreCollectionsEqual(expected.Values, result);
    }

    [TestMethod]
    public void MoveTo()
    {
        var destDirName = System.IO.Path.Combine("C:\\", Guid.NewGuid().ToString());
        DirectoryService.Setup(x => x.Move(Path, destDirName)).Verifiable(Verifiables, Times.Once());
        Sut.MoveTo(destDirName);
    }

#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
    [TestMethod]
    public void EnumerateDirectories_SearchPattern_EnumerationOptions()
    {
        var searchPattern = Guid.NewGuid().ToString();
        var enumerationOptions = new EnumerationOptions();
        var expected = new Dictionary<string, IDirectoryInfo>()
        {
            [System.IO.Path.Combine(Path, "abc")] = Mocks.Create<IDirectoryInfo>().Object,
            [System.IO.Path.Combine(Path, "def")] = Mocks.Create<IDirectoryInfo>().Object,
        };
        DirectoryService.Setup(x => x.EnumerateDirectories(Path, searchPattern, enumerationOptions)).Returns(expected.Keys).Verifiable(Verifiables, Times.Once());
        foreach (var item in expected)
            DirectoryService.Setup(x => x.GetInfo(item.Key)).Returns(item.Value).Verifiable(Verifiables, Times.Once());

        var result = Sut.EnumerateDirectories(searchPattern, enumerationOptions);

        Assert.AreCollectionsEqual(expected.Values, result);
    }

    [TestMethod]
    public void EnumerateFiles_SearchPattern_EnumerationOptions()
    {
        var searchPattern = Guid.NewGuid().ToString();
        var enumerationOptions = new EnumerationOptions();
        var expected = new Dictionary<string, IFileInfo>()
        {
            [System.IO.Path.Combine(Path, "abc")] = Mocks.Create<IFileInfo>().Object,
            [System.IO.Path.Combine(Path, "def")] = Mocks.Create<IFileInfo>().Object,
        };
        DirectoryService.Setup(x => x.EnumerateFiles(Path, searchPattern, enumerationOptions)).Returns(expected.Keys).Verifiable(Verifiables, Times.Once());
        foreach (var item in expected)
            FileService.Setup(x => x.GetInfo(item.Key)).Returns(item.Value).Verifiable(Verifiables, Times.Once());

        var result = Sut.EnumerateFiles(searchPattern, enumerationOptions);

        Assert.AreCollectionsEqual(expected.Values, result);
    }

    [TestMethod]
    public void EnumerateFileSystemInfos_SearchPattern_EnumerationOptions()
    {
        var searchPattern = Guid.NewGuid().ToString();
        var enumerationOptions = new EnumerationOptions();
        var expected = new Dictionary<string, IFileSystemInfo>()
        {
            [System.IO.Path.Combine(Path, "abc")] = Mocks.Create<IFileInfo>().Object,
            [System.IO.Path.Combine(Path, "def")] = Mocks.Create<IDirectoryInfo>().Object,
        };
        var expectedArray = expected.ToArray();
        DirectoryService.Setup(x => x.EnumerateFileSystemEntries(Path, searchPattern, enumerationOptions)).Returns(expected.Keys).Verifiable(Verifiables, Times.Once());
        DirectoryService.Setup(x => x.Exists(expectedArray[0].Key)).Returns(false).Verifiable(Verifiables, Times.Once());
        DirectoryService.Setup(x => x.Exists(expectedArray[1].Key)).Returns(true).Verifiable(Verifiables, Times.Once());
        FileService.Setup(x => x.GetInfo(expectedArray[0].Key)).Returns((IFileInfo)expectedArray[0].Value).Verifiable(Verifiables, Times.Once());
        DirectoryService.Setup(x => x.GetInfo(expectedArray[1].Key)).Returns((IDirectoryInfo)expectedArray[1].Value).Verifiable(Verifiables, Times.Once());

        var result = Sut.EnumerateFileSystemInfos(searchPattern, enumerationOptions);

        Assert.AreCollectionsEqual(expected.Values, result);
    }

    [TestMethod]
    public void GetDirectories_SearchPattern_EnumerationOptions()
    {
        var searchPattern = Guid.NewGuid().ToString();
        var enumerationOptions = new EnumerationOptions();
        var expected = new Dictionary<string, IDirectoryInfo>()
        {
            [System.IO.Path.Combine(Path, "abc")] = Mocks.Create<IDirectoryInfo>().Object,
            [System.IO.Path.Combine(Path, "def")] = Mocks.Create<IDirectoryInfo>().Object,
        };
        DirectoryService.Setup(x => x.GetDirectories(Path, searchPattern, enumerationOptions)).Returns(expected.Keys.ToArray()).Verifiable(Verifiables, Times.Once());
        foreach (var item in expected)
            DirectoryService.Setup(x => x.GetInfo(item.Key)).Returns(item.Value).Verifiable(Verifiables, Times.Once());

        var result = Sut.GetDirectories(searchPattern, enumerationOptions);

        Assert.AreCollectionsEqual(expected.Values, result);
    }

    [TestMethod]
    public void GetFiles_SearchPattern_EnumerationOptions()
    {
        var searchPattern = Guid.NewGuid().ToString();
        var enumerationOptions = new EnumerationOptions();
        var expected = new Dictionary<string, IFileInfo>()
        {
            [System.IO.Path.Combine(Path, "abc")] = Mocks.Create<IFileInfo>().Object,
            [System.IO.Path.Combine(Path, "def")] = Mocks.Create<IFileInfo>().Object,
        };
        DirectoryService.Setup(x => x.GetFiles(Path, searchPattern, enumerationOptions)).Returns(expected.Keys.ToArray()).Verifiable(Verifiables, Times.Once());
        foreach (var item in expected)
            FileService.Setup(x => x.GetInfo(item.Key)).Returns(item.Value).Verifiable(Verifiables, Times.Once());

        var result = Sut.GetFiles(searchPattern, enumerationOptions);

        Assert.AreCollectionsEqual(expected.Values, result);
    }

    [TestMethod]
    public void GetFileSystemInfos_SearchPattern_EnumerationOptions()
    {
        var searchPattern = Guid.NewGuid().ToString();
        var enumerationOptions = new EnumerationOptions();
        var expected = new Dictionary<string, IFileSystemInfo>()
        {
            [System.IO.Path.Combine(Path, "abc")] = Mocks.Create<IFileInfo>().Object,
            [System.IO.Path.Combine(Path, "def")] = Mocks.Create<IDirectoryInfo>().Object,
        };
        var expectedArray = expected.ToArray();
        DirectoryService.Setup(x => x.GetFileSystemEntries(Path, searchPattern, enumerationOptions)).Returns(expected.Keys.ToArray()).Verifiable(Verifiables, Times.Once());
        DirectoryService.Setup(x => x.Exists(expectedArray[0].Key)).Returns(false).Verifiable(Verifiables, Times.Once());
        DirectoryService.Setup(x => x.Exists(expectedArray[1].Key)).Returns(true).Verifiable(Verifiables, Times.Once());
        FileService.Setup(x => x.GetInfo(expectedArray[0].Key)).Returns((IFileInfo)expectedArray[0].Value).Verifiable(Verifiables, Times.Once());
        DirectoryService.Setup(x => x.GetInfo(expectedArray[1].Key)).Returns((IDirectoryInfo)expectedArray[1].Value).Verifiable(Verifiables, Times.Once());

        var result = Sut.GetFileSystemInfos(searchPattern, enumerationOptions);

        Assert.AreCollectionsEqual(expected.Values, result);
    }
#endif

    protected override void OnInitializeTest()
    {
        base.OnInitializeTest();
        Sut = CreateInfo(Path, out var ds, out var fs);
        DirectoryService = ds;
        FileService = fs;
    }

    private DirectoryInfoBase CreateInfo(string path, out Mock<IDirectoryService> directoryService, out Mock<IFileService> fileService)
    {
        var dirService = directoryService = Mocks.Create<IDirectoryService>();
        var localFileService = fileService = Mocks.Create<IFileService>();
        var fileSystemService = Mocks.Create<IFileSystemService>();

        fileSystemService.Setup(x => x.Directory).Returns(directoryService.Object);
        fileSystemService.Setup(x => x.File).Returns(localFileService.Object);
        fileSystemService.Setup(x => x.GetDirectoryInfo(It.IsAny<string>())).Returns<string>(x => dirService.Object.GetInfo(x));
        fileSystemService.Setup(x => x.GetFileInfo(It.IsAny<string>())).Returns<string>(x => localFileService.Object.GetInfo(x));

        var info = Mocks.Create<DirectoryInfoBase>(MockBehavior.Loose, fileSystemService.Object, path);
        info.CallBase = true;
        return info.Object;
    }
}
