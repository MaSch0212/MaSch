using MaSch.FileSystem.Physical;

namespace MaSch.FileSystem.UnitTests.Physical;

[TestClass]
public class PhysicalDirectoryInfoTests : TestClassBase
{
    private string TestPath { get; set; } = null!;
    private DirectoryInfo TestInfo { get; set; } = null!;
    private PhysicalDirectoryInfo Sut { get; set; } = null!;
    private Mock<IFileSystemService> FileSystemService { get; set; } = null!;

    [TestMethod]
    public void Constructor_Checks()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new PhysicalDirectoryInfo(null!, TestInfo));
        Assert.ThrowsException<ArgumentNullException>(() => new PhysicalDirectoryInfo(FileSystemService.Object, null!));
    }

    [TestMethod]
    public void Get_FileSystem()
    {
        Assert.AreSame(FileSystemService.Object, Sut.FileSystem);
    }

    [TestMethod]
    public void Get_FullName()
    {
        Assert.AreEqual(TestInfo.FullName, Sut.FullName);
    }

    [TestMethod]
    public void Get_Extension()
    {
        Assert.AreEqual(TestInfo.Extension, Sut.Extension);
    }

    [TestMethod]
    public void Get_Name()
    {
        Assert.AreEqual(TestInfo.Name, Sut.Name);
    }

    [TestMethod]
    public void Get_Exists_True()
    {
        Assert.IsTrue(Sut.Exists);
    }

    [TestMethod]
    public void Get_Exists_False()
    {
        Directory.Delete(TestPath);
        Assert.IsFalse(Sut.Exists);
    }

    [TestMethod]
    public void Get_CreationTime()
    {
        Assert.AreEqual(TestInfo.CreationTime, Sut.CreationTime);
    }

    [TestMethod]
    public void Set_CreationTime()
    {
        var expected = DateTime.Now.AddDays(-1);
        Sut.CreationTime = expected;
        Assert.AreEqual(expected, Directory.GetCreationTime(TestPath), "CreationTime has not be set correctly.");
    }

    [TestMethod]
    public void Get_CreationTimeUtc()
    {
        Assert.AreEqual(TestInfo.CreationTimeUtc, Sut.CreationTimeUtc);
    }

    [TestMethod]
    public void Set_CreationTimeUtc()
    {
        var expected = DateTime.UtcNow.AddDays(-1);
        Sut.CreationTimeUtc = expected;
        Assert.AreEqual(expected, Directory.GetCreationTimeUtc(TestPath), "CreationTimeUtc has not be set correctly.");
    }

    [TestMethod]
    public void Get_LastAccessTime()
    {
        Assert.AreEqual(TestInfo.LastAccessTime, Sut.LastAccessTime);
    }

    [TestMethod]
    public void Set_LastAccessTime()
    {
        var expected = DateTime.Now.AddDays(-1);
        Sut.LastAccessTime = expected;
        Assert.AreEqual(expected, Directory.GetLastAccessTime(TestPath), "LastAccessTime has not be set correctly.");
    }

    [TestMethod]
    public void Get_LastAccessTimeUtc()
    {
        Assert.AreEqual(TestInfo.LastAccessTimeUtc, Sut.LastAccessTimeUtc);
    }

    [TestMethod]
    public void Set_LastAccessTimeUtc()
    {
        var expected = DateTime.UtcNow.AddDays(-1);
        Sut.LastAccessTimeUtc = expected;
        Assert.AreEqual(expected, Directory.GetLastAccessTimeUtc(TestPath), "LastAccessTimeUtc has not be set correctly.");
    }

    [TestMethod]
    public void Get_LastWriteTime()
    {
        Assert.AreEqual(TestInfo.LastWriteTime, Sut.LastWriteTime);
    }

    [TestMethod]
    public void Set_LastWriteTime()
    {
        var expected = DateTime.Now.AddDays(-1);
        Sut.LastWriteTime = expected;
        Assert.AreEqual(expected, Directory.GetLastWriteTime(TestPath), "LastWriteTime has not be set correctly.");
    }

    [TestMethod]
    public void Get_LastWriteTimeUtc()
    {
        Assert.AreEqual(TestInfo.LastWriteTimeUtc, Sut.LastWriteTimeUtc);
    }

    [TestMethod]
    public void Set_LastWriteTimeUtc()
    {
        var expected = DateTime.UtcNow.AddDays(-1);
        Sut.LastWriteTimeUtc = expected;
        Assert.AreEqual(expected, Directory.GetLastWriteTimeUtc(TestPath), "LastWriteTimeUtc has not be set correctly.");
    }

    [TestMethod]
    public void Get_Attributes()
    {
        Assert.AreEqual(TestInfo.Attributes, Sut.Attributes);
    }

    [TestMethod]
    public void Set_Attributes()
    {
        var expected = FileAttributes.Directory | FileAttributes.Hidden;
        Sut.Attributes = expected;
        Assert.AreEqual(expected, new DirectoryInfo(TestPath).Attributes, "Attributes has not be set correctly.");
    }

    [TestMethod]
    public void Get_Parent_HasParent()
    {
        var parent = Sut.Parent!;
        Assert.AreEqual(TestInfo.Parent!.FullName, GetWrappedInfo(parent).FullName);
    }

    [TestMethod]
    public void Get_Parent_DoesNotHaveParent()
    {
        Sut = new PhysicalDirectoryInfo(FileSystemService.Object, TestInfo.Root);
        Assert.IsNull(Sut.Parent);
    }

    [TestMethod]
    public void Get_Root()
    {
        var root = Sut.Root;
        Assert.AreEqual(TestInfo.Root.FullName, GetWrappedInfo(root).FullName);
    }

    [TestMethod]
    public void Refresh()
    {
        _ = Sut.CreationTime;
        var newCreationTime = DateTime.Now.AddDays(-1);
        Directory.SetCreationTime(TestPath, newCreationTime);
        Sut.Refresh();
        Assert.AreEqual(newCreationTime, Sut.CreationTime);
    }

    [TestMethod]
    public void Create()
    {
        Directory.Delete(TestPath);
        Assert.IsFalse(Directory.Exists(TestPath), "Test directory still exists after deletion.");

        Sut.Create();

        Assert.IsTrue(Directory.Exists(TestPath), "Test directory does not exist after calling .Create().");
    }

    [TestMethod]
    public void CreateSubDirectory()
    {
        var subDirName = Guid.NewGuid().ToString();

        var subDirInfo = Sut.CreateSubdirectory(subDirName);

        Assert.AreEqual(Path.Combine(TestPath, subDirName), subDirInfo.FullName);
        Assert.IsTrue(Directory.Exists(subDirInfo.FullName), "Sub dir was not created.");
    }

    [TestMethod]
    public void Delete_EmptyDirectory()
    {
        Sut.Delete();
        Assert.IsFalse(Directory.Exists(TestPath), "Directory has not been deleted.");
    }

    [TestMethod]
    public void Delete_NonEmptyDirectory()
    {
        Directory.CreateDirectory(Path.Combine(TestPath, Guid.NewGuid().ToString()));

        Assert.ThrowsException<IOException>(() => Sut.Delete());
    }

    [TestMethod]
    public void Delete_Recursive()
    {
        Directory.CreateDirectory(Path.Combine(TestPath, Guid.NewGuid().ToString()));

        Sut.Delete(true);

        Assert.IsFalse(Directory.Exists(TestPath), "Directory has not been deleted.");
    }

    [TestMethod]
    public void Delete_NonRecursive_EmptyDirectory()
    {
        Sut.Delete(false);
        Assert.IsFalse(Directory.Exists(TestPath), "Directory has not been deleted.");
    }

    [TestMethod]
    public void Delete_NonRecursive_NonEmptyDirectory()
    {
        Directory.CreateDirectory(Path.Combine(TestPath, Guid.NewGuid().ToString()));

        Assert.ThrowsException<IOException>(() => Sut.Delete(false));
    }

    [TestMethod]
    [DataRow(false, DisplayName = "EnumerateDirectories")]
    [DataRow(true, DisplayName = "GetDirectories")]
    public void EnumerateDirectories(bool useGet)
    {
        Func<IEnumerable<IDirectoryInfo>> method = useGet ? Sut.GetDirectories : Sut.EnumerateDirectories;
        CreateSubdirectories("abc", "def");
        Assert.AreCollectionsEquivalent(
            new[] { Path.Combine(TestPath, "abc"), Path.Combine(TestPath, "def") },
            method().Select(x => GetWrappedInfo(x).FullName));
    }

    [TestMethod]
    [DataRow(false, DisplayName = "EnumerateDirectories")]
    [DataRow(true, DisplayName = "GetDirectories")]
    public void EnumerateDirectories_SearchPattern(bool useGet)
    {
        Func<string, IEnumerable<IDirectoryInfo>> method = useGet ? Sut.GetDirectories : Sut.EnumerateDirectories;
        CreateSubdirectories("abcd", "abced", "abbbcd", "abbbcfd");
        Assert.AreCollectionsEquivalent(
            new[] { Path.Combine(TestPath, "abced"), Path.Combine(TestPath, "abbbcfd") },
            method("a*c?d").Select(x => GetWrappedInfo(x).FullName));
    }

    [TestMethod]
    [DataRow(false, DisplayName = "EnumerateDirectories")]
    [DataRow(true, DisplayName = "GetDirectories")]
    public void EnumerateDirectories_SearchPattern_SearchOptionTopDirectoryOnly(bool useGet)
    {
        Func<string, SearchOption, IEnumerable<IDirectoryInfo>> method = useGet ? Sut.GetDirectories : Sut.EnumerateDirectories;
        CreateSubdirectories("abcd", "abced", "abbbcd", "abbbcfd", Path.Combine("sub", "abced"), Path.Combine("sub", "abbbcfd"));
        Assert.AreCollectionsEquivalent(
            new[] { Path.Combine(TestPath, "abced"), Path.Combine(TestPath, "abbbcfd") },
            method("a*c?d", SearchOption.TopDirectoryOnly).Select(x => GetWrappedInfo(x).FullName));
    }

    [TestMethod]
    [DataRow(false, DisplayName = "EnumerateDirectories")]
    [DataRow(true, DisplayName = "GetDirectories")]
    public void EnumerateDirectories_SearchPattern_SearchOptionAllDirectories(bool useGet)
    {
        Func<string, SearchOption, IEnumerable<IDirectoryInfo>> method = useGet ? Sut.GetDirectories : Sut.EnumerateDirectories;
        CreateSubdirectories("abcd", "abced", "abbbcd", "abbbcfd", Path.Combine("sub", "abced"), Path.Combine("sub", "abbbcfd"));
        Assert.AreCollectionsEquivalent(
            new[] { Path.Combine(TestPath, "abced"), Path.Combine(TestPath, "sub", "abced"), Path.Combine(TestPath, "abbbcfd"), Path.Combine(TestPath, "sub", "abbbcfd") },
            method("a*c?d", SearchOption.AllDirectories).Select(x => GetWrappedInfo(x).FullName));
    }

    [TestMethod]
    [DataRow(false, DisplayName = "EnumerateFiles")]
    [DataRow(true, DisplayName = "GetFiles")]
    public void EnumerateFiles(bool useGet)
    {
        Func<IEnumerable<IFileInfo>> method = useGet ? Sut.GetFiles : Sut.EnumerateFiles;
        CreateFiles("abc.txt", "def.txt");
        Assert.AreCollectionsEquivalent(
            new[] { Path.Combine(TestPath, "abc.txt"), Path.Combine(TestPath, "def.txt") },
            method().Select(x => GetWrappedInfo(x).FullName));
    }

    [TestMethod]
    [DataRow(false, DisplayName = "EnumerateFiles")]
    [DataRow(true, DisplayName = "GetFiles")]
    public void EnumerateFiles_SearchPattern(bool useGet)
    {
        Func<string, IEnumerable<IFileInfo>> method = useGet ? Sut.GetFiles : Sut.EnumerateFiles;
        CreateFiles("abcd.txt", "abced.txt", "abbbcd.txt", "abbbcfd.txt");
        Assert.AreCollectionsEquivalent(
            new[] { Path.Combine(TestPath, "abced.txt"), Path.Combine(TestPath, "abbbcfd.txt") },
            method("a*c?d.txt").Select(x => GetWrappedInfo(x).FullName));
    }

    [TestMethod]
    [DataRow(false, DisplayName = "EnumerateFiles")]
    [DataRow(true, DisplayName = "GetFiles")]
    public void EnumerateFiles_SearchPattern_SearchOptionTopDirectoryOnly(bool useGet)
    {
        Func<string, SearchOption, IEnumerable<IFileInfo>> method = useGet ? Sut.GetFiles : Sut.EnumerateFiles;
        CreateFiles("abcd.txt", "abced.txt", "abbbcd.txt", "abbbcfd.txt", Path.Combine("sub", "abced.txt"), Path.Combine("sub", "abbbcfd.txt"));
        Assert.AreCollectionsEquivalent(
            new[] { Path.Combine(TestPath, "abced.txt"), Path.Combine(TestPath, "abbbcfd.txt") },
            method("a*c?d.txt", SearchOption.TopDirectoryOnly).Select(x => GetWrappedInfo(x).FullName));
    }

    [TestMethod]
    [DataRow(false, DisplayName = "EnumerateFiles")]
    [DataRow(true, DisplayName = "GetFiles")]
    public void EnumerateFiles_SearchPattern_SearchOptionAllDirectories(bool useGet)
    {
        Func<string, SearchOption, IEnumerable<IFileInfo>> method = useGet ? Sut.GetFiles : Sut.EnumerateFiles;
        CreateFiles("abcd.txt", "abced.txt", "abbbcd.txt", "abbbcfd.txt", Path.Combine("sub", "abced.txt"), Path.Combine("sub", "abbbcfd.txt"));
        Assert.AreCollectionsEquivalent(
            new[] { Path.Combine(TestPath, "abced.txt"), Path.Combine(TestPath, "sub", "abced.txt"), Path.Combine(TestPath, "abbbcfd.txt"), Path.Combine(TestPath, "sub", "abbbcfd.txt") },
            method("a*c?d.txt", SearchOption.AllDirectories).Select(x => GetWrappedInfo(x).FullName));
    }

    [TestMethod]
    [DataRow(false, DisplayName = "EnumerateFileSystemInfos")]
    [DataRow(true, DisplayName = "GetFileSystemInfos")]
    public void EnumerateFileSystemInfos(bool useGet)
    {
        Func<IEnumerable<IFileSystemInfo>> method = useGet ? Sut.GetFileSystemInfos : Sut.EnumerateFileSystemInfos;
        CreateSubdirectories("abc", "def");
        CreateFiles("abc.txt", "def.txt");
        Assert.AreCollectionsEquivalent(
            new[] { Path.Combine(TestPath, "abc"), Path.Combine(TestPath, "def"), Path.Combine(TestPath, "abc.txt"), Path.Combine(TestPath, "def.txt") },
            method().Select(x => GetWrappedInfo(x).FullName));
    }

    [TestMethod]
    [DataRow(false, DisplayName = "EnumerateFileSystemInfos")]
    [DataRow(true, DisplayName = "GetFileSystemInfos")]
    public void EnumerateFileSystemInfos_SearchPattern(bool useGet)
    {
        Func<string, IEnumerable<IFileSystemInfo>> method = useGet ? Sut.GetFileSystemInfos : Sut.EnumerateFileSystemInfos;
        CreateSubdirectories("abcd", "abced", "abbbcd", "abbbcfd");
        CreateFiles("abcd.txt", "abced.txt", "abbbcd.txt", "abbbcfd.txt");
        Assert.AreCollectionsEquivalent(
            new[] { Path.Combine(TestPath, "abced"), Path.Combine(TestPath, "abbbcfd"), Path.Combine(TestPath, "abced.txt"), Path.Combine(TestPath, "abbbcfd.txt") },
            method("a*c?d*").Select(x => GetWrappedInfo(x).FullName));
    }

    [TestMethod]
    [DataRow(false, DisplayName = "EnumerateFileSystemInfos")]
    [DataRow(true, DisplayName = "GetFileSystemInfos")]
    public void EnumerateFileSystemInfos_SearchPattern_SearchOptionTopDirectoryOnly(bool useGet)
    {
        Func<string, SearchOption, IEnumerable<IFileSystemInfo>> method = useGet ? Sut.GetFileSystemInfos : Sut.EnumerateFileSystemInfos;
        CreateSubdirectories("abcd", "abced", "abbbcd", "abbbcfd", Path.Combine("sub", "abced"), Path.Combine("sub", "abbbcfd"));
        CreateFiles("abcd.txt", "abced.txt", "abbbcd.txt", "abbbcfd.txt", Path.Combine("sub", "abced.txt"), Path.Combine("sub", "abbbcfd.txt"));
        Assert.AreCollectionsEquivalent(
            new[] { Path.Combine(TestPath, "abced"), Path.Combine(TestPath, "abbbcfd"), Path.Combine(TestPath, "abced.txt"), Path.Combine(TestPath, "abbbcfd.txt") },
            method("a*c?d*", SearchOption.TopDirectoryOnly).Select(x => GetWrappedInfo(x).FullName));
    }

    [TestMethod]
    [DataRow(false, DisplayName = "EnumerateFileSystemInfos")]
    [DataRow(true, DisplayName = "GetFileSystemInfos")]
    public void EnumerateFileSystemInfos_SearchPattern_SearchOptionAllDirectories(bool useGet)
    {
        Func<string, SearchOption, IEnumerable<IFileSystemInfo>> method = useGet ? Sut.GetFileSystemInfos : Sut.EnumerateFileSystemInfos;
        CreateSubdirectories("abcd", "abced", "abbbcd", "abbbcfd", Path.Combine("sub", "abced"), Path.Combine("sub", "abbbcfd"));
        CreateFiles("abcd.txt", "abced.txt", "abbbcd.txt", "abbbcfd.txt", Path.Combine("sub", "abced.txt"), Path.Combine("sub", "abbbcfd.txt"));
        Assert.AreCollectionsEquivalent(
            new[] { Path.Combine(TestPath, "abced"), Path.Combine(TestPath, "sub", "abced"), Path.Combine(TestPath, "abbbcfd"), Path.Combine(TestPath, "sub", "abbbcfd"), Path.Combine(TestPath, "abced.txt"), Path.Combine(TestPath, "sub", "abced.txt"), Path.Combine(TestPath, "abbbcfd.txt"), Path.Combine(TestPath, "sub", "abbbcfd.txt") },
            method("a*c?d*", SearchOption.AllDirectories).Select(x => GetWrappedInfo(x).FullName));
    }

    [TestMethod]
    public void MoveTo()
    {
        var dirInfo = new PhysicalDirectoryInfo(FileSystemService.Object, TestInfo.CreateSubdirectory("sub"));
        dirInfo.MoveTo(Path.Combine(TestPath, "sub2"));

        Assert.IsFalse(Directory.Exists(Path.Combine(TestPath, "sub")));
        Assert.IsTrue(Directory.Exists(Path.Combine(TestPath, "sub2")));
    }

