using MaSch.FileSystem.Physical;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MaSch.FileSystem.UnitTests.Physical;

[TestClass]
public class PhysicalDirectoryInfoTests : TestClassBase
{
    private string TestPath { get; set; } = null!;
    private DirectoryInfo TestInfo { get; set; } = null!;
    private PhysicalDirectoryInfo Sut { get; set; } = null!;
    private Mock<IFileSystemService> FileSystemService { get; set; } = null!;

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
    public void EnumerateDirectories()
    {
        TestInfo.CreateSubdirectory("abc");
        TestInfo.CreateSubdirectory("def");
        Assert.AreCollectionsEquivalent(
            new[] { Path.Combine(TestPath, "abc"), Path.Combine(TestPath, "def") },
            Sut.EnumerateDirectories().Select(x => GetWrappedInfo(x).FullName));
    }

    [TestMethod]
    public void EnumerateDirectories_SearchPattern()
    {
        TestInfo.CreateSubdirectory("abcd");
        TestInfo.CreateSubdirectory("abced");
        TestInfo.CreateSubdirectory("abbbcd");
        TestInfo.CreateSubdirectory("abbbcfd");
        Assert.AreCollectionsEquivalent(
            new[] { Path.Combine(TestPath, "abced"), Path.Combine(TestPath, "abbbcfd") },
            Sut.EnumerateDirectories("a*c?d").Select(x => GetWrappedInfo(x).FullName));
    }

    [TestMethod]
    public void EnumerateDirectories_SearchPattern_SearchOptionTopDirectoryOnly()
    {
        TestInfo.CreateSubdirectory("abcd");
        TestInfo.CreateSubdirectory("abced");
        TestInfo.CreateSubdirectory(Path.Combine("sub", "abced"));
        TestInfo.CreateSubdirectory("abbbcd");
        TestInfo.CreateSubdirectory("abbbcfd");
        TestInfo.CreateSubdirectory(Path.Combine("sub", "abbbcfd"));
        Assert.AreCollectionsEquivalent(
            new[] { Path.Combine(TestPath, "abced"), Path.Combine(TestPath, "abbbcfd") },
            Sut.EnumerateDirectories("a*c?d", SearchOption.TopDirectoryOnly).Select(x => GetWrappedInfo(x).FullName));
    }

    [TestMethod]
    public void EnumerateDirectories_SearchPattern_SearchOptionAllDirectories()
    {
        TestInfo.CreateSubdirectory("abcd");
        TestInfo.CreateSubdirectory("abced");
        TestInfo.CreateSubdirectory(Path.Combine("sub", "abced"));
        TestInfo.CreateSubdirectory("abbbcd");
        TestInfo.CreateSubdirectory("abbbcfd");
        TestInfo.CreateSubdirectory(Path.Combine("sub", "abbbcfd"));
        Assert.AreCollectionsEquivalent(
            new[] { Path.Combine(TestPath, "abced"), Path.Combine(TestPath, "sub", "abced"), Path.Combine(TestPath, "abbbcfd"), Path.Combine(TestPath, "sub", "abbbcfd") },
            Sut.EnumerateDirectories("a*c?d", SearchOption.AllDirectories).Select(x => GetWrappedInfo(x).FullName));
    }

    private DirectoryInfo GetWrappedInfo(IDirectoryInfo info)
    {
        Assert.IsInstanceOfType<PhysicalDirectoryInfo>(info);
        var property = typeof(PhysicalDirectoryInfo).GetProperty("WrappedInfo", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly, null, typeof(DirectoryInfo), Type.EmptyTypes, null);
        Assert.IsNotNull(property, "Correct WrappedInfo property was not found.");
        return (DirectoryInfo)property.GetValue(info)!;
    }
}
