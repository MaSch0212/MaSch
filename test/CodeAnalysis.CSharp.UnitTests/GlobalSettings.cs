using DiffEngine;
using VerifyMSTest;
using VerifyTests;

namespace MaSch.CodeAnalysis.CSharp.UnitTests;

[TestClass]
public static class GlobalSettings
{
    [AssemblyInitialize]
    public static void InitializeAssembly(TestContext context)
    {
        VerifyBase.DerivePathInfo(DerivePathInfo);
        DiffTools.UseOrder(new[] { DiffTool.VisualStudioCode });
        DiffRunner.MaxInstancesToLaunch(50);
    }

    private static PathInfo DerivePathInfo(string sourceFile, string projectDirectory, Type type, MethodInfo method)
    {
        var subDir = string.Empty;
        if (sourceFile.StartsWith(projectDirectory))
            subDir = Path.GetDirectoryName(sourceFile)?.Substring(projectDirectory.Length).Trim(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

        return new(
            directory: Path.Combine(projectDirectory, ".verify", subDir ?? string.Empty),
            typeName: type.Name,
            methodName: method.Name);
    }
}