#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
    [TestMethod]
    [DataRow(false, DisplayName = "EnumerateDirectories")]
    [DataRow(true, DisplayName = "GetDirectories")]
    public void EnumerateDirectories_SearchPattern_EnumerationOptions_SearchOptionTopDirectoryOnly(bool useGet)
    {
        Func<string, EnumerationOptions, IEnumerable<IDirectoryInfo>> method = useGet ? Sut.GetDirectories : Sut.EnumerateDirectories;
        CreateSubdirectories("abcd", "abced", "abbbcd", "abbbcfd", Path.Combine("sub", "abced"), Path.Combine("sub", "abbbcfd"));
        Assert.AreCollectionsEquivalent(
            new[] { Path.Combine(TestPath, "abced"), Path.Combine(TestPath, "abbbcfd") },
            method("a*c?d", new EnumerationOptions { RecurseSubdirectories = false }).Select(x => GetWrappedInfo(x).FullName));
    }

    [TestMethod]
    [DataRow(false, DisplayName = "EnumerateDirectories")]
    [DataRow(true, DisplayName = "GetDirectories")]
    public void EnumerateDirectories_SearchPattern_EnumerationOptions_SearchOptionAllDirectories(bool useGet)
    {
        Func<string, EnumerationOptions, IEnumerable<IDirectoryInfo>> method = useGet ? Sut.GetDirectories : Sut.EnumerateDirectories;
        CreateSubdirectories("abcd", "abced", "abbbcd", "abbbcfd", Path.Combine("sub", "abced"), Path.Combine("sub", "abbbcfd"));
        Assert.AreCollectionsEquivalent(
            new[] { Path.Combine(TestPath, "abced"), Path.Combine(TestPath, "sub", "abced"), Path.Combine(TestPath, "abbbcfd"), Path.Combine(TestPath, "sub", "abbbcfd") },
            method("a*c?d", new EnumerationOptions { RecurseSubdirectories = true }).Select(x => GetWrappedInfo(x).FullName));
    }

    [TestMethod]
    [DataRow(false, DisplayName = "EnumerateFiles")]
    [DataRow(true, DisplayName = "GetFiles")]
    public void EnumerateFiles_SearchPattern_EnumerationOptions_SearchOptionTopDirectoryOnly(bool useGet)
    {
        Func<string, EnumerationOptions, IEnumerable<IFileInfo>> method = useGet ? Sut.GetFiles : Sut.EnumerateFiles;
        CreateFiles("abcd.txt", "abced.txt", "abbbcd.txt", "abbbcfd.txt", Path.Combine("sub", "abced.txt"), Path.Combine("sub", "abbbcfd.txt"));
        Assert.AreCollectionsEquivalent(
            new[] { Path.Combine(TestPath, "abced.txt"), Path.Combine(TestPath, "abbbcfd.txt") },
            method("a*c?d.txt", new EnumerationOptions { RecurseSubdirectories = false }).Select(x => GetWrappedInfo(x).FullName));
    }

    [TestMethod]
    [DataRow(false, DisplayName = "EnumerateFiles")]
    [DataRow(true, DisplayName = "GetFiles")]
    public void EnumerateFiles_SearchPattern_EnumerationOptions_SearchOptionAllDirectories(bool useGet)
    {
        Func<string, EnumerationOptions, IEnumerable<IFileInfo>> method = useGet ? Sut.GetFiles : Sut.EnumerateFiles;
        CreateFiles("abcd.txt", "abced.txt", "abbbcd.txt", "abbbcfd.txt", Path.Combine("sub", "abced.txt"), Path.Combine("sub", "abbbcfd.txt"));
        Assert.AreCollectionsEquivalent(
            new[] { Path.Combine(TestPath, "abced.txt"), Path.Combine(TestPath, "sub", "abced.txt"), Path.Combine(TestPath, "abbbcfd.txt"), Path.Combine(TestPath, "sub", "abbbcfd.txt") },
            method("a*c?d.txt", new EnumerationOptions { RecurseSubdirectories = true }).Select(x => GetWrappedInfo(x).FullName));
    }

    [TestMethod]
    [DataRow(false, DisplayName = "EnumerateFileSystemInfos")]
    [DataRow(true, DisplayName = "GetFileSystemInfos")]
    public void EnumerateFileSystemInfos_SearchPattern_EnumerationOptions_SearchOptionTopDirectoryOnly(bool useGet)
    {
        Func<string, EnumerationOptions, IEnumerable<IFileSystemInfo>> method = useGet ? Sut.GetFileSystemInfos : Sut.EnumerateFileSystemInfos;
        CreateSubdirectories("abcd", "abced", "abbbcd", "abbbcfd", Path.Combine("sub", "abced"), Path.Combine("sub", "abbbcfd"));
        CreateFiles("abcd.txt", "abced.txt", "abbbcd.txt", "abbbcfd.txt", Path.Combine("sub", "abced.txt"), Path.Combine("sub", "abbbcfd.txt"));
        Assert.AreCollectionsEquivalent(
            new[] { Path.Combine(TestPath, "abced"), Path.Combine(TestPath, "abbbcfd"), Path.Combine(TestPath, "abced.txt"), Path.Combine(TestPath, "abbbcfd.txt") },
            method("a*c?d*", new EnumerationOptions { RecurseSubdirectories = false }).Select(x => GetWrappedInfo(x).FullName));
    }

    [TestMethod]
    [DataRow(false, DisplayName = "EnumerateFileSystemInfos")]
    [DataRow(true, DisplayName = "GetFileSystemInfos")]
    public void EnumerateFileSystemInfos_SearchPattern_EnumerationOptions_SearchOptionAllDirectories(bool useGet)
    {
        Func<string, EnumerationOptions, IEnumerable<IFileSystemInfo>> method = useGet ? Sut.GetFileSystemInfos : Sut.EnumerateFileSystemInfos;
        CreateSubdirectories("abcd", "abced", "abbbcd", "abbbcfd", Path.Combine("sub", "abced"), Path.Combine("sub", "abbbcfd"));
        CreateFiles("abcd.txt", "abced.txt", "abbbcd.txt", "abbbcfd.txt", Path.Combine("sub", "abced.txt"), Path.Combine("sub", "abbbcfd.txt"));
        Assert.AreCollectionsEquivalent(
            new[] { Path.Combine(TestPath, "abced"), Path.Combine(TestPath, "sub", "abced"), Path.Combine(TestPath, "abbbcfd"), Path.Combine(TestPath, "sub", "abbbcfd"), Path.Combine(TestPath, "abced.txt"), Path.Combine(TestPath, "sub", "abced.txt"), Path.Combine(TestPath, "abbbcfd.txt"), Path.Combine(TestPath, "sub", "abbbcfd.txt") },
            method("a*c?d*", new EnumerationOptions { RecurseSubdirectories = true }).Select(x => GetWrappedInfo(x).FullName));
    }
#endif

    protected override void OnInitializeTest()
    {
        base.OnInitializeTest();
        TestPath = Path.Combine(TestContext.DeploymentDirectory, Guid.NewGuid().ToString());
        TestInfo = Directory.CreateDirectory(TestPath);
        FileSystemService = Mocks.Create<IFileSystemService>();
        Sut = new PhysicalDirectoryInfo(FileSystemService.Object, TestInfo);
    }

    protected override void OnCleanupTest()
    {
        if (Directory.Exists(TestPath))
            Directory.Delete(TestPath, true);
        base.OnCleanupTest();
    }

    private DirectoryInfo GetWrappedInfo(IDirectoryInfo info)
    {
        Assert.IsInstanceOfType<PhysicalDirectoryInfo>(info);
        var property = typeof(PhysicalDirectoryInfo).GetProperty("WrappedInfo", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly, null, typeof(DirectoryInfo), Type.EmptyTypes, null);
        Assert.IsNotNull(property, "Correct WrappedInfo property was not found.");
        return (DirectoryInfo)property.GetValue(info)!;
    }

    private FileInfo GetWrappedInfo(IFileInfo info)
    {
        Assert.IsInstanceOfType<PhysicalFileInfo>(info);
        var property = typeof(PhysicalFileInfo).GetProperty("WrappedInfo", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly, null, typeof(FileInfo), Type.EmptyTypes, null);
        Assert.IsNotNull(property, "Correct WrappedInfo property was not found.");
        return (FileInfo)property.GetValue(info)!;
    }

    private FileSystemInfo GetWrappedInfo(IFileSystemInfo info)
    {
        Assert.IsInstanceOfType<PhysicalFileSystemInfo>(info);
        var property = typeof(PhysicalFileSystemInfo).GetProperty("WrappedInfo", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly, null, typeof(FileSystemInfo), Type.EmptyTypes, null);
        Assert.IsNotNull(property, "Correct WrappedInfo property was not found.");
        return (FileSystemInfo)property.GetValue(info)!;
    }

    private void CreateSubdirectories(params string[] subdirs)
    {
        foreach (var subdir in subdirs)
            TestInfo.CreateSubdirectory(subdir);
    }

    private void CreateFiles(params string[] files)
    {
        foreach (var file in files)
        {
            var fullPath = Path.GetFullPath(Path.Combine(TestPath, file));
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
            File.Create(fullPath).Dispose();
        }
    }
}
